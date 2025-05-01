using System; // Added for DateTimeOffset
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Overdose_PublicStealer.Models
{
    // Represents a field in the embed (for custom data)
    internal class EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }

    // Represents the footer section of the embed
    internal class EmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        // Added IconUrl property as it's common in Discord webhooks
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
    }

    // Represents an image inside the embed (optional)
    internal class EmbedImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    // Represents the author section of the embed (optional)
    internal class EmbedAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } // Optional: URL that the author name links to

        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; } // Optional: URL of the author's icon
    }

    // Represents a thumbnail image for the embed (optional)
    internal class EmbedThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }


    // Represents the embed content
    internal class Embed
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonPropertyName("fields")]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();

        [JsonPropertyName("image")]
        public EmbedImage Image { get; set; }

        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; set; }

        // Added Timestamp property for the embed
        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        // NEW: Added Author property to the Embed class
        [JsonPropertyName("author")]
        public EmbedAuthor Author { get; set; }

        // NEW: Added Thumbnail property to the Embed class
        [JsonPropertyName("thumbnail")]
        public EmbedThumbnail Thumbnail { get; set; }
    }

    // Represents the full webhook payload
    internal class WebhookPayload
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = "Overdose PublicStealer";

        // Static property to hold the default Avatar GIF URL
        public static string DefaultAvatarUrl { get; } = "https://i.pinimg.com/736x/56/a5/3c/56a53c0a581d8036e41f6de0656a869e.jpg"; // Your desired GIF URL for the avatar

        // NEW: Static property to hold the default Embed Image GIF URL
        public static string DefaultEmbedImageUrl { get; } = "https://i.pinimg.com/736x/11/f6/2c/11f62cf257786c26fcf190441808db12.jpg"; // <-- Replace with your desired GIF URL for the embed image

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = DefaultAvatarUrl; // Use the static DefaultAvatarUrl

        [JsonPropertyName("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();

        // Added content property for general text content (optional)
        [JsonPropertyName("content")]
        public string Content { get; set; }

        // Added tts property for text-to-speech (optional)
        [JsonPropertyName("tts")]
        public bool Tts { get; set; }
    }
}
