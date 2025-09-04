using CVA.AddOn.Common.Controllers;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model
{
    public class ReposicaoInsumoModel
    {
        public ReposicaoInsumoModel()
        {
            Itens = new List<ReposicaoInsumoItensModel>();
        }

        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        [Display(Name = "Motivo da Solicitação")]
        public string MotivoCode { get; set; }
        public SelectList MotivoList { get; set; }

        [Display(Name = "Observação")]
        public string observacao { get; set; }

        [Display(Name = "Anexo")]
        public IEnumerable<HttpPostedFileBase> Anexo { get; set; }

        public List<ReposicaoInsumoItensModel> Itens { get; set; }
        public SelectList InsumoList { get; set; }
        public string UserCode { get; set; }
    }

    public class ReposicaoInsumoItensModel
    {
        [Display(Name = "Código")]
        public string InsumoCode { get; set; }

        [Display(Name = "Descrição")]
        public string InsumoName { get; set; }

        [Display(Name = "Quantidade")]
        public double Qty { get; set; }

        [Display(Name = "Data da Necessidade")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtNecessidade { get; set; }

        public bool Delete { get; set; }
        public int idList { get; set; }
    }

    public class ReposicaoInsumoAPIModel : ReposicaoInsumoModel
    {
        public ReposicaoInsumoAPIModel()
        {
            Anexo = new List<AttachmentsAPI>();
        }
        public new List<AttachmentsAPI> Anexo { get; set; }
    }
}
