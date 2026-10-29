using AgentDeskWebConsole.Infrastructure.IService;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Helper;
using System.Dynamic;
using System.Web.Mvc;
using System;

namespace AgentDeskWebConsole.Controllers
{
    public class AgencyController : Controller
    {
        private readonly IAgencyService _service;
        private readonly IDropdownService _dropdownService;

        public AgencyController(IAgencyService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            ViewBag.ShowCompany = StaticMethods.IsSystemOwner();
            return View(new Domain.AgencyLister());
        }

        public ActionResult List(Domain.AgencyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.CompanyId = StaticMethods.ResolveCompanyId(mLister.SearchCriteria.CompanyId);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_List", mLister);
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.AgencyType = _dropdownService.GetAgencyTypeOptionsAsync();
            return PartialView("_Create", new Domain.Agency());
        }

        [HttpPost]
        public ActionResult Create(Domain.Agency mAgency)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAgency = StaticMethods.ApplyDefaults(mAgency);
                mAgency = StaticMethods.SetAuditOnUpsert(mAgency);
                
                mAgency = _service.CreateAsync(mAgency).GetAwaiter().GetResult();

                if (mAgency != null && mAgency.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Agency has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving agency!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.AgencyType = new SelectList(_dropdownService.GetAgencyTypeOptionsAsync(), "Value", "Text");
            }
            return PartialView("_Create", mAgency);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mAgency = new Domain.Agency();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAgency = _service.GetAsync(id).GetAwaiter().GetResult();

                if (mAgency == null || !StaticMethods.CanAccess(mAgency.CompanyId))
                    return HttpNotFound();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.AgencyType = new SelectList(_dropdownService.GetAgencyTypeOptionsAsync(), "Value", "Text");
            }

            return PartialView("_Create", mAgency);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Agency has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting agency!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }


    }
}