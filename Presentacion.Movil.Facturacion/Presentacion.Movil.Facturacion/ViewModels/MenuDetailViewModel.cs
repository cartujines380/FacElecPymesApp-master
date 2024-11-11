using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.User;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class MenuDetailViewModel : ViewModelBase
    {
        #region Campos

        private string m_planContratado;
        private string m_nombreUsuario;
        private string m_documentosContratados;
        private string m_documentosFaltantes;
        private decimal m_porcentaje;
        private string m_facturaAU;
        private string m_facturaAN;
        private string m_facturaER;
        private readonly IFacturaService m_facturaServices;
        private readonly IEstablecimientoService m_establecimientoService;
        private readonly IClienteService m_clienteService;

        private readonly IUserService m_userService;
        private readonly ISettingsService m_settingsService;

        #endregion

        #region Propiedades

        public string NombreUsuario
        {
            get
            {
                return m_nombreUsuario;
            }
            set
            {
                m_nombreUsuario = value;
                RaisePropertyChanged(() => NombreUsuario);
            }
        }

        public string PlanContratado
        {
            get
            {
                return m_planContratado;
            }
            set
            {
                m_planContratado = value;
                RaisePropertyChanged(() => PlanContratado);
            }
        }

        public string DocumentosContratados
        {
            get
            {
                return m_documentosContratados;
            }
            set
            {
                m_documentosContratados = value;
                RaisePropertyChanged(() => DocumentosContratados);
            }
        }

        public string DocumentosFaltantes
        {
            get
            {
                return m_documentosFaltantes;
            }
            set
            {
                m_documentosFaltantes = value;
                RaisePropertyChanged(() => DocumentosFaltantes);
            }
        }

        public decimal Porcentaje
        {
            get
            {
                return m_porcentaje;
            }
            set
            {
                m_porcentaje = value;
                RaisePropertyChanged(() => Porcentaje);
            }
        }


        public string FacturaAU
        {
            get
            {
                return m_facturaAU;
            }
            set
            {
                m_facturaAU = value;
                RaisePropertyChanged(() => FacturaAU);
            }
        }

        public string FacturaAN
        {
            get
            {
                return m_facturaAN;
            }
            set
            {
                m_facturaAN = value;
                RaisePropertyChanged(() => FacturaAN);
            }
        }

        public string FacturaER
        {
            get
            {
                return m_facturaER;
            }
            set
            {
                m_facturaER = value;
                RaisePropertyChanged(() => FacturaER);
            }
        }

        #endregion

        #region Command

        public ICommand MostrarFacturaCommand => new Command(async () => await MostrarFacturaAsync());

        public ICommand MostrarConsultaComprobanteCommand => new Command(async () => await MostrarConsultaComprobanteCommandAsync());

        #endregion

        #region Constructor

        public MenuDetailViewModel(
            IEstablecimientoService establecimientoService, 
            IFacturaService facturaServices, 
            IUserService userService,
            ISettingsService settingsService,
            IClienteService clienteService)
        {
            m_establecimientoService = establecimientoService ?? throw new ArgumentNullException(nameof(establecimientoService));
            m_facturaServices = facturaServices ?? throw new ArgumentNullException(nameof(facturaServices));
            m_userService = userService ?? throw new ArgumentNullException(nameof(userService));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));

        }

        #endregion

        private async Task MostrarFacturaAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();

                bool esTransportista = await m_clienteService.EsTransportista();
                if (esTransportista)
                {
                    await m_navigationService.NavigateToAsync<FacturaTransportistaViewModel>();
                }
                else
                {
                    await m_navigationService.NavigateToAsync<FacturaViewModel>();
                }

                HideLoading();
            }
               
        }

        private async Task MostrarConsultaComprobanteCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                await m_navigationService.NavigateToAsync<ConsultaComprobanteFiltroViewModel>();
                HideLoading();
            }

        }

        private async Task Obtener()
        {
            try
            {
                var userInfo = await m_userService.GetUserInfoAsync(m_settingsService.AuthAccessToken);
                NombreUsuario = userInfo.UniqueName;
                var establecimiento = await m_establecimientoService.ObtenerEstablecimiento();
                var ruc = establecimiento != null ? establecimiento.Ruc : "";

                var plan = await m_establecimientoService.ObtenerPlan(ruc);

                var facturaT = await m_facturaServices.ObtenerTotalComprobante(ruc, "AU");
                var facturaAU = await m_facturaServices.ObtenerTotalComprobantePorTipo(ruc, "AU", "01");
                var facturaAN = await m_facturaServices.ObtenerTotalComprobantePorTipo(ruc, "AN", "01");
                var facturaER = await m_facturaServices.ObtenerTotalComprobantePorTipo(ruc, "E1", "01");

                var cantidadDocHasta = Convert.ToInt32(plan.CantidadDocHasta);
                var docfaltanttes = cantidadDocHasta - facturaT;
                if (cantidadDocHasta != 0)
                {
                    var _porcentaje = facturaT * 100 / cantidadDocHasta;
                    Porcentaje = ((decimal)_porcentaje) / 100M;
                }

                PlanContratado = plan.NombrePlan;
                DocumentosContratados = plan.CantidadDocHasta + " Documentos Contratados";
                DocumentosFaltantes = docfaltanttes.ToString() + " Documentos Faltantes";
                FacturaAU = facturaAU.ToString();
                FacturaAN = facturaAN.ToString();
                FacturaER = facturaER.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await Obtener();

            await base.InitializeAsync(navigationData);
        }

        public override async Task OnPopModalAsync(object navigationData, string nombrePage)
        {
            //IsVisible = false;

            await base.OnPopModalAsync(navigationData, nombrePage);
        }
    }
}
