using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace STP.DataAccess
{
    public class MenuAccess
    {
        public List<Menus> MenuInfo { get; set; }
        public List<SubMenus> SubMenuInfo { get; set; }
        public List<MenuPrivileage> MenuAccessInfo { get; set; }        
    }

    public class Menus
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
    }

    public class SubMenus
    {
        public string MainMenuId { get; set; }
        public string SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string Navigation { get; set; }
    }

    public class MenuPrivileage
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
    }    

}
