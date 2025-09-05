using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Electra.Currency.BLL
{
    public class CurrencyBLL
    {
        public static List<string> GetCurrencies()
        {
            List<string> list = new List<string>();

            string query = $"SELECT \"ISOCurrCod\" FROM {ConfigurationManager.AppSettings["Database"]}.\"OCRN\" WHERE \"CurrCode\" != 'R$'";

            Recordset rst = ConnectionBLL.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            rst.DoQuery(query);

            while (!rst.EoF)
            {
                list.Add(rst.Fields.Item(0).Value.ToString());

                rst.MoveNext();
            }

            return list;
        }

        public static string GetCurrency(string currency)
        {
            string query = $"SELECT TOP 1 \"CurrCode\" FROM {ConfigurationManager.AppSettings["Database"]}.\"OCRN\" WHERE \"ISOCurrCod\" = '{currency}'";

            Recordset rst = ConnectionBLL.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            rst.DoQuery(query);
            currency = rst.Fields.Item(0).Value.ToString();

            return currency;
        }

        public static void UpdateCurrency(string currency, double rate, DateTime date)
        {
            try
            {
                SBObob bob = ConnectionBLL.Company.GetBusinessObject(BoObjectTypes.BoBridge) as SBObob;
                bob.SetCurrencyRate(currency, date, rate, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
