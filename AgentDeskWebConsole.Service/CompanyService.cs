using AgentDeskWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace AgentDeskWebConsole.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public CompanyService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.CompanyLister> GetAllAsync(Domain.CompanyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.CompanyLister, Domain.CompanyLister>(
                url: $"{_baseUrl}/Company/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Company> CreateAsync(Domain.Company mCompany)
        {
            return await _httpClientService.PostAsync<Domain.Company, Domain.Company>(
                url: $"{_baseUrl}/Company",
                obj: mCompany,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Company> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Company>(
                url: $"{_baseUrl}/Company/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Company/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

    }
}
