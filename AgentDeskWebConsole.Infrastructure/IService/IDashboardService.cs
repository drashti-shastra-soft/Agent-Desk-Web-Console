using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IDashboardService
    {

        Task<List<Domain.Policy>> GetAllLifeInsuranceAsync();

        Task<List<Domain.Policy>> GetAllHealthInsuranceAsync();

        Task<List<Domain.Policy>> GetAllPropertyInsuranceAsync();

        Task<List<Domain.Policy>> GetAllVehicleInsuranceAsync();

        Task<int> GetTotalLifeInsuranceAsync();

        Task<int> GetHealthInsuranceAsync();

        Task<int> GetPropertyInsuranceAsync();

        Task<int> GetVehicleInsuranceAsync();
    }
}
