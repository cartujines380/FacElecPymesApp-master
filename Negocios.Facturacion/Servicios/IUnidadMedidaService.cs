using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IUnidadMedidaService : IDisposable
    {
        IEnumerable<UnidadMedida> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo);
    }
}
