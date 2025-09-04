using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class UserFields
    {
        public static bool Exists(string tableName, string fieldName)
        {
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
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

                field.DefaultValue = defaultValue;
            }

            if (field.Add() != 0)
            {
                int errCode;
                string errMsg;
                B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(field);

                throw new Exception($"Erro ao criar campo de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
        }
    }
}
