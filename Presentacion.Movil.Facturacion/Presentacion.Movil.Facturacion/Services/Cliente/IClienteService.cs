using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente
{
    public interface IClienteService
    {
        Task<ObservableCollection<ClienteModel>> ObtenerCliente(string ruc, bool esTransportista);

        Task<bool> EsTransportista();

        Task<ClienteModel> GuardarCliente(ClienteModel cliente);

        Task<string> ObtenerDireccionCliente(InfoComprobanteModel info);
    }
}
