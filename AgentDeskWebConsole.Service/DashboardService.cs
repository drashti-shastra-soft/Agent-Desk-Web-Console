using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public DashboardService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<List<Domain.Policy>> GetAllLifeInsuranceAsync()
        {
            var result = await _httpClientService.GetAsync<List<Domain.Policy>>(
                  url: $"{_baseUrl}/Dashboard/GetAllLifeInsurance",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.Policy>> GetAllHealthInsuranceAsync()
        {
            var result = await _httpClientService.GetAsync<List<Domain.Policy>>(
                  url: $"{_baseUrl}/Dashboard/GetAllHealthInsurance",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.Policy>> GetAllPropertyInsuranceAsync()
        {
            var result = await _httpClientService.GetAsync<List<Domain.Policy>>(
                  url: $"{_baseUrl}/Dashboard/GetAllPropertyInsurance",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.Policy>> GetAllVehicleInsuranceAsync()
        {
            var result = await _httpClientService.GetAsync<List<Domain.Policy>>(
                  url: $"{_baseUrl}/Dashboard/GetAllVehicleInsurance",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<int> GetTotalLifeInsuranceAsync()
        {
            return await _httpClientService.GetAsync<int>(
                url: $"{_baseUrl}/Dashboard/GetTotalLifeInsurance",
                token: _token
            );
        }

        public async Task<int> GetHealthInsuranceAsync()
        {
            return await _httpClientService.GetAsync<int>(
                url: $"{_baseUrl}/Dashboard/GetHealthInsuranceCount",
                token: _token
            );
        }

        public async Task<int> GetPropertyInsuranceAsync()
        {
            return await _httpClientService.GetAsync<int>(
                url: $"{_baseUrl}/Dashboard/GetPropertyInsuranceCount",
                token: _token
            );
        }

        public async Task<int> GetVehicleInsuranceAsync()
        {
            return await _httpClientService.GetAsync<int>(
                url: $"{_baseUrl}/Dashboard/GetVehicleInsuranceCount",
                token: _token
            );
        }
    }
}
