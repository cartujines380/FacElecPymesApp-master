using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.PDF;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ConsultaComprobanteViewModel : ViewModelBase
    {

        #region Campos

        private string m_empresa;
        private TipoModel m_tipo;
        private bool m_tipoTodos;
        private bool m_tipoFactura;
        private bool m_tipoFacturaExportacion;
        private bool m_tipoFacturaReembolso;
        private bool m_tipoFacturaTransportista;
        private bool m_tipoNotasCredito;
        private bool m_tipoNotasDebito;
        private bool m_tipoGuiaRemision;
        private bool m_tipoLiquidacionCompra;
        private bool m_tipoComprobantesRetencion;

        private bool m_frameCero;
        private bool m_frameUno;
        private bool m_frameDos;
        private bool m_frameTres;
        private bool m_frameCuatro;
        private bool m_frameCinco;
        private bool m_frameSeis;
        private bool m_frameSiete;
        private bool m_frameOcho;
        private bool m_frameNueve;

        private string m_labelCero;
        private string m_labelUno;
        private string m_labelDos;
        private string m_labelTres;
        private string m_labelCuatro;
        private string m_labelCinco;
        private string m_labelSeis;
        private string m_labelSiete;
        private string m_labelOcho;
        private string m_labelNueve;

        private string m_estado;
        private string m_fecha;
        private string m_numeroComprobante;
        private string m_identificacionCliente;
        private string m_codigoError;
        private bool m_isVisibleNumeroComprobante;
        private bool m_isVisibleIdentificacionCliente;
        private bool m_isVisibleCodigoError;

        private readonly IComprobanteService m_comprobanteService;
        private readonly IEstablecimientoService m_establecimientoService;
        private readonly IClienteService m_clienteService;

        private bool m_isVisibleFrameListComprobante;
        private bool m_isVisibleFrameNotResult;

        private int pageIndex = 0;
        private const int pageSize = 10;

        #endregion

        #region Propiedades

        public FiltroModel Filtro { get; set; }

        public string Empresa
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

        public TipoModel Tipo
        {
            get
            {
                return m_tipo;
            }
            set
            {
                m_tipo = value;
                RaisePropertyChanged(() => Tipo);
            }
        }

        public bool TipoTodos
        {
            get
            {
                return m_tipoTodos;
            }
            set
            {
                m_tipoTodos = value;
                RaisePropertyChanged(() => TipoTodos);
            }
        }

        public bool TipoFactura
        {
            get
            {
                return m_tipoFactura;
            }
            set
            {
                m_tipoFactura = value;
                RaisePropertyChanged(() => TipoFactura);
            }
        }

        public bool TipoFacturaExportacion
        {
            get
            {
                return m_tipoFacturaExportacion;
            }
            set
            {
                m_tipoFacturaExportacion = value;
                RaisePropertyChanged(() => TipoFacturaExportacion);
            }
        }

        public bool TipoFacturaReembolso
        {
            get
            {
                return m_tipoFacturaReembolso;
            }
            set
            {
                m_tipoFacturaReembolso = value;
                RaisePropertyChanged(() => TipoFacturaReembolso);
            }
        }

        public bool TipoFacturaTransportista
        {
            get
            {
                return m_tipoFacturaTransportista;
            }
            set
            {
                m_tipoFacturaTransportista = value;
                RaisePropertyChanged(() => TipoFacturaTransportista);
            }
        }

        public bool TipoNotasCredito
        {
            get
            {
                return m_tipoNotasCredito;
            }
            set
            {
                m_tipoNotasCredito = value;
                RaisePropertyChanged(() => TipoNotasCredito);
            }
        }

        public bool TipoNotasDebito
        {
            get
            {
                return m_tipoNotasDebito;
            }
            set
            {
                m_tipoNotasDebito = value;
                RaisePropertyChanged(() => TipoNotasDebito);
            }
        }

        public bool TipoGuiaRemision
        {
            get
            {
                return m_tipoGuiaRemision;
            }
            set
            {
                m_tipoGuiaRemision = value;
                RaisePropertyChanged(() => TipoGuiaRemision);
            }
        }

        public bool TipoLiquidacionCompra
        {
            get
            {
                return m_tipoLiquidacionCompra;
            }
            set
            {
                m_tipoLiquidacionCompra = value;
                RaisePropertyChanged(() => TipoLiquidacionCompra);
            }
        }

        public bool TipoComprobantesRetencion
        {
            get
            {
                return m_tipoComprobantesRetencion;
            }
            set
            {
                m_tipoComprobantesRetencion = value;
                RaisePropertyChanged(() => TipoComprobantesRetencion);
            }
        }

        public bool FrameCero
        {
            get
            {
                return m_frameCero;
            }
            set
            {
                m_frameCero = value;
                RaisePropertyChanged(() => FrameCero);
            }
        }

        public bool FrameUno
        {
            get
            {
                return m_frameUno;
            }
            set
            {
                m_frameUno = value;
                RaisePropertyChanged(() => FrameUno);
            }
        }

        public bool FrameDos
        {
            get
            {
                return m_frameDos;
            }
            set
            {
                m_frameDos = value;
                RaisePropertyChanged(() => FrameDos);
            }
        }

        public bool FrameTres
        {
            get
            {
                return m_frameTres;
            }
            set
            {
                m_frameTres = value;
                RaisePropertyChanged(() => FrameTres);
            }
        }

        public bool FrameCuatro
        {
            get
            {
                return m_frameCuatro;
            }
            set
            {
                m_frameCuatro = value;
                RaisePropertyChanged(() => FrameCuatro);
            }
        }

        public bool FrameCinco
        {
            get
            {
                return m_frameCinco;
            }
            set
            {
                m_frameCinco = value;
                RaisePropertyChanged(() => FrameCinco);
            }
        }

        public bool FrameSeis
        {
            get
            {
                return m_frameSeis;
            }
            set
            {
                m_frameSeis = value;
                RaisePropertyChanged(() => FrameSeis);
            }
        }

        public bool FrameSiete
        {
            get
            {
                return m_frameSiete;
            }
            set
            {
                m_frameSiete = value;
                RaisePropertyChanged(() => FrameSiete);
            }
        }

        public bool FrameOcho
        {
            get
            {
                return m_frameOcho;
            }
            set
            {
                m_frameOcho = value;
                RaisePropertyChanged(() => FrameOcho);
            }
        }

        public bool FrameNueve
        {
            get
            {
                return m_frameNueve;
            }
            set
            {
                m_frameNueve = value;
                RaisePropertyChanged(() => FrameNueve);
            }
        }

        public string LabelCero
        {
            get
            {
                return m_labelCero;
            }
            set
            {
                m_labelCero = value;
                RaisePropertyChanged(() => LabelCero);
            }
        }

        public string LabelUno
        {
            get
            {
                return m_labelUno;
            }
            set
            {
                m_labelUno = value;
                RaisePropertyChanged(() => LabelUno);
            }
        }

        public string LabelDos
        {
            get
            {
                return m_labelDos;
            }
            set
            {
                m_labelDos = value;
                RaisePropertyChanged(() => LabelDos);
            }
        }

        public string LabelTres
        {
            get
            {
                return m_labelTres;
            }
            set
            {
                m_labelTres = value;
                RaisePropertyChanged(() => LabelTres);
            }
        }

        public string LabelCuatro
        {
            get
            {
                return m_labelCuatro;
            }
            set
            {
                m_labelCuatro = value;
                RaisePropertyChanged(() => LabelCuatro);
            }
        }

        public string LabelCinco
        {
            get
            {
                return m_labelCinco;
            }
            set
            {
                m_labelCinco = value;
                RaisePropertyChanged(() => LabelCinco);
            }
        }

        public string LabelSeis
        {
            get
            {
                return m_labelSeis;
            }
            set
            {
                m_labelSeis = value;
                RaisePropertyChanged(() => LabelSeis);
            }
        }

        public string LabelSiete
        {
            get
            {
                return m_labelSiete;
            }
            set
            {
                m_labelSiete = value;
                RaisePropertyChanged(() => LabelSiete);
            }
        }

        public string LabelOcho
        {
            get
            {
                return m_labelOcho;
            }
            set
            {
                m_labelOcho = value;
                RaisePropertyChanged(() => LabelOcho);
            }
        }

        public string LabelNueve
        {
            get
            {
                return m_labelNueve;
            }
            set
            {
                m_labelNueve = value;
                RaisePropertyChanged(() => LabelNueve);
            }
        }

        public string Fecha
        {
            get
            {
                return m_fecha;
            }
            set
            {
                m_fecha = value;
                RaisePropertyChanged(() => Fecha);
            }
        }

        public string Estado
        {
            get
            {
                return m_estado;
            }
            set
            {
                m_estado = value;
                RaisePropertyChanged(() => Estado);
            }
        }

        public string NumeroComprobante
        {
            get
            {
                return m_numeroComprobante;
            }
            set
            {
                m_numeroComprobante = value;
                RaisePropertyChanged(() => NumeroComprobante);
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

        public bool IsVisibleNumeroComprobante
        {
            get
            {
                return m_isVisibleNumeroComprobante;
            }
            set
            {
                m_isVisibleNumeroComprobante = value;
                RaisePropertyChanged(() => IsVisibleNumeroComprobante);
            }
        }

        public bool IsVisibleIdentificacionCliente
        {
            get
            {
                return m_isVisibleIdentificacionCliente;
            }
            set
            {
                m_isVisibleIdentificacionCliente = value;
                RaisePropertyChanged(() => IsVisibleIdentificacionCliente);
            }
        }

        public bool IsVisibleCodigoError
        {
            get
            {
                return m_isVisibleCodigoError;
            }
            set
            {
                m_isVisibleCodigoError = value;
                RaisePropertyChanged(() => IsVisibleCodigoError);
            }
        }

        private ObservableCollection<InfoComprobanteModel> m_infoComprobante;

        public ObservableCollection<InfoComprobanteModel> InfoComprobante
        {
            get
            {
                return m_infoComprobante;
            }
            set
            {
                m_infoComprobante = value;
                RaisePropertyChanged(() => InfoComprobante);
            }
        }

        public bool IsVisibleFrameListComprobante
        {
            get
            {
                return m_isVisibleFrameListComprobante;
            }
            set
            {
                m_isVisibleFrameListComprobante = value;
                RaisePropertyChanged(() => IsVisibleFrameListComprobante);
            }
        }

        public bool IsVisibleFrameNotResult
        {
            get
            {
                return m_isVisibleFrameNotResult;
            }
            set
            {
                m_isVisibleFrameNotResult = value;
                RaisePropertyChanged(() => IsVisibleFrameNotResult);
            }
        }




        #endregion

        #region Command

        public ICommand MostarMasFiltrosCommand => new Command(async () => await MostrarMasFiltrosAsync());
      
        public ICommand DescargarPDFCommand => new Command<InfoComprobanteModel>(async (infoComp) => await DescargarPDFAsync(infoComp));

        public ICommand CompartirPDFCommand => new Command<InfoComprobanteModel>(async (infoComp) => await CompartirPDFAsync(infoComp));

        private readonly IPDFService m_pdfService;

        #endregion

        #region Constructor

        public ConsultaComprobanteViewModel(
            IComprobanteService comprobanteService, 
            IPDFService pdfService,
            IEstablecimientoService establecimientoService,
            IClienteService clienteService)
        {
            try
            {
                m_comprobanteService = comprobanteService ?? throw new ArgumentNullException(nameof(comprobanteService));
                m_establecimientoService = establecimientoService ?? throw new ArgumentNullException(nameof(establecimientoService));
                m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
                Filtro = new FiltroModel();
                InfoComprobante = new ObservableCollection<InfoComprobanteModel>();
                m_pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
            }
            catch (Exception ex)
            {
            }
        }

        
        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is FiltroModel filtro)
            {
                Estado = filtro.Estado;
                Fecha = ConcatenarFecha(filtro);
                Empresa = filtro.Establecimiento.RazonSocial;
                List<string> listTipo = filtro.ListTipo;
                MostrarTipo(listTipo);
                NumeroComprobante = filtro.NumeroComprobante;
                IsVisibleNumeroComprobante = filtro.NumeroComprobante != null && filtro.NumeroComprobante != "";
                IdentificacionCliente = filtro.IdentificacionCliente;
                IsVisibleIdentificacionCliente = filtro.IdentificacionCliente != null && filtro.IdentificacionCliente != "";
                CodigoError = filtro.CodigoError;
                IsVisibleCodigoError = filtro.CodigoError != null && filtro.CodigoError != "";
                
                Filtro = filtro;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
                }
                else
                {
                    ShowLoading();
                    await ConsultarComprobanteInterno();
                    HideLoading();
                }
            }

            await base.InitializeAsync(navigationData);
        }


        public async Task ConsultarComprobanteInterno()
        {
            Filtro.PageIndex = pageIndex;
            Filtro.PageSize = pageSize;
            var comprobantes = await m_comprobanteService.ConsultarComprobante(Filtro);

            foreach (var item in comprobantes)
            {
                InfoComprobante.Add(new InfoComprobanteModel
                {
                    IdDocumento = item.IdDocumento,
                    Ruc = item.Ruc,
                    Establecimiento = item.Establecimiento,
                    TipoBase = item.TipoBase,
                    TipoComprobante = item.TipoDoc,
                    NumeroComprobante = item.Numero,
                    Identificacion = item.IdentificacionCliente,
                    Fecha = item.FechaEmision,
                    Valor = "$" + item.MontoFacturado.ToString()
                });
            }

            pageIndex++;

            if (InfoComprobante != null && InfoComprobante.Count > 0)
            {
                IsVisibleFrameListComprobante = true;
                IsVisibleFrameNotResult = false;
            }
            else
            {
                IsVisibleFrameListComprobante = false;
                IsVisibleFrameNotResult = true;
            }
        }

        private string ConcatenarFecha(FiltroModel filtro)
        {
            string fecha = string.Empty;
            if (filtro != null)
            {
                string fechaInicio = filtro.FechaInicio.ToString("dd/MM/yyyy");
                string fechaFin = filtro.FechaFin.ToString("dd/MM/yyyy");

                fecha = fechaInicio + " - " + fechaFin;
            }

            return fecha;
        }

        private void MostrarTipo(List<string> listTipo)
        {
            int x = 0;
            foreach (string item in listTipo)
            {
                switch (x)
                {
                    case 0:
                        FrameCero = true;
                        LabelCero = item;
                        break;
                    case 1:
                        FrameUno = true;
                        LabelUno = item;
                        break;
                    case 2:
                        FrameDos = true;
                        LabelDos = item;
                        break;
                    case 3:
                        FrameTres = true;
                        LabelTres = item;
                        break;
                    case 4:
                        FrameCuatro = true;
                        LabelCuatro = item;
                        break;
                    case 5:
                        FrameCinco = true;
                        LabelCinco = item;
                        break;
                    case 6:
                        FrameSeis = true;
                        LabelSeis = item;
                        break;
                    case 7:
                        FrameSiete = true;
                        LabelSiete = item;
                        break;
                    case 8:
                        FrameOcho = true;
                        LabelOcho = item;
                        break;
                    case 9:
                        FrameNueve = true;
                        LabelNueve = item;
                        break;
                    default:
                        break;
                }

                x++;
            }
        }

        private async Task MostrarMasFiltrosAsync()
        {
            await m_navigationService.NavigateBackAsync();
        }


        private async Task DescargarPDFAsync(InfoComprobanteModel infoComp)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                var infoDoc = new ComprobanteModel();
                infoDoc = await m_comprobanteService.ConsultarComprobanteXML(infoComp);
                var dirSucursal = await m_establecimientoService.ObtenerDireccionSucursal(infoComp);
                var dirCliente = await m_clienteService.ObtenerDireccionCliente(infoComp);
                infoDoc.DirSucursal = dirSucursal;
                infoDoc.DirCliente = dirCliente;

                var pdf = await m_pdfService.GenerarPDF(infoDoc);

                if (pdf.PDFbtye != null)
                {
                    string fileName = infoComp.NumeroComprobante + ".pdf";
                    await SavePdf(pdf.PDFbtye, fileName);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al descargar RIDE", "Aceptar");
                }

                HideLoading();
            }
        }

        private async Task CompartirPDFAsync(InfoComprobanteModel infoComp)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                var infoDoc = new ComprobanteModel();
                infoDoc = await m_comprobanteService.ConsultarComprobanteXML(infoComp);
                var dirSucursal = await m_establecimientoService.ObtenerDireccionSucursal(infoComp);
                var dirCliente = await m_clienteService.ObtenerDireccionCliente(infoComp);
                infoDoc.DirSucursal = dirSucursal;
                infoDoc.DirCliente = dirCliente;

                var pdf = await m_pdfService.GenerarPDF(infoDoc);

                if (pdf.PDFbtye != null)
                {
                    string fileName = infoComp.NumeroComprobante + ".pdf";
                    await SharePdf(pdf.PDFbtye, fileName);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al compartir RIDE", "Aceptar");
                }

                HideLoading();
            }
        }

        public async Task SavePdf(byte[] pdfBytes, string fileName)
        {
            try
            {
                string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                File.WriteAllBytes(filePath, pdfBytes);
                await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(filePath) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task SharePdf(byte[] pdfBytes, string fileName)
        {
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllBytes(filePath, pdfBytes);

            var file = Path.Combine(FileSystem.CacheDirectory, fileName);
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "RIDE",
                File = new ShareFile(file)
            });
        }

        #endregion


    }
}
