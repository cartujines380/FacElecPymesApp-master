namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ValidaLoginRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ValidaLogin", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public ValidaLoginRequestBody Body;

        public ValidaLoginRequest()
        {
        }

        public ValidaLoginRequest(ValidaLoginRequestBody Body)
        {
            this.Body = Body;
        }
    }
}
