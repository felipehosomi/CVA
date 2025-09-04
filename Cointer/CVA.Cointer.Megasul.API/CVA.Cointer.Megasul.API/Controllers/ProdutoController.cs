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
    public class ProdutoController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public ItemModel Get(int pagina, int quantidade_registros, string data_de = null, string codigo_produto_sap = null)
        {
            string sql = Hana.Item_Get;
            string sqlCodigosBarra = Hana.Item_GetCodigosBarra;
            string where = String.Empty;

            if (!String.IsNullOrEmpty(data_de))
            {
                DateTime date;
                if (DateTime.TryParseExact(data_de, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    where += $" AND (OITM.\"CreateDate\" >= '{date.ToString("yyyyMMdd")}' OR OITM.\"UpdateDate\" >= '{date.ToString("yyyyMMdd")}') ";
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
                where += $" AND OITM.\"ItemCode\" = '{codigo_produto_sap}' ";
            }

            sql = String.Format(sql, where, System.Configuration.ConfigurationManager.AppSettings["ListaPreco"]);
            sql += $" limit {quantidade_registros} offset {quantidade_registros * (pagina - 1)} ";

            sqlCodigosBarra = String.Format(sqlCodigosBarra, where);

            ItemModel itemModel = new ItemModel();
            itemModel.produtos = dao.FillListFromCommand<Produto>(sql);
            List<Barra> barrasList = dao.FillListFromCommand<Barra>(sqlCodigosBarra);

            foreach (var item in itemModel.produtos)
            {
                item.barras = new List<Barra>();

                List<Barra> barrasListItem = barrasList.Where(m => m.ItemCode == item.codigo_sap).ToList();
                if (barrasListItem.Count > 0)
                {
                    item.barras.AddRange(barrasListItem);
                    foreach (var itemBarra in barrasListItem)
                    {
                        barrasList.Remove(itemBarra);
                    }
                }
                else
                {
                    item.barras.Add(new Barra() { codigo = item.codigo_barras });
                }
                
                item.imposto = new Imposto();
                item.imposto.icms = new Icms();
                item.imposto.icms.tipo = item.TipoIcms;
                item.imposto.icms.aliquota = item.AliquotaIcms;

                item.imposto.pis_cofins = new Pis_Cofins();
                item.imposto.ipi = new Ipi();


            }

            itemModel.quantidade_registros = itemModel.produtos.Count;
            if (itemModel.produtos.Count > 0)
            {
                itemModel.quantidade_registros_total = itemModel.produtos[0].TotalRecords;
            }

            return itemModel;
        }
    }
}
