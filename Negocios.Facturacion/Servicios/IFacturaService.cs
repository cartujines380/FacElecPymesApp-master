using System;
using System.Collections.Generic;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IFacturaService : IDisposable
    {
        IEnumerable<Sucursal> ObtenerEstablecimiento(string ruc, string esTransportista);

        IEnumerable<PuntoEmision> ObtenerPtoEmision(string ruc, string establecimiento, string esTransportista);

        int ObtenerSecuencial(ObtenerSecuencialRequest request);

        GuardarFacturaResponse GuardarFactura(GuardarFacturaRequest request);

        int ObtenerTotalComprobante(string ruc, string estado);

        int ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc);
    }
}
