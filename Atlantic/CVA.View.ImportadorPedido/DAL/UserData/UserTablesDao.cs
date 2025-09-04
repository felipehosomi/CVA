using System;
using SAPbobsCOM;
using DAL.Connection;

namespace DAL.UserData
{
    public class UserTablesDao
    {
        public static bool Exists(string name)
        {
            var table = (UserTablesMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            var ret = table.GetByKey(name);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);

            return ret;
        }

        public static void Create(string name, string description, BoUTBTableType type = BoUTBTableType.bott_NoObject)
        {
            var table = (UserTablesMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            table.TableName = name;
            table.TableDescription = description;
            table.TableType = type;

            if (table.Add() != 0)
            {
                int errCode;
                string errMsg;
                ConnectionDao.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(table);

                throw new Exception($"Erro ao criar tabela de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);
        }
    }
}
