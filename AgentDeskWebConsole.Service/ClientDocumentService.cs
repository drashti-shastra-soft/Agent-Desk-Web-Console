using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class ClientDocumentService : IClientDocumentService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientDocumentService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.ClientDocument> CreateAsync(Domain.ClientDocument mClientDocument)
        {
            return await _httpClientService.PostAsync<Domain.ClientDocument>(
                            url: $"{_baseUrl}/ClientDocument",
                            obj: mClientDocument,
                            token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.ClientDocument> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.ClientDocument>(
                         url: $"{_baseUrl}/ClientDocument/{id}",
                         token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.ClientDocument>> Fetch(int clientId)
        {
            var result = await _httpClientService.GetAsync<List<Domain.ClientDocument>>(
                        url: $"{_baseUrl}/ClientDocument/Fetch/{clientId}",
                        token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                            url: $"{_baseUrl}/ClientDocument/{id}/LoginUser/{_loginUserId}",
                            token: _token
            ).ConfigureAwait(false);
        }
    }

}
