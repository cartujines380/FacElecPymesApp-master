using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;

using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes
{
    public partial class ServicioClienteSoapClient : ClientBase<ServicioClienteSoap>, ServicioClienteSoap
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials);

        public ServicioClienteSoapClient(EndpointConfiguration endpointConfiguration) :
                base(ServicioClienteSoapClient.GetBindingForEndpoint(endpointConfiguration), ServicioClienteSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioClienteSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(ServicioClienteSoapClient.GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioClienteSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
                base(ServicioClienteSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioClienteSoapClient(Binding binding, EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        Task<ValidaLoginResponse> ServicioClienteSoap.ValidaLoginAsync(ValidaLoginRequest request)
        {
            return base.Channel.ValidaLoginAsync(request);
        }

        public Task<ValidaLoginResponse> ValidaLoginAsync(string PI_xmlParam)
        {
            ValidaLoginRequest inValue = new ValidaLoginRequest();
            inValue.Body = new ValidaLoginRequestBody();
            inValue.Body.PI_xmlParam = PI_xmlParam;
            return ((ServicioClienteSoap)(this)).ValidaLoginAsync(inValue);
        }

        Task<CambiaClaveResponse> ServicioClienteSoap.CambiaClaveAsync(CambiaClaveRequest request)
        {
            return base.Channel.CambiaClaveAsync(request);
        }

        public Task<CambiaClaveResponse> CambiaClaveAsync(string PI_xmlParam)
        {
            CambiaClaveRequest inValue = new CambiaClaveRequest();
            inValue.Body = new CambiaClaveRequestBody();
            inValue.Body.PI_xmlParam = PI_xmlParam;
            return ((ServicioClienteSoap)(this)).CambiaClaveAsync(inValue);
        }

        Task<ResetClaveResponse> ServicioClienteSoap.ResetClaveAsync(ResetClaveRequest request)
        {
            return base.Channel.ResetClaveAsync(request);
        }

        public Task<ResetClaveResponse> ResetClaveAsync(string PI_xmlParam)
        {
            ResetClaveRequest inValue = new ResetClaveRequest();
            inValue.Body = new ResetClaveRequestBody();
            inValue.Body.PI_xmlParam = PI_xmlParam;
            return ((ServicioClienteSoap)(this)).ResetClaveAsync(inValue);
        }

        public virtual Task OpenAsync()
        {
            return Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginOpen(null, null), new Action<IAsyncResult>(((ICommunicationObject)(this)).EndOpen));
        }

        public virtual Task CloseAsync()
        {
            return Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginClose(null, null), new Action<IAsyncResult>(((ICommunicationObject)(this)).EndClose));
        }

        private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServicioClienteSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioClienteSoap12))
            {
                CustomBinding result = new CustomBinding();
                TextMessageEncodingBindingElement textBindingElement = new TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServicioClienteSoap))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioCliente.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioClienteSoap12))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioCliente.asmx");
            }
            throw new InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            ServicioClienteSoap,

            ServicioClienteSoap12,
        }
    }
}
