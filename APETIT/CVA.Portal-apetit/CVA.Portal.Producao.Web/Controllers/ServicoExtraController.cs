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
    public class ServicoExtraController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region [ GetContrato ]
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
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("ServicoExtra", $"GetInsumos?bplid={BPLId}");

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
            var ret = await api.GetAsync<ServicoExtraModelGetItemOnHand>("SaidaMaterial", $"CheckItemOnHandBPLId?itemCode={itemCode}&bplId={BPLId}");
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [CvaAuthorize("ServicoExtra")]
        // GET: SaidaMateriais
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new ServicoExtraModel();

            if (TempData["modelServicoExtra"] != null)
            {
                obj = TempData["modelServicoExtra"] as ServicoExtraModel;

                if (obj?.Itens?.Count > 0)
                    obj.Itens = obj.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();
            }
                
            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();
            
            //if (obj.InsumoList == null && obj.BPLIdList?.Count() > 0)
            //    obj.InsumoList = await GetInsumoAsync(!string.IsNullOrEmpty(obj.BPLIdCode) ? obj.BPLIdCode : obj.BPLIdList?.Where(x => x.Value != string.Empty).FirstOrDefault().Value);

            if (obj.Dt == DateTime.MinValue)
                obj.Dt = DateTime.Now;

            //obj.Itens.Add(new ServicoExtraItensModel() { InsumoCode = "ABA", InsumoName = "ABCCC", Qty = 1, idList = 0, Delete = false });

            SetTempAlert();

            return View(obj);
        }


        [CvaAuthorize("ServicoExtra")]
        // POST: SaidaMateriais/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(ServicoExtraModel obj)
        {
            try
            {
                if (obj?.Itens?.Count > 0)
                    obj.Itens = obj.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

                if (string.IsNullOrEmpty(obj.BPLIdCode))
                    throw new Exception($"Filial não foi identificado.");

                if (string.IsNullOrEmpty(obj.ClienteCode))
                    throw new Exception($"Cliente não foi identificado.");

                if (obj?.Anexo == null || obj?.Anexo?.Count() <= 0 || obj?.Anexo?.First() == null)
                    throw new Exception($"Anexo não foi identificado.");

                if (obj.Dt == null)
                    throw new Exception($"Data não foi identificado.");


                if (obj?.Itens?.Count > 0)
                {
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

                    var objNew = new ServicoExtraAPIModel() {
                        BPLIdCode = obj.BPLIdCode,
                        BPLIdList = obj.BPLIdList,
                        Dt = obj.Dt,
                        ClienteCode = obj.ClienteCode,
                        ClienteName = obj.ClienteName,
                        InsumoList = obj.InsumoList,
                        Itens = obj.Itens,
                        UserCode = CvaSession.Usuario.Usuario,
                        Anexo = listOfAttach
                    };

                    var error = await api.PostAsync("ServicoExtra", objNew);
                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                    else
                        TempData["Success"] = "Serviço Extra cadastrado com sucesso.";
                }
                else
                    TempData["Error"] = "Nenhum item foi informado.";

                TempData["modelServicoExtra"] = (TempData["Success"] != null ? null : obj);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["modelServicoExtra"] = obj;
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        public async System.Threading.Tasks.Task<string> checkItemServExtra(string client)
        {
            string retorno = await api.GetAsync<string>("ServicoExtra", $"client={client}", "?");

            return retorno;
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
