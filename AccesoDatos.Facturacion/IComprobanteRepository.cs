using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IComprobanteRepository : IRepository<Comprobante, string>
    {
        IEnumerable<Comprobante> ObtenerComprobantes(FiltroModel request);

        Comprobante ObtenerComprobantesXML(InfoComprobanteModel request);
    }
}
