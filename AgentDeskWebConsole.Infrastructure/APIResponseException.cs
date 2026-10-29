using System;
using System.Linq;

namespace AgentDeskWebConsole.Infrastructure
{
    public class APIResponseException : Exception
    {
        public APIResponseException(string error) : base(error)
        {

        }
    }
}
