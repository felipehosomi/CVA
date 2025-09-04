using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{
    public class FormaPagamentoModel : PagingModel
    {
        public List<FormaPagamento> formas_pagamento { get; set; }
    }

    public class FormaPagamento
    {
        [Newtonsoft.Json.JsonIgnore]
        public System.Int64 TotalRecords { get; set; }
        public string codigo_sap { get; set; }
        public string descricao { get; set; }
    }
}