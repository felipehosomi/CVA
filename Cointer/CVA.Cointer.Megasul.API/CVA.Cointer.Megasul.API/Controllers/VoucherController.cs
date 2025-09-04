using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class VoucherController : ApiController
    {
        HanaDAO dao = new HanaDAO();
        
        public VoucherModel Get(int pagina, int quantidade_registros, string data_de = null, string cnpj_cpf = null, string codigo_sap = "", string identificador = "")
        {
            string sql = Hana.Voucher_Get;
            string where = String.Empty;

            if (!String.IsNullOrEmpty(data_de))
            {
                DateTime date;
                if (DateTime.TryParseExact(data_de, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    where += $" AND ODPI.\"DocDate\" >= '{date.ToString("yyyyMMdd")}' ";
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(string.Format("data_de deve estar no formato dd/mm/aaaa")),
                        ReasonPhrase = "Formato do campo 'data_de' inválido"
                    };
                    throw new HttpResponseException(resp);
                }
            }

            if (!String.IsNullOrEmpty(cnpj_cpf))
            {
                where += $" AND (REPLACE(REPLACE(REPLACE(\"TaxId0\", '.', ''), '/', ''), '-', '') = '{cnpj_cpf}' OR REPLACE(REPLACE(\"TaxId4\", '.', ''), '-', '') = '{cnpj_cpf}') ";
            }
            if (!String.IsNullOrEmpty(codigo_sap))
            {
                where += $" AND ODPI.\"CardCode\" = '{codigo_sap}' ";
            }
            if (!String.IsNullOrEmpty(identificador))
            {
                where += $" AND ODPI.\"DocNum\" = '{identificador}' ";
            }

            sql = String.Format(sql, where);
            sql += $" limit {quantidade_registros} offset {quantidade_registros * (pagina - 1)} ";

            VoucherModel voucherModel = new VoucherModel();
            voucherModel.vouchers = dao.FillListFromCommand<VoucherModel.Voucher>(sql);
            voucherModel.quantidade_registros = voucherModel.vouchers.Count;
            if (voucherModel.vouchers.Count > 0)
            {
                voucherModel.quantidade_registros_total = voucherModel.vouchers[0].TotalRecords;
            }

            return voucherModel;
        }

    }
}