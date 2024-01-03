using STP.Domain;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Structures.Persistance.StructureManager;
using STP.Domain.Structures;
using STP.Common.Constants;

namespace STP.Structures.Interface
{
    public interface IStructureManager
    {
        List<StructureInfo> MyStructureInfoList(int organisationId, int otherOrganisation, int left, int right, int bottom, int top);
        List<StructureContact> GetStructureContactListInfo(long structureId, string userSchema = UserSchema.Portal);
        List<RoadContactModal> GetRoadContactList(long linkId, long length, string userSchema = UserSchema.Portal);
    }
}
