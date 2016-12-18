
using System.Web;
using System.Web.Optimization;

namespace pasteBin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-ng-grid.js",
                        "~/Scripts/angular-resource.js",
                        "~/Scripts/angular-route.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/main.js",

                        "~/Scripts/textAngular/textAngular.js",
                        "~/Scripts/textAngular/textAngular-sanitize.js",
                        "~/Scripts/textAngular/textAngularSetup.js",
                        "~/Scripts/textAngular/textAngular-rangy.min.js",
                        "~/Scripts/textAngular/rangy-core.js",
                        "~/Scripts/textAngular/rangy-selectionsaverestore.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css"
                ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/ng-grid.css",
                "~/Content/textAngular.css",
                "~/Content/font-awesome.css"
                ));

        }
    }
}