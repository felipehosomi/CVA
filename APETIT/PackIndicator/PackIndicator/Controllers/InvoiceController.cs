using SAPbobsCOM;

namespace PackIndicator.Controllers
{
    public class InvoiceController
    {
        public static bool IsDefaultCustomer(string cardCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset.DoQuery($@"select * from OBPL where ""DflCust"" = '{cardCode}'");
            return recordset.RecordCount > 0;
        }
    }
}
