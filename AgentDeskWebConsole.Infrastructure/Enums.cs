using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Infrastructure
{
    public static class Enums
    {
        public enum Operation
        {
            Save = 1,
            Delete = 2,
        }

        public enum MenuItem
        {
            Company = 1,
            User = 2,
            ChangePassword = 3
        }

        public enum Result
        {
            Success = 1,
            Error = 2
        }

        public enum Role
        {
            System_Owner = 1,
            Super_Admin = 2,
            Admin = 3,
            Employee = 4
        }

        public enum AgencyType 
        {
            Life_Insurers = 1,
            Standalone_Health_Insurers = 2,
            Specialised_Insurers = 3,
            General_Insurers = 4
        }


    }
}
