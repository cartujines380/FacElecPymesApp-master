using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalClienteViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<ClienteModel> m_cliente;

        private ClienteModel clienteSeleccionado;

        #endregion

        #region Propiedades

        public string RucEmpresa { get; set; }

        public ObservableCollection<ClienteModel> ClienteData { get; set; }

        public ObservableCollection<ClienteModel> Cliente
        {
            get
            {
                return m_cliente;
            }
            set
            {
                m_cliente = value;
                RaisePropertyChanged(() => Cliente);
            }
        }


        public ClienteModel ClienteSeleccionado
        {
            get
            {
                return clienteSeleccionado;
            }
            set
            {
                clienteSeleccionado = value;
                _ = Seleccion();
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand CrearClienteComannd => new Command(async () => await CrearClienteAsync());

        public ICommand BuscarCommand => new Command<string>(BuscarAsync);

        #endregion

        #region Metodos

        public override Task InitializeAsync(object navigationData)
        {
            var clienteInfo = navigationData as ClienteRequest;

            if (clienteInfo != null)
            {
                Cliente = clienteInfo.Cliente;
                ClienteData = clienteInfo.Cliente;

                RucEmpresa = clienteInfo.RucEmpresa;
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task Seleccion()
        {
            await m_navigationService.NavigateBackModalAsync(clienteSeleccionado);
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task CrearClienteAsync()
        {
            await m_navigationService.NavigateToModalAsync<ModalClienteNuevoViewModel>(RucEmpresa);
        }

        private void BuscarAsync(string text)
        {
            if (text != "")
            {
                var clientes = ClienteData.Where(x => x.RazonSocial.ToUpper().Contains(text.ToUpper()));
                var myObservableCollection = new ObservableCollection<ClienteModel>(clientes);
                Cliente = myObservableCollection;
            }
            else
            {
                Cliente = ClienteData;
            }
        }

        #endregion
    }
}
