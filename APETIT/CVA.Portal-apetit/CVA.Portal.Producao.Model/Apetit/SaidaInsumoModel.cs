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
    public class SaidaInsumoModel
    {
        public SaidaInsumoModel()
        {
            //TipoSaidaList = new List<SelectListItem>();
            //TipoSaidaList.Add(new SelectListItem { Text = "", Value = "" });
            ////TipoSaidaList.Add(new SelectListItem { Text = "Consumo Extra", Value = "1" });
            //TipoSaidaList.Add(new SelectListItem { Text = "Perda de Material", Value = "2" });
            //TipoSaidaList.Add(new SelectListItem { Text = "Ajuste de estoque", Value = "3" });
            //TipoSaidaList.Add(new SelectListItem { Text = "Descarte", Value = "4" });
            //TipoSaidaList.Add(new SelectListItem { Text = "Correção para falta de lançamento de nota fiscal", Value = "5" });
            //TipoSaidaList.Add(new SelectListItem { Text = "Ajuste Primeiro MRP", Value = "6" });

            //ContratoList = new SelectList();
            //InsumoList = new SelectList();
            Itens = new List<SaidaInsumoItensModel>();
        }

        //[ModelController(IsPK = true)]
        //public string Code { get; set; }
        //public string Error { get; set; }

        [Display(Name = "Tipo de Saída")]
        public string TipoSaidaCode { get; set; }
        public List<SelectListItem> TipoSaidaList { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        [Display(Name = "Motivo o Ajuste")]
        public string Motivo { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        public List<SaidaInsumoItensModel> Itens { get; set; }
        public SelectList InsumoList { get; set; }
    }

    public class SaidaInsumoItensModel: SaidaInsumoItensModel01
    {
        public bool Delete { get; set; }
        public int idList { get; set; }
    }

    public class DocumentoMarketingInsumoModel : DocumentoMarketingModel
    {
        public new List<DocumentInsumoline> DocumentLines { get; set; }
        public string TaxDate { get; set; }
        public string JournalMemo { get; set; }
    }

    public class DocumentInsumoline : Documentline
    {
        
        public string WhsCode { get; set; }
        public string U_CVA_TpAjuste { get; set; }
        public string U_CVA_TipoSaida { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        
        
    }

    public class SaidaInsumoModelGetOBPL
    {
        public string DflWhs { get; set; }
        public string U_CVA_Dim1Custo { get; set; }
        public string U_CVA_Dim2Custo { get; set; }
    }

    public class SaidaInsumoModelGetItemOnHand
    {
        public double? OnHand { get; set; }
        public double? AvgPrice { get; set; }
    }

    public class SaidaInsumoItensModel01
    {

        [Display(Name = "Insumo")]
        public string InsumoCode { get; set; }
        public string InsumoName { get; set; }

        [Display(Name = "Quantidade")]
        public double Qty { get; set; }
    }

    public class RepportSaidaInsumoModel
    {
        public RepportSaidaInsumoModel()
        {
            Itens = new List<SaidaInsumoItensModel02>();
        }
                

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }


        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        public List<SaidaInsumoItensModel02> Itens { get; set; }
        public SelectList InsumoList { get; set; }
    }

    public class SaidaInsumoItensModel02 : SaidaInsumoItensModel01
    {
        [Display(Name = "Custo")]
        public double Custo { get; set; }
    }
}
