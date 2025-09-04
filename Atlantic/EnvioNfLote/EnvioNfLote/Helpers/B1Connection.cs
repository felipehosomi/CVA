using System;
using System.Windows.Forms;

namespace EnvioNfLote.Helpers
{
    public class B1Connection
    {
        private static readonly Lazy<B1Connection> _instance = new Lazy<B1Connection>(() => new B1Connection());

        public SAPbouiCOM.Application Application;
        public SAPbobsCOM.Company Company;

        private B1Connection()
        {
            Connect();
        }

        public static B1Connection Instance
        {
            get { return _instance.Value; }
        }

        private void Connect()
        {
            SetApplication();
            Company = (SAPbobsCOM.Company)Application.Company.GetDICompany();
        }

        private void SetApplication()
        {
            try
            {
                string connString = Environment.GetCommandLineArgs().GetValue(1).ToString();
                var oSboGuiApi = new SAPbouiCOM.SboGuiApi();
                oSboGuiApi.Connect(connString);
                Application = oSboGuiApi.GetApplication();
            }
            catch (Exception ex)
            {
                string msg = ex.Message + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg, @"Erro de conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
