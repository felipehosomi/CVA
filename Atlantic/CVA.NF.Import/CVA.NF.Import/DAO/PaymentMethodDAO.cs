using CVA.NF.Import.DAO.Resources;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.NF.Import.DAO
{
    public class PaymentMethodDAO
    {
        public static string GetCode(string desc)
        {
            Recordset rst = SBOConnectionDao.Companies[0].GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            try
            {
                rst.DoQuery(String.Format(Query.PaymentMehod_GetCode, desc));
                return rst.Fields.Item(0).Value.ToString();
            }
            finally
            {
                Marshal.ReleaseComObject(rst);
                rst = null;
            }
        }
    }
}
