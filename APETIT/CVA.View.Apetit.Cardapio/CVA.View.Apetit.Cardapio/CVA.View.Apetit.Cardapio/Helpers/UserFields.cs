using System;
using System.Collections.Generic;
using SAPbobsCOM;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public class UserFields
    {
        public StringBuilder Log { get; set; }
        int CodErro;
        string MsgErro;

        public UserFields()
        {
            Log = new StringBuilder();
        }

        public bool Exists(string tableName, string fieldName)
        {
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $@"SELECT top 1 1 as {"result".Aspas()} 
                           FROM {"SYS".Aspas()}.{"TABLE_COLUMNS".Aspas()}
                           WHERE {"TABLE_NAME".Aspas()} = '{tableName}'
                             AND {"COLUMN_NAME".Aspas()} = 'U_{fieldName}'
                             AND {"SCHEMA_NAME".Aspas()} = '{B1Connection.Instance.Company.CompanyDB}'
                        ;";

            oRecordset.DoQuery(query);

            var lRet = (int)oRecordset.Fields.Item("result").Value;
            var ret = lRet == 1;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return ret;
        }

        public void Create(string tableName, string fieldName, string description, int size,
            BoFieldTypes type = BoFieldTypes.db_Alpha, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null,
            Dictionary<object, object> validValues = null, string linkedTable = null)
        {
            UserFieldsMD field = (UserFieldsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserFields);

            field.TableName = tableName;
            field.Name = fieldName;
            field.Type = type;
            field.Size = size;
            field.EditSize = size;
            field.Description = description;
            field.SubType = subType;
            field.Mandatory = mandatory;
            field.LinkedTable = linkedTable;

            if (validValues != null)
            {
                foreach (var item in validValues)
                {
                    field.ValidValues.Add();
                    field.ValidValues.Description = item.Value.ToString();
                    field.ValidValues.Value = item.Key.ToString();
                }
            }

            if (defaultValue != null)
                field.DefaultValue = defaultValue;

            if (field.Add() != 0)
            {
                int errCode;
                string errMsg;
                B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
                //throw new Exception($"Erro ao criar campo de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
        }

        public void CreateIfNotExist(string tableName, string fieldName, string description, int size,
            BoFieldTypes type = BoFieldTypes.db_Alpha, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null,
            Dictionary<object, object> validValues = null, string linkedTable = null)
        {
            if (!Exists(tableName, fieldName))
                Create(tableName, fieldName, description, size, type, subType, mandatory, defaultValue, validValues, linkedTable);
        }

        public void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog = false, bool CanYearTransfer = false)
        {
            CreateUserObject(ObjectName, ObjectDesc, TableName, ObjectType, CanLog, CanYearTransfer, false, false, false, true, true, 0, 0, null);
        }

        public void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog, bool CanYearTransfer, bool CanCancel, bool CanClose, bool CanCreateDefaultForm, bool CanDelete, bool CanFind, int FatherMenuId, int menuPosition, string srfFile = "", GenericModel findColumns = null)
        {
            // se não preenchido um table name separado, usa o mesmo do objeto
            if (String.IsNullOrEmpty(TableName))
                TableName = ObjectName;

            UserObjectsMD UserObjectsMD = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

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

            ValidateAction();

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            //GC.Collect();
        }

        public void AddChildTableToUserObject(string ObjectName, string ChildTableName)
        {
            UserObjectsMD UserObjectsMD = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

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
                ValidateAction();
            }

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            //GC.Collect();
        }


        public BoYesNoEnum GetSapBoolean(bool Variavel)
        {
            if (Variavel)
                return BoYesNoEnum.tYES;
            else
                return BoYesNoEnum.tNO;

        }

        public void ValidateAction()
        {
            if (CodErro != 0)
            {
                B1Connection.Instance.Company.GetLastError(out CodErro, out MsgErro);
                Log.AppendFormat("FALHA ({0}){1}", MsgErro, Environment.NewLine);
            }
            else
            {
                MsgErro = "";
            }
        }
    }

    /// <summary>
    /// Model genérico com os campos criados a partir de um SELECT
    /// </summary>
    public class GenericModel
    {
        /// <summary>
        /// Campos e valores
        /// </summary>
        public Dictionary<String, object> Fields { get; set; }

        /// <summary>
        /// Retorna valor do campo
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns></returns>
        public object GetFieldValue(string fieldName)
        {
            return this.Fields[fieldName];
        }

        /// <summary>
        /// Retorna tipo do campo
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns></returns>
        public Type GetFieldType(string fieldName)
        {
            return this.Fields[fieldName].GetType();
        }
    }
}
