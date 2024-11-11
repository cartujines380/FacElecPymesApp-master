namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ResetClaveRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ResetClave", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public ResetClaveRequestBody Body;

        public ResetClaveRequest()
        {
        }

        public ResetClaveRequest(ResetClaveRequestBody Body)
        {
            this.Body = Body;
        }
    }
}
