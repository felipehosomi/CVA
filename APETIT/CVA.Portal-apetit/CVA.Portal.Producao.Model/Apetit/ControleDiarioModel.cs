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
    public class ControleDiarioModel
    {
        public ControleDiarioModel()
        {
            Itens = new List<ControleDiarioItensModel>();
        }

        [Display(Name = "Nº do Contrato")]
        public string ContratoCode { get; set; }
        public SelectList ContratoList { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        [Display(Name = "Nº do CDR")]
        public string CDRCode { get; set; }

        [Display(Name = "Data")]
        public DateTime dt { get; set; }

        [Display(Name = "Encarregado UEN")]
        public string EncarregadoUEN { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string Obs { get; set; }

        public List<ControleDiarioItensModel> Itens { get; set; }
        public string UserCode { get; set; }

        [Display(Name = "Total")]
        public double TotalApontada { get; set; }
    }

    public class ControleDiarioItensModel
    {
        [Display(Name = "Serviço")]
        public string ServicoCode { get; set; }
        public string ServicoName { get; set; }

        [Display(Name = "Qtde Apontada")]
        public double QtdeApontada { get; set; }
    }






}
