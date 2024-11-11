using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes
{
    public class ServicioClienteSoapClientFactory : SoapClientFactory<ServicioClienteSoapClient, ServicioClienteSoap>
    {
        public ServicioClienteSoapClientFactory(FrameworkEndpointConfiguration configuration) : base(configuration)
        {
        }

        protected override string RelativeAddress
        {
            get
            {
                return Direcciones.url_ClienteAgente;
            }
        }

        protected override ServicioClienteSoapClient CreateClientIntern(Binding binding, EndpointAddress endpointAddress)
        {
            return new ServicioClienteSoapClient(binding, endpointAddress);
        }
    }
}
