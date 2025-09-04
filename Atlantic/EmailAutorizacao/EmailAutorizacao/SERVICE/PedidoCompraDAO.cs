using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using EmailAutorizacao.MODEL;
using EmailAutorizacao.SERVICE.Portal.Resource;
using EmailAutorizacao.HELPER;

namespace EmailAutorizacao.SERVICE
{
    public class PedidoCompraDAO
    {
        public static List<EmailMessageModel> RetrieveEmailList(int docNum, double docTotal)
        {
            var emailList = new List<EmailMessageModel>();
            var currentUser = B1Connection.Instance.Company.UserName;

            var sql = string.Format(Query.Draft_GetPurchaseOrder, docNum, docTotal.ToString().Replace(",", "."), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmm"), currentUser);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery(sql);

            while (!oRecordset.EoF)
            {
                var emailMessageModel = new EmailMessageModel
                {
                    BPLName = oRecordset.Fields.Item("BPLName").Value.ToString(),
                    CardCode = oRecordset.Fields.Item("CardCode").Value.ToString(),
                    CardName = oRecordset.Fields.Item("CardName").Value.ToString(),
                    CreateTS = int.Parse(oRecordset.Fields.Item("CreateTS").Value.ToString()),
                    DocDate = Format_StringToDate(oRecordset.Fields.Item("DocDate").Value.ToString()),
                    DocNum = int.Parse(oRecordset.Fields.Item("DocNum").Value.ToString()),
                    DocTotal = Format_StringToDouble(oRecordset.Fields.Item("DocTotal").Value.ToString()),
                    Email = oRecordset.Fields.Item("Email").Value.ToString(),
                    UserName = oRecordset.Fields.Item("UserName").Value.ToString()
                };
                emailList.Add(emailMessageModel);
                oRecordset.MoveNext();
            }

            // Se não encontrou tenta voltar 1 minuto, pois pode ter passado
            if (emailList.Count == 0)
            {
                sql = string.Format(Query.Draft_GetPurchaseOrder, docNum, docTotal.ToString().Replace(",", "."), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.AddMinutes(-1).ToString("HHmm"), currentUser);
                oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(sql);

                while (!oRecordset.EoF)
                {
                    var emailMessageModel = new EmailMessageModel
                    {
                        BPLName = oRecordset.Fields.Item("BPLName").Value.ToString(),
                        CardCode = oRecordset.Fields.Item("CardCode").Value.ToString(),
                        CardName = oRecordset.Fields.Item("CardName").Value.ToString(),
                        CreateTS = int.Parse(oRecordset.Fields.Item("CreateTS").Value.ToString()),
                        DocDate = Format_StringToDate(oRecordset.Fields.Item("DocDate").Value.ToString()),
                        DocNum = int.Parse(oRecordset.Fields.Item("DocNum").Value.ToString()),
                        DocTotal = Format_StringToDouble(oRecordset.Fields.Item("DocTotal").Value.ToString()),
                        Email = oRecordset.Fields.Item("Email").Value.ToString(),
                        UserName = oRecordset.Fields.Item("UserName").Value.ToString()
                    };
                    emailList.Add(emailMessageModel);
                    oRecordset.MoveNext();
                }
            }

            return emailList;
        }

        private static string Format_MoneyToString(double valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_MoneyToString(valor, BoMoneyPrecisionTypes.mpt_Sum);
            return oRecordset.Fields.Item(0).Value;
        }

        private static string Format_DateToString(DateTime valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_DateToString(valor);
            return oRecordset.Fields.Item(0).Value;
        }

        private static DateTime Format_StringToDate(string valor)
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

        private static double Format_StringToDouble(string s)
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

    }
}
