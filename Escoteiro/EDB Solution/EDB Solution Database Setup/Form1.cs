using SAPbobsCOM;
using System;
using System.Windows.Forms;

namespace EDB_Solution_Database_Setup
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
                txtLog.Text = string.Concat(txtLog.Text, "Iniciando conexão ao banco de dados do SAP Business One.", Environment.NewLine);

                // Nova instância do objeto Company
                SAPbouiCOM.SboGuiApi oSBOGuiApi = null;
                oSBOGuiApi = new SAPbouiCOM.SboGuiApi();
                oSBOGuiApi.Connect(CONN_STRING);
                SBO_Application = oSBOGuiApi.GetApplication(-1);

                if (SBO_Application == null)
                {
                    txtLog.Text = string.Concat(txtLog, Environment.NewLine, "Não possível se conectar ao SAP Business One. Erro", Environment.NewLine);
                    return;
                }

                Company = UserObjectsService.oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();

                txtLog.Text = string.Concat(txtLog.Text, "Conexão realizada com sucesso.", Environment.NewLine);
            }
            catch (Exception ex)
            {
                txtLog.Text = string.Concat(txtLog, Environment.NewLine, ex.Message, Environment.NewLine);
            }

            var tableName = String.Empty;
            var tableDesc = String.Empty;

            #region Categorias do produto
            tableName = "@CVA_OICT";
            tableDesc = "CVA|Categorias do produto";

            // UDT
            UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

            txtLog.AppendText(String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName) + Environment.NewLine);

            // UDF
            UserObjectsService.CreateUserField(tableName, "ID", "ID da categoria", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Name", "Nome da categoria", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "ItemCode", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            #endregion

            #region Cadastro de Parceiro de Negócios
            UserObjectsService.CreateUserField("OCRD", "CVA_EntityId", "ID Cliente Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            #endregion

            #region Pedido de Venda
            UserObjectsService.CreateUserField("ORDR", "CVA_EntityId", "ID Pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField("ORDR", "CVA_Increment_id", "Nº Pedido Site Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField("ORDR", "CVA_IntegratedCancellation", "Magento|Cancelamento integrado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

            UserObjectsService.AddValidValueToUserField("ORDR", "CVA_IntegratedCancellation", "N", "Não", true, ref txtLog);
            UserObjectsService.AddValidValueToUserField("ORDR", "CVA_IntegratedCancellation", "Y", "Sim", ref txtLog);
            #endregion

            #region Cadastro de Itens
            UserObjectsService.CreateUserField("OITM", "CVA_ShortDescription", "Magento|Descrição curta", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0, ref txtLog);
            UserObjectsService.CreateUserField("OITM", "CVA_Url", "Magento|URL", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0, ref txtLog);
            UserObjectsService.CreateUserField("OITM", "CVA_Integrated", "Magento|Integrado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

            UserObjectsService.AddValidValueToUserField("OITM", "CVA_Integrated", "N", "Não", true, ref txtLog);
            UserObjectsService.AddValidValueToUserField("OITM", "CVA_Integrated", "Y", "Sim", ref txtLog);
            #endregion

            #region Contas a Receber
            UserObjectsService.CreateUserField("ORCT", "CVA_EntityId", "ID Pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField("ORCT", "CVA_Increment_id", "Nº Pedido Site Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            #endregion

            #region Pedido de Venda Magento
            tableName = "@CVA_ORDERS_MAGENTO";
            tableDesc = "CVA|Pedido de Venda Magento";

            UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObjectAutoIncrement, ref txtLog);

            txtLog.AppendText(String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName) + Environment.NewLine);

            UserObjectsService.CreateUserField(tableName, "EntityId", "ID pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "State", "Situação do pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Status", "Status do pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Data", "Data criação", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Hora", "Hora criação", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "JSON", "JSON", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 254, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "StatusProc", "Status processamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 3, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Mensagem", "Mensagem", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 254, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "DataProc", "Data processado", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "HoraProc", "Hora processao", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "DocEntry", "Nº interno SAP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "ObjType", "Tipo de objeto SAP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "IntegrateStatus", "Integrar novo status ao Magento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);
            #endregion

            #region Magento Parâmetrizações
            tableName = "@CVA_MAGENTO_PARAM";
            tableDesc = "CVA|Magento Parâmetrizações";

            UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObjectAutoIncrement, ref txtLog);

            txtLog.AppendText(String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName) + Environment.NewLine);

            UserObjectsService.CreateUserField(tableName, "BplID", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Deposito", "Depósito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Sequencia", "Sequência", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Utilizacao", "Utilização", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Series", "Série Cliente", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "TaxExpsCode", "Imposto do Frete", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Metodo", "Método Crédito Magento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "State", "State Pagamento Pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "Status", "Status Pagamento Pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "CrTypeName", "Método Crédito SAP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "CashAccount", "Conta contábil de pagamento com dinheiro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "CreditCardOp", "Operadora do cartão de crédito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
            #endregion

            #region Magento Registro de Datas
            tableName = "@CVA_MAGENTO_DT";
            tableDesc = "CVA|Magento Registro de Datas";

            UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObjectAutoIncrement, ref txtLog);

            txtLog.AppendText(String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName) + Environment.NewLine);

            UserObjectsService.CreateUserField(tableName, "EndPoint", "EndPoint", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "DataCreate", "Data Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "HoraCreate", "Hora Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "SegundoCreate", "Segundo Criação", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Time, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "DataUpdate", "Data Update", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "HoraUpdate", "Hora Update", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "SegundoUpdate", "Segundo Update", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Time, 10, ref txtLog);
            #endregion


            #region Magento Sincronização de Estoque
            tableName = "@CVA_STOCK_MAGENTO";
            tableDesc = "CVA|Magento Sinc. Estoque";

            UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

            txtLog.AppendText(String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName) + Environment.NewLine);

            UserObjectsService.CreateUserField(tableName, "BarCode", "Código de barras", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "ItemCode", "Código do item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "ItemName", "Descrição do item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "WhsQty", "Quantidade em estoque", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 0, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "LastSyncDt", "data da última  de sincronização", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, ref txtLog);
            UserObjectsService.CreateUserField(tableName, "LastSyncHr", "Hora da última sincronização", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10, ref txtLog);
            #endregion

            txtLog.AppendText(string.Concat(Environment.NewLine, "Configuração finalizada com sucesso."));
        }
    }
}
