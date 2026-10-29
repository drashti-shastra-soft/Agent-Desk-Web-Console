using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class ClientFinancialInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string AnnualIncome { get; set; }
        public string ExistingLoansEMI { get; set; }
        public string TaxSavingInvestments { get; set; }
        public string BankName { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string Branch { get; set; }
        public string AccountType { get; set; }
        public int? LastModifiedBy { get; set; }
        public int CreatedBy { get; set; }

    }
}
