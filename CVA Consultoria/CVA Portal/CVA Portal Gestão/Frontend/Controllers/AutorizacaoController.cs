using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize(new string[] { "Apontamento" })]
    public class AutorizacaoController : Controller
    {
        #region Atributos
        private GetSession _getSession = null;
        private CVAGestaoService.AuthorizationClient _authorizationClient { get; set; }
        #endregion

        #region Construtor
        public AutorizacaoController()
        {
            this._getSession = new GetSession();
            this._authorizationClient = new CVAGestaoService.AuthorizationClient();
            
            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }
        #endregion

        public string Get_DiasAutorizados(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_DiasAutorizados(idCol));
        }

        public string AddDiaAutorizado(AuthorizedDayModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_authorizationClient.AddDiaAutorizado(model));
        }

        public string RemoveDiaAutorizado(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveDiaAutorizado(id));
        }

        public string Get_HorasAutorizadas(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_HorasAutorizadas(idCol));
        }

        public string AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_authorizationClient.AddHorasAutorizadas(model));
        }

        public string RemoveHorasAutorizadas(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveHorasAutorizadas(id));
        }

        public string Get_LimiteHoras(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_LimiteHoras(idCol));
        }

        public string AddLimiteHoras(HoursLimitModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_authorizationClient.AddLimiteHoras(model));
        }

        public string RemoveLimiteHoras(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveLimiteHoras(id));
        }
    }
}