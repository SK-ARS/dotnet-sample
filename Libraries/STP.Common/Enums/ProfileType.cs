#region

using System.ComponentModel;

#endregion

namespace STP.Common.Enums
{
    public enum ProfileType
    {
        [Description("Country")] Country = 0,

        [Description("Company")] Company = 1,

        [Description("Department")] Department = 2,

        [Description("Company for Search Application")] CompanyForSearch = 3,

        [Description("Department for Search Application")] DepartmentForSearch = 4,
    }
}