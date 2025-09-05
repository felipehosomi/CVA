using System;
using System.Collections.Generic;
using SAPbobsCOM;
using System.IO;
using System.Xml.Linq;

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

        //public static void Create(string fileName)
        //{
        //    var xml = XDocument.Load(fileName);

        //    var strXml = xml.ToString();

        //    strXml = strXml.Replace(" xmlns=\"\"", "");
        //    File.WriteAllText(fileName, strXml);

        //    xml = XDocument.Load(fileName);

        //    foreach (var bo in xml.Descendants("BO"))
        //    {
        //        foreach (var userField in bo.Descendants("UserFieldsMD"))
        //        {
        //            var name = "";
        //            var type = "";
        //            var size = "";
        //            var description = "";
        //            var subtype = "";
        //            var linkedTable = "";
        //            var defaultValue = "";
        //            var tableName = "";
        //            var fieldId = "";
        //            var editSize = "";
        //            var mandatory = "";
        //            var vv_list = new Dictionary<object, object>();

        //            foreach (var item in userField.Descendants("row"))
        //            {
        //                name = item.Element("Name").Value;
        //                type = item.Element("Type").Value;
        //                size = item.Element("Size").Value;
        //                description = item.Element("Description").Value;
        //                subtype = item.Element("SubType").Value;
        //                linkedTable = item.Element("LinkedTable").Value;
        //                defaultValue = item.Element("DefaultValue").Value;
        //                tableName = item.Element("TableName").Value;
        //                fieldId = item.Element("FieldID").Value;
        //                editSize = item.Element("EditSize").Value;
        //                mandatory = item.Element("Mandatory").Value;
        //            }

        //            foreach (var vv in userField.Descendants("ValidValuesMD"))
        //            {
        //                foreach (var item in vv.Descendants("row"))
        //                {
        //                    var vv_value = item.Element("Value").Value;
        //                    var vv_description = item.Element("Description").Value;
        //                    vv_list.Add(vv_value, vv_description);
        //                }
        //            }

        //            if (!Exists(tableName, name))
        //                Create(tableName, name, description, int.Parse(size), (BoFieldTypes)Enum.Parse(typeof(BoFieldTypes), type), (BoFldSubTypes)Enum.Parse(typeof(BoFldSubTypes), subtype), (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), mandatory), defaultValue, vv_list, linkedTable);
        //        }
        //    }
        //}
    }
}
