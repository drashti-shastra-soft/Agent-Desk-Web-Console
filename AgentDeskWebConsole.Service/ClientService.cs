using AgentDeskWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace AgentDeskWebConsole.Service
{
    public class ClientService : IClientService
    {

        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.ClientLister> GetAllAsync(Domain.ClientLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.ClientLister>(
                    url: $"{_baseUrl}/Client/GetAll",
                    obj: mLister,
                    token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.Client> CreateAsync(Domain.Client mClient)
        {
            return await _httpClientService.PostAsync<Domain.Client>(
                    url: $"{_baseUrl}/Client",
                    obj: mClient,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Client> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Client>(
                  url: $"{_baseUrl}/Client/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/Client/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task UpdateAddressAsync(int id, int addressId)
        {
            await _httpClientService.PatchAsync(
                url: $"{_baseUrl}/Client/{id}/AddressId/{addressId}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

    }
}
