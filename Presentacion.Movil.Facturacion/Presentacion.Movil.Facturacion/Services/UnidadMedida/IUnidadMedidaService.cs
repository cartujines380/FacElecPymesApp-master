using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.UnidadMedida;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.UnidadMedida
{
    public interface IUnidadMedidaService
    {
        Task<ObservableCollection<UnidadMedidaModel>> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo);
    }
}
