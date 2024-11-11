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
    public class ModalExportacionViewModel : ViewModelBase
    {
        #region Campos

        public int m_codigoPaisAdquisicion;
        public int m_codigoPaisOrigen;
        public int m_codigoPaisDestino;
        public string m_definicionTermino;
        public string m_defTerSinImpuesto;
        public string m_puertoEmbarque;
        public string m_lugarConvenio;
        public string m_puertoDestino;
        public decimal m_fleteInternacional;
        public decimal m_seguroInternacional;
        public decimal m_gastosAduaneros;
        public decimal m_gastosTransporte;
        public ObservableCollection<CatalogoModel> m_paisAdquisicion;
        public CatalogoModel m_paisAdquisicionSeleccionado;
        public ObservableCollection<CatalogoModel> m_paisOrigen;
        public CatalogoModel m_paisOrigenSeleccionado;
        public ObservableCollection<CatalogoModel> m_paisDestino;
        public CatalogoModel m_paisDestinoSeleccionado;

        private readonly ICatalogoService m_catalogoService;

        #endregion

        #region Propiedades

        public bool EsEdicion { get; set; }

        public string TipoFactura { get; set; }

        public string DefinicionTermino
        {
            get
            {
                return m_definicionTermino;
            }
            set
            {
                m_definicionTermino = value;
                RaisePropertyChanged(() => DefinicionTermino);
            }
        }

        public string DefTerSinImpuesto
        {
            get
            {
                return m_defTerSinImpuesto;
            }
            set
            {
                m_defTerSinImpuesto = value;
                RaisePropertyChanged(() => DefTerSinImpuesto);
            }
        }

        public string PuertoEmbarque
        {
            get
            {
                return m_puertoEmbarque;
            }
            set
            {
                m_puertoEmbarque = value;
                RaisePropertyChanged(() => PuertoEmbarque);
            }
        }

        public ObservableCollection<CatalogoModel> PaisAdquisicion
        {
            get
            {
                return m_paisAdquisicion;
            }
            set
            {
                m_paisAdquisicion = value;
                RaisePropertyChanged(() => PaisAdquisicion);
            }
        }

        public CatalogoModel PaisAdquisicionSeleccionado
        {
            get
            {
                return m_paisAdquisicionSeleccionado;
            }
            set
            {
                m_paisAdquisicionSeleccionado = value;
                RaisePropertyChanged(() => PaisAdquisicionSeleccionado);
            }
        }

        public ObservableCollection<CatalogoModel> PaisOrigen
        {
            get
            {
                return m_paisOrigen;
            }
            set
            {
                m_paisOrigen = value;
                RaisePropertyChanged(() => PaisOrigen);
            }
        }

        public CatalogoModel PaisOrigenSeleccionado
        {
            get
            {
                return m_paisOrigenSeleccionado;
            }
            set
            {
                m_paisOrigenSeleccionado = value;
                RaisePropertyChanged(() => PaisOrigenSeleccionado);
            }
        }

        public string LugarConvenio
        {
            get
            {
                return m_lugarConvenio;
            }
            set
            {
                m_lugarConvenio = value;
                RaisePropertyChanged(() => LugarConvenio);
            }
        }

        public string PuertoDestino
        {
            get
            {
                return m_puertoDestino;
            }
            set
            {
                m_puertoDestino = value;
                RaisePropertyChanged(() => PuertoDestino);
            }
        }

        public ObservableCollection<CatalogoModel> PaisDestino
        {
            get
            {
                return m_paisDestino;
            }
            set
            {
                m_paisDestino = value;
                RaisePropertyChanged(() => PaisDestino);
            }
        }

        public CatalogoModel PaisDestinoSeleccionado
        {
            get
            {
                return m_paisDestinoSeleccionado;
            }
            set
            {
                m_paisDestinoSeleccionado = value;
                RaisePropertyChanged(() => PaisDestinoSeleccionado);
            }
        }

        public decimal FleteInternacional
        {
            get
            {
                return m_fleteInternacional;
            }
            set
            {
                m_fleteInternacional = value;
                RaisePropertyChanged(() => FleteInternacional);
            }
        }

        public decimal SeguroInternacional
        {
            get
            {
                return m_seguroInternacional;
            }
            set
            {
                m_seguroInternacional = value;
                RaisePropertyChanged(() => SeguroInternacional);
            }
        }

        public decimal GastosAduaneros
        {
            get
            {
                return m_gastosAduaneros;
            }
            set
            {
                m_gastosAduaneros = value;
                RaisePropertyChanged(() => GastosAduaneros);
            }
        }

        public decimal GastosTransporte
        {
            get
            {
                return m_gastosTransporte;
            }
            set
            {
                m_gastosTransporte = value;
                RaisePropertyChanged(() => GastosTransporte);
            }
        }

        public int CodigoPaisAdquisicion
        {
            get
            {
                return m_codigoPaisAdquisicion;
            }
            set
            {
                m_codigoPaisAdquisicion = value;
                RaisePropertyChanged(() => CodigoPaisAdquisicion);
            }
        }

        public int CodigoPaisOrigen
        {
            get
            {
                return m_codigoPaisOrigen;
            }
            set
            {
                m_codigoPaisOrigen = value;
                RaisePropertyChanged(() => CodigoPaisOrigen);
            }
        }

        public int CodigoPaisDestino
        {
            get
            {
                return m_codigoPaisDestino;
            }
            set
            {
                m_codigoPaisDestino = value;
                RaisePropertyChanged(() => CodigoPaisDestino);
            }
        }

        #endregion

        #region Constructor

        public ModalExportacionViewModel(ICatalogoService catalogoService)
        {
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());
        public ICommand GuardarExportacionComannd => new Command(async () => await GuardarExportacionAsync());

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            var tipoFactura = navigationData as FacturaModel;
            TipoFactura = tipoFactura.TipoFactura;
            EsEdicion = tipoFactura.EsEdicion;

            LlenarModal(tipoFactura);

            ShowLoading();
            var _pais = await m_catalogoService.ObtenerCatalogoPais(2);
            HideLoading();
           
            var pais = _pais.OrderBy(x => x.Detalle);
            var oc = new ObservableCollection<CatalogoModel>();
            pais.ForEach(x => oc.Add(x));
            PaisAdquisicion = oc;
            PaisDestino = oc;
            PaisOrigen = oc;

            var _paisAdquisicion = PaisAdquisicion.Where(X => X.Codigo == tipoFactura.PaisAdquisicion.Codigo).FirstOrDefault();
            var _paisDestino = PaisDestino.Where(X => X.Codigo == tipoFactura.PaisOrigen.Codigo).FirstOrDefault();
            var _paisOrigen = PaisOrigen.Where(X => X.Codigo == tipoFactura.PaisDestino.Codigo).FirstOrDefault();

            CodigoPaisAdquisicion = PaisAdquisicion.IndexOf(_paisAdquisicion);
            CodigoPaisOrigen = PaisDestino.IndexOf(_paisDestino);
            CodigoPaisDestino = PaisOrigen.IndexOf(_paisOrigen);

            await base.InitializeAsync(navigationData);
        }

        private void LlenarModal(FacturaModel facturaModel)
        {
            DefinicionTermino = facturaModel.DefinicionTermino;
            DefTerSinImpuesto = facturaModel.DefTerminoSinImpuesto;
            PuertoEmbarque = facturaModel.PuertoEmbarque;
            PaisAdquisicionSeleccionado = facturaModel.PaisAdquisicion;
            PaisOrigenSeleccionado = facturaModel.PaisOrigen;
            FleteInternacional = facturaModel.FleteInternacional;
            LugarConvenio = facturaModel.LugarConvenio;
            SeguroInternacional = facturaModel.SeguroInternacional;
            PuertoDestino = facturaModel.PuertoDestino;
            GastosAduaneros = facturaModel.GastosAduaneros;
            PaisDestinoSeleccionado = facturaModel.PaisDestino;
            GastosTransporte = facturaModel.GastosTransporte;
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task GuardarExportacionAsync()
        {
            if (ValidarCamposVacios())
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Llene los campos obligatorios", "Aceptar");
            }
            else
            {
                var exportacion = new FacturaModel
                {
                    DefinicionTermino = DefinicionTermino,
                    DefTerminoSinImpuesto = DefTerSinImpuesto,
                    PuertoEmbarque = PuertoEmbarque,
                    PaisAdquisicion = PaisAdquisicionSeleccionado,
                    PaisOrigen = PaisOrigenSeleccionado,
                    FleteInternacional = FleteInternacional,
                    LugarConvenio = LugarConvenio,
                    SeguroInternacional = SeguroInternacional,
                    PuertoDestino = PuertoDestino,
                    GastosAduaneros = GastosAduaneros,
                    PaisDestino = PaisDestinoSeleccionado,
                    GastosTransporte = GastosTransporte,
                    EsExportacion = true
                };

                exportacion.TipoFactura = TipoFactura;

                if (!EsEdicion)
                {
                    await m_navigationService.NavigateBackModalAsync();
                }

                await m_navigationService.NavigateBackModalAsync(exportacion);
            }
        }

        private bool ValidarCamposVacios()
        {
            var esVacio = false;

            if (DefinicionTermino == null || DefTerSinImpuesto == null || PuertoEmbarque == null || PaisAdquisicionSeleccionado == null || PaisOrigenSeleccionado == null ||
                LugarConvenio == null || PuertoDestino == null || PaisDestinoSeleccionado == null)
            {
                esVacio = true;
            }

            return esVacio;
        }

        #endregion

    }
}
