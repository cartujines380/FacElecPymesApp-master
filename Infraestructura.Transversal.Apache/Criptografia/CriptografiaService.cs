using System;
using System.Security.Cryptography;
using System.Text;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Criptografia
{
    public class CriptografiaService : ICriptografiaService
    {
        #region Constantes

        private const int CLAVE_LONGITUD = 16;
        private const char PADDING_CARACTER = '*';
        private const string FORMATO_HEXADECIMAL = "X2";
        private const string CONTRASENA_ENCRIPTACION_DEFAULT = "S1pec0m";

        #endregion

        #region Metodos privados

        private string CrearSemilla()
        {
            var key1 = new StringBuilder();
            var key2 = new StringBuilder();

            key1.Append((2 * 1).ToString());
            key1.Append((2 * 2).ToString());
            key1.Append(key1);
            key2.Append((2 * 2).ToString());
            key2.Append((2 * 1).ToString());
            key2.Append(key2);
            key1.Append("-").Append(key2.ToString());
            key1.Append("-").Append(key1.ToString());

            return key1.ToString();
        }

        private byte[] ObtenerClave(string semilla)
        {
            var retorno = string.Empty;

            if (semilla.Length > CLAVE_LONGITUD)
            {
                retorno = semilla.Substring(0, CLAVE_LONGITUD);
            }
            else if (semilla.Length < CLAVE_LONGITUD)
            {
                retorno = semilla.PadRight(CLAVE_LONGITUD, PADDING_CARACTER);
            }
            else
            {
                retorno = semilla;
            }

            return Encoding.ASCII.GetBytes(retorno);
        }

        private string ACadena(byte[] data)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(FORMATO_HEXADECIMAL));
            }

            return sb.ToString();
        }

        private byte[] AByte(string data)
        {
            var retorno = new byte[(data.Length / 2)];
            var j = 0;

            for (var i = 0; i < data.Length; i += 2)
            {
                var hexStr = data.Substring(i, 2);

                retorno[j] = Convert.ToByte(hexStr, 16);

                j++;
            }

            return retorno;
        }

        private byte[] CrearSalt()
        {
            return new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        }

        #endregion

        #region ICriptografiaService

        public string Encriptar(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var semilla = CrearSemilla();

            return Encriptar(data, semilla);
        }

        public string Encriptar(string data, string semilla)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(semilla))
            {
                throw new ArgumentNullException(nameof(semilla));
            }

            using (var td = new TripleDesCryptoService())
            {
                td.Key = ObtenerClave(semilla);

                var encryptData = td.EncryptText(data);

                return ACadena(encryptData);
            }
        }

        public string Desencriptar(string dataEncriptada)
        {
            if (string.IsNullOrEmpty(dataEncriptada))
            {
                throw new ArgumentNullException(nameof(dataEncriptada));
            }

            var semilla = CrearSemilla();

            return Desencriptar(dataEncriptada, semilla);
        }

        public string Desencriptar(string dataEncriptada, string semilla)
        {
            if (string.IsNullOrEmpty(dataEncriptada))
            {
                throw new ArgumentNullException(nameof(dataEncriptada));
            }

            if (string.IsNullOrEmpty(semilla))
            {
                throw new ArgumentNullException(nameof(semilla));
            }

            var encryptData = AByte(dataEncriptada);

            using (var td = new TripleDesCryptoService())
            {
                td.Key = ObtenerClave(semilla);

                return td.DecryptText(encryptData);
            }
        }

        public string EncriptarClave(string clave)
        {
            return EncriptarClave(clave, CONTRASENA_ENCRIPTACION_DEFAULT);
        }

        public string EncriptarClave(string clave, string semilla)
        {
            if (string.IsNullOrEmpty(clave))
            {
                throw new ArgumentNullException(nameof(clave));
            }

            if (string.IsNullOrEmpty(semilla))
            {
                throw new ArgumentNullException(nameof(semilla));
            }

            var claveBytes = Encoding.Unicode.GetBytes(clave);

            using (var aes = new AesCryptoService())
            {
                var salt = CrearSalt();
                var pdb = new Rfc2898DeriveBytes(semilla, salt);

                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                var encryptData = aes.Encrypt(claveBytes);

                return Convert.ToBase64String(encryptData);

            }
        }

        #endregion
    }
}
