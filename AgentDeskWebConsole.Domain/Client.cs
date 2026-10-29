using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class Client : IAuditable, ICompanyScoped
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string ClientName { get; set; }
        public string Occupation { get; set; }
        public int? AddressId { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
    }
}
