using System;

namespace Sipecom.FactElec.Pymes.Agentes.Soap.Base
{
    public interface IEndpointConfiguration
    {
        void Initialize();

        string BaseAddress { get; set; }

        double OpenTimeout { get; set; }

        double ReceiveTimeout { get; set; }

        double SendTimeout { get; set; }

        double CloseTimeout { get; set; }
    }
}
