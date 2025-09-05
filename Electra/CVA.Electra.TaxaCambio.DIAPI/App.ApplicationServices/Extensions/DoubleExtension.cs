using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace App.ApplicationServices
{
    public static class DoubleExtension
    {
        public static bool IsDouble(this object ValObj)
        {
            bool vRet = true;

            try
            {
                Convert.ToDouble(ValObj, CultureInfo.InvariantCulture);
            }
            catch
            {
                if (ValObj.IsNullOrEmpty())
                {
                    vRet = false;
                }
            }

            return vRet;
        }

        public static Double ToDouble(this object ObjVal)
        {
            try
            {

                // Unify string (no spaces, only .)
                var output = ObjVal.ToString().Trim().Replace(" ", "").Replace(",", ".");

                // Split it on points
                string[] split = output.Split('.');

                if (split.Count() > 1)
                {
                    // Take all parts except last
                    output = string.Join("", split.Take(split.Count() - 1).ToArray());

                    // Combine token parts with last part
                    output = string.Format("{0}.{1}", output, split.Last());
                }

                double vDouble = 0;

                if (ObjVal.IsDouble())
                {
                    vDouble = Convert.ToDouble(output, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception(string.Format("O valor de {0} não é uma data valida", ObjVal));
                }

                return vDouble;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
