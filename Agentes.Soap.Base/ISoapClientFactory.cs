using System;

namespace Sipecom.FactElec.Pymes.Agentes.Soap.Base
{
    public interface ISoapClientFactory<TClient>
        where TClient : class
    {
        TClient CreateClient();
    }
}
