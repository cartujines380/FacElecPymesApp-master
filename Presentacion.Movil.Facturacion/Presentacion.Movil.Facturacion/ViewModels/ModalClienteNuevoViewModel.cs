using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalClienteNuevoViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<CatalogoModel> m_tipoEntidad;
        private ObservableCollection<CatalogoModel> m_tipoId;
        private ObservableCollection<CatalogoModel> m_provincia;
        private ObservableCollection<CatalogoModel> m_ciudad;
        private CatalogoModel m_tipoEntidadSeleccionado;
        private CatalogoModel m_tipoIdSeleccionado;
        private CatalogoModel m_provinciaSeleccionado;
        private CatalogoModel m_ciudadSeleccionado;
        public string m_numeroID;
        public string m_razonSocial;
        public string m_email;
        public string m_telefono;
        public string m_celular;
        public string m_direccion;

        private readonly ICatalogoService m_catalogoService;
        private readonly IClienteService m_clienteService;

        #endregion

        #region Propiedades

        public string RucEmpresa { get; set; }

        public ClienteModel Cliente { get; set; }

        public ObservableCollection<CatalogoModel> TipoEntidad
        {
            get
            {
                return m_tipoEntidad;
            }
            set
            {
                m_tipoEntidad = value;
                RaisePropertyChanged(() => TipoEntidad);
            }
        }

        public CatalogoModel TipoEntidadSeleccionado
        {
            get
            {
                return m_tipoEntidadSeleccionado;
            }
            set
            {
                m_tipoEntidadSeleccionado = value;
                RaisePropertyChanged(() => TipoEntidadSeleccionado);
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

        public string NumeroID
        {
            get
            {
                return m_numeroID;
            }
            set
            {
                m_numeroID = value;
                RaisePropertyChanged(() => NumeroID);
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

        public string Email
        {
            get
            {
                return m_email;
            }
            set
            {
                m_email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public string Telefono
        {
            get
            {
                return m_telefono;
            }
            set
            {
                m_telefono = value;
                RaisePropertyChanged(() => Telefono);
            }
        }

        public string Celular
        {
            get
            {
                return m_celular;
            }
            set
            {
                m_celular = value;
                RaisePropertyChanged(() => Celular);
            }
        }

        public ObservableCollection<CatalogoModel> Provincia
        {
            get
            {
                return m_provincia;
            }
            set
            {
                m_provincia = value;
                RaisePropertyChanged(() => Provincia);
            }
        }

        public CatalogoModel ProvinciaSeleccionado
        {
            get
            {
                return m_provinciaSeleccionado;
            }
            set
            {
                m_provinciaSeleccionado = value;
                _ = Seleccion();
            }
        }

        public ObservableCollection<CatalogoModel> Ciudad
        {
            get
            {
                return m_ciudad;
            }
            set
            {
                m_ciudad = value;
                RaisePropertyChanged(() => Ciudad);
            }
        }

        public CatalogoModel CiudadSeleccionado
        {
            get
            {
                return m_ciudadSeleccionado;
            }
            set
            {
                m_ciudadSeleccionado = value;
                RaisePropertyChanged(() => CiudadSeleccionado);
            }
        }

        public string Direccion
        {
            get
            {
                return m_direccion;
            }
            set
            {
                m_direccion = value;
                RaisePropertyChanged(() => Direccion);
            }
        }

        #endregion

        #region Constructor

        public ModalClienteNuevoViewModel(ICatalogoService catalogoService,
             IClienteService clienteService)
        {
            m_catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
            m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand GuardarClienteComannd => new Command(async () => await GuardarClienteAsync());

        #endregion

        #region Metodos

        public override async Task InitializeAsync(object navigationData)
        {
            var rucEmpresa = navigationData as string;

            ShowLoading();
            TipoEntidad = await m_catalogoService.ObtenerCatalogoGeneral(1021);
            TipoId = await m_catalogoService.ObtenerCatalogo("tbl_tipoIdentificacion");
            Provincia = await m_catalogoService.ObtenerCatalogoProvinciaCiudad(3, "593");
            HideLoading();

            RucEmpresa = rucEmpresa;
            await base.InitializeAsync(navigationData);
        }

        private async Task Seleccion()
        {
            var idProvincia = ProvinciaSeleccionado.Codigo;

            ShowLoading();
            Ciudad = await m_catalogoService.ObtenerCatalogoProvinciaCiudad(4, idProvincia);
            HideLoading();

        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task GuardarClienteAsync()
        {
            if (ValidarCamposVacios())
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Llene los campos obligatorios", "Aceptar");
            }
            else
            {
                Cliente = new ClienteModel()
                {
                    Celular = Celular ?? "",
                    Ciudad = CiudadSeleccionado != null ? CiudadSeleccionado.Codigo : "",
                    Correo = Email ?? "",
                    Direccion = Direccion ?? "",
                    Estado = "A",
                    Identificacion = NumeroID,
                    IdTipoEntidad = TipoEntidadSeleccionado != null ? TipoEntidadSeleccionado.Codigo : "",
                    IdTipoIdentificacion = TipoIdSeleccionado != null ? TipoIdSeleccionado.Codigo : "",
                    Provincia = ProvinciaSeleccionado != null ? ProvinciaSeleccionado.Codigo : "",
                    RazonSocial = RazonSocial,
                    RucEmpresa = RucEmpresa,
                    RucEmpresaAnterior = RucEmpresa,
                    Telefono = Telefono ?? "",
                    TipoIdentificacion = TipoIdSeleccionado != null ? TipoIdSeleccionado.Detalle : ""
                };

                var info = await m_clienteService.GuardarCliente(Cliente);
                var mensaje = info.Mensaje;

                if (mensaje == "registrado")
                {
                    await m_navigationService.NavigateBackModalAsync();
                    await m_navigationService.NavigateBackModalAsync(Cliente);
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

            if (NumeroID == null || TipoEntidadSeleccionado == null || TipoIdSeleccionado == null || RazonSocial == null)
            {
                esVacio = true;
            }

            return esVacio;
        }

        #endregion
    }
}
