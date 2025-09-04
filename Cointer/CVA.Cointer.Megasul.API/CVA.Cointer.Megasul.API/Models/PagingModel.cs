using Newtonsoft.Json;
using System;

namespace CVA.Cointer.Megasul.API.Models
{
    public class PagingModel
    {
        [JsonProperty(Order = -2)]
        public string cnpj_filial { get; set; }
        [JsonProperty(Order = -2)]
        public Int64 quantidade_registros_total { get; set; }
        [JsonProperty(Order = -2)]
        public int quantidade_registros { get; set; }
    }
}