using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.FiltroRegional.Helpers
{
    public class B1Connection
    {
        private static readonly Lazy<B1Connection> _instance = new Lazy<B1Connection>(() => new B1Connection());
        public SAPbobsCOM.Company Company { get; private set; }
        public SAPbouiCOM.Application Application { get; private set; }
        public SAPbouiCOM.EventFilters Filters { get; private set; }
        public static B1Connection Instance { get { return _instance.Value; } }

        private B1Connection()
        {
            Connect();
        }

        private void Connect()
        {
            SetApplication();
            Company = (SAPbobsCOM.Company)Application.Company.GetDICompany();
            Filters = new SAPbouiCOM.EventFilters();
        }

        private void SetApplication()
        {
            var sboGuiApi = new SAPbouiCOM.SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            sboGuiApi.Connect(str);
            Application = sboGuiApi.GetApplication();
        }
    }
}
