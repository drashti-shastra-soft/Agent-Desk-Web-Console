using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class Report
    {
        public DateTime FUPDate { get; set; }
        public bool NeedToPremiumCollect { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string Area { get; set; }
        public string Client { get; set; }
    }
}
