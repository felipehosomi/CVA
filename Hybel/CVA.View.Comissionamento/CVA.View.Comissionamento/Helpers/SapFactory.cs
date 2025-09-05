using System;
using Company = SAPbobsCOM.Company;
using SAPbouiCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class SapFactory
    {
        public Company Company { get; private set; }
        public Application Application { get; private set; }
        public EventFilters Filters { get; private set; }

        public SapFactory()
        {
            Connect();
        }

        private void Connect()
        {
            SetApplication();
            Company = (Company)Application.Company.GetDICompany();
            Filters = new EventFilters();
        }

        private void SetApplication()
        {
            var sboGuiApi = new SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            sboGuiApi.Connect(Environment.GetCommandLineArgs().GetValue(1).ToString());
            Application = sboGuiApi.GetApplication();
        }
    }
}
