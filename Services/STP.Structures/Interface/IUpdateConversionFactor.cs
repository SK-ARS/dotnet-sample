using STP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;
namespace STP.Structures.Interface
{
    public interface IUpdateConversionFactor
    {
        /// <summary>
        /// Get HB Rating Values
        /// </summary>
        List<double?> GetHBRatings(long structureId, long sectionId);
        List<SvReserveFactors> GetCalculatedHBToSV(long structureId, long sectionId, double? hbWithLoad, double? hbWithoutLoad, int saveFlag, string userName);
    }
}
