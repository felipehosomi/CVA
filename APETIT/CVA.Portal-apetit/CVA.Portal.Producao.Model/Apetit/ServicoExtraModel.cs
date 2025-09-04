using CVA.AddOn.Common.Controllers;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model
{
    public class ServicoExtraModel
    {
        public ServicoExtraModel()
        {
            //ContratoList = new SelectList();
            //InsumoList = new SelectList();
            Itens = new List<ServicoExtraItensModel>();
        }
        
        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        [Display(Name = "Anexo")]
        public IEnumerable<HttpPostedFileBase> Anexo { get; set; }

        [Display(Name = "Data")]
        public DateTime Dt { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        public List<ServicoExtraItensModel> Itens { get; set; }
        public SelectList InsumoList { get; set; }
    }

    public class ServicoExtraAPIModel : ServicoExtraModel
    {
        public ServicoExtraAPIModel()
        {
            Anexo = new List<AttachmentsAPI>();
        }
        public new List<AttachmentsAPI> Anexo { get; set; }
        public string UserCode { get; set; }
    }
    
    public class ServicoExtraItensModel : SaidaInsumoItensModel01
    {
        public bool Delete { get; set; }
        public int idList { get; set; }
    }

    public class DocumentoMarketingServicoExtraModel : DocumentoMarketingModel
    {
        public string CardCode { get; set; }
        public string TaxDate { get; set; }
        public string DocDueDate { get; set; }
        public int? AttachmentEntry { get; set; }
        public int? AgreementNo { get; set; }
        public int? SequenceCode { get; set; }
    }

    public class ServicoExtraModelGetItemOnHand
    {
        public double? OnHand { get; set; }
    }

   

    public class ServicoExtraModelGetItemCodeServicoExtra
    {
        public string ItemCode { get; set; }
    }

    public class ServicoExtraModelGetBlanketId
    {
        public int AbsID { get; set; }
    }
    

    public class ServicoExtraItensModel01
    {

        [Display(Name = "Insumo")]
        public string InsumoCode { get; set; }
        public string InsumoName { get; set; }

        [Display(Name = "Quantidade")]
        public double Qty { get; set; }
    }
}
