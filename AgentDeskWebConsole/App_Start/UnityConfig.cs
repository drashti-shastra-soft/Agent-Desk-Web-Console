using AgentDeskWebConsole.Infrastructure.IService;
using AgentDeskWebConsole.Service;
using System.Web.Mvc;
using Unity.Mvc5;
using Unity;

namespace AgentDeskWebConsole
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAgencyService, AgencyService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IClientDocumentService, ClientDocumentService>();
            container.RegisterType<IClientFinancialInfoService, ClientFinancialInfoService>();
            container.RegisterType<IClientHealthService, ClientHealthService>();
            container.RegisterType<IClientIdentificationService, ClientIdentificationService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<ICompanyService, CompanyService>();
            container.RegisterType<ICSVReaderService, CSVReaderService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<IDropdownService, DropdownService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IHttpClientService, HttpClientService>();
            container.RegisterType<IPolicyDocumentService, PolicyDocumentService>();
            container.RegisterType<IPolicyNomineeService, PolicyNomineeService>();
            container.RegisterType<IPolicyPremiumService, PolicyPremiumService>();
            container.RegisterType<IPolicyService, PolicyService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IUserService, UserService>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}