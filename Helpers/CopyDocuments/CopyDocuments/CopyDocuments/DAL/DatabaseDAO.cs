using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class DatabaseDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;

        public DatabaseDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
        }

        public void Verify()
        {
            try
            {
                //if (!UserFieldExist(ConnectionTo, "OINV", "CVA_DocEntryFrom"))
                //    CreateUserfield<int, string>(ConnectionTo, "OINV", "CVA_DocEntryFrom", "DocEntry Base", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None);

                //if (!UserFieldExist(ConnectionTo, "OWOR", "CVA_DocEntryFrom"))
                //    CreateUserfield<int, string>(ConnectionTo, "OWOR", "CVA_DocEntryFrom", "DocEntry Base", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None);

                //if (!UserFieldExist(ConnectionTo, "OWTR", "CVA_DocEntryFrom"))
                //    CreateUserfield<int, string>(ConnectionTo, "OWTR", "CVA_DocEntryFrom", "DocEntry Base", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None);

                if (!UserFieldExist(ConnectionFrom, "OJDT", "CVA_Imported"))
                    CreateUserfield<int, string>(ConnectionFrom, "OJDT", "CVA_Imported", "Documento Importado", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, "0", BoYesNoEnum.tNO, new Dictionary<int, string> { { 0, "Não Importado" }, { 1, "Importado" } });

                if (!UserFieldExist(ConnectionFrom, "OINV", "CVA_Imported"))
                    CreateUserfield<int, string>(ConnectionFrom, "OINV", "CVA_Imported", "Documento Importado", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, "0", BoYesNoEnum.tNO, new Dictionary<int, string> { { 0, "Não Importado" }, { 1, "Importado" } });

                if (!UserFieldExist(ConnectionFrom, "OWOR", "CVA_Imported"))
                    CreateUserfield<int, string>(ConnectionFrom, "OWOR", "CVA_Imported", "Documento Importado", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, "0", BoYesNoEnum.tNO, new Dictionary<int, string> { { 0, "Não Importado" }, { 1, "Importado" } });

                if (!UserFieldExist(ConnectionFrom, "OWTR", "CVA_Imported"))
                    CreateUserfield<int, string>(ConnectionFrom, "OWTR", "CVA_Imported", "Documento Importado", 11, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, "0", BoYesNoEnum.tNO, new Dictionary<int, string> { { 0, "Não Importado" }, { 1, "Importado" } });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateUserfield<K, V>(B1Connection connection, string tablename, string fieldname, string description, int fieldsize, BoFieldTypes fieldtype = BoFieldTypes.db_Alpha, BoFldSubTypes fieldsubtype = BoFldSubTypes.st_None, string defaultvalue = null, BoYesNoEnum mandatory = BoYesNoEnum.tNO, Dictionary<K, V> validvalues = null)
        {
            UserFieldsMD userfield = (UserFieldsMD)connection.oCompany.GetBusinessObject(BoObjectTypes.oUserFields);

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

                connection.oCompany.GetLastError(out lErrCode, out sErrMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(userfield);
                userfield = null;

                throw new Exception(string.Format("[{0}] {1}", lErrCode, sErrMsg));
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(userfield);
            userfield = null;
        }

        private static bool UserFieldExist(B1Connection connection, string tablename, string fieldname)
        {
            bool retorno = false;

            Recordset oRecordset = (Recordset)connection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
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
