using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity
{
    public interface IGlobalIdentitySettings
    {
        string ClientId { get; set; }

        string ClientSecret { get; set; }

        string EndpointBase { get; set; }
    }
}
