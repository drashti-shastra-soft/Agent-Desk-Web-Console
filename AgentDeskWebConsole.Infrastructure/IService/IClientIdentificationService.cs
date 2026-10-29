using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IClientIdentificationService
    {
        Task<Domain.ClientIdentification> CreateAsync(Domain.ClientIdentification mClientIdentification);
        Task<Domain.ClientIdentification> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
