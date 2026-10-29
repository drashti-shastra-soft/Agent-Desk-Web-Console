using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class ClientFinancialInfoService : IClientFinancialInfoService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientFinancialInfoService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.ClientFinancialInfo> CreateAsync(Domain.ClientFinancialInfo mClientFinancialInfo)
        {
            return await _httpClientService.PostAsync<Domain.ClientFinancialInfo>(
                    url: $"{_baseUrl}/ClientFinancialInfo",
                    obj: mClientFinancialInfo,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.ClientFinancialInfo> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.ClientFinancialInfo>(
                  url: $"{_baseUrl}/FinancialInfo/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/FinancialInfo/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }
    }
}
