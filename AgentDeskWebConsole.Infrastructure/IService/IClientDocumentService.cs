using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IClientDocumentService
    {
        Task<Domain.ClientDocument> CreateAsync(Domain.ClientDocument mClientDocument);

        Task<Domain.ClientDocument> GetAsync(int id);

        Task<List<Domain.ClientDocument>> Fetch(int clientId);

        Task DeleteAsync(int id);
    }
}
