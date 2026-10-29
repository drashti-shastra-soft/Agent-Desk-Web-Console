namespace AgentDeskWebConsole.Domain
{
    public class Group : IAuditable, ICompanyScoped
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string HeadName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
