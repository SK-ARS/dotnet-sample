namespace STP.Domain.Routes
{
    public class LibraryRouteListParams
    {
        public int presetFilter;
        public int? sortOrder;

        public int OrganisationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int RouteType { get; set; }
        public string SerchString { get; set; }
        public string UserSchema { get; set; }
        public int FilterFavouritesRoutes { get; set; }
    }
}