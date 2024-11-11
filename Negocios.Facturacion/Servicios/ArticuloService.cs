using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class ArticuloService : IArticuloService
    {
        #region Campos

        private readonly IArticuloRepository m_articuloRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public ArticuloService(
            IArticuloRepository articuloRepository
        )
        {
            m_articuloRepository = articuloRepository ?? throw new ArgumentNullException(nameof(articuloRepository));
        }

        #endregion

        #region ICatalogoService

        public IEnumerable<Articulo> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista, string idUsuario)
        {
            var result = m_articuloRepository.ObtenerArticulosPorEmpresa(criterioBusquedad, ruc, esTransportista, idUsuario);

            return result;
        }

        public Articulo Add(Articulo entity)
        {
            var result = m_articuloRepository.Add(entity);

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
                m_articuloRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ArticuloService()
        {
            Dispose(false);
        }

        #endregion
    }
}
