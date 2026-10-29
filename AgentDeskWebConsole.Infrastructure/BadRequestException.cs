using System;
using System.Linq;

namespace AgentDeskWebConsole.Infrastructure
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message) { }
    }
}
