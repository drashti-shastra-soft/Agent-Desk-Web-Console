using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IPolicyDocumentService
    {
        Task<Domain.PolicyDocument> CreateAsync(Domain.PolicyDocument mPolicyDocument);
        Task<Domain.PolicyDocument> GetAsync(int id);
        Task<List<Domain.PolicyDocument>> Fetch(int policyId);

        Task DeleteAsync(int id);
    }
}
