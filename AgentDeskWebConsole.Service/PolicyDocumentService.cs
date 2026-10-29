using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class PolicyDocumentService : IPolicyDocumentService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PolicyDocumentService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.PolicyDocument> CreateAsync(Domain.PolicyDocument mPolicyDocument)
        {
            return await _httpClientService.PostAsync<Domain.PolicyDocument>(
                            url: $"{_baseUrl}/PolicyDocument",
                            obj: mPolicyDocument,
                            token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.PolicyDocument> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.PolicyDocument>(
                         url: $"{_baseUrl}/PolicyDocument/{id}",
                         token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.PolicyDocument>> Fetch(int policyId)
        {
            var result = await _httpClientService.GetAsync<List<Domain.PolicyDocument>>(
                        url: $"{_baseUrl}/PolicyDocument/Fetch/{policyId}",
                        token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                            url: $"{_baseUrl}/PolicyDocument/{id}/LoginUser/{_loginUserId}",
                            token: _token
            ).ConfigureAwait(false);
        }
    }
}

