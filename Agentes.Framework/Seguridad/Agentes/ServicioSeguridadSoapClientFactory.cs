using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes
{
    public class ServicioSeguridadSoapClientFactory : SoapClientFactory<ServicioSeguridadSoapClient, ServicioSeguridadSoap>
    {
        public ServicioSeguridadSoapClientFactory(FrameworkEndpointConfiguration configuration) : base(configuration)
        {
        }

        protected override string RelativeAddress
        {
            get
            {
                return Direcciones.url_SeguridadAgente;
            }
        }

        protected override ServicioSeguridadSoapClient CreateClientIntern(Binding binding, EndpointAddress endpointAddress)
        {
            return new ServicioSeguridadSoapClient(binding, endpointAddress);
        }
    }
}
