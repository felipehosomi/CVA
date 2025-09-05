using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
   public class ListaProdutosConf1
    {
        [System.ComponentModel.DisplayName("Código Item")]
        public string ItemCode      { get; set; }

        [System.ComponentModel.DisplayName("Descrição")]
        public string ItemName      { get; set; }        

        [System.ComponentModel.DisplayName("Quantidade")]        
        public int Quantidade       { get; set; }

        [System.ComponentModel.DisplayName("Quantidade em Unidades")]
        public int  Qtde_Um         { get; set; }

        [System.ComponentModel.DisplayName("Status")]
        public string Status { get; set; }

        [System.ComponentModel.DisplayName("Código Barras")]
        public string CodBarras { get; set; }
        
    }

    public class ListaConferecia
    {
        [System.ComponentModel.DisplayName("Nº Pedido")]
        public int N_Pedido { get; set; }

        [System.ComponentModel.DisplayName("Data Estimada Entrega")]
        public DateTime Data_Entrega { get; set; }

        [System.ComponentModel.DisplayName("Data do Pedido")]
        public DateTime Data_Pedido { get; set; }

        [System.ComponentModel.DisplayName("Origem")]
        public string Origem { get; set; }

        [System.ComponentModel.DisplayName("Cliente")]
        public string Cliente { get; set; }

        [System.ComponentModel.DisplayName("Transportadora")]
        public string Transportadora { get; set; }

        [System.ComponentModel.DisplayName("Loja")]
        public string Filial { get; set; }
    }


}
