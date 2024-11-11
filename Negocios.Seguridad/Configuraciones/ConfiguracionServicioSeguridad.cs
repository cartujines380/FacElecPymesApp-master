using System;
using Microsoft.Extensions.Configuration;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones
{
    public class ConfiguracionServicioSeguridad : IConfiguracionServicioSeguridad
    {
        #region Constantes

        public const string MAQUINA_DEFAULT = "1::";
        public const int EMPRESA_ID_DEFAULT = 2; // Sipecom
        public const int SUSCURSAL_ID_DEFAULTD = 3; // Sipecom Matriz
        public const int APLICACION_ID_DEFAULT = 1; // Servientrega Administracion Pos
        public const string DOMINIO_DEFAULT = "DOMSIPECOM"; //
        public const char PERFIL_DEFAULT = 'M'; // 77
        public const string TOKEN_DEFAULT = "111111111111111111111111111111"; //
        public const string TOKEN_SITIO_DEFAULT = "111111111111111111111111111111"; //
        public const string USUARIO_SITIO_DEFAULT = "usrWebServiceFS"; //
        public const string FORMULARIO_DEFAULT = "Index"; //
        public const char LOGIN_DEFAULT = '1'; //
        public const string SERVIDOR_BASE_DATOS_DEFAULT = "25";
        public const string CORREO_REMITENTE_DEFAULT = "facturacion_electronica@invoicec.com";
        public const string CORREO_PLANTILLA_RUTA_DEFAULT = "~/notificacion.htm";

        public const string SECTION_KEY = "ConfiguracionServicioSeguridad";

        public const string MAQUINA_KEY = "Maquina";
        public const string EMPRESA_ID_KEY = "EmpresaId";
        public const string SUSCURSAL_ID_KEY = "SucursalId";
        public const string APLICACION_ID_KEY = "AplicacionId";
        public const string DOMINIO_KEY = "Dominio";
        public const string PERFIL_KEY = "Perfil";
        public const string TOKEN_KEY = "Token";
        public const string TOKEN_SITIO_KEY = "TokenSitio";
        public const string USUARIO_SITIO_KEY = "UsuarioSitio";
        public const string FORMULARIO_KEY = "Formulario";
        public const string LOGIN_KEY = "Login";
        public const string SERVIDOR_BASE_DATOS_KEY = "ServidorBaseDatosId";
        public const string CORREO_REMITENTE_KEY = "CorreoRemitente";
        public const string CORREO_PLANTILLA_RUTA_KEY = "CorreoPlantillaRuta";

        #endregion

        #region Campos

        private readonly IConfiguration m_configuration;
        private string m_maquina;
        private int m_empresaId;
        private int m_sucursalId;
        private int m_aplicacionId;
        private string m_dominio;
        private char m_perfil;
        private string m_token;
        private string m_tokenSitio;
        private string m_usuarioSitio;
        private string m_formulario;
        private char m_login;
        private string m_servidorBaseDatosId;
        private string m_correoRemitente;
        private string m_correoPlantillaRuta;

        #endregion

        #region Constructores

        public ConfiguracionServicioSeguridad(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;
            m_maquina = MAQUINA_DEFAULT;
            m_empresaId = EMPRESA_ID_DEFAULT;
            m_sucursalId = SUSCURSAL_ID_DEFAULTD;
            m_aplicacionId = APLICACION_ID_DEFAULT;
            m_dominio = DOMINIO_DEFAULT;
            m_perfil = PERFIL_DEFAULT;
            m_token = TOKEN_DEFAULT;
            m_tokenSitio = TOKEN_SITIO_DEFAULT;
            m_usuarioSitio = USUARIO_SITIO_DEFAULT;
            m_formulario = FORMULARIO_DEFAULT;
            m_servidorBaseDatosId = SERVIDOR_BASE_DATOS_DEFAULT;
            m_correoRemitente = CORREO_REMITENTE_DEFAULT;
            m_correoPlantillaRuta = CORREO_PLANTILLA_RUTA_DEFAULT;
        }

        #endregion

        #region IConfiguracionServicioSeguridad

        public void Initialize()
        {
            m_maquina = ObtenerConfiguracion(MAQUINA_KEY, MAQUINA_DEFAULT);
            m_empresaId = ObtenerConfiguracionInt(EMPRESA_ID_KEY, EMPRESA_ID_DEFAULT).Value;
            m_sucursalId = ObtenerConfiguracionInt(SUSCURSAL_ID_KEY, SUSCURSAL_ID_DEFAULTD).Value;
            m_aplicacionId = ObtenerConfiguracionInt(APLICACION_ID_KEY, APLICACION_ID_DEFAULT).Value;
            m_dominio = ObtenerConfiguracion(DOMINIO_KEY, DOMINIO_DEFAULT);
            m_perfil = ObtenerConfiguracionChar(PERFIL_KEY, PERFIL_DEFAULT).Value;
            m_token = ObtenerConfiguracion(TOKEN_KEY, TOKEN_DEFAULT);
            m_tokenSitio = ObtenerConfiguracion(TOKEN_SITIO_KEY, TOKEN_SITIO_DEFAULT);
            m_usuarioSitio = ObtenerConfiguracion(USUARIO_SITIO_KEY, USUARIO_SITIO_DEFAULT);
            m_formulario = ObtenerConfiguracion(FORMULARIO_KEY, FORMULARIO_DEFAULT);
            m_login = ObtenerConfiguracionChar(FORMULARIO_KEY, LOGIN_DEFAULT).Value;
            m_servidorBaseDatosId = ObtenerConfiguracion(SERVIDOR_BASE_DATOS_KEY, SERVIDOR_BASE_DATOS_DEFAULT);
            m_correoRemitente = ObtenerConfiguracion(CORREO_REMITENTE_KEY, CORREO_REMITENTE_DEFAULT);
            m_correoPlantillaRuta = ObtenerConfiguracion(CORREO_PLANTILLA_RUTA_KEY, CORREO_PLANTILLA_RUTA_DEFAULT);
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

        private int? ObtenerConfiguracionInt(string clave, int? valorPredeterminado)
        {
            var valor = ObtenerConfiguracion(clave, null);

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            var aux = 0;

            return int.TryParse(valor, out aux) ? aux : valorPredeterminado;
        }

        private char? ObtenerConfiguracionChar(string clave, char? valorPredeterminado)
        {
            var valor = ObtenerConfiguracion(clave, null);

            if (string.IsNullOrEmpty(valor))
            {
                return valorPredeterminado;
            }

            var aux = '0';

            return char.TryParse(valor, out aux) ? aux : valorPredeterminado;
        }

        #endregion

        #region Propiedades Publicas

        public string Maquina
        {
            get => m_maquina;
            set => m_maquina = value;
        }

        public int EmpresaId
        {
            get => m_empresaId;
            set => m_empresaId = value;
        }

        public int SucursalId
        {
            get => m_sucursalId;
            set => m_sucursalId = value;
        }

        public int AplicacionId
        {
            get => m_aplicacionId;
            set => m_aplicacionId = value;
        }

        public string Dominio
        {
            get => m_dominio;
            set => m_dominio = value;
        }

        public char Perfil
        {
            get => m_perfil;
            set => m_perfil = value;
        }

        public string Token
        {
            get => m_token;
            set => m_token = value;
        }

        public string TokenSitio
        {
            get => m_tokenSitio;
            set => m_tokenSitio = value;
        }

        public string UsuarioSitio
        {
            get => m_usuarioSitio;
            set => m_usuarioSitio = value;
        }

        public string Formulario
        {
            get => m_formulario;
            set => m_formulario = value;
        }

        public char Login
        {
            get => m_login;
            set => m_login = value;
        }

        public string ServidorBaseDatosId
        {
            get => m_servidorBaseDatosId;
            set => m_servidorBaseDatosId = value;
        }

        public string CorreoRemitente
        {
            get => m_correoRemitente;
            set => m_correoRemitente = value;
        }

        public string CorreoPlantillaRuta
        {
            get => m_correoPlantillaRuta;
            set => m_correoPlantillaRuta = value;
        }

        #endregion
    }
}
