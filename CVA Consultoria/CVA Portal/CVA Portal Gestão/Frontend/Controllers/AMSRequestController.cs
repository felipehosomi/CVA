using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class AMSRequestController : Controller
    {
        private CVAGestaoService.AMSRequest _AMSRequest { get; set; }
        // GET: AMSRequest
        public ActionResult Get()
        {
            return JsonConvert.SerializeObject(_AMSRequest.Get());
        }
    }
}