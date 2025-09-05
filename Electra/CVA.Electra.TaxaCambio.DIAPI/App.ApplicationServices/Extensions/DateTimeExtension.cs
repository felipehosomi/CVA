using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace App.ApplicationServices
{
    public static class DateTimeExtension
    {
        public static bool IsDateTime(this object ValObj)
        {
            bool vRet = true;

            try
            {
                Convert.ToDateTime(ValObj, CultureInfo.InvariantCulture);
            }
            catch
            {
                if (ValObj.IsNullOrEmpty() || ValObj.ToString().Length != 8)
                {
                    vRet = false;
                }
            }

            return vRet;
        }

        public static DateTime? ConvertToDateTime(this object ObjVal, bool ValidNull = false)
        {
            try
            {
                DateTime vDateTime = new DateTime();

                if (ObjVal.IsDateTime())
                {
                    string vObjStr = ObjVal.ToString();

                    if (vObjStr.Length == 8)
                    {
                        int vAno = vObjStr.Substring(0, 4).ConvertToInt().Value;
                        int vMes = vObjStr.Substring(4, 2).ConvertToInt().Value;
                        int vDia = vObjStr.Substring(6, 2).ConvertToInt().Value;

                        vDateTime = new DateTime(vAno, vMes, vDia);
                    }
                    else
                    {
                        vDateTime = Convert.ToDateTime(ObjVal, CultureInfo.InvariantCulture);
                    }
                }
                else if (ValidNull && ObjVal.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    throw new Exception(string.Format("O valor de {0} não é uma data valida", ObjVal));
                }

                return vDateTime;
            }
            catch (Exception ex)
            {
                throw ex; 
            }

        }
        
        public static DateTime ToDateTime(this object ObjVal)
        {
            try
            {
                DateTime vDateTime = new DateTime();

                if (ObjVal.IsDateTime())
                {
                    string vObjStr = ObjVal.ToString();

                    if (vObjStr.Length == 8)
                    {
                        int vAno = vObjStr.Substring(0, 4).ConvertToInt().Value;
                        int vMes = vObjStr.Substring(4, 2).ConvertToInt().Value;
                        int vDia = vObjStr.Substring(6, 2).ConvertToInt().Value;

                        vDateTime = new DateTime(vAno, vMes, vDia);
                    }
                    else
                    {
                        vDateTime = Convert.ToDateTime(ObjVal, CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    throw new Exception(string.Format("O valor de {0} não é uma data valida", ObjVal));
                }

                return vDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
