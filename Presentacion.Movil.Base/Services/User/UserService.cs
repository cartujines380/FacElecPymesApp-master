using System;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.Identity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRequestProvider m_requestProvider;
        private readonly IIdentitySettings m_settings;

        public UserService(IRequestProvider requestProvider, IIdentitySettings settings)
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<UserInfo> GetUserInfoAsync(string authToken)
        {
            var uri = UriHelper.CombineUri(m_settings.UserInfoEndpoint);

            var userInfo = await m_requestProvider.GetAsync<UserInfo>(uri, authToken);
            return userInfo;
        }
    }
}
