using STP.Domain.NonESDAL;

namespace STP.Applications.Interface
{
    interface INEApplication
    {
        long SaveNEApplication(NEAppGeneralDetails generalDetails);
        string GetNEApplicationStatus(string ESDALReferenceNumber);
    }
}
