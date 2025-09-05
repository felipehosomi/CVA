using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;

using System.Xml;

namespace CVA.Electra.ImportaFolha
{
    public static class Util
    {
        
        public static string Decrypt(string password)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(password.Trim());
            string returnValue = System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);

            return returnValue;
        }

        public static string Encrypt(string password)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(password.Trim());
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }


    }

}
