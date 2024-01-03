using System.Web;
using System.Web.Optimization;

namespace STP.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
     

            #region OpenLayers Scripts
            bundles.Add(new ScriptBundle("~/bundles/OpenLayers").Include(
                "~/Scripts/Common/Openlayers/OpenLayers-{version}.js",
                "~/Scripts/Common/Openlayers/ifx.OpenLayers_fix-{version}.js"
                ));
            #endregion

            //script bundling started here
            #region jQueryPageList.list Scripts
            bundles.Add(new ScriptBundle("~/bundles/jQueryPageListlist").Include(
                    "~/Scripts/Common/jQueryPageList.list.js"
            ));
            #endregion

            #region IFX_Route Scripts
            bundles.Add(new ScriptBundle("~/bundles/IFXRouteFolder").Include(
                "~/Scripts/IFX_Route/ifx.jquery.contextMenu-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.waypoint-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.importroute-{version}.js",
                "~/Scripts/IFX_Route/ifx.RouteLibrary-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.routemanager-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.common-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.interface-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.main-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.contextmenu-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.structures-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.slidingpanel-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.roaddelegation-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.roadownership-{version}.js",
                "~/Scripts/IFX_Route/ifx.stpmap.forelayers-{version}.js",
                "~/Scripts/IFX_Route/ScaleBar-{version}.js",
                "~/Scripts/IFX_Route/OpenLayers.Control.ZoomStatus.js",
                "~/Scripts/IFX_Route/ifx.stpmap.lrsutil-{version}.js",
                "~/Scripts/Common/Openlayers/ifx.OpenLayers_fix-{version}.js",
                "~/Scripts/Common/Layout/ifx.AutocompleteControl.js",
                "~/Scripts/IFX_Route/ifx.stpmap.config-{version}.js"
                ));
            #endregion

            #region Util scripts
            bundles.Add(new ScriptBundle("~/bundles/Util").Include(
                      "~/Scripts/Common/Util/util.js",
                      "~/Scripts/Common/Util/aes.js",
                      "~/Scripts/Common/Util/pako-2.1.0.min.js"
                      ));
            #endregion
        }
    }
}
