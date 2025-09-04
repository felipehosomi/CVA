using SAPbobsCOM;
using System.Runtime.InteropServices;

namespace PackIndicator.Controllers
{
    public class PurchaseOrderController
    {
        public static int GetDocEntry(int docNum)
        {
            Recordset rst = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                rst.DoQuery($@"SELECT ""DocEntry"" FROM OPOR WHERE ""DocNum"" = {docNum}");
                return (int)rst.Fields.Item(0).Value;
            }
            finally
            {
                Marshal.ReleaseComObject(rst);
                rst = null;
            }
        }
    }
}
