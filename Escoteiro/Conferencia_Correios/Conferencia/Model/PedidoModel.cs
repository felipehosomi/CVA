using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
    public class PedidoModel
    {
        public string Cliente { get; set; }
        public string DocStatus { get; set; }
        public string TipoPag { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string ParcFaturado { get; set; }
        public string CardCode { get; set; }
        public string ParcAntecipado { get; set; }
        public string CodBarras { get; set; }
        public string Origem { get; set; }
        public string Transportadora { get; set; }

        public int DocNum { get; set; }
        public int Qtd_pedido { get; set; }

        public DateTime Data_Entrega { get; set; }
        public DateTime Data_Pedido { get; set; }


    }

    public class NfModel
    {        
        public int DocType          { get; set; }
        public int DocEntry         { get; set; }
        public int DocLine          { get; set; }
        public int SysNumber        { get; set; }
        public int LocCode          { get; set; }                
        
        public int Usage            { get; set; }
        public int BPLId            { get; set; }
        public int DocSubLine       { get; set; }

        public double QtyOnHand     { get; set; }
        public double QtdCommit     { get; set; }
        public double QtySelected   { get; set; }

        public string ManagedBy     { get; set; }
        public string ItemCode      { get; set; }
        public string WhsCode       { get; set; }
        public string DistNumber    { get; set; }
        public string CardCode      { get; set; }
        public string MnfSerial     { get; set; }

        public DateTime ExpDate     { get; set; }

        public string NfeTipoEnv { get; set; }

    }

    public class Login
    {
        public string Logon         { get; set; }
        public string Password      { get; set; }
        public string Autorizacao   { get; set; }
    }

    public class CabecalhoPedidoModel
    {
        public int N_Pedido { get; set; }

        public DateTime Data_Entrega { get; set; }
        public DateTime Data_Pedido { get; set; }

        public string Transportadora { get; set; }
        public string Cliente { get; set; }
        public string Origem { get; set; }
    }

    public class GriConf
    {
        [System.ComponentModel.DisplayName("Código Item")]
        public string ItemCode { get; set; }

        [System.ComponentModel.DisplayName("Descrição")]
        public string ItemName { get; set; }

        [System.ComponentModel.DisplayName("Cód Barras")]
        public string Codbarras { get; set; }

        [System.ComponentModel.DisplayName("Quantidade")]
        public int Quantidade { get; set; }

        [System.ComponentModel.DisplayName("Escaneado")]
        public int Escaneado { get; set; }
        
    }

}
