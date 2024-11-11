using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }

        string AuthIdToken { get; set; }

        string UserName { get; set; }

        string IdentityEndpointBase { get; set; }

        string FacturacionEndpointBase { get; set; }

        string PDFEndpointBase { get; set; }
    }
}
