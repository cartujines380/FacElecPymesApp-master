namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.sipecom.com/WSFrameWork/")]
    public partial class ResetClaveResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string ResetClaveResult;

        public ResetClaveResponseBody()
        {
        }

        public ResetClaveResponseBody(string ResetClaveResult)
        {
            this.ResetClaveResult = ResetClaveResult;
        }
    }
}
