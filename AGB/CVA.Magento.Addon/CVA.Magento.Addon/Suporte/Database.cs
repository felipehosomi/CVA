using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SAPbobsCOM;

namespace CVA.Magento.Addon
{
    public static class Database
    {
        private static SAPbouiCOM.Application sboApp;
        private static Company oCompany;
        private static int retCode;

        public static int RetCode
        {
            get { return retCode; }
            set
            {
                if (value != 0)
                {
                    int errorCode;
                    string errorMessage;
                    oCompany.GetLastError(out errorCode, out errorMessage);
                    throw new Exception($"[{errorCode}] {errorMessage}");
                }

                retCode = value;
            }
        }

        public static SAPbouiCOM.Application SboApp
        {
            get
            {
                return sboApp;
            }
        }

        public static Company Company
        {
            get
            {
                return oCompany;
            }
        }

        static Database()
        {
            sboApp = Application.SBO_Application;
            oCompany = (Company)sboApp.Company.GetDICompany();
        }

        public static void Initialize()
        {
            try
            {
                CreateDatabaseObjects();
            }
            catch (Exception ex)
            {
                sboApp.StatusBar.SetText("Erro ao inicializar add-on: " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                throw;
            }
        }

        #region [ AddUserField ]

        public static void AddUserField(string NomeTabela, string NomeCampo, string DescCampo, SAPbobsCOM.BoFieldTypes Tipo, SAPbobsCOM.BoFldSubTypes SubTipo, Int16 Tamanho, string[,] valoresValidos, string valorDefault, string linkedTable)
        {
            int lErrCode;
            string sErrMsg = "";
            var oRS = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string sQuery = string.Format(@"select COUNT(*)  
                                            from CUFD 
                                            where ""TableID"" = '{0}' 
                                              and ""AliasID"" = '{1}'", NomeTabela, NomeCampo);
            oRS.DoQuery(sQuery);
            //0 - Campo Não exite
            //1 - Campos Existe
            int resultado = (int)oRS.Fields.Item(0).Value;
            if (resultado == 0)
            {
                try
                {
                    //string sSquery = "SELECT ""[name]"" FROM syscolumns WHERE ""[name]"" = 'U_" + NomeCampo + " ' and id = (SELECT id FROM sysobjects WHERE type = 'U'AND [NAME] = '" + NomeTabela.Replace("[", "").Replace("]", "") + "')";
                    //object oResult = ConexaoSAP.ExecuteHanaScalar(sSquery);
                    //if (oResult != null) return;

                    sboApp.StatusBar.SetText($"Criando campo {NomeTabela}.U_{NomeCampo}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    //ConexaoSAP.uiApplication.SetStatusBarMessage(String.Format(@"Criando campo [{0}] na tabela [{1}]", NomeCampo, NomeTabela), BoMessageTime.bmt_Short, false);

                    SAPbobsCOM.UserFieldsMD oUserField;
                    oUserField = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                    oUserField.TableName = NomeTabela.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                    oUserField.Name = NomeCampo;
                    oUserField.Description = DescCampo;
                    oUserField.Type = Tipo;
                    oUserField.SubType = SubTipo;
                    oUserField.DefaultValue = valorDefault;
                    if (!string.IsNullOrEmpty(linkedTable)) oUserField.LinkedTable = linkedTable;

                    //adicionar valores válidos
                    if (valoresValidos != null)
                    {
                        Int32 qtd = valoresValidos.GetLength(0);
                        if (qtd > 0)
                        {
                            for (int i = 0; i < qtd; i++)
                            {
                                oUserField.ValidValues.Value = valoresValidos[i, 0];
                                oUserField.ValidValues.Description = valoresValidos[i, 1];
                                oUserField.ValidValues.Add();
                            }
                        }
                    }

                    if (Tamanho != 0)
                        oUserField.EditSize = Tamanho;

                    try
                    {
                        oUserField.Add();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserField);
                        oUserField = null;
                        oCompany.GetLastError(out lErrCode, out sErrMsg);

                        if (lErrCode != 0)
                            throw new Exception($@"Erro ao criar campo - {NomeCampo} - {sErrMsg}");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    oUserField = null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion

        #region [ CreateDatabaseObjects ]

        public static void CreateDatabaseObjects()
        {
            sboApp.StatusBar.SetText("Verificando tabelas e campos de usuário...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            Dictionary<string, string> validValuesOITM = new Dictionary<string, string>();
            validValuesOITM.Add("Sim", "S");
            validValuesOITM.Add("Não", "N");
            
            CreateUserField("OITM", "CVA_Magento_Id", "Magento | Id", BoFieldTypes.db_Alpha, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("OITM", "CVA_Magento_Data", "Magento | Data", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("OITM", "CVA_Magento_Hora", "Magento | Hora", BoFieldTypes.db_Numeric, 7, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("OITM", "CVA_Magento_Integrar", "Magento | Integrar", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO, "N", validValuesOITM);
            CreateUserField("OITM", "CVA_Magento_Msg", "Magento | Msg", BoFieldTypes.db_Alpha, 254, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            Dictionary<string, string> validValuesORDR = new Dictionary<string, string>();
            validValuesORDR.Add("Integrado", "1");
            validValuesORDR.Add("Despachado", "2");
            validValuesORDR.Add("NF-e Emitida", "3");
            validValuesORDR.Add("Cancelado", "4");

            Dictionary<string, string> validValuesTipoPV = new Dictionary<string, string>();
            validValuesTipoPV.Add("PDV", "PD");
            validValuesTipoPV.Add("E-Commerce", "EC");

            Dictionary<string, string> vvORDR = new Dictionary<string, string>();
            vvORDR.Add("Não Emitir", "A");
            vvORDR.Add("PDV", "B");
            vvORDR.Add("e-commerce", "C");
            vvORDR.Add("SAP c/ Picking", "D");
            vvORDR.Add("SAP s/ Picking", "E");

            CreateUserField("ORDR", "CVA_Magento_Entity", "Magento | Entity", BoFieldTypes.db_Numeric, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("ORDR", "CVA_Magento_Id", "Magento | Id", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("ORDR", "CVA_Magento_Data", "Magento | Data", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("ORDR", "CVA_Magento_Hora", "Magento | Hora", BoFieldTypes.db_Numeric, 7, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("ORDR", "CVA_Magento_Status", "Magento | Status", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, validValuesORDR);
            CreateUserField("ORDR", "CVA_Magento_Msg", "Magento | Msg", BoFieldTypes.db_Alpha, 254, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            //CreateUserField("ORDR", "CVA_Magento_TipoPV", "Magento | Tipo PV", BoFieldTypes.db_Alpha, 2, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, validValuesTipoPV);
            CreateUserField("ORDR", "CVA_Vcto", "Magento | Vencto.", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("RDR1", "CVA_Magento_ItemId", "Magento | ItemId", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("RDR4", "pIUFDest", "Magento | Difal", BoFieldTypes.db_Float, 10, BoFldSubTypes.st_Percentage, BoYesNoEnum.tNO);
            CreateUserField("ORDR", "CVA_SourceChannel", "Magento | Origem do Pedido", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, vvORDR);

            CreateUserField("OCRD", "Magento_Id", "Magento Id", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("OCRD", "datadenascimento", "Magento Nascimento", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserTable("@CVA_CONFIG_MAG", "CVA - Config. Magento", BoUTBTableType.bott_MasterData);
            CreateUserField("@CVA_CONFIG_MAG", "Utilizacao", "Utilização", BoFieldTypes.db_Alpha, 5, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "Despesa", "Despesa", BoFieldTypes.db_Alpha, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "ApiUrl", "API URL", BoFieldTypes.db_Memo, 1000, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "ApiUsuario", "API Usuário", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "ApiSenha", "API Senha", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "ApiClientId", "API Client Id", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG", "ApiClientSecret", "API Client Secret", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            
            CreateUserTable("@CVA_CONFIG_MAG1", "CVA - Config. Magento Linhas", BoUTBTableType.bott_MasterDataLines);
            CreateUserField("@CVA_CONFIG_MAG1", "FilialSap", "Filial SAP", BoFieldTypes.db_Alpha, 4, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG1", "FilialMagento", "Filial Magento", BoFieldTypes.db_Alpha, 30, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG1", "WebsiteId", "Website Id Magento", BoFieldTypes.db_Alpha, 30, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG1", "DepositoMagento", "Depósito Magento", BoFieldTypes.db_Alpha, 30, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG1", "Deposito", "Depósito", BoFieldTypes.db_Alpha, 8, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserTable("@CVA_CONFIG_MAG2", "CVA - Config. Magento Cond.", BoUTBTableType.bott_MasterDataLines);
            CreateUserField("@CVA_CONFIG_MAG2", "CondSap", "Condição SAP", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG2", "CondMagento", "Condição Magento", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            
            CreateUserTable("@CVA_CONFIG_MAG3", "CVA - Config. Magento Formas", BoUTBTableType.bott_MasterDataLines);
            CreateUserField("@CVA_CONFIG_MAG3", "FormaSap", "Forma SAP", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG3", "Conta", "Conta Contábil", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG3", "FormaMagento", "Forma Magento", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG3", "Adiant", "Adiantamento", BoFieldTypes.db_Alpha, 8, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, validValuesOITM);

            CreateUserTable("@CVA_CONFIG_MAG4", "CVA - Config. Magento Datas", BoUTBTableType.bott_MasterDataLines);
            CreateUserField("@CVA_CONFIG_MAG4", "DataDe", "Data De", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG4", "DataAte", "Data Até", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG4", "DataVenc", "Data Vencimento", BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserTable("@CVA_CONFIG_MAG5", "CVA - Config. Magento Frete", BoUTBTableType.bott_MasterDataLines);
            CreateUserField("@CVA_CONFIG_MAG5", "TipoPV", "Tipo PV", BoFieldTypes.db_Alpha, 20, BoFldSubTypes.st_None, BoYesNoEnum.tNO, "EC", validValuesTipoPV);
            CreateUserField("@CVA_CONFIG_MAG5", "FreteSap", "Frete SAP", BoFieldTypes.db_Alpha, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_CONFIG_MAG5", "FreteMagento", "Frete Magento", BoFieldTypes.db_Alpha, 254, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserDefinedObjects();

            sboApp.StatusBar.SetText("Tabelas e campos de usuário OK", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        #endregion

        private static void CreateUserDefinedObjects()
        {
            UserObjectsMD oUserObjectMD;

            if (!UserObjectExists("CVA_CONFIG_MAG"))
            {
                oUserObjectMD = (UserObjectsMD)Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
                oUserObjectMD.ObjectType = BoUDOObjType.boud_MasterData;
                oUserObjectMD.CanCancel = BoYesNoEnum.tNO;
                oUserObjectMD.CanClose = BoYesNoEnum.tNO;
                oUserObjectMD.CanCreateDefaultForm = BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = BoYesNoEnum.tNO;
                oUserObjectMD.CanFind = BoYesNoEnum.tYES;
                oUserObjectMD.CanYearTransfer = BoYesNoEnum.tNO;
                oUserObjectMD.ManageSeries = BoYesNoEnum.tNO;
                oUserObjectMD.Code = "CVA_CONFIG_MAG";
                oUserObjectMD.Name = "[CVA] Configurações Magento";
                oUserObjectMD.TableName = "CVA_CONFIG_MAG";
                oUserObjectMD.ChildTables.SetCurrentLine(0);
                oUserObjectMD.ChildTables.TableName = "CVA_CONFIG_MAG1";
                oUserObjectMD.ChildTables.Add();
                oUserObjectMD.ChildTables.SetCurrentLine(1);
                oUserObjectMD.ChildTables.TableName = "CVA_CONFIG_MAG2";
                oUserObjectMD.ChildTables.Add();
                oUserObjectMD.ChildTables.SetCurrentLine(2);
                oUserObjectMD.ChildTables.TableName = "CVA_CONFIG_MAG3";
                oUserObjectMD.ChildTables.Add();
                oUserObjectMD.ChildTables.SetCurrentLine(3);
                oUserObjectMD.ChildTables.TableName = "CVA_CONFIG_MAG4";
                oUserObjectMD.ChildTables.Add();
                oUserObjectMD.ChildTables.SetCurrentLine(4);
                oUserObjectMD.ChildTables.TableName = "CVA_CONFIG_MAG5";
                RetCode = oUserObjectMD.Add();
            }
        }

        public static void CreateUserTable(string tableName, string tableDescription, BoUTBTableType tableType)
        {
            tableName = tableName.Replace("@", string.Empty);

            if (TableExists(tableName))
                return;

            sboApp.StatusBar.SetText($"Criando tabela @{tableName}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            var oUserTableMD = (UserTablesMD)oCompany.GetBusinessObject(BoObjectTypes.oUserTables);

            try
            {
                oUserTableMD.TableName = tableName;
                oUserTableMD.TableDescription = tableDescription;
                oUserTableMD.TableType = tableType;
                RetCode = oUserTableMD.Add();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ReleaseComObject(oUserTableMD);
            }
        }

        public static void CreateUserField(string tableName, string fieldName, string description, BoFieldTypes type, int fieldSize, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null, Dictionary<string, string> validValues = null, string linkedTable = null, UDFLinkedSystemObjectTypesEnum? systemObj = null)
        {
            if (UserFieldExists(tableName, fieldName))
                return;

            sboApp.StatusBar.SetText($"Criando campo {tableName}.U_{fieldName}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            var oUserFieldsMD = (UserFieldsMD)oCompany.GetBusinessObject(BoObjectTypes.oUserFields);

            try
            {
                oUserFieldsMD.TableName = tableName;
                oUserFieldsMD.Name = fieldName;
                oUserFieldsMD.Description = description;
                oUserFieldsMD.Type = type;
                oUserFieldsMD.SubType = subType;
                oUserFieldsMD.Mandatory = mandatory;

                if (!string.IsNullOrEmpty(defaultValue))
                    oUserFieldsMD.DefaultValue = defaultValue;
                if (!string.IsNullOrEmpty(linkedTable))
                    oUserFieldsMD.LinkedTable = linkedTable;
                if (systemObj.HasValue)
                    oUserFieldsMD.LinkedSystemObject = systemObj.Value;

                if (validValues != null)
                {
                    foreach (var validValue in validValues)
                    {
                        oUserFieldsMD.ValidValues.Description = validValue.Key;
                        oUserFieldsMD.ValidValues.Value = validValue.Value;
                        oUserFieldsMD.ValidValues.Add();
                    }
                }

                if (fieldSize > 0)
                    oUserFieldsMD.EditSize = fieldSize;

                RetCode = oUserFieldsMD.Add();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ReleaseComObject(oUserFieldsMD);
            }
        }

        public static int GetNextUserTableCode(string userTable)
        {
            int newCode = 0;
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string query = $"SELECT MAX(Code) AS NewCode FROM [{userTable}] WITH(NOLOCK)";
            recordSet.DoQuery(query);

            if (recordSet.RecordCount > 0)
            {
                string result = Convert.ToString(recordSet.Fields.Item("NewCode").Value);

                if (!string.IsNullOrEmpty(result))
                    newCode = Convert.ToInt32(recordSet.Fields.Item("NewCode").Value);
            }

            ReleaseComObject(recordSet);
            return newCode + 1;
        }

        public static bool UserObjectExists(string objCode)
        {
            try
            {
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT 1 FROM \"OUDO\" T0 WHERE T0.\"Code\" = '{objCode}'";
                recordSet.DoQuery(query);
                bool found = recordSet.RecordCount > 0;
                ReleaseComObject(recordSet);
                return found;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool TableExists(string tableName)
        {
            try
            {
                tableName = tableName.Replace("@", string.Empty);
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT 1 FROM \"OUTB\" T0 WHERE T0.\"TableName\" = '{tableName}'";
                recordSet.DoQuery(query);
                bool found = recordSet.RecordCount > 0;
                ReleaseComObject(recordSet);
                return found;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool UserFieldExists(string tableName, string fieldName)
        {
            try
            {
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sQuery = string.Format(@"select COUNT(*)  
                                                from CUFD 
                                                where ""TableID"" = '{0}' 
                                                  and ""AliasID"" = '{1}'", tableName, fieldName);
                //string query = $"SELECT 1 FROM CUFD T0 WHERE T0.TableID = '{tableName}' AND T0.AliasID = '{fieldName}'";
                recordSet.DoQuery(sQuery);
                bool found = (Convert.ToInt32(recordSet.Fields.Item(0).Value) > 0 ? true : false);
                ReleaseComObject(recordSet);
                return found;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void EmptyUserTable(string tableName)
        {
            try
            {
                if (tableName != null && !tableName.StartsWith("@"))
                    tableName = "@" + tableName;

                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"DELETE FROM [{tableName}]";
                recordSet.DoQuery(query);
                ReleaseComObject(recordSet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void ReleaseComObject(object obj)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
