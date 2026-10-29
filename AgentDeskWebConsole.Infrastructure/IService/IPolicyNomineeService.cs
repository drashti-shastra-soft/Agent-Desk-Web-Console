using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IPolicyNomineeService
    {
        Task<Domain.PolicyNomineeLister> GetAllAsync(Domain.PolicyNomineeLister mLister);
        Task<Domain.PolicyNominee> CreateAsync(Domain.PolicyNominee mPolicyNominee);
        Task<Domain.PolicyNominee> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
