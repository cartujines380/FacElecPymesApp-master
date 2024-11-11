using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura
{
    public interface IFacturaService
    {
        Task<ObservableCollection<NumeroFacturaModel>> ObtenerCodigoEstablecimiento(string ruc, bool esTransportista);

        Task<ObservableCollection<NumeroFacturaModel>> ObtenerCodigoPuntoEmision(string ruc, string codEstablecimiento, bool esTransportista);

        Task<int> ObtenerSecuencial(string ruc, string codEstablecimiento, string codPtoEmision);

        Task<int> ObtenerTotalComprobante(string ruc, string estado);

        Task<int> ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc);

        Task<GuardarFacturaResponse> GuardarFactura(GuardarFacturaRequest request);
    }
}
