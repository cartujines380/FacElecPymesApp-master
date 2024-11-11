using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalDetalleNuevoViewModel : ViewModelBase
    {
        #region Campos

        private string m_codigo;
        private string m_nombre;
        private int m_cantidad;
        private decimal m_precioUnidadBase;
        private string m_precioFinalStr;
        private ObservableCollection<ImpuestoModel> m_iva;
        private ImpuestoModel m_ivaSeleccionado;
        private ObservableCollection<ImpuestoModel> m_ice;
        private ImpuestoModel m_iceSeleccionado;

        private ObservableCollection<CatalogoModel> m_tipoArticulo;
        private CatalogoModel m_tipoArticuloSeleccionado;

        private readonly IArticuloService m_articuloService;
        private readonly ICatalogoService m_catalogoService;

        private const string IMPUESTO_CERO_PORCIENTO = "0";
        private const string IMPUESTO_DOCE_PORCIENTO = "2";
        private const string IMPUESTO_CATORCE_PORCIENTO = "3";
        private const string IMPUESTO_NO_OBJETO_IVA = "6";
        private const string IMPUESTO_EXCENTO_IVA = "7";
        private const string IMPUESTO_NO_GRABA_ICE = "8";

        #endregion

        #region Propiedades

        public ArticuloModel Articulo { get; set; }

        public string RucEmpresa { get; set; }

        public decimal PorcentajeIva { get; set; }

        public string CodigoIva { get; set; }

        public decimal PorcentajeIce { get; set; }

        public string CodigoIce { get; set; }

        private decimal SubtotalSinImpuesto { get; set; }

        private decimal SubtotalCeroPorciento { get; set; }

        private decimal SubtotalDocePorciento { get; set; }

        private decimal SubtotalNoObjetoIva { get; set; }

        private decimal SubtotalExentoIva { get; set; }

        private decimal IvaCalculado { get; set; }

        private decimal IceCalculado { get; set; }

        private decimal PrecioFinalConIva { get; set; }

        private decimal PrecioFinal { get; set; }

        public ObservableCollection<CatalogoModel> TipoArticulo
        {
            get
            {
                return m_tipoArticulo;
            }
            set
            {
                m_tipoArticulo = value;
                RaisePropertyChanged(() => TipoArticulo);
            }
        }


        public CatalogoModel TipoArticuloSeleccionado
        {
            get
            {
                return m_tipoArticuloSeleccionado;
            }
            set
            {
                m_tipoArticuloSeleccionado = value;
                RaisePropertyChanged(() => TipoArticuloSeleccionado);
            }
        }

        public string Codigo
        {
            get
            {
                return m_codigo;
            }
            set
            {
                m_codigo = value;
                RaisePropertyChanged(() => Codigo);
            }
        }

        public string Nombre
        {
            get
            {
                return m_nombre;
            }
            set
            {
                m_nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }

        public int Cantidad
        {
            get
            {
                CalcularPrecioFinal();
                return m_cantidad;
            }
            set
            {
                m_cantidad = value;
                RaisePropertyChanged(() => Cantidad);
            }
        }

        public decimal PrecioUnidadBase
        {
            get
            {
                return m_precioUnidadBase;
            }
            set
            {
                m_precioUnidadBase = value;
                RaisePropertyChanged(() => PrecioUnidadBase);
            }
        }

        public string PrecioFinalConIvaStr
        {
            get
            {
                return m_precioFinalStr;
            }
            set
            {
                m_precioFinalStr = value;
                RaisePropertyChanged(() => PrecioFinalConIvaStr);
            }
        }

        public ObservableCollection<ImpuestoModel> IVA
        {
            get
            {
                return m_iva;
            }
            set
            {
                m_iva = value;
                RaisePropertyChanged(() => IVA);
            }
        }

        public ImpuestoModel IVASeleccionado
        {
            get
            {
                return m_ivaSeleccionado;
            }
            set
            {
                m_ivaSeleccionado = value;
                PorcentajeIva = m_ivaSeleccionado.Porcentaje;
                CodigoIva = m_ivaSeleccionado.Codigo;
                CalcularPrecioFinal();
            }
        }

        public ObservableCollection<ImpuestoModel> ICE
        {
            get
            {
                return m_ice;
            }
            set
            {
                m_ice = value;
                RaisePropertyChanged(() => ICE);
            }
        }

        public ImpuestoModel ICESeleccionado
        {
            get
            {
                return m_iceSeleccionado;
            }
            set
            {
                m_iceSeleccionado = value;
                PorcentajeIce = m_iceSeleccionado.Porcentaje;
                CodigoIce = m_iceSeleccionado.Codigo;
                CalcularPrecioFinalInterno();
            }
        }

        #endregion

        #region Constructor

        public ModalDetalleNuevoViewModel(
             IArticuloService articuloService,
             ICatalogoService catalogoService)
        {
            m_articuloService = articuloService ?? throw new ArgumentNullException(nameof(articuloService));
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand GuardarClienteComannd => new Command(async () => await GuardarClienteAsync());

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            var articuloInfo = navigationData as ArticuloRequest;

            if (articuloInfo != null)
            {
                TipoArticulo = articuloInfo.Articulo;
                RucEmpresa = articuloInfo.RucEmpresa;
            }

            IVA = await m_catalogoService.ObtenerImpuestoPorCodigo("2016");
            ICE = await m_catalogoService.ObtenerImpuestoPorCodigo("2019");

            await base.InitializeAsync(navigationData);
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private void CalcularPrecioFinal()
        {
            CalcularPrecioFinalInterno();
        }

        private void CalcularPrecioFinalInterno()
        {
            SubtotalSinImpuesto = 0M;
            SubtotalDocePorciento = 0M;
            SubtotalCeroPorciento = 0M;
            SubtotalNoObjetoIva = 0M;
            IvaCalculado = 0M;
            PrecioFinalConIva = 0M;

            if (CodigoIva == IMPUESTO_DOCE_PORCIENTO || CodigoIva == IMPUESTO_CATORCE_PORCIENTO)
            {
                SubtotalDocePorciento = m_cantidad * PrecioUnidadBase;
                IceCalculado = SubtotalDocePorciento * PorcentajeIce;
                SubtotalDocePorciento = SubtotalDocePorciento + IceCalculado;
                IvaCalculado = SubtotalDocePorciento * PorcentajeIva;
                PrecioFinalConIva = SubtotalDocePorciento + IvaCalculado;
                PrecioFinalConIvaStr = PrecioFinalConIva.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_CERO_PORCIENTO)
            {
                SubtotalCeroPorciento = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalCeroPorciento.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_NO_OBJETO_IVA)
            {
                SubtotalNoObjetoIva = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalNoObjetoIva.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_EXCENTO_IVA)
            {
                SubtotalExentoIva = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalExentoIva.ToString("N2");
            }

            SubtotalSinImpuesto = SubtotalDocePorciento - IceCalculado + SubtotalCeroPorciento + SubtotalNoObjetoIva + SubtotalExentoIva;
            PrecioFinal = PrecioFinalConIva + SubtotalCeroPorciento + SubtotalNoObjetoIva + SubtotalExentoIva;
        }

        private async Task GuardarClienteAsync()
        {

            if (ValidarCamposVacios())
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Llene los campos obligatorios", "Aceptar");
            }
            else
            {
                Articulo = new ArticuloModel()
                {
                    TipoInventario = Convert.ToInt32(TipoArticuloSeleccionado.Codigo),
                    Codigo = Codigo,
                    Nombre = Nombre,
                    Descripcion = Nombre,
                    CantidadArticulo = Convert.ToInt32(Cantidad),
                    Cantidad = Cantidad.ToString(),
                    IdIVA = IVASeleccionado.Codigo,
                    IdICE = ICESeleccionado.Codigo,
                    RucEmpresa = RucEmpresa,
                    PrecioUnidadBase = PrecioUnidadBase.ToString("N2"),
                    SubtotalDocePorciento = SubtotalDocePorciento,
                    SubtotalCeroPorciento = SubtotalCeroPorciento,
                    SubtotalSinImpuesto = SubtotalSinImpuesto,
                    SubtotalNoObjetoIva = SubtotalNoObjetoIva,
                    SubtotalExentoIva = SubtotalExentoIva,
                    IvaCalculadoArticulo = IvaCalculado,
                    IceCalculadoArticulo = IceCalculado,
                    PrecioFinal = PrecioFinal,
                    ImpuestoIVA = IVASeleccionado,
                    ImpuestoICE = ICESeleccionado
                };

                var info = await m_articuloService.GuardarArticulo(Articulo);
                var mensaje = info.Mensaje;

                if (mensaje == "registrado")
                {
                    await m_navigationService.NavigateBackModalAsync();
                    await m_navigationService.NavigateBackModalAsync(Articulo);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al crear el cliente", "Aceptar");
                }
            }
            
        }

        private bool ValidarCamposVacios()
        {
            var esVacio = false;

            if (TipoArticuloSeleccionado == null || Codigo == null || Nombre == null || Cantidad == 0
                || IVASeleccionado == null || ICESeleccionado == null || RucEmpresa == null || PrecioUnidadBase == 0 || PrecioFinal == 0)
            {
                esVacio = true;
            }

            return esVacio;
        }

        #endregion
    }
}
