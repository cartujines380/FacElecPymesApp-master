using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class UnidadMedidaService : IUnidadMedidaService
    {

        #region Campos

        private readonly IUnidadMedidaRepository m_unidadMedidaRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public UnidadMedidaService(
            IUnidadMedidaRepository unidadMedidaRepository
        )
        {
            m_unidadMedidaRepository = unidadMedidaRepository ?? throw new ArgumentNullException(nameof(unidadMedidaRepository));
        }

        #endregion

        #region ICatalogoService

        public IEnumerable<UnidadMedida> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo)
        {
            var result = m_unidadMedidaRepository.ObtenerUnidadMedidaPorArticulo(ruc, codArticulo);

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
                m_unidadMedidaRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnidadMedidaService()
        {
            Dispose(false);
        }

        #endregion




    }
}
