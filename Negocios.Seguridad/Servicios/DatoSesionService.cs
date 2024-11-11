using System;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class DatoSesionService : IDatoSesionService
    {
        #region Campos

        private readonly IConfiguracionServicioSeguridad m_configuracion;
        private readonly IDatoAplicacionService m_datoAplicacionService;
        private readonly IAutenticacionService m_autenticacionService;

        private bool m_desechado = false;

        #endregion

        #region Contructores

        public DatoSesionService(
            IConfiguracionServicioSeguridad configuracion,
            IDatoAplicacionService datoAplicacionService,
            IAutenticacionService autenticacionService
        )
        {
            m_configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            m_datoAplicacionService = datoAplicacionService ?? throw new ArgumentNullException(nameof(datoAplicacionService));
            m_autenticacionService = autenticacionService ?? throw new ArgumentNullException(nameof(autenticacionService));

            m_configuracion.Initialize();
        }

        #endregion

        #region IDatoSesionService

        public Sesion ObtenerSesionAgente()
        {
            var appData = m_datoAplicacionService.ObtenerAplicacionData();

            if (appData == null)
            {
                throw new InvalidOperationException("Sesion de aplicacion no existe");
            }

            var usuData = m_autenticacionService.ObtenerUsuarioDataAutenticado();

            if (usuData == null)
            {
                throw new InvalidOperationException("Sesion de usuario no existe");
            }

            return new Sesion()
            {
                Formulario = m_configuracion.Formulario,
                AplicacionId = m_configuracion.AplicacionId,
                EmpresaId = m_configuracion.EmpresaId,
                OrganizacionId = 1,
                SucursalId = m_configuracion.SucursalId,
                UsuarioId = usuData.NombreUsuario,
                Login = m_configuracion.Login,
                MaqSitio = appData.SitioMaquina,
                Maquina = usuData.Maquina,
                Token = usuData.Token,
                TokenSitio = appData.SitioToken,
                UsrSitio = m_configuracion.UsuarioSitio
            };
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
                m_autenticacionService.Dispose();
            }

            //Liberar objetos no manejados

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DatoSesionService()
        {
            Dispose(false);
        }

        #endregion
    }
}
