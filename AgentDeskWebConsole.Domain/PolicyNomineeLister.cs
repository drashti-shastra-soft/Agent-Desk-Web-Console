using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class PolicyNomineeLister
    {
        public List<PolicyNominee> List { get; set; } = new List<PolicyNominee>();

        public PolicyNominee SearchCriteria { get; set; } = new PolicyNominee();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
