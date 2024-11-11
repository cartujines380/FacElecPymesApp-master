using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Bodega
{
    public interface IBodegaService 
    {
        Task<ObservableCollection<BodegaModel>> ObtenerBodegaPorEmpresa(string ruc);
    }
}
