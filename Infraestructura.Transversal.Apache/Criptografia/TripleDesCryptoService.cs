using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Criptografia
{
    public class TripleDesCryptoService : ITripleDesService
    {
        #region Campos

        private TripleDESCryptoServiceProvider m_provider;
        private byte[] m_key;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public TripleDesCryptoService()
        {
            m_key = new byte[24];

            m_provider = new TripleDESCryptoServiceProvider()
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB
            };
        }

        #endregion

        #region ITripleDesService

        public byte[] Key
        {
            get
            {
                return m_key;
            }
            set
            {
                m_key = value;
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] retorno = null;

            int num = m_provider.BlockSize / 8;
            int num2 = data.Length;

            if (data.Length % num != 0)
            {
                num2 += num;
            }

            var array = new byte[num2];

            array.Initialize();
            data.CopyTo(array, 0);

            byte b = (byte)(data.Length % num);

            for (int i = array.Length - 1; i > array.Length - 1 - b; i--)
            {
                array[i] = 0;
            }

            var encryptor = m_provider.CreateEncryptor(Key, null);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(array, 0, array.Length);

                    retorno = new byte[ms.Length];

                    ms.Seek(0L, SeekOrigin.Begin);
                    ms.Read(retorno, 0, retorno.Length);
                }
            }

            return retorno;
        }

        public byte[] EncryptText(string data)
        {
            var toEncrypt = Encoding.Default.GetBytes(data);

            return Encrypt(toEncrypt);
        }

        public byte[] Decrypt(byte[] data)
        {
            byte[] retorno = null;

            var decryptor = m_provider.CreateDecryptor(Key, null);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    retorno = new byte[ms.Length];
                    ms.Seek(0L, SeekOrigin.Begin);
                    ms.Read(retorno, 0, retorno.Length);
                }
            }

            return retorno;
        }

        public string DecryptText(byte[] data)
        {
            var retornoByte = Decrypt(data);

            var retorno = Encoding.Default.GetString(retornoByte);

            var trimChars = new char[1];

            return retorno.TrimEnd(trimChars);
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                m_provider.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TripleDesCryptoService()
        {
            Dispose(false);
        }

        #endregion
    }
}
