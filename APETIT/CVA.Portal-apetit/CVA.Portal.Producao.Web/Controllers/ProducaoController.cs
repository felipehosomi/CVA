using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Model.Estoque;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("Producao")]
    public class ProducaoController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region Index
        public async Task<ActionResult> Index()
        {
            /**/
            return RedirectToAction("Menu", "Apetit");
            /**/

            DateTime dataFiltro = DateTime.Today;
            ViewBag.DataDe = dataFiltro.ToString("yyyy-MM-dd");
            ViewBag.DataAte = dataFiltro.ToString("yyyy-MM-dd");

            ViewBag.Percentual = await api.GetAsync<string>("ProducaoPercentual", $"dataDe={dataFiltro.ToString("yyyy-MM-dd")}&dataAte={dataFiltro.ToString("yyyy-MM-dd")}&nrOP=&nrPedido=", "?");

            List<UsuarioEtapaModel> etapaList = await api.GetListAsync<UsuarioEtapaModel>("UsuarioEtapa", $"codUsuario={CvaSession.Usuario.Code}");
            etapaList = etapaList.Where(s => s.SelectedInt == 1).ToList();
            ViewBag.Etapa = new SelectList(etapaList, "CodEtapa", "CodEtapa");
            List<ProducaoModel> list = await api.GetListAsync<ProducaoModel>("Producao", $"codUsuario={CvaSession.Usuario.Usuario}&dataDe={dataFiltro.ToString("yyyy-MM-dd")}&dataAte={dataFiltro.ToString("yyyy-MM-dd")}&nrOP=&nrPedido=&itemDesc=&etapa=");
            Session[$"OP_{CvaSession.Usuario.Usuario}"] = list;
            return View(list);
        }

        public async Task<ActionResult> IndexFiltro(string dataDe, string dataAte, string nrOP, string nrPedido, string item, string etapa)
        {
            ViewBag.DataDe = dataDe;
            ViewBag.DataAte = dataAte;
            ViewBag.NrOP = nrOP;
            ViewBag.NrPedido = nrPedido;
            ViewBag.Item = item;

            List<UsuarioEtapaModel> etapaList = await api.GetListAsync<UsuarioEtapaModel>("UsuarioEtapa", $"codUsuario={CvaSession.Usuario.Code}");
            etapaList = etapaList.Where(s => s.SelectedInt == 1).ToList();
            ViewBag.Etapa = new SelectList(etapaList, "CodEtapa", "CodEtapa", etapa);

            ViewBag.Percentual = await api.GetAsync<string>("ProducaoPercentual", $"dataDe={dataDe}&dataAte={dataAte}&nrOP={nrOP}&nrPedido={nrPedido}", "?");
            List<ProducaoModel> list = await api.GetListAsync<ProducaoModel>("Producao", $"codUsuario={CvaSession.Usuario.Usuario}&dataDe={dataDe}&dataAte={dataAte}&nrOP={nrOP}&nrPedido={nrPedido}&itemDesc={item}&etapa={etapa}");
            Session[$"OP_{CvaSession.Usuario.Usuario}"] = list;
            return View("Index", list);
        }

        public async Task<ActionResult> TabelaOrdensProducao(string data)
        {
            DateTime dataFiltro = DateTime.Today;
            DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataFiltro);

            ViewBag.Percentual = await api.GetAsync<string>("ProducaoPercentual", $"data={dataFiltro.ToString("yyyy-MM-ddTHH:mm:ss")}", "?");
            return PartialView(await api.GetListAsync<ProducaoModel>("Producao", $"data={dataFiltro.ToString("yyyy-MM-ddTHH:mm:ss")}"));
        }
        #endregion

        #region Historico
        public async Task<ActionResult> ModalHistorico(int docEntry)
        {
            List<ProducaoModel> list = Session[$"OP_{CvaSession.Usuario.Usuario}"] as List<ProducaoModel>;
            ProducaoModel producaoModel;
            if (list != null)
            {
                producaoModel = list.FirstOrDefault(m => m.DocEntry == docEntry);
            }
            else
            {
                producaoModel = new ProducaoModel();
                producaoModel.ItemCode = "Sessão perdida, por favor refaça o login";
            }

            producaoModel.Historico = await api.GetListAsync<HistoricoProducaoModel>("HistoricoProducao", $"docEntry={docEntry}");
            return PartialView(producaoModel);
        }
        #endregion

        #region Apontamento
        public async Task<ActionResult> ModalEstruturaProducao(int nrOP, int codEtapa)
        {
            ModelState.Clear();

            ProducaoModel producaoModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={nrOP}&codEtapa={codEtapa}", paramSeparator: "?");
            producaoModel.Concluir = true;
            producaoModel.Estrutura = await api.GetListAsync<EstruturaProducaoModel>("EstruturaProducao", $"nrOP={nrOP}&codEtapa={codEtapa}");

            return PartialView(producaoModel);
        }

        [HttpPost]
        public async Task<JsonResult> Apontamento(ProducaoModel model)
        {
            try
            {
                var opModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={model.NrOP}&codEtapa={model.CodEtapa}", paramSeparator: "?");

                if (opModel != null && Convert.ToDecimal(opModel.Quantidade) == Convert.ToDecimal(opModel.QtdeRealizada))
                {
                    throw new Exception("Quantidade planejada igual a quantidade realizada");
                }

                foreach (var item in model.Estrutura)
                {
                    if (item.QtdeRealizada > 0)
                    {
                        if (item.ControlePorLote == "Y" || item.ControlePorSerie == "Y")
                        {
                            item.LoteSerie = System.Web.HttpContext.Current.Session[$"CVA_LoteSerie_{CvaSession.Usuario.Code}_{item.DocEntry}_{item.LineNum}"] as List<LoteSerieModel>;
                            if (item.ControlePorSerie == "Y")
                            {
                                if (item.LoteSerie == null || item.LoteSerie.Count(m => m.Selecionado) != item.QtdeRealizada)
                                {
                                    throw new Exception("Quantidade de números de série deve ser a mesma da quantidade realizada");
                                }

                                item.LoteSerie = item.LoteSerie.Where(m => m.Selecionado).ToList();
                            }
                            if (item.ControlePorLote == "Y")
                            {
                                if (item.LoteSerie == null || Convert.ToDecimal(item.LoteSerie.Sum(m => m.Quantidade)) != Convert.ToDecimal(item.QtdeRealizada))
                                {
                                    throw new Exception("Quantidade dos lotes deve ser a mesma da quantidade realizada");
                                }

                                item.LoteSerie = item.LoteSerie.Where(m => m.Quantidade > 0).ToList();
                            }
                        }
                    }
                }

                string retorno = await api.PostAsync<ProducaoModel>("SaidaInsumos", model);
                // Se não ocorreu erro, limpas os dados do lote/série na session 
                if (String.IsNullOrEmpty(retorno))
                {
                    foreach (var item in model.Estrutura)
                    {
                        System.Web.HttpContext.Current.Session[$"CVA_LoteSerie_{CvaSession.Usuario.Code}_{item.DocEntry}_{item.LineNum}"] = null;
                    }
                }
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region LoteSerie
        public async Task<ActionResult> ModalLoteSerie(int docEntry, int lineNum, string codMP, string descMP, double qtde, string lote, string serie, string deposito)
        {
            try
            {
                string controle = "N";
                if (lote == "Y")
                {
                    controle = "L";
                }
                else if (serie == "Y")
                {
                    controle = "S";
                }

                EstruturaProducaoModel model = new EstruturaProducaoModel();
                model.DescMP = codMP + " - " + descMP;
                model.QtdeRealizada = qtde;
                model.ControlePorLote = lote;
                model.ControlePorSerie = serie;

                model.LoteSerie = System.Web.HttpContext.Current.Session[$"CVA_LoteSerie_{CvaSession.Usuario.Code}_{docEntry}_{lineNum}"] as List<LoteSerieModel>;

                if (model.LoteSerie == null)
                {
                    model.LoteSerie = await api.GetListAsync<LoteSerieModel>("LoteSerie", $"itemCode={codMP}&controle={controle}&deposito={deposito}");
                }
                else
                {
                    if (controle == "L")
                    {
                        model.QtdeSelecionada = Convert.ToDouble(Convert.ToDecimal(model.LoteSerie.Sum(m => m.Quantidade)));
                    }
                    else
                    {
                        model.QtdeSelecionada = model.LoteSerie.Count(m => m.Selecionado);
                    }
                }
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoteSerie(EstruturaProducaoModel model)
        {
            try
            {
                if (model.ControlePorLote == "Y")
                {
                    if (Convert.ToDecimal(model.QtdeRealizada) != Convert.ToDecimal(model.LoteSerie.Sum(m => m.Quantidade)))
                    {
                        throw new Exception("Somatório da quantidade informada do lote deve ser a mesma que a quantidade realizada");
                    }
                }
                else if (model.ControlePorSerie == "Y")
                {
                    if (model.QtdeRealizada != model.LoteSerie.Count(m => m.Selecionado))
                    {
                        throw new Exception("Quantidade informada de números de série deve ser a mesma que a quantidade realizada");
                    }
                }

                System.Web.HttpContext.Current.Session[$"CVA_LoteSerie_{CvaSession.Usuario.Code}_{model.DocEntry}_{model.LineNum}"] = model.LoteSerie;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CalculaLote(EstruturaProducaoModel model)
        {
            try
            {
                return Json(Convert.ToDecimal(model.LoteSerie.Sum(m => m.Quantidade)).ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Apontamento OP alternativo
        public ActionResult ApontamentoOPMenu()
        {
            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View();
        }

        public async Task<ActionResult> ApontamentoOPSelecao()
        {
            var opList = await api.GetListAsync<ProducaoModel>("Producao/GetListRecursosByUsuario", $"codUsuario={CvaSession.Usuario.Code}");

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View(opList);
        }

        [HttpPost]
        public ActionResult ApontamentoOPSelecaoConfirmar(List<ProducaoModel> ops)
        {
            try
            {
                if (ops != null && ops.Count > 0 && ops.Any(x => x.Checked))
                {
                    var opsSelecionadas = ops.Where(x => x.Checked).ToList();
                    ModelState.Clear();
                    return View(opsSelecionadas);
                }
                else
                {
                    throw new Exception("Nenhuma OP selecionada");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ApontamentoOPMenu");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ApontamentoOPSelecaoAdd(List<ProducaoModel> opsSelecionadas)
        {
            try
            {
                string retorno = string.Empty;
                retorno = await api.PostAsync("SaidaEtapa", opsSelecionadas);

                if (string.IsNullOrEmpty(retorno))
                {
                    TempData["SuccessMessage"] = "Apontamento realizado com sucesso.";
                }
                else
                {
                    throw new Exception(retorno);
                }
                return RedirectToAction("ApontamentoOPMenu");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ApontamentoOPMenu");
            }
        }

        public ActionResult ApontamentoOPPesquisaTrans()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ApontamentoOPPesquisaTransResultado(string barcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barcode) && barcode.Length == 13)
                {
                    int docNum = 0, stageId = 0, lineNum = 0;
                    int.TryParse(barcode.Substring(0, 6), out docNum);
                    int.TryParse(barcode.Substring(6, 3), out stageId);
                    int.TryParse(barcode.Substring(9, 4), out lineNum);

                    var transferencia = await api.GetAsync<TransferenciaEstoqueModel>("TransferenciaEstoque",
                        $"GetByOPDocNumStageId?opDocNum={docNum}&stageId={stageId}");

                    if (transferencia.DocNum > 0)
                        return View(transferencia);
                    else
                        throw new Exception("Solicitação de transferência não encontrada.");
                }

                throw new Exception("Código de barras inválido.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApontamentoOPPesquisaTrans");
            }
        }

        public async Task<ActionResult> ApontamentoOPIniciar()
        {
            var opList = await api.GetListAsync<ProducaoModel>("Producao/GetListRecursosByUsuario", $"codUsuario={CvaSession.Usuario.Code}");
            ViewBag.OpList = opList;

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View();
        }

        public async Task<ActionResult> ApontamentoOPFinalizar()
        {
            var apontamentos = await api.GetListAsync<ApontamentoHRModel>("ApontamentoHR/GetApontamentosAbertos", $"usuario={CvaSession.Usuario.Usuario}");

            if (apontamentos == null || apontamentos.Count == 0)
            {
                TempData["Error"] = "Você não possui um apontamento iniciado para finalizar.";
                return RedirectToAction("ApontamentoOPMenu");
            }

            foreach (var apontamento in apontamentos)
            {
                var producaoModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={apontamento.OPDocNum}&codEtapa={apontamento.CodEtapa}", paramSeparator: "?");
                apontamento.OP = producaoModel;
            }

            ViewBag.Apontamentos = apontamentos;

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View();
        }

        public async Task<ActionResult> ApontamentoOPConfirmarFinalizacao(string barcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barcode) && barcode.Length == 13)
                {
                    int docNum = 0, stageId = 0, lineNum = 0;
                    int.TryParse(barcode.Substring(0, 6), out docNum);
                    int.TryParse(barcode.Substring(6, 3), out stageId);
                    int.TryParse(barcode.Substring(9, 4), out lineNum);

                    var apontamento = await api.GetAsync<ApontamentoHRModel>("ApontamentoHR",
                        $"GetApontamentoAberto?usuario={CvaSession.Usuario.Usuario}&opDocNum={docNum}&codEtapa={stageId}&recursoLineNum={lineNum}");

                    if (apontamento == null || string.IsNullOrEmpty(apontamento.Code))
                    {
                        throw new Exception("Registro não encontrado.");
                    }

                    var producaoModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={docNum}&codEtapa={stageId}", paramSeparator: "?");

                    if (producaoModel == null || producaoModel.NrOP == 0)
                    {
                        throw new Exception("Registro não encontrado.");
                    }

                    apontamento.OP = producaoModel;
                    return View(apontamento);
                }

                throw new Exception("Código de barras inválido.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApontamentoOPFinalizar");
            }
        }

        public async Task<ActionResult> ApontamentoOPConfirmarInicio(string barcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barcode) && barcode.Length == 13)
                {
                    int docNum = 0, stageId = 0, lineNum = 0;
                    int.TryParse(barcode.Substring(0, 6), out docNum);
                    int.TryParse(barcode.Substring(6, 3), out stageId);
                    int.TryParse(barcode.Substring(9, 4), out lineNum);

                    // Verifica se já existe um apontamento iniciado para o código informado
                    var apontamento = await api.GetAsync<ApontamentoHRModel>("ApontamentoHR",
                        $"GetApontamentoAberto?usuario={CvaSession.Usuario.Usuario}&opDocNum={docNum}&codEtapa={stageId}&recursoLineNum={lineNum}");

                    if (apontamento != null && !string.IsNullOrEmpty(apontamento.Code))
                    {
                        throw new Exception("Já existe um apontamento iniciado para este código de barras.");
                    }

                    var producaoModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={docNum}&codEtapa={stageId}", paramSeparator: "?");

                    if (producaoModel == null || producaoModel.NrOP == 0)
                    {
                        throw new Exception("Registro não encontrado.");
                    }

                    producaoModel.BarCode = barcode;
                    producaoModel.Recursos = await api.GetListAsync<RecursoProducaoModel>("RecursoProducao", $"nrOP={docNum}&codEtapa={stageId}");

                    if (producaoModel.Recursos != null)
                        producaoModel.Recursos = producaoModel.Recursos.Where(x => x.LineNum == lineNum).ToList();

                    if (producaoModel.Recursos != null && producaoModel.Recursos.Count > 0)
                    {
                        producaoModel.RecursoCode = producaoModel.Recursos[0].VisResCode;
                        producaoModel.RecursoName = producaoModel.Recursos[0].ResName;
                        producaoModel.RecursoLineNum = producaoModel.Recursos[0].LineNum;
                    }
                    else
                    {
                        throw new Exception("Registro não encontrado.");
                    }

                    return View(producaoModel);
                }

                throw new Exception("Código de barras inválido.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApontamentoOPIniciar");
            }
        }

        public ActionResult ApontamentoOPTransIniciar()
        {
            return View();
        }

        public async Task<ActionResult> ApontamentoOPTransConfirmar()
        {
            try
            {
                var ops = TempData["ListaOps"] as List<ProducaoModel>;

                if (ops == null)
                    return View("ApontamentoOPTransIniciar");

                foreach (var op in ops)
                {
                    op.Itens = await api.GetListAsync<ItemOPModel>("ItemOP", $"opDocEntry={op.DocEntry}&stageId={op.CodEtapa}");

                    // Só são transferidos itens onde o depósito na linha da OP difere do depósito padrão do item
                    if (op.Itens != null)
                        op.Itens = op.Itens.Where(x => x.wareHouse != x.DefaultWareHouse).ToList();

                    if (op.Itens == null || op.Itens.Count == 0)
                        throw new Exception($"A etapa {op.Etapa} da ordem {op.NrOP} não possui itens que requerem transferência.");
                }

                return View(ops);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ApontamentoOPTransIniciar");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ApontamentoOPTransPost(ProducaoModel op)
        {
            try
            {
                string[] barcodesInput = op.BarCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var barcodes = new List<string>();
                var ops = new List<ProducaoModel>();

                for (int i = 0; i < barcodesInput.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(barcodesInput[i]))
                    {
                        if (barcodesInput[i].Trim().Length == 13)
                        {
                            // Ignora o recurso do código de barras. Considera apenas OP + itinerário
                            string shortBarcode = barcodesInput[i].Trim().Substring(0, 9);

                            if (!barcodes.Contains(shortBarcode))
                                barcodes.Add(shortBarcode);
                        }
                        else
                        {
                            throw new Exception($"Código de barras {barcodesInput[i]} inválido.");
                        }
                    }
                }

                foreach (string barcode in barcodes)
                {
                    int docNum = 0, stageId = 0;
                    int.TryParse(barcode.Trim().Substring(0, 6), out docNum);
                    int.TryParse(barcode.Trim().Substring(6, 3), out stageId);

                    var producaoModel = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={docNum}&codEtapa={stageId}", paramSeparator: "?");

                    if (producaoModel == null || producaoModel.NrOP == 0)
                    {
                        throw new Exception($"Registro {barcode.Trim()} não encontrado.");
                    }

                    ops.Add(producaoModel);
                }

                if (ops.Count == 0)
                {
                    throw new Exception("Registro não encontrado.");
                }

                TempData["ListaOps"] = ops;
                return RedirectToAction("ApontamentoOPTransConfirmar");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ApontamentoOPTransIniciar");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ApontamentoOPTransAdd(List<ProducaoModel> ops)
        {
            try
            {
                string retorno = string.Empty;
                retorno = await api.PostAsync($"TransferenciaEstoque?usuario={CvaSession.Usuario.Usuario}", ops);

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception(retorno);

                TempData["SuccessMessage"] = "Solicitação de transferência lançada com sucesso.";
                return RedirectToAction("ApontamentoOPMenu");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("ApontamentoOPMenu");
            }
        }

        public async Task<ActionResult> ApontamentoOPAddApontamentoHR(ProducaoModel op)
        {
            string retorno = string.Empty;

            try
            {
                var apontamento = new ApontamentoHRModel();
                apontamento.OPDocEntry = op.DocEntry;
                apontamento.OPDocNum = op.NrOP;
                apontamento.CodEtapa = op.CodEtapa;
                apontamento.NomeEtapa = op.Etapa;
                apontamento.RecursoCod = op.RecursoCode;
                apontamento.RecursoNome = op.RecursoName;
                apontamento.RecursoLineNum = op.RecursoLineNum;
                apontamento.CodUsuario = CvaSession.Usuario.Usuario;
                apontamento.StartDateTimeFormat = DateTime.Now;

                retorno = await api.PostAsync("ApontamentoHR", apontamento);

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception(retorno);

                TempData["SuccessMessage"] = "Apontamento iniciado com sucesso.";
                return RedirectToAction("ApontamentoOPMenu");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApontamentoOPMenu");
            }
        }

        public async Task<ActionResult> ApontamentoOPCloseApontamentoHR(ApontamentoHRModel apontamento)
        {
            try
            {
                var recursosOP = await api.GetListAsync<RecursoProducaoModel>("RecursoProducao", $"nrOP={apontamento.OPDocNum}&codEtapa={apontamento.CodEtapa}");

                if (recursosOP != null)
                    recursosOP = recursosOP.Where(x => x.LineNum == apontamento.RecursoLineNum).ToList();

                if (recursosOP != null && recursosOP.Count > 0 && !string.IsNullOrEmpty(recursosOP[0].CentroCusto))
                    apontamento.OcrCode = recursosOP[0].CentroCusto;

                apontamento.EndDateTimeFormat = DateTime.Now;
                apontamento.Duration = (apontamento.EndDateTimeFormat - apontamento.StartDateTimeFormat).TotalMinutes;

                string retorno = await api.PutAsync("ApontamentoHR", apontamento.Code, apontamento);

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception(retorno);

                retorno = await api.PostAsync("SaidaRecurso", apontamento);

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception(retorno);

                TempData["SuccessMessage"] = "Apontamento finalizado com sucesso.";
                return RedirectToAction("ApontamentoOPMenu");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApontamentoOPMenu");
            }
        }
        #endregion
    }
}
