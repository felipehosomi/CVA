
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ApplicationServices
{
    public static class IntExtension
    {
        public static bool IsInt(this object ObjVal)
        {
            bool vRet = true;

            try
            {
                Convert.ToInt32(ObjVal);
            }
            catch
            {
                vRet = false;
            }

            return vRet;
        }

        public static int? ConvertToInt(this object ObjVal, bool ValidNull = false)
        {
            try
            {
                int vInt = 0;

                if (ObjVal.IsInt())
                {
                    vInt = Convert.ToInt32(ObjVal);
                }
                else if (ValidNull && ObjVal.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    throw new Exception(string.Format("O valor de {0} não é um inteiro valido", ObjVal));
                }

                return vInt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int ToInt(this object ObjVal)
        {
            try
            {
                int vInt = 0;

                if (ObjVal.IsInt())
                {
                    vInt = Convert.ToInt32(ObjVal);
                }
                else if (ObjVal.IsNullOrEmpty())
                {
                    vInt = -1;
                }
                else
                {
                    throw new Exception(string.Format("O valor de {0} não é um inteiro valido", ObjVal));
                }

                return vInt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
