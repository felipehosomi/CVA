using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ApplicationServices
{
    public static class BoolExtension
    {
        public static bool In(this string sVal, params string[] args)
        {
            try
            {
                var vRet = false;

                for (int i = 0; i < args.Length; i++)
                {
                    if (sVal == args[i])
                    {
                        vRet = true;
                        break;
                    }
                }

                return vRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
