namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.sipecom.com/WSFrameWork/")]
    public partial class ValidaLoginResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string ValidaLoginResult;

        public ValidaLoginResponseBody()
        {
        }

        public ValidaLoginResponseBody(string ValidaLoginResult)
        {
            this.ValidaLoginResult = ValidaLoginResult;
        }
    }
}
