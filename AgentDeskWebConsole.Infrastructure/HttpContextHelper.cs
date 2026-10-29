using System;
using System.Linq;
using System.Web;

namespace AgentDeskWebConsole.Infrastructure
{
    public class HttpContextHelper
    {
        private const string LoginUserKey = "LoginUser";

        public static Domain.User LoginUser
        {
            get => HttpContext.Current?.Session?[LoginUserKey] as Domain.User;
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[LoginUserKey] = value;
            }
        }
    }
}
