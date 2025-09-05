using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
    public class GestaoPickingModel
    {

        [System.ComponentModel.DisplayName("Impressão")]
        public string Impresso { get; set; }

        [System.ComponentModel.DisplayName("Nº Pedido")]
        public int N_Pedido             { get; set; }

        [System.ComponentModel.DisplayName("Nº Pedido Magento")]
        public int N_PedidoMagento { get; set; }

        [System.ComponentModel.DisplayName("Data do Pedido")]
        public DateTime DocDate         { get; set; }

        [System.ComponentModel.DisplayName("Abertura Picking")]
        public string AberturaPicking { get; set; }

        [System.ComponentModel.DisplayName("Usuário Picking")]
        public string UsuarioPicking { get; set; }

        [System.ComponentModel.DisplayName("Status Picking")]
        public string StatusPicking { get; set; }

        [System.ComponentModel.DisplayName("Tipo de Transporte")]
        public string Transportadora { get; set; }

        [System.ComponentModel.DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [System.ComponentModel.DisplayName("Cliente")]
        public string Cliente           { get; set; }

        [System.ComponentModel.DisplayName("E-Mail")]
        public string E_Mail            { get; set; }

        [System.ComponentModel.DisplayName("Origem")]
        public string Origem { get; set; }

        [System.ComponentModel.DisplayName("Código do Rastreio")]
        public string CodigoRastreio { get; set; }

        



        

        

    }

    public class Data
    {
        public string Code { get; set; }
        public string Descricao { get; set; }
    }

    public class Transportadora
    {
        public string Code { get; set; }
        public string Descricao { get; set; }
    }


    public class _Status
    {
        public string Code { get; set; }
        public string Descricao { get; set; }
    }
}
