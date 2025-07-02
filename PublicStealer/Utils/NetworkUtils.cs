using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataHarvester.Utils
{
    internal static class NetworkUtils
    {
        // Construcción dinámica de la URL de la API
        private static readonly string ApiUrl = "https" + "://api" + "." + "ipify" + "." + "org"; // API URL para obtener la IP

        internal static string GetIp()
        {
            try
            {
                // Separar el host y el path de la URL
                var uri = new Uri(ApiUrl);
                var host = uri.Host;
                var path = uri.AbsolutePath;

                // Crear un socket TCP
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(host, 443); // Conectar a la API a través de HTTPS (puerto 443)

                    // Crear un flujo SSL sobre el socket
                    using (var sslStream = new SslStream(new NetworkStream(socket), false, new RemoteCertificateValidationCallback(ValidateServerCertificate)))
                    {
                        // Realizar handshake SSL
                        sslStream.AuthenticateAsClient(host);

                        // Preparar la solicitud HTTP GET
                        string request = $"GET {path} HTTP/1.1\r\n" +
                                         $"Host: {host}\r\n" +
                                         $"User-Agent: Mozilla/5.0\r\n" +
                                         $"Connection: close\r\n\r\n";

                        // Convertir la solicitud a bytes y enviarla
                        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                        sslStream.Write(requestBytes, 0, requestBytes.Length);

                        // Leer la respuesta
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        StringBuilder response = new StringBuilder();
                        while ((bytesRead = sslStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                        }

                        // Extraer la IP de la respuesta (simplemente obtenemos el cuerpo de la respuesta)
                        var responseString = response.ToString();
                        var ipStartIndex = responseString.IndexOf("\r\n\r\n") + 4; // El contenido comienza después de los headers
                        var ipAddress = responseString.Substring(ipStartIndex).Trim();

                        return ipAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An error occurred: {ex.Message}");
                return "N/A";
            }
        }

        // Método de validación del certificado del servidor (básicamente ignorando la validación para el ejemplo)
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Ignoramos cualquier error de certificado (solo para pruebas, no es seguro en producción)
        }
    }
}
