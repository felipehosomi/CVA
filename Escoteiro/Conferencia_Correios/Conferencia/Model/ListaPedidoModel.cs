using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
   public class Status
    {
        public string Code      { get; set; }
        public string Descricao { get; set; }
    }

    public class Filial
    {
        public string Code      { get; set; }
        public string Descricao { get; set; }
    }

    public class Tipo_Envio
    {
        public string Code      { get; set; }
        public string Descricao { get; set; }
    }

    public class UF_Cliente
    {
        public string Code      { get; set; }
        public string Descricao { get; set; }
    }

    public class ListaPedidoModel
    {
        [System.ComponentModel.DisplayName("Nº Pedido")]
        public int N_Pedido             { get; set; }

        [System.ComponentModel.DisplayName("Data do Pedido")]
        public DateTime Data_Pedido     { get; set; }

        [System.ComponentModel.DisplayName("Total do Pedido")]
        public double  Total_Pedido      { get; set; }

        [System.ComponentModel.DisplayName("Forma Pagamento")]
        public string Forma_Pagamento { get; set; }

        [System.ComponentModel.DisplayName("Cliente")]
        public string Cliente           { get; set; }

        [System.ComponentModel.DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [System.ComponentModel.DisplayName("Tipo de Transporte")]
        public string Transportadora    { get; set; }

        [System.ComponentModel.DisplayName("Origem")]
        public string Origem            { get; set; }

        [System.ComponentModel.DisplayName("Status Pedido")]
        public string Status            { get; set; }

    }

    public class ListaPedidoModelPendente
    {
        [System.ComponentModel.DisplayName("Nº Pedido")]
        public int N_Pedido { get; set; }

        [System.ComponentModel.DisplayName("Data do Pedido")]
        public DateTime Data_Pedido { get; set; }

        [System.ComponentModel.DisplayName("Total do Pedido")]
        public double Total_Pedido { get; set; }

        [System.ComponentModel.DisplayName("Forma Pagamento")]
        public string Forma_Pagamento { get; set; }

        [System.ComponentModel.DisplayName("Status Pedido")]
        public string Status { get; set; }

        [System.ComponentModel.DisplayName("Cliente")]
        public string Cliente { get; set; }

        [System.ComponentModel.DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [System.ComponentModel.DisplayName("Tipo de Transporte")]
        public string Transportadora { get; set; }

        [System.ComponentModel.DisplayName("Origem")]
        public string Origem { get; set; }

    }


}
