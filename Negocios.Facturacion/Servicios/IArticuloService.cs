using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IArticuloService : IDisposable
    {
        IEnumerable<Articulo> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista, string idUsuario);

        Articulo Add(Articulo entity);
    }
}
