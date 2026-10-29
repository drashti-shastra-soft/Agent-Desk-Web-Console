using AgentDeskWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Service
{
    public class GroupService : IGroupService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public GroupService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.GroupLister> GetAllAsync(Domain.GroupLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.GroupLister, Domain.GroupLister>(
                url: $"{_baseUrl}/Group/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Group> CreateAsync(Domain.Group mGroup)
        {
            return await _httpClientService.PostAsync<Domain.Group>(
               url: $"{_baseUrl}/Group",
               obj: mGroup,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Group> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Group>(
                url: $"{_baseUrl}/Group/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Group/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
