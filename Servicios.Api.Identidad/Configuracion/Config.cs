using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Configuracion
{
    public static class Config
    {
        private const string FACTURACION_API = "Servicios.Api.Facturacion";
        public const string XAMARIN_CLIENT_KEY = "XamarinClient";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(FACTURACION_API, "API Web de servicios de facturación electrónica")
            };

        public static IEnumerable<Client> Clients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientName = "Cliente API Web de servicios de facturación electrónica",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { FACTURACION_API }
                },
                new Client
                {
                    ClientId = "xamarin",
                    ClientName = "Cliente aplicacion movil de facturación electrónica",
                    AllowedGrantTypes = GrantTypes.Hybrid,                    
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //RedirectUris = { "http://localhost:5105/xamarincallback" },
                    RedirectUris = { $"{clientsUrl[XAMARIN_CLIENT_KEY]}/xamarincallback" },
                    RequireConsent = false,
                    RequirePkce = true,
                    //PostLogoutRedirectUris = { $"http://localhost:5105/xamarincallback/Account/Redirecting" },
                    PostLogoutRedirectUris = { $"{clientsUrl[XAMARIN_CLIENT_KEY]}/xamarincallback/Account/Redirecting" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        FACTURACION_API
                    },
                    //Allow requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                },
            };
        }
    }
}
