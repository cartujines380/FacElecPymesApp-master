using System.Collections.ObjectModel;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente
{
    public class ClienteRequest
    {
        public string RucEmpresa { get; set; }

        public ObservableCollection<ClienteModel> Cliente { get; set; }
    }
}
