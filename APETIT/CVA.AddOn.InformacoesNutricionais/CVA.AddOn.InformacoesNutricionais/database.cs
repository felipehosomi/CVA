using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SAPbobsCOM;


namespace CVA.AddOn.InformacoesNutricionais
{
    class Database
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

        public static void CreateDatabaseObjects()
        {
            sboApp.StatusBar.SetText("Verificando tabelas e campos de usuário...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            CreateUserTable("CVA_GrupoNutriente", "[CVA] - Grupos de Nutrientes", BoUTBTableType.bott_NoObject);

            CreateUserTable("CVA_Nutriente", "[CVA] - Nutrientes", BoUTBTableType.bott_NoObject);
            CreateUserField("@CVA_NUTRIENTE", "Sigla", "Sigla", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_NUTRIENTE", "UN", "Unidade", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_NUTRIENTE", "GrupoID", "Grupo de Nutrientes", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO,null,null, "CVA_GRUPONUTRIENTE");

            CreateUserTable("CVA_NutriProduto", "[CVA] - Nutrientes por Produto", BoUTBTableType.bott_NoObjectAutoIncrement);
            CreateUserField("@CVA_NUTRIPRODUTO", "Codigo", "Item", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("@CVA_NUTRIPRODUTO", "NutrienteID", "Nutriente", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_NUTRIENTE");
            CreateUserField("@CVA_NUTRIPRODUTO", "Quantidade", "Quantidade", BoFieldTypes.db_Float, 50, BoFldSubTypes.st_Quantity, BoYesNoEnum.tNO);
            CreateUserField("@CVA_NUTRIPRODUTO", "Referencia", "Referência Bibliográfica", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            sboApp.StatusBar.SetText("Tabelas e campos de usuário OK", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        public static void CreateUserTable(string tableName, string tableDescription, BoUTBTableType tableType)
        {
            tableName = tableName.Replace("@", string.Empty);

            if (ExistUserTable(oCompany, tableName))
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
            fieldName = fieldName.Trim();
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

        public static bool ExistUserTable(SAPbobsCOM.Company oCompany, string UserTableName)
        {
            SAPbobsCOM.UserTablesMD oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            GC.Collect();

            oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            // Remove a arroba do usertable Name
            UserTableName = UserTableName.Replace("@", "");

            bool bUpdate = oUserTableMD.GetByKey(UserTableName);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            GC.Collect();

            return bUpdate;

        }

        public static bool UserFieldExists(string tableName, string fieldName)
        {
            try
            {
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $@"SELECT 1 FROM CUFD T0 WHERE T0.""TableID"" = '{tableName}' AND T0.""AliasID"" = '{fieldName}'";
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


        public static void ReleaseComObject(object obj)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


        public static bool CampoExiste(string codigo, string nutrienteID)
        {
            try
            {
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $@"SELECT 1 FROM ""@CVA_NUTRIPRODUTO"" T0 WHERE T0.""U_Codigo"" = '{codigo}' AND T0.""U_NutrienteID"" = '{nutrienteID}'";
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

        public static void UpdateTabelaNutricional(string codigo, string nutrienteID, string quantidade, string referencia)
        {
            try
            {
                if(quantidade != "" || referencia != "")
                {
                    decimal i = 0;
                    quantidade = quantidade != "" && decimal.TryParse(quantidade,out i) ? quantidade : "0";
                    bool update = CampoExiste(codigo, nutrienteID);
                    var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    string query = "";
                    if (update)
                    {
                        query = $@"Update ""@CVA_NUTRIPRODUTO"" T0 Set ""U_Quantidade"" = '{quantidade}', ""U_Referencia"" = '{referencia}'  WHERE T0.""U_Codigo"" = '{codigo}' AND T0.""U_NutrienteID"" = '{nutrienteID}'";
                    }
                    else
                    {
                        query = $@"Insert Into ""@CVA_NUTRIPRODUTO"" Values ((Select Top 1 (""Code"") + 1 From ""@CVA_NUTRIPRODUTO"" Order By ""Code"" Desc),'','{codigo}','{nutrienteID}','{quantidade}','{referencia}')";
                    }

                    recordSet.DoQuery(query);
                    bool found = recordSet.RecordCount > 0;
                    ReleaseComObject(recordSet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
