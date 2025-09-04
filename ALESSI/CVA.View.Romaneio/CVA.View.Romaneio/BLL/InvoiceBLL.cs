
using CVA.View.Romaneio.DAO.Resources;
using CVA.View.Romaneio.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.BLL
{
    public class InvoiceBLL
    {
        public static string GetQuery(InvoiceFilterModel filterModel)
        {
            string sql = Query.Invoice_Get;
            if (!String.IsNullOrEmpty(filterModel.Branches))
            {
                sql += $" AND OINV.BPLId IN ({filterModel.Branches}) ";
            }
            if (!String.IsNullOrEmpty(filterModel.CarrierCode))
            {
                sql += $" AND INV12.Carrier = '{filterModel.CarrierCode}' ";
            }
            if (filterModel.DateFrom.HasValue)
            {
                sql += $" AND OINV.DocDate >= CAST('{filterModel.DateFrom.Value.ToString("yyyyMMdd")}' AS DATETIME) ";
            }
            if (filterModel.DateTo.HasValue)
            {
                sql += $" AND OINV.DocDate <= CAST('{filterModel.DateTo.Value.ToString("yyyyMMdd")}' AS DATETIME) ";
            }
            if (filterModel.NFFrom.HasValue)
            {
                sql += $" AND OINV.Serial >= {filterModel.NFFrom} ";
            }
            if (filterModel.NFTo.HasValue)
            {
                sql += $" AND OINV.Serial <= {filterModel.NFTo} ";
            }
            if (!String.IsNullOrEmpty(filterModel.State))
            {
                sql += $" AND INV12.State = '{filterModel.State}' ";
            }
            if (!String.IsNullOrEmpty(filterModel.City))
            {
                sql += $" AND INV12.CityS LIKE '%{filterModel.City}%' ";
            }

            sql += " ORDER BY OINV.DocNum ";

            return sql;
        }
    }
}
