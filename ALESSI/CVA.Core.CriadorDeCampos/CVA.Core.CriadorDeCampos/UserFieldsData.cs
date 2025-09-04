using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.Core.CriadorDeCampos
{
    internal static class UserFieldsData
    {
        private static Company oCompany;

        internal static void Create(Company company)
        {
            oCompany = company;
            var uri = $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\UserFields.xml";
            Console.WriteLine("**** Arquivo com os campos de usuário para criação: " + uri);
            var fields = XmlHelper.StreamUserFields(uri);
            Console.WriteLine("**** Total de campos para criação: " + fields.Count());
            foreach (var item in fields)
            {
                if (!UserFieldExists(item.TableName, item.Name))
                {
                    var size = Int32.Parse(item.Size);
                    var type = (BoFieldTypes)Enum.Parse(typeof(BoFieldTypes), item.Type);
                    var subType = (BoFldSubTypes)Enum.Parse(typeof(BoFldSubTypes), item.SubType);
                    var mandatory = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), item.Mandatory);
                    var dict = new Dictionary<object, object>();

                    foreach (var vv in item.ValidValues)
                    {
                        dict.Add(vv.Value, vv.Description);
                    }

                    CreateUserField(item.TableName, item.Name, item.Description, size, type, subType, mandatory, item.DefaultValue, dict, item.LinkedTable);
                    Console.WriteLine("**** Campo [" + item.Name + "] criado na tabela [" + item.TableName + "]");
                }
                else
                {
                    Console.WriteLine("**** Campo [" + item.Name + "] já existe na tabela [" + item.TableName + "]");
                }
            }

        }

        private static bool UserFieldExists(string tableName, string fieldName)
        {
            Recordset oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
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

        private static void CreateUserField(string tableName, string fieldName, string description, int size,
            BoFieldTypes type = BoFieldTypes.db_Alpha, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null,
            Dictionary<object, object> validValues = null, string linkedTable = null)
        {
            UserFieldsMD field = (UserFieldsMD)oCompany.GetBusinessObject(BoObjectTypes.oUserFields);

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
                    if (!string.IsNullOrEmpty(item.Key.ToString()) && !string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        field.ValidValues.Add();
                        field.ValidValues.Description = item.Value.ToString();
                        field.ValidValues.Value = item.Key.ToString(); 
                    }
                }

                field.DefaultValue = defaultValue;
            }

            if (field.Add() != 0)
            {
                int errCode;
                string errMsg;
                oCompany.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(field);

                throw new Exception($"Erro ao criar campo de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
        }
    }
}
