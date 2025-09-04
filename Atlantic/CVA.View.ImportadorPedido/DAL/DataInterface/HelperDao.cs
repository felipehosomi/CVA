using SAPbobsCOM;
using DAL.Connection;

namespace DAL.DataInterface
{
    public class HelperDao
    {
        public static int GetNextCode(string tableName)
        {
            var oRecordset = (Recordset)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@{tableName}]");

            var retorno = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public static string GetIdByDesc(string description, string tableName, string columnNameID, string columnNameDesc)
        {
            var oRecordset = (Recordset)ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT {columnNameID} FROM {tableName} WHERE {columnNameDesc} = '{description}'");

            string id = oRecordset.Fields.Item(0).Value.ToString();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
            oRecordset = null;

            return id;
        }
    }
}
