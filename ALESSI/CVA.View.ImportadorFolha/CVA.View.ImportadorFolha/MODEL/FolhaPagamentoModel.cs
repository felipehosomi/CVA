using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.MODEL
{
    public class FolhaPagamentoModel
    {
        public int BPlId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<FolhaPagamentoLinhaModel> Lines { get; set; }
    }
}
