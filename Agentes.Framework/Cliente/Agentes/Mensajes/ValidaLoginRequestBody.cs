namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.sipecom.com/WSFrameWork/")]
    public partial class ValidaLoginRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string PI_xmlParam;

        public ValidaLoginRequestBody()
        {
        }

        public ValidaLoginRequestBody(string PI_xmlParam)
        {
            this.PI_xmlParam = PI_xmlParam;
        }
    }
}
