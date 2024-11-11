using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();

        string CreateLogoutRequest(string token);

        Task<UserToken> GetTokenAsync(string code);
    }
}
