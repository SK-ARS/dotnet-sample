namespace STP.Common.Enums
{
    public enum AgentApplicationStatus
    {
        Received = 1,
        Delete = 999,
        SD_Approved = 1010,
        SD_Location = 1020,
        SD_Pending = 1030,
        SD_Declined = 1040,
        CD_Received = 2010,
        CD_InProcress_Pending_Credit_Report = 2020,
        CD_InProcress_Pending_OFAC = 2030,
        CD_InProcress_Pending_CrossReference = 2040,
        CD_InProcress_Pending_NY_Crimminal_Background_Check = 2050,
        CD_InProcress_Pending_PropertyRealEstateSearch = 2060,
        CD_OnHold_Manager_Approval = 2070,
        CD_OnHold_CoSigner = 2080,
        CD_Declined = 2090,
        NA_Approved = 3010,
        NA_Pending = 3020,
        NA_Rejected = 3030
    }
}