using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IBodegaRepository : IRepository<Bodega, int>
    {
        IEnumerable<Bodega> ObtenerBodegaPorEmpresa(string ruc);
    }
}
