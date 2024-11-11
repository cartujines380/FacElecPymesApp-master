using System;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Cache;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Recursos;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class DatoAplicacionService : IDatoAplicacionService
    {
        #region Constantes

        private const double TIEMPO_VIGENCIA_DEFAULT = 28800000D;

        #endregion

        #region Campos

        private readonly ICacheManager m_cacheManager;

        #endregion

        #region Constructores

        public DatoAplicacionService(ICacheManager cacheManager)
        {
            m_cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        #endregion

        #region IDatoAplicacionService

        public AplicacionData ObtenerAplicacionData()
        {
            return m_cacheManager.Get<AplicacionData>(CodigosAplicacion.cod_AppData);
        }

        public void EstablecerAplicacionData(AplicacionData aplicacion)
        {
            EstablecerAplicacionData(aplicacion, TIEMPO_VIGENCIA_DEFAULT);
        }

        public void EstablecerAplicacionData(AplicacionData aplicacion, double tiempoVigencia)
        {
            m_cacheManager.Set<AplicacionData>(CodigosAplicacion.cod_AppData, aplicacion, tiempoVigencia);
        }

        #endregion
    }
}
