using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{
    public class UnidadeMedidaModel : PagingModel
    {
        public List<UnidadeMedida> unidade_medida { get; set; }
    }

    public class UnidadeMedida
    {
        [Newtonsoft.Json.JsonIgnore]
        public System.Int64 TotalRecords { get; set; }
        public string descricao { get; set; }
        public double fracao_minima { get; set; }
        public string sigla { get; set; }
    }
}