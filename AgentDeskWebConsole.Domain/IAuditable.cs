using System;
using System.Linq;

namespace AgentDeskWebConsole.Domain
{
    public interface IAuditable
    {
        int CreatedBy { get; set; }
        int? LastModifiedBy { get; set; }
    }
}
