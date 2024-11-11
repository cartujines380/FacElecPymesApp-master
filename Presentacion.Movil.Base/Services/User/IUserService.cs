using System;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.User
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(string authToken);
    }
}
