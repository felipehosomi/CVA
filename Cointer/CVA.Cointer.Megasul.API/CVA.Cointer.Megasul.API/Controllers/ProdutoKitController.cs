using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class ProdutoKitController : ApiController
    {
        HanaDAO dao = new HanaDAO();
        public ItemEstruturaModel Get(int pagina, int quantidade_registros, string data_de = null, string codigo_produto_sap = null)
        {
            string sql = Hana.ItemEstrutura_Get;
            string where = String.Empty;

            if (!String.IsNullOrEmpty(data_de))
            {
                DateTime date;
                if (DateTime.TryParseExact(data_de, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    where += $" AND (OITT.\"CreateDate\" >= '{date.ToString("yyyyMMdd")}' OR OITT.\"UpdateDate\" >= '{date.ToString("yyyyMMdd")}') ";
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

            if (!String.IsNullOrEmpty(codigo_produto_sap))
            {
                where += String.Format(Hana.ItemEstrutura_WhereItemCode, codigo_produto_sap);
            }

            sql = String.Format(sql, where, System.Configuration.ConfigurationManager.AppSettings["ListaPreco"]);
            sql += $" limit {quantidade_registros} offset {quantidade_registros * (pagina - 1)} ";

            ItemEstruturaModel itemEstruturaModel = new ItemEstruturaModel();
            itemEstruturaModel.produtos = dao.FillListFromCommand<ItemEstrutura>(sql);
            foreach (var item in itemEstruturaModel.produtos)
            {
                item.itens = new List<Item>();
                item.itens.Add(new Item() { codigo_sap = item.codigo_sap_item, preco = item.preco, quantidade = item.quantidade });
            }

            itemEstruturaModel.quantidade_registros = itemEstruturaModel.produtos.GroupBy(m => m.codigo_sap).Count();
            if (itemEstruturaModel.produtos.Count > 0)
            {
                itemEstruturaModel.quantidade_registros_total = itemEstruturaModel.produtos[0].TotalRecords;
            }

            return itemEstruturaModel;
        }
    }
}
