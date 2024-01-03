#region

using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IRouteConfigProvider
    {
        #region RoutesRoute

        RouteCollection GetRouteCollection();
        //int TotalRoutes();
        //MURoute GetRoute(string routeName);
        //bool IsRoutEnabled(string routeName);

        #endregion
    }
}