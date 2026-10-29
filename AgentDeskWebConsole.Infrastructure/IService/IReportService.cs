using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IReportService
    {
        Task<Domain.PolicyLister> LifeInsuranceList(Domain.PolicyLister mLister);

        Task<Domain.PolicyLister> HealthInsuranceList(Domain.PolicyLister mLister);

        Task<Domain.PolicyLister> PropertyInsuranceList(Domain.PolicyLister mLister);

        Task<Domain.PolicyLister> VehicleInsuranceList(Domain.PolicyLister mLister);

        Task<List<Domain.Report>> GetAsync();
    }
}
