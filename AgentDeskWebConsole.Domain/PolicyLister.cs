using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class PolicyLister
    {
        public string ClientName { get; set; }

        public string Type { get; set; }

        public List<Policy> List { get; set; } = new List<Policy>();

        public Policy SearchCriteria { get; set; } = new Policy();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
