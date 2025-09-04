using DAL.Connection;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataInterface
{
    public class FilialDao
    {
        public static int GetId(string cnpj)
        {
            if (!cnpj.Contains("."))
            {
                cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            }

            var oRecordset = (Recordset)ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT BPlId FROM OBPL WHERE TaxIdNum = '{cnpj}'");

            int id = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
            oRecordset = null;

            return id;
        }
    }
}
