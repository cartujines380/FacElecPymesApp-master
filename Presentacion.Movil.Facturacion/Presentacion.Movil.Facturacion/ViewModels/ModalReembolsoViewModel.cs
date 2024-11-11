using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalReembolsoViewModel : ViewModelBase
    {
        #region Campos

        private string m_numeroId;
        private string m_razonSocial;
        private decimal m_baseImponible;
        private string m_impuestoReembolso;
        private string m_baseImponibleTotal;
        private string m_noComprobante;
        private DateTime m_fechaEmisionReembolso;
        private string m_noAutorizacion;
        private int m_codigoTipoId;
        private int m_codigoPais;
        private int m_codigoTipoProveedorReembolso;
        private int m_codigoTipoDocReembolso;
        private int m_codigoTipoImpuesto;
        private int m_codigoTipoPorcentaje;
        private ObservableCollection<CatalogoModel> m_tipoId;
        private ObservableCollection<CatalogoModel> m_pais;
        private ObservableCollection<CatalogoModel> m_tipoProveedorReembolso;
        private ObservableCollection<CatalogoModel> m_tipoDocReembolso;
        private ObservableCollection<CatalogoModel> m_tipoImpuesto;
        private ObservableCollection<ImpuestoModel> m_tipoPorcentaje;
        private CatalogoModel m_tipoIdSeleccionado;
        private CatalogoModel m_paisSeleccionado;
        private CatalogoModel m_tipoProveedorReembolsoSeleccionado;
        private CatalogoModel m_tipoDocReembolsoSeleccionado;
        private CatalogoModel m_tipoImpuestoSeleccionado;
        private ImpuestoModel m_tipoPorcentajeSeleccionado;

        private readonly ICatalogoService m_catalogoService;

        #endregion

        #region Propiedades

        public string TipoFactura { get; set; }

        public bool EsEdicion { get; set; }

        public string PorcentajeCodigo { get; set; }

        public string NumeroId
        {
            get
            {
                return m_numeroId;
            }
            set
            {
                m_numeroId = value;
                RaisePropertyChanged(() => NumeroId);
            }
        }

        public string RazonSocial
        {
            get
            {
                return m_razonSocial;
            }
            set
            {
                m_razonSocial = value;
                RaisePropertyChanged(() => RazonSocial);
            }
        }

        public string NoComprobante
        {
            get
            {
                return m_noComprobante;
            }
            set
            {
                m_noComprobante = value;
                RaisePropertyChanged(() => NoComprobante);
            }
        }

        public DateTime FechaEmisionReembolso
        {
            get
            {
                return m_fechaEmisionReembolso;
            }
            set
            {
                m_fechaEmisionReembolso = value;
                RaisePropertyChanged(() => FechaEmisionReembolso);
            }
        }

        public string NoAutorizacion
        {
            get
            {
                return m_noAutorizacion;
            }
            set
            {
                m_noAutorizacion = value;
                RaisePropertyChanged(() => NoAutorizacion);
            }
        }

        public decimal BaseImponible
        {
            get
            {
                CalcularReembolsoTotal();
                return m_baseImponible;
            }
            set
            {
                m_baseImponible = value;
                RaisePropertyChanged(() => BaseImponible);
            }
        }

        public string ImpuestoReembolso
        {
            get
            {
                return m_impuestoReembolso;
            }
            set
            {
                m_impuestoReembolso = value;
                RaisePropertyChanged(() => ImpuestoReembolso);
            }
        }

        public string BaseImponibleTotal
        {
            get
            {
                return m_baseImponibleTotal;
            }
            set
            {
                m_baseImponibleTotal = value;
                RaisePropertyChanged(() => BaseImponibleTotal);
            }
        }

        public ObservableCollection<CatalogoModel> TipoId
        {
            get
            {
                return m_tipoId;
            }
            set
            {
                m_tipoId = value;
                RaisePropertyChanged(() => TipoId);
            }
        }

        public CatalogoModel TipoIdSeleccionado
        {
            get
            {
                return m_tipoIdSeleccionado;
            }
            set
            {
                m_tipoIdSeleccionado = value;
                RaisePropertyChanged(() => TipoId);
            }
        }

        public ObservableCollection<CatalogoModel> Pais
        {
            get
            {
                return m_pais;
            }
            set
            {
                m_pais = value;
                RaisePropertyChanged(() => Pais);
            }
        }

        public CatalogoModel PaisSeleccionado
        {
            get
            {
                return m_paisSeleccionado;
            }
            set
            {
                m_paisSeleccionado = value;
                RaisePropertyChanged(() => PaisSeleccionado);
            }
        }

        public ObservableCollection<CatalogoModel> TipoProveedorReembolso
        {
            get
            {
                return m_tipoProveedorReembolso;
            }
            set
            {
                m_tipoProveedorReembolso = value;
                RaisePropertyChanged(() => TipoProveedorReembolso);
            }
        }

        public CatalogoModel TipoProveedorReembolsoSeleccionado
        {
            get
            {
                return m_tipoProveedorReembolsoSeleccionado;
            }
            set
            {
                m_tipoProveedorReembolsoSeleccionado = value;
                RaisePropertyChanged(() => TipoProveedorReembolsoSeleccionado);
            }
        }

        public ObservableCollection<CatalogoModel> TipoDocReembolso
        {
            get
            {
                return m_tipoDocReembolso;
            }
            set
            {
                m_tipoDocReembolso = value;
                RaisePropertyChanged(() => TipoDocReembolso);
            }
        }

        public CatalogoModel TipoDocReembolsoSeleccionado
        {
            get
            {
                return m_tipoDocReembolsoSeleccionado;
            }
            set
            {
                m_tipoDocReembolsoSeleccionado = value;
                RaisePropertyChanged(() => TipoDocReembolsoSeleccionado);
            }
        }

        public ObservableCollection<CatalogoModel> TipoImpuesto
        {
            get
            {
                return m_tipoImpuesto;
            }
            set
            {
                m_tipoImpuesto = value;
                RaisePropertyChanged(() => TipoImpuesto);
            }
        }

        public CatalogoModel TipoImpuestoSeleccionado
        {
            get
            {
                return m_tipoImpuestoSeleccionado;
            }
            set
            {
                m_tipoImpuestoSeleccionado = value;
                _ = Seleccion();
            }
        }

        public ObservableCollection<ImpuestoModel> TipoPorcentaje
        {
            get
            {
                return m_tipoPorcentaje;
            }
            set
            {
                m_tipoPorcentaje = value;
                RaisePropertyChanged(() => TipoPorcentaje);
            }
        }

        public ImpuestoModel TipoPorcentajeSeleccionado
        {
            get
            {
                return m_tipoPorcentajeSeleccionado;
            }
            set
            {
                m_tipoPorcentajeSeleccionado = value;
                CalcularReembolsoTotal();
            }
        }

        public int CodigoTipoId
        {
            get
            {
                return m_codigoTipoId;
            }
            set
            {
                m_codigoTipoId = value;
                RaisePropertyChanged(() => CodigoTipoId);
            }
        }

        public int CodigoPais
        {
            get
            {
                return m_codigoPais;
            }
            set
            {
                m_codigoPais = value;
                RaisePropertyChanged(() => CodigoPais);
            }
        }

        public int CodigoTipoProveedorReembolso
        {
            get
            {
                return m_codigoTipoProveedorReembolso;
            }
            set
            {
                m_codigoTipoProveedorReembolso = value;
                RaisePropertyChanged(() => CodigoTipoProveedorReembolso);
            }
        }

        public int CodigoTipoDocReembolso
        {
            get
            {
                return m_codigoTipoDocReembolso;
            }
            set
            {
                m_codigoTipoDocReembolso = value;
                RaisePropertyChanged(() => CodigoTipoDocReembolso);
            }
        }

        public int CodigoTipoImpuesto
        {
            get
            {
                return m_codigoTipoImpuesto;
            }
            set
            {
                m_codigoTipoImpuesto = value;
                RaisePropertyChanged(() => CodigoTipoImpuesto);
            }
        }

        public int CodigoTipoPorcentaje
        {
            get
            {
                return m_codigoTipoPorcentaje;
            }
            set
            {
                m_codigoTipoPorcentaje = value;
                RaisePropertyChanged(() => CodigoTipoPorcentaje);
            }
        }

        #endregion

        #region Constructor

        public ModalReembolsoViewModel(ICatalogoService catalogoService)
        {
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));

            FechaEmisionReembolso = DateTime.Now;
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());
        public ICommand GuardarReembolsoComannd => new Command(async () => await GuardarReembolsoAsync());

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            var tipoFactura = navigationData as FacturaModel;
            TipoFactura = tipoFactura.TipoFactura;
            EsEdicion = tipoFactura.EsEdicion;
            ShowLoading();
            TipoId = await m_catalogoService.ObtenerCatalogo("tbl_tipoIdentificacion");
            TipoProveedorReembolso = await m_catalogoService.ObtenerCatalogo("tbl_tipoProveedorReembolso");
            TipoDocReembolso = await m_catalogoService.ObtenerCatalogo("tbl_tipoDocReembolso");
            TipoImpuesto = await m_catalogoService.ObtenerCatalogo("tbl_tipoImpuesto");
            HideLoading();

            var _pais = await m_catalogoService.ObtenerCatalogoPais(2);
            var pais = _pais.OrderBy(x => x.Detalle);
            var oc = new ObservableCollection<CatalogoModel>();
            pais.ForEach(x => oc.Add(x));
            Pais = oc;

            if (EsEdicion)
            {
                LlenarModal(tipoFactura);

                var _tipoId = TipoId.Where(x => x.Codigo == tipoFactura.Reembolso.InfoDetalle.Proveedor.TipoIdentificacionCodigo).FirstOrDefault();
                var _paisEdit = Pais.Where(x => x.Codigo == tipoFactura.Reembolso.InfoDetalle.Proveedor.PaisPagoCodigo).FirstOrDefault();
                var _tipoProveedorReembolso = TipoProveedorReembolso.Where(x => x.Codigo == tipoFactura.Reembolso.InfoDetalle.Proveedor.Tipo).FirstOrDefault();
                var _tipoDocReembolso = TipoDocReembolso.Where(x => x.Codigo == tipoFactura.Reembolso.InfoDetalle.Documento.Codigo).FirstOrDefault();
                var _tipoImpuesto = TipoImpuesto.Where(x => x.Codigo == tipoFactura.Reembolso.InfoDetalle.Impuesto.TipoImpuetsoCodigo).FirstOrDefault();
                var tipo = ObtenerTipoImpuesto(_tipoImpuesto.Codigo);
                ShowLoading();
                TipoPorcentaje = await m_catalogoService.ObtenerImpuestoPorCodigo(tipo);
                HideLoading();
                PorcentajeCodigo = tipoFactura.Reembolso.InfoDetalle.Impuesto.PorcentajeCodigo;

                CodigoTipoId = TipoId.IndexOf(_tipoId);
                CodigoPais = Pais.IndexOf(_paisEdit);
                CodigoTipoProveedorReembolso = TipoProveedorReembolso.IndexOf(_tipoProveedorReembolso);
                CodigoTipoDocReembolso = TipoDocReembolso.IndexOf(_tipoDocReembolso);
                CodigoTipoImpuesto = TipoImpuesto.IndexOf(_tipoImpuesto);
            }

            await base.InitializeAsync(navigationData);
        }

        private void LlenarModal(FacturaModel facturaModel)
        {
            NumeroId = facturaModel.Reembolso.InfoDetalle.Proveedor.Identificacion;
            RazonSocial = facturaModel.Reembolso.InfoDetalle.Proveedor.RazonSocial;

            FechaEmisionReembolso = Convert.ToDateTime(facturaModel.Reembolso.InfoDetalle.Documento.FechaEmision);
            NoAutorizacion = facturaModel.Reembolso.InfoDetalle.Documento.NumeroAutorizacion;

            var establecimiento = facturaModel.Reembolso.InfoDetalle.Documento.EstablecimientoCodigo;
            var ptoEmision = facturaModel.Reembolso.InfoDetalle.Documento.PuntoEmisionCodigo;
            var seceuncial = facturaModel.Reembolso.InfoDetalle.Documento.Secuencial;

            NoComprobante = establecimiento + "-" + ptoEmision + "-" + seceuncial;
            BaseImponible = facturaModel.Reembolso.InfoDetalle.Impuesto.BaseImponible ?? 0;
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task Seleccion()
        {
            var codigo = TipoImpuestoSeleccionado.Codigo;

            var tipo = ObtenerTipoImpuesto(codigo);

            ShowLoading();
            TipoPorcentaje = await m_catalogoService.ObtenerImpuestoPorCodigo(tipo);
            HideLoading();

            var _tipoPorcentaje = TipoPorcentaje.Where(x => x.Codigo == PorcentajeCodigo).FirstOrDefault();
            CodigoTipoPorcentaje = TipoPorcentaje.IndexOf(_tipoPorcentaje);
        }

        private string ObtenerTipoImpuesto(string codigo)
        {
            var tipo = "";

            switch (codigo)
            {
                case "2":
                    tipo = "2016";
                    break;
                case "3":
                    tipo = "2019";
                    break;
                default:
                    break;
            }

            return tipo;
        }

        private void CalcularReembolsoTotal()
        {
            var porcImp = TipoPorcentajeSeleccionado != null ? TipoPorcentajeSeleccionado.Porcentaje : 0;
            var impuestoRem = m_baseImponible * porcImp;
            ImpuestoReembolso = impuestoRem.ToString("N2");
            var baseImp = m_baseImponible + impuestoRem;
            BaseImponibleTotal = baseImp.ToString("N2");
        }

        private string ValidarIdentificacion()
        {
            var mensaje = "";
            var tipoIdent = TipoIdSeleccionado.Codigo;
            var longIdent = NumeroId != null ? NumeroId.Length : 0;

            if (tipoIdent.Equals("04") && (longIdent != 13))
                mensaje = "Error en la identificacion RUC";
            if (tipoIdent.Equals("05") && (longIdent != 10))
                mensaje = "Error en la identificacion Cedula";

            return mensaje;
        }

        private async Task GuardarReembolsoAsync()
        {
            if (ValidarCamposVacios())
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Llene los campos obligatorios", "Aceptar");
            }
            else
            {
                var proveedor = new ProveedorModel
                {
                    Identificacion = NumeroId,
                    PaisPagoCodigo = PaisSeleccionado.Codigo,
                    RazonSocial = RazonSocial,
                    Tipo = TipoProveedorReembolsoSeleccionado.Codigo,
                    TipoIdentificacionCodigo = TipoIdSeleccionado.Codigo,
                    TipoIdentificacion = TipoIdSeleccionado.Detalle
                };

                var mensaje = ValidarIdentificacion();

                if (mensaje == "")
                {
                    var split = m_noComprobante.Split('-');

                    if (split.Length < 1 || split.Length != 3)
                    {
                        await Application.Current.MainPage.DisplayAlert("Advertencia", "No. Comprobante no cumple el formato", "Aceptar");
                    }
                    else
                    {
                        var establecimiento = split[0];
                        var ptoEmision = split[1];
                        var secuencial = split[2];

                        var documento = new DocumentoModel
                        {
                            Codigo = TipoDocReembolsoSeleccionado.Codigo,
                            EstablecimientoCodigo = establecimiento,
                            FechaEmision = FechaEmisionReembolso,
                            NumeroAutorizacion = NoAutorizacion,
                            PuntoEmisionCodigo = ptoEmision,
                            Secuencial = secuencial
                        };

                        var impuesto = new ImpuestoReembolsoModel
                        {
                            BaseImponible = BaseImponible,
                            TipoImpuetsoCodigo = TipoImpuestoSeleccionado.Codigo,
                            Codigo = TipoPorcentajeSeleccionado.Codigo,
                            PorcentajeCodigo = TipoPorcentajeSeleccionado.Codigo,
                            PorcentajeValor = TipoPorcentajeSeleccionado.Porcentaje,
                        };

                        var reembolso = new FacturaModel();
                        reembolso.TipoFactura = TipoFactura;
                        reembolso.EsEdicion = EsEdicion;
                        reembolso.Reembolso.InfoDetalle.Documento = documento;
                        reembolso.Reembolso.InfoDetalle.Impuesto = impuesto;
                        reembolso.Reembolso.InfoDetalle.Proveedor = proveedor;

                        if (!EsEdicion)
                        {
                            await m_navigationService.NavigateBackModalAsync();
                        }

                        await m_navigationService.NavigateBackModalAsync(reembolso);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", mensaje, "Aceptar");
                }
            }
        }

        private bool ValidarCamposVacios()
        {
            var esVacio = false;

            if (NumeroId == null || PaisSeleccionado == null || TipoProveedorReembolsoSeleccionado == null || TipoIdSeleccionado == null
                || TipoDocReembolsoSeleccionado == null || NoComprobante == "" || NoAutorizacion == null || TipoImpuestoSeleccionado == null || TipoPorcentajeSeleccionado == null)
            {
                esVacio = true;
            }

            return esVacio;
        }

        #endregion
    }
}