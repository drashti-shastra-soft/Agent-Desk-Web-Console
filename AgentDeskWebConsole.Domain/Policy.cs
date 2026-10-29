using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class Policy : IAuditable, ICompanyScoped
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PolicyTypeId { get; set; }
        public int CompanyId { get; set; }
        public string PolicyNumber { get; set; }
        public string Address  { get; set; }
        public decimal? SumAssured { get; set; }
        public string IDV { get; set; }
        public DateTime? FUPDate { get; set; }
        public DateTime? CommercementDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PremiumAmount { get; set; }
        public string PayMode { get; set; }
        public int PolicyStatusId { get; set; }
        public string MIME { get; set; }
        public string FilePath { get; set; }
        public string Client { get; set; }
        public string PolicyStatus { get; set; }
        public bool IsActive { get; set; } = true;
        public string ClientName { get; set; }
        public bool NeedToPremiumCollect { get; set; }
        public string PolicyType { get; set; }
        public string Name { get; set; }
        public string Base64String { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? DOB { get; set; }
        public int AddressId { get; set; }
        public string Group { get; set; }
        public string Plan { get; set; }
        public int Term { get; set; }
        public int PPT { get; set; }
        public string PlanTermPpt { get; set; }
        public string NomineeName { get; set; }
        public int NomineeAge { get; set; }
        public string NomineeRelation { get; set; }
    }
}
