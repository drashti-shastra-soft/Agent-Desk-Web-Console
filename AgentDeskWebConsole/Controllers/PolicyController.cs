using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Helper;
using AgentDeskWebConsole.Infrastructure;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class PolicyController : Controller
    {
        private readonly IPolicyService _service;
        private readonly IClientService _clientService;
        private readonly IDropdownService _dropdownService;
        private readonly IPolicyDocumentService _policyDocumentService;

        public PolicyController(IPolicyService service, IDropdownService dropdownService, IClientService clientService, IPolicyDocumentService policyDocumentService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _clientService = clientService;
            _policyDocumentService = policyDocumentService;
        }

        public ActionResult Index()
        {
            ViewBag.ShowCompany = StaticMethods.IsSystemOwner();
            return View(new Domain.PolicyLister());
        }

        public ActionResult List(Domain.PolicyLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.CompanyId = StaticMethods.ResolveCompanyId(mLister.SearchCriteria.CompanyId);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();

                var client = _clientService.GetAsync(mLister.SearchCriteria.ClientId).GetAwaiter().GetResult();
                ViewBag.ClientName = client.FirstName + " " + client.LastName;
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
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.PayMode = _dropdownService.GetPayModeOptions();
                ViewBag.PolicyTypes = _dropdownService.GetPolicyTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.PolicyStatus = _dropdownService.GetPolicyStatusOptionsAsync().GetAwaiter().GetResult();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create(int clientId)
        {
            ViewBag.PolicyTypes = _dropdownService.GetPolicyTypeOptionsAsync().GetAwaiter().GetResult();
            ViewBag.PolicyStatus = _dropdownService.GetPolicyStatusOptionsAsync().GetAwaiter().GetResult();
            ViewBag.PayMode = _dropdownService.GetPayModeOptions();
            return PartialView("_Create", new Domain.Policy
            {
                ClientId = clientId
            });
        }

        [HttpPost]
        public ActionResult Create(Domain.Policy mPolicy, HttpPostedFileBase FilePath)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicy = StaticMethods.SetAuditOnUpsert(mPolicy);
                mPolicy = StaticMethods.ApplyDefaults(mPolicy);

                if (FilePath != null && FilePath.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        FilePath.InputStream.CopyTo(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        mPolicy.Base64String = Convert.ToBase64String(fileBytes);
                        mPolicy.MIME = FilePath.ContentType;
                        mPolicy.FilePath = FilePath.FileName;
                    }
                }

                mPolicy = _service.CreateAsync(mPolicy).GetAwaiter().GetResult();

                if (mPolicy != null && mPolicy.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Policy has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving policy!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.PolicyTypes = _dropdownService.GetPolicyTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.PolicyStatus = _dropdownService.GetPolicyStatusOptionsAsync(mPolicy.PolicyTypeId.ToString(), mPolicy.PolicyStatusId.ToString()).GetAwaiter().GetResult();
                ViewBag.PayMode = new SelectList(_dropdownService.GetPayModeOptions(), "Value", "Text");

            }
            return PartialView("_Create", mPolicy);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mPolicy = new Domain.Policy();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicy = _service.GetAsync(id).GetAwaiter().GetResult();

                if (mPolicy == null || !StaticMethods.CanAccess(mPolicy.CompanyId))
                    return HttpNotFound();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.PolicyTypes = _dropdownService.GetPolicyTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.PolicyStatus = _dropdownService.GetPolicyStatusOptionsAsync(mPolicy.PolicyTypeId.ToString(), mPolicy.PolicyStatusId.ToString()).GetAwaiter().GetResult();
                ViewBag.PayMode = new SelectList(_dropdownService.GetPayModeOptions(), "Value", "Text");
            }

            return PartialView("_Create", mPolicy);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Policy has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting policy!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPolicyStatusBy(string policyTypeId)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                var mPolicyStatus = _dropdownService.GetPolicyStatusOptionsAsync(policyTypeId).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "policy status has been saved successfully!", PolicyStatus = mPolicyStatus };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting policy status!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPdf(int id)
        {
            var mPolicy = new Domain.Policy();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPolicy = _service.GetPdfAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return Json(mPolicy, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckPolicyNumberExist(Domain.Policy mPolicy)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                var isExist = _service.CheckPolicyNumberExist(mPolicy).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", IsExist = isExist };
            }
            catch (Exception)
            {
                resultData = new { StatusCode = "Error", IsExist = false };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }


        #region Policy Document
        [HttpGet, Route("GetPolicyDocument")]
        public ActionResult GetPolicyDocument(int id)
        {
            var mPolicyDocument = new List<Domain.PolicyDocument>();
            try
            {
                mPolicyDocument = _policyDocumentService.Fetch(id).GetAwaiter().GetResult();
                ViewBag.PolicyId = id;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internalservererror!";
            }

            return View("_PolicyDocument", mPolicyDocument);
        }

        [HttpPost, Route("SavePolicyDocument")]
        public ActionResult SavePolicyDocument(int policyId, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string base64String;

                            using (var memoryStream = new MemoryStream())
                            {
                                file.InputStream.CopyTo(memoryStream);
                                byte[] fileBytes = memoryStream.ToArray();
                                base64String = Convert.ToBase64String(fileBytes);
                            }

                            var mPolicyDocument = new Domain.PolicyDocument()
                            {
                                PolicyId = policyId,
                                FilePath = file.FileName,
                                MIME = file.ContentType,
                                Base64String = base64String
                            };

                            mPolicyDocument = _policyDocumentService.CreateAsync(mPolicyDocument).GetAwaiter().GetResult();
                        }
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Json(new { success = true });
        }


        [HttpGet, Route("FetchPolicyDocument")]
        public ActionResult FetchPolicyDocument(int policyId)
        {
            dynamic jsonResult = new ExpandoObject();
            try
            {
                var mPolicyDocuments = _policyDocumentService.Fetch(policyId).GetAwaiter().GetResult();

                jsonResult = Json(mPolicyDocuments.Select(img => new
                {
                    Id = img.Id,
                    FilePath = img.FilePath,
                    MIME = img.MIME,
                    Base64String = img.Base64String
                }), JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue; // ￼ This is the key line
            }

            catch (BadRequestException ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            catch (Exception)
            {
                jsonResult = new { NotificationType = "error", NotificationMsg = "Error occurred while get product images!" };
            }
            return jsonResult;
        }

        [HttpGet, Route("DeletePolicyDocument")]
        public ActionResult DeletePolicyDocument(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _policyDocumentService.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Policy Document has been deleted!" };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized" };
            }
            catch (BadRequestException ex)
            {
                resultData = new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting Client Document" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}