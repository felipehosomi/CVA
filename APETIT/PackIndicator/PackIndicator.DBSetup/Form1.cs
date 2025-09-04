using SAPbobsCOM;
using System;
using System.Windows.Forms;

namespace PackIndicator.DBSetup
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

                #region CVA|Priorização de PN
                tableName = "@CVA_OBPP";
                tableDesc = "CVA|Priorização de PN";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject("CVA_OBPP", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                                                    CanArchive: false,
                                                    CanCancel: false,
                                                    CanClose: false,
                                                    CanCreateDefaultForm: false,
                                                    CanDelete: true,
                                                    CanFind: true,
                                                    CanLog: true,
                                                    CanYearTransfer: false,
                                                    ManageSeries: false,
                                                    tbxLog: ref txtLog);
                #endregion

                #region CVA|Priorização de PN(L)
                tableName = "@CVA_BPP1";
                tableDesc = "CVA|Priorização de PN(L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "WHsCode", "Código do depósito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Address", "Nome endereço entrega", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Priority", "Prioridade", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_OBPP", tableName, ref txtLog);
                #endregion

                #region CVA|Rotas de entrega
                tableName = "@CVA_ROTAENTREGA";
                tableDesc = "CVA|Rotas de entrega";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_DESCRICAO", "Descrição da Rota", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_FILIAL_PRINCIPAL", "Filial Principal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_FILIAL_P_NOME", "Nome Filial Principal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject("CVA_ROTAENTREGA", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                                                    CanArchive: false,
                                                    CanCancel: false,
                                                    CanClose: false,
                                                    CanCreateDefaultForm: false,
                                                    CanDelete: true,
                                                    CanFind: true,
                                                    CanLog: true,
                                                    CanYearTransfer: false,
                                                    ManageSeries: false,
                                                    tbxLog: ref txtLog);
                #endregion

                #region CVA|Rotas de entrega(L)
                tableName = "@CVA_LN_ROTAENTREGA";
                tableDesc = "CVA|Rotas de entrega(L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_FILIAL_DESTINO", "Filial Destino", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_FILIAL_D_NOME", "Nome Filial Destino", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Calendar", "Calendário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "TranspDays", "Dias de transporte", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_ROTAENTREGA", tableName, ref txtLog);
                #endregion

                #region Warehouses
                tableName = "OWHS";
                tableDesc = "Depósitos";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_UomControl", "Controle de UM", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_DueDateLimit", "Tempo mínimo de vencimento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                // Valid values
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_UomControl", "N", "Não", true, ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_UomControl", "Y", "Sim", ref txtLog);
                #endregion

                #region Orders
                tableName = "RDR1";
                tableDesc = "Linhas do pedidos de venda";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_OriginalQty", "Quantidade original", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_OriginalUom", "UM original", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_DueDate", "Data de vencimento", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_SuggestedTotal", "Total sugerido", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_Break", "Ruptura", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                // Valid values
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_Break", "N", "Não", true, ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_Break", "Y", "Sim", ref txtLog);
                #endregion

                #region Payment Methods
                tableName = "OPYM";
                tableDesc = "Formas de pagamento";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_ValidaCNPJ", "Valida CNPJ conta bancária", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_OPYM_CNPJ", "CNPJ Conta Bancária", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 14, ref txtLog);

                // Valid values
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_ValidaCNPJ", "N", "Não", true, ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "CVA_ValidaCNPJ", "Y", "Sim", ref txtLog);
                #endregion



                //#region Business Places
                //tableName = "OBPL";
                //tableDesc = "Filiais";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "DadosCobranca", "Dados de cobrança", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ObsFilial", "Observação da filial", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0, ref txtLog);
                //#endregion

                txtLog.AppendText("\r\n\r\n" + "Setup finalizado." + "\r\n");
            }
            catch (Exception ex)
            {
                txtLog.Text = string.Concat(txtLog, Environment.NewLine, ex.Message, Environment.NewLine);
            }
        }
    }
}
