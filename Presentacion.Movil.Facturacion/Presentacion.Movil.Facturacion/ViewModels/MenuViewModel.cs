using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenuModel> menuItems;

        public bool EsTransportista { get; set; }

        public ObservableCollection<MenuModel> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        private MenuModel selectedMenuItem;

        public MenuModel SelectedMenuItem
        {
            get
            {
                return selectedMenuItem;
            }
            set
            {
                selectedMenuItem = value;
                RaisePropertyChanged(() => SelectedMenuItem);
            }
        }

        private readonly IClienteService m_clienteService;

        public ICommand CerrarSesionCommand => new Command(async () => await CerrarSesionAsync());

        private async Task CerrarSesionAsync()
        {
            ShowLoading();
            await m_navigationService.NavigateToAsync<IniciarSesionViewModel>(new CerrarSesionParametros { CerrarSesion = true });
            HideLoading();
        }

        public MenuViewModel(IClienteService clienteService)
        {
            m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));

            MenuItems = new ObservableCollection<MenuModel>()
            {
                new MenuModel(){Icono = "\xf571;", Opcion =" Facturas"},
                new MenuModel(){Icono = "\xf688;", Opcion ="Consultar Documentos"}
            };

            PropertyChanged += RootPageViewModel_PropertyChanged;
        }

        private void RootPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "SelectedMenuItem" && SelectedMenuItem != null)
            {
                var opcion = SelectedMenuItem.Opcion.Trim();
                if (opcion == "Facturas")
                {
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
                    }
                    else
                    {
                        if (EsTransportista)
                        {
                            m_navigationService.NavigateToAsync<FacturaTransportistaViewModel>();
                        }
                        else
                        {
                            m_navigationService.NavigateToAsync<FacturaViewModel>();
                        }
                    }
                }
                if (opcion == "Consultar Documentos")
                {
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
                    }
                    else
                    {
                        m_navigationService.NavigateToAsync<ConsultaComprobanteFiltroViewModel>();
                    }
                }
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {

            EsTransportista = await m_clienteService.EsTransportista();

            await base.InitializeAsync(navigationData);
        }
    }
}
