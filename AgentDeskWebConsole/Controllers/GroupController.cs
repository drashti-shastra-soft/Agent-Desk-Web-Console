using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Helper;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class GroupController : Controller
    {
        private readonly IGroupService _service;
        private readonly IDropdownService _dropdownService;

        public GroupController(IGroupService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            ViewBag.ShowCompany = StaticMethods.IsSystemOwner();
            return View(new Domain.GroupLister());
        }

        public ActionResult List(Domain.GroupLister mLister)
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
            return PartialView("_Create", new Domain.Group());
        }

        [HttpPost]
        public ActionResult Create(Domain.Group mGroup)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mGroup = StaticMethods.SetAuditOnUpsert(mGroup);
                mGroup = StaticMethods.ApplyDefaults(mGroup);
                mGroup = _service.CreateAsync(mGroup).GetAwaiter().GetResult();

                if (mGroup != null && mGroup.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Group has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving group!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Create", mGroup);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mGroup = new Domain.Group();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mGroup = _service.GetAsync(id).GetAwaiter().GetResult();

                if (mGroup == null || !StaticMethods.CanAccess(mGroup.CompanyId))
                    return HttpNotFound();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return PartialView("_Create", mGroup);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Group has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting group!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

    }
}