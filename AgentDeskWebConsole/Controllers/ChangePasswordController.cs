using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class ChangePasswordController : Controller
    {
        private readonly IChangePasswordService _service;

        public ChangePasswordController(IChangePasswordService changepasswordService)
        {
            _service = changepasswordService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Domain.ChangePassword());
        }

        [HttpPost]
        public ActionResult Index(Domain.ChangePassword mChangePassword)
        {
            try
            {
                mChangePassword.UserId = HttpContextHelper.LoginUser.Id;
                _service.Initiate(mChangePassword).GetAwaiter().GetResult();


                bool success = mChangePassword != null && mChangePassword.UserId > 0;

                ViewBag.NotificationType = success
                   ? EnumsHelper.GetEnumString(Enums.Result.Success).StrToLower() : EnumsHelper.GetEnumString(Enums.Result.Error).StrToLower();

                ViewBag.NotificationMsg = success
                    ? string.Format(Constant.SuccessMessage, EnumsHelper.GetEnumString(Enums.Operation.Save).StrToLower(), EnumsHelper.GetEnumString(Enums.MenuItem.ChangePassword).StrToLower())
                    : string.Format(Constant.ErrorMessage, EnumsHelper.GetEnumString(Enums.Operation.Save).StrToLower(), EnumsHelper.GetEnumString(Enums.MenuItem.ChangePassword).StrToLower());

            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Authentication");
            }
            catch (BadRequestException ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return View(mChangePassword);
        }

    }
}