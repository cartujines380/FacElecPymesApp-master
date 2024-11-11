using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IClienteRepository : IRepository<Cliente, string>
    {
        IEnumerable<Cliente> ObtenerClientesPorEmpresa(ObtenerClientesPorEmpresaRequest request);

        bool EsTransportista(string idUsuario);

        string ObtenerDireccionCliente(InfoComprobanteModel info);
    }
}
