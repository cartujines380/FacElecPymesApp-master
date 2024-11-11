using System;
using System.Collections.Generic;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IFacturaRepository : IRepository<Factura>
    {
        IEnumerable<Sucursal> ObtenerEstablecimiento(string ruc, string esTransportista);

        IEnumerable<PuntoEmision> ObtenerPtoEmision(string ruc, string establecimiento, string esTransportista);

        int ObtenerSecuencial(ObtenerSecuencialRequest request);

        void GuardarFactura(Factura factura, string usuario);

        int ObtenerTotalComprobante(string ruc, string estado);

        int ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc);
    }
}
