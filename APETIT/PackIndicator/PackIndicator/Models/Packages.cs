using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PackIndicator.Models
{
    public partial class Packages
    {
        [JsonProperty("codigocliente")]
        public string Codigocliente { get; set; }

        [JsonProperty("embalagem")]
        public string Embalagem { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("fatorconversao")]
        public double Fatorconversao { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("validade")]
        public DateTime Validade { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        public int UoMEntry { get; set; }
        public double AttributedQuantity { get; set; }
        public List<int> AttributedLines { get; set; }

        public Packages()
        {
            AttributedLines = new List<int>();
        }
    }
}
