using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Agentes
{
    public class ServiceParticipanteClientFactory : SoapClientFactory<ServicioParticipanteSoapClient, ServicioParticipanteSoap>
    {
        public ServiceParticipanteClientFactory(FrameworkEndpointConfiguration configuration) : base(configuration)
        {
        }

        protected override string RelativeAddress
        {
            get
            {
                return Direcciones.url_ParticipanteAgente;
            }
        }

        protected override ServicioParticipanteSoapClient CreateClientIntern(Binding binding, EndpointAddress endpointAddress)
        {
            return new ServicioParticipanteSoapClient(binding, endpointAddress);
        }
    }
}
