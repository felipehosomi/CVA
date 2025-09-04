using CVA.Portal.Producao.Model;
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
    [CvaAuthorize("InspecaoMP")]
    public class InspecaoMPController : Controller
    {
        APICallUtil api = new APICallUtil();

        #region Index
        public async Task<ActionResult> Index()
        {
            object dataDeStr = Session["RECDATADE"];
            object dataAteStr = Session["RECDATAATE"];
            object status = Session["RECSTATUS"];
            object nfStr = Session["RECNF"];
            object itemCodeStr = Session["RECITEMCODE"];
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
            ViewBag.NF = nfStr;
            ViewBag.CodigoItem = itemCodeStr;

            this.GetStatus(statusStr);

            return View(await api.GetListAsync<DocumentoModel>("InspecaoMP", $"dataDe={dataDeStr}&dataAte={dataAteStr}&status={statusStr}&nf={nfStr}&itemCode={itemCodeStr}"));
        }

        public async Task<ActionResult> TabelaRecebimentos(string dataDe, string dataAte, string status, int? nf, string itemCode)
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

            Session["RECDATADE"] = dataDe;
            Session["RECDATAATE"] = dataAte;
            Session["RECSTATUS"] = status;
            Session["RECNF"] = nf;
            Session["RECITEMCODE"] = itemCode;

            return PartialView(await api.GetListAsync<DocumentoModel>("InspecaoMP", $"dataDe={dataDeStr}&dataAte={dataAteStr}&status={status}&nf={nf}&itemCode={itemCode}"));
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
        #endregion
    }
}
