using System;
using Company = SAPbobsCOM.Company;
using SAPbouiCOM;
using Application = SAPbouiCOM.Application;
using System.Windows.Forms;

namespace EmailAutorizacao.HELPER
{
    public class B1Connection
    {
        private static readonly Lazy<B1Connection> _instance = new Lazy<B1Connection>(() => new B1Connection());

        public Application Application;
        public Company Company;

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
            Company = (Company)Application.Company.GetDICompany();
        }

        private void SetApplication()
        {
            try
            {
                var connString = Environment.GetCommandLineArgs().GetValue(1).ToString();
                var oSboGuiApi = new SboGuiApi();
                oSboGuiApi.Connect(connString);
                Application = oSboGuiApi.GetApplication();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    @"Erro de conexão", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
                throw;
            }
        }
    }
}
