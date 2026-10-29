using AgentDeskWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;

namespace AgentDeskWebConsole.Service
{
    public class ReportService : IReportService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ReportService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PolicyLister> LifeInsuranceList(Domain.PolicyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyLister>(
                url: $"{_baseUrl}/Report/GetAllLifeInsuranceList",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.PolicyLister> HealthInsuranceList(Domain.PolicyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyLister>(
                url: $"{_baseUrl}/Report/GetAllHealthInsuranceList",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.PolicyLister> PropertyInsuranceList(Domain.PolicyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyLister>(
                url: $"{_baseUrl}/Report/GetAllPropertyInsuranceList",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.PolicyLister> VehicleInsuranceList(Domain.PolicyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyLister>(
                url: $"{_baseUrl}/Report/GetAllVehicleInsuranceList",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Domain.Report>> GetAsync()
        {
            var result = await _httpClientService.GetAsync<List<Domain.Report>>(
                url: $"{_baseUrl}/Report/PremiumCollectionReport",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }
    }
}
