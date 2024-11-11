using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IRequestProvider m_requestProvider;
        private readonly IIdentitySettings m_settings;
        private string m_codeVerifier;

        public IdentityService(IRequestProvider requestProvider, IIdentitySettings settings)
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
            m_codeVerifier = string.Empty;
        }

        private string CreateAuthorizeUri(string authorizeEndpoint, IDictionary<string, string> values)
        {
            var authorizeUri = new Uri(authorizeEndpoint);

            var keysValues = values
                .Select(kvp => string.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value)))
                .ToArray();

            var queryString = string.Join("&", keysValues);

            return string.Format("{0}?{1}", authorizeUri.AbsoluteUri, queryString);
        }

        public string CreateAuthorizationRequest()
        {
            m_codeVerifier = CryptoHelper.CreateUniqueId();

            var codeChallenge = CryptoHelper.CreateCodeChallenge(m_codeVerifier);

            var dic = new Dictionary<string, string>();
            dic.Add("client_id", m_settings.ClientId);
            dic.Add("client_secret", m_settings.ClientSecret);
            dic.Add("response_type", "code id_token");
            dic.Add("scope", "openid profile Servicios.Api.Facturacion offline_access");
            dic.Add("redirect_uri", m_settings.Callback);
            dic.Add("nonce", Guid.NewGuid().ToString("N"));
            dic.Add("code_challenge", codeChallenge);
            dic.Add("code_challenge_method", "S256");

            var currentCSRFToken = Guid.NewGuid().ToString("N");
            dic.Add("state", currentCSRFToken);

            var authorizeUri = CreateAuthorizeUri(m_settings.AuthorizeEndpoint, dic);

            return authorizeUri;
        }

        public string CreateLogoutRequest(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            return string.Format(
                "{0}?id_token_hint={1}&post_logout_redirect_uri={2}",
                m_settings.LogoutEndpoint,
                token,
                WebUtility.UrlEncode(m_settings.LogoutCallback)
            );
        }

        public async Task<UserToken> GetTokenAsync(string code)
        {
            var data = string.Format(
                "grant_type=authorization_code&code={0}&redirect_uri={1}&code_verifier={2}",
                code,
                WebUtility.UrlEncode(m_settings.Callback),
                m_codeVerifier
            );

            var token = await m_requestProvider.PostAsync<UserToken>(
                m_settings.TokenEndpoint,
                data,
                m_settings.ClientId,
                m_settings.ClientSecret
            );

            return token;
        }
    }
}
