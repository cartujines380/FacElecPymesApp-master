using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity
{
    public class IdentitySettings : IIdentitySettings
    {
        private readonly IGlobalIdentitySettings m_globalSettings;

        private string m_registerWebsite;
        private string m_logoutCallback;
        private string m_errorCallback;
        private string m_authorizeEndpoint;
        private string m_userInfoEndpoint;
        private string m_tokenEndpoint;
        private string m_logoutEndpoint;
        private string m_callback;

        public IdentitySettings(IGlobalIdentitySettings globalSettings)
        {
            m_globalSettings = globalSettings ?? throw new ArgumentNullException(nameof(globalSettings));

            Inicializar();
        }

        private string ExtractBaseUri(string uriValue)
        {
            var uri = new Uri(uriValue);
            var baseUri = uri.GetLeftPart(UriPartial.Authority);

            return baseUri;
        }

        private void Inicializar()
        {
            m_registerWebsite = $"{m_globalSettings.EndpointBase}/Account/Register";
            m_logoutCallback = $"{m_globalSettings.EndpointBase}/xamarincallback/Account/Redirecting";
            m_errorCallback = "xamarincallbackError";
            m_authorizeEndpoint = $"{m_globalSettings.EndpointBase}/connect/authorize";
            m_userInfoEndpoint = $"{m_globalSettings.EndpointBase}/connect/userinfo";
            m_tokenEndpoint = $"{m_globalSettings.EndpointBase}/connect/token";
            m_logoutEndpoint = $"{m_globalSettings.EndpointBase}/connect/endsession";
            m_callback = $"{ExtractBaseUri(m_globalSettings.EndpointBase)}/xamarincallback";
        }

        public string ClientId
        {
            get
            {
                return m_globalSettings.ClientId;
            }
        }

        public string ClientSecret
        {
            get
            {
                return m_globalSettings.ClientSecret;
            }
        }

        public string RegisterWebsite
        {
            get
            {
                return m_registerWebsite;
            }
        }

        public string LogoutCallback
        {
            get
            {
                return m_logoutCallback;
            }
        }

        public string ErrorCallback
        {
            get
            {
                return m_errorCallback;
            }
        }

        public string AuthorizeEndpoint
        {
            get
            {
                return m_authorizeEndpoint;
            }
        }

        public string UserInfoEndpoint
        {
            get
            {
                return m_userInfoEndpoint;
            }
        }

        public string TokenEndpoint
        {
            get
            {
                return m_tokenEndpoint;
            }
        }

        public string LogoutEndpoint
        {
            get
            {
                return m_logoutEndpoint;
            }
        }

        public string Callback
        {
            get
            {
                return m_callback;
            }
        }
    }
}
