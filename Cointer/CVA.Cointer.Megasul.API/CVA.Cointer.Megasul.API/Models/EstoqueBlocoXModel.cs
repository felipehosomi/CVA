using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models
{
    public class EstoqueBlocoXModel : PagingModel
    {
        public List<EstoqueBlocoX> produtos { get; set; }
    }

    public class EstoqueBlocoX
    {
        public int aliquota { get; set; }
        public string arredondamento { get; set; }
        public string situacao_estoque { get; set; }
        public string codigo_gn { get; set; }
        public string codigo_ncm { get; set; }
        public string codigo_proprio { get; set; }
        public int codigo_sap { get; set; }
        public string descricao { get; set; }
        public string ippt { get; set; }
        public double situacao_tributaria { get; set; }
        public string unidade { get; set; }
        public double valor_base_calculo_icms_st { get; set; }
        public double valor_total_icms_st { get; set; }
        public double valor_total_aquisicao { get; set; }
        public double valor_total_icms_debito_fornecedor { get; set; }
        public double valor_unitario { get; set; }
    }

}