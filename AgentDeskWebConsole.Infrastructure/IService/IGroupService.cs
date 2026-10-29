using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IGroupService
    {
        Task<Domain.GroupLister> GetAllAsync(Domain.GroupLister mLister);

        Task<Domain.Group> CreateAsync(Domain.Group mGroup);

        Task<Domain.Group> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
