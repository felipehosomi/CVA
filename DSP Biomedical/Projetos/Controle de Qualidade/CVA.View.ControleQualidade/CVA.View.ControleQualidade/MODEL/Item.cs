using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.MODEL
{
    public class Item
    {
        public string ItemCode { get; set; }
        public string PlanoInspecao { get; set; }
        public string TipoInspecao { get; set; }

        public List<ItemLote> LoteList { get; set; }
    }

    public class ItemLote
    {
        public string Lote { get; set; }
    }
}
