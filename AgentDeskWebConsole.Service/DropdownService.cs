using AgentDeskWebConsole.Infrastructure.IService;
using AgentDeskWebConsole.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using System;

namespace AgentDeskWebConsole.Service
{
    public class DropdownService : IDropdownService
    {
        #region Declaration

        private readonly IHttpClientService _httpClientService;
        private readonly string baseUrl;

        public DropdownService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        #endregion

        #region Status
        public dynamic GetStatusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Active", Value = "Active" },
                new SelectListItem { Text = "Disabled", Value = "Disabled" }
            };
        }

        #endregion

        #region Pagenation
        public dynamic GetPageSizeOptions(int selectedSize = 20)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "All", Value = "-1" }
            };

            list.ForEach(item => item.Selected = item.Value == selectedSize.ToString());
            return list;
        }

        #endregion

        #region Roles
        public async Task<dynamic> GetRolesOptionsAsync(int roleId = 0)
        {
            var mSelectListItems = new List<SelectListItem>();
            var systemOwnerRoleId = EnumsHelper.GetEnumIndex(Enums.Role.System_Owner);
            var isSystemOwner = roleId == systemOwnerRoleId;

            var mRoles = await _httpClientService.GetAsync<List<Domain.Role>>(
                url: $"{baseUrl}/Role",
                token: HttpContextHelper.LoginUser?.Token).ConfigureAwait(false);

            var filteredRoles = mRoles.Where(role => isSystemOwner || role.Id != systemOwnerRoleId).ToList();

            foreach (var filteredRole in filteredRoles)
            {
                mSelectListItems.Add(new SelectListItem() { Text = filteredRole.Name, Value = Convert.ToString(filteredRole.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Agency Type
        public dynamic GetAgencyTypeOptionsAsync()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Life Insurers", Value = "Life Insurers" },
                new SelectListItem { Text = "Standalone Health Insurers", Value = "Standalone Health Insurers" },
                new SelectListItem { Text = "Specialised Insurers", Value = "Specialised Insurers" },
                new SelectListItem { Text = "General Insurers", Value = "General Insurers" }
            };
        }

        #endregion

        #region Company
        public async Task<dynamic> GetCompanyOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mCompanies = await _httpClientService.GetAsync<List<Domain.Company>>(
                url: $"{baseUrl}/Company/GetActiveCompanies",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mCompany in mCompanies)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mCompany.Name, Value = Convert.ToString(mCompany.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Policy Type

        public async Task<dynamic> GetPolicyTypeOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mPolicyTypes = await _httpClientService.GetAsync<List<Domain.PolicyType>>(
                url: $"{baseUrl}/PolicyType",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mPolicyType in mPolicyTypes)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mPolicyType.Name, Value = Convert.ToString(mPolicyType.Id) });
            }
            return mSelectListItems;
        }

        #endregion

        #region Policy Status

        public async Task<dynamic> GetPolicyStatusOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mPolicyStatus = await _httpClientService.GetAsync<List<Domain.PolicyStatus>>(
                url: $"{baseUrl}/PolicyStatus",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mPolicyStatu in mPolicyStatus)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mPolicyStatu.Name, Value = Convert.ToString(mPolicyStatu.Id) });
            }
            return mSelectListItems;
        }

        #endregion

        #region Policy Status
        public async Task<dynamic> GetPolicyStatusOptionsAsync(string policyTypes, string policyStatus = null)
        {
            var mSelectListItems = new List<SelectListItem>();

            var mpolicy = await _httpClientService.GetAsync<List<Domain.PolicyStatus>>(
                    url: $"{baseUrl}/PolicyStatus/GetActiveBy/PolicyTypes/{policyTypes}",
                    token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mpolicyStatus in mpolicy)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mpolicyStatus.Name, Value = mpolicyStatus.Id.ToString() });
                //mSelectListItems.Add(new SelectListItem() { Text = mpolicyStatus.Name, Value = mpolicyStatus.Name });
            }
            if (policyStatus.IsNotNullOrEmpty())
                mSelectListItems.ForEach(item => item.Selected = item.Value == policyStatus);

            return mSelectListItems;
        }
        #endregion

        #region Pay Mod

        public dynamic GetPayModeOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Monthly", Value = "Monthly" },
                new SelectListItem { Text = "Quarterly", Value = "Quarterly" },
                new SelectListItem { Text = "Half Year", Value = "Half Year" },
                new SelectListItem { Text = "Yearly", Value = "Yearly" }
            };
        }
        #endregion

        #region Gender
        public dynamic GetGenderOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = "Male" },
                new SelectListItem { Text = "Female", Value = "Female" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };
        }
        #endregion

        #region Marital Status

        public dynamic GetMaritalStatusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Single", Value = "Single" },
                new SelectListItem { Text = "Married", Value = "Married" },
                new SelectListItem { Text = "Widowed", Value = "Widowed" },
                new SelectListItem { Text = "Divorced", Value = "Divorced" }
            };
        }
        #endregion

        #region Group

        public async Task<dynamic> GetGroupOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mGroups = await _httpClientService.GetAsync<List<Domain.Group>>(
                url: $"{baseUrl}/Group/GetActiveGroup",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mGroup in mGroups)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mGroup.Name, Value = Convert.ToString(mGroup.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Account Type
        public dynamic GetAccountTypeOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Savings Account", Value = "Savings Account" },
                new SelectListItem { Text = "Current Account", Value = "Current Account" },
                new SelectListItem { Text = "Salary Account", Value = "Salary Account" }
            };
        }
        #endregion
    }
}
