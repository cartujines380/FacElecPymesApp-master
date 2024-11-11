using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings
{
    public class SettingsService : ISettingsService, IGlobalIdentitySettings, IRequestSettings
    {
        private const string CLIENT_ID_KEY = "client_id";
        private const string CLIENT_ID_DEFAULT = "xamarin";
        private const string CLIENT_SECRET_KEY = "client_secret";
        private const string CLIENT_SECRET_DEFAULT = "secret";
        private const string AUTH_ACCESS_TOKEN_KEY = "access_token";

        private const string AUTH_ACCESS_TOKEN_DEFAULT = "";
        private const string AUTH_ID_TOKEN_KEY = "id_token";
        private const string AUTH_ID_TOKEN_DEFAULT = "";
        private const string USER_NAME_TOKEN_KEY = "user_name";
        private const string USER_NAME_TOKEN_DEFAULT = "";
        private const string IDENTITY_ENDPOINT_BASE_KEY = "url_base";
        private static string IDENTITY_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44360" : "https://localhost:44360";
        //private static string IDENTITY_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://cm78twpymes2.domsipecom.com:44360" : "https://api1ampdev.invoicec.com/";
        private const string FACTURACION_ENDPOINT_BASE_KEY = "facturacion_url_base";
        private static string FACTURACION_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44367" : "https://localhost:44367";
        //private static string FACTURACION_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://cm78twpymes2.domsipecom.com:44367" : "https://api2ampdev.invoicec.com/";
        private const string PDF_ENDPOINT_BASE_KEY = "pdf_url_base";
        private static string PDF_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44338/" : "https://localhost:44338/";
        //private static string PDF_ENDPOINT_BASE_DEFAULT = Device.RuntimePlatform == Device.Android ? "https://cm78twpymes2.domsipecom.com:44338/" : "https://api2ampdev.invoicec.com/";

        private const string VALIDATE_SSL_CERTIFICATE_KEY = "val_ssl_cert";
        private const bool VALIDATE_SSL_CERTIFICATE_DEFAULT = false;

        //#if DEBUG
        //        private const bool VALIDATE_SSL_CERTIFICATE_DEFAULT = false;
        //#else
        //        private const bool VALIDATE_SSL_CERTIFICATE_DEFAULT = true;
        //#endif

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public string AuthAccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AUTH_ACCESS_TOKEN_KEY, AUTH_ACCESS_TOKEN_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AUTH_ACCESS_TOKEN_KEY, value);
            }
        }

        public string AuthIdToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AUTH_ID_TOKEN_KEY, AUTH_ID_TOKEN_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AUTH_ID_TOKEN_KEY, value);
            }
        }

        public string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(USER_NAME_TOKEN_KEY, USER_NAME_TOKEN_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(USER_NAME_TOKEN_KEY, value);
            }
        }

        public string IdentityEndpointBase
        {
            get
            {
                return AppSettings.GetValueOrDefault(IDENTITY_ENDPOINT_BASE_KEY, IDENTITY_ENDPOINT_BASE_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IDENTITY_ENDPOINT_BASE_KEY, value);
            }
        }

        public string FacturacionEndpointBase
        {
            get
            {
                return AppSettings.GetValueOrDefault(FACTURACION_ENDPOINT_BASE_KEY, FACTURACION_ENDPOINT_BASE_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FACTURACION_ENDPOINT_BASE_KEY, value);
            }
        }

        public string PDFEndpointBase
        {
            get
            {
                return AppSettings.GetValueOrDefault(PDF_ENDPOINT_BASE_KEY, PDF_ENDPOINT_BASE_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PDF_ENDPOINT_BASE_KEY, value);
            }
        }

        string IGlobalIdentitySettings.ClientId
        {
            get
            {
                return AppSettings.GetValueOrDefault(CLIENT_ID_KEY, CLIENT_ID_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CLIENT_ID_KEY, value);
            }
        }

        string IGlobalIdentitySettings.ClientSecret
        {
            get
            {
                return AppSettings.GetValueOrDefault(CLIENT_SECRET_KEY, CLIENT_SECRET_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CLIENT_SECRET_KEY, value);
            }
        }

        string IGlobalIdentitySettings.EndpointBase
        {
            get
            {
                return IdentityEndpointBase;
            }
            set
            {
                IdentityEndpointBase = value;
            }
        }

        bool IRequestSettings.ValidateSslCertificate
        {
            get
            {
                return AppSettings.GetValueOrDefault(VALIDATE_SSL_CERTIFICATE_KEY, VALIDATE_SSL_CERTIFICATE_DEFAULT);
            }
            set
            {
                AppSettings.AddOrUpdateValue(VALIDATE_SSL_CERTIFICATE_KEY, value);
            }
        }

    }
}
