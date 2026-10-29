using AgentDeskWebConsole.Domain;
using System;

namespace AgentDeskWebConsole.Infrastructure.Helper
{
    public static class DefaultSetter
    {
        public static T ApplyDefaults<T>(T entity) where T : ICompanyScoped
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CompanyId = HttpContextHelper.LoginUser.CompanyId;
            return entity;
        }
    }
}
