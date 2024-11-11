using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Services
{
    public class ProfileService : IProfileService
    {
        #region Campos

        private readonly IUsuarioService m_usuarioService;

        #endregion

        #region Constructores

        public ProfileService(IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            m_usuarioService = usuarioService;
        }

        #endregion

        #region IProfileService

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subClaims = subject.Claims.Where(c => c.Type == "sub");

            if (!subClaims.Any())
            {
                throw new ArgumentException("Identificador de sujeto invalido");
            }

            var usuarioId = Converter.ToInt32(subClaims.First().Value);

            if (!usuarioId.HasValue)
            {
                throw new ArgumentException("Identificador de sujeto invalido");
            }

            var usuario = await m_usuarioService.ObtenerUsuarioAsync(usuarioId.Value);

            if (usuario == null)
            {
                throw new ArgumentException("Identificador de sujeto invalido");
            }

            var reclamaciones = UsuarioAyudante.ObtenerReclamaciones(usuario);

            var subClaimsData = subject.Claims.Where(c => c.Type == ClaimTypes.UserData);

            if (subClaimsData.Any())
            {
                reclamaciones.Add(subClaimsData.First());
            }

            context.IssuedClaims = reclamaciones.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            context.IsActive = false;

            var subClaims = subject.Claims.Where(c => c.Type == "sub");

            if (!subClaims.Any())
            {
                return;
            }

            var usuarioId = Converter.ToInt32(subClaims.First().Value);

            if (!usuarioId.HasValue)
            {
                return;
            }

            var usuario = await m_usuarioService.ObtenerUsuarioAsync(usuarioId.Value);

            if (usuario == null)
            {
                return;
            }

            context.IsActive = (usuario.EstaActivo() || usuario.EstaActivoTemporal());
        }

        #endregion
    }
}
