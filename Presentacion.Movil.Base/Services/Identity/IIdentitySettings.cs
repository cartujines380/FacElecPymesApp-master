using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity
{
    public interface IIdentitySettings
    {
        string ClientId { get; }

        string ClientSecret { get; }

        string RegisterWebsite { get; }

        string LogoutCallback { get; }

        string ErrorCallback { get; }
        

        string AuthorizeEndpoint { get; }

        string UserInfoEndpoint { get; }

        string TokenEndpoint { get; }

        string LogoutEndpoint { get; }

        string Callback { get; }
    }
}
