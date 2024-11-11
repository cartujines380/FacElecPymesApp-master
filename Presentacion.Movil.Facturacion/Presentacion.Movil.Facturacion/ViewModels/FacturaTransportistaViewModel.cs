using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class FacturaTransportistaViewModel : ViewModelBase
    {
        #region Constantes

        public const string IMPUESTO_CERO_PORCIENTO = "0";
        public const string IMPUESTO_DOCE_PORCIENTO = "2";
        public const string IMPUESTO_CATORCE_PORCIENTO = "3";
        public const string IMPUESTO_NO_OBJETO_IVA = "6";
        public const string IMPUESTO_EXCENTO_IVA = "7";
        public const string IMPUESTO_NO_GRABA_ICE = "8";

        #endregion

        #region Campos

        private string m_valorUno;
        private string m_valorDos;
        private string m_valorTres;
        private string m_valorCuatro;
        private string m_guiaRemision;
        private string m_descripcion;
        private string m_TipoFacturaDetalle;
        private string m_numeroFactura;
        private string m_valorTotal;
        private string m_subtotalDocePorciento;
        private string m_ivaCalculado;
        private string m_tituloComplemetario;
        private readonly ICatalogoService m_catalogoService;
        private readonly IEstablecimientoService m_establecimientoService;
        private readonly IClienteService m_clienteService;
        private readonly IFacturaService m_facturaService;
        private readonly IArticuloService m_articuloService;
        private EstablecimientoModel m_empresa;
        private ClienteModel m_cliente;
        private DateTime m_fechaEmision;
        private FormaPagoModel m_formaPago;
        private ObservableCollection<FacturaDetalleModel> m_detalleInfo;
        private FacturaDetalleModel m_detalleSeleccionado;
        private ObservableCollection<InformacionAdicional> m_infoAdicional;
        private InformacionAdicional m_infoAdicionalSeleccionado;

        #endregion

        #region Propiedades

        public string SubtotalNoObjetoIva { get; set; }

        public string SubtotalExentoIva { get; set; }

        public string SubtotalCeroPorciento { get; set; }

        public string SubtotalSinImpuesto { get; set; }

        public string IceCalculado { get; set; }

        public NumeroFacturaModel NumeroFacturaInfo { get; set; }

        public ICollection<FormaPagoModel> ListFormaPago { get; set; }

        public FacturaReembolsoModel Reembolso { get; set; }

        public bool EsReembolso { get; set; }

        public FacturaModel Exportacion { get; set; }

        public FacturaModel Factura { get; set; }

        public bool EsExportacion { get; set; }

        public string Descripcion
        {
            get
            {
                return m_descripcion;
            }
            set
            {
                m_descripcion = value;
                RaisePropertyChanged(() => Descripcion);
            }
        }

        public string VerTipoFacturaDetalle
        {
            get
            {
                return m_TipoFacturaDetalle;
            }
            set
            {
                m_TipoFacturaDetalle = value;
                RaisePropertyChanged(() => VerTipoFacturaDetalle);
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

        public ClienteModel Cliente
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

        public string NumeroFactura
        {
            get
            {
                return m_numeroFactura;
            }
            set
            {
                m_numeroFactura = value;
                RaisePropertyChanged(() => NumeroFactura);
            }
        }

        public DateTime FechaEmision
        {
            get
            {
                return m_fechaEmision;
            }
            set
            {
                m_fechaEmision = value;
                RaisePropertyChanged(() => FechaEmision);
            }
        }

        public string GuiaRemision
        {
            get
            {
                return m_guiaRemision;
            }
            set
            {
                m_guiaRemision = value;
                RaisePropertyChanged(() => GuiaRemision);
            }
        }

        public FormaPagoModel FormaPago
        {
            get
            {
                return m_formaPago;
            }
            set
            {
                m_formaPago = value;
                RaisePropertyChanged(() => FormaPago);
            }
        }


        public string SubtotalDocePorciento
        {
            get
            {
                return m_subtotalDocePorciento;
            }
            set
            {
                m_subtotalDocePorciento = value;
                RaisePropertyChanged(() => SubtotalDocePorciento);
            }
        }

        public string IvaCalculado
        {
            get
            {
                return m_ivaCalculado;
            }
            set
            {
                m_ivaCalculado = value;
                RaisePropertyChanged(() => IvaCalculado);
            }
        }

        public string ValorTotal
        {
            get
            {
                return m_valorTotal;
            }
            set
            {
                m_valorTotal = value;
                RaisePropertyChanged(() => ValorTotal);
            }
        }

        public ObservableCollection<FacturaDetalleModel> DetalleInfo
        {
            get
            {
                return m_detalleInfo;
            }
            set
            {
                m_detalleInfo = value;
                RaisePropertyChanged(() => DetalleInfo);
            }
        }

        public FacturaDetalleModel DetalleSeleccionado
        {
            get
            {
                return m_detalleSeleccionado;
            }
            set
            {
                m_detalleSeleccionado = value;
                RaisePropertyChanged(() => DetalleSeleccionado);
            }
        }

        public ObservableCollection<InformacionAdicional> InfoAdicional
        {
            get
            {
                return m_infoAdicional;
            }
            set
            {
                m_infoAdicional = value;
                RaisePropertyChanged(() => InfoAdicional);
            }
        }

        public InformacionAdicional InfoAdicionalSeleccionado
        {
            get
            {
                return m_infoAdicionalSeleccionado;
            }
            set
            {
                m_infoAdicionalSeleccionado = value;
            }
        }

        public string ValorUno
        {
            get
            {
                return m_valorUno;
            }
            set
            {
                m_valorUno = value;
                RaisePropertyChanged(() => ValorUno);
            }
        }

        public string ValorDos
        {
            get
            {
                return m_valorDos;
            }
            set
            {
                m_valorDos = value;
                RaisePropertyChanged(() => ValorDos);
            }
        }

        public string ValorTres
        {
            get
            {
                return m_valorTres;
            }
            set
            {
                m_valorTres = value;
                RaisePropertyChanged(() => ValorTres);
            }
        }

        public string ValorCuatro
        {
            get
            {
                return m_valorCuatro;
            }
            set
            {
                m_valorCuatro = value;
                RaisePropertyChanged(() => ValorCuatro);
            }
        }

        public string TituloComplemetario
        {
            get
            {
                return m_tituloComplemetario;
            }
            set
            {
                m_tituloComplemetario = value;
                RaisePropertyChanged(() => TituloComplemetario);
            }
        }

        #endregion

        #region Constructor

        public FacturaTransportistaViewModel(ICatalogoService catalogoService,
            IEstablecimientoService establecimientoService,
            IClienteService clienteService,
            IFacturaService facturaService,
            IArticuloService articuloService)
        {
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
            m_establecimientoService = establecimientoService ?? throw new ArgumentNullException(nameof(establecimientoService));
            m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            m_facturaService = facturaService ?? throw new ArgumentNullException(nameof(facturaService));
            m_articuloService = articuloService ?? throw new ArgumentNullException(nameof(articuloService));

            DetalleInfo = new ObservableCollection<FacturaDetalleModel>();
            InfoAdicional = new ObservableCollection<InformacionAdicional>();
            Reembolso = new FacturaReembolsoModel();
            Empresa = new EstablecimientoModel();
            NumeroFacturaInfo = new NumeroFacturaModel();
            FormaPago = new FormaPagoModel();
            ListFormaPago = new Collection<FormaPagoModel>();
            Exportacion = new FacturaModel();
            Factura = new FacturaModel();

            FechaEmision = DateTime.Now;
            EsReembolso = false;
        }

        #endregion

        #region Command

        public ICommand MostrarModalEmpresaCommand => new Command(async () => await ModalEmpresaAsync());

        public ICommand MostrarModalClienteCommand => new Command(async () => await ModalClienteAsync());

        public ICommand MostrarModalNumeroFacturaCommand => new Command(async () => await ModalNumeroFacturaAsync());

        public ICommand MostrarModalFormaPagoCommand => new Command(async () => await ModalFormaPagoAsync());

        public ICommand MostrarModalDetalleCommand => new Command(async () => await ModalDetalleAsync());

        public ICommand EliminarDetalleCommand => new Command(async () => await EliminarDetalleAsync());

        public ICommand MostrarDetalleCalculoCommand => new Command(async () => await MostrarDetalleCalculoAsync());

        public ICommand MostrarModalInformacionAdicionalCommand => new Command(async () => await MostrarModalInformacionAdicionalAsync());

        public ICommand EliminarInformacionAdicionalCommand => new Command(async () => await EliminarInformacionAdicionalAsync());

        public ICommand GuardarFacturaCommand => new Command(async () => await GuardarFacturaAsync());

        public ICommand CancelarCommand => new Command(async () => await CancelarCommandAsync());

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            Descripcion = "Factura Transportista";

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await m_navigationService.NavigateToModalAsync<ModalSinInternetViewModel>();
            }
            else
            {
                ShowLoading();
                var establecimientos = await m_establecimientoService.ObtenerDataEstablecimientosTransportitasPorUsuario();
                HideLoading();

                var empresa = establecimientos.FirstOrDefault();
                Empresa = empresa;

                SubtotalDocePorciento = "0.00";
                SubtotalCeroPorciento = "0.00";
                SubtotalSinImpuesto = "0.00";
                SubtotalNoObjetoIva = "0.00";
                SubtotalExentoIva = "0.00";
                IvaCalculado = "0.00";
                IceCalculado = "0.00";
                ValorTotal = "0.00";
                ValorUno = "";
                ValorDos = "";
                ValorTres = "";
                ValorCuatro = "";
            }

            await base.InitializeAsync(navigationData);
        }

        private async Task CancelarCommandAsync()
        {
            await m_navigationService.NavigateBackAsync();
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
                var establecimientos = await m_establecimientoService.ObtenerDataEstablecimientosTransportitasPorUsuario();
                HideLoading();

                await m_navigationService.NavigateToModalAsync<ModalEmpresaViewModel>(establecimientos);
            }
        }

        private async Task ModalClienteAsync()
        {
            if (Empresa != null && Empresa.Ruc != null)
            {
                ShowLoading();
                var clientes = await m_clienteService.ObtenerCliente(Empresa.Ruc, true);
                HideLoading();

                var request = new ClienteRequest
                {
                    Cliente = clientes,
                    RucEmpresa = Empresa.Ruc
                };

                await m_navigationService.NavigateToModalAsync<ModalClienteViewModel>(request);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Seleccione una empresa", "Aceptar");
            }

        }

        private async Task ModalNumeroFacturaAsync()
        {
            if (Empresa != null && Empresa.Ruc != null)
            {
                ShowLoading();
                var codEstablecimiento = await m_facturaService.ObtenerCodigoEstablecimiento(Empresa.Ruc, true);
                HideLoading();

                var request = new RequestModel
                {
                    RucEmpresa = Empresa.Ruc,
                    ListCodEstablecimiento = codEstablecimiento
                };
                await m_navigationService.NavigateToModalAsync<ModalNumeroFacturaViewModel>(request);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Seleccione una empresa", "Aceptar");
            }

        }

        private async Task ModalFormaPagoAsync()
        {
            ShowLoading();
            var formaPago = await m_catalogoService.ObtenerFormaPago();
            HideLoading();

            await m_navigationService.NavigateToModalAsync<ModalFormaPagoViewModel>(formaPago);
        }

        private async Task ModalDetalleAsync()
        {
            if (Empresa != null && Empresa.Ruc != null)
            {
                ShowLoading();
                var articulo = await m_articuloService.ObtenerArticulosPorEmpresa("", Empresa.Ruc, true);
                HideLoading();

                var request = new RequestModel
                {
                    RucEmpresa = Empresa.Ruc,
                    Articulo = articulo
                };

                await m_navigationService.NavigateToModalAsync<ModalDetalleViewModel>(request);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Seleccione una empresa", "Aceptar");
            }

        }

        private async Task MostrarDetalleCalculoAsync()
        {
            var info = new FacturaModel
            {
                SubtotalDocePorciento = Convert.ToDecimal(SubtotalDocePorciento),
                SubtotalCeroPorciento = Convert.ToDecimal(SubtotalCeroPorciento),
                SubtotalSinImpuestos = Convert.ToDecimal(SubtotalSinImpuesto),
                SubtotalNoObjetoIva = Convert.ToDecimal(SubtotalNoObjetoIva),
                SubtotalExentoIva = Convert.ToDecimal(SubtotalExentoIva),
                IvaDocePorCiento = Convert.ToDecimal(IvaCalculado),
                Ice = Convert.ToDecimal(IceCalculado),
                Total = Convert.ToDecimal(ValorTotal)
            };
            await m_navigationService.NavigateToModalAsync<ModalDetalleCalculoViewModel>(info);
        }

        private async Task MostrarModalInformacionAdicionalAsync()
        {
            await m_navigationService.NavigateToModalAsync<ModalInformacionAdicionalViewModel>();
        }

        public override async Task OnPopModalAsync(object navigationData, string nombrePage)
        {
            if (navigationData != null)
            {
                switch (nombrePage)
                {
                    case "ModalEmpresaView":
                        var establecimiento = navigationData as EstablecimientoModel;
                        Empresa = establecimiento;
                        break;
                    case "ModalClienteView":
                        var cliente = navigationData as ClienteModel;
                        Cliente = cliente;
                        break;
                    case "ModalNumeroFacturaView":
                        var numFactura = navigationData as NumeroFacturaModel;
                        FormatearNumeroFactura(numFactura);
                        break;
                    case "ModalFormaPagoView":
                        var formaPago = navigationData as FormaPagoModel;
                        FormaPago = new FormaPagoModel();
                        ListFormaPago = new Collection<FormaPagoModel>();
                        FormaPago = formaPago;
                        ListFormaPago.Add(formaPago);
                        break;
                    case "ModalDetalleView":
                        var detalle = navigationData as ArticuloModel;
                        await AgregarDetalle(detalle);
                        break;
                    case "ModalInformacionAdicionalView":
                        var infoAcional = navigationData as CatalogoModel;
                        AgregarInfoAcional(infoAcional);
                        break;
                    default:
                        break;
                }
            }

            await base.OnPopModalAsync(navigationData, nombrePage);
        }

        private void FormatearNumeroFactura(NumeroFacturaModel numeroFactura)
        {
            NumeroFacturaInfo = numeroFactura;
            var codEstab = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodEstablecimiento : "";
            var codPtoEm = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodPuntoEmision : "";
            var codSec = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodSecuencial : "";
            NumeroFactura = codEstab + "-" + codPtoEm + "-" + codSec;
        }

        private async Task AgregarDetalle(ArticuloModel detalle)
        {
            if (detalle != null)
            {
                var id = 0;

                if (DetalleInfo.Count > 0)
                {
                    id = DetalleInfo.Select(x => x.Id).Max();
                    id++;
                }

                DetalleInfo.Add(new FacturaDetalleModel()
                {
                    Id = id,
                    Cantidad = detalle.CantidadArticulo,
                    CantidadStr = detalle.CantidadArticulo.ToString(),
                    ProductoCodigo = detalle.Codigo,
                    Descripcion = detalle.Descripcion,
                    InformacionDetalle = detalle.CantidadArticulo + " X $ " + detalle.PrecioUnidadBase.ToString(),
                    PrecioUnidadBase = detalle.PrecioUnidadBase,
                    PrecioUnitarioStr = detalle.PrecioUnidadBase,
                    PrecioUnitario = Convert.ToDecimal(detalle.PrecioUnidadBase),
                    DescuentoStr = "0",
                    SubtotalDocePorciento = detalle.SubtotalDocePorciento,
                    SubtotalCeroPorciento = detalle.SubtotalCeroPorciento,
                    SubtotalNoObjetoIva = detalle.SubtotalNoObjetoIva,
                    SubtotalSinImpuesto = detalle.SubtotalSinImpuesto,
                    SubtotalExentoIva = detalle.SubtotalExentoIva,
                    IvaCalculadoArticulo = detalle.IvaCalculadoArticulo,
                    IceCalculadoArticulo = detalle.IceCalculadoArticulo,
                    IceValor = detalle.IceCalculadoArticulo,
                    PrecioFinal = detalle.PrecioFinal,
                    ImpuestoIVA = detalle.ImpuestoIVA,
                    PorcentajeIvaCodigo = detalle.ImpuestoIVA != null ? detalle.ImpuestoIVA.Codigo : "",
                    IceCodigo = detalle.ImpuestoICE != null ? detalle.ImpuestoICE.Codigo : ""
                });

                CalcularValorDetalles();

                await CargarFormaPagoPorDefecto();
            }
        }

        private async Task CargarFormaPagoPorDefecto()
        {
            ShowLoading();
            var formaPago = await m_catalogoService.ObtenerFormaPago();
            HideLoading();

            var _formaPago = formaPago.Select(x => x).OrderByDescending(x => x.Contador).FirstOrDefault();

            var newFormaPgo = new FormaPagoModel
            {
                MetodoPago = _formaPago.Detalle,
                FormaPagoCodigo = _formaPago.Codigo,
                Monto = Convert.ToDecimal(ValorTotal),
                Plazo = "1",
                TiempoCodigo = "01"
            };

            FormaPago = newFormaPgo;
            ListFormaPago.Add(FormaPago);
        }

        private void CalcularValorDetalles()
        {
            var subtotalDocePorciento = DetalleInfo.Select(x => x.SubtotalDocePorciento).Sum();
            SubtotalDocePorciento = subtotalDocePorciento.ToString("N2");

            var subtotalCeroPorciento = DetalleInfo.Select(x => x.SubtotalCeroPorciento).Sum();
            SubtotalCeroPorciento = subtotalCeroPorciento.ToString("N2");

            var subtotalNoObjetoIva = DetalleInfo.Select(x => x.SubtotalNoObjetoIva).Sum();
            SubtotalNoObjetoIva = subtotalNoObjetoIva.ToString("N2");

            var subtotalSinImpuesto = DetalleInfo.Select(x => x.SubtotalSinImpuesto).Sum();
            SubtotalSinImpuesto = subtotalSinImpuesto.ToString("N2");

            var subtotalExentoIva = DetalleInfo.Select(x => x.SubtotalExentoIva).Sum();
            SubtotalExentoIva = subtotalExentoIva.ToString("N2");

            var ivaCalculado = DetalleInfo.Select(x => x.IvaCalculadoArticulo).Sum();
            IvaCalculado = ivaCalculado.ToString("N2");

            var iceCalculado = DetalleInfo.Select(x => x.IceCalculadoArticulo).Sum();
            IceCalculado = iceCalculado.ToString("N2");

            var valorTotal = DetalleInfo.Select(x => x.PrecioFinal).Sum();
            ValorTotal = valorTotal.ToString("N2");
        }

        private async Task EliminarDetalleAsync()
        {
            if (DetalleSeleccionado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Detalle", "Seleccione el detalle que desea eliminar", "Aceptar");
            }
            else
            {
                var info = ObtenerDetalle(DetalleSeleccionado.Id);
                DetalleInfo.Remove(info);

                CalcularValorDetalles();

                await Application.Current.MainPage.DisplayAlert("Mensaje satisfactorio", "Detalle eliminado correctamente", "Aceptar");
            }
        }

        private FacturaDetalleModel ObtenerDetalle(int id)
        {
            var detalle = DetalleInfo.Where(x => x.Id == id);

            if (!detalle.Any())
            {
                return null;
            }

            return detalle.FirstOrDefault();
        }

        private void AgregarInfoAcional(CatalogoModel infoAcional)
        {
            if (infoAcional != null)
            {
                var id = 0;

                if (InfoAdicional.Count > 0)
                {
                    id = InfoAdicional.Select(x => x.Id).Max();
                    id++;
                }

                InfoAdicional.Add(new InformacionAdicional()
                {
                    Id = id,
                    Codigo = infoAcional.Codigo,
                    Valor = infoAcional.Detalle,
                    Icono = "icon_minus.png"
                });
            }
        }

        private async Task EliminarInformacionAdicionalAsync()
        {
            var infoAdicional = InfoAdicionalSeleccionado;

            if (infoAdicional == null)
            {
                await Application.Current.MainPage.DisplayAlert("Información Adicional", "Seleccione la información adicional que desea eliminar", "Aceptar");

            }
            else
            {
                var info = ObtenerInfoAdicional(infoAdicional.Id);
                InfoAdicional.Remove(info);
                await Application.Current.MainPage.DisplayAlert("Mensaje satisfactorio", "Información Adicional eliminado correctamente", "Aceptar");
            }
        }

        private InformacionAdicional ObtenerInfoAdicional(int id)
        {
            var infoAdicional = InfoAdicional.Where(x => x.Id == id);

            if (!infoAdicional.Any())
            {
                return null;
            }

            return infoAdicional.FirstOrDefault();
        }

        private async Task GuardarFacturaAsync()
        {
            var factura = new FacturaModel
            {
                Adicionales = InfoAdicional,
                CompradorDireccion = Cliente != null ? Cliente.Direccion : "",
                ContribuyenteEspecialNumero = Empresa != null ? Empresa.ContribEsp : "",
                Detalle = DetalleInfo,
                EmpresaRuc = Empresa != null ? Empresa.Ruc : "",
                EstablecimientoNumero = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodEstablecimiento : "",
                FechaEmision = FechaEmision,
                FormasPago = ListFormaPago,
                GuiaRemision = GuiaRemision,
                Identificacion = Cliente != null ? Cliente.Identificacion : "",
                ImpuestoRenta = 0M,
                IvaDocePorCiento = Convert.ToDecimal(IvaCalculado),
                MatrizDireccion = Empresa != null ? Empresa.DirMatriz : "",
                Moneda = "DOLAR",
                ObligadoCont = Empresa != null ? Empresa.ObligadoContab : false,
                PuntoEmisionNumero = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodPuntoEmision : "",
                PuntoEmisionTransportista = "",
                RazonSocial = Empresa != null ? Empresa.RazonSocial : "",
                RazonSocialProveedor = Cliente != null ? Cliente.RazonSocial : "",
                RazonSocialTransporista = "",
                DefinicionTermino = Exportacion != null ? Exportacion.DefinicionTermino : "",
                DefTerminoSinImpuesto = Exportacion != null ? Exportacion.DefTerminoSinImpuesto : "",
                PuertoEmbarque = Exportacion != null ? Exportacion.PuertoEmbarque : "",
                PaisAdquisicion = Exportacion != null ? Exportacion.PaisAdquisicion : new CatalogoModel(),
                PaisOrigen = Exportacion != null ? Exportacion.PaisOrigen : new CatalogoModel(),
                FleteInternacional = Exportacion != null ? Exportacion.FleteInternacional : 0,
                LugarConvenio = Exportacion != null ? Exportacion.LugarConvenio : "",
                SeguroInternacional = Exportacion != null ? Exportacion.SeguroInternacional : 0,
                PuertoDestino = Exportacion != null ? Exportacion.PuertoDestino : "",
                GastosAduaneros = Exportacion != null ? Exportacion.GastosAduaneros : 0,
                PaisDestino = Exportacion != null ? Exportacion.PaisDestino : new CatalogoModel(),
                GastosTransporte = Exportacion != null ? Exportacion.GastosTransporte : 0,
                Estransportista = true,
                Secuencial = NumeroFacturaInfo != null ? NumeroFacturaInfo.CodSecuencial : "",
                SubtotalSinImpuestos = Convert.ToDecimal(SubtotalSinImpuesto),
                SubtotalDocePorciento = Convert.ToDecimal(SubtotalDocePorciento),
                SubtotalCeroPorciento = Convert.ToDecimal(SubtotalCeroPorciento),
                SubtotalNoObjetoIva = Convert.ToDecimal(SubtotalNoObjetoIva),
                SubtotalExentoIva = Convert.ToDecimal(SubtotalExentoIva),
                TipoIdentificacionCodigo = Cliente != null ? Cliente.IdTipoIdentificacion : "",
                Total = Convert.ToDecimal(ValorTotal),
                TotalDescuento = 0M
            };

            var request = new GuardarFacturaRequest();
            request.Factura = factura;

            ShowLoading();
            var response = await m_facturaService.GuardarFactura(request);
            HideLoading();

            var mensajeError = "";
            if (response.Exito)
            {
                LimpiarFactura();
                await m_navigationService.NavigateToAsync<FacturaExitoViewModel>(factura);
                await m_navigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                foreach (var item in response.Validaciones)
                {
                    mensajeError = mensajeError + Environment.NewLine + item;
                }
                await Application.Current.MainPage.DisplayAlert("Mensaje Error", mensajeError, "Aceptar");
            }
        }

        private void LimpiarFactura()
        {
            VerTipoFacturaDetalle = "";
            Empresa = new EstablecimientoModel();
            Cliente = new ClienteModel();
            NumeroFactura = "";
            NumeroFacturaInfo = new NumeroFacturaModel();
            FechaEmision = DateTime.Now;
            GuiaRemision = "";
            DetalleInfo = new ObservableCollection<FacturaDetalleModel>();
            SubtotalSinImpuesto = "0.00";
            SubtotalDocePorciento = "0.00";
            IvaCalculado = "0.00";
            ValorTotal = "0.00";
            FormaPago = new FormaPagoModel();
            InfoAdicional = new ObservableCollection<InformacionAdicional>();
        }

        #endregion
    }
}
