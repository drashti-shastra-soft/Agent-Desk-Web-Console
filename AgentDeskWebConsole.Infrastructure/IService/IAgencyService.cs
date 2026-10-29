using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IAgencyService
    {
        Task<Domain.AgencyLister> GetAllAsync(Domain.AgencyLister mLister);

        Task<Domain.Agency> CreateAsync(Domain.Agency mAgency);

        Task<Domain.Agency> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
