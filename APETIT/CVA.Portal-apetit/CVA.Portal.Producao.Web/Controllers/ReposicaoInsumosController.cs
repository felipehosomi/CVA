using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    
    public class ReposicaoInsumosController : Controller
    {
        APICallUtil api = new APICallUtil();

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
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInsumo(string BPLId)
        {
            var retlist = await GetInsumoAsync(BPLId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetInsumoAsync(string BPLId)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("ReposicaoInsumos", $"GetInsumos?bplid={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }

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

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetMotivo(string BPLId)
        {
            var retlist = await GetMotivoAsync(BPLId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetMotivoAsync(string BPLId)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("ReposicaoInsumos", $"GetMotivo?motivoBplid={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }

        // GET: ReposicaoInsumos
        [CvaAuthorize("ReposicaoInsumos")]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new ReposicaoInsumoModel();

            if (TempData["modelReposicaoInsumos"] != null)
                obj = TempData["modelReposicaoInsumos"] as ReposicaoInsumoModel;

            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();
            
            //if (obj.ContratoList == null && obj.BPLIdList?.Count() > 0)
            //    obj.ContratoList = await GetContratoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value);
            
            //if (obj.InsumoList == null && obj.BPLIdList?.Count() > 0)
            //    obj.InsumoList = await GetInsumoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value);

            if (obj.MotivoList == null && obj.BPLIdList?.Count() > 0)
                obj.MotivoList = await GetMotivoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList.FirstOrDefault().Value);

            SetTempAlert();

            return View(obj);
        }

        [CvaAuthorize("ReposicaoInsumos")]
        // POST: ReposicaoInsumos/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(ReposicaoInsumoModel obj)
        {
            try
            {
                if (obj.Itens.Count > 0)
                    obj.Itens = obj.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

                if (string.IsNullOrEmpty(obj.BPLIdCode))
                    throw new Exception($"Filial não foi identificado.");

                if (string.IsNullOrEmpty(obj.ClienteCode))
                    throw new Exception($"Cliente não foi identificado.");

                if (string.IsNullOrEmpty(obj.MotivoCode))
                    throw new Exception($"Motivo da Solicitação não foi identificado.");

                if (string.IsNullOrEmpty(obj.observacao))
                    throw new Exception($"Observação não foi identificado.");

                //if (obj?.Anexo == null || obj?.Anexo?.Count() <= 0 || obj?.Anexo?.First() == null)
                //    throw new Exception($"Anexo não foi identificado.");

                if (obj?.Itens?.Count > 0)
                {
                    foreach (var item in obj.Itens)
                    {
                        if (string.IsNullOrEmpty(item.InsumoCode))
                            throw new Exception($"Nº do Item  não foi identificado.");

                        if (string.IsNullOrEmpty(item.InsumoName))
                            throw new Exception($"Descrição não foi identificado.");

                        if (item.Qty <=0)
                            throw new Exception($"Quantidade não foi identificado.");

                        if (string.IsNullOrEmpty(item.DtNecessidade.ToString()))
                            throw new Exception($"Data da Necessidade não foi identificado.");
                    }

                    obj.UserCode = CvaSession.Usuario.Usuario;

                    var listOfAttach = new List<AttachmentsAPI>();
                    foreach (var itemAttach in obj.Anexo)
                    {
                        if(itemAttach != null)
                            listOfAttach.Add(new AttachmentsAPI
                            {
                                attachmentByte = ConvertToBytes(itemAttach),
                                type = itemAttach.ContentType
                            });
                    }

                    var objNew = new ReposicaoInsumoAPIModel()
                    {
                        BPLIdCode = obj.BPLIdCode,
                        BPLIdList = obj.BPLIdList,
                        ClienteCode = obj.ClienteCode,
                        ClienteName = obj.ClienteName,
                        MotivoCode = obj.MotivoCode,
                        MotivoList = obj.MotivoList,
                        observacao = obj.observacao,
                        InsumoList = obj.InsumoList,
                        Itens = obj.Itens,
                        UserCode = CvaSession.Usuario.Usuario,
                        Anexo = listOfAttach
                    };

                    var error = await api.PostAsync("ReposicaoInsumos", objNew);
                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                    else
                        TempData["Success"] = "Solicitação foi enviada com sucesso.";
                }
                else
                    TempData["Error"] = "Nenhum item foi informado.";

                //TempData["modelReposicaoInsumos"] = (TempData["Error"] != null ? obj : null);
                TempData["modelReposicaoInsumos"] = (TempData["Success"] != null ? null : obj);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["modelReposicaoInsumos"] = obj;
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

        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            int fileSizeInBytes = file.ContentLength;
            byte[] data = null;
            using (var br = new BinaryReader(file.InputStream))
            {
                data = br.ReadBytes(fileSizeInBytes);
            }

            return data;
        }
    }
}
