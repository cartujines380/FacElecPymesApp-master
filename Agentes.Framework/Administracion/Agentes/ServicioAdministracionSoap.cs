using System;
using System.ServiceModel;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Administracion.Agentes
{
    [ServiceContract(Namespace = "http://www.sipecom.com/WSFrameWork/", ConfigurationName = "ServicioAdministracionSoap")]
    public interface ServicioAdministracionSoap
    {

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsTabla", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsTablaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsCatalogo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsCatalogoAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ActCatalogosxTabla", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ActCatalogosxTablaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consParamAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consParamAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consAplicacionTipo", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consAplicacionTipoAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consServidoresBaseDatos", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consServidoresBaseDatosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consServEjecuta", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consServEjecutaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/VerificaSP", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> VerificaSPAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/VerificaServidor", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> VerificaServidorAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consOrgTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consOrgTransaccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliCategoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliCategoriaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consOrganizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consOrganizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consOrganizacionAut", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consOrganizacionAutAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingOrganizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingOrganizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actOrganizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actOrganizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliOrganizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliOrganizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conOrganizacionIdAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conOrganizacionIdAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consAplicacionGen", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consAplicacionGenAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consHorarios", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consHorariosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consHorarioId", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consHorarioIdAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consHorarioDia", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consHorarioDiaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingHorario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingHorarioAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actHorario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actHorarioAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliHorario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliHorarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/IngresaUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IngresaUsuarioAsync(string PI_xmlSeccion, string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ActualizaUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ActualizaUsuarioAsync(string PI_xmlSeccion, string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/EliminaUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EliminaUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consOpcTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consOpcTransaccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consultaUsuarioRol", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consultaUsuarioRolAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consultaUsuarioAut", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consultaUsuarioAutAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consultaUsuarioTr", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consultaUsuarioTrAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consDescIdentificacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consDescIdentificacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consDescAutorizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consDescAutorizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consCodUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consCodUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliRol", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliRolAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingRol", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingRolAsync(string PI_xmlSeccion, string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actRol", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actRolAsync(string PI_xmlSeccion, string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consultaRol", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consultaRolAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRolTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRolTransaccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consTransaccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consOpcionTrn", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consOpcionTrnAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conHorarioOpciones", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conHorarioOpcionesAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conAutorHora", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conAutorHoraAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ingTransacciones", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ingTransaccionesAsync(string PI_xmlSeccion, string PI_xmlParam, string PI_xmlEntrada, string PI_xmlSalida);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actTransacciones", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actTransaccionesAsync(string PI_xmlSeccion, string PI_xmlParam, string PI_xmlEntrada, string PI_xmlSalida);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/eliTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> eliTransaccionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consTransOpcOrg", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consTransOpcOrgAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/verificaUsuarioAsoc", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> verificaUsuarioAsocAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/verificaRolAsoc", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> verificaRolAsocAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conParamSP", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conParamSPAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conOrganizacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conOrganizacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consBaseDatos", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consBaseDatosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consSPBaseDatos", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consSPBaseDatosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conTransForm", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conTransFormAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/IngresaFormulario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IngresaFormularioAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conFormulario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> conFormularioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consModTrasFormulario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consModTrasFormularioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conObjetosForm", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conObjetosFormAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/conPropiedadObjetos", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> conPropiedadObjetosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consTransaccionUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consTransaccionUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRolesUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRolesUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRolesEmpresa", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRolesEmpresaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRegistroEmpresaF", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRegistroEmpresaFAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRegistroUsuarioF", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRegistroUsuarioFAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consRegistroGeneralF", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consRegistroGeneralFAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consAuditoriaOrganizacionF", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consAuditoriaOrganizacionFAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consAuditoriaUsuarioF", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consAuditoriaUsuarioFAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsUsuarioOficina", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsUsuarioOficinaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsAuditoriaCuentas", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsAuditoriaCuentasAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsDiasFeriados", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsDiasFeriadosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ActDiasFeriados", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ActDiasFeriadosAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/IngresaLLave", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IngresaLLaveAsync(string PI_xmlParam, string PI_xmlDatos, ArrayOfXElement PI_File);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ActualizaLLave", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ActualizaLLaveAsync(string PI_xmlParam, string PI_xmlDatos, ArrayOfXElement PI_File);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsultaLLave", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsultaLLaveAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsReporteLLave", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsReporteLLaveAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsReporteAuditoria", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsReporteAuditoriaAsync(string PI_xmlParam, string PI_xmlDatos);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsultaLLaveEsp", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsultaLLaveEspAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsultaLLaveEspFile", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsultaLLaveEspFileAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsultaLLaveKEY", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsultaLLaveKEYAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsLLavesServicio", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsLLavesServicioAsync(string PI_xmlParam);
    }
}