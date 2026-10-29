using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IClientFinancialInfoService
    {
        Task<Domain.ClientFinancialInfo> CreateAsync(Domain.ClientFinancialInfo mClientFinancialInfo);
        Task<Domain.ClientFinancialInfo> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
