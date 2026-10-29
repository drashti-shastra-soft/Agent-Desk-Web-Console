using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class PolicyNomineeService : IPolicyNomineeService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PolicyNomineeService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PolicyNomineeLister> GetAllAsync(Domain.PolicyNomineeLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyNomineeLister>(
                    url: $"{_baseUrl}/PolicyNominee/GetAll",
                    obj: mLister,
                    token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.PolicyNominee> CreateAsync(Domain.PolicyNominee mPolicyNominee)
        {
            return await _httpClientService.PostAsync<Domain.PolicyNominee>(
                    url: $"{_baseUrl}/PolicyNominee",
                    obj: mPolicyNominee,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.PolicyNominee> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.PolicyNominee>(
                  url: $"{_baseUrl}/PolicyNominee/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/PolicyNominee/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }

    }
}
