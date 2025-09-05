using CVA.ImportacaoLCM.DAO;
using CVA.ImportacaoLCM.Model;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Controller
{ /// <summary>
    /// ATENÇÂO - Cuidado ao efetuar alterações:
    /// Não é possível iniciar um Recordset (ou algum outro objeto) junto com os objetos de criação de campos/tabelas
    /// </summary>
    public class UserObjectController
    {
        public StringBuilder Log { get; set; }
        int CodErro;
        string MsgErro;
        //private GenericModel FindColumns;

        public UserObjectController()
        {
            Log = new StringBuilder();
        }

        public void CreateUserTable(string UserTableName, string UserTableDesc, BoUTBTableType UserTableType)
        {
            UserTableName = UserTableName.Replace("@", "");
            UserTablesMD oUserTableMD = (UserTablesMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserTables);

            try
            {
                bool update = oUserTableMD.GetByKey(UserTableName);
                if (update)
                {
                    return;
                }

                oUserTableMD.TableName = UserTableName;
                oUserTableMD.TableDescription = UserTableDesc;
                oUserTableMD.TableType = UserTableType;

                CodErro = oUserTableMD.Add();
                this.ValidateAction();
            }
            catch (Exception ex)
            {
                Log.AppendFormat("Erro geral ao criar tabela: " + ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(oUserTableMD);
                oUserTableMD = null;
                GC.Collect();
            }
        }

        public void RemoveUserTable(string UserTableName)
        {
            UserTablesMD oUserTableMD = (UserTablesMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserTables);

            // Remove a arroba do usertable Name
            UserTableName = UserTableName.Replace("@", "");

            if (oUserTableMD.GetByKey(UserTableName))
            {
                CodErro = oUserTableMD.Remove();
                this.ValidateAction();
            }
            else
            {
                CodErro = 0;
                MsgErro = "";
            }
            Marshal.ReleaseComObject(oUserTableMD);
            oUserTableMD = null;
            GC.Collect();
        }

        public void InsertUserField(string TableName, string FieldName, string FieldDescription, BoFieldTypes oType, BoFldSubTypes oSubType, int FieldSize, bool MandatoryYN = false, string DefaultValue = "", string linkedTable = "")
        {
            if (FieldDescription.Length > 30)
            {
                FieldDescription = FieldDescription.Substring(0, 30);
            }
            string Sql = string.Empty;
            if (SBOApp.oCompany.DbServerType == BoDataServerTypes.dst_HANADB)
                Sql = @"SELECT ""FieldId"" FROM ""CUFD"" WHERE ""TableID"" = '{0}' AND ""AliasID"" = '{1}' ";
            else
                Sql = " SELECT FieldId FROM CUFD WHERE TableID = '{0}' AND AliasID = '{1}' ";
            Sql = String.Format(Sql, TableName, FieldName);
            string FieldId = QueryForValue(Sql);

            if (FieldId != null)
            {
                return;
            }

            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));
            oUserFieldsMD.TableName = TableName;
            oUserFieldsMD.Name = FieldName;
            oUserFieldsMD.Description = FieldDescription;
            oUserFieldsMD.Type = oType;
            oUserFieldsMD.SubType = oSubType;
            oUserFieldsMD.Mandatory = GetSapBoolean(MandatoryYN);

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                oUserFieldsMD.DefaultValue = DefaultValue;
            }
            if (!String.IsNullOrEmpty(linkedTable))
            {
                oUserFieldsMD.LinkedTable = linkedTable;
            }

            if (FieldSize > 0)
                oUserFieldsMD.EditSize = FieldSize;
            CodErro = oUserFieldsMD.Add();
            this.ValidateAction();

            Marshal.ReleaseComObject(oUserFieldsMD);
            oUserFieldsMD = null;
            GC.Collect();
        }


        public void UpsertUserField(string TableName, string FieldName, string FieldDescription, BoFieldTypes oType, BoFldSubTypes oSubType, int FieldSize, bool MandatoryYN = false, string DefaultValue = "")
        {
            if (FieldDescription.Length > 30)
            {
                FieldDescription = FieldDescription.Substring(0, 30);
            }

            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));
            bool bUpdate;

            string Sql = " SELECT FieldId FROM CUFD WHERE TableID = '{0}' AND AliasID = '{1}' ";
            Sql = String.Format(Sql, TableName, FieldName);
            string FieldId = QueryForValue(Sql);

            if (FieldId != null)
            {
                bUpdate = oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldId));
            }
            else
                bUpdate = false;

            oUserFieldsMD.TableName = TableName;
            oUserFieldsMD.Name = FieldName;
            oUserFieldsMD.Description = FieldDescription;
            oUserFieldsMD.Type = oType;
            oUserFieldsMD.SubType = oSubType;
            oUserFieldsMD.Mandatory = GetSapBoolean(MandatoryYN);
            if (!String.IsNullOrEmpty(DefaultValue))
            {
                oUserFieldsMD.DefaultValue = DefaultValue;
            }

            if (FieldSize > 0)
                oUserFieldsMD.EditSize = FieldSize;

            if (bUpdate)
                //CodErro = oUserFieldsMD.Update();
                CodErro = 0;
            else
                CodErro = oUserFieldsMD.Add();
            this.ValidateAction();

            Marshal.ReleaseComObject(oUserFieldsMD);
            oUserFieldsMD = null;
            GC.Collect();
        }

        public void RemoveUserField(string TableName, string FieldName)
        {
            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));

            string Sql = " SELECT FieldId FROM CUFD WHERE TableID = '{0}' AND AliasID = '{1}' ";
            Sql = String.Format(Sql, TableName, FieldName);

            string FieldId = QueryForValue(Sql);

            if (FieldId != null)
            {
                if (oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldId)))
                {
                    CodErro = oUserFieldsMD.Remove();
                    this.ValidateAction();
                }
            }
            else
            {
                MsgErro = "";
                CodErro = 0;
                Log.AppendLine(" Tabela/Campo não encontrado ");
            }

            Marshal.ReleaseComObject(oUserFieldsMD);
            oUserFieldsMD = null;
            GC.Collect();
        }

        public void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description)
        {
            // se não foi passado o parâmetro de "É Valor Padrão" trata como não
            // chamando a função que realmente insere o valor como "false" a variável IsDefault
            AddValidValueToUserField(TableName, FieldName, Value, Description, false);
        }

        public void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description, bool IsDefault)
        {
            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));
            try
            {
                bool bUpdate;

                string Sql = @" SELECT FieldId FROM CUFD WHERE TableID = '{0}' AND AliasID = '{1}' ";
                Sql = String.Format(Sql, TableName, FieldName.Replace("U_", ""));
                string FieldId = QueryForValue(Sql);

                bUpdate = oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldId));

                Sql = @" SELECT COUNT(1) FROM CUFD
                            INNER JOIN UFD1 
                                ON CUFD.TableID = UFD1.TableID 
                                AND CUFD.FieldID = UFD1.FieldID
                        WHERE CUFD.TableID = '{0}' 
                        AND CUFD.AliasID = '{1}' 
                        AND LEN(UFD1.FldValue) > 0 "; //AND UFD1.FldValue = '" + Value + "'";
                Sql = String.Format(Sql, TableName, FieldName.Replace("U_", ""));

                string ContaValoresValidos = QueryForValue(Sql);

                if (Convert.ToInt32(ContaValoresValidos) > 0)
                {
                    Sql = @" SELECT UFD1.IndexID FROM CUFD
                            INNER JOIN UFD1 
                                ON CUFD.TableID = UFD1.TableID 
                                AND CUFD.FieldID = UFD1.FieldID
                         WHERE CUFD.TableID = '{0}' 
                         AND CUFD.AliasID = '{1}' 
                         AND UFD1.FldValue = '{2}'";
                    Sql = String.Format(Sql, TableName, FieldName.Replace("U_", ""), Value);

                    string IndexId = QueryForValue(Sql);

                    if (IndexId == null)
                        oUserFieldsMD.ValidValues.Add();

                    if (IndexId != null)
                        return;
                }

                oUserFieldsMD.ValidValues.Value = Value;
                oUserFieldsMD.ValidValues.Description = Description;

                if (IsDefault)
                    oUserFieldsMD.DefaultValue = Value;

                CodErro = oUserFieldsMD.Update();

                this.ValidateAction();
            }
            catch (Exception ex)
            {
                Log.AppendFormat("Erro geral ao inserir valor válido: {0}", ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();
            }
        }

        public void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog = false, bool CanYearTransfer = false)
        {
            this.CreateUserObject(ObjectName, ObjectDesc, TableName, ObjectType, CanLog, CanYearTransfer, false, false, false, true, true, 0, 0, null);
        }

        public void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog, bool CanYearTransfer, bool CanCancel, bool CanClose, bool CanCreateDefaultForm, bool CanDelete, bool CanFind, int FatherMenuId, int menuPosition, string srfFile = "", GenericModel findColumns = null)
        {
            // se não preenchido um table name separado, usa o mesmo do objeto
            if (String.IsNullOrEmpty(TableName))
                TableName = ObjectName;

            UserObjectsMD UserObjectsMD = (UserObjectsMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            TableName = TableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            UserObjectsMD.Code = ObjectName;
            UserObjectsMD.Name = ObjectDesc;
            UserObjectsMD.ObjectType = ObjectType;
            UserObjectsMD.TableName = TableName;

            //UserObjectsMD.CanArchive = GetSapBoolean(CanArchive);
            UserObjectsMD.CanCancel = GetSapBoolean(CanCancel);
            UserObjectsMD.CanClose = GetSapBoolean(CanClose);
            UserObjectsMD.CanCreateDefaultForm = GetSapBoolean(CanCreateDefaultForm);
            UserObjectsMD.CanDelete = GetSapBoolean(CanDelete);
            UserObjectsMD.CanFind = GetSapBoolean(CanFind);
            UserObjectsMD.CanLog = GetSapBoolean(CanLog);
            UserObjectsMD.CanYearTransfer = GetSapBoolean(CanYearTransfer);

            if (CanFind)
            {
                UserObjectsMD.FindColumns.ColumnAlias = "Code";
                UserObjectsMD.FindColumns.ColumnDescription = "Código";
                UserObjectsMD.FindColumns.Add();
            }

            if (CanCreateDefaultForm)
            {
                UserObjectsMD.CanCreateDefaultForm = BoYesNoEnum.tYES;
                UserObjectsMD.CanCancel = GetSapBoolean(CanCancel);
                UserObjectsMD.CanClose = GetSapBoolean(CanClose);
                UserObjectsMD.CanDelete = GetSapBoolean(CanDelete);
                UserObjectsMD.CanFind = GetSapBoolean(CanFind);
                UserObjectsMD.ExtensionName = "";
                UserObjectsMD.OverwriteDllfile = BoYesNoEnum.tYES;
                UserObjectsMD.ManageSeries = BoYesNoEnum.tYES;
                UserObjectsMD.UseUniqueFormType = BoYesNoEnum.tYES;
                UserObjectsMD.EnableEnhancedForm = BoYesNoEnum.tNO;
                UserObjectsMD.RebuildEnhancedForm = BoYesNoEnum.tNO;
                UserObjectsMD.FormSRF = srfFile;

                UserObjectsMD.FormColumns.FormColumnAlias = "Code";
                UserObjectsMD.FormColumns.FormColumnDescription = "Código";
                UserObjectsMD.FormColumns.Add();

                if (findColumns != null && findColumns.Fields != null)
                {
                    foreach (KeyValuePair<string, object> pair in findColumns.Fields)
                    {
                        UserObjectsMD.FormColumns.FormColumnAlias = pair.Key;
                        UserObjectsMD.FormColumns.FormColumnDescription = pair.Value.ToString();
                        UserObjectsMD.FormColumns.Add();
                    }
                }

                if (findColumns != null && findColumns.Fields != null)
                {
                    foreach (KeyValuePair<string, object> pair in findColumns.Fields)
                    {
                        UserObjectsMD.FindColumns.ColumnAlias = pair.Key;
                        UserObjectsMD.FindColumns.ColumnDescription = pair.Value.ToString();
                        UserObjectsMD.FindColumns.Add();
                    }
                }

                UserObjectsMD.FatherMenuID = FatherMenuId;
                UserObjectsMD.Position = menuPosition;
                UserObjectsMD.MenuItem = BoYesNoEnum.tYES;
                UserObjectsMD.MenuUID = ObjectName;
                UserObjectsMD.MenuCaption = ObjectDesc;
            }

            if (bUpdate)
            {
                //CodErro = UserObjectsMD.Update();
            }
            else
                CodErro = UserObjectsMD.Add();

            this.ValidateAction();

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            GC.Collect();
        }
        public void RegistraObj(string Name, SAPbobsCOM.BoYesNoEnum Series, SAPbobsCOM.BoYesNoEnum CanLog, SAPbobsCOM.BoYesNoEnum CanCancel, SAPbobsCOM.BoYesNoEnum CanDelete, SAPbobsCOM.BoYesNoEnum CanFind, BoYesNoEnum CanCreateDefaultForm, BoYesNoEnum EnableEnhancedForm, SAPbobsCOM.BoUDOObjType ObjectType, string[] childTable, string[] findColumns)
        {
            int i;

            try
            {
                SAPbobsCOM.UserObjectsMD oUserObjectMD;

                oUserObjectMD = (SAPbobsCOM.UserObjectsMD)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);

                oUserObjectMD.Code = Name;
                oUserObjectMD.Name = Name;
                oUserObjectMD.ObjectType = ObjectType;
                oUserObjectMD.TableName = Name;
                oUserObjectMD.CanFind = CanFind;
                oUserObjectMD.CanDelete = CanDelete;
                oUserObjectMD.CanCancel = CanCancel;
                oUserObjectMD.CanLog = CanLog;
                oUserObjectMD.ManageSeries = Series;
                oUserObjectMD.CanCreateDefaultForm = CanCreateDefaultForm;
                oUserObjectMD.EnableEnhancedForm = EnableEnhancedForm;

                if (CanCreateDefaultForm == SAPbobsCOM.BoYesNoEnum.tYES)
                {
                    for (i = 0; i < findColumns.Length; i++)
                    {
                        oUserObjectMD.FormColumns.Add();
                        oUserObjectMD.FormColumns.SonNumber = 0;
                        oUserObjectMD.FormColumns.FormColumnAlias = findColumns[i].ToString();
                        oUserObjectMD.FormColumns.FormColumnDescription = findColumns[i].ToString();
                        oUserObjectMD.FormColumns.Editable = BoYesNoEnum.tYES;
                    }
                }

                if (CanFind == SAPbobsCOM.BoYesNoEnum.tYES)
                {
                    for (i = 0; i < findColumns.Length; i++)
                    {
                        oUserObjectMD.FindColumns.Add();
                        oUserObjectMD.FindColumns.ColumnAlias = findColumns[i].ToString();
                    }
                }

                if (childTable != null)
                {
                    for (i = 0; i < childTable.Length; i++)
                    {
                        oUserObjectMD.ChildTables.Add();
                        oUserObjectMD.ChildTables.TableName = childTable[i].ToString();
                    }
                }

                int iRetCode = oUserObjectMD.Add();

                if (iRetCode != 0)
                {
                    string sErrMsg;
                    int iErrCode;

                    SBOApp.oCompany.GetLastError(out iErrCode, out sErrMsg);
                    //ShowErrorMessageInStatusBar("Erro de registro " + Name + ". Retorno: " + sErrMsg);
                }
                else
                {
                    //ShowSuccessMessageInStatusBar("Registro " + Name + " realizado com sucesso.");
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                oUserObjectMD = null;
                GC.Collect();
            }
            catch (Exception ex)
            {

            }
        }
        public void RemoveUserObject(string ObjectName)
        {
            UserObjectsMD UserObjectsMD = (UserObjectsMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            CodErro = 0;
            if (bUpdate)
                CodErro = UserObjectsMD.Remove();
            this.ValidateAction();

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            GC.Collect();
        }

        public void AddChildTableToUserObject(string ObjectName, string ChildTableName)
        {
            UserObjectsMD UserObjectsMD = (UserObjectsMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            ChildTableName = ChildTableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            bool JaAdicionada = false;
            for (int i = 0; i < UserObjectsMD.ChildTables.Count; i++)
            {
                UserObjectsMD.ChildTables.SetCurrentLine(i);
                if (ChildTableName == UserObjectsMD.ChildTables.TableName)
                {
                    JaAdicionada = true;
                    break;
                }
            }

            if (!JaAdicionada)
            {
                UserObjectsMD.ChildTables.Add();
                UserObjectsMD.ChildTables.TableName = ChildTableName;

                CodErro = UserObjectsMD.Update();
                this.ValidateAction();
            }

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            GC.Collect();
        }


        public static string QueryForValue(string Sql)
        {
            Recordset oRecordset = (Recordset)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset));
            string Retorno = null;
            try
            {
                Sql = SBOApp.TranslateToHana(Sql);
                oRecordset.DoQuery(Sql);

                // Executa e, caso exista ao menos um registro, devolve o mesmo.
                // retorna sempre o primeiro campo da consulta (SEMPRE)
                if (!oRecordset.EoF)
                {
                    Retorno = oRecordset.Fields.Item(0).Value.ToString();
                }

            }
            catch
            {

            }
            finally
            {
                Marshal.ReleaseComObject(oRecordset);
                oRecordset = null;
                GC.Collect();

            }

            return Retorno;
        }


        public string CreateUserKey(string KeyName, string TableName, string Fields, bool isUnique)
        {
            UserKeysMD oUserKeysMD = (UserKeysMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserKeys);

            oUserKeysMD.TableName = TableName;
            oUserKeysMD.KeyName = KeyName;

            string[] arrAux = Fields.Split(Convert.ToChar(","));
            for (int i = 0; i < arrAux.Length; i++)
            {
                if (i > 0)
                    oUserKeysMD.Elements.Add();

                oUserKeysMD.Elements.ColumnAlias = arrAux[i].Trim();

            }

            oUserKeysMD.Unique = GetSapBoolean(isUnique);

            string Retorno = "";

            CodErro = oUserKeysMD.Add();
            this.ValidateAction();

            Marshal.ReleaseComObject(oUserKeysMD);
            oUserKeysMD = null;
            GC.Collect();

            return Retorno;
        }

        public void ValidateAction()
        {
            if (CodErro != 0)
            {
                SBOApp.oCompany.GetLastError(out CodErro, out MsgErro);
                Log.AppendFormat("FALHA ({0}){1}", MsgErro, Environment.NewLine);
            }
            else
            {
                MsgErro = "";
            }
        }

        public void MakeFieldsSearchable(string tableName)
        {
            UserObjectsMD userObjectsMD = (UserObjectsMD)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            Dictionary<string, string> fields = this.GetTableFields(tableName);

            tableName = tableName.Replace("@", "");
            if (userObjectsMD.GetByKey(tableName))
            {
                userObjectsMD.CanFind = BoYesNoEnum.tYES;
                bool hasNewColumn = false;
                foreach (var item in fields)
                {
                    bool found = false;
                    for (int i = 0; i < userObjectsMD.FindColumns.Count; i++)
                    {
                        userObjectsMD.FindColumns.SetCurrentLine(i);
                        if (userObjectsMD.FindColumns.ColumnAlias == item.Key)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        hasNewColumn = true;
                        userObjectsMD.FindColumns.ColumnAlias = item.Key;
                        userObjectsMD.FindColumns.ColumnDescription = item.Value;
                        userObjectsMD.FindColumns.Add();
                    }
                }

                if (hasNewColumn)
                {
                    CodErro = userObjectsMD.Update();
                }

                this.ValidateAction();
            }
            Marshal.ReleaseComObject(userObjectsMD);
            userObjectsMD = null;

        }

        public Dictionary<string, string> GetTableFields(string tableName)
        {
            Recordset rs = (Recordset)(SBOApp.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset));
            string sql = "SELECT * FROM CUFD WHERE TableID = '{0}'";

            Dictionary<string, string> fields = new Dictionary<string, string>();

            rs.DoQuery(String.Format(sql, tableName));
            while (!rs.EoF)
            {
                fields.Add("U_" + rs.Fields.Item("AliasID").Value.ToString(), rs.Fields.Item("Descr").Value.ToString());
                rs.MoveNext();
            }

            Marshal.ReleaseComObject(rs);
            rs = null;
            GC.Collect();
            return fields;
        }

        public static BoYesNoEnum GetSapBoolean(bool Variavel)
        {
            if (Variavel)
                return BoYesNoEnum.tYES;
            else
                return BoYesNoEnum.tNO;

        }
    }
}
