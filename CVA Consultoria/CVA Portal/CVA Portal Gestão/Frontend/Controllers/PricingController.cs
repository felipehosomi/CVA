using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class PricingController : Controller
    {
        #region Atributos
        private CVAGestaoService.PricingClient _pricingClient { get; set; }
        private GetSession GetSession = null;
        #endregion

        #region Construtor
        public PricingController()
        {
            this._pricingClient = new CVAGestaoService.PricingClient();
            LoadViewBags();
        }
        #endregion

        public string Get(int id)
        {
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Get(id));
        }

        public string Get_Info(int id)
        {
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Get_Info(id));
        }

        public string Get_By_Project(int id)
        {
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Get_By_Project(id));
        }

        public string Get_By_Opportunitty(int id)
        {
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Get_By_Opportunitty(id));
        }

        public string Insert(PricingModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Insert(model, 1));
        }

        public string Opportunitty_Insert(PricingModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Opportunitty_Insert(model, 1));
        }

        public string Update(PricingModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Update(model, 1));
        }

        public string Opportunitty_Update(PricingModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_pricingClient.Pricing_Opportunitty_Update(model, 1));
        }

        private void LoadViewBags()
        {
            this.GetSession = new GetSession();
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;
        }
    }
}
