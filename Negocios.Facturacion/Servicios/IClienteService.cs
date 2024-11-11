using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IClienteService : IDisposable
    {
        IEnumerable<Cliente> ObtenerClientesPorEmpresa(ObtenerClientesPorEmpresaRequest request);

        bool EsTransportista(string idUsuario);

        Cliente Add(Cliente entity);

        string ObtenerDireccionCliente(InfoComprobanteModel info);
    }
}
