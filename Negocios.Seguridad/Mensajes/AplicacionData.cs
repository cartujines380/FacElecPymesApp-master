using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    [Serializable]
    public class AplicacionData : ISerializable
    {
        #region Constantes

        private const string CAMPO_ID = "m_Id";
        private const string CAMPO_USUARIO_ID = "m_usuarioId";
        private const string CAMPO_TOKEN = "m_token";
        private const string CAMPO_MAQUINA = "m_maquina";
        private const string CAMPO_SITIO_TOKEN = "m_sitioToken";
        private const string CAMPO_SITIO_MAQUINA = "m_sitioMaquina";

        #endregion

        #region Campos

        private int m_Id;
        private string m_usuarioId;
        private string m_token;
        private string m_maquina;
        private string m_sitioToken;
        private string m_sitioMaquina;

        #endregion

        #region Propiedades

        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        public string UsuarioId
        {
            get
            {
                return m_usuarioId;
            }
            set
            {
                m_usuarioId = value;
            }
        }

        public string Token
        {
            get
            {
                return m_token;
            }
            set
            {
                m_token = value;
            }
        }

        public string Maquina
        {
            get
            {
                return m_maquina;
            }
            set
            {
                m_maquina = value;
            }
        }

        public string SitioToken
        {
            get
            {
                return m_sitioToken;
            }
            set
            {
                m_sitioToken = value;
            }
        }

        public string SitioMaquina
        {
            get
            {
                return m_sitioMaquina;
            }
            set
            {
                m_sitioMaquina = value;
            }
        }

        public ICollection<ServidorData> Servidores { get; set; }

        #endregion

        #region Constructores

        public AplicacionData()
        {

        }

        public AplicacionData(SerializationInfo info, StreamingContext context)
        {
            m_Id = AInt32((string)info.GetValue(CAMPO_ID, typeof(string))).Value;
            m_usuarioId = (string)info.GetValue(CAMPO_USUARIO_ID, typeof(string));
            m_token = (string)info.GetValue(CAMPO_TOKEN, typeof(string));
            m_maquina = (string)info.GetValue(CAMPO_MAQUINA, typeof(string));
            m_sitioToken = (string)info.GetValue(CAMPO_SITIO_TOKEN, typeof(string));
            m_sitioMaquina = (string)info.GetValue(CAMPO_SITIO_MAQUINA, typeof(string));
        }

        #endregion

        #region ISerializable

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(CAMPO_ID, ACadena(m_Id), typeof(string));
            info.AddValue(CAMPO_USUARIO_ID, m_usuarioId, typeof(string));
            info.AddValue(CAMPO_TOKEN, m_token, typeof(string));
            info.AddValue(CAMPO_MAQUINA, m_maquina, typeof(string));
            info.AddValue(CAMPO_SITIO_TOKEN, m_sitioToken, typeof(string));
            info.AddValue(CAMPO_SITIO_MAQUINA, m_sitioMaquina, typeof(string));
        }

        #endregion

        #region Metodos privados

        private string ACadena(int? valor)
        {
            return valor.HasValue ? valor.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        }

        private int? AInt32(string valor)
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
