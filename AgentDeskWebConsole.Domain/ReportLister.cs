using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class ReportLister
    {
        public List<Report> List { get; set; } = new List<Report>();

        public Report SearchCriteria { get; set; } = new Report();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
