using Newtonsoft.Json;
using System;

namespace CVA.Cointer.Megasul.API.Models
{
    public class ClienteModel : PagingModel
    {
        public System.Collections.Generic.List<Cliente> clientes { get; set; }
        public string Error { get; set; }
    }

    public class Cliente
    {
        public string bairro { get; set; }
        public string celular { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string cnpj_cpf { get; set; }
        public string codigo_municipio { get; set; }
        public string codigo_pais { get; set; }
        public string codigo_sap { get; set; }
        public string codigo_megasul { get; set; }
        public string complemento { get; set; }
        public string email { get; set; }
        public string endereco { get; set; }
        public string ie_rg { get; set; }
        public string im { get; set; }
        public string nome { get; set; }
        public string numero { get; set; }
        public string razao_social { get; set; }
        public string telefone { get; set; }
        public string tipo_pessoa { get; set; }
        public string uf { get; set; }

        [JsonIgnore]
        public Int64 TotalRecords { get; set; }
        [JsonIgnore]
        public Int64 RN { get; set; }
    }
}