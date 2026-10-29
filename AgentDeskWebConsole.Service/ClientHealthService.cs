using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class ClientHealthService : IClientHealthService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientHealthService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.ClientHealth> CreateAsync(Domain.ClientHealth mClientHealth)
        {
            return await _httpClientService.PostAsync<Domain.ClientHealth>(
                    url: $"{_baseUrl}/ClientHealth",
                    obj: mClientHealth,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.ClientHealth> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.ClientHealth>(
                  url: $"{_baseUrl}/ClientHealth/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/ClientHealth/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }
    }
}
