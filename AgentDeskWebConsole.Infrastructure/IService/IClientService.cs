using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IClientService
    {
        Task<Domain.ClientLister> GetAllAsync(Domain.ClientLister mLister);

        Task<Domain.Client> CreateAsync(Domain.Client mClient);

        Task<Domain.Client> GetAsync(int id);

        Task DeleteAsync(int id);

        Task UpdateAddressAsync(int id, int addressId);
    }
}
