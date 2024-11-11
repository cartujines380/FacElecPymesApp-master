using System;
using System.ServiceModel;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes
{
    [ServiceContract(Namespace = "http://www.sipecom.com/WSFrameWork/", ConfigurationName = "ServicioClienteSoap")]
    public interface ServicioClienteSoap
    {

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ValidaLogin", ReplyAction = "*")]
        Task<ValidaLoginResponse> ValidaLoginAsync(ValidaLoginRequest request);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/CambiaClave", ReplyAction = "*")]
        Task<CambiaClaveResponse> CambiaClaveAsync(CambiaClaveRequest request);

        [OperationContract(Action = "http://www.sipecom.com/WSFrameWork/ResetClave", ReplyAction = "*")]
        Task<ResetClaveResponse> ResetClaveAsync(ResetClaveRequest request);
    }
}
