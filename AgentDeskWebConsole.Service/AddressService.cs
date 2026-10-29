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
    public class AddressService : IAddressService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public AddressService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.Address> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Address>(
                url: $"{_baseUrl}/Address/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.Address> CreateAsync(Domain.Address mAddress)
        {
            return await _httpClientService.PostAsync<Domain.Address, Domain.Address>(
                url: $"{_baseUrl}/Address",
                obj: mAddress,
                token: _token
            ).ConfigureAwait(false);
        }


        public async Task<Domain.Address> GetAddressAsync(int clientId)
        {
            var result = await _httpClientService.GetAsync<Domain.Address>(
                url: $"{_baseUrl}/Address/Get/Client/Id/{clientId}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }
    }
}
