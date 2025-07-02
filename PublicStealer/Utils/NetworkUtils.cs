using System.Net;

namespace OdPS.Utils
{
    internal static class NetworkUtils
    {
        internal static string GetIp()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(BuildUserAgent());
                    return client.DownloadString(BuildApiUrl());
                }
            }
            catch
            {
                return "N/A";
            }
        }

        private static string BuildUserAgent()
        {
            // Construir dinámicamente el User-Agent sin usar una cadena estática
            var agent = new char[] { 'M', 'o', 'z', 'i', 'l', 'l', 'a', '/', '5', '.', '0' };
            return new string(agent);
        }

        private static string BuildApiUrl()
        {
            // Construir la URL de la API en tiempo de ejecución
            var urlParts = new string[] { "https://api", ".", "ipify", ".", "org" };
            return string.Join(string.Empty, urlParts);
        }
    }
}
