using Newtonsoft.Json;
using System.Web.Mvc;
using MODEL.Classes;
using CVAGestaoLayout.Helper;

namespace CVAGestaoLayout.Controllers
{
    public class ChangeRequestController : Controller
    {
        #region Atributos
        private GetSession GetSession;
        private CVAGestaoService.ChangeRequestClient _changeRequestClient { get; set; }
        #endregion

        #region Construtor
        public ChangeRequestController()
        {
            this.GetSession = new GetSession();
            this._changeRequestClient = new CVAGestaoService.ChangeRequestClient();
        }
        #endregion

        public string Get(int id)
        {
            return JsonConvert.SerializeObject(_changeRequestClient.ChangeRequest_Get(id));
        }

        public string Get_for_Project(int id)
        {
            return JsonConvert.SerializeObject(_changeRequestClient.ChangeRequest_Get_for_Project(id));
        }

        public string Save(ChangeRequestModel model)
        {
            model.User = new UserModel{Id = GetSession.UserConnected.Id};
            return JsonConvert.SerializeObject(_changeRequestClient.ChangeRequest_Save(model));
        }
    }
}
