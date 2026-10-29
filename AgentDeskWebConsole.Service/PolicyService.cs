using AgentDeskWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class PolicyService : IPolicyService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PolicyService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PolicyLister> GetAllAsync(Domain.PolicyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PolicyLister>(
                    url: $"{_baseUrl}/Policy/GetAll",
                    obj: mLister,
                    token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.Policy> CreateAsync(Domain.Policy mPolicy)
        {
            return await _httpClientService.PostAsync<Domain.Policy>(
                    url: $"{_baseUrl}/Policy",
                    obj: mPolicy,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Policy> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Policy>(
                  url: $"{_baseUrl}/Policy/{id}",
                  token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/Policy/{id}/LoginUser/{_loginUserId}",
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Policy> GetPdfAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Policy>(
                url: $"{_baseUrl}/Policy/GetPdf/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<bool> CheckPolicyNumberExist(Domain.Policy mPolicy)
        {
            var isExist = await _httpClientService.PostAsync<Domain.Policy, bool>(
                url: $"{_baseUrl}/Policy/CheckPolicyNumberExist",
                obj: mPolicy,
                token: _token
            ).ConfigureAwait(false);
            return isExist;
        }

        public async Task<Domain.Policy> ImportPolicy(Domain.Policy mPolicy)
        {
            return await _httpClientService.PostAsync<Domain.Policy>(
                    url: $"{_baseUrl}/Policy/ImportPolicy",
                    obj: mPolicy,
                    token: _token
            ).ConfigureAwait(false);
        }
    }
}
