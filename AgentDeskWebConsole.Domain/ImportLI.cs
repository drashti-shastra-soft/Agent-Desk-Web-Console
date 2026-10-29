using System;

namespace AgentDeskWebConsole.Domain
{
    public class ImportLI
    {
        public int SNo { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public string PolicyNo { get; set; }
        public string AgCode { get; set; }
        public DateTime? ComDate { get; set; }
        public string PlanTermPpt { get; set; }
        public decimal? SumAssured { get; set; }
        public string Mode { get; set; }
        public DateTime? FupDate { get; set; }
        public decimal? Premium { get; set; }
        public string Nominee { get; set; }
        public string NomineeAge { get; set; }
        public string Relation { get; set; }
    }
}
