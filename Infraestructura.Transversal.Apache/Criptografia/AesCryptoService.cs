using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Criptografia
{
    public class AesCryptoService : IAesService
    {
        #region Campos

        private AesCryptoServiceProvider m_provider;

        private byte[] m_key;
        private byte[] m_IV;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public AesCryptoService()
        {
            m_provider = new AesCryptoServiceProvider();
            m_key = m_provider.Key;
            m_IV = m_provider.IV;
        }

        #endregion

        #region IAesService

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

        public byte[] IV
        {
            get
            {
                return m_IV;
            }
            set
            {
                m_IV = value;
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] retorno = null;

            var encryptor = m_provider.CreateEncryptor(Key, IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                retorno = ms.ToArray();
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

            var decryptor = m_provider.CreateDecryptor(Key, IV);

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

            return Encoding.Default.GetString(retornoByte);
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

        ~AesCryptoService()
        {
            Dispose(false);
        }

        #endregion
    }
}
