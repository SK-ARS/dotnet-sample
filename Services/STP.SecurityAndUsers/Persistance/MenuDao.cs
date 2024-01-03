using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Persistance
{
    internal static class MenuDAO
    {
        #region All Db Call should be called here from SP
        internal static List<MenuPrivileage> GetMenuInfo(int userId)
        {
            var menuList = new List<MenuPrivileage>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                menuList,
                 UserSchema.Portal + ".STP_LOGIN_PKG.GetUser_Previleges",
                parameter =>
                {
                    parameter.AddWithValue("p_USERID", userId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_resultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {

                    instance.MenuId = Convert.ToString(records.GetLongOrDefault("PRIVILEGE_ID"));
                    instance.MenuName = Convert.ToString(records.GetStringOrDefault("PRIVILEGE_NAME"));
                }
                );

            return menuList;
        }
        #endregion
    }
}