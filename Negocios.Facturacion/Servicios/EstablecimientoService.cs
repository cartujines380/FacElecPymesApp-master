using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class EstablecimientoService : IEstablecimientoService
    {
        #region Campos

        private readonly IEstablecimientoRepository m_establecimientoRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public EstablecimientoService(
            IEstablecimientoRepository establecimientoRepository
        )
        {
            m_establecimientoRepository = establecimientoRepository ?? throw new ArgumentNullException(nameof(establecimientoRepository));
        }

        #endregion

        #region IEstablecimientoService

        public IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuarioCmb(string usuarioId)
        {
            var result = m_establecimientoRepository.ObtenerEstablecimientosPorUsuarioCmb(usuarioId);

            return result;
        }

        public IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuario(string usuarioId)
        {
            var result = m_establecimientoRepository.ObtenerEstablecimientosPorUsuario(usuarioId);

            return result;
        }

        public EstablecimientoData Obtenerplan(string empresaRuc)
        {
            var result = m_establecimientoRepository.Obtenerplan(empresaRuc);

            return result;
        }

        public IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosTransportitasPorUsuario(string usuarioId)
        {
            var result = m_establecimientoRepository.ObtenerDataEstablecimientosTransportitasPorUsuario(usuarioId);

            return result;
        }

        public string ObtenerDireccionSucursal(InfoComprobanteModel info)
        {
            var result = m_establecimientoRepository.ObtenerDireccionSucursal(info);

            return result;
        }

        #endregion

        #region IDispose

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                m_establecimientoRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~EstablecimientoService()
        {
            Dispose(false);
        }

        #endregion
    }
}
