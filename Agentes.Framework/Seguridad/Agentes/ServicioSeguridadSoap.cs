using System;
using System.ServiceModel;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes
{
    [ServiceContract(Namespace = "http://www.sipecom.com/WSFrameWork/", ConfigurationName = "ServicioSeguridadSoap")]
    public interface ServicioSeguridadSoap
    {

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/LoginAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> LoginAplicacionAsync(int IdAplicacion, string UsrApl);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/LoginAplicacionMT", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> LoginAplicacionMTAsync(int IdAplicacion, string UsrApl);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/RegistraUser", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraUserAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ValidaUsuario", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ValidaUsuarioAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/DesRegistraUser", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> DesRegistraUserAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getPerfil", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> getPerfilAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consPermisoUserTransOpcion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> consPermisoUserTransOpcionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/IsPermisoUserTransOpcion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IsPermisoUserTransOpcionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/Consulta", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsultaAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/actEstadoRegistro", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> actEstadoRegistroAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/Encrypt", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EncryptAsync(string PI_Original, string PI_Key);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/Decrypt", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> DecryptAsync(string PI_Original, string PI_Key);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/VerificaUser", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> VerificaUserAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsAuditoriaTransacciones", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsAuditoriaTransaccionesAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsAuditoriaRoles", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsAuditoriaRolesAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsAuditoriaUsuarios", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> ConsAuditoriaUsuariosAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsParamAplicacion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsParamAplicacionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/IsPermisoAplTransOpcion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> IsPermisoAplTransOpcionAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsSemillaPart", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsSemillaPartAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/consTransaccionesMT", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> consTransaccionesMTAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/EjecutaTransacciones", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EjecutaTransaccionesAsync(string XmlSeccion, string XmlEntrada);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/EjecutaTransaccion", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EjecutaTransaccionAsync(string XmlSeccion, string XmlEntrada);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/EjecutaTransaccionDS", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> EjecutaTransaccionDSAsync(string XmlSeccion, string XmlEntrada);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/CambioClave", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> CambioClaveAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getGruposAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> getGruposADAsync(string PI_xmlSession);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ConsUsuarioAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ConsUsuarioADAsync(string PI_xmlSession);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/CrearUsuarioAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> CrearUsuarioADAsync(string PI_xmlSession, string PI_xmlUsuario);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ModificaUsuarioAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> ModificaUsuarioADAsync(string PI_xmlSession, string PI_xmlUsuario);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/EliminaUsuarioAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> EliminaUsuarioADAsync(string PI_xmlSession);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getUsuariosAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> getUsuariosADAsync(string PI_xmlSession);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getUnidadOrgAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> getUnidadOrgADAsync(string PI_xmlSession);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/AtributosAD", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<object> AtributosADAsync(string usuario);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getXmlConfig", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<ArrayOfXElement> getXmlConfigAsync(string opcion);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/setXmlConfig", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> setXmlConfigAsync(string Opcion, ArrayOfXElement PI_Ds);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/getUrlWSFrameWork", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> getUrlWSFrameWorkAsync();

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/RegistraUserLocal", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraUserLocalAsync(string PI_xmlParam);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/RegistraUserRemoto", ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Task<string> RegistraUserRemotoAsync(string PI_xmlParam);
    }
}
