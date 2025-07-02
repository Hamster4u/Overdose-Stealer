using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace X.A
{
    internal static class Z
    {
        internal static string P(string x, byte[] k)
        {
            if (k == null || x == null) return null;

            try
            {
                byte[] a = Convert.FromBase64String(x);
                if (a.Length < 29) return null;

                const int o = 3, nl = 12, tl = 16;
                if (a.Length < o + nl + tl) return null;

                byte[] n = new byte[nl];
                Buffer.BlockCopy(a, o, n, 0, nl);

                int co = o + nl;
                int cl = a.Length - co - tl;
                if (cl < 0) return null;

                byte[] c = new byte[cl];
                Buffer.BlockCopy(a, co, c, 0, cl);

                byte[] t = new byte[tl];
                Buffer.BlockCopy(a, a.Length - tl, t, 0, tl);

                byte[] d = new byte[cl];

                var cp = new AeadParameters(new KeyParameter(k), 128, n);
                var e = new AesEngine();
                var g = new GcmBlockCipher(e);
                g.Init(false, cp);

                byte[] ib = new byte[c.Length + t.Length];
                Buffer.BlockCopy(c, 0, ib, 0, c.Length);
                Buffer.BlockCopy(t, 0, ib, c.Length, t.Length);

                int bp = g.ProcessBytes(ib, 0, ib.Length, d, 0);
                g.DoFinal(d, bp);

                return Encoding.UTF8.GetString(d);
            }
            catch (InvalidCipherTextException) { return null; }
            catch (FormatException) { return null; }
            catch (Exception) { return null; }
        }
    }
}
