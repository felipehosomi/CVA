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
    public class ApontamentoEncerramentoModel
    {
        [Display(Name = "Nº do Contrato")]
        public string ContratoCode { get; set; }
        public SelectList ContratoList { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        [Display(Name = "Período de")]
        public DateTime contratoDe { get; set; }

        [Display(Name = "até")]
        public DateTime contratoAte { get; set; }
        
        public string UserCode { get; set; }
        
    }

}
