using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Agentes
{
    public partial class ServicioParticipanteSoapClient : ClientBase<ServicioParticipanteSoap>, ServicioParticipanteSoap
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials);

        public ServicioParticipanteSoapClient(EndpointConfiguration endpointConfiguration) :
                base(ServicioParticipanteSoapClient.GetBindingForEndpoint(endpointConfiguration), ServicioParticipanteSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioParticipanteSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(ServicioParticipanteSoapClient.GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioParticipanteSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
                base(ServicioParticipanteSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ServicioParticipanteSoapClient(Binding binding, EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public Task<ArrayOfXElement> conEmpresasOficinaAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpresasOficinaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conOficinasEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.conOficinasEmpresaAsync(PI_xmlParam);
        }

        public Task<string> ConsEmpresaxCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsEmpresaxCategoriaAsync(PI_xmlParam);
        }

        public Task<string> ConsAgenciasAsync(string PI_xmlParam)
        {
            return base.Channel.ConsAgenciasAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsPaisxCategoriaEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsPaisxCategoriaEmpresaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsEmpleadoEmpresaExtAsync(string PI_xmlParam)
        {
            return base.Channel.ConsEmpleadoEmpresaExtAsync(PI_xmlParam);
        }

        public Task<string> ConsEmpTrabajaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsEmpTrabajaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsProvCatAsync(string PI_xmlParam)
        {
            return base.Channel.ConsProvCatAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConsContProvAsync(string PI_xmlParam)
        {
            return base.Channel.ConsContProvAsync(PI_xmlParam);
        }

        public Task<string> ConsClientesAsync(string PI_xmlParam)
        {
            return base.Channel.ConsClientesAsync(PI_xmlParam);
        }

        public Task<string> ModGastoClienteAsync(string PI_xmlParam)
        {
            return base.Channel.ModGastoClienteAsync(PI_xmlParam);
        }

        public Task<string> CreaUsuarioProvAsync(string PI_xmlParam)
        {
            return base.Channel.CreaUsuarioProvAsync(PI_xmlParam);
        }

        public Task<string> ConsCatalogoNombreAsync(string PI_xmlParam)
        {
            return base.Channel.ConsCatalogoNombreAsync(PI_xmlParam);
        }

        public Task<string> ConsCatalogoUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.ConsCatalogoUsuarioAsync(PI_xmlParam);
        }

        public Task<string> ConsCatalogoProvinciaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsCatalogoProvinciaAsync(PI_xmlParam);
        }

        public Task<string> ConsCatalogoCiudadAsync(string PI_xmlParam)
        {
            return base.Channel.ConsCatalogoCiudadAsync(PI_xmlParam);
        }

        public Task<string> SetVisitanteAsync(string PI_xmlParam)
        {
            return base.Channel.SetVisitanteAsync(PI_xmlParam);
        }

        public Task<string> ConEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpresaAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConEmpresaDsAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpresaDsAsync(PI_xmlParam);
        }

        public Task<string> ConOficinaAsync(string PI_xmlParam)
        {
            return base.Channel.ConOficinaAsync(PI_xmlParam);
        }

        public Task<string> ConOficinaGeneralAsync(string PI_xmlParam)
        {
            return base.Channel.ConOficinaGeneralAsync(PI_xmlParam);
        }

        public Task<string> ConEmpleadoEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpleadoEmpresaAsync(PI_xmlParam);
        }

        public Task<string> ConEmpresaGenAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpresaGenAsync(PI_xmlParam);
        }

        public Task<string> conEmpresaGrupoAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpresaGrupoAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> CargaMasivaAsync(string strXmlSession, string strXmlDatos)
        {
            return base.Channel.CargaMasivaAsync(strXmlSession, strXmlDatos);
        }

        public Task<string> RegistraClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie)
        {
            return base.Channel.RegistraClienteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosClie);
        }

        public Task<string> RegistraUsuarioAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie)
        {
            return base.Channel.RegistraUsuarioAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosClie);
        }

        public Task<string> RegistraParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie)
        {
            return base.Channel.RegistraParticipanteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosClie);
        }

        public Task<string> SolicitudClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie)
        {
            return base.Channel.SolicitudClienteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosClie);
        }

        public Task<string> ConsPregSecretaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsPregSecretaAsync(PI_xmlParam);
        }

        public Task<string> ConsRespSecretaAsync(string PI_xmlParam)
        {
            return base.Channel.ConsRespSecretaAsync(PI_xmlParam);
        }

        public Task<string> ConsUsuarioIdentAsync(string PI_xmlParam)
        {
            return base.Channel.ConsUsuarioIdentAsync(PI_xmlParam);
        }

        public Task<string> ConsPartRegistroAsync(string PI_xmlParam)
        {
            return base.Channel.ConsPartRegistroAsync(PI_xmlParam);
        }

        public Task<string> ActRegistroClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie)
        {
            return base.Channel.ActRegistroClienteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosClie);
        }

        public Task<ArrayOfXElement> ConsGenSolicitudAsync(string PI_xmlParam)
        {
            return base.Channel.ConsGenSolicitudAsync(PI_xmlParam);
        }

        public Task<string> ConsSolicitudPartAsync(string PI_xmlParam)
        {
            return base.Channel.ConsSolicitudPartAsync(PI_xmlParam);
        }

        public Task<string> ConsUsuarioRegistroAsync(string PI_xmlParam)
        {
            return base.Channel.ConsUsuarioRegistroAsync(PI_xmlParam);
        }

        public Task<string> EnviaCorreoAsync(string PI_xmlParam)
        {
            return base.Channel.EnviaCorreoAsync(PI_xmlParam);
        }

        public Task<string> ConOficinaZonaAsync(string PI_xmlParam)
        {
            return base.Channel.ConOficinaZonaAsync(PI_xmlParam);
        }

        public Task<string> ConZonaOficinaAsync(string PI_xmlParam)
        {
            return base.Channel.ConZonaOficinaAsync(PI_xmlParam);
        }

        public Task<string> conNaturalezaEmpresaAsync(string PI_xmlParam)
        {
            return base.Channel.conNaturalezaEmpresaAsync(PI_xmlParam);
        }

        public Task<string> conEmpresaNatAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpresaNatAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConEmpresaClienteAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpresaClienteAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConEmpleadoCargoAsync(string PI_xmlParam)
        {
            return base.Channel.ConEmpleadoCargoAsync(PI_xmlParam);
        }

        public Task<string> ConOficinaSRIAsync(string PI_xmlParam)
        {
            return base.Channel.ConOficinaSRIAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConCargaFamiliarAsync(string PI_xmlParam)
        {
            return base.Channel.ConCargaFamiliarAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConLotesHacAsync(string PI_xmlParam)
        {
            return base.Channel.ConLotesHacAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> ConReferenciasAsync(string PI_xmlParam)
        {
            return base.Channel.ConReferenciasAsync(PI_xmlParam);
        }

        public Task<string> IngParticipanteContactoAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.IngParticipanteContactoAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> ConParticipanteContactoAsync(string PI_xmlParam)
        {
            return base.Channel.ConParticipanteContactoAsync(PI_xmlParam);
        }

        public Task<string> ConMedioContactoCAsync(string PI_xmlParam)
        {
            return base.Channel.ConMedioContactoCAsync(PI_xmlParam);
        }

        public Task<string> ConDireccionContactoAsync(string PI_xmlParam)
        {
            return base.Channel.ConDireccionContactoAsync(PI_xmlParam);
        }

        public Task<string> ModParticipanteContactoAFSAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.ModParticipanteContactoAFSAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<string> conCategoriaIdAsync(string PI_xmlParam)
        {
            return base.Channel.conCategoriaIdAsync(PI_xmlParam);
        }

        public Task<string> ingCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.ingCategoriaAsync(PI_xmlParam);
        }

        public Task<string> modCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.modCategoriaAsync(PI_xmlParam);
        }

        public Task<string> eliCategoriaAsync(string PI_xmlParam)
        {
            return base.Channel.eliCategoriaAsync(PI_xmlParam);
        }

        public Task<string> conOrganigramaIdAsync(string PI_xmlParam)
        {
            return base.Channel.conOrganigramaIdAsync(PI_xmlParam);
        }

        public Task<string> ingOrganigramaAsync(string PI_xmlParam)
        {
            return base.Channel.ingOrganigramaAsync(PI_xmlParam);
        }

        public Task<string> modOrganigramaAsync(string PI_xmlParam)
        {
            return base.Channel.modOrganigramaAsync(PI_xmlParam);
        }

        public Task<string> eliOrganigramaAsync(string PI_xmlParam)
        {
            return base.Channel.eliOrganigramaAsync(PI_xmlParam);
        }

        public Task<string> ingParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosCEP)
        {
            return base.Channel.ingParticipanteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosCEP);
        }

        public Task<string> modParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosCEP)
        {
            return base.Channel.modParticipanteAsync(PI_xmlParam, PI_xmlDatos, PI_xmlDatosCEP);
        }

        public Task<string> eliParticipanteAsync(string PI_xmlParam)
        {
            return base.Channel.eliParticipanteAsync(PI_xmlParam);
        }

        public Task<string> eliUsuarioAsync(string PI_xmlParam)
        {
            return base.Channel.eliUsuarioAsync(PI_xmlParam);
        }

        public Task<string> conParticipanteIdAsync(string PI_xmlParam)
        {
            return base.Channel.conParticipanteIdAsync(PI_xmlParam);
        }

        public Task<string> conParticipanteIdentificacionAsync(string PI_xmlParam)
        {
            return base.Channel.conParticipanteIdentificacionAsync(PI_xmlParam);
        }

        public Task<string> conPersonaIdAsync(string PI_xmlParam)
        {
            return base.Channel.conPersonaIdAsync(PI_xmlParam);
        }

        public Task<string> conEmpresaIdAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpresaIdAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conDireccionAsync(string PI_xmlParam)
        {
            return base.Channel.conDireccionAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conMedioContactoAsync(string PI_xmlParam)
        {
            return base.Channel.conMedioContactoAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conContactoAsync(string PI_xmlParam)
        {
            return base.Channel.conContactoAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conCuentaParticipanteAsync(string PI_xmlParam)
        {
            return base.Channel.conCuentaParticipanteAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conDocumentoParticipanteAsync(string PI_xmlParam)
        {
            return base.Channel.conDocumentoParticipanteAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conFotoParticipanteAsync(string PI_xmlParam)
        {
            return base.Channel.conFotoParticipanteAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conParametroParticipanteAsync(string PI_xmlParam)
        {
            return base.Channel.conParametroParticipanteAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conEmpleadoGeneralAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpleadoGeneralAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conClienteGeneralAsync(string PI_xmlParam)
        {
            return base.Channel.conClienteGeneralAsync(PI_xmlParam);
        }

        public Task<string> conClienteIdAsync(string PI_xmlParam)
        {
            return base.Channel.conClienteIdAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conProveedorGeneralAsync(string PI_xmlParam)
        {
            return base.Channel.conProveedorGeneralAsync(PI_xmlParam);
        }

        public Task<string> conEmpresaHijoAsync(string PI_xmlParam)
        {
            return base.Channel.conEmpresaHijoAsync(PI_xmlParam);
        }

        public Task<string> IngDocumentoParticipanteAsync(string PI_xmlParam, ArrayOfXElement PI_dsDocumento)
        {
            return base.Channel.IngDocumentoParticipanteAsync(PI_xmlParam, PI_dsDocumento);
        }

        public Task<string> ConNivelCtaEmpAsync(string PI_xmlParam)
        {
            return base.Channel.ConNivelCtaEmpAsync(PI_xmlParam);
        }

        public Task<string> ConsUsuarioIdAsync(string PI_xmlParam)
        {
            return base.Channel.ConsUsuarioIdAsync(PI_xmlParam);
        }

        public Task<ArrayOfXElement> conCarteraClientesAsync(string PI_xmlParam)
        {
            return base.Channel.conCarteraClientesAsync(PI_xmlParam);
        }

        public Task<string> modVendedorCliAsync(string PI_xmlParam, string PI_xmlDatos)
        {
            return base.Channel.modVendedorCliAsync(PI_xmlParam, PI_xmlDatos);
        }

        public Task<ArrayOfXElement> repClientesAsync(string PI_xmlParam)
        {
            return base.Channel.repClientesAsync(PI_xmlParam);
        }

        public Task<string> ConClienteEmpPadreAsync(string PI_xmlParam)
        {
            return base.Channel.ConClienteEmpPadreAsync(PI_xmlParam);
        }

        public Task<string> ConTodasOfixTodasZonaEmpAsync(string PI_xmlParam)
        {
            return base.Channel.ConTodasOfixTodasZonaEmpAsync(PI_xmlParam);
        }

        public Task<string> ConEliLogicaCEPAsync(string PI_xmlParam)
        {
            return base.Channel.ConEliLogicaCEPAsync(PI_xmlParam);
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
            if ((endpointConfiguration == EndpointConfiguration.ServicioParticipanteSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioParticipanteSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.ServicioParticipanteSoap))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioParticipante.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ServicioParticipanteSoap12))
            {
                return new EndpointAddress("http://10.1.8.134:90/WsFramework/ServicioParticipante.asmx");
            }
            throw new InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            ServicioParticipanteSoap,

            ServicioParticipanteSoap12,
        }
    }
}
