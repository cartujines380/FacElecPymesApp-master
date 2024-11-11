namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.sipecom.com/WSFrameWork/")]
    public partial class CambiaClaveResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CambiaClaveResult;

        public CambiaClaveResponseBody()
        {
        }

        public CambiaClaveResponseBody(string CambiaClaveResult)
        {
            this.CambiaClaveResult = CambiaClaveResult;
        }
    }
}
