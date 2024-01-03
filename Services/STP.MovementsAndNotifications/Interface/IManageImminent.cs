namespace STP.MovementsAndNotifications.Interface
{
  public  interface IManageImminent
    {
        int ShowImminentMovement(string moveStartDate, string countryId, int countryIdCount, int vehicleClass);
        //int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, decimal GrossWeight, int WorkingDays, decimal FrontPRJ, decimal RearPRJ, decimal LeftPRJ, decimal RightPRJ, GetImminentChkDetailsDomain objImminent, string Notif_type = null);
        //GetImminentChkDetailsDomain GetDetailsToChkImminent(long notificationId, string contentReferenceNo, long revisionId, string userSchema);
    }
}
