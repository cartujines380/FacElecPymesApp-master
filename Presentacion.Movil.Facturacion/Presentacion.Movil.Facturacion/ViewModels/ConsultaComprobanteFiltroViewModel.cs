using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.PDF;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ConsultaComprobanteFiltroViewModel : ViewModelBase
    {
        #region Constantes

        private const string colorActivo = "#0F75BC";
        private const string colorLila = "#333B8E";
        private const string colorBlanco = "#FFFFFF";
        private const string estadoAU = "AU";
        private const string estadoAN = "AN";
        private const string estadoNoAU_PreAU = "FI|VA|NP|E1|E2|ER|PE";

        #endregion

        #region Campos

        private string m_backgroundColorTodos;
        private string m_textColorTodos;
        private string m_borderColorTodos;

        private string m_backgroundColorAutorizados;
        private string m_textColorAutorizados;
        private string m_borderColorAutorizados;

        private string m_backgroundColorNoAutorizados;
        private string m_textColorNoAutorizados;
        private string m_borderColorNoAutorizados;

        private string m_backgroundColorPreAutorizados;
        private string m_textColorPreAutorizados;
        private string m_borderColorPreAutorizados;

        private string m_backgroundColorAnulados;
        private string m_textColorAnulados;
        private string m_borderColorAnulados;

        private readonly IEstablecimientoService m_establecimientoService;
        private readonly IPDFService m_pdfService;
        private EstablecimientoModel m_empresa;

        private bool m_isCheckedTodos;
        private bool m_checkFactura;
        private bool m_checkFacturaExportacion;
        private bool m_checkFacturaReembolso;
        private bool m_checkFacturaTransportista;
        private bool m_checkNotaCredito;
        private bool m_checkNotaDebito;
        private bool m_checkGuiaRemision;
        private bool m_checkLiquidacionCompra;
        private bool m_checkComprobanteRetencion;

        private DateTime m_fechaInicio;
        private DateTime m_fechaFin;

        private string m_numComprobante;
        private string m_identificacionCliente;
        private string m_codigoError;

        #endregion

        #region Propiedades

        public string Estado { get; set; }

        public string CodigoEstado { get; set; }

        public string BackgroundColorTodos
        {
            get
            {
                return m_backgroundColorTodos;
            }
            set
            {
                m_backgroundColorTodos = value;
                RaisePropertyChanged(() => BackgroundColorTodos);
            }
        }

        public string TextColorTodos
        {
            get
            {
                return m_textColorTodos;
            }
            set
            {
                m_textColorTodos = value;
                RaisePropertyChanged(() => TextColorTodos);
            }
        }

        public string BorderColorTodos
        {
            get
            {
                return m_borderColorTodos;
            }
            set
            {
                m_borderColorTodos = value;
                RaisePropertyChanged(() => BorderColorTodos);
            }
        }

        public string BackgroundColorAutorizados
        {
            get
            {
                return m_backgroundColorAutorizados;
            }
            set
            {
                m_backgroundColorAutorizados = value;
                RaisePropertyChanged(() => BackgroundColorAutorizados);
            }
        }

        public string TextColorAutorizados
        {
            get
            {
                return m_textColorAutorizados;
            }
            set
            {
                m_textColorAutorizados = value;
                RaisePropertyChanged(() => TextColorAutorizados);
            }
        }

        public string BorderColorAutorizados
        {
            get
            {
                return m_borderColorAutorizados;
            }
            set
            {
                m_borderColorAutorizados = value;
                RaisePropertyChanged(() => BorderColorAutorizados);
            }
        }

        public string BackgroundColorNoAutorizados
        {
            get
            {
                return m_backgroundColorNoAutorizados;
            }
            set
            {
                m_backgroundColorNoAutorizados = value;
                RaisePropertyChanged(() => BackgroundColorNoAutorizados);
            }
        }

        public string TextColorNoAutorizados
        {
            get
            {
                return m_textColorNoAutorizados;
            }
            set
            {
                m_textColorNoAutorizados = value;
                RaisePropertyChanged(() => TextColorNoAutorizados);
            }
        }

        public string BorderColorNoAutorizados
        {
            get
            {
                return m_borderColorNoAutorizados;
            }
            set
            {
                m_borderColorNoAutorizados = value;
                RaisePropertyChanged(() => BorderColorNoAutorizados);
            }
        }

        public string BackgroundColorPreAutorizados
        {
            get
            {
                return m_backgroundColorPreAutorizados;
            }
            set
            {
                m_backgroundColorPreAutorizados = value;
                RaisePropertyChanged(() => BackgroundColorPreAutorizados);
            }
        }

        public string TextColorPreAutorizados
        {
            get
            {
                return m_textColorPreAutorizados;
            }
            set
            {
                m_textColorPreAutorizados = value;
                RaisePropertyChanged(() => TextColorPreAutorizados);
            }
        }

        public string BorderColorPreAutorizados
        {
            get
            {
                return m_borderColorPreAutorizados;
            }
            set
            {
                m_borderColorPreAutorizados = value;
                RaisePropertyChanged(() => BorderColorPreAutorizados);
            }
        }

        public string BackgroundColorAnulados
        {
            get
            {
                return m_backgroundColorAnulados;
            }
            set
            {
                m_backgroundColorAnulados = value;
                RaisePropertyChanged(() => BackgroundColorAnulados);
            }
        }

        public string TextColorAnulados
        {
            get
            {
                return m_textColorAnulados;
            }
            set
            {
                m_textColorAnulados = value;
                RaisePropertyChanged(() => TextColorAnulados);
            }
        }

        public string BorderColorAnulados
        {
            get
            {
                return m_borderColorAnulados;
            }
            set
            {
                m_borderColorAnulados = value;
                RaisePropertyChanged(() => BorderColorAnulados);
            }
        }

        public EstablecimientoModel Empresa
        {
            get
            {
                return m_empresa;
            }
            set
            {
                m_empresa = value;
                RaisePropertyChanged(() => Empresa);
            }
        }

        public bool IsCheckedTodos
        {
            get
            {
                return m_isCheckedTodos;
            }
            set
            {
                m_isCheckedTodos = value;
                RaisePropertyChanged(() => IsCheckedTodos);
            }
        }

        public bool CheckFactura
        {
            get
            {
                return m_checkFactura;
            }
            set
            {
                m_checkFactura = value;
                RaisePropertyChanged(() => CheckFactura);
            }
        }

        public bool CheckFacturaExportacion
        {
            get
            {
                return m_checkFacturaExportacion;
            }
            set
            {
                m_checkFacturaExportacion = value;
                RaisePropertyChanged(() => CheckFacturaExportacion);
            }
        }

        public bool CheckFacturaReembolso
        {
            get
            {
                return m_checkFacturaReembolso;
            }
            set
            {
                m_checkFacturaReembolso = value;
                RaisePropertyChanged(() => CheckFacturaReembolso);
            }
        }

        public bool CheckFacturaTransportista
        {
            get
            {
                return m_checkFacturaTransportista;
            }
            set
            {
                m_checkFacturaTransportista = value;
                RaisePropertyChanged(() => CheckFacturaTransportista);
            }
        }

        public bool CheckNotaCredito
        {
            get
            {
                return m_checkNotaCredito;
            }
            set
            {
                m_checkNotaCredito = value;
                RaisePropertyChanged(() => CheckNotaCredito);
            }
        }

        public bool CheckNotaDebito
        {
            get
            {
                return m_checkNotaDebito;
            }
            set
            {
                m_checkNotaDebito = value;
                RaisePropertyChanged(() => CheckNotaDebito);
            }
        }

        public bool CheckGuiaRemision
        {
            get
            {
                return m_checkGuiaRemision;
            }
            set
            {
                m_checkGuiaRemision = value;
                RaisePropertyChanged(() => CheckGuiaRemision);
            }
        }

        public bool CheckLiquidacionCompra
        {
            get
            {
                return m_checkLiquidacionCompra;
            }
            set
            {
                m_checkLiquidacionCompra = value;
                RaisePropertyChanged(() => CheckLiquidacionCompra);
            }
        }

        public bool CheckComprobanteRetencion
        {
            get
            {
                return m_checkComprobanteRetencion;
            }
            set
            {
                m_checkComprobanteRetencion = value;
                RaisePropertyChanged(() => CheckComprobanteRetencion);
            }
        }

        public DateTime FechaInicio
        {
            get
            {
                return m_fechaInicio;
            }
            set
            {
                m_fechaInicio = value;
                RaisePropertyChanged(() => FechaInicio);
            }
        }

        public DateTime FechaFin
        {
            get
            {
                return m_fechaFin;
            }
            set
            {
                m_fechaFin = value;
                RaisePropertyChanged(() => FechaFin);
            }
        }

        public string NumComprobante
        {
            get
            {
                return m_numComprobante;
            }
            set
            {
                m_numComprobante = value;
                RaisePropertyChanged(() => NumComprobante);
            }
        }

        public string IdentificacionCliente
        {
            get
            {
                return m_identificacionCliente;
            }
            set
            {
                m_identificacionCliente = value;
                RaisePropertyChanged(() => IdentificacionCliente);
            }
        }

        public string CodigoError
        {
            get
            {
                return m_codigoError;
            }
            set
            {
                m_codigoError = value;
                RaisePropertyChanged(() => CodigoError);
            }
        }

        #endregion

        #region Constructor

        public ConsultaComprobanteFiltroViewModel(IEstablecimientoService establecimientoService, IPDFService pdfService)
        {
            m_establecimientoService = establecimientoService ?? throw new ArgumentNullException(nameof(establecimientoService));
            m_pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));

            FechaInicio = DateTime.Now;
            FechaFin = DateTime.Now;
        }

        #endregion

        #region Command

        public ICommand ActivarBtnEstadoComprobanteCommand => new Command<string>(ActivarBtnEstadoComprobanteAsync);

        public ICommand ConsultarComprobantesCommand => new Command(async () => await ConsultarComprobantesAsync());

        public ICommand MostrarModalEmpresaCommand => new Command(async () => await ModalEmpresaAsync());

        public ICommand ActivarCheckCommand => new Command<string>(ActivarCheckCommandAsync);

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            ActivarBtnTodos();
            DesactivarBtnAutorizados();
            DesactivarBtnNoAutorizados();
            DesactivarBtnPreAutorizados();
            DesactivarBtnAnulados();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                var establecimientos = await m_establecimientoService.ObtenerEstablecimientoCmb();
                HideLoading();

                var empresa = establecimientos.FirstOrDefault();
                Empresa = empresa;
            }

            await base.InitializeAsync(navigationData);
        }

        public override async Task OnPopModalAsync(object navigationData, string nombrePage)
        {
            if (navigationData != null)
            {
                switch (nombrePage)
                {
                    case "ModalEmpresaView":
                        EstablecimientoModel establecimiento = navigationData as EstablecimientoModel;
                        Empresa = establecimiento;
                        break;
                    default:
                        break;
                }
            }

            await base.OnPopModalAsync(navigationData, nombrePage);
        }

        private async Task ConsultarComprobantesAsync()
        {
            List<string> listTipos = GuardarTiposSeleccionados();

            FiltroModel obj = new FiltroModel
            {
                CodigoError = CodigoError,
                Estado = Estado,
                CodigoEstado = CodigoEstado,
                Establecimiento = Empresa,
                ListTipo = listTipos,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                IdentificacionCliente = IdentificacionCliente,
                NumeroComprobante = NumComprobante
            };

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                await m_navigationService.NavigateToAsync<ConsultaComprobanteViewModel>(obj);
            }


        }

        private List<string> GuardarTiposSeleccionados()
        {
            List<string> listTipos = new List<string>();

            if (IsCheckedTodos)
            {
                listTipos.Add("Todos");
                return listTipos;
            }

            if (CheckComprobanteRetencion)
            {
                listTipos.Add("Comprobantes de Retención");
            }

            if (CheckFactura)
            {
                listTipos.Add("Factura");
            }

            if (CheckFacturaExportacion)
            {
                listTipos.Add("Factura Exportación");
            }

            if (CheckFacturaReembolso)
            {
                listTipos.Add("Factura Reembolso");
            }

            if (CheckFacturaTransportista)
            {
                listTipos.Add("Factura Transportista");
            }

            if (CheckGuiaRemision)
            {
                listTipos.Add("Guía de Remisión");
            }

            if (CheckLiquidacionCompra)
            {
                listTipos.Add("Liquidación de Compra");
            }

            if (CheckNotaCredito)
            {
                listTipos.Add("Nota de Crédito");
            }

            if (CheckNotaDebito)
            {
                listTipos.Add("Nota de Débito");
            }

            return listTipos;
        }

        private async Task ModalEmpresaAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                var establecimientos = await m_establecimientoService.ObtenerEstablecimientoCmb();
                HideLoading();
                await m_navigationService.NavigateToModalAsync<ModalEmpresaViewModel>(establecimientos);
            }
               
        }

        private void ActivarBtnEstadoComprobanteAsync(string btnEstado)
        {
            switch (btnEstado)
            {
                case "btnTodos":
                    ActivarBtnTodos();
                    DesactivarBtnAutorizados();
                    DesactivarBtnNoAutorizados();
                    DesactivarBtnPreAutorizados();
                    DesactivarBtnAnulados();
                    break;
                case "btnAutorizados":
                    ActivarBtnAutorizados();
                    DesactivarBtnTodos();
                    DesactivarBtnNoAutorizados();
                    DesactivarBtnPreAutorizados();
                    DesactivarBtnAnulados();
                    break;
                case "btnNoAutorizados":
                    ActivarBtnNoAutorizados();
                    DesactivarBtnTodos();
                    DesactivarBtnAutorizados();
                    DesactivarBtnPreAutorizados();
                    DesactivarBtnAnulados();
                    break;
                case "btnPreAutorizados":
                    ActivarBtnPreAutorizados();
                    DesactivarBtnTodos();
                    DesactivarBtnAutorizados();
                    DesactivarBtnNoAutorizados();
                    DesactivarBtnAnulados();
                    break;
                case "btnAnulados":
                    ActivarBtnAnulados();
                    DesactivarBtnTodos();
                    DesactivarBtnAutorizados();
                    DesactivarBtnNoAutorizados();
                    DesactivarBtnPreAutorizados();
                    break;
                default:
                    break;
            }
        }

        private void ActivarBtnTodos()
        {
            BackgroundColorTodos = colorActivo;
            BorderColorTodos = colorActivo;
            TextColorTodos = colorBlanco;
            Estado = "Todos";
            CodigoEstado = estadoAU + "|" + estadoNoAU_PreAU + "|" + estadoAN;
        }

        private void DesactivarBtnTodos()
        {
            BackgroundColorTodos = colorBlanco;
            BorderColorTodos = colorActivo;
            TextColorTodos = colorLila;
        }

        private void ActivarBtnAutorizados()
        {
            BackgroundColorAutorizados = colorActivo;
            BorderColorTodos = colorActivo;
            TextColorAutorizados = colorBlanco;
            Estado = "Autorizados";
            CodigoEstado = estadoAU;
        }

        private void DesactivarBtnAutorizados()
        {
            BackgroundColorAutorizados = colorBlanco;
            BorderColorAutorizados = colorActivo;
            TextColorAutorizados = colorLila;
        }

        private void ActivarBtnNoAutorizados()
        {
            BackgroundColorNoAutorizados = colorActivo;
            BorderColorNoAutorizados = colorActivo;
            TextColorNoAutorizados = colorBlanco;
            Estado = "No Autorizados";
            CodigoEstado = estadoNoAU_PreAU;
        }

        private void DesactivarBtnNoAutorizados()
        {
            BackgroundColorNoAutorizados = colorBlanco;
            BorderColorNoAutorizados = colorActivo;
            TextColorNoAutorizados = colorLila;
        }

        private void ActivarBtnPreAutorizados()
        {
            BackgroundColorPreAutorizados = colorActivo;
            BorderColorPreAutorizados = colorActivo;
            TextColorPreAutorizados = colorBlanco;
            Estado = "Pre Autorizados";
            CodigoEstado = estadoNoAU_PreAU;
        }

        private void DesactivarBtnPreAutorizados()
        {
            BackgroundColorPreAutorizados = colorBlanco;
            BorderColorPreAutorizados = colorActivo;
            TextColorPreAutorizados = colorLila;
        }

        private void ActivarBtnAnulados()
        {
            BackgroundColorAnulados = colorActivo;
            BorderColorAnulados = colorActivo;
            TextColorAnulados = colorBlanco;
            Estado = "Anulados";
            CodigoEstado = estadoAN;
        }

        private void DesactivarBtnAnulados()
        {
            BackgroundColorAnulados = colorBlanco;
            BorderColorAnulados = colorActivo;
            TextColorAnulados = colorLila;
        }

        private void ActivarCheckCommandAsync(string obj)
        {
            switch (obj)
            {
                case "Todo":
                    if (IsCheckedTodos)
                    {
                        ActivarTodos();
                    }
                    else
                    {
                        DesactivarTodos();
                    }

                    break;
                default:
                    break;
            }
        }

        private void DesactivarTodos()
        {
            IsCheckedTodos = false;
            CheckFactura = false;
            CheckFacturaExportacion = false;
            CheckFacturaReembolso = false;
            CheckFacturaTransportista = false;
            CheckNotaCredito = false;
            CheckNotaDebito = false;
            CheckGuiaRemision = false;
            CheckLiquidacionCompra = false;
            CheckComprobanteRetencion = false;
        }

        private void ActivarTodos()
        {
            IsCheckedTodos = true;
            CheckFactura = true;
            CheckFacturaExportacion = true;
            CheckFacturaReembolso = true;
            CheckFacturaTransportista = true;
            CheckNotaCredito = true;
            CheckNotaDebito = true;
            CheckGuiaRemision = true;
            CheckLiquidacionCompra = true;
            CheckComprobanteRetencion = true;
        }

        #endregion

    }
}