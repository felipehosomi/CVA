using MODEL.Enumerators;
using MODEL.Status;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class StatusController : Controller
    {
        private CVAGestaoService.StatusClient StatusClient { get; set; }

        public StatusController()
        {
            this.StatusClient = new CVAGestaoService.StatusClient();
        }

        public string Get()
        {
            return JsonConvert.SerializeObject(StatusClient.GetStatus((int)ObjectType.All_Objects));
        }
    }
}
