using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices
{
    public static class DBDataSourceExtension
    {
        public static string GetValue(this DBDataSource val, object Index)
        {
            try
            {
                var vLine = val.Offset;

                return val.GetValue(Index, vLine).Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetValue(this DBDataSource DBSource, object Index, string NewVal)
        {
            try
            {
                var vLine = DBSource.Offset;

                DBSource.SetValue(Index, vLine, NewVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
