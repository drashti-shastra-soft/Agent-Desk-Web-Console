using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IPolicyService
    {
        Task<Domain.PolicyLister> GetAllAsync(Domain.PolicyLister mLister);

        Task<Domain.Policy> CreateAsync(Domain.Policy mPolicy);

        Task<Domain.Policy> GetAsync(int id);

        Task<Domain.Policy> GetPdfAsync(int id);

        Task<bool> CheckPolicyNumberExist(Domain.Policy mPolicy);

        Task DeleteAsync(int id);
        Task<Domain.Policy> ImportPolicy(Domain.Policy mPolicy);
    }
}
