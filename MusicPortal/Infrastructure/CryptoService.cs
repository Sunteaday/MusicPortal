using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MusicPortal.Infrastructure
{
    static class CryptoService
    {
        static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        static readonly MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

        public static byte[] GetRandomBytes(int count)
        {
            byte[] bytebuf = new byte[count];

            rng.GetBytes(bytebuf);

            return bytebuf;
        }
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length);

            foreach (byte @byte in bytes)
                sb.Append(string.Format("{0:X2}", @byte));

            return sb.ToString();
        }
        public static byte[] ComputeMD5Hash(byte[] bytes)
        {
            return CSP.ComputeHash(bytes);
        }
    }
}