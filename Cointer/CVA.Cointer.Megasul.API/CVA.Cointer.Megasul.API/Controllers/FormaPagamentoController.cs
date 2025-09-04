using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class FormaPagamentoController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public FormaPagamentoModel Get(int pagina, int quantidade_registros)
        {
            string sql = Hana.FormaPagamento_Get;
            FormaPagamentoModel formaPagamentoModel = new FormaPagamentoModel();
            formaPagamentoModel.formas_pagamento = dao.FillListFromCommand<FormaPagamento>(sql);
            formaPagamentoModel.quantidade_registros = formaPagamentoModel.formas_pagamento.Count;
            if (formaPagamentoModel.formas_pagamento.Count > 0)
            {
                formaPagamentoModel.quantidade_registros_total = formaPagamentoModel.formas_pagamento[0].TotalRecords;
            }
            return formaPagamentoModel;
        }
    }
}
