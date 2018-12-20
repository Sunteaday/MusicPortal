using System.Web;
using System.Web.Optimization;

namespace MusicPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalunobtrusive").Include(
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new StyleBundle("~/Content/_layout").Include(
                      "~/Content/_Layout.css"));

            bundles.Add(new StyleBundle("~/Content/GenresPartial").Include(
                      "~/Content/GenresPartial.css"));
            
        }
    }
}
