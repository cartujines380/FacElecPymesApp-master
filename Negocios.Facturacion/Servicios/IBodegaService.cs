using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IBodegaService : IDisposable
    {
        IEnumerable<Bodega> ObtenerBodegaPorEmpresa(string ruc);
    }
}
