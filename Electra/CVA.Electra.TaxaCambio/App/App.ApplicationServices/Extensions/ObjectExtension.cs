
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace App.ApplicationServices
{
    public static class ObjectExtension
    {
        public static bool IsNullOrEmpty(this object ValStr)
        {
            string vRet = "";

            if (ValStr != null)
            {
                vRet = ValStr.ToString();
            }

            return string.IsNullOrEmpty(vRet);
        }
    }
}
