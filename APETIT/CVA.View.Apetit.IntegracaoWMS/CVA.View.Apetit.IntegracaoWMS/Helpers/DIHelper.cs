using System;
using System.Text;
using SAPbobsCOM;
using System.Threading;
using System.Windows.Forms;

namespace CVA.View.Apetit.IntegracaoWMS.Helpers
{
    public static class DIHelper
    {
        public static string FillSpacesAtLeft(this string text, int length)
        {
            return FillSpaces(text, length);
        }

        public static string FillSpacesAtRight(this string text, int length)
        {
            return FillSpaces(text, length, false);
        }

        private static string FillSpaces(string text, int length, bool isLeft = true)
        {
            if (length - text.Length <= 0)
                return text.Substring(0, length);

            StringBuilder str = new StringBuilder();
            str.Append(' ', (length - text.Length));

            if (isLeft)
                return $"{str.ToString()}{text}";

            return $"{text}{str.ToString()}";
        }

        public static string FillZeroes(this string inText, int length)
        {
            var text = inText.Replace(",", "").Replace(".", "");

            if (length - text.Length <= 0)
                return text.Substring(0, length);

            StringBuilder str = new StringBuilder();
            str.Append('0', (length - text.Length));

            return $"{str.ToString()}{text}";
        }

        public static string FillZeroes(this double num, int length)
        {
            return num.ToString().FillZeroes(length);
        }

        public static string FillZeroes(this int num, int length)
        {
            return num.ToString().FillZeroes(length);
        }

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


        private static string resultString;
        /// <summary>
        /// Dialog para selecionar arquivo
        /// </summary>
        /// <returns>Arquivo Selecionado</returns>
        public static string OpenFileDialog(string filter = "")
        {
            Thread thread = new Thread(() => ShowOpenFileDialog(filter));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        public static string OpenFolderBrowserDialog(string filter = "")
        {
            Thread thread = new Thread(() => ShowFolderBrowserDialog(filter));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        private static void ShowFolderBrowserDialog(string selectedPath = null)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(selectedPath))
            {
                fbd.SelectedPath = selectedPath;
            }

            if (fbd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = fbd.SelectedPath;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }

        private static void ShowOpenFileDialog(string filter = "")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!string.IsNullOrEmpty(filter))
            {
                ofd.Filter = filter;
            }
            if (ofd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = ofd.FileName;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }

        public static string OpenSaveFileDialog(string filter = "")
        {
            Thread thread = new Thread(() => ShowOpenSaveFileDialog(filter));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        private static void ShowOpenSaveFileDialog(string filter = "")
        {
            SaveFileDialog ofd = new SaveFileDialog();
            if (!string.IsNullOrEmpty(filter))
            {
                ofd.Filter = filter;
            }
            if (ofd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = ofd.FileName;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}
