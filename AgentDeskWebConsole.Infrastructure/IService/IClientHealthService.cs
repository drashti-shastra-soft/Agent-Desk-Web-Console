using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IClientHealthService
    {
        Task<Domain.ClientHealth> CreateAsync(Domain.ClientHealth mClientHealth);
        Task<Domain.ClientHealth> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
