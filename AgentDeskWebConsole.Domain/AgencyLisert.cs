using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class AgencyLister
    {
        public List<Agency> List { get; set; } = new List<Agency>();

        public Agency SearchCriteria { get; set; } = new Agency();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
