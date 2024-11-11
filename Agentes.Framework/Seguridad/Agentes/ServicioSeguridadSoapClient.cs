using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes
{
    public partial class ServicioSeguridadSoapClient : ClientBase<ServicioSeguridadSoap>, ServicioSeguridadSoap
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials);

        public ServicioSeguridadSoapClient(EndpointConfiguration endpointConfiguration) :
                base(ServicioSeguridadSoapClient.GetBindingForEndpoint(endpointConfiguration), ServicioSeguridadSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioSeguridadSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(ServicioSeguridadSoapClient.GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioSeguridadSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
                base(ServicioSeguridadSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioSeguridadSoapClient(Binding binding, EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public Task<string> LoginAplicacionAsync(int IdAplicacion, string UsrApl)
        {
            return base.Channel.LoginAplicacionAsync(IdAplicacion, UsrApl);
        }

        public Task<string> LoginAplicacionMTAsync(int IdAplicacion, string UsrApl)
        {
            return base.Channel.LoginAplicacionMTAsync(IdAplicacion, UsrApl);
        }

        public Task<string> RegistraUserAsync(string PI_xmlParam)
        {
            return base.Channel.RegistraUserAsync(PI_xmlParam);
        }

        public Task<string> ValidaUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.ValidaUsuarioAsync(PI_xmlParam);
        }

        public Task<string> DesRegistraUserAsync(string PI_xmlParam)
        {
            return base.Channel.DesRegistraUserAsync(PI_xmlParam);
        }

        public Task<string> getPerfilAsync(string PI_xmlParam)
        {
            return base.Channel.getPerfilAsync(PI_xmlParam);
        }

        public Task<string> consPermisoUserTransOpcionAsync(string PI_xmlParam)
        {
            return base.Channel.consPermisoUserTransOpcionAsync(PI_xmlParam);
        }

        public Task<string> IsPermisoUserTransOpcionAsync(string PI_xmlParam)
        {
            return base.Channel.IsPermisoUserTransOpcionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsultaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsultaAsync(PI_xmlParam);
        }

        public Task<string> actEstadoRegistroAsync(string PI_xmlParam)
        {
            return base.Channel.actEstadoRegistroAsync(PI_xmlParam);
        }

        public Task<string> EncryptAsync(string PI_Original, string PI_Key)
        {
            return base.Channel.EncryptAsync(PI_Original, PI_Key);
        }

        public Task<string> DecryptAsync(string PI_Original, string PI_Key)
        {
            return base.Channel.DecryptAsync(PI_Original, PI_Key);
        }

        public Task<string> VerificaUserAsync(string PI_xmlParam)
        {
            return base.Channel.VerificaUserAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsAuditoriaTransaccionesAsync(string PI_xmlParam)
        {
            return base.Channel.ConsAuditoriaTransaccionesAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsAuditoriaRolesAsync(string PI_xmlParam)
        {
            return base.Channel.ConsAuditoriaRolesAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsAuditoriaUsuariosAsync(string PI_xmlParam)
        {
            return base.Channel.ConsAuditoriaUsuariosAsync(PI_xmlParam);
        }

        public Task<string> ConsParamAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.ConsParamAplicacionAsync(PI_xmlParam);
        }

        public Task<string> IsPermisoAplTransOpcionAsync(string PI_xmlParam)
        {
            return base.Channel.IsPermisoAplTransOpcionAsync(PI_xmlParam);
        }

        public Task<string> ConsSemillaPartAsync(string PI_xmlParam)
        {
            return base.Channel.ConsSemillaPartAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consTransaccionesMTAsync(string PI_xmlParam)
        {
            return base.Channel.consTransaccionesMTAsync(PI_xmlParam);
        }

        public Task<string> EjecutaTransaccionesAsync(string XmlSeccion, string XmlEntrada)
        {
            return base.Channel.EjecutaTransaccionesAsync(XmlSeccion, XmlEntrada);
        }

        public Task<string> EjecutaTransaccionAsync(string XmlSeccion, string XmlEntrada)
        {
            return base.Channel.EjecutaTransaccionAsync(XmlSeccion, XmlEntrada);
        }

        public Task<ArrayOfXElement> EjecutaTransaccionDSAsync(string XmlSeccion, string XmlEntrada)
        {
            return base.Channel.EjecutaTransaccionDSAsync(XmlSeccion, XmlEntrada);
        }

        public Task<string> CambioClaveAsync(string PI_xmlParam)
        {
            return base.Channel.CambioClaveAsync(PI_xmlParam);
        }

        public Task<string> getGruposADAsync(string PI_xmlSession)
        {
            return base.Channel.getGruposADAsync(PI_xmlSession);
        }

        public Task<string> ConsUsuarioADAsync(string PI_xmlSession)
        {
            return base.Channel.ConsUsuarioADAsync(PI_xmlSession);
        }

        public Task<string> CrearUsuarioADAsync(string PI_xmlSession, string PI_xmlUsuario)
        {
            return base.Channel.CrearUsuarioADAsync(PI_xmlSession, PI_xmlUsuario);
        }

        public Task<string> ModificaUsuarioADAsync(string PI_xmlSession, string PI_xmlUsuario)
        {
            return base.Channel.ModificaUsuarioADAsync(PI_xmlSession, PI_xmlUsuario);
        }

        public Task<string> EliminaUsuarioADAsync(string PI_xmlSession)
        {
            return base.Channel.EliminaUsuarioADAsync(PI_xmlSession);
        }

        public Task<string> getUsuariosADAsync(string PI_xmlSession)
        {
            return base.Channel.getUsuariosADAsync(PI_xmlSession);
        }

        public Task<string> getUnidadOrgADAsync(string PI_xmlSession)
        {
            return base.Channel.getUnidadOrgADAsync(PI_xmlSession);
        }

        public Task<object> AtributosADAsync(string usuario)
        {
            return base.Channel.AtributosADAsync(usuario);
        }

        public Task<ArrayOfXElement> getXmlConfigAsync(string opcion)
        {
            return base.Channel.getXmlConfigAsync(opcion);
        }

        public Task<string> setXmlConfigAsync(string Opcion, ArrayOfXElement PI_Ds)
        {
            return base.Channel.setXmlConfigAsync(Opcion, PI_Ds);
        }

        public Task<string> getUrlWSFrameWorkAsync()
        {
            return base.Channel.getUrlWSFrameWorkAsync();
        }

        public Task<string> RegistraUserLocalAsync(string PI_xmlParam)
        {
            return base.Channel.RegistraUserLocalAsync(PI_xmlParam);
        }

        public Task<string> RegistraUserRemotoAsync(string PI_xmlParam)
        {
            return base.Channel.RegistraUserRemotoAsync(PI_xmlParam);
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
            if ((endpointConfiguration == EndpointConfiguration.ServicioSeguridadSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioSeguridadSoap12))
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
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServicioSeguridadSoap))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioSeguridad.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioSeguridadSoap12))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioSeguridad.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            ServicioSeguridadSoap,

            ServicioSeguridadSoap12,
        }
    }
}
