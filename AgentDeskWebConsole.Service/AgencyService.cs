using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class AgencyService : IAgencyService
    {

        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public AgencyService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.AgencyLister> GetAllAsync(Domain.AgencyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.AgencyLister, Domain.AgencyLister>(
                url: $"{_baseUrl}/Agency/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Agency> CreateAsync(Domain.Agency mAgency)
        {
            return await _httpClientService.PostAsync<Domain.Agency>(
               url: $"{_baseUrl}/Agency",
               obj: mAgency,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Agency> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Agency>(
                url: $"{_baseUrl}/Agency/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Agency/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
