using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Helpers;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models.Account;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Campos

        private readonly IIdentityServerInteractionService m_interaction;
        private readonly IClientStore m_clientStore;
        private readonly IAuthenticationSchemeProvider m_schemeProvider;
        private readonly IEventService m_events;
        private readonly IRegistroUsuarioService m_registroUsuarioService;
        private readonly IAutenticacionService m_autenticacionService;
        private readonly IUsuarioService m_usuarioService;

        #endregion

        #region Constructores

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IRegistroUsuarioService registroUsuarioService,
            IAutenticacionService autenticacionService,
            IUsuarioService usuarioService
        )
        {
            m_interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            m_clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
            m_schemeProvider = schemeProvider ?? throw new ArgumentNullException(nameof(schemeProvider));
            m_events = events ?? throw new ArgumentNullException(nameof(events));
            m_registroUsuarioService = registroUsuarioService ?? throw new ArgumentNullException(nameof(registroUsuarioService));
            m_autenticacionService = autenticacionService ?? throw new ArgumentNullException(nameof(autenticacionService));
            m_usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        }

        #endregion

        #region Metodos privados

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await m_interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await m_schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = (context.IdP == IdentityServerConstants.LocalIdentityProvider);

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await m_schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;

            if (context?.Client.ClientId != null)
            {
                var client = await m_clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;

            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await m_interaction.GetLogoutContextAsync(logoutId);

            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await m_interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);

                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await m_interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        private async Task<string> GetCookieAuthenticationSchemeAsync()
        {
            var options = HttpContext.RequestServices.GetRequiredService<IdentityServerOptions>();

            if (options.Authentication.CookieAuthenticationScheme != null)
            {
                return options.Authentication.CookieAuthenticationScheme;
            }

            var scheme = await m_schemeProvider.GetDefaultAuthenticateSchemeAsync();

            if (scheme == null)
            {
                throw new InvalidOperationException("No DefaultAuthenticateScheme found or no CookieAuthenticationScheme configured on IdentityServerOptions.");
            }

            return scheme.Name;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var context = await m_interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (button != "login")
            {
                if (context != null)
                {
                    await m_interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var response = await m_registroUsuarioService.AutenticarAsync(model.Username, model.Password);

                if (response.Exito)
                {
                    var user = new UserViewModel(response.Usuario);

                    var userLoginSuccessEvent = new UserLoginSuccessEvent(
                        user.Username,
                        user.SubjectId,
                        user.Name,
                        clientId: context?.Client.ClientId
                    );

                    await m_events.RaiseAsync(userLoginSuccessEvent);

                    var esquemaAutenticacion = await GetCookieAuthenticationSchemeAsync();

                    await m_autenticacionService.IniciarSesionAsync(
                        esquemaAutenticacion,
                        response.Usuario,
                        (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    );

                    var usuario = await m_usuarioService.ObtenerUsuarioAsync(response.Usuario.Id);

                    var retornoUrl = usuario.EstaActivo()
                        ? model.ReturnUrl
                        : (
                            usuario.EstaActivoTemporal()
                            ? Url.Action(nameof(ChangePassword), new { url = model.ReturnUrl })
                            : string.Empty
                        );

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            return this.LoadingPage("Redirect", retornoUrl);
                        }

                        return Redirect(retornoUrl);
                    }

                    if (Url.IsLocalUrl(retornoUrl))
                    {
                        return Redirect(retornoUrl);
                    }
                    else if (string.IsNullOrEmpty(retornoUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        throw new Exception("URL de retorno invalido");
                    }
                }

                var userLoginFailEvent = new UserLoginFailureEvent(
                    model.Username,
                    "Credenciales invalidas",
                    clientId: context?.Client.ClientId
                );

                await m_events.RaiseAsync(userLoginFailEvent);

                ModelState.AddModelError(string.Empty, "Nombre o contraseña de usuario invalidas");
            }

            var vm = await BuildLoginViewModelAsync(model);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                return await Logout(vm);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                var esquemaAutenticacion = await GetCookieAuthenticationSchemeAsync();

                await m_autenticacionService.CerrarSesionAsync(esquemaAutenticacion);

                await m_events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RecuperarPassword(string url)
        {
            var model = new RecuperarPasswordViewModel
            {
                ReturnUrl = url
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecuperarPassword(RecuperarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await m_registroUsuarioService.ResetearClaveAsync(model.Username);

            if (!response.Exito)
            {
                foreach (var validacion in response.Validaciones)
                {
                    ModelState.TryAddModelError(string.Empty, validacion);
                }

                return View(model);
            }

            return RedirectToAction(nameof(RecuperarPasswordConfirmation), new { url = model.ReturnUrl });
        }

        [HttpGet]
        public IActionResult RecuperarPasswordConfirmation(string url)
        {
            var model = new RecuperarPasswordViewModel
            {
                ReturnUrl = url
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string url)
        {
            var model = new ChangePasswordViewModel
            {
                ReturnUrl = url
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await m_registroUsuarioService.CambiarClaveAsync(
                model.Username,
                model.OldPassword,
                model.NewPassword,
                model.NewPassword
            );

            if (!response.Exito)
            {
                foreach (var validacion in response.Validaciones)
                {
                    ModelState.TryAddModelError(string.Empty, validacion);
                }

                return View(model);
            }

            return RedirectToAction(nameof(ChangePasswordConfirmation), new { url = model.ReturnUrl });
        }

        [HttpGet]
        public IActionResult ChangePasswordConfirmation(string url)
        {
            var model = new ChangePasswordViewModel
            {
                ReturnUrl = url
            };

            return View(model);
        }
    }
}
