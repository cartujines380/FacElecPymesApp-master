using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface ICatalogoService : IDisposable
    {
        IEnumerable<Catalogo> ObtenerCatalogo(string nombreTabla);

        IEnumerable<Impuesto> ObtenerImpuestoPorCodigo(string tipo);

        IEnumerable<Catalogo> ObtenerCatalogoPais(int codigo);

        IEnumerable<Catalogo> ObtenerCatalogoProvinciaCiudad(int codigo, string descAlterno);

        IEnumerable<Catalogo> ObtenerCatalogoGeneral(int codigoTabla);

        IEnumerable<Catalogo> ObtenerFormaPago();

    }
}
