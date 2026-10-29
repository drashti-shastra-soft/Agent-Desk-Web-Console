using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class ClientIdentification
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string PANNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string NomineeContact { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
