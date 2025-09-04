using System;
using System.Collections.Generic;
using SAPbobsCOM;
using DAL.Connection;

namespace DAL.UserData
{
    public class UserFieldsDao
    {
        public static bool Exists(string tableName, string fieldName)
        {
            var oRecordset = (Recordset)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = "SELECT 1 [result] FROM SYS.OBJECTS so " +
                        "INNER JOIN SYS.COLUMNS sc ON sc.object_id = so.object_id " +
                        "WHERE so.schema_id = 1 " +
                        $"AND so.name = '{tableName}' AND sc.name = 'U_{fieldName}'";

            oRecordset.DoQuery(query);

            var lRet = (int)oRecordset.Fields.Item("result").Value;
            var ret = lRet == 1;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return ret;
        }

        public static void Create(string tableName, string fieldName, string description, int size,
            BoFieldTypes type = BoFieldTypes.db_Alpha, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null,
            Dictionary<object, object> validValues = null, string linkedTable = null)
        {
            var field = (UserFieldsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserFields);

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

                field.DefaultValue = defaultValue;
            }

            if (field.Add() != 0)
            {
                int errCode;
                string errMsg;
                ConnectionDao.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(field);

                throw new Exception($"Erro ao criar campo de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
        }
    }
}
