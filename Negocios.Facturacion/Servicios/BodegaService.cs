using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class BodegaService : IBodegaService
    {
        #region Campos

        private readonly IBodegaRepository m_bodegaRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public BodegaService(
            IBodegaRepository bodegaRepository
        )
        {
            m_bodegaRepository = bodegaRepository ?? throw new ArgumentNullException(nameof(bodegaRepository));
        }

        #endregion

        #region IBodegaService

        public IEnumerable<Bodega> ObtenerBodegaPorEmpresa(string ruc)
        {
            var result = m_bodegaRepository.ObtenerBodegaPorEmpresa(ruc);

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
                m_bodegaRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BodegaService()
        {
            Dispose(false);
        }

        #endregion

    }
}
