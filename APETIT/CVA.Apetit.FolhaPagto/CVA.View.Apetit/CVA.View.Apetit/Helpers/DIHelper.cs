using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CVA.View.Apetit.Helpers
{
    public static class DIHelper
    {
        public static string Format_MoneyToString(double valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_MoneyToString(valor, BoMoneyPrecisionTypes.mpt_Sum);
            return oRecordset.Fields.Item(0).Value;
        }

        public static string Format_DateToString(DateTime valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_DateToString(valor);
            return oRecordset.Fields.Item(0).Value;
        }

        public static DateTime Format_StringToDate(string valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            var sValor = string.Empty;

            if (valor.Contains(" 00:00:00"))
                sValor = valor.Length > 10 ? valor.Substring(0, 10) : valor;
            else
                sValor = valor;

            oRecordset = oSboBob.Format_StringToDate(sValor);
            return Convert.ToDateTime(oRecordset.Fields.Item(0).Value);
        }

        public static int GetNextCode(string tableName)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT COALESCE(MAX(CAST({"Code".Aspas()} AS INT)), 0) + 1 FROM {$"@{tableName}".Aspas()}");

            var retorno = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public static double Format_StringToDouble(string s)
        {
            double d = 0;
            // This part is fast, when regional settings equal to sap B1 settings:
            try
            {
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
            }
            // Speed up performance: extend CompaneService variables to global variables and query them at addon startup.
            try
            {
                var nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                var oCompanyService = B1Connection.Instance.Company.GetCompanyService();
                var oAdminInfo = oCompanyService.GetAdminInfo();
                var sbodsep = oAdminInfo.DecimalSeparator;
                var sbotsep = oAdminInfo.ThousandsSeparator;
                // ReSharper disable StringIndexOfIsCultureSpecific.1
                if (s.IndexOf("".PadLeft(1)) > 0)
                // ReSharper restore StringIndexOfIsCultureSpecific.1
                {
                    // ReSharper disable StringIndexOfIsCultureSpecific.1
                    s = oAdminInfo.DisplayCurrencyontheRight == BoYesNoEnum.tYES
                        ? s.Substring(0, s.IndexOf("".PadLeft(1)))
                        : s.Substring(s.IndexOf("".PadLeft(1)), s.Length - s.IndexOf("".PadLeft(1)));
                    // ReSharper restore StringIndexOfIsCultureSpecific.1
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

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
