namespace STP.Domain.Communications
{
    public class AutoResponseMailParams
    {
        public string EsdalReference { get; set; }
        public string HaulierEmailAddress { get; set; }
        public string OrganisationName { get; set; }
        public MailResponse ResponseMailDetails { get; set; }
    }
}