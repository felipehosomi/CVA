using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Model.Qualidade;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class FichaInspecaoController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region CreateOP
        public async Task<ActionResult> CreateManual(int nrOP, string codEtapaOP)
        {
            ProducaoModel model = await api.GetAsync<ProducaoModel>("Producao", $"nrOP={nrOP}&codEtapa={codEtapaOP}", "?");
            FichaInspecaoModel fichaInspecaoModel = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", $"codItem={model.ItemCode}&codEtapa={model.Etapa}", "?");

            fichaInspecaoModel.DocNum = model.NrOP;
            fichaInspecaoModel.DocEntry = model.DocEntry;
            fichaInspecaoModel.NrNF = model.NrPedido;
            fichaInspecaoModel.TipoDoc = "OP";
            fichaInspecaoModel.DataDoc = model.DataOP;
            fichaInspecaoModel.CodItem = model.ItemCode;
            fichaInspecaoModel.DescItem = model.ItemName;
            fichaInspecaoModel.Quantidade = 1;
            fichaInspecaoModel.QtdeAnalisada = 1;

            fichaInspecaoModel.DataInspStr = DateTime.Today.ToString("yyyy-MM-dd");
            fichaInspecaoModel.CodUsuario = CvaSession.Usuario.Usuario;

            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");

            int i = 1;
            if (fichaInspecaoModel.ItemList != null)
            {
                foreach (var item in fichaInspecaoModel.ItemList)
                {
                    item.ID = i;
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                    i++;
                }
            }
            else if (fichaInspecaoModel.AmostraList != null)
            {
                foreach (var amostraModel in fichaInspecaoModel.AmostraList)
                {
                    foreach (var item in amostraModel.ItemList)
                    {
                        item.ID = i;
                        item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                        i++;
                    }
                }
            }

            this.GetStatusSelectLists();

            return View("Create", fichaInspecaoModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateManual(FichaInspecaoModel model)
        {
            try
            {
                model.DataInsp = DateTime.ParseExact(model.DataInspStr, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                string error = await api.PostAsync<FichaInspecaoModel>("FichaInspecao", model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index", "InspecaoOP");
            }
            catch (Exception ex)
            {
                this.GetStatusSelectLists(model.Status, model.StatusLote);
                ModelState.AddModelError("", ex);
                return View("CreateManual", model);
            }
        }

        public async Task<ActionResult> CreateOP(FichaInspecaoModel model)
        {
            ModelState.Clear();
            FichaInspecaoModel fichaInspecaoModel = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", $"docEntryOP={model.DocEntry}&codItem={model.CodItem}&codEtapa={model.CodEtapa}&modal=0", "?");
            model.TipoDoc = "OP";
            model.QtdeAnalisada = 1;
            model.DataInspStr = DateTime.Today.ToString("yyyy-MM-dd");
            model.CodUsuario = CvaSession.Usuario.Usuario;
            model.Sequencia = fichaInspecaoModel.Sequencia;
            model.SequenciaParcial = fichaInspecaoModel.SequenciaParcial;
            model.QtdeSeq = fichaInspecaoModel.QtdeSeq;
            model.CodModelo= fichaInspecaoModel.CodModelo;
            model.DescModelo = fichaInspecaoModel.DescModelo;
            model.DescEtapa = fichaInspecaoModel.DescEtapa;

            model.ItemList = fichaInspecaoModel.ItemList;
            model.AmostraList = fichaInspecaoModel.AmostraList;

            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");

            int i = 1;
            if (model.ItemList != null)
            {
                foreach (var item in model.ItemList)
                {
                    item.ID = i;
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                    i++;
                }
            }
            else if (model.AmostraList != null)
            {
                foreach (var amostraModel in model.AmostraList)
                {
                    foreach (var item in amostraModel.ItemList)
                    {
                        item.ID = i;
                        item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                        i++;
                    }
                }
            }

            this.GetStatusSelectLists();
            return View("Create", model);
        }

        [HttpPost]
        [ActionName("CreateOP")]
        public async Task<ActionResult> CreateOPPost(FichaInspecaoModel model)
        {
            try
            {
                model.DataInsp = DateTime.ParseExact(model.DataInspStr, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                string error = await api.PostAsync<FichaInspecaoModel>("FichaInspecao", model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }

                if (model.SequenciaParcial + 1 > model.QtdeSeq)
                {
                    return RedirectToAction("Index", "InspecaoOP");
                }
                else
                {
                    ModelState.Clear();
                    return RedirectToAction("CreateOP", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                return RedirectToAction("CreateOP", model);
            }
        }
        #endregion

        #region CreateModal
        [HttpPost]
        public async Task<ActionResult> GetModal(ProducaoModel model)
        {
            ModelState.Clear();

            FichaInspecaoModel fichaInspecaoModel = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", $"docEntryOP={model.DocEntry}&codItem={model.ItemCode}&codEtapa={model.Etapa}&modal=1", "?");
            fichaInspecaoModel.DocNum = model.NrOP;
            fichaInspecaoModel.DocEntry = model.DocEntry;
            fichaInspecaoModel.NrNF = model.NrPedido;
            fichaInspecaoModel.TipoDoc = "OP";
            fichaInspecaoModel.DataDoc = DateTime.ParseExact(model.Estrutura[0].DocDateStr, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            //fichaInspecaoModel.CodPN = model.CardCode;
            //fichaInspecaoModel.NomePN = model.CardName;
            fichaInspecaoModel.CodItem = model.ItemCode;
            fichaInspecaoModel.DescItem = model.ItemName;
            fichaInspecaoModel.Quantidade = model.Quantidade;

            fichaInspecaoModel.QtdeSeq = model.Estrutura[0].QtdeRealizada;
            fichaInspecaoModel.QtdeAnalisada = 1;

            fichaInspecaoModel.DataInspStr = DateTime.Today.ToString("yyyy-MM-dd");
            fichaInspecaoModel.CodUsuario = CvaSession.Usuario.Usuario;

            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");

            int i = 1;
            if (fichaInspecaoModel.ItemList != null)
            {
                foreach (var item in fichaInspecaoModel.ItemList)
                {
                    item.ID = i;
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
                    i++;
                }
            }
            else if (fichaInspecaoModel.AmostraList != null)
            {
                foreach (var amostraModel in fichaInspecaoModel.AmostraList)
                {
                    foreach (var item in amostraModel.ItemList)
                    {
                        item.ID = i;
                        item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
                        i++;
                    }
                }
            }

            this.GetStatusSelectLists(fichaInspecaoModel.Status, fichaInspecaoModel.StatusLote);
            return PartialView("CreatePartial", fichaInspecaoModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateModal(FichaInspecaoModel model)
        {
            string error = String.Empty;
            try
            {
                model.DataInsp = DateTime.ParseExact(model.DataInspStr, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                if (String.IsNullOrEmpty(model.StatusLote))
                {
                    throw new Exception($"Status deve ser informado");
                }

                if (model.ItemList != null)
                {
                    foreach (var item in model.ItemList)
                    {
                        if (item.Resultado == null || item.Resultado.Trim() == "")
                        {
                            throw new Exception($"Linha {item.ID} - Resultado deve ser informado");
                        }
                    }
                }
                else if (model.AmostraList != null)
                {
                    foreach (var amostraModel in model.AmostraList)
                    {
                        foreach (var item in amostraModel.ItemList)
                        {
                            if (item.Resultado == null || item.Resultado.Trim() == "")
                            {
                                throw new Exception($"Amostra {amostraModel.IdAmostra} - Linha {item.ID} - Resultado deve ser informado");
                            }
                        }
                    }
                }

                error = await api.PostAsync<FichaInspecaoModel>("FichaInspecao", model);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GoNext(FichaInspecaoModel model)
        {
            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");
            model.SequenciaParcial++;
            model.Sequencia++;
            foreach (var item in model.ItemList)
            {
                item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                item.CodRecurso = String.Empty;
                item.Resultado = String.Empty;
            }
            this.GetStatusSelectLists();
            ModelState.Clear();
            return PartialView("CreatePartial", model);
        }
        #endregion

        #region CreateRecebimento
        public async Task<ActionResult> CreateRecebimento(DocumentoModel model)
        {
            FichaInspecaoModel fichaInspecaoModel = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", $"codItem={model.ItemCode}&codEtapa=Recebimento de Matéria-Prima", "?");
            fichaInspecaoModel.DocNum = model.DocNum;
            fichaInspecaoModel.DocEntry = model.DocEntry;
            fichaInspecaoModel.NrNF = model.Serial;
            fichaInspecaoModel.TipoDoc = "RE";
            fichaInspecaoModel.DataDoc = model.DocDate;
            fichaInspecaoModel.CodPN = model.CardCode;
            fichaInspecaoModel.NomePN = model.CardName;
            fichaInspecaoModel.CodItem = model.ItemCode;
            fichaInspecaoModel.DescItem = model.ItemName;
            fichaInspecaoModel.Quantidade = model.Quantity;
            fichaInspecaoModel.QtdeAnalisada = model.Quantity;

            fichaInspecaoModel.DataInspStr = DateTime.Today.ToString("yyyy-MM-dd");

            fichaInspecaoModel.CodUsuario = CvaSession.Usuario.Usuario;

            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");

            int i = 1;
            if (fichaInspecaoModel.ItemList  != null)
            {
                foreach (var item in fichaInspecaoModel.ItemList)
                {
                    item.ID = i;
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                    i++;
                }
            }
            else if (fichaInspecaoModel.AmostraList != null)
            {
                foreach (var amostraModel in fichaInspecaoModel.AmostraList)
                {
                    foreach (var item in amostraModel.ItemList)
                    {
                        item.ID = i;
                        item.Recursos = new SelectList(recursoList, "ResCode", "ResName");
                        i++;
                    }
                }
            }
            

            this.GetStatusSelectLists();
            return View("Create", fichaInspecaoModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecebimento(FichaInspecaoModel model)
        {
            try
            {
                model.DataInsp = DateTime.ParseExact(model.DataInspStr, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                string error = await api.PostAsync<FichaInspecaoModel>("FichaInspecao", model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index", "InspecaoMP");
            }
            catch (Exception ex)
            {
                this.GetStatusSelectLists(model.Status, model.StatusLote);
                ModelState.AddModelError("", ex);
                return View("CreateRecebimento", model);
            }
        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(string code)
        {
            FichaInspecaoModel model = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", code);
            //model.DataDocStr = model.DataDoc.ToString("yyyy-MM-dd");
            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");
            if (model.ItemList != null)
            {
                foreach (var item in model.ItemList)
                {
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
                }
            }
            else if (model.AmostraList != null)
            {
                foreach (var amostraModel in model.AmostraList)
                {
                    foreach (var item in amostraModel.ItemList)
                    {
                        item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
                    }
                }
            }
            
            this.GetStatusSelectLists(model.Status, model.StatusLote);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(FichaInspecaoModel model)
        {
            try
            {
                model.DataInsp = DateTime.ParseExact(model.DataInspStr, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                string error = await api.PutAsync<FichaInspecaoModel>("FichaInspecao", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                if (model.TipoDoc == "RE")
                {
                    return RedirectToAction("Index", "InspecaoMP");
                }
                else
                {
                    return RedirectToAction("Index", "InspecaoOP");
                }
            }
            catch (Exception ex)
            {
                this.GetStatusSelectLists(model.Status, model.StatusLote);
                List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");
                foreach (var item in model.ItemList)
                {
                    item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
                }

                ModelState.AddModelError("", ex);
                return View("Create", model);
            }
        }
        #endregion

        #region Detail
        public async Task<ActionResult> Detail(string code)
        {
            FichaInspecaoModel model = await api.GetAsync<FichaInspecaoModel>("FichaInspecao", code);
            List<RecursoModel> recursoList = await api.GetListAsync<RecursoModel>("Recurso", "tipo=M");
            foreach (var item in model.ItemList)
            {
                item.Recursos = new SelectList(recursoList, "ResCode", "ResName", item.CodRecurso);
            }
            this.GetStatusSelectLists(model.Status, model.StatusLote);
            return View(model);
        }
        #endregion

        #region GetStatus
        public void GetStatusSelectLists(string status = "", string statusLote = "")
        {
            List<ComboBoxModel> list = new List<ComboBoxModel>();
            list.Add(new ComboBoxModel() { Code = "A", Name = "Aberto" });
            list.Add(new ComboBoxModel() { Code = "C", Name = "Cancelado" });
            list.Add(new ComboBoxModel() { Code = "F", Name = "Fechado" });
            ViewBag.Status = new SelectList(list, "Code", "Name", status);

            list = new List<ComboBoxModel>();
            list.Add(new ComboBoxModel() { Code = "E", Name = "Em Análise" });
            list.Add(new ComboBoxModel() { Code = "R", Name = "Reprovado" });
            list.Add(new ComboBoxModel() { Code = "A", Name = "Aprovado" });
            ViewBag.StatusLote = new SelectList(list, "Code", "Name", statusLote);
        }
        #endregion
    }
}
