using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class FichaInspecaoController : ApiController
    {
        FichaInspecaoBLL BLL;

        public FichaInspecaoController()
        {
            BLL = new FichaInspecaoBLL();
        }

        public List<FichaInspecaoModel> GetList(DateTime? startDate, DateTime? endDate, string tipoDoc, int? nf = null, string codEtapa = null)
        {
            return BLL.GetList(startDate, endDate, tipoDoc, nf, codEtapa);
        }

        public FichaInspecaoModel Get(string id)
        {
            return BLL.Get(id);
        }

        public FichaInspecaoModel GetByOPItemEtapa(int docEntryOP, string codItem, string codEtapa, int modal)
        {
            return BLL.GetByOPItemEtapa(docEntryOP, codItem, codEtapa, modal);
        }

        public FichaInspecaoModel GetByItemEtapa(string codItem, string codEtapa)
        {
            return BLL.GetByItemEtapa(codItem, codEtapa);
        }

        public void Post(FichaInspecaoModel model)
        {
            BLL.Create(model);
        }

        public void Put(FichaInspecaoModel model)
        {
            BLL.Update(model);
        }
    }
}
