using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Bodega;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.UnidadMedida;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalDetalleViewModel : ViewModelBase
    {

        #region Campos

        private ObservableCollection<ArticuloModel> m_articulo;
        private ArticuloModel articuloSeleccionado;

        private readonly IBodegaService m_bodegaService;
        private readonly IUnidadMedidaService m_unidadMedidaService;
        private readonly ICatalogoService m_catalogoService;

        #endregion

        #region Propiedades

        public string Ruc { get; set; }

        public ObservableCollection<ArticuloModel> ArticuloData { get; set; }

        public ObservableCollection<ArticuloModel> Articulo
        {
            get
            {
                return m_articulo;
            }
            set
            {
                m_articulo = value;
                RaisePropertyChanged(() => Articulo);
            }
        }

        public ArticuloModel ArticuloSeleccionado
        {
            get
            {
                return articuloSeleccionado;
            }
            set
            {
                articuloSeleccionado = value;
                _ = Seleccion();
            }
        }

        #endregion

        #region Constructor

        public ModalDetalleViewModel(IBodegaService bodegaService, 
            IUnidadMedidaService unidadMedidaService,
            ICatalogoService catalogoService)
        {
            m_bodegaService = bodegaService ?? throw new ArgumentNullException(nameof(bodegaService));
            m_unidadMedidaService = unidadMedidaService ?? throw new ArgumentNullException(nameof(unidadMedidaService));
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand CrearArticuloComannd => new Command(async () => await CrearArticuloAsync());

        public ICommand BuscarCommand => new Command<string>(BuscarAsync);

        #endregion

        #region Metodos

        public override Task InitializeAsync(object navigationData)
        {
            var request = navigationData as RequestModel;
            Ruc = request.RucEmpresa;
            Articulo = request.Articulo;
            ArticuloData = request.Articulo;
            return base.InitializeAsync(navigationData);
        }

        private async Task Seleccion()
        {
            ShowLoading();
            var bodega = await m_bodegaService.ObtenerBodegaPorEmpresa(Ruc);
            articuloSeleccionado.ListBodegas = bodega;
            var unidaMedida = await m_unidadMedidaService.ObtenerUnidadMedidaPorArticulo(Ruc, articuloSeleccionado.Codigo);
            articuloSeleccionado.ListUnidadMedida = unidaMedida;
            var impuestoIva = await m_catalogoService.ObtenerImpuestoPorCodigo("2016");
            articuloSeleccionado.ListImpuestoIVA = impuestoIva;
            var impuestoIce = await m_catalogoService.ObtenerImpuestoPorCodigo("2019");
            articuloSeleccionado.ListImpuestoICE = impuestoIce;
            HideLoading();

            await m_navigationService.NavigateToModalAsync<ModalDetalleInformacionViewModel>(articuloSeleccionado);
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task CrearArticuloAsync()
        {
            ShowLoading();
            var tipoArticulo = await m_catalogoService.ObtenerCatalogo("tbl_TipoArticulo");
            HideLoading();

            var request = new ArticuloRequest
            {
                Articulo = tipoArticulo,
                RucEmpresa = Ruc
            };

            await m_navigationService.NavigateToModalAsync<ModalDetalleNuevoViewModel>(request);
        }

        private void BuscarAsync(string text)
        {
            if (text != "")
            {
                var articulos = ArticuloData.Where(x => x.Nombre.ToUpper().Contains(text.ToUpper()));
                var myObservableCollection = new ObservableCollection<ArticuloModel>(articulos);
                Articulo = myObservableCollection;
            }
            else
            {
                Articulo = ArticuloData;
            }
        }

        #endregion
    }
}
