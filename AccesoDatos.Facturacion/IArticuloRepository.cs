using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IArticuloRepository : IRepository<Articulo, string>
    {
        IEnumerable<Articulo> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista, string idUsuario);
    }
}
