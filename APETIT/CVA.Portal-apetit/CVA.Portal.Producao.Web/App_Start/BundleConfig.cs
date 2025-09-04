using System.Web;
using System.Web.Optimization;

namespace CVA.Portal.Producao.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include("~/Scripts/jquery-{version}.min.js"));
            bundles.Add(new ScriptBundle("~/Script/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            #region gentelella

            bundles.Add(new ScriptBundle("~/gentelella/js").Include(
                "~/bower_components/gentelella/vendors/fastclick/lib/fastclick.js"
                , "~/bower_components/gentelella/vendors/nprogress/nprogress.js"
                , "~/bower_components/gentelella/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"
                , "~/bower_components/gentelella/build/js/custom.min.js"));

            bundles.Add(new StyleBundle("~/gentelella/css").Include(
                "~/bower_components/gentelella/build/cs/custom.css"
                , "~/bower_components/gentelella/vendors/nprogress/nprogress.css"
                , "~/bower_components/gentelella/vendors/iCheck/skings/square/green.css"));

            #endregion

            bundles.Add(new ScriptBundle("~/Scripts/Views").Include("~/Scripts/Views/Producao/index.js"));
        }
    }
}
