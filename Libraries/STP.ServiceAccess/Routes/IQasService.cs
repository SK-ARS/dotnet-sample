using STP.Domain.Routes.QAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.Routes
{
    public interface IQasService
    {
        AddrDetails GetAddress(string moniker);
        List<AddrDetails> Search(string searchKeyword);
    }
}
