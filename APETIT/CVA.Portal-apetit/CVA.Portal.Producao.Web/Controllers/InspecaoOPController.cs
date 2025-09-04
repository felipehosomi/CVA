using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Configuracoes;
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
    [CvaAuthorize("InspecaoOP")]
    public class InspecaoOPController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region Index
        public async Task<ActionResult> Index()
        {
            object dataDeStr = Session["OPDATADE"];
            object dataAteStr = Session["OPDATAATE"];
            object status = Session["OPSTATUS"];
            object pedido = Session["OPPEDIDO"];
            object op = Session["OPNUM"];
            object codigoItem = Session["OPCODIGOITEM"];
            string statusStr = "P";

            if (dataAteStr == null)
            {
                dataAteStr = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (status != null)
            {
                statusStr = status.ToString();
            }

            ViewBag.DataDe = dataDeStr;
            ViewBag.DataAte = dataAteStr;
            ViewBag.Pedido = pedido;
            ViewBag.OP = op;
            ViewBag.CodigoItem = codigoItem;

            this.GetStatus(statusStr);

            var result = await api.GetListAsync<DocumentoModel>("InspecaoOP", $"dataDe={dataDeStr}&dataAte={dataAteStr}&status={statusStr}&pedido={pedido}&op={op}&codigoItem={codigoItem}");
            return View(result);
        }

        public async Task<ActionResult> TabelaOPs(string dataDe, string dataAte, string status, int? pedido, int? op, string codigoItem)
        {
            this.GetStatus(status);

            DateTime dataFiltro;
            string dataDeStr = String.Empty;
            string dataAteStr = String.Empty;
            if (DateTime.TryParseExact(dataDe, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataFiltro))
            {
                dataDeStr = dataFiltro.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            if (DateTime.TryParseExact(dataAte, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataFiltro))
            {
                dataAteStr = dataFiltro.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            Session["OPDATADE"] = dataDe;
            Session["OPDATAATE"] = dataAte;
            Session["OPSTATUS"] = status;
            Session["OPPEDIDO"] = pedido;
            Session["OPNUM"] = op;
            Session["OPCODIGOITEM"] = codigoItem;

            var result = await api.GetListAsync<DocumentoModel>("InspecaoOP", $"dataDe={dataDeStr}&dataAte={dataAteStr}&status={status}&pedido={pedido}&op={op}&codigoItem={codigoItem}");
            return PartialView(result);
        }

        public void GetStatus(string status = "")
        {
            List<ComboBoxModel> list = new List<ComboBoxModel>();
            list = new List<ComboBoxModel>();
            list.Add(new ComboBoxModel() { Code = "P", Name = "Pendente" });
            list.Add(new ComboBoxModel() { Code = "A", Name = "Aberto" });
            list.Add(new ComboBoxModel() { Code = "C", Name = "Cancelado" });
            list.Add(new ComboBoxModel() { Code = "F", Name = "Fechado" });
            ViewBag.StatusSelectList = new SelectList(list, "Code", "Name", status);
        }

        public async Task<JsonResult> GetByOP(int nrOP)
        {
            List<FichaProdutoModel> list = await api.GetListAsync<FichaProdutoModel>("FichaProduto", "nrOP=" + nrOP);
            SelectList sls = new SelectList(list, "StageId", "CodEtapa");
            return Json(sls, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
