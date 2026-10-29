using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using AgentDeskWebConsole.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;
        private readonly IDropdownService _dropdownService;

        public DashboardController(IDashboardService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            ViewBag.ShowCompany = StaticMethods.IsSystemOwner();
            return View();
        }

        public ActionResult GetLifeInsuranceRenewal()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                var list = _service.GetAllLifeInsuranceAsync().GetAwaiter().GetResult().Take(10).ToList();
                return PartialView("_GetLifeInsuranceRenewalList", list);
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.PayMode = _dropdownService.GetPayModeOptions();
            }
            return PartialView("_GetLifeInsuranceRenewalList", new List<Domain.Policy>());
        }

        public ActionResult GetHealthInsuranceRenewal()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                var list = _service.GetAllHealthInsuranceAsync().GetAwaiter().GetResult().Take(10).ToList();
                return PartialView("_GetHealthInsuranceRenewalList", list);
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.PayMode = _dropdownService.GetPayModeOptions();
            }
            return PartialView("_GetHealthInsuranceRenewalList", new List<Domain.Policy>());
        }

        public ActionResult GetPropertyInsuranceRenewal()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                var list = _service.GetAllPropertyInsuranceAsync().GetAwaiter().GetResult().Take(10).ToList();
                return PartialView("_GetPropertyInsuranceRenewalList", list);
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.PayMode = _dropdownService.GetPayModeOptions();
            }
            return PartialView("_GetPropertyInsuranceRenewalList", new List<Domain.Policy>());
        }


        public ActionResult GetVehicleInsuranceRenewal()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                var list = _service.GetAllVehicleInsuranceAsync().GetAwaiter().GetResult().Take(10).ToList();
                return PartialView("_GetVehicleInsuranceRenewalList", list);
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.PayMode = _dropdownService.GetPayModeOptions();
            }
            return PartialView("_GetVehicleInsuranceRenewalList", new List<Domain.Policy>());
        }

        public JsonResult GetTotalLifeInsurance()
        {
            int count = _service.GetTotalLifeInsuranceAsync().Result;
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHealthInsuranceCount()
        {
            var count = _service.GetHealthInsuranceAsync().GetAwaiter().GetResult();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPropertyInsuranceCount()
        {
            var count = _service.GetPropertyInsuranceAsync().GetAwaiter().GetResult();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleInsuranceCount()
        {
            var count = _service.GetVehicleInsuranceAsync().GetAwaiter().GetResult();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDashboardCounts()
        {
            var result = new Domain.DashboardCount
            {
                LifeCount = await _service.GetTotalLifeInsuranceAsync(),
                HealthCount = await _service.GetHealthInsuranceAsync(),
                PropertyCount = await _service.GetPropertyInsuranceAsync(),
                VehicleCount = await _service.GetVehicleInsuranceAsync()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
