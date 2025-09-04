using SAPbobsCOM;
using System;
using System.Windows.Forms;

namespace MenuPlanner.DatabaseSetup
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //#region [ Tabelas de Backup ]

                //tableName = "@CVA_PACK_CONFIG";
                //tableDesc = "[CVA] Config. Embalagem";

                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_URL", "URL", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TOKEN", "Token", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                //#region Serviços_2
                //tableName = "@CVA_SERVICO_PLAN_2";
                //tableDesc = "[CVA] Serviços_2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ATIVO", "ATIVO", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_ATIVO", "Y", "Sim", true, ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_ATIVO", "N", "Não", ref txtLog);
                //#endregion

                //#region Turnos
                //tableName = "@CVA_TURNO_2";
                //tableDesc = "[CVA] Turnos_2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);
                //#endregion

                //#region Tipos de proteína_2
                //tableName = "@CVA_TIPOPROTEINA_2";
                //tableDesc = "[CVA] Tipos de proteína_2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_FAMILIA", "Id da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_FAMILIA", "Descrição da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SUB_FAMILIA", "Id da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_SUB_FAMILIA", "Descrição da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //#endregion

                //#region Tipos de prato_2
                //tableName = "@CVA_TIPOPRATO_2";
                //tableDesc = "[CVA] Tipos de prato_2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObject, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_FAMILIA", "Id da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_FAMILIA", "Descrição da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SUB_FAMILIA", "Id da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_SUB_FAMILIA", "Descrição da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_PROTEINA", "Proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_PROTEINA", "Y", "Sim", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_PROTEINA", "N", "Não", true, ref txtLog);
                //#endregion

                //#region Planejamento de cardápio_2
                //tableName = "@CVA_PLANEJAMENTO_2";
                //tableDesc = "[CVA] Plan. de Cadápio_2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObjectAutoIncrement, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CLIENTE", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "ID do contrato guarda-chuva", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_MODEL_CARD", "Id do modelo de cardápio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_VIGENCIA_CONTR", "Vigência do contrato", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_CLIENTE", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_MODELO_CARD", "Descr. Modelo Card", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DATA_REF", "Data de referência", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_SERVICO", "ID do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_SERVICO", "Descrição do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_G_SERVICO", "ID do grupo de serviços", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_G_SERVICO", "Descrição do grupo de serviços", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //#endregion

                //#region Ln Plan. de Cad._2
                //tableName = "@CVA_LN_PLANEJAMEN_2";
                //tableDesc = "[CVA] Ln Plan. de Cad._2";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_NoObjectAutoIncrement, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_PLAN_ID", "Id Planejamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO", "Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO_DES", "Descr. Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_INSUMO", "Id Insumo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_INSUMO_DES", "Descr. Insumo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_PERCENT", "%", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QTD_ORI", "Qtd. Original", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Price, 5, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_CUSTO_ITEM", "Custo Médio", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TOTAL", "Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_MODELO_LIN_ID", "Id Lin Model", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DIA_SEMANA", "Dia Semana", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_LOTE_CONSOL", "Lote Consolidado", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //#endregion

                //#endregion

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //#region [ Remoção de tabelas antigas ]
                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_SERVICO_PLAN");
                //UserObjectsService.RemoveUserTable("@CVA_SERVICO_PLAN", ref txtLog);

                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_TURNO");
                //UserObjectsService.RemoveUserTable("@CVA_TURNO", ref txtLog);

                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_TIPOPRATO");
                //UserObjectsService.RemoveUserTable("@CVA_TIPOPRATO", ref txtLog);

                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_TIPOPROTEINA");
                //UserObjectsService.RemoveUserTable("@CVA_TIPOPROTEINA", ref txtLog);

                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_PLANEJAMENTO");
                //UserObjectsService.RemoveUserTable("@CVA_PLANEJAMENTO", ref txtLog);

                //txtLog.AppendText("\r\n\r\n" + "Remoção da tabela @CVA_LN_PLANEJAMENTO");
                //UserObjectsService.RemoveUserTable("@CVA_LN_PLANEJAMENTO", ref txtLog);
                //#endregion

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                #region [ Recriação das tabelas e campos ]

                //#region Serviços
                //tableName = "@CVA_SERVICO_PLAN";
                //tableDesc = "[CVA] Serviços";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ATIVO", "ATIVO", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_ATIVO", "Y", "Sim", true, ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_ATIVO", "N", "Não", ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Turnos
                //tableName = "@CVA_TURNO";
                //tableDesc = "[CVA] Turnos";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Grupo de serviços
                //tableName = "@CVA_GRPSERVICOS";
                //tableDesc = "[CVA] Grupo de serviços";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_DESCRICAO", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "ID do contrato guarda-chuva", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "ID contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_C_PADRAO", "Custo padrão", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QTD_MIN_FAT", "Qtd. Min Fatura", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ServiceId", "Id do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ServiceName", "Descrição do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CARDGRPS", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Grupo de serviços (L)
                //tableName = "@CVA_LIN_GRPSERVICOS";
                //tableDesc = "[CVA] Grupo de serviços (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_SERVICO", "Id Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_SERVICO", "Descr. Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TURNO", "Turno", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_AGRUP", "Id Agrupamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_AGRUP", "Descr. Agrupamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_VALOR", "Valor R$", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SEGUNDA", "Segunda", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TERCA", "Terça", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QUARTA", "Quarta", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QUINTA", "Quinta", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SEXTA", "Sexta", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SABADO", "Sábado", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DOMINGO", "Doming", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CARDGRPS", tableName, ref txtLog);
                //#endregion

                //#region Tipos de prato
                //tableName = "@CVA_TIPOPRATO";
                //tableDesc = "[CVA] Tipos de prato";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_PROTEINA", "Proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_PROTEINA", "Y", "Sim", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "CVA_PROTEINA", "N", "Não", true, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Tipos de prato (L)
                //tableName = "@CVA_DST1";
                //tableDesc = "[CVA] Tipos de prato (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "ItmsGrpCod", "Código do grupo de itens", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ItmsGrpNam", "Nome do grupo de itens", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CVA_TIPOPRATO", tableName, ref txtLog);
                //#endregion

                //#region Tipos de proteína
                //tableName = "@CVA_TIPOPROTEINA";
                //tableDesc = "[CVA] Tipos de proteína";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_FAMILIA", "Id da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_FAMILIA", "Descrição da Familia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_SUB_FAMILIA", "Id da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_D_SUB_FAMILIA", "Descrição da Subfamilia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject(tableName.Replace("@", ""), tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Modelos de cardápio
                //tableName = "@CVA_MCARDAPIO";
                //tableDesc = "[CVA] Modelos de cardápio";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_DESCRICAO", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_SERVICO", "Id Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_SERVICO", "Descr. Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "Id Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_CONTRATO", "Descr. Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "Agreement AbsID", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CARDMDLC", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Modelos de Cardápio (L)
                //tableName = "@CVA_LIN_MCARDAPIO";
                //tableDesc = "[CVA] Modelos de cardápio (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO", "Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO_DES", "Descr. Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CARDMDLC", tableName, ref txtLog);
                //#endregion

                //#region Incidência e gramatura de proteínas
                //tableName = "@CVA_OIGP";
                //tableDesc = "[CVA] Inc. Gram. Proteína";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "ID do contrato guarda-chuva", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CVA_OIGP", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Incidência e gramatura de proteínas (L)
                //tableName = "@CVA_IGP1";
                //tableDesc = "[CVA] Inc. Gram. Proteína (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "ProteinCode", "Código do tipo da proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ProteinName", "Nome do tipo da proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Weight", "Gramtura", BoFieldTypes.db_Float, BoFldSubTypes.st_Measurement, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Incidence", "Incidência", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CVA_OIGP", tableName, ref txtLog);
                //#endregion

                //#region Itens denegados
                //tableName = "@CVA_ODNI";
                //tableDesc = "[CVA] Itens denegados";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "ID do contrato guarda-chuva", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CVA_ODNI", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Itens denegados (L)
                //tableName = "@CVA_DNI1";
                //tableDesc = "[CVA] Itens denegados (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "ItemCode", "Código do tipo da proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ItemName", "Nome do tipo da proteína", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CVA_ODNI", tableName, ref txtLog);
                //#endregion

                //#region Dias sem serviço
                //tableName = "@CVA_CALENDSC";
                //tableDesc = "[CVA] Itens denegados";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "ID Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_GRPSERVICO", "ID Grupo Serv.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_GRPSERVICO", "Descr. Grupo Ser.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "ID do contrato guarda-chuva", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CARDCALE", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Dias sem serviços (L)
                //tableName = "@CVA_LIN_CALENDSC";
                //tableDesc = "[CVA] Itens denegados (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_DATA", "Data", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_MOTIVO", "Motivo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CARDCALE", tableName, ref txtLog);
                //#endregion

                //#region Preço X Volume
                //tableName = "@CVA_TABPRCVOL";
                //tableDesc = "[CVA] Preço X Volume";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterData, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "ID Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_GRPSERVICO", "ID Grupo Serv.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_GRPSERVICO", "Descr. Grupo Ser.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_APARTIR", "A Partir De", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardCode", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CardName", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "ID do contrato guarda-chuva", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ServiceId", "Id do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "ServiceName", "Descrição do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CARDPRXV", tableDesc, tableName, BoUDOObjType.boud_MasterData,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Preço X Volume (L)
                //tableName = "@CVA_LIN_TABPRCVOL";
                //tableDesc = "[CVA] Preço X Volume (L)";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_MasterDataLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_QTD_DE", "Qtd De", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 5, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QTD_ATE", "Qtd Até", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 5, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_PRECO_UNIT", "Preço Unitário", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 0, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CARDPRXV", tableName, ref txtLog);
                //#endregion

                //#region Planejamento de cardápio
                tableName = "@CVA_PLANEJAMENTO";
                tableDesc = "[CVA] Plan. de Cadápio";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_Document, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CLIENTE", "Código do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_CONTRATO", "ID do contrato guarda-chuva", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "AbsID", "Contrato guarda-chuva", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_MODEL_CARD", "Id do modelo de cardápio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_VIGENCIA_CONTR", "Vigência do contrato", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_CLIENTE", "Nome do PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_MODELO_CARD", "Descr. Modelo Card", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DATA_REF", "Data de referência", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_SERVICO", "ID do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_SERVICO", "Descrição do serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ID_G_SERVICO", "ID do grupo de serviços", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DES_G_SERVICO", "Descrição do grupo de serviços", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                UserObjectsService.CreateUserField(tableName, "CVA_COST_DATE_FROM", "Data custo de", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 1, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_COST_DATE_TO", "Data custo até", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 1, ref txtLog);

                //UserObjectsService.AddValidValueToUserField(tableName, "Status", "P", "Planejeado", true, ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "Status", "R", "Liberado", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "Status", "L", "Fechado", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "Status", "C", "Cancelado", ref txtLog);

                //// UDO
                //UserObjectsService.CreateUserObject("CVA_OMNP", tableDesc, tableName, BoUDOObjType.boud_Document,
                //                                    CanArchive: false,
                //                                    CanCancel: false,
                //                                    CanClose: false,
                //                                    CanCreateDefaultForm: false,
                //                                    CanDelete: true,
                //                                    CanFind: true,
                //                                    CanLog: true,
                //                                    CanYearTransfer: false,
                //                                    ManageSeries: false,
                //                                    tbxLog: ref txtLog);
                //#endregion

                //#region Ln Plan. de Cad.
                tableName = "@CVA_LN_PLANEJAMENTO";
                tableDesc = "[CVA] Ln Plan. de Cad.";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_DocumentLines, ref txtLog);

                //// UDF
                ////UserObjectsService.CreateUserField(tableName, "CVA_PLAN_ID", "Id Planejamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO", "Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TIPO_PRATO_DES", "Descr. Tipo de prato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_INSUMO", "Id Insumo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_INSUMO_DES", "Descr. Insumo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_PERCENT", "%", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_QTD_ORI", "Qtd. Original", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Price, 5, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_CUSTO_ITEM", "Custo Médio", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_TOTAL", "Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 0, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_MODELO_LIN_ID", "Id Lin Model", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_DIA_SEMANA", "Dia Semana", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Day", "Dia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_LOTE_CONSOL", "Lote Consolidado", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "LineStatus", "Status da linha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, ref txtLog);

                //// Valid values
                //UserObjectsService.AddValidValueToUserField(tableName, "LineStatus", "P", "Planejeado", true, ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "LineStatus", "R", "Liberado", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "LineStatus", "L", "Fechado", ref txtLog);
                //UserObjectsService.AddValidValueToUserField(tableName, "LineStatus", "C", "Cancelado", ref txtLog);

                //UserObjectsService.CreateUserField(tableName, "CVA_OWOR", "DocEntry OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                UserObjectsService.CreateUserField(tableName, "CVA_OWOR_ALL", "Todos DocEntry OP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_ERROR", "Erro geração OP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CVA_OMNP", tableName, ref txtLog);
                //#endregion

                //#region Quantidade por turnos
                //tableName = "@CVA_MNP2";
                //tableDesc = "[CVA] Quantidade por turnos";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDT
                //UserObjectsService.CreateUserTable(tableName, tableDesc, BoUTBTableType.bott_DocumentLines, ref txtLog);

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "Day", "Dia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Weekday", "Dia da semana", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 3, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Shift", "Turno", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "Quantity", "Quantidade de pratos", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //// UDO
                //UserObjectsService.AddChildTableToUserObject("CVA_OMNP", tableName, ref txtLog);
                //#endregion

                //#region Purchase Orders
                //tableName = "OPOR";
                //tableDesc = "Pedidos de compra";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_AgrAbsID", "Contrato guarda-chuva de venda", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //#endregion

                //#region Production Order
                //tableName = "OWOR";
                //tableDesc = "Ordem de produção";

                //txtLog.AppendText("\r\n\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                //// UDF
                //UserObjectsService.CreateUserField(tableName, "CVA_PlanCode", "Nº planejamento de cardápio", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);
                //UserObjectsService.CreateUserField(tableName, "CVA_LineId", "Linha planej. cardápio", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);

                //#endregion

                tableName = "OUOM";
                tableDesc = "Unidade de Medida";

                UserObjectsService.CreateUserField(tableName, "CVA_CodUM", "Unidade Medida Anterior", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, ref txtLog);

                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                txtLog.AppendText("\r\n\r\n" + "Setup finalizado." + "\r\n");
            }
            catch (Exception ex)
            {
                txtLog.Text = string.Concat(txtLog, Environment.NewLine, ex.Message, Environment.NewLine);
            }
        }
    }
}
