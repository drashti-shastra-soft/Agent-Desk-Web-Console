using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IChangePasswordService
    {
        Task Initiate(Domain.ChangePassword mChangePassword);
    }
}
