using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class EstoqueController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public EstoqueModel Get(int pagina, int quantidade_registros, string data_de = null, string codigo_produto_sap = null)
        {
            string sql = Hana.Estoque_Get;

            if (!String.IsNullOrEmpty(data_de))
            {
                DateTime date;
                if (DateTime.TryParseExact(data_de, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    data_de = date.ToString("yyyyMMdd");
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
            else
            {
                data_de = "21000101";
            }

            sql = String.Format(sql, quantidade_registros, quantidade_registros * (pagina - 1), data_de, codigo_produto_sap);

            EstoqueModel estoqueModel = new EstoqueModel();
            estoqueModel.EstoqueItemList = dao.FillListFromCommand<EstoqueItemModel>(sql);
            estoqueModel.produtos = new System.Collections.Generic.List<EstoqueItem>();

            foreach (var item in estoqueModel.EstoqueItemList.GroupBy(m => m.codigo_produto_sap))
            {
                EstoqueItem produto = new EstoqueItem();
                produto.codigo_produto_sap = item.Key;
                produto.estoques = new System.Collections.Generic.List<Estoque>();

                foreach (var itemEstoque in item)
                {
                    Estoque estoque = new Estoque();
                    estoque.lote = itemEstoque.lote;
                    estoque.numero_serie = itemEstoque.numero_serie;
                    estoque.quantidade = itemEstoque.quantidade;

                    produto.estoques.Add(estoque);
                }
                estoqueModel.produtos.Add(produto);
            }

            estoqueModel.quantidade_registros = estoqueModel.produtos.Count;
            if (estoqueModel.EstoqueItemList.Count > 0)
            {
                estoqueModel.quantidade_registros_total = estoqueModel.EstoqueItemList[0].TotalRecords;
            }

            return estoqueModel;
        }
    }
}
