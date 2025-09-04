using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{
    public class ItemEstruturaModel : PagingModel
    {
        public List<ItemEstrutura> produtos { get; set; }
    }

    public class ItemEstrutura
    {
        [Newtonsoft.Json.JsonIgnore]
        public System.Int64 TotalRecords { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string codigo_sap_item { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string lote { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public double preco { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public double quantidade { get; set; }

        public string codigo_sap { get; set; }
        public List<Item> itens { get; set; }
    }

    public class Item
    {
        public string codigo_sap { get; set; }
        public string lote { get; set; }
        public double preco { get; set; }
        public double quantidade { get; set; }
    }

}