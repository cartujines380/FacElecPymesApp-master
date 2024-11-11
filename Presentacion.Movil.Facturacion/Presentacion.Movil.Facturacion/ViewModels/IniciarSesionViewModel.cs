using IdentityModel.Client;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Validations;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class IniciarSesionViewModel : ViewModelBase
    {
        #region Campos

        private readonly IIdentitySettings m_identitySettings;
        private readonly IIdentityService m_identityService;
        private readonly ISettingsService m_settingsService;

        private ValidatableObject<string> m_usuarioNombre;
        private ValidatableObject<string> m_contrasenia;
        private bool m_esValido;
        private bool m_esLogin;
        private bool m_esBtnLogin;
        private string m_loginUrl;
        bool _conectado;
        bool _sinconexion;

        #endregion

        #region Constructore

        public IniciarSesionViewModel(
            IIdentitySettings identitySettings,
            IIdentityService identityService,
            ISettingsService settingsService
        )
        {
            m_identitySettings = identitySettings ?? throw new ArgumentNullException(nameof(identitySettings));
            m_identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            m_usuarioNombre = new ValidatableObject<string>();
            m_contrasenia = new ValidatableObject<string>();

            AnadirValidaciones();
        }

        #endregion

        #region Propiedades

        public ValidatableObject<string> UsuarioNombre
        {
            get
            {
                return m_usuarioNombre;
            }
            set
            {
                m_usuarioNombre = value;
                RaisePropertyChanged(() => UsuarioNombre);
            }
        }

        public ValidatableObject<string> Contrasenia
        {
            get
            {
                return m_contrasenia;
            }
            set
            {
                m_contrasenia = value;
                RaisePropertyChanged(() => Contrasenia);
            }
        }

        public bool EsValido
        {
            get
            {
                return m_esValido;
            }
            set
            {
                m_esValido = value;
                RaisePropertyChanged(() => EsValido);
            }
        }

        public bool EsLogin
        {
            get
            {
                return m_esLogin;
            }
            set
            {
                m_esLogin = value;
                RaisePropertyChanged(() => EsLogin);
            }
        }

        public bool EsBtnLogin
        {
            get
            {
                return m_esBtnLogin;
            }
            set
            {
                m_esBtnLogin = value;
                RaisePropertyChanged(() => EsBtnLogin);
            }
        }

        public string LoginUrl
        {
            get
            {
                return m_loginUrl;
            }
            set
            {
                m_loginUrl = value;
                RaisePropertyChanged(() => LoginUrl);
            }
        }

        public bool Conectado
        {
            get
            {
                return _conectado;
            }
            set
            {
                _conectado = value;
                RaisePropertyChanged(() => Conectado);
            }
        }

        public bool Sinconexion
        {
            get
            {
                return _sinconexion;
            }
            set
            {
                _sinconexion = value;
                RaisePropertyChanged(() => Sinconexion);
            }
        }

        #endregion

        #region Commands

        public ICommand IniciarSesionCommand => new Command(async () => await IniciarSesionAsync());

        public ICommand ValidarUsuarioNombreCommand => new Command(() => ValidarUsuarioNombre());

        public ICommand ValidarContraseniaCommand => new Command(() => ValidarContrasenia());

        public ICommand NavegarCommand => new Command<string>(async (url) => await NavegarAsync(url));

        #endregion

        #region Metodos y Funciones Publicas

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is CerrarSesionParametros)
            {
                var parametro = navigationData as CerrarSesionParametros;

                if (parametro.CerrarSesion)
                {
                    CerrarSesion();
                }
            }
            else
            {
                await IniciarSesionAsync();
            }

            await base.InitializeAsync(navigationData);
        }

        #endregion

        #region Metodos y Funciones Privadas

        private void CerrarSesion()
        {
            var authIdToken = m_settingsService.AuthIdToken;
            var cerrarSesionUrl = m_identityService.CreateLogoutRequest(authIdToken);

            if (!string.IsNullOrEmpty(cerrarSesionUrl))
            {
                LoginUrl = cerrarSesionUrl;
            }
        }

        private async Task IniciarSesionAsync()
        {
            ShowLoading();

            IsBusy = true;
            EsBtnLogin = false;

            await Task.Delay(1);
            LoginUrl = m_identityService.CreateAuthorizationRequest();

            EsValido = true;
            EsLogin = true;

            IsBusy = false;

            HideLoading();

        }

        private async Task NavegarAsync(string url)
        {
            var unescapedUrl = WebUtility.UrlDecode(url);
            ShowLoading();
            if (unescapedUrl.Equals(m_identitySettings.LogoutCallback))
            {
                m_settingsService.AuthAccessToken = string.Empty;
                m_settingsService.AuthIdToken = string.Empty;
                m_settingsService.UserName = string.Empty;
                await IniciarSesionAsync();
            }
            else if (unescapedUrl.Contains(m_identitySettings.Callback))
            {
                var authResponse = new AuthorizeResponse(url);

                if (!string.IsNullOrWhiteSpace(authResponse.Code))
                {
                    var userToken = await m_identityService.GetTokenAsync(authResponse.Code);


                    var accessToken = userToken.AccessToken;

                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        m_settingsService.AuthAccessToken = accessToken;
                        m_settingsService.AuthIdToken = authResponse.IdentityToken;
                        await m_navigationService.NavigateToAsync<MenuViewModel>();
                        await m_navigationService.RemoveLastFromBackStackAsync();
                    }
                }

            }
            else if (unescapedUrl.Contains(m_identitySettings.ErrorCallback))
            {
                m_settingsService.AuthAccessToken = string.Empty;
                m_settingsService.AuthIdToken = string.Empty;
                m_settingsService.UserName = string.Empty;
                await IniciarSesionAsync();
            }

            HideLoading();

        }

        private bool Validar()
        {
            return ValidarUsuarioNombre() && ValidarContrasenia();
        }

        private bool ValidarUsuarioNombre()
        {
            return m_usuarioNombre.Validate();
        }

        private bool ValidarContrasenia()
        {
            return m_contrasenia.Validate();
        }

        private void AnadirValidaciones()
        {
            m_usuarioNombre.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Nombre de usuario es requerido."
            });

            m_contrasenia.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Contraseña es requerida."
            });
        }

        #endregion
    }
}
