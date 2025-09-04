using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class UserTables
    {
        public static bool Exists(string name)
        {
            UserTablesMD table = (UserTablesMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            var ret = table.GetByKey(name);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);

            return ret;
        }

        public static void Create(string name, string description, BoUTBTableType type = BoUTBTableType.bott_NoObject)
        {
            UserTablesMD table = (UserTablesMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            table.TableName = name;
            table.TableDescription = description;
            table.TableType = type;

            if (table.Add() != 0)
            {
                int errCode;
                string errMsg;
                B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(table);

                throw new Exception($"Erro ao criar tabela de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);
        }
    }
}
