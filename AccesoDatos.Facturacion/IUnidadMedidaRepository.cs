using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IUnidadMedidaRepository : IRepository<UnidadMedida, int>
    {
        IEnumerable<UnidadMedida> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo);
    }
}
