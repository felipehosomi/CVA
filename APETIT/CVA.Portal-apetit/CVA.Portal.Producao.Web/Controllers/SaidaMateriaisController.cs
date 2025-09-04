using CVA.Portal.Producao.BLL.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    
    public class SaidaMateriaisController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region  [ GetContrato ]
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetContrato(string BPLId)
        {
            var retlist = await GetContratoAsync(BPLId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetContratoAsync(string BPLId)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Contratos", $"GetContratos?filial={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }
        #endregion

        #region [ GetInsumo ]
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInsumo(string BPLId)
        {
            var retlist = await GetInsumoAsync(BPLId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetInsumoAsync(string BPLId)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("SaidaMaterial", $"GetInsumos?bplid={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }
        #endregion

        #region [ GetBPLId ]
        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetBPLIdAsync()
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Filial", $"GetFilial?userId={CvaSession.Usuario.Usuario}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }
        #endregion

        #region [ GetInsumoOnHand ]
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInsumoOnHand(string itemCode, string BPLId)
        {
            var ret = await api.GetAsync<SaidaInsumoModelGetItemOnHand>("SaidaMaterial", $"CheckItemOnHandBPLId?itemCode={itemCode}&bplId={BPLId}");
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion


        // GET: SaidaMateriais
        [CvaAuthorize("SaidaMateriais")]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new SaidaInsumoModel();

            if (TempData["modelSaidaMateriais"] != null)
            {
                obj = TempData["modelSaidaMateriais"] as SaidaInsumoModel;

                if (obj?.Itens?.Count > 0)
                    obj.Itens = obj.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();
            }
                
            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();
            
            //if (obj.ContratoList == null && obj.BPLIdList?.Where(x=>x.Value!=string.Empty).Count() > 0)
            //    obj.ContratoList = await GetContratoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList?.Where(x => x.Value != string.Empty).FirstOrDefault().Value);
            
            if (obj.InsumoList == null && obj.BPLIdList?.Count() > 0)
                obj.InsumoList = await GetInsumoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList?.Where(x => x.Value != string.Empty).FirstOrDefault().Value);

            obj.TipoSaidaList = await api.GetListAsync<SelectListItem>("SaidaMaterial", $"GetTiposSaida");

            obj.TipoSaidaList.Add(new SelectListItem() { Value = "0", Text = "(Selecione)" });

            obj.TipoSaidaList = obj.TipoSaidaList.OrderBy(x => x.Value).ToList();

            //obj.Itens.Add(new SaidaInsumoItensModel() { InsumoCode = "ABA", InsumoName = "ABCCC", Qty = 1, idList = 0, Delete = false });

            SetTempAlert();

            return View(obj);
        }

        
        // POST: SaidaMateriais/Create
        [HttpPost]
        [CvaAuthorize("SaidaMateriais")]
        public async System.Threading.Tasks.Task<ActionResult> Create(SaidaInsumoModel obj)
        {
            try
            {
                if (obj?.Itens?.Count > 0)
                    obj.Itens = obj.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

                if (string.IsNullOrEmpty(obj.BPLIdCode))
                    throw new Exception($"Filial não foi identificado.");

                if (string.IsNullOrEmpty(obj.Motivo))
                    throw new Exception($"Motivo não foi identificado.");

                if (string.IsNullOrEmpty(obj.ClienteCode))
                    throw new Exception($"Cliente não foi identificado.");

                if (string.IsNullOrEmpty(obj.TipoSaidaCode) || obj.TipoSaidaCode == "0")
                    throw new Exception($"Tipo de saída não foi identificado.");


                if (obj?.Itens?.Count > 0)
                {
                    var retOBPL = await api.GetAsync<SaidaInsumoModelGetOBPL>("Filial", $"GetSaidaInsumoOBPL?BPLId={obj.BPLIdCode}");
                    if (retOBPL == null)
                        throw new Exception("Nenhum registro foi informado nas configurações das filiais.");

                    if (string.IsNullOrEmpty(retOBPL.DflWhs))
                        throw new Exception("Campo DflWhs - está vazio nas configurações das filiais.");

                    if (string.IsNullOrEmpty(retOBPL.U_CVA_Dim1Custo))
                        throw new Exception("Campo U_CVA_Dim1Custo - está vazio nas configurações das filiais.");

                    foreach (var item in obj.Itens)
                    {
                        var retItem = await api.GetAsync<SaidaInsumoModelGetItemOnHand>("SaidaMaterial", $"CheckItemOnHand?itemCode={item.InsumoCode}&whs={retOBPL.DflWhs}");
                        if(retItem == null && retItem.OnHand == null)
                            throw new Exception($"Item: {item.InsumoCode} - Não foi encontrado.");

                        if (retItem.OnHand <= 0)
                            throw new Exception($"Item: {item.InsumoCode} - Não possui quantidade suficiente.");

                        if (item.Qty > retItem.OnHand)
                            throw new Exception($"Item: {item.InsumoCode} - Quantidade é a maior que a disponivel.");
                    }

                    var error = await api.PostAsync("SaidaMaterial", obj);
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                    else
                    {
                        TempData["Success"] = "Saída de Materiais cadastrado com sucesso.";
                    }
                }
                else
                    TempData["Error"] = "Nenhum item foi informado.";

                TempData["modelSaidaMateriais"] = (TempData["Success"] != null ? null : obj);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["modelSaidaMateriais"] = obj;
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }
        
        public void SetTempAlert()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"].ToString();
        }

        #region [ Repport ]

        [CvaAuthorize("ReportPosicaoEstoque")]
        public async System.Threading.Tasks.Task<ActionResult> ReportPosicaoEstoque()
        {
            var obj = new RepportSaidaInsumoModel();

            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();

            //if (obj.ContratoList == null && obj.BPLIdList?.Count() > 0)
            //    obj.ContratoList = await GetContratoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value);
            
            return View(obj);
        }


        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallReportPosicaoEstoque(string RptBPLId)
        {
            var ret = await api.GetAsync<List<SaidaInsumoItensModel02>>("SaidaMaterial", $"ReportPosicaoEstoque?RptBPLId={RptBPLId}");
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
