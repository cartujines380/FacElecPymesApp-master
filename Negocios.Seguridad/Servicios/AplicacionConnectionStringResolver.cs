using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class AplicacionConnectionStringResolver : IConnectionStringResolver
    {
        #region Constantes

        public const string CONNECTION_STRING_DEFAULT = "Default";
        public const string DEFAULT_PROVIDER_NAME = "System.Data.SqlClient";

        #endregion

        #region Campos

        private readonly IConfiguracionServicioSeguridad m_configuracion;
        private readonly IDatoAplicacionService m_datoAplicacionService;
        private readonly IRegistroAplicacionService m_registroAppSrv;
        private static readonly ILogger m_logger = LoggerFactory.CreateLogger<AplicacionConnectionStringResolver>();

        #endregion

        #region Constructores

        public AplicacionConnectionStringResolver(
            IConfiguracionServicioSeguridad configuracion,
            IDatoAplicacionService datoAplicacionService,
            IRegistroAplicacionService registroAppSrv
        )
        {
            m_configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            m_datoAplicacionService = datoAplicacionService ?? throw new ArgumentNullException(nameof(datoAplicacionService));
            m_registroAppSrv = registroAppSrv ?? throw new ArgumentNullException(nameof(registroAppSrv));

            m_configuracion.Initialize();
        }

        #endregion
        
        #region Metodos privados

        private string GetParameterValue(string clave, IDictionary<string, string> valores)
        {
            if (valores.TryGetValue(clave, out var valor))
            {
                return valor;
            }

            return null;
        }

        private string GetConnectionString(ServidorData servidor)
        {
            var user = GetParameterValue("Usuario", servidor.Parametros);
            var password = GetParameterValue("Clave", servidor.Parametros);
            var connectTimeoutStr = GetParameterValue("ConnectionTimeOut", servidor.Parametros);

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = GetParameterValue("Servidor", servidor.Parametros),
                InitialCatalog = GetParameterValue("BaseDatos", servidor.Parametros)
            };

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                builder.UserID = user;
                builder.Password = password;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            if (int.TryParse(connectTimeoutStr, out var connectTimeout))
            {
                builder.ConnectTimeout = connectTimeout;
            }

            return builder.ConnectionString;
        }

        #endregion

        #region IConnectionStringResolver

        public ConnectionStringItem GetConnectionString()
        {
            return GetConnectionString(CONNECTION_STRING_DEFAULT);
        }

        public ConnectionStringItem GetConnectionString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var connStrings = GetAllConnectionStrings();

            var connStrItems = connStrings.Where(cs => cs.Name == name);

            if (connStrItems.Any())
            {
                return connStrItems.First();
            }

            var message = string.Format("No existe cadena conexion con nombre [{0}]", name);

            throw new InvalidOperationException(message);
        }

        public IEnumerable<ConnectionStringItem> GetAllConnectionStrings()
        {
            var retorno = new List<ConnectionStringItem>();

            var appData = m_datoAplicacionService.ObtenerAplicacionData();

            if (appData == null)
            {
                //Intentamos autentica la aplicacion nuevamente
                var response = m_registroAppSrv.AutenticarAplicacion();

                if (response.Exito)
                {
                    m_datoAplicacionService.EstablecerAplicacionData(response.Aplicacion);
                    appData = m_datoAplicacionService.ObtenerAplicacionData();
                }
            }

            if (appData == null)
            {
                m_logger.Error("appData es nulo");
            }

            m_logger.Info(string.Format("Servicio base Datos ID: [{0}]", m_configuracion.ServidorBaseDatosId));

            if (appData != null)
            {
                foreach (var srv in appData.Servidores)
                {
                    m_logger.Info(string.Format("ServicioID ID: [{0}]", srv.Id));
                }
            }

            var servidor = appData != null ? appData.Servidores
                .Where(s => s.Id == m_configuracion.ServidorBaseDatosId)
                .FirstOrDefault() : null;


            if (servidor == null)
            {
                m_logger.Error("servidor es nulo");
            }

            if (servidor != null)
            {
                var connStrall = GetConnectionString(servidor);

                m_logger.Info("Cadeb conexion es : " + connStrall);

                var connStrItem = new ConnectionStringItem()
                {
                    Name = CONNECTION_STRING_DEFAULT,
                    ConnectionString = connStrall ?? string.Empty,
                    ProviderName = DEFAULT_PROVIDER_NAME
                };

                retorno.Add(connStrItem);
            }

            return retorno;
        }

        #endregion
    }
}
