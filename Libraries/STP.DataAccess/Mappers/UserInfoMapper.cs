using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;

namespace STP.DataAccess.Mappers
{
    public class UserInfoMapper
    {
        public UserInfo MapUserInfo(IRecord records)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = Convert.ToString(records.GetDecimalOrDefault("USER_ID"));
            userInfo.UserName = records.GetStringOrDefault("NAME");
            //userInfo.FirstName = records.GetStringOrDefault("NAME");
            //userInfo.IsValidUser = (!int.IsNullOrEmpty(userInfo.UserID));
            userInfo.userTypeId = records.GetInt32OrDefault("PORTAL_TYPE");
            userInfo.IsAdmin = records.GetInt32OrDefault("IS_ADMINISTRATOR");
            userInfo.ProjectId = records.GetInt32OrDefault("PROJECT_ID");
            userInfo.VehicleUnits = records.GetInt32OrDefault("VEHICLE_UNITS");
            userInfo.RoutePlanUnits = records.GetInt32OrDefault("ROUTEPLAN_UNITS_1");
            return userInfo;
        }
    }
}
