using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{

    public class EstoqueModel : PagingModel
    {
        public System.Collections.Generic.List<EstoqueItem> produtos { get; set; }
        [JsonIgnore]
        public List<EstoqueItemModel> EstoqueItemList { get; set; }
    }

    public class EstoqueItem
    {
        public string codigo_produto_sap { get; set; }
        public List<Estoque> estoques { get; set; }
    }

    public class Estoque
    {
        public string lote { get; set; }
        public double quantidade { get; set; }
        public string numero_serie { get; set; }
    }

    public class EstoqueItemModel
    {
        [JsonIgnore]
        public Int64 TotalRecords { get; set; }
        public string codigo_produto_sap { get; set; }
        public string lote { get; set; }
        public double quantidade { get; set; }
        public string numero_serie { get; set; }
    }
}