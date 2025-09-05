using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
    public class EmbalagemModel
    {
        public int Codigo   { get; set; }

        public string Name  { get; set; }
    }

    public class Serie
    {
        public int Serie_Atual          { get; set; }
        public int Serie_Final          { get; set; }
        public int TipoDoc              { get; set; }
        public int DigTrackNumber       { get; set; }

        public string TipoServico       { get; set; }
        public string SufixoCodRastreio { get; set; }

    }

    public class ListaEmbalagemModel
    {
        [System.ComponentModel.DisplayName("Nº Pedido")]
        public int Pedido { get; set; }

        [System.ComponentModel.DisplayName("Linha")]
        public int Linha { get; set; }

        [System.ComponentModel.DisplayName("Tipo Embalagem")]
        public string Tipo_Embalagem { get; set; }

        [System.ComponentModel.DisplayName("Quantidade")]
        public string Quantidade{ get; set; }        

        [System.ComponentModel.DisplayName("Peso")]
        public decimal Peso { get; set; }

        


    }


}
