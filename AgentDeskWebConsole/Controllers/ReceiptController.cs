using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Helper;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class ReceiptController : Controller
    {
        private readonly IReceiptService _service;
        private readonly IDropdownService _dropdownService;
        private readonly IFileReaderService _fileReaderService;
        private readonly IValidationService _validationService;

        public ReceiptController(IReceiptService service, IDropdownService dropdownService, IFileReaderService fileReaderService, IValidationService validationService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _fileReaderService = fileReaderService;
            _validationService = validationService;
        }

        public ActionResult Index()
        {
            return View(new Domain.ReceiptLister());
        }

        public ActionResult List(Domain.ReceiptLister mLister)
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
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new Domain.Receipt());
        }

        [HttpPost]
        public ActionResult Create(Domain.Receipt mReceipt, HttpPostedFileBase FilePath)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);

                if (FilePath != null && FilePath.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        FilePath.InputStream.CopyTo(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        mReceipt.Base64String = Convert.ToBase64String(fileBytes);
                        mReceipt.MIME = FilePath.ContentType;
                        mReceipt.FilePath = FilePath.FileName;
                    }
                }

                mReceipt = _service.CreateAsync(mReceipt).GetAwaiter().GetResult();

                if (mReceipt != null && mReceipt.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Receipt saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving receipt!";
                }
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return PartialView("_Create", mReceipt);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mReceipt = new Domain.Receipt();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mReceipt = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return PartialView("_Create", mReceipt);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Receipt has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting receipt!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPdf(int id)
        {
            var mReceipt = new Domain.Receipt();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mReceipt = _service.GetPdfAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return Json(mReceipt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPolicyDetails(string policyNumber)
        {
            System.Threading.Thread.Sleep(1000);
            var policy = _service.GetPolicyByNumberAsync(policyNumber).GetAwaiter().GetResult();

            if (policy != null && policy.Id > 0)
            {
                return Json(new
                {
                    success = true,
                    policyId = policy.Id,
                    clientId = policy.ClientId,
                    name = policy.ClientName,
                    premiumAmount = policy.PremiumAmount
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        #region Import Recipt

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Import(HttpPostedFileBase file)
        {
            var mReceipts = new List<Domain.Receipt>();
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Path.Combine(Server.MapPath("~/Uploads/Receipts"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(Server.MapPath("~/Uploads/Receipts"), $"{StaticMethods.GenerateGUID(15)}{Path.GetExtension(file.FileName)}");
                file.SaveAs(filePath);
                mReceipts = _fileReaderService.ReadReceipts(filePath);
                mReceipts = _validationService.Initiate(mReceipts);
            }
            return Json(mReceipts, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}