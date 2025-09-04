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
    public class PainelApontamentoModel
    {
        public PainelApontamentoModel()
        {
            Itens = new List<PainelContratoItens>();
        }
        
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
        
        public List<PainelContratoItens> Itens { get; set; }
        public string UserCode { get; set; }

        public class PainelContratoItens
        {
            [Display(Name = "Data")]
            public DateTime DT { get; set; }

            public int DAYOFWEEK { get; set; }

            [Display(Name = "Dia da Semana")]
            public string DAYOFWEEKDESC { get; set; }

            [Display(Name = "Código do Serviço")]
            public string SERVICOCODE { get; set; }
            public string SERVICONAME { get; set; }

            [Display(Name = "Código do Turno")]
            public string TURNOCODE { get; set; }
            public string TURNONAME { get; set; }

            [Display(Name = "Qtd Planejada")]
            public double QTYPL { get; set; }

            [Display(Name = "Qtd. Apontada")]
            public double QTYAP { get; set; }

            [Display(Name = "Diferença")]
            public double DIFF { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }
        }
    }

}
