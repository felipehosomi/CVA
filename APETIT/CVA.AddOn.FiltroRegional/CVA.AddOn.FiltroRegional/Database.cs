using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SAPbobsCOM;
using System.Runtime.InteropServices;

namespace CVA.AddOn.FiltroRegional
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


            //CreateUserTable("CVA_Indicadores", "[CVA] Indicadores PV", BoUTBTableType.bott_NoObject);
            //CreateUserField("ORDR", "CVA_Indicador", "Indicador", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_INDICADORES");

            //CreateUserTable("CVA_SEQUENCIAIND_PV", "[CVA] Sequência de indicadores", BoUTBTableType.bott_NoObject);
            //CreateUserField("@CVA_SEQUENCIAIND_PV", "Origem", "Indicador Origem", BoFieldTypes.db_Alpha, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_INDICADORES");
            //CreateUserField("@CVA_SEQUENCIAIND_PV", "Destino", "Indicador Destino", BoFieldTypes.db_Alpha, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_INDICADORES");
            //CreateUserField("@CVA_SEQUENCIAIND_PV", "ObrigaMensagem", "Obriga Mensagem", BoFieldTypes.db_Alpha, 1, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserTable("CVA_Regionais", "[CVA] - Regionais", BoUTBTableType.bott_NoObject);
            
            CreateUserField("OCRD", "CVA_IdRegional", "Regional", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_REGIONAIS");
            CreateUserField("OBPL", "CVA_IdRegional", "Regional", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_REGIONAIS");

            CreateUserField("OPQT", "CVA_ObsRevisao", "Obs. de revisão", BoFieldTypes.db_Memo, 500, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("PQT1", "CVA_CodigoUM", "Embalagem Fornecimento", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("PQT1", "CVA_UMItem", "UM do Item", BoFieldTypes.db_Alpha, 100, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
            CreateUserField("PQT1", "CVA_LeadTime", "LeadTime", BoFieldTypes.db_Numeric, 10, BoFldSubTypes.st_None, BoYesNoEnum.tNO);

            CreateUserField("OPQT", "CVA_Status", "Status oferta", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, "0", null, null);
            AddValidValueToUserField("OPQT", "U_CVA_Status", "0", "Aguardando Preenchimento", true);
            AddValidValueToUserField("OPQT", "U_CVA_Status", "1", "Enviada para Avaliação");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "2", "Solicitado revisão pelo Comprador");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "4", "Aprovadas");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "5", "Reprovadas");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "6", "Recusadas");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "7", "Aprovado Parcialmente");
            AddValidValueToUserField("OPQT", "U_CVA_Status", "9", "Canceladas");

            CreateUserTable("CVA_CAD_REGIONAIS", "CVA: Regionais de compras", BoUTBTableType.bott_NoObject);
            CreateUserField("@CVA_CAD_REGIONAIS", "Regional", "Sinaliza regional", BoFieldTypes.db_Alpha, 1, BoFldSubTypes.st_None);
            CreateUserField("@CVA_CAD_REGIONAIS", "Estado", "Sinaliza estado", BoFieldTypes.db_Alpha, 1, BoFldSubTypes.st_None);
            CreateUserField("@CVA_CAD_REGIONAIS", "Regiao", "Sinaliza região", BoFieldTypes.db_Alpha, 1, BoFldSubTypes.st_None);

            CreateUserTable("CVA_REGIONAIS_PN", "CVA: Regionais/PN", BoUTBTableType.bott_NoObject);
            CreateUserField("@CVA_REGIONAIS_PN", "Regional", "Codigo da Regional", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None);
            CreateUserField("@CVA_REGIONAIS_PN", "CardCode", "Codigo do PN", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None);

            //CreateUserTable("CVA_FAMILIA_ITEM", "CVA: Familia Item", BoUTBTableType.bott_NoObject);
            //CreateUserField("OITM", "CVA_Familia_Item", "Familia do Item", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_FAMILIA_ITEM");

            //CreateUserTable("CVA_SFAMILIAITEM", "CVA Sfamilia Item", BoUTBTableType.bott_NoObject);
            //CreateUserField("OITM", "CVA_SfamiliaItem", "Subfamilia Item", BoFieldTypes.db_Alpha, 50, BoFldSubTypes.st_None, BoYesNoEnum.tNO, null, null, "CVA_SFAMILIAITEM");

            sboApp.StatusBar.SetText("Tabelas e campos de usuário OK", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        public static void CreateUserTable(string tableName, string tableDescription, BoUTBTableType tableType)
        {
            tableName = tableName.Replace("@", string.Empty);

            if (ExistUserTable (oCompany, tableName))
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

        public static void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description)
        {
            // se não foi passado o parâmetro de "É Valor Padrão" trata como não
            // chamando a função que realmente insere o valor como "false" a variável IsDefault
            AddValidValueToUserField(TableName.ToUpper(), FieldName, Value, Description, false);
        }

        public static void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description, bool IsDefault)
        {
            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));
            try
            {
                string sql = @" SELECT UFD1.""IndexID"" FROM CUFD
                            INNER JOIN UFD1 
                                ON CUFD.""TableID"" = UFD1.""TableID"" 
                               AND CUFD.""FieldID"" = UFD1.""FieldID""
                         WHERE CUFD.""TableID"" = '{0}' 
                         AND CUFD.""AliasID""= '{1}' 
                         AND UFD1.""FldValue"" = '{2}'";
                sql = String.Format(sql, TableName, FieldName.Replace("U_", ""), Value);

                string IndexId = QueryForValue(sql);

                if (IndexId != null)
                {
                    return;
                }

                sql = @" SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{0}' AND ""AliasID"" = '{1}' ";
                sql = String.Format(sql, TableName, FieldName.Replace("U_", ""));
                string FieldId = QueryForValue(sql);

                if (!oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldId)))
                {
                    throw new Exception("Campo não encontrado!");
                }

                if (!String.IsNullOrEmpty(oUserFieldsMD.ValidValues.Value))
                {
                    oUserFieldsMD.ValidValues.Add();
                }

                oUserFieldsMD.ValidValues.Value = Value;
                oUserFieldsMD.ValidValues.Description = Description;

                if (IsDefault)
                    oUserFieldsMD.DefaultValue = Value;

                oUserFieldsMD.Update();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
            }
        }

        public static string QueryForValue(string Sql)
        {
            Recordset oRecordset = (Recordset)(oCompany.GetBusinessObject(BoObjectTypes.BoRecordset));
            string Retorno = null;

            try
            {
                oRecordset.DoQuery(Sql);

                // Executa e, caso exista ao menos um registro, devolve o mesmo.
                // retorna sempre o primeiro campo da consulta (SEMPRE)
                if (!oRecordset.EoF)
                {
                    Retorno = oRecordset.Fields.Item(0).Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(oRecordset);
                oRecordset = null;
            }

            return Retorno;
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


        public static string GetRegionalID(string code,string tableName, string fieldName)
        {
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string query = $@"SELECT TOP 1 ""U_CVA_IdRegional""  FROM ""{tableName}"" WHERE ""{fieldName}"" = " + code;
            recordSet.DoQuery(query);
            if(recordSet.RecordCount > 0)
            {
                return (string)recordSet.Fields.Item("U_CVA_IdRegional").Value;
            }else
            {
                return "";
            }
        }
    }
}
