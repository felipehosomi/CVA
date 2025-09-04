using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models
{
    public class ClienteResponseModel
    {
        public List<ClienteResponse> clientes { get; set; }
    }

    public class ClienteResponse
    {
        public string codigo_megasul { get; set; }
        public string codigo_sap { get; set; }
        public bool erro { get; set; }
        public string msgDeRetorno { get; set; }
    }
}