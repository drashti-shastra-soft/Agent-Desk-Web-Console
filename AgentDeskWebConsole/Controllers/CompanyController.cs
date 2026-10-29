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
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;
        private readonly IDropdownService _dropdownService;

        public CompanyController(ICompanyService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            //ViewBag.ShowCompany = false;
            //if (HttpContextHelper.LoginUser.RoleId == EnumsHelper.GetEnumIndex(Enums.Role.System_Owner))
            //    ViewBag.ShowCompany = true;

            return View(new Domain.CompanyLister());
        }

        public ActionResult List(Domain.CompanyLister mLister)
        {
            try
            {
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = EnumsHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = ex.Message;
            }
            finally
            {
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                //ViewBag.Companies = _dropdownService.GetCompanyOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.Companies = _dropdownService.GetCompanyOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", new Domain.Company());
        }

        [HttpPost]
        public ActionResult Create(Domain.Company mCompany)
        {
            try
            {
                mCompany = _service.CreateAsync(mCompany).GetAwaiter().GetResult();

                bool success = mCompany != null && mCompany.Id > 0;
                ViewBag.NotificationType = success
                    ? EnumsHelper.GetEnumString(Enums.Result.Success).StrToLower()
                    : EnumsHelper.GetEnumString(Enums.Result.Error).StrToLower();

                ViewBag.NotificationMsg = success
                    ? string.Format(Constant.SuccessMessage, EnumsHelper.GetEnumString(Enums.Operation.Save).StrToLower(), EnumsHelper.GetEnumString(Enums.MenuItem.Company).StrToLower())
                    : string.Format(Constant.ErrorMessage, EnumsHelper.GetEnumString(Enums.Operation.Save).StrToLower(), EnumsHelper.GetEnumString(Enums.MenuItem.Company).StrToLower());

                return base.PartialView("_Create", mCompany);
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = EnumsHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = ex.Message;
                return PartialView("_Create", mCompany);
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                //ViewBag.Companies = _dropdownService.GetCompanyOptionsAsync().GetAwaiter().GetResult();
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mCompany = new Domain.Company();
            try
            {
                mCompany = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = EnumsHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = ex.Message;
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                //ViewBag.Companies = _dropdownService.GetCompanyOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mCompany);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { Type = EnumsHelper.GetEnumString(Enums.Result.Success).StrToLower(), Message = string.Format(Constant.SuccessMessage, EnumsHelper.GetEnumString(Enums.Operation.Delete).StrToLower(), EnumsHelper.GetEnumString(Enums.MenuItem.Company).StrToLower()) };
            }
            catch (Exception)
            {
                resultData = new { Type = EnumsHelper.GetEnumString(Enums.Result.Success).StrToLower(), Message = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        public class SystemOwnerOnlyAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!StaticMethods.IsSystemOwner())
                    filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}