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
    public class ClientController : Controller
    {
        #region Declaration

        private readonly IClientService _service;
        private readonly IDropdownService _dropdownService;
        private readonly IAddressService _addressService;
        private readonly IClientIdentificationService _clientIdentificationService;
        private readonly IClientHealthService _clientHealthService;
        private readonly IClientFinancialInfoService _clientFinancialInfoService;
        private readonly IClientDocumentService _clientDocument;

        public ClientController(IClientService service, IDropdownService dropdownService, IAddressService addressService, IClientIdentificationService clientIdentificationService, IClientHealthService clientHealthService, IClientFinancialInfoService clientFinancialInfoService, IClientDocumentService clientDocument)
        {
            _service = service;
            _dropdownService = dropdownService;
            _addressService = addressService;
            _clientIdentificationService = clientIdentificationService;
            _clientHealthService = clientHealthService;
            _clientFinancialInfoService = clientFinancialInfoService;
            _clientDocument = clientDocument;
        }

        #endregion

        #region Client

        public ActionResult Index()
        {
            ViewBag.ShowCompany = StaticMethods.IsSystemOwner();
            return View(new Domain.ClientLister());
        }

        public ActionResult List(Domain.ClientLister mLister)
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
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Groups = _dropdownService.GetGroupOptionsAsync().GetAwaiter().GetResult();
            ViewBag.Gender = new SelectList(_dropdownService.GetGenderOptions(), "Value", "Text");
            ViewBag.MaritalStatus = new SelectList(_dropdownService.GetMaritalStatusOptions(), "Value", "Text");

            return PartialView("_Create", new Domain.Client());
        }

        [HttpPost]
        public ActionResult Create(Domain.Client mClient)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = StaticMethods.SetAuditOnUpsert(mClient);
                mClient = StaticMethods.ApplyDefaults(mClient);

                mClient = _service.CreateAsync(mClient).GetAwaiter().GetResult();

                if (mClient != null && mClient.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Client has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving client!";
                }
            }

            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Groups = _dropdownService.GetGroupOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Gender = new SelectList(_dropdownService.GetGenderOptions(), "Value", "Text");
                ViewBag.MaritalStatus = new SelectList(_dropdownService.GetMaritalStatusOptions(), "Value", "Text");
            }
            return PartialView("_Create", mClient);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mClient = new Domain.Client();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = _service.GetAsync(id).GetAwaiter().GetResult();

                if (mClient == null || !StaticMethods.CanAccess(mClient.CompanyId))
                    return HttpNotFound();

            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Groups = _dropdownService.GetGroupOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Gender = new SelectList(_dropdownService.GetGenderOptions(), "Value", "Text");
                ViewBag.MaritalStatus = new SelectList(_dropdownService.GetMaritalStatusOptions(), "Value", "Text");
            }
            return PartialView("_Create", mClient);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Client has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting client!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Address
        [HttpGet]
        public ActionResult GetAddress(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                mAddress = _addressService.GetAddressAsync(id).GetAwaiter().GetResult();
                mAddress = mAddress ?? new Domain.Address();
                mAddress.ClientId = id;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }

        [HttpPost]
        public ActionResult SaveAddress(Domain.Address mAddress)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAddress = StaticMethods.SetAuditOnUpsert(mAddress);
                mAddress = StaticMethods.ApplyDefaults(mAddress);

                mAddress = _addressService.CreateAsync(mAddress).GetAwaiter().GetResult();

                var client = _service.GetAsync(mAddress.ClientId ?? 0).GetAwaiter().GetResult();


                if (mAddress != null && mAddress.Id > 0)
                {
                    if (client != null)
                    {
                        client.AddressId = mAddress.Id;
                        client = StaticMethods.SetAuditOnUpsert(client);
                        _service.CreateAsync(client).GetAwaiter().GetResult();
                    }

                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Address has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Address!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }

        #endregion

        #region Client Details
        [HttpGet]
        public JsonResult GetClientMetadata(int id)
        {
            string redirectUrl = Url.Action("Detail", "Client", new { id = id });
            return Json(new { success = true, redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            ViewBag.ClientId = id;
            return View();
        }

        [HttpGet]
        public ActionResult GetClient(int id)
        {
            var mClient = new Domain.Client();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Groups = _dropdownService.GetGroupOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Gender = new SelectList(_dropdownService.GetGenderOptions(), "Value", "Text");
                ViewBag.MaritalStatus = new SelectList(_dropdownService.GetMaritalStatusOptions(), "Value", "Text");
            }
            return PartialView("_Client", mClient);
        }

        [HttpPost, Route("SaveClient")]
        public ActionResult SaveClient(Domain.Client mClient)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = _service.CreateAsync(mClient).GetAwaiter().GetResult();
                if (mClient != null && mClient.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Client has been saved Successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Client!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.Groups = _dropdownService.GetGroupOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Gender = new SelectList(_dropdownService.GetGenderOptions(), "Value", "Text");
                ViewBag.MaritalStatus = _dropdownService.GetMaritalStatusOptions();
            }
            return PartialView("_Client", mClient);
        }
        #endregion

        #region Client Identification

        [HttpGet]
        public ActionResult GetClientIdentification(int id)
        {
            var mClientIdentification = new Domain.ClientIdentification();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientIdentification = _clientIdentificationService.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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

            return PartialView("_ClientIdentification", mClientIdentification);
        }

        [HttpPost, Route("SaveClientIdentification")]
        public ActionResult SaveClientIdentification(Domain.ClientIdentification mClientIdentification)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientIdentification = _clientIdentificationService.CreateAsync(mClientIdentification).GetAwaiter().GetResult();
                if (mClientIdentification != null && mClientIdentification.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Client Identification has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Client Identification!";
                }
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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
            return PartialView("_ClientIdentification", mClientIdentification);
        }
        #endregion

        #region Client Financial

        [HttpGet]
        public ActionResult GetClientFinancial(int id)
        {
            var mClientFinancialInfo = new Domain.ClientFinancialInfo();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientFinancialInfo = _clientFinancialInfoService.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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
            finally
            {
                ViewBag.AccountType = new SelectList(_dropdownService.GetAccountTypeOptions(), "Value", "Text");
            }

            return PartialView("_ClientFinancial", mClientFinancialInfo);
        }

        [HttpPost, Route("SaveClientFinancial")]
        public ActionResult SaveClientFinancial(Domain.ClientFinancialInfo mClientFinancialInfo)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientFinancialInfo = _clientFinancialInfoService.CreateAsync(mClientFinancialInfo).GetAwaiter().GetResult();
                if (mClientFinancialInfo != null && mClientFinancialInfo.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Client Financial Infomation has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving client Financial Infomation!";
                }
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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
            finally
            {
                ViewBag.AccountType = new SelectList(_dropdownService.GetAccountTypeOptions(), "Value", "Text");
            }
            return PartialView("_ClientFinancial", mClientFinancialInfo);
        }

        #endregion

        #region Client Health

        [HttpGet]
        public ActionResult GetClientHealth(int id)
        {
            var mClientHealth = new Domain.ClientHealth();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientHealth = _clientHealthService.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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

            return PartialView("_ClientHealth", mClientHealth);
        }

        [HttpPost, Route("SaveClientHealth")]
        public ActionResult SaveClientHealth(Domain.ClientHealth mClientHealth)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClientHealth = _clientHealthService.CreateAsync(mClientHealth).GetAwaiter().GetResult();
                if (mClientHealth != null && mClientHealth.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Client Health has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Client Health!";
                }
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_LoginPrompt");
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
            return PartialView("_ClientHealth", mClientHealth);
        }

        #endregion

        #region Client Document

        [HttpGet, Route("GetClientDocument")]
        public ActionResult GetClientDocument(int id)
        {
            var mClientDocument = new List<Domain.ClientDocument>();
            try
            {
                mClientDocument = _clientDocument.Fetch(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internalservererror!";
            }

            return PartialView("_ClientDocument", mClientDocument);
        }

        [HttpPost, Route("SaveClientDocument")]
        public ActionResult SaveClientDocument(int clientId, IEnumerable<HttpPostedFileBase> files)
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

                            var mClientDocument = new Domain.ClientDocument()
                            {
                                ClientId = clientId,
                                FilePath = file.FileName,
                                MIME = file.ContentType,
                                Base64String = base64String
                            };

                            mClientDocument = _clientDocument.CreateAsync(mClientDocument).GetAwaiter().GetResult();
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

        [HttpGet, Route("FetchClientDocument")]
        public ActionResult FetchClientDocument(int clientId)
        {
            dynamic jsonResult = new ExpandoObject();
            try
            {
                var mClientDocuments = _clientDocument.Fetch(clientId).GetAwaiter().GetResult();

                jsonResult = Json(mClientDocuments.Select(img => new
                {
                    Id = img.Id,
                    FilePath = img.FilePath,
                    MIME = img.MIME,
                    Base64String = img.Base64String
                }), JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
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

        [HttpGet, Route("DeleteClientDocument")]
        public ActionResult DeleteClientDocument(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _clientDocument.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Client Document has been deleted!" };
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