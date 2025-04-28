using System.Net;

namespace Overdose_PublicStealer.Utils
{
    internal static class NetworkUtils
    {
        internal static string GetIp()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");
                    return client.DownloadString("https://api.ipify.org");
                }
            }
            catch
            {
                return "N/A";
            }
        }
    }
}
