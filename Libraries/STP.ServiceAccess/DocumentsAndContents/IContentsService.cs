using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
  public  interface IContentsService
    {
        int ManageFavourites(int categoryId, int categoryType, int isFavourite);
    }
}
