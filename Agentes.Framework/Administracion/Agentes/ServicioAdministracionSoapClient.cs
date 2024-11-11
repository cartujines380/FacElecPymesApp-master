using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Administracion.Agentes
{
    public partial class ServicioAdministracionSoapClient : ClientBase<ServicioAdministracionSoap>, ServicioAdministracionSoap
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials);

        public ServicioAdministracionSoapClient(EndpointConfiguration endpointConfiguration) :
                base(ServicioAdministracionSoapClient.GetBindingForEndpoint(endpointConfiguration), ServicioAdministracionSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioAdministracionSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(ServicioAdministracionSoapClient.GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioAdministracionSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
                base(ServicioAdministracionSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioAdministracionSoapClient(Binding binding, EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public Task<string> ConsTablaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsTablaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsCatalogoAsync(string PI_xmlParam)
        {
            return base.Channel.ConsCatalogoAsync(PI_xmlParam);
        }

        public Task<string> ActCatalogosxTablaAsync(string PI_xmlParam)
        {
            return base.Channel.ActCatalogosxTablaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.consAplicacionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consParamAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.consParamAplicacionAsync(PI_xmlParam);
        }

        public Task<string> consAplicacionTipoAsync(string PI_xmlParam)
        {
            return base.Channel.consAplicacionTipoAsync(PI_xmlParam);
        }

        public Task<string> ingAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.ingAplicacionAsync(PI_xmlParam);
        }

        public Task<string> actAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.actAplicacionAsync(PI_xmlParam);
        }

        public Task<string> eliAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.eliAplicacionAsync(PI_xmlParam);
        }

        public Task<string> consServidoresBaseDatosAsync(string PI_xmlParam)
        {
            return base.Channel.consServidoresBaseDatosAsync(PI_xmlParam);
        }

        public Task<string> consServEjecutaAsync(string PI_xmlParam)
        {
            return base.Channel.consServEjecutaAsync(PI_xmlParam);
        }

        public Task<string> VerificaSPAsync(string PI_xmlParam)
        {
            return base.Channel.VerificaSPAsync(PI_xmlParam);
        }

        public Task<string> VerificaServidorAsync(string PI_xmlParam)
        {
            return base.Channel.VerificaServidorAsync(PI_xmlParam);
        }

        public Task<string> consOrgTransaccionAsync(string PI_xmlParam)
        {
            return base.Channel.consOrgTransaccionAsync(PI_xmlParam);
        }

        public Task<string> consCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.consCategoriaAsync(PI_xmlParam);
        }

        public Task<string> ingCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.ingCategoriaAsync(PI_xmlParam);
        }

        public Task<string> actCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.actCategoriaAsync(PI_xmlParam);
        }

        public Task<string> eliCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.eliCategoriaAsync(PI_xmlParam);
        }

        public Task<string> consOrganizacionAsync(string PI_xmlParam)
        {
            return base.Channel.consOrganizacionAsync(PI_xmlParam);
        }

        public Task<string> consOrganizacionAutAsync(string PI_xmlParam)
        {
            return base.Channel.consOrganizacionAutAsync(PI_xmlParam);
        }

        public Task<string> ingOrganizacionAsync(string PI_xmlParam)
        {
            return base.Channel.ingOrganizacionAsync(PI_xmlParam);
        }

        public Task<string> actOrganizacionAsync(string PI_xmlParam)
        {
            return base.Channel.actOrganizacionAsync(PI_xmlParam);
        }

        public Task<string> eliOrganizacionAsync(string PI_xmlParam)
        {
            return base.Channel.eliOrganizacionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conOrganizacionIdAplicacionAsync(string PI_xmlParam)
        {
            return base.Channel.conOrganizacionIdAplicacionAsync(PI_xmlParam);
        }

        public Task<string> consAplicacionGenAsync(string PI_xmlParam)
        {
            return base.Channel.consAplicacionGenAsync(PI_xmlParam);
        }

        public Task<string> consHorariosAsync(string PI_xmlParam)
        {
            return base.Channel.consHorariosAsync(PI_xmlParam);
        }

        public Task<string> consHorarioIdAsync(string PI_xmlParam)
        {
            return base.Channel.consHorarioIdAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consHorarioDiaAsync(string PI_xmlParam)
        {
            return base.Channel.consHorarioDiaAsync(PI_xmlParam);
        }

        public Task<string> ingHorarioAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.ingHorarioAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> actHorarioAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.actHorarioAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> eliHorarioAsync(string PI_xmlParam)
        {
            return base.Channel.eliHorarioAsync(PI_xmlParam);
        }

        public Task<string> IngresaUsuarioAsync(string PI_xmlSeccion, string PI_xmlParam)
        {
            return base.Channel.IngresaUsuarioAsync(PI_xmlSeccion, PI_xmlParam);
        }

        public Task<string> ActualizaUsuarioAsync(string PI_xmlSeccion, string PI_xmlParam)
        {
            return base.Channel.ActualizaUsuarioAsync(PI_xmlSeccion, PI_xmlParam);
        }

        public Task<string> EliminaUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.EliminaUsuarioAsync(PI_xmlParam);
        }

        public Task<string> consOpcTransaccionAsync(string PI_xmlParam)
        {
            return base.Channel.consOpcTransaccionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consultaUsuarioRolAsync(string PI_xmlParam)
        {
            return base.Channel.consultaUsuarioRolAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consultaUsuarioAutAsync(string PI_xmlParam)
        {
            return base.Channel.consultaUsuarioAutAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consultaUsuarioTrAsync(string PI_xmlParam)
        {
            return base.Channel.consultaUsuarioTrAsync(PI_xmlParam);
        }

        public Task<string> consDescIdentificacionAsync(string PI_xmlParam)
        {
            return base.Channel.consDescIdentificacionAsync(PI_xmlParam);
        }

        public Task<string> consDescAutorizacionAsync(string PI_xmlParam)
        {
            return base.Channel.consDescAutorizacionAsync(PI_xmlParam);
        }

        public Task<string> consCodUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.consCodUsuarioAsync(PI_xmlParam);
        }

        public Task<string> eliRolAsync(string PI_xmlParam)
        {
            return base.Channel.eliRolAsync(PI_xmlParam);
        }

        public Task<string> ingRolAsync(string PI_xmlSeccion, string PI_xmlParam)
        {
            return base.Channel.ingRolAsync(PI_xmlSeccion, PI_xmlParam);
        }

        public Task<string> actRolAsync(string PI_xmlSeccion, string PI_xmlParam)
        {
            return base.Channel.actRolAsync(PI_xmlSeccion, PI_xmlParam);
        }

        public Task<ArrayOfXElement> consultaRolAsync(string PI_xmlParam)
        {
            return base.Channel.consultaRolAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRolTransaccionAsync(string PI_xmlParam)
        {
            return base.Channel.consRolTransaccionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consTransaccionAsync(string PI_xmlParam)
        {
            return base.Channel.consTransaccionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consOpcionTrnAsync(string PI_xmlParam)
        {
            return base.Channel.consOpcionTrnAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conHorarioOpcionesAsync(string PI_xmlParam)
        {
            return base.Channel.conHorarioOpcionesAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conAutorHoraAsync(string PI_xmlParam)
        {
            return base.Channel.conAutorHoraAsync(PI_xmlParam);
        }

        public Task<string> ingTransaccionesAsync(string PI_xmlSeccion, string PI_xmlParam, string PI_xmlEntrada, string PI_xmlSalida)
        {
            return base.Channel.ingTransaccionesAsync(PI_xmlSeccion, PI_xmlParam, PI_xmlEntrada, PI_xmlSalida);
        }

        public Task<string> actTransaccionesAsync(string PI_xmlSeccion, string PI_xmlParam, string PI_xmlEntrada, string PI_xmlSalida)
        {
            return base.Channel.actTransaccionesAsync(PI_xmlSeccion, PI_xmlParam, PI_xmlEntrada, PI_xmlSalida);
        }

        public Task<string> eliTransaccionAsync(string PI_xmlParam)
        {
            return base.Channel.eliTransaccionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consTransOpcOrgAsync(string PI_xmlParam)
        {
            return base.Channel.consTransOpcOrgAsync(PI_xmlParam);
        }

        public Task<string> verificaUsuarioAsocAsync(string PI_xmlParam)
        {
            return base.Channel.verificaUsuarioAsocAsync(PI_xmlParam);
        }

        public Task<string> verificaRolAsocAsync(string PI_xmlParam)
        {
            return base.Channel.verificaRolAsocAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conParamSPAsync(string PI_xmlParam)
        {
            return base.Channel.conParamSPAsync(PI_xmlParam);
        }

        public Task<string> conOrganizacionAsync(string PI_xmlParam)
        {
            return base.Channel.conOrganizacionAsync(PI_xmlParam);
        }

        public Task<string> consBaseDatosAsync(string PI_xmlParam)
        {
            return base.Channel.consBaseDatosAsync(PI_xmlParam);
        }

        public Task<string> consSPBaseDatosAsync(string PI_xmlParam)
        {
            return base.Channel.consSPBaseDatosAsync(PI_xmlParam);
        }

        public Task<string> conTransFormAsync(string PI_xmlParam)
        {
            return base.Channel.conTransFormAsync(PI_xmlParam);
        }

        public Task<string> IngresaFormularioAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.IngresaFormularioAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> conFormularioAsync(string PI_xmlParam)
        {
            return base.Channel.conFormularioAsync(PI_xmlParam);
        }

        public Task<string> consModTrasFormularioAsync(string PI_xmlParam)
        {
            return base.Channel.consModTrasFormularioAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conObjetosFormAsync(string PI_xmlParam)
        {
            return base.Channel.conObjetosFormAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conPropiedadObjetosAsync(string PI_xmlParam)
        {
            return base.Channel.conPropiedadObjetosAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consTransaccionUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.consTransaccionUsuarioAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRolesUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.consRolesUsuarioAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRolesEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.consRolesEmpresaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRegistroEmpresaFAsync(string PI_xmlParam)
        {
            return base.Channel.consRegistroEmpresaFAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRegistroUsuarioFAsync(string PI_xmlParam)
        {
            return base.Channel.consRegistroUsuarioFAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consRegistroGeneralFAsync(string PI_xmlParam)
        {
            return base.Channel.consRegistroGeneralFAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consAuditoriaOrganizacionFAsync(string PI_xmlParam)
        {
            return base.Channel.consAuditoriaOrganizacionFAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> consAuditoriaUsuarioFAsync(string PI_xmlParam)
        {
            return base.Channel.consAuditoriaUsuarioFAsync(PI_xmlParam);
        }

        public Task<string> ConsUsuarioOficinaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsUsuarioOficinaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsAuditoriaCuentasAsync(string PI_xmlParam)
        {
            return base.Channel.ConsAuditoriaCuentasAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsDiasFeriadosAsync(string PI_xmlParam)
        {
            return base.Channel.ConsDiasFeriadosAsync(PI_xmlParam);
        }

        public Task<string> ActDiasFeriadosAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.ActDiasFeriadosAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> IngresaLLaveAsync(string PI_xmlParam, string PI_xmlDatos, ArrayOfXElement PI_File)
        {
            return base.Channel.IngresaLLaveAsync(PI_xmlParam, PI_xmlDatos, PI_File);
        }

        public Task<string> ActualizaLLaveAsync(string PI_xmlParam, string PI_xmlDatos, ArrayOfXElement PI_File)
        {
            return base.Channel.ActualizaLLaveAsync(PI_xmlParam, PI_xmlDatos, PI_File);
        }

        public Task<ArrayOfXElement> ConsultaLLaveAsync(string PI_xmlParam)
        {
            return base.Channel.ConsultaLLaveAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsReporteLLaveAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.ConsReporteLLaveAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<ArrayOfXElement> ConsReporteAuditoriaAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.ConsReporteAuditoriaAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> ConsultaLLaveEspAsync(string PI_xmlParam)
        {
            return base.Channel.ConsultaLLaveEspAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsultaLLaveEspFileAsync(string PI_xmlParam)
        {
            return base.Channel.ConsultaLLaveEspFileAsync(PI_xmlParam);
        }

        public Task<string> ConsultaLLaveKEYAsync(string PI_xmlParam)
        {
            return base.Channel.ConsultaLLaveKEYAsync(PI_xmlParam);
        }

        public Task<string> ConsLLavesServicioAsync(string PI_xmlParam)
        {
            return base.Channel.ConsLLavesServicioAsync(PI_xmlParam);
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
            if ((endpointConfiguration == EndpointConfiguration.ServicioAdministracionSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioAdministracionSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.ServicioAdministracionSoap))
            {
                return new EndpointAddress("http://localhost/WsFramework/ServicioAdministracion.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioAdministracionSoap12))
            {
                return new EndpointAddress("http://localhost/WsFramework/ServicioAdministracion.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            ServicioAdministracionSoap,

            ServicioAdministracionSoap12,
        }
    }
}
