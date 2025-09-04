using System.Web;
using System.Web.Optimization;

namespace CVAGestaoLayout
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts Plugins and Layout
            bundles.Add(new ScriptBundle("~/Script/jquery").Include("~/Scripts/plugins/jQuery/jQuery-2.1.4.js"));
            bundles.Add(new ScriptBundle("~/Script/angular").Include("~/Scripts/angular.js"));
            bundles.Add(new ScriptBundle("~/Script/jqueryui").Include("~/Scripts/plugins/jQueryUI/jqueryui.js"));
            bundles.Add(new ScriptBundle("~/Script/bootstrap").Include("~/Scripts/plugins/bootstrap/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/Script/icheck").Include("~/Scripts/plugins/iCheck/icheck.js"));
            bundles.Add(new ScriptBundle("~/Script/customscroll").Include("~/Scripts/plugins/mcustomscrollbar/jquery.mCustomScrollbar.js"));
            bundles.Add(new ScriptBundle("~/Script/settings").Include("~/Scripts/settings.js"));
            bundles.Add(new ScriptBundle("~/Script/plugins").Include("~/Scripts/plugins.js"));
            bundles.Add(new ScriptBundle("~/Script/action").Include("~/Scripts/actions.js"));
            bundles.Add(new ScriptBundle("~/Script/dashboard").Include("~/Scripts/demo_dashboard.js"));
            bundles.Add(new ScriptBundle("~/Script/d3").Include("~/Scripts/plugins/rickshaw/d3.v3.js"));
            bundles.Add(new ScriptBundle("~/Script/rick").Include("~/Scripts/plugins/rickshaw/rickshaw.js"));
            bundles.Add(new ScriptBundle("~/Script/vector").Include("~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.js"));
            bundles.Add(new ScriptBundle("~/Script/vectorworld").Include("~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));
            bundles.Add(new ScriptBundle("~/Script/datepicker").Include("~/Scripts/plugins/bootstrap/bootstrap-datepicker.js"));
            bundles.Add(new ScriptBundle("~/Script/owl").Include("~/Scripts/plugins/owl/owl.carousel.js"));
            bundles.Add(new ScriptBundle("~/Script/moment").Include("~/Scripts/plugins/moment.js"));
            bundles.Add(new ScriptBundle("~/Script/moment").Include("~/Scripts/plugins/moment.js"));
            bundles.Add(new ScriptBundle("~/Script/dateRange").Include("~/Scripts/plugins/daterangepicker/daterangepicker.js"));
            bundles.Add(new ScriptBundle("~/Script/datatable").Include("~/Scripts/plugins/datatables/jquery.dataTables.js"));
            bundles.Add(new ScriptBundle("~/Script/calendar").Include("~/Scripts/plugins/fullcalendar/fullcalendar.js"));
            bundles.Add(new ScriptBundle("~/Script/calendarLang").Include("~/Scripts/plugins/fullcalendar/lang-all.js"));
            bundles.Add(new ScriptBundle("~/Script/mirror")
                .Include("~/Scripts/plugins/codemirror/codemirror.js",
                "~/Scripts/plugins/codemirror/mode/htmlmixed/htmlmixed.js",
                "~/Scripts/plugins/codemirror/mode/xml/xml.js",
                "~/Scripts/plugins/codemirror/mode/javascript/javascript.js",
                "~/Scripts/plugins/codemirror/mode/css/css.js",
                "~/Scripts/plugins/codemirror/mode/clike/clike.js",
                "~/Scripts/plugins/summernote/summernote.js"));
            bundles.Add(new ScriptBundle("~/Script/dropzone").Include("~/Scripts/plugins/dropzone/dropzone.js"));
            bundles.Add(new ScriptBundle("~/Script/masked").Include("~/Scripts/plugins/maskedinput/jquery.maskedinput.js"));
            bundles.Add(new ScriptBundle("~/Script/SignalR").Include("~/Scripts/jquery.signalR-2.2.0.js"));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/Content/themedefault").Include("~/Content/theme-default.css"));
            bundles.Add(new StyleBundle("~/Content/calendar").Include("~/Scripts/plugins/fullcalendar/fullcalendar.css"));
            bundles.Add(new StyleBundle("~/Content/Loading").Include("~/Content/Loading.css"));
            #endregion

            #region Business Scripts
            bundles.Add(new ScriptBundle("~/Business/Start").Include("~/Scripts/ScriptPage/StartModule.js"));
            bundles.Add(new ScriptBundle("~/Business/CalendarController").Include("~/Scripts/ScriptPage/CalendarController.js"));
            bundles.Add(new ScriptBundle("~/Business/Pointing").Include("~/Scripts/ScriptPage/PointingHoursScript.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectController").Include("~/Scripts/ScriptPage/ProjectController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectMainController").Include("~/Scripts/ScriptPage/ProjectMainController.js"));
            bundles.Add(new ScriptBundle("~/Business/SpecialtyController").Include("~/Scripts/ScriptPage/SpecialtyController.js"));
            bundles.Add(new ScriptBundle("~/Business/CollaboratorController").Include("~/Scripts/ScriptPage/CollaboratorController.js"));
            bundles.Add(new ScriptBundle("~/Business/CollaboratorMainController").Include("~/Scripts/ScriptPage/CollaboratorMainController.js"));
            bundles.Add(new ScriptBundle("~/Business/BranchController").Include("~/Scripts/ScriptPage/BranchController.js"));
            bundles.Add(new ScriptBundle("~/Business/ClientController").Include("~/Scripts/ScriptPage/ClientController.js"));
            bundles.Add(new ScriptBundle("~/Business/ClientMainController").Include("~/Scripts/ScriptPage/ClientMainController.js"));
            bundles.Add(new ScriptBundle("~/Business/SalesChannelController").Include("~/Scripts/ScriptPage/SalesChannelController.js"));
            bundles.Add(new ScriptBundle("~/Business/OportunittyController").Include("~/Scripts/ScriptPage/OportunittyController.js"));
            bundles.Add(new ScriptBundle("~/Business/OportunittyMainController").Include("~/Scripts/ScriptPage/OportunittyMainController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectStepController").Include("~/Scripts/ScriptPage/ProjectStepController.js"));
            bundles.Add(new ScriptBundle("~/Business/AuthorizationController").Include("~/Scripts/ScriptPage/AuthorizationController.js"));
            bundles.Add(new ScriptBundle("~/Business/NoteController").Include("~/Scripts/ScriptPage/NoteController.js"));
            bundles.Add(new ScriptBundle("~/Business/NotePeriodController").Include("~/Scripts/ScriptPage/NotePeriodController.js"));
            bundles.Add(new ScriptBundle("~/Business/NoteExtractedController").Include("~/Scripts/ScriptPage/NoteExtractedController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectParametersController").Include("~/Scripts/ScriptPage/ProjectParametersController.js"));
            bundles.Add(new ScriptBundle("~/Business/ExpenseTypeController").Include("~/Scripts/ScriptPage/ExpenseTypeController.js"));
            bundles.Add(new ScriptBundle("~/Business/ExpensesController").Include("~/Scripts/ScriptPage/ExpensesController.js"));
            bundles.Add(new ScriptBundle("~/Business/ExpenseAdministratorController").Include("~/Scripts/ScriptPage/ExpenseAdministratorController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProfileController").Include("~/Scripts/ScriptPage/ProfileController.js"));
            bundles.Add(new ScriptBundle("~/Business/UserController").Include("~/Scripts/ScriptPage/UserController.js"));
            bundles.Add(new ScriptBundle("~/Business/MenuController").Include("~/Scripts/ScriptPage/MenuController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectTypeMainController").Include("~/Scripts/ScriptPage/ProjectTypeMainController.js"));
            bundles.Add(new ScriptBundle("~/Business/ProjectTypeController").Include("~/Scripts/ScriptPage/ProjectTypeController.js"));
            bundles.Add(new ScriptBundle("~/Business/ExpenseExtractedController").Include("~/Scripts/ScriptPage/ExpenseExtractedController.js"));
            bundles.Add(new ScriptBundle("~/Business/NoteAdministratorController").Include("~/Scripts/ScriptPage/NoteAdministratorController.js"));
            bundles.Add(new ScriptBundle("~/Business/MessageController").Include("~/Scripts/ScriptPage/MessageController.js"));
            bundles.Add(new ScriptBundle("~/Business/LocalConfigurationController").Include("~/Scripts/ScriptPage/LocalConfigurationController.js"));
            #endregion
        }
    }
}