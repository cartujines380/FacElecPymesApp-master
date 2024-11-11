using System;
using System.Text;
using IdentityModel;
using PCLCrypto;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers
{
    internal static class CryptoHelper
    {
        private static string ByteArrayToString(byte[] array)
        {
            var hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        internal static string CreateUniqueId(int length = 64)
        {
            var bytes = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);
            return ByteArrayToString(bytes);
        }

        internal static string CreateCodeChallenge(string codeVerifier)
        {
            var sha256 = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            var challengeBuffer = sha256.HashData(WinRTCrypto.CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(codeVerifier)));
            byte[] challengeBytes;
            WinRTCrypto.CryptographicBuffer.CopyToByteArray(challengeBuffer, out challengeBytes);
            return Base64Url.Encode(challengeBytes);
        }
    }
}
