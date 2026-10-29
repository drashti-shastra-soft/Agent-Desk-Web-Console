using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class PolicyPremium
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime AdjDate { get; set; }
        public string Premium { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
