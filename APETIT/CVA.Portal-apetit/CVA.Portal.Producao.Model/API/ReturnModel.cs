using CVA.AddOn.Common.Controllers;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model
{
    public class ReturnModel
    {        
        public bool Sucess { get; set; }
        public string Code { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }

}
