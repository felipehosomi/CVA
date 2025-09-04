using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Compras;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("OfertaCompra")]
    public class OfertaCompraController : Controller
    {
        APICallUtil api = new APICallUtil();

        public async Task<ActionResult> Index()
        {
            List<OfertaCompraListModel> model = new List<OfertaCompraListModel>();

            if (CvaSession.Usuario.CodPerfil == "0000000003")
            {
                model = await api.GetListAsync<OfertaCompraListModel>("OfertaCompra", $"sFornecedor={CvaSession.Usuario.Usuario}");
                ViewBag.Comprador = false;
                ViewBag.Fornecedores = new SelectList(await api.GetListAsync<ComboBoxModel>("Fornecedor"), "Code", "Name", CvaSession.Usuario.Usuario);
            } else
            {
                model = await api.GetListAsync<OfertaCompraListModel>("OfertaCompra", "sFornecedor=");
                ViewBag.Comprador = true;
                ViewBag.Fornecedores = new SelectList(await api.GetListAsync<ComboBoxModel>("Fornecedor"), "Code", "Name");   
            }

            ViewBag.Filial = new SelectList(await api.GetListAsync<ComboBoxModel>("Filial"), "Code", "Name");

            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            OfertaCompraModel model = await api.GetAsync<OfertaCompraModel>("OfertaCompra", id);
            
            int i = 0;
            foreach (OfertaCompraItemModel item in model.Itens)
            {
                List<OfertaCompraItemUMModel> itensUM = await api.GetListAsync<OfertaCompraItemUMModel>("ItemUnidadeMedida", $"itemCode={item.ItemCode}");
                model.Itens[i].CodigosUM = new SelectList(itensUM, "Code", "Desc", item.UomEntry);

                i++;
            }

            ViewBag.CondicaoPagamento = new SelectList(await api.GetListAsync<ComboBoxModel>("CondicaoPagamento"), "Code", "Name", model.GroupNum);

            ViewBag.Comprador = (CvaSession.Usuario.CodPerfil == "0000000002" ? true : false);

            return View(model);
        }

        public async Task<ActionResult> Patch(OfertaCompraModel modelItens)
        {
            try
            {
                var result = await api.PutAsync("OfertaCompra", $"DocEntry={modelItens.DocEntry}", modelItens);

                if (!string.IsNullOrEmpty(result))
                {
                    throw new Exception(result);
                }

                var result1 = await SendEmail(TiposEmail.EnvioCotacao, modelItens.DocNum.ToString(), modelItens.CompradorNome, modelItens.CompradorEmail, "", "");

                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<ActionResult> SendEmail(TiposEmail tipoEmail, string sCotacaoCompra, string sNomeContato, string sEmailContato, string sObsRevisao = "", string sOrdemCompra = "")
        {
            var result = await api.GetAsync<string>("OfertaCompra/SendEmail", $"tipoEmail={Convert.ToInt32(tipoEmail)}&sCotacaoCompra={sCotacaoCompra}&sNomeContato={sNomeContato}&sEmailContato={sEmailContato}&sObsRevisao={sObsRevisao}&sOrdemCompra={sOrdemCompra}", "?");

            return RedirectToAction("Index");
        }
    }
}