using System;
using Microsoft.Extensions.Configuration;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones
{
    public class FrameworkEndpointConfiguration : IEndpointConfiguration
    {
        #region Constantes

        public const string BASE_ADDRESS_DEFAULT = "http://localhost/WsFramework/";
        public const double OPEN_TIMEOUT_DEFAULT = 60000D; //1 minuto
        public const double RECEIVE_TIMEOUT_DEFAULT = 600000D; //10 minutos
        public const double SEND_TIMEOUT_DEFAULT = 60000D; //1 minuto
        public const double CLOSE_TIMEOUT_DEFAULT = 60000D; //1 minuto

        public const string SECTION_KEY = "FrameworkSeguridad";

        public const string BASE_ADDRESS_KEY = "BaseAddress";
        public const string OPEN_TIMEOUT_KEY = "OpenTimeout";
        public const string RECEIVE_TIMEOUT_KEY = "ReceiveTimeout";
        public const string SEND_TIMEOUT_KEY = "SendTimeout";
        public const string CLOSE_TIMEOUT_KEY = "CloseTimeout";

        #endregion

        #region Campos

        private readonly IConfiguration m_configuration;
        private string m_baseAddress;
        private double m_openTimeout;
        private double m_receiveTimeout;
        private double m_sendTimeout;
        private double m_closeTimeout;

        #endregion

        #region Constructores

        public FrameworkEndpointConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;

            m_baseAddress = BASE_ADDRESS_DEFAULT;
            m_openTimeout = OPEN_TIMEOUT_DEFAULT;
            m_receiveTimeout = RECEIVE_TIMEOUT_DEFAULT;
            m_sendTimeout = SEND_TIMEOUT_DEFAULT;
            m_closeTimeout = CLOSE_TIMEOUT_DEFAULT;
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

        private double? ObtenerConfiguracionDouble(string clave, double? valorPredeterminado)
        {
            var valor = ObtenerConfiguracion(clave, null);

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            var aux = 0D;

            return double.TryParse(valor, out aux) ? aux : valorPredeterminado;
        }

        #endregion

        #region IEndpointConfiguration

        public void Initialize()
        {
            m_baseAddress = ObtenerConfiguracion(BASE_ADDRESS_KEY, BASE_ADDRESS_DEFAULT);
            m_openTimeout = ObtenerConfiguracionDouble(OPEN_TIMEOUT_KEY, OPEN_TIMEOUT_DEFAULT).Value;
            m_receiveTimeout = ObtenerConfiguracionDouble(RECEIVE_TIMEOUT_KEY, RECEIVE_TIMEOUT_DEFAULT).Value;
            m_sendTimeout = ObtenerConfiguracionDouble(SEND_TIMEOUT_KEY, SEND_TIMEOUT_DEFAULT).Value;
            m_closeTimeout = ObtenerConfiguracionDouble(CLOSE_TIMEOUT_KEY, CLOSE_TIMEOUT_DEFAULT).Value;
        }

        public string BaseAddress
        {
            get
            {
                return m_baseAddress;
            }
            set
            {
                m_baseAddress = value;
            }
        }

        public double OpenTimeout
        {
            get
            {
                return m_openTimeout;
            }
            set
            {
                m_openTimeout = value;
            }
        }

        public double ReceiveTimeout
        {
            get
            {
                return m_receiveTimeout;
            }
            set
            {
                m_receiveTimeout = value;
            }
        }

        public double SendTimeout
        {
            get
            {
                return m_sendTimeout;
            }
            set
            {
                m_sendTimeout = value;
            }
        }

        public double CloseTimeout
        {
            get
            {
                return m_closeTimeout;
            }
            set
            {
                m_closeTimeout = value;
            }
        }

        #endregion
    }
}
