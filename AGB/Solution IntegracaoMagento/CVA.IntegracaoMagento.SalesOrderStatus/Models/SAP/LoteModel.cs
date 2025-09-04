using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrderStatus
{
    public class LoteModel
    {
        public string SKU { get; set; }
        public double Qty { get; set; }
        public string Lote { get; set; }
        public string DataFabricacao { get; set; }
        public string DataValidade { get; set; }
        public string Atributo1 { get; set; }
        public string Atributo2 { get; set; }
        public string ItemCodeSAP { get; set; }

        public int Linha { get; set; }
        public int Status { get; set; }
        public string Erro { get; set; }
    }
}
