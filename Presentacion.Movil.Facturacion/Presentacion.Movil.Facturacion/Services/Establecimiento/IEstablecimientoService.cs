using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento
{
    public interface IEstablecimientoService
    {
        Task<ObservableCollection<EstablecimientoModel>> ObtenerEstablecimientoCmb();

        Task<EstablecimientoModel> ObtenerEstablecimiento();

        Task<EstablecimientoModel> ObtenerPlan(string empresaRuc);

        Task<ObservableCollection<EstablecimientoModel>> ObtenerDataEstablecimientosTransportitasPorUsuario();

        Task<string> ObtenerDireccionSucursal(InfoComprobanteModel info);

    }
}
