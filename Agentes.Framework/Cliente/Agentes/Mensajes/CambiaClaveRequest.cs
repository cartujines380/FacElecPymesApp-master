namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CambiaClaveRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CambiaClave", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public CambiaClaveRequestBody Body;

        public CambiaClaveRequest()
        {
        }

        public CambiaClaveRequest(CambiaClaveRequestBody Body)
        {
            this.Body = Body;
        }
    }
}
