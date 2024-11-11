using System;
using TinyIoC;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.User;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Navigation;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Values;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Bodega;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.UnidadMedida;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.PDF;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Infrastructure
{
    public class Bootstrapper
    {
        #region Campos

        private static readonly Bootstrapper m_instance = new Bootstrapper();

        private readonly TinyIoCContainer m_container;

        #endregion

        #region Propiedades

        public static Bootstrapper Instance
        {
            get
            {
                return m_instance;
            }
        }

        public TinyIoCContainer Container
        {
            get
            {
                return m_container;
            }
        }

        public bool ValidateSslCertificate
        {
            get
            {
                var requestSettings = m_container.Resolve<IRequestSettings>();

                return requestSettings.ValidateSslCertificate;
            }
        }

        #endregion

        #region Constructores

        private Bootstrapper()
        {
            m_container = TinyIoCContainer.Current;
        }

        #endregion

        #region Metodos privados

        private void RegisterDependencies()
        {
            //View models
            m_container.Register<IniciarSesionViewModel>();
            m_container.Register<PrincipalViewModel>();
            m_container.Register<FacturaViewModel>();
            m_container.Register<ModalTipoFacturaViewModel>();
            m_container.Register<ModalEmpresaViewModel>();
            m_container.Register<ModalClienteViewModel>();
            m_container.Register<ModalNumeroFacturaViewModel>();
            m_container.Register<ModalFormaPagoDetalleViewModel>();
            m_container.Register<ModalDetalleViewModel>();
            m_container.Register<ModalTipoFacturaViewModel>();
            m_container.Register<MenuDetailViewModel>();
            m_container.Register<ConsultaComprobanteFiltroViewModel>();
            m_container.Register<ConsultaComprobanteViewModel>();
            m_container.Register<ModalSinInternetViewModel>();

            //Servicios
            m_container.Register<IRequestProvider, RequestProvider>();
            m_container.Register<IIdentitySettings, IdentitySettings>();
            m_container.Register<IIdentityService, IdentityService>();
            m_container.Register<INavigationService, NavigationService>();
            m_container.Register<ISettingsService, SettingsService>();
            m_container.Register<IGlobalIdentitySettings, SettingsService>();
            m_container.Register<ICatalogoService, CatalogoService>();
            m_container.Register<IEstablecimientoService, EstablecimientoService>();
            m_container.Register<IPDFService, PDFService>();
            m_container.Register<IClienteService, ClienteService>();
            m_container.Register<IFacturaService, FacturaService>();
            m_container.Register<IArticuloService, ArticuloService>();
            m_container.Register<IBodegaService, BodegaService>();
            m_container.Register<IUnidadMedidaService, UnidadMedidaService>();
            m_container.Register<IComprobanteService, ComprobanteService>();
            m_container.Register<IValuesService, ValuesService>();
            m_container.Register<IUserService, UserService>();
            m_container.Register<IRequestSettings, SettingsService>();
        }

        #endregion

        #region Metodos publicos

        public void Run()
        {
            RegisterDependencies();
        }

        public void Stop()
        {

        }

        #endregion
    }
}
