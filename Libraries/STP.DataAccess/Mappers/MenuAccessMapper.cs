using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;
using STP.Domain.SecurityAndUsers;

namespace STP.DataAccess.Mappers
{
    public class MenuAccessMapper
    {
        public MenuPrivileage MapMenuAccessInfo(IRecord records)
        {
            MenuPrivileage menuPrivileage = new MenuPrivileage();
            List<MenuPrivileage> menuPrivileageList = new List<MenuPrivileage>();
            menuPrivileage.MenuId = Convert.ToString(records.GetInt32OrDefault("MENU_ID"));
            menuPrivileageList.Add(menuPrivileage);
            return menuPrivileage;
        }
    }
}
