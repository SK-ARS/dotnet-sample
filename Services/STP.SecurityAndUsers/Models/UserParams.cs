using STP.SecurityAndUsers.Models;

namespace SecurityAndUsers.Models
{
    public class UserParams
    {
        public UserRegistration RegDet { get; set; }
        public int UserTypeId { get; set; }
        public int ? UserId { get; set; }
        public int ? ContactId { get; set; }
    }

   
}