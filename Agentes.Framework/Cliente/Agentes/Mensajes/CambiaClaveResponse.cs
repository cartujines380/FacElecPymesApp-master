namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CambiaClaveResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CambiaClaveResponse", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public CambiaClaveResponseBody Body;

        public CambiaClaveResponse()
        {
        }

        public CambiaClaveResponse(CambiaClaveResponseBody Body)
        {
            this.Body = Body;
        }
    }
}
