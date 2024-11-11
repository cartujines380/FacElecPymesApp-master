using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    [Serializable]
    public class UsuarioData : ISerializable
    {
        #region Constantes

        private const string CAMPO_ID = "m_Id";
        private const string CAMPO_NOMBRE = "m_nombre";
        private const string CAMPO_NOMBRE_USUARIO = "m_nombreUsuario";
        private const string CAMPO_TOKEN = "m_token";
        private const string CAMPO_MAQUINA = "m_maquina";

        #endregion

        #region Campos

        private int m_Id;
        private string m_nombre;
        private string m_nombreUsuario;
        private string m_token;
        private string m_maquina;

        #endregion

        #region Propiedades

        public int Id { get => m_Id; set => m_Id = value; }

        public string Nombre { get => m_nombre; set => m_nombre = value; }

        public string NombreUsuario { get => m_nombreUsuario; set => m_nombreUsuario = value; }

        public string Token { get => m_token; set => m_token = value; }

        public string Maquina { get => m_maquina; set => m_maquina = value; }

        #endregion

        #region Constructores

        public UsuarioData()
        {

        }

        public UsuarioData(SerializationInfo info, StreamingContext context)
        {
            m_Id = AInt32((string)info.GetValue(CAMPO_ID, typeof(string))).Value;
            m_nombre = (string)info.GetValue(CAMPO_NOMBRE, typeof(string));
            m_nombreUsuario = (string)info.GetValue(CAMPO_NOMBRE_USUARIO, typeof(string));
            m_token = (string)info.GetValue(CAMPO_TOKEN, typeof(string));
            m_maquina = (string)info.GetValue(CAMPO_MAQUINA, typeof(string));
        }

        #endregion

        #region ISerializable

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(CAMPO_ID, ACadena(m_Id), typeof(string));
            info.AddValue(CAMPO_NOMBRE, m_nombre, typeof(string));
            info.AddValue(CAMPO_NOMBRE_USUARIO, m_nombreUsuario, typeof(string));
            info.AddValue(CAMPO_TOKEN, m_token, typeof(string));
            info.AddValue(CAMPO_MAQUINA, m_maquina, typeof(string));
        }

        #endregion

        #region Metodos privados

        protected virtual string ACadena(int? valor)
        {
            return valor.HasValue ? valor.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        }

        protected virtual int? AInt32(string valor)
        {
            int retorno = default(int);

            if (int.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out retorno))
            {
                return retorno;
            }

            return (int?)null;
        }

        #endregion
    }
}
