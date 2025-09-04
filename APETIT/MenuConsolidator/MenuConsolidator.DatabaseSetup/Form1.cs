using SAPbobsCOM;
using System;
using System.Windows.Forms;

namespace MenuConsolidator.DatabaseSetup
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

                #region Categorias de estocagem
                tableName = "@CVA_OSCT";
                tableDesc = "CVA|Categorias de estocagem";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
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

                #region Categorias de estocagem (L)
                tableName = "@CVA_SCT1";
                tableDesc = "CVA|Cat. estocagem (L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "Description", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_OSCT", tableName, ref txtLog);
                #endregion

                #region Parâmetros MRP
                tableName = "@CVA_OPAM";
                tableDesc = "CVA|Parâmetros MRP";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
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

                #region Parâmetros MRP (linhas)
                tableName = "@CVA_PAM1";
                tableDesc = "CVA|Parâmetros MRP (L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "ItmsGrpCod", "Grupo de item", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Familia", "Família", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "SFamilia", "Sub-Família", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "ItemCode", "Nº do item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "ItemName", "Descrição do item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CardCode", "Cód. Fornecedor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CardName", "Nome do fornecedor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Calendar", "Calendário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "LeadTime", "Lead time", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "NonDeliveryDate", "Datas de não entrega", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Category", "Categoria de estocagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "BUsage", "Utilização compra", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "SUsage", "Utilização venda", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "ListNum", "Lista de preço", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_OPAM", tableName, ref txtLog);
                #endregion

                #region Calendário de recebimento
                tableName = "@CVA_OCLN";
                tableDesc = "CVA|Calendário recebimento";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "Period", "Periodicidade", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Year", "Ano", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 4, ref txtLog);

                UserObjectsService.AddValidValueToUserField(tableName, "Period", "8", "Manual", true, ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "1", "Toda segunda-feira", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "2", "Toda terça-feira", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "3", "Toda quarta-feira", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "4", "Toda quinta-feira", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "5", "Toda sexta-feira", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "6", "Todo sábado", ref txtLog);
                UserObjectsService.AddValidValueToUserField(tableName, "Period", "0", "Todo domingo", ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
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

                #region Calendário recebimento (L)
                tableName = "@CVA_CLN1";
                tableDesc = "CVA|Calendário recebimento (L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "Date", "Data", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Weekday", "Dia da semana", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 13, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_OCLN", tableName, ref txtLog);
                #endregion

                #region Datas de não entrega
                tableName = "@CVA_ONDL";
                tableDesc = "CVA|Datas de não entrega";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                // UDO
                UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
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

                #region Datas de não entrega (L)
                tableName = "@CVA_NDL1";
                tableDesc = "CVA|Datas de não entrega (L)";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "Date", "Data", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Rescheduling", "Reagendamento", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "Comments", "Observações", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                // UDO
                UserObjectsService.AddChildTableToUserObject("CVA_ONDL", tableName, ref txtLog);
                #endregion

                #region Previsão de transferência
                tableName = "@CVA_OPTR";
                tableDesc = "CVA|Previsão de transferência";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDT
                UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

                // UDF
                UserObjectsService.CreateUserField(tableName, "DocType", "Tipo de documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "BPLId", "ID da filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                #endregion

                #region Linhas de documentos de marketing
                tableName = "RDR1";
                tableDesc = "Linhas de documentos de marketing";

                txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDF
                UserObjectsService.CreateUserField(tableName, "CVA_CatEstoque", "Categoria de estocagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_PrchDueDate", "Data de entrega original", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_RotaEntrega", "Rota de entrega", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                #endregion

                txtLog.AppendText("\r\n\r\n" + "Setup finalizado." + "\r\n");
            }
            catch (Exception ex)
            {
                txtLog.Text = string.Concat(txtLog, Environment.NewLine, ex.Message, Environment.NewLine);
            }
        }
    }
}
