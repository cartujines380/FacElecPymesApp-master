using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider
{
    public interface IRequestSettings
    {
        bool ValidateSslCertificate { get; set; }
    }
}
