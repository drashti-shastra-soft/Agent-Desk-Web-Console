using AgentDeskWebConsole.Domain;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class ClientIdentificationService : IClientIdentificationService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientIdentificationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.ClientIdentification> CreateAsync(Domain.ClientIdentification mClientIdentification)
        {
            return await _httpClientService.PostAsync<Domain.ClientIdentification>(
                    url: $"{_baseUrl}/ClientIdentification",
                    obj: mClientIdentification,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.ClientIdentification> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.ClientIdentification> (
                  url: $"{_baseUrl}/ClientIdentification/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/ClientIdentification/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }
    }
}
