using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Helper;
using AgentDeskWebConsole.Infrastructure.IService;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class ReportController : Controller
    {
        private readonly IReportService _service;
        private readonly IDropdownService _dropdownService;

        public ReportController(IReportService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index(string type)
        {
            var model = new Domain.PolicyLister();
            model.Type = type;
            return View(model);
        }

        public ActionResult LifeInsuranceReportList(Domain.PolicyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.LifeInsuranceList(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_GetLifeInsuranceReportList", mLister);
        }

        public ActionResult HealthInsuranceReportList(Domain.PolicyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.HealthInsuranceList(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_GetHealthInsuranceReportList", mLister);
        }

        public ActionResult PropertyInsuranceReportList(Domain.PolicyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.PropertyInsuranceList(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_GetPropertyInsuranceReportList", mLister);
        }

        public ActionResult VehicleInsuranceReportList(Domain.PolicyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.VehicleInsuranceList(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }

            return PartialView("_GetVehicleInsuranceReportList", mLister);
        }

    }
}