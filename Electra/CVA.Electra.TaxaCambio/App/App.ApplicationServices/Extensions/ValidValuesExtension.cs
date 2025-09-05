using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices
{
    public static class ValidValuesExtension
    {
        public static void RemoveAll(this ValidValues val)
        {
            try
            {
                while (val.Count > 0)
                {
                    val.Remove(0, BoSearchKey.psk_Index);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddFirstLine(this ValidValues val, string Description)
        {
            try
            {
                val.Add("-1", Description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
