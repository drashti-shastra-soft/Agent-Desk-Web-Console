using AgentDeskWebConsole.Filter;
using AgentDeskWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgentDeskWebConsole.Controllers
{
    [Authentication]
    public class PremiumReportController : Controller
    {
        private readonly IReportService _reportService;

        public PremiumReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public ActionResult Index()
        {
            var mReport = _reportService.GetAsync().GetAwaiter().GetResult();
            return View(mReport);
        }
    }
}