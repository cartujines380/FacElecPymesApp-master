using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class CatalogoService : ICatalogoService
    {
        #region Campos

        private readonly ICatalogoRepository m_catalogoRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public CatalogoService(
            ICatalogoRepository catalagoRepository
        )
        {
            m_catalogoRepository = catalagoRepository ?? throw new ArgumentNullException(nameof(catalagoRepository));
        }

        #endregion

        #region ICatalogoService

        public IEnumerable<Catalogo> ObtenerCatalogo(string nombreTabla)
        {
            var result = m_catalogoRepository.ObtenerCatalogo(nombreTabla);

            return result;
        }

        public IEnumerable<Impuesto> ObtenerImpuestoPorCodigo(string tipo)
        {
            var result = m_catalogoRepository.ObtenerImpuestoPorCodigo(tipo);

            return result;
        }

        public IEnumerable<Catalogo> ObtenerCatalogoPais(int codigo)
        {
            var result = m_catalogoRepository.ObtenerCatalogoPais(codigo);

            return result;
        }

        public IEnumerable<Catalogo> ObtenerCatalogoProvinciaCiudad(int codigo, string descAlterno)
        {
            var result = m_catalogoRepository.ObtenerCatalogoProvinciaCiudad(codigo,descAlterno);

            return result;
        }

        public IEnumerable<Catalogo> ObtenerCatalogoGeneral(int codigoTabla)
        {
            var result = m_catalogoRepository.ObtenerCatalogoGeneral(codigoTabla);

            return result;
        }

       

        public IEnumerable<Catalogo> ObtenerFormaPago()
        {
            var result = m_catalogoRepository.ObtenerFormaPago();

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
                m_catalogoRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CatalogoService()
        {
            Dispose(false);
        }

        #endregion
    }
}
