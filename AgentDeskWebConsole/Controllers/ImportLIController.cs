using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class ImportLIController : Controller
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly IPolicyPremiumService _policyPremiumService;
        private readonly IPolicyService _policyService;


        public ImportLIController(IFileReaderService fileReaderService, IPolicyPremiumService policyPremiumService, IPolicyService policyService)
        {
            _fileReaderService = fileReaderService;
            _policyPremiumService = policyPremiumService;
            _policyService = policyService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase excelFile)
        {
            string filePath = null;
            try
            {
                var records = new List<Domain.ImportLI>();

                if (excelFile != null && excelFile.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Uploads/Fup");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    filePath = Path.Combine(folderPath,
                        Guid.NewGuid().ToString("N") + Path.GetExtension(excelFile.FileName));
                    excelFile.SaveAs(filePath);

                    records = _fileReaderService.ReadExcel(filePath);
                    Session["ExcelRecords"] = records;
                }

                return new JsonResult
                {
                    Data = records,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
            finally
            {
                if (filePath != null && System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
        }


        [HttpPost]
        public ActionResult ImportPolicy(Domain.Policy mPolicy)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicy = StaticMethods.SetAuditOnUpsert(mPolicy);
                mPolicy = StaticMethods.ApplyDefaults(mPolicy);

                mPolicy = _policyService.ImportPolicy(mPolicy).GetAwaiter().GetResult();
                if (mPolicy != null && mPolicy.Id > 0)
                    resultData = new { type = "success" };
                else
                    resultData = new { type = "error" };
            }
            catch (BadRequestException ex)
            {
                resultData = new { type = "error", message = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { type = "error", message = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}