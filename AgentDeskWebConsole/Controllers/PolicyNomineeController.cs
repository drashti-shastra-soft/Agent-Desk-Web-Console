using AgentDeskWebConsole.Helper;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    public class PolicyNomineeController : Controller
    {
        private readonly IPolicyNomineeService _service;
        private readonly IDropdownService _dropdownService;

        public PolicyNomineeController(IPolicyNomineeService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index(int policyId)
        {
            if (policyId == 0)
                return RedirectToAction("Index", "Policy");

            var mLister = new Domain.PolicyNomineeLister();
            mLister.SearchCriteria.PolicyId = policyId;

            TempData["PolicyId"] = policyId;

            return View(mLister);
        }


        public ActionResult List(Domain.PolicyNomineeLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
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
            var mPolicyNominee = new Domain.PolicyNominee();
            mPolicyNominee.PolicyId = Convert.ToInt32(TempData.Peek("PolicyId"));

            return PartialView("_Create", mPolicyNominee);
        }

        [HttpPost]
        public ActionResult Create(Domain.PolicyNominee mPolicyNominee)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicyNominee = _service.CreateAsync(mPolicyNominee).GetAwaiter().GetResult();

                if (mPolicyNominee != null && mPolicyNominee.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Policy Nominee has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving policy nominee!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Create", mPolicyNominee);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mPolicyNominee = new Domain.PolicyNominee();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicyNominee = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return PartialView("_Create", mPolicyNominee);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Policy Nominee has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting policy nominee!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}