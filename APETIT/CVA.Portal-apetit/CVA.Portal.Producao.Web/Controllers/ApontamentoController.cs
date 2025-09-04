using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    
    public class ApontamentoController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region [ GetOpenOrders ]
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallOpenOrders(string ordersBPLId, DateTime? ordersDate)
        {
            var retlist = await GetOpenOrdersAsync(ordersBPLId, ordersDate);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<SelectList> GetOpenOrdersAsync(string ordersBPLId, DateTime? ordersDate)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("OrdemProducao", $"GetOpenOrders?ordersBPLId={ordersBPLId}&ordersDate={ordersDate}");

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

        #region [ GetCliente ]
        public async System.Threading.Tasks.Task<ApontamentoGetClienteModel> GetClienteAsync(string clienteBPLID)
        {
            var ret = await api.GetAsync<ApontamentoGetClienteModel>("Apontamento", $"GetCliente?clienteBPLID={clienteBPLID}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallCliente(string clienteBPLID)
        {
            var retlist = await GetClienteAsync(clienteBPLID);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetTerceiros ]
        public async System.Threading.Tasks.Task<CVA_APTO_TERCEIROSModel.APIModel> GetTerceirosAsync(string tBPLID, DateTime? tData, string tTurno, string tServico)
        {
            var ret = await api.GetAsync<CVA_APTO_TERCEIROSModel.APIModel>("Apontamento", $"GetTerceiros?tBPLID={tBPLID}&tData={tData}&tTurno={tTurno}&tServico={tServico}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallTerceiros(string tBPLID, DateTime? tData, string tTurno, string tServico)
        {
            var retlist = await GetTerceirosAsync(tBPLID, tData, tTurno, tServico);
            return Json((retlist == null ? new CVA_APTO_TERCEIROSModel.APIModel() : retlist), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetTerceirosBPs ]
        public async System.Threading.Tasks.Task<SelectList> GetTerceirosBPAsync(string bBPLID)
        {
            var ret = await api.GetAsync<List<CVA_APTO_TERCEIROSModel.CVA_APTO_TerceirosSAP>>("Apontamento", $"GetTerceirosBP?bBPLID={bBPLID}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CardCode, Name = item.CardCode + " | " + item.CardName + " | " +item.U_CVA_CNPJ});
            }

            return new SelectList(list, "Code", "Name");
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallTerceirosBP(string bBPLID)
        {
            var retlist = await GetTerceirosBPAsync(bBPLID);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ApontamentoGetClienteModel> GetTerceirosBPCodeAsync(string bCardCode)
        {
            var ret = await api.GetAsync<ApontamentoGetClienteModel>("Apontamento", $"GetTerceirosBPCode?bCardCode={bCardCode}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallTerceirosBPCode(string bCardCode)
        {
            var retlist = await GetTerceirosBPCodeAsync(bCardCode);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetInfo ]
        public async System.Threading.Tasks.Task<List<ApontamentoGetInfoItensContratoModel>> GetInfoAsync(string contratoId, string servicoId)
        {
            var ret = await api.GetAsync<List<ApontamentoGetInfoItensContratoModel>>("Apontamento", $"GetInfoPlanejamentoItens?contratoIdInfoItens={contratoId}&servicoIdInfoItens={servicoId}");
            return ret;
        }
        #endregion
        #region [ GetInfo ]
        public async System.Threading.Tasks.Task<List<ApontamentoGetInfoItensContratoModel>> GetInfoFechadoAsync(string contratoId, string servicoId)
        {
            var ret = await api.GetAsync<List<ApontamentoGetInfoItensContratoModel>>("Apontamento", $"GetInfoPlanejamentoItensFechado?contratoIdInfoItens={contratoId}&servicoIdInfoItens={servicoId}");
            return ret;
        }


        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInfo(string contratoId, string servicoId)
        {
            var retlist = await GetInfoAsync(contratoId, servicoId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInfoFechado(string contratoId, string servicoId)
        {
            var retlist = await GetInfoFechadoAsync(contratoId, servicoId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetItemInfo ]
        public async System.Threading.Tasks.Task<ApontamentoItemInfo> GetItemInfoAsync(string infoItem, string infoBPLID)
        {
            var ret = await api.GetAsync<ApontamentoItemInfo>("Apontamento", $"GetItemInfo?infoItem={infoItem}&infoBPLID={infoBPLID}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetItemInfo(string infoItem, string infoBPLID)
        {
            var retlist = await GetItemInfoAsync(infoItem, infoBPLID);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetServico ]
        public async System.Threading.Tasks.Task<SelectList> GetServicoAsync(string grpServicoId)
        {
            var ret = await api.GetAsync<List<ApontamentoGetServicoModel>>("Apontamento", $"GetServico?contratoIdGrupo={grpServicoId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.U_CVA_ID_SERVICO, Name = item.U_CVA_ID_SERVICO + " - " + item.U_CVA_D_SERVICO });
            }

            return new SelectList(list, "Code", "Name");
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetServico(string servicoTurno)
        {
            var retlist = await GetServicoAsync(servicoTurno);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ GetItens ]
        public async System.Threading.Tasks.Task<SelectList> GetItensListAsync(string itemCodeList, string BPLIdList)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Apontamento", $"GetItensList?itemCodeList={itemCodeList}&BPLIdList={BPLIdList}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.CODE + " - " + item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetItensList(string itemCodeList, string BPLIdList)
        {
            var retlist = await GetItensListAsync(itemCodeList, BPLIdList);
            return Json(retlist, JsonRequestBehavior.AllowGet);
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

        #region [ GetQty ]
        public async System.Threading.Tasks.Task<ApontamentoGetQtyModel> GetQtyAsync(string qtySERVICO, string qtyTURNO, DateTime date, string qtyBPLID)
        {
            var ret = await api.GetAsync<ApontamentoGetQtyModel>("Apontamento", $"GetQty?qtySERVICO={qtySERVICO}&qtyTURNO={qtyTURNO}&date={date}&qtyBPLID={qtyBPLID}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetQtyAsync(string qtySERVICO, string qtyTURNO, DateTime date, string qtyBPLID)
        {
            var retlist = await GetQtyAsync(qtySERVICO, qtyTURNO, date, qtyBPLID);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ CheckPreviousDay ] 
        public async System.Threading.Tasks.Task<ApontamentoGetQtyModel> CheckPreviousDayAsync(string qtyFilial, string qtyCONTRATO, string qtyGRPSERVICO, string qtySERVICO, DateTime date)
        {
            var ret = await api.GetAsync<ApontamentoGetQtyModel>("Apontamento", $"CheckPreviousDay?qtyFilial={qtyFilial}&qtyCONTRATO={qtyCONTRATO}&qtyGRPSERVICO={qtyGRPSERVICO}&qtySERVICO={qtySERVICO}&date={date}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallCheckPreviousDay(string qtyFilial, string qtyCONTRATO, string qtyGRPSERVICO, string qtySERVICO, DateTime date)
        {
            var retlist = await CheckPreviousDayAsync(qtyFilial, qtyCONTRATO, qtyGRPSERVICO, qtySERVICO, date);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ CheckDay ]
        public async System.Threading.Tasks.Task<ApontamentoCheckDay> CheckDayAsync(DateTime date)
        {
            var ret = await api.GetAsync<ApontamentoCheckDay>("Apontamento", $"CheckDay?date={date}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallCheckDay(DateTime date)
        {
            var retlist = await CheckDayAsync(date);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ Complementacao ]
        public async System.Threading.Tasks.Task<string> ComplementacaoAsync(string cpFilial, string cpCONTRATO, string cpCliente, string cpSERVICO, DateTime date, string cpUser)
        {
            var ret = await api.GetAsync<string>("Apontamento", $"Complementacao?cpFilial={cpFilial}&cpCONTRATO={cpCONTRATO}&cpCliente={cpCliente}&cpSERVICO={cpSERVICO}&date={date}&cpUser={cpUser}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallComplementacao(string cpFilial, string cpCONTRATO, string cpCliente, string cpSERVICO, DateTime date)
        {
            var retlist = await ComplementacaoAsync(cpFilial, cpCONTRATO, cpCliente, cpSERVICO, date, CvaSession.Usuario.Usuario);
            if (retlist != null && !retlist.ToUpper().StartsWith("ERRO"))
                retlist = "OK";
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [ InsumoOnHand ]
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInsumoOnHand(string itemCode, string BPLId)
        {
            var ret = await api.GetAsync<ApontamentoGetItemOnHand>("Apontamento", $"CheckItemOnHandBPLId?itemCode={itemCode}&bplId={BPLId}");
            return Json(ret, JsonRequestBehavior.AllowGet);
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
            string itemCodeList = "";
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Apontamento", $"GetItensList?itemCodeList={itemCodeList}&BPLIdList={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.CODE +" - "+ item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }
        #endregion


        [CvaAuthorize("Apontamento")]
        // GET: PainelApontamento
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new ApontamentoModel();

            if(TempData["painelToApontamento"]!=null)
            {
                obj = TempData["painelToApontamento"] as ApontamentoModel;
                TempData["painelToApontamento"] = null;
            }
            else
            {
                if (TempData["modelApontamento"] != null)
                    obj = TempData["modelApontamento"] as ApontamentoModel;
            }

            

            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();


            //obj.apontamentoData = Convert.ToDateTime("2020-04-23");
            //TODO: REMOVER
            
            if (obj.apontamentoData == DateTime.MinValue)
                obj.apontamentoData = DateTime.Now;
            

            if (!string.IsNullOrEmpty(obj.BPLIdCode))
                obj.ContratoList = await GetOpenOrdersAsync(obj.BPLIdCode, obj.apontamentoData);
                

            //if(!string.IsNullOrEmpty(obj.ServicoCode))
            //    obj.ServicoList = await GetServicoAsync(obj.ServicoCode);

            #region [ Comentado ]
            //if (obj.ContratoList == null && obj.BPLIdList?.Count() > 0)
            //    obj.ContratoList = await GetContratoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value);

            //if(obj.ContratoList.Count() == 1)
            //{
            //    var firstTst = obj.ContratoList.FirstOrDefault();
            //    obj.ContratoCode = firstTst.Value;
            //}

            //if (!string.IsNullOrEmpty(obj.ContratoCode))
            //{
            //    var contratoInfo = await GetInfoAsync(obj.ContratoCode, obj.apontamentoData);
            //    if(contratoInfo != null)
            //    {
            //        obj.ClienteCode = contratoInfo.U_CVA_ID_CLIENTE;
            //        obj.ClienteName = contratoInfo.U_CVA_ID_CLIENTE + " - " + contratoInfo.U_CVA_DES_CLIENTE;

            //        obj.GrupoServicoCode = contratoInfo.U_CVA_GRPSERVICO;
            //        obj.GrupoServicoName = contratoInfo.U_CVA_GRPSERVICO + " - " + contratoInfo.U_CVA_DES_GRPSERVICO;

            //        obj.Qty = contratoInfo.ComensaisDia;

            //        obj.Itens = contratoInfo.Itens;

            //        foreach (var item in obj.Itens)
            //        {
            //            item.U_CVA_INSUMO_DES = item.U_CVA_INSUMO + " - " + item.U_CVA_INSUMO_DES;
            //            item.U_CVA_TIPO_PRATO_DES = item.U_CVA_TIPO_PRATO + " - " + item.U_CVA_TIPO_PRATO_DES;

            //            if (obj.Qty > 0)
            //            {
            //                item.QtyPlanejado = (obj.Qty * item.U_CVA_PERCENT) / 100;
            //                item.QtyConsumido = item.QtyPlanejado;
            //            }

            //        }

            //        if(contratoInfo.Services.Count() > 0)
            //        {
            //            var list = new List<ComboBoxModel>();
            //            foreach (var item in contratoInfo.Services)
            //                list.Add(new ComboBoxModel() { Code = item.U_CVA_ID_SERVICO, Name = item.U_CVA_ID_SERVICO + " - " + item.U_CVA_D_SERVICO });

            //            obj.ServicoList = new SelectList(list, "Code", "Name");
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(obj.GrupoServicoCode))
            //            {
            //                obj.ServicoList = await GetServicoSelectAsync(obj.GrupoServicoCode);

            //                if (string.IsNullOrEmpty(obj.ServicoCode))
            //                {
            //                    if (obj.ServicoList.Count() > 0)
            //                        obj.ServicoCode = obj.ServicoList.FirstOrDefault().Value;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
            
            SetTempAlert();

            return View(obj);
        }

        [CvaAuthorize("Apontamento")]
        public async System.Threading.Tasks.Task<ActionResult> Painel(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Apontamento");

            var splitValue = id.Split('|');

            var obj = new ApontamentoModel();
            obj.BPLIdCode = splitValue[0];
            obj.ClienteCode = splitValue[1];
            obj.ClienteName = splitValue[2];
            obj.apontamentoData = Convert.ToDateTime(splitValue[3]);
            obj.ServicoCode = splitValue[4];
            //obj.ContratoCode = splitValue[5];

            TempData["painelToApontamento"] = obj;

            return RedirectToAction("Index", "Apontamento");
        }

        //[CvaAuthorize("Apontamento")]
        // POST: ReposicaoInsumos/Create
        [HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult> Create(ApontamentoModel obj)
        public async System.Threading.Tasks.Task<JsonResult> Create(ApontamentoModel obj)
        {
            var ret = new ReturnModel(); 
            try
            {

                if (string.IsNullOrEmpty(obj.BPLIdCode))
                    throw new Exception($"Filial não foi identificado.");

                if (string.IsNullOrEmpty(obj.ContratoCode))
                    throw new Exception($"Ordem de produção não foi identificado.");

                if (obj.apontamentoData == null)
                    throw new Exception($"Data não foi identificado.");

                if (string.IsNullOrEmpty(obj.OrderCode))
                    throw new Exception($"Ordem de produção para efetivar não foi identificado.");

                if (string.IsNullOrEmpty(obj.ClienteCode))
                    throw new Exception($"Cliente não foi identificado.");
                
                if (string.IsNullOrEmpty(obj.ServicoCode))
                    throw new Exception($"Serviço não foi identificado.");

                if (obj?.BPs?.Count == 0)
                    throw new Exception($"Nenhum Terceiros foi encontrado.");

                if(obj.apontamentoData > DateTime.Now)
                {
                    throw new Exception($"A data do apontamento deve ser menor ou igual a data atual ({DateTime.Now:dd/MM/yyyy}) do sistema.");
                }

                if (obj?.Itens?.Count > 0)
                {
                    var intOrder = Convert.ToInt32(obj.OrderCode);
                    obj.Itens = obj.Itens.Where(x => x.DocEntry == intOrder && x.Delete == false).ToList();
                    obj.BPs = obj.BPs.Where(x => x.Remove == false).ToList();

                    obj.Itens.RemoveAll(x => x.DocEntry == intOrder && x.Delete == true);
                    obj.BPs.RemoveAll(x => x.Remove == true);

                    obj.UserCode = CvaSession.Usuario.Usuario;
                    var error = await api.PostAsync("Apontamento", obj);
                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                    else
                    {
                        ret.Sucess = true;
                        ret.Message = "Apontamento cadastrado com sucesso.";
                    }
                }
                else
                {
                    ret.Sucess = false;
                    ret.Message = "Nenhum item foi encontrado";
                }
                                
            }
            catch (Exception ex)
            {
                ret.Sucess = false;
                ret.Message = ex.Message;
            }
           
            //return RedirectToAction("Index");
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task<ApontamentoModel> CheckListNullAsync(ApontamentoModel obj)
        {
            if (obj.BPLIdList == null && !string.IsNullOrEmpty(obj.BPLIdCode))
                obj.BPLIdList = await GetBPLIdAsync();

            if (obj.ContratoList == null && !string.IsNullOrEmpty(obj.ContratoCode))
                obj.ContratoList = await GetOpenOrdersAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value, DateTime.Now);

            //VERIFICAR
            //if (obj.ServicoList == null && !string.IsNullOrEmpty(obj.ServicoCode) && !string.IsNullOrEmpty(obj.GrupoServicoCode))
            //    obj.ServicoList = await GetServicoSelectAsync(obj.GrupoServicoCode);

            return obj;
        }

        public void SetTempAlert()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"].ToString();
        }
    }
}
