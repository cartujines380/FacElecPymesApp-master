using System;
using System.ServiceModel;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Agentes
{
    [ServiceContract(Namespace = "http:\'www.sipecom.com/WSFrameWork/", ConfigurationName = "ServicioParticipanteSoap")]
    public interface ServicioParticipanteSoap
    {

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpresasOficina", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conEmpresasOficinaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conOficinasEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conOficinasEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsEmpresaxCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsEmpresaxCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsAgencias", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsAgenciasAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsPaisxCategoriaEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsPaisxCategoriaEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsEmpleadoEmpresaExt", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsEmpleadoEmpresaExtAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsEmpTrabaja", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsEmpTrabajaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsProvCat", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsProvCatAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsContProv", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsContProvAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsClientes", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsClientesAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ModGastoCliente", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ModGastoClienteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/CreaUsuarioProv", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> CreaUsuarioProvAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsCatalogoNombre", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsCatalogoNombreAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsCatalogoUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsCatalogoUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsCatalogoProvincia", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsCatalogoProvinciaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsCatalogoCiudad", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsCatalogoCiudadAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/SetVisitante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> SetVisitanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpresaDs", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConEmpresaDsAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConOficina", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConOficinaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConOficinaGeneral", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConOficinaGeneralAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpleadoEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConEmpleadoEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpresaGen", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConEmpresaGenAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpresaGrupo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conEmpresaGrupoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/CargaMasiva", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> CargaMasivaAsync(string strXmlSession, string strXmlDatos);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/RegistraCliente", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/RegistraUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraUsuarioAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/RegistraParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/SolicitudCliente", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> SolicitudClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsPregSecreta", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsPregSecretaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsRespSecreta", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsRespSecretaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsUsuarioIdent", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsUsuarioIdentAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsPartRegistro", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsPartRegistroAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ActRegistroCliente", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ActRegistroClienteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosClie);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsGenSolicitud", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsGenSolicitudAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsSolicitudPart", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsSolicitudPartAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsUsuarioRegistro", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsUsuarioRegistroAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/EnviaCorreo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EnviaCorreoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConOficinaZona", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConOficinaZonaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConZonaOficina", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConZonaOficinaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conNaturalezaEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conNaturalezaEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpresaNat", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conEmpresaNatAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpresaCliente", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConEmpresaClienteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEmpleadoCargo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConEmpleadoCargoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConOficinaSRI", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConOficinaSRIAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConCargaFamiliar", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConCargaFamiliarAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConLotesHac", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConLotesHacAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConReferencias", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConReferenciasAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/IngParticipanteContacto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IngParticipanteContactoAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConParticipanteContacto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConParticipanteContactoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConMedioContactoC", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConMedioContactoCAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConDireccionContacto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConDireccionContactoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ModParticipanteContactoAFS", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ModParticipanteContactoAFSAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conCategoriaId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conCategoriaIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ingCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/modCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> modCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/eliCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conOrganigramaId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conOrganigramaIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ingOrganigrama", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingOrganigramaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/modOrganigrama", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> modOrganigramaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/eliOrganigrama", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliOrganigramaAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ingParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosCEP);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/modParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> modParticipanteAsync(string PI_xmlParam, string PI_xmlDatos, string PI_xmlDatosCEP);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/eliParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliParticipanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/eliUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conParticipanteId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conParticipanteIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conParticipanteIdentificacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conParticipanteIdentificacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conPersonaId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conPersonaIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpresaId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conEmpresaIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conDireccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conDireccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conMedioContacto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conMedioContactoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conContacto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conContactoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conCuentaParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conCuentaParticipanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conDocumentoParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conDocumentoParticipanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conFotoParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conFotoParticipanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conParametroParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conParametroParticipanteAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpleadoGeneral", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conEmpleadoGeneralAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conClienteGeneral", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conClienteGeneralAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conClienteId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conClienteIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conProveedorGeneral", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conProveedorGeneralAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conEmpresaHijo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conEmpresaHijoAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/IngDocumentoParticipante", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IngDocumentoParticipanteAsync(string PI_xmlParam, ArrayOfXElement PI_dsDocumento);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConNivelCtaEmp", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConNivelCtaEmpAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConsUsuarioId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsUsuarioIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/conCarteraClientes", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conCarteraClientesAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/modVendedorCli", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> modVendedorCliAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/repClientes", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> repClientesAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConClienteEmpPadre", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConClienteEmpPadreAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConTodasOfixTodasZonaEmp", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConTodasOfixTodasZonaEmpAsync(string PI_xmlParam);

        [OperationContract(Action = "http:\'www.sipecom.com/WSFrameWork/ConEliLogicaCEP", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConEliLogicaCEPAsync(string PI_xmlParam);
    }
}
