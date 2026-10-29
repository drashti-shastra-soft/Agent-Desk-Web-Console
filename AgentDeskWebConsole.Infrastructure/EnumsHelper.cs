using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AgentDeskWebConsole.Infrastructure
{
    public static class EnumsHelper
    {
        public static List<string> GetEnumDisplayNames(Enum enm)
        {
            var type = enm.GetType();
            var displayNames = new List<string>();
            var names = Enum.GetNames(type);

            foreach (var name in names)
            {
                var field = type.GetField(name);
                var objArray = field.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (objArray.Length == 0)
                    displayNames.Add(name);

                foreach (DisplayAttribute fd in objArray)
                    displayNames.Add(fd.Name);
            }
            return displayNames;
        }
        public static string GetEnumString(Enums.Operation operation)
        {

            switch (operation)
            {
                case Enums.Operation.Save:
                    return nameof(Enums.Operation.Save);
                case Enums.Operation.Delete:
                    return nameof(Enums.Operation.Delete);
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        public static string GetEnumString(Enums.MenuItem menuItem)
        {

            switch (menuItem)
            {
                case Enums.MenuItem.Company:
                    return nameof(Enums.MenuItem.Company);
                case Enums.MenuItem.User:
                    return nameof(Enums.MenuItem.User);
                case Enums.MenuItem.ChangePassword:
                    return nameof(Enums.MenuItem.ChangePassword);
                default:
                    throw new ArgumentOutOfRangeException(nameof(menuItem), menuItem, null);
            }
        }

        public static string GetEnumString(Enums.Result result)
        {
            switch (result)
            {
                case Enums.Result.Success:
                    return nameof(Enums.Result.Success);
                case Enums.Result.Error:
                    return nameof(Enums.Result.Error);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        public static int GetEnumIndex(Enums.Role role)
        {
            switch (role)
            {
                case Enums.Role.System_Owner:
                    return (int)Enums.Role.System_Owner;
                case Enums.Role.Super_Admin:
                    return (int)Enums.Role.Super_Admin;
                case Enums.Role.Admin:
                    return (int)Enums.Role.Admin;
                case Enums.Role.Employee:
                    return (int)Enums.Role.Employee;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public static int GetEnum(Enums.AgencyType agencyType)
        {
            switch (agencyType)
            {
                case Enums.AgencyType.Life_Insurers:
                    return (int)Enums.AgencyType.Life_Insurers;
                case Enums.AgencyType.Standalone_Health_Insurers:
                    return (int)Enums.AgencyType.Standalone_Health_Insurers;
                case Enums.AgencyType.Specialised_Insurers:
                    return (int)Enums.AgencyType.Specialised_Insurers;
                case Enums.AgencyType.General_Insurers:
                    return (int)Enums.AgencyType.General_Insurers;
                default:
                    throw new ArgumentOutOfRangeException(nameof(agencyType), agencyType, null);
            }
        }

        public static string GetDisplayValue(this Enum en)
        {
            string displayName = string.Empty;
            displayName = en.GetType()
                .GetMember(en.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (displayName.IsNullOrEmpty())
            {
                displayName = en.ToString();
            }
            return displayName;
        }

    }
}
