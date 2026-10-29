using System.Collections.Generic;

namespace AgentDeskWebConsole.Domain
{
    public class GroupLister
    {
        public List<Group> List { get; set; } = new List<Group>();

        public Group SearchCriteria { get; set; } = new Group();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
