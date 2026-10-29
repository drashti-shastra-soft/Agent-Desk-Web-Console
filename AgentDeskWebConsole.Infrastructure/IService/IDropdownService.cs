using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IDropdownService
    {
        dynamic GetStatusOptions();

        dynamic GetPageSizeOptions(int selectedSize = 20);

        Task<dynamic> GetRolesOptionsAsync(int roleId = 0);

        Task<dynamic> GetCompanyOptionsAsync();

        Task<dynamic> GetPolicyTypeOptionsAsync();

        Task<dynamic> GetPolicyStatusOptionsAsync();

        Task<dynamic> GetPolicyStatusOptionsAsync(string policyType, string policyStatus = null);

        dynamic GetPayModeOptions();

        dynamic GetGenderOptions();

        dynamic GetMaritalStatusOptions();

        Task<dynamic> GetGroupOptionsAsync();

        dynamic GetAccountTypeOptions();

        dynamic GetAgencyTypeOptionsAsync();

    }
}
