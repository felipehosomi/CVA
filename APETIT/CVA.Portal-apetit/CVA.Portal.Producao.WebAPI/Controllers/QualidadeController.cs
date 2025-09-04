using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class QualidadeController : ApiController
    {
        // GET: api/Qualidade
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Qualidade/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Qualidade
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Qualidade/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Qualidade/5
        public void Delete(int id)
        {
        }
    }
}
