using System;
using System.Collections.Generic;
using System.Text;
using SAPbobsCOM;

namespace EnvioNfLote.Helpers
{
    public class Database
    {
        public static void Verify()
        {
            var oProgress = B1Connection.Instance.Application.StatusBar.CreateProgressBar("Verificando metadados...", 12, true);

            try
            {
                if (!UserTableExist("CVA_EMLCONF"))
                    CreateUsertable("CVA_EMLCONF", "CVA: Configurações de E-mail");

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Emp"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Emp", "Empresa", 11, BoFieldTypes.db_Numeric);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Eml"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Eml", "E-mail", 254);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Usr"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Usr", "Usuário", 254);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Pas"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Pas", "Senha", 254);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Smtp"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Smtp", "SMTP", 254);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Ssl"))
                    CreateUserfield<string, string>("CVA_EMLCONF", "Ssl", "SSL", 1, BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, "Y", BoYesNoEnum.tNO, new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } });

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Por"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Por", "Porta", 11, BoFieldTypes.db_Numeric);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "DflMes"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "DflMes", "Mensagem padrão", 254, BoFieldTypes.db_Memo);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "EmlCop"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "EmlCop", "E-mail cópia", 254);

                oProgress.Value++;

                if (!UserFieldExist("@CVA_EMLCONF", "Subject"))
                    CreateUserfield<DBNull, DBNull>("CVA_EMLCONF", "Subject", "Assunto", 254);

                oProgress.Value++;

                if (!UserFieldExist("OINV", "CVA_DocEnviado"))
                    CreateUserfield<int, string>("OINV", "CVA_DocEnviado", "Documento enviado", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, "0", BoYesNoEnum.tNO, new Dictionary<int, string> { { 0, "Não enviado" }, { 1, "Enviado" } });

                oProgress.Value++;
                oProgress.Stop();
            }
            catch (Exception)
            {
                oProgress.Stop();
                throw;
            }

        }

        private static void CreateUsertable(string tablename, string description, BoUTBTableType tabletype = BoUTBTableType.bott_NoObject)
        {
            UserTablesMD usertable = (UserTablesMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            usertable.TableName = tablename;
            usertable.TableDescription = description;
            usertable.TableType = tabletype;

            if (usertable.Add() != 0)
            {
                string sErrMsg;
                int lErrCode;

                B1Connection.Instance.Company.GetLastError(out lErrCode, out sErrMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(usertable);
                usertable = null;

                throw new Exception(string.Format("[{0}] {1}", lErrCode, sErrMsg));
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(usertable);
            usertable = null;
        }

        private static bool UserTableExist(string tablename)
        {
            UserTablesMD usertable = (UserTablesMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            bool retorno = usertable.GetByKey(tablename);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(usertable);
            usertable = null;

            return retorno;
        }

        private static void CreateUserfield<K, V>(string tablename, string fieldname, string description, int fieldsize, BoFieldTypes fieldtype = BoFieldTypes.db_Alpha, BoFldSubTypes fieldsubtype = BoFldSubTypes.st_None, string defaultvalue = null, BoYesNoEnum mandatory = BoYesNoEnum.tNO, Dictionary<K,V> validvalues = null)
        {
            UserFieldsMD userfield = (UserFieldsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserFields);

            userfield.TableName = tablename;
            userfield.Name = fieldname;
            userfield.Description = description;
            userfield.EditSize = fieldsize;
            userfield.Size = fieldsize;
            userfield.Type = fieldtype;
            userfield.SubType = fieldsubtype;

            if (validvalues != null)
            {
                foreach (var item in validvalues)
                {
                    userfield.ValidValues.Add();
                    userfield.ValidValues.Value = item.Key.ToString();
                    userfield.ValidValues.Description = item.Value.ToString();
                }

                userfield.DefaultValue = defaultvalue;
            }

            userfield.Mandatory = mandatory;

            if (userfield.Add() != 0)
            {
                int lErrCode;
                string sErrMsg;

                B1Connection.Instance.Company.GetLastError(out lErrCode, out sErrMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(userfield);
                userfield = null;

                throw new Exception(string.Format("[{0}] {1}", lErrCode, sErrMsg));
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(userfield);
            userfield = null;
        }

        private static bool UserFieldExist(string tablename, string fieldname)
        {
            bool retorno = false;

            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("SELECT 1 [result] FROM SYS.OBJECTS so ");
            builder.AppendLine("INNER JOIN SYS.COLUMNS sc ON sc.object_id = so.object_id ");
            builder.AppendLine("WHERE so.schema_id = 1 ");
            builder.AppendLine(string.Format("AND so.name = '{0}' AND sc.name = 'U_{1}'", tablename, fieldname));

            oRecordset.DoQuery(builder.ToString());

            int lRet = (int)oRecordset.Fields.Item("result").Value;

            retorno = (lRet == 1);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
            oRecordset = null;

            return retorno;
        }
    }
}
