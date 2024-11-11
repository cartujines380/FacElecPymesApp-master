using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;

using Sipecom.FactElec.Pymes.Entidades.Seguridad;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Services
{
    internal static class UsuarioAyudante
    {
        internal static ICollection<Claim> ObtenerReclamaciones(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Subject, Converter.ToString(usuario.Id)),
                new Claim(JwtClaimTypes.Name, usuario.NombreCompleto ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Nombre ?? string.Empty)
            };

            var persona = usuario as Persona;

            if (persona != null)
            {
                claims.AddRange(new Claim[]
                {
                    new Claim(JwtClaimTypes.GivenName, persona.PrimerNombre ?? string.Empty),
                    new Claim(JwtClaimTypes.MiddleName, persona.SegundoNombre ?? string.Empty),
                    new Claim(JwtClaimTypes.FamilyName, persona.PrimerApellido ?? string.Empty)
                });
            }

            return claims;
        }
    }
}
