using System;
using System.Linq;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Base.Services
{
    public class AplicacionService : IAplicacionService
    {
        private static readonly ILogger m_logger = LoggerFactory.CreateLogger<AplicacionService>();

        private readonly IRegistroAplicacionService m_registroAppSrv;
        private readonly IDatoAplicacionService m_datoAppSrv;

        public AplicacionService(
            IRegistroAplicacionService registroAppSrv,
            IDatoAplicacionService datoAppSrv
        )
        {
            m_registroAppSrv = registroAppSrv;
            m_datoAppSrv = datoAppSrv;
        }

        public void IniciaSesion()
        {
            try
            {
                var response = m_registroAppSrv.AutenticarAplicacion();

                if (response.Exito)
                {
                   
                    m_datoAppSrv.EstablecerAplicacionData(response.Aplicacion);
                }
                else
                {
                    m_logger.Error(response.Validaciones.First());
                }
            }
            catch (Exception ex)
            {
                m_logger.Error("Error al iniciar sesion de aplicacion", ex);
            }
        }
    }
}
