using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo
{
    public interface ICatalogoService
    {
        Task<ObservableCollection<CatalogoModel>> ObtenerCatalogo(string nombreTabla);

        Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoGeneral(int codigo);

        Task<ObservableCollection<CatalogoModel>> ObtenerFormaPago();

        Task<ObservableCollection<ImpuestoModel>> ObtenerImpuestoPorCodigo(string tipo);

        Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoPais(int codigo);

        Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoProvinciaCiudad(int codigo, string descAlterno);
    }
}
