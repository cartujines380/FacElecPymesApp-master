using System;
using Microsoft.Extensions.Configuration;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Mensajeria
{
    public class AppSettingsEmailConfiguration : IEmailConfiguration
    {
        #region Constantes

        public const string SECTION_KEY = "ConfiguracionEmail";

        public const string HOST_CLAVE = "Host";
        public const string PORT_CLAVE = "Port";
        public const string USER_NAME_CLAVE = "User";
        public const string PASSWORD_CLAVE = "Password";
        public const string DEFAULT_CREDENTIALS_CLAVE = "DefaultCredentials";
        public const string ENABLE_SSL_CLAVE = "EnableSsl";
        public const string TIMEOUT_CLAVE = "Timeout";

        public const string HOST_DEFAULT = "localhost";
        public const int PORT_DEFAULT = 25;
        public const string USER_NAME_DEFAULT = "";
        public const string PASSWORD_DEFAULT = "";
        public const bool DEFAULT_CREDENTIALS_DEFAULT = false;
        public const bool ENABLE_SSL_DEFAULT = false;
        public const int TIMEOUT_DEFAULT = 100000;

        #endregion

        #region Campos

        private readonly IConfiguration m_configuration;

        private string m_host;
        private int m_port;
        private string m_userName;
        private string m_password;
        private bool m_useDefaultCredentials;
        private bool m_enableSsl;
        private int m_timeout;

        #endregion

        #region Constructores

        public AppSettingsEmailConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;

            m_host = HOST_DEFAULT;
            m_port = PORT_DEFAULT;
            m_userName = USER_NAME_DEFAULT;
            m_password = PASSWORD_DEFAULT;
            m_useDefaultCredentials = DEFAULT_CREDENTIALS_DEFAULT;
            m_enableSsl = ENABLE_SSL_DEFAULT;
            m_timeout = TIMEOUT_DEFAULT;
        }

        #endregion

        #region Metodos privados

        private string ObtenerConfiguracion(string clave, string valorPredeterminado)
        {
            var valor = m_configuration.GetSection(SECTION_KEY).GetSection(clave).Value;

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            return valor;
        }

        private int? ObtenerConfiguracionInt32(string clave, int? valorPredeterminado)
        {
            var valor = ObtenerConfiguracion(clave, null);

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            return int.TryParse(valor, out int aux) ? aux : valorPredeterminado;
        }

        private bool? ObtenerConfiguracionBool(string clave, bool? valorPredeterminado)
        {
            var valor = ObtenerConfiguracion(clave, null);

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            return bool.TryParse(valor, out bool aux) ? aux : valorPredeterminado;
        }

        #endregion

        #region IEmailConfiguration

        public void Initialize()
        {
            m_host = ObtenerConfiguracion(HOST_CLAVE, HOST_DEFAULT);
            m_port = ObtenerConfiguracionInt32(PORT_CLAVE, PORT_DEFAULT).Value;
            m_userName = ObtenerConfiguracion(USER_NAME_CLAVE, USER_NAME_DEFAULT);
            m_password = ObtenerConfiguracion(PASSWORD_CLAVE, PASSWORD_DEFAULT);
            m_useDefaultCredentials = ObtenerConfiguracionBool(DEFAULT_CREDENTIALS_CLAVE, DEFAULT_CREDENTIALS_DEFAULT).Value;
            m_enableSsl = ObtenerConfiguracionBool(ENABLE_SSL_CLAVE, ENABLE_SSL_DEFAULT).Value;
            m_timeout = ObtenerConfiguracionInt32(TIMEOUT_CLAVE, TIMEOUT_DEFAULT).Value;
        }

        public string Host
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public int Port
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
            }
        }

        public string UserName
        {
            get
            {
                return m_userName;
            }
            set
            {
                m_userName = value;
            }
        }

        public string Password
        {
            get
            {
                return m_password;
            }
            set
            {
                m_password = value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                return m_useDefaultCredentials;
            }
            set
            {
                m_useDefaultCredentials = value;
            }
        }

        public bool EnableSsl
        {
            get
            {
                return m_enableSsl;
            }
            set
            {
                m_enableSsl = value;
            }
        }

        public int Timeout
        {
            get
            {
                return m_timeout;
            }
            set
            {
                m_timeout = value;
            }
        }

        #endregion
    }
}
