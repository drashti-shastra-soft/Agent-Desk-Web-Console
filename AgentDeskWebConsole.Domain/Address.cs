using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class Address : ICompanyScoped , IAuditable
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? ClientId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
