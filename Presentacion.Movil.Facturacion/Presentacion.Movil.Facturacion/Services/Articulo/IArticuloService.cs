using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Articulo
{
    public interface IArticuloService
    {
        Task<ObservableCollection<ArticuloModel>> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista);

        Task<ArticuloModel> GuardarArticulo(ArticuloModel articulo);
    }
}
