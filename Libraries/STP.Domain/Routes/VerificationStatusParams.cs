namespace STP.Domain.Routes
{
    public class VerificationStatusParams
    {
        public int routeId { get; set; }

        public int isLib { get; set; }

        public int replanStatus { get; set; }

        public string userSchema { get; set; }
    }
}