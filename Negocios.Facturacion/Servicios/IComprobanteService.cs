using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IComprobanteService : IDisposable
    {
        IEnumerable<Comprobante> ObtenerComprobantes(FiltroModel request);

        Comprobante ObtenerComprobantesXML(InfoComprobanteModel request);
    }
}
