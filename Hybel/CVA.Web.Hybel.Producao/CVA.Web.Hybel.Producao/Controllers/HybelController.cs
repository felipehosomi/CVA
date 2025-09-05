using CVA.Web.Hybel.Producao.DAO;
using CVA.Web.Hybel.Producao.Helpers;
using CVA.Web.Hybel.Producao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Web.Hybel.Producao.Controllers
{
    public class HybelController : ApiController
    {
        public HttpResponseMessage Get(string ordemProducao, string item)
        {
            if (String.IsNullOrEmpty(ordemProducao) && String.IsNullOrEmpty(item))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            string sql = String.Format(SQL.Item_GetByOP, ordemProducao + item);
            ItemModel itemModel = new SqlHelper().FillModelAccordingToSql<ItemModel>(sql);
            //ItemModel itemModel = new ItemModel();
            //itemModel.CodigoProduto = "12345";
            //itemModel.Quantidade = 10;

            if (String.IsNullOrEmpty(itemModel.CodigoProduto))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, itemModel);
            }
        }
    }
}
