using CVA.ImportacaoLCM.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Controller
{
    public class FormatterHelper
    {


        //public static string DateToString(DateTime valor)
        //{
        //    var oSboBob = (SBObob)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
        //    var oRecordset = (Recordset)SBOApp.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
        //    oRecordset = oSboBob.Format_DateToString(valor);
        //    return oRecordset.Fields.Item(0).Value;
        //}

        public static DateTime StringToDate(string valor)
        {
            var sValor = valor;
            //		valor	"22/01/2012 00:00:00"	string
            if (valor.Contains(":"))
                sValor = @"{valor.Substring(6, 4)}{valor.Substring(3, 2)}{valor.Substring(0, 2)}";
            return DateTime.ParseExact(sValor, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        }

        public static double StringToDouble(string s)
        {
            double d = 0;
            try
            {
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
            }
            try
            {
                var nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                var oCompanyService = SBOApp.oCompany.GetCompanyService();
                var oAdminInfo = oCompanyService.GetAdminInfo();
                var sbodsep = oAdminInfo.DecimalSeparator;
                var sbotsep = oAdminInfo.ThousandsSeparator;
                if (s.IndexOf("".PadLeft(1)) > 0)
                {
                    s = oAdminInfo.DisplayCurrencyontheRight == BoYesNoEnum.tYES
                        ? s.Substring(0, s.IndexOf("".PadLeft(1)))
                        : s.Substring(s.IndexOf("".PadLeft(1)), s.Length - s.IndexOf("".PadLeft(1)));
                }
                var s1 = s.Replace(sbotsep, nfi.NumberGroupSeparator);

                s1 = s1.Replace(sbodsep, nfi.NumberDecimalSeparator);
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
                return 0;
            }
        }
    }
}

