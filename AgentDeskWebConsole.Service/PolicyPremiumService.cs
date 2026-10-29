using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class PolicyPremiumService : IPolicyPremiumService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PolicyPremiumService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task Upsert(List<Domain.PolicyPremium> mPolicyPremium)
        {
            await _httpClientService.PostAsync<List<Domain.PolicyPremium>>(
                    url: $"{_baseUrl}/PolicyPremium/Upsert",
                    obj: mPolicyPremium,
                    token: _token
            ).ConfigureAwait(false);
        }

    }

}
