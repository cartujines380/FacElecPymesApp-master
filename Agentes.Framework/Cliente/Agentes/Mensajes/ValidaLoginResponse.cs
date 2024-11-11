namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes
{
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ValidaLoginResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ValidaLoginResponse", Namespace = "http://www.sipecom.com/WSFrameWork/", Order = 0)]
        public ValidaLoginResponseBody Body;

        public ValidaLoginResponse()
        {
        }

        public ValidaLoginResponse(ValidaLoginResponseBody Body)
        {
            this.Body = Body;
        }
    }
}
