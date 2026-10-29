using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class PolicyNominee
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string Policy { get; set; }
        public int Age { get; set; }
        public string Relation { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal NomineePercentage { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
