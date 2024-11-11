namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ResetClaveResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ResetClaveResponse", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public ResetClaveResponseBody Body;

        public ResetClaveResponse()
        {
        }

        public ResetClaveResponse(ResetClaveResponseBody Body)
        {
            this.Body = Body;
        }
    }
}
