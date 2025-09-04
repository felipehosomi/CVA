using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models
{
    public class NotaFiscalCancelamentoModel
    {
        public string cnpj_filial { get; set; }
        public Identificador identificador { get; set; }
        public Identificador_Cancelamento identificador_cancelamento { get; set; }
        public Ecf ecf { get; set; }
    }

    public class Identificador_Cancelamento
    {
        public int coo { get; set; }
        public int ccf { get; set; }
        public string data_hora { get; set; }
        public int ef { get; set; }
        public int si { get; set; }
        public int tr { get; set; }
    }
}