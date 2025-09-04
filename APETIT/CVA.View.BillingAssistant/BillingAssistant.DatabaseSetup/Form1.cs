using SAPbobsCOM;
using System;
using System.Windows.Forms;

namespace BillingAssistant.DatabaseSetup
{
    public partial class Form1 : Form
    {
        public Company Company = new Company();
        public string MsgErro = string.Empty;
        public int CodErro;
        private const string CONN_STRING = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
        private static SAPbouiCOM.Application SBO_Application = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                #region [ Conexões ]
                txtLog.AppendText("Iniciando conexão à instância aberta do SAP Business One.");

                // Nova instância do objeto Company
                SAPbouiCOM.SboGuiApi oSBOGuiApi = null;
                oSBOGuiApi = new SAPbouiCOM.SboGuiApi();
                oSBOGuiApi.Connect(CONN_STRING);
                SBO_Application = oSBOGuiApi.GetApplication(-1);

                if (SBO_Application == null)
                {
                    txtLog.AppendText(String.Format("\r\n" + "Erro de conexão: Não possível se conectar à instância aberta do SAP Business One. Verifique se há alguma instância aberta do ERP."));
                    return;
                }

                Company = UserObjectsService.oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();

                txtLog.AppendText("\r\n" + "Conexão realizada com sucesso.");
                #endregion

                var tableName = String.Empty;
                var tableDesc = String.Empty;

                ///UDF
                ///
                ///

                txtLog.AppendText("\r\n\r\n" + "Setup finalizado." + "\r\n");
            }
            catch (Exception ex)
            {
                txtLog.Text = string.Concat(txtLog, Environment.NewLine, ex.Message, Environment.NewLine);
            }
        }
    }
}
