using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public class B1Connection
    {
        private static readonly Lazy<B1Connection> _instance = new Lazy<B1Connection>(() => new B1Connection());
        public SAPbobsCOM.Company Company { get; private set; }
        public SAPbouiCOM.Application Application { get; private set; }
        public SAPbouiCOM.EventFilters Filters { get; private set; }
        public static B1Connection Instance { get { return _instance.Value; } }
        public SAPbouiCOM.Framework.Application App { get; private set; }

        private B1Connection()
        {
            Connect();
        }

        private void Connect()
        {
            var app = SetApplication();
            Company = (SAPbobsCOM.Company)Application.Company.GetDICompany();
            Filters = new SAPbouiCOM.EventFilters();
            App = app;
        }

        private SAPbouiCOM.Framework.Application SetApplication()
        {
            //var sboGuiApi = new SAPbouiCOM.SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            //sboGuiApi.Connect(str);
            //Application = sboGuiApi.GetApplication();

            var app = new SAPbouiCOM.Framework.Application(str);
            Application = SAPbouiCOM.Framework.Application.SBO_Application;
            return app;
        }
    }
}
