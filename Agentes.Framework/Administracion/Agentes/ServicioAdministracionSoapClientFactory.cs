using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Administracion.Agentes
{
    public class ServicioAdministracionSoapClientFactory : SoapClientFactory<ServicioAdministracionSoapClient, ServicioAdministracionSoap>
    {
        public ServicioAdministracionSoapClientFactory(FrameworkEndpointConfiguration configuration) : base(configuration)
        {
        }

        protected override string RelativeAddress
        {
            get
            {
                return Direcciones.url_AdministracionAgente;
            }
        }

        protected override ServicioAdministracionSoapClient CreateClientIntern(Binding binding, EndpointAddress endpointAddress)
        {
            return new ServicioAdministracionSoapClient(binding, endpointAddress);
        }

    }
}
