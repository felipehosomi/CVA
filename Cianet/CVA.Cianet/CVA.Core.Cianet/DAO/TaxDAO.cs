using System;

namespace CVA.Core.Cianet.DAO
{
    public class TaxDAO
    {
        public double GetIPIRate(string taxCode)
        {
            SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)AddOn.Common.SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string sql = String.Format(Resources.Query.Tax_GetIPIRate, taxCode);
            double ipiRate = 0;
            try
            {
                rst.DoQuery(sql);
                if (rst.RecordCount > 0)
                {
                    ipiRate = (double)rst.Fields.Item(0).Value;
                }

                return ipiRate;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rst);
                rst = null;
            }
        }
    }
}
