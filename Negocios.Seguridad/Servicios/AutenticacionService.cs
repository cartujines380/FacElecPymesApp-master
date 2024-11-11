using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Json;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class AutenticacionService : IAutenticacionService
    {
        #region Campos

        private readonly IHttpContextAccessor m_httpContextAccessor;
        private readonly IJsonSerializer m_jsonSerializer;

        private UsuarioData m_usuarioDataCache;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public AutenticacionService(
            IHttpContextAccessor httpContextAccessor,
            IJsonSerializer jsonSerializer
        )
        {
            m_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            m_jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        #endregion

        #region Metodos privados

        private ClaimsIdentity CrearIdentidad(string esquemaAutenticacion, UsuarioData usuario)
        {
            var strUsuario = m_jsonSerializer.Serialize<UsuarioData>(usuario);

            var retorno = new ClaimsIdentity(esquemaAutenticacion, JwtClaimTypes.Name, JwtClaimTypes.Role);

            retorno.AddClaim(new Claim(JwtClaimTypes.Name, usuario.Nombre));
            retorno.AddClaim(new Claim(JwtClaimTypes.Subject, usuario.Id.ToString()));
            retorno.AddClaim(new Claim(ClaimTypes.UserData, strUsuario, ClaimValueTypes.String));

            return retorno;
        }

        private AuthenticationProperties CrearPropiedades(UsuarioData usuario, bool persistente)
        {
            var data = new Dictionary<string, string>
            {
                { "userName", usuario.NombreUsuario }
            };

            var ahora = DateTime.UtcNow.ToLocalTime();

            var propiedades = new AuthenticationProperties(data)
            {
                AllowRefresh = true,
                IssuedUtc = ahora
                //RedirectUri = <string>
            };

            if (persistente)
            {
                propiedades.IsPersistent = true;
                propiedades.ExpiresUtc = ahora.AddDays(30);
            }
            else
            {
                propiedades.ExpiresUtc = ahora.AddMinutes(60);
            }

            return propiedades;
        }

        private UsuarioData ObtenerUsuarioAutenticadoPorIdentidad(ClaimsIdentity identidad)
        {
            var usrDataClaim = identidad.FindFirst(ClaimTypes.UserData);

            if (usrDataClaim == null)
            {
                return null;
            }

            var strUsuarioData = usrDataClaim.Value;

            if (string.IsNullOrWhiteSpace(strUsuarioData))
            {
                return null;
            }

            return m_jsonSerializer.Deserialize<UsuarioData>(strUsuarioData);
        }

        #endregion

        #region IAutenticacionService

        public void IniciarSesion(UsuarioData usuario, bool persistente)
        {
            IniciarSesion(CookieAuthenticationDefaults.AuthenticationScheme, usuario, persistente);
        }

        public void IniciarSesion(string esquemaAutenticacion, UsuarioData usuario, bool persistente)
        {
            AsyncHelper.RunSync(() => IniciarSesionAsync(esquemaAutenticacion, usuario, persistente));
        }

        public async Task IniciarSesionAsync(UsuarioData usuario, bool persistente)
        {
            await IniciarSesionAsync(CookieAuthenticationDefaults.AuthenticationScheme, usuario, persistente);
        }

        public async Task IniciarSesionAsync(string esquemaAutenticacion, UsuarioData usuario, bool persistente)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            var identidad = CrearIdentidad(esquemaAutenticacion, usuario);
            var principal = new ClaimsPrincipal(identidad);
            var propiedades = CrearPropiedades(usuario, persistente);

            await m_httpContextAccessor.HttpContext.SignInAsync(esquemaAutenticacion, principal, propiedades);

            m_usuarioDataCache = usuario;
        }

        public void CerrarSesion()
        {
            CerrarSesion(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public void CerrarSesion(string esquemaAutenticacion)
        {
            AsyncHelper.RunSync(() => CerrarSesionAsync(esquemaAutenticacion));
        }

        public async Task CerrarSesionAsync()
        {
            await CerrarSesionAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task CerrarSesionAsync(string esquemaAutenticacion)
        {
            m_usuarioDataCache = null;
            await m_httpContextAccessor.HttpContext.SignOutAsync(esquemaAutenticacion);
        }

        public UsuarioData ObtenerUsuarioDataAutenticado()
        {
            if (m_usuarioDataCache != null)
            {
                return m_usuarioDataCache;
            }

            var identidad = m_httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;

            if ((identidad == null) || (!identidad.IsAuthenticated))
            {
                return null;
            }

            var usuario = ObtenerUsuarioAutenticadoPorIdentidad(identidad);

            if (usuario != null)
            {
                m_usuarioDataCache = usuario;
            }

            return m_usuarioDataCache;
        }

        public Task<UsuarioData> ObtenerUsuarioDataAutenticadoAsync()
        {
            return Task.FromResult(ObtenerUsuarioDataAutenticado());
        }

        public void EstablecerUsuarioDataAutenticado(UsuarioData usuario)
        {
            m_usuarioDataCache = usuario;
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                //
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AutenticacionService()
        {
            Dispose(false);
        }

        #endregion
    }
}
