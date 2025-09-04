using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.MODEL
{
    public class FolhaPagamentoLinhaModel
    {
        public int Linha { get; set; }
        public List<FolhaPagamentoItemModel> Items { get; set; }
    }
}
