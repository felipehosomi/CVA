using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Arquivo
    {
        public string NOMEARQUIVO { get; set; }
        public DateTime DATAIMPORTACAO { get; set; }
        public IList<ArquivoLinha> LINHAS { get; set; }
        public int STATUSARQUIVO { get; set; }
        public string MENSAGEMSTATUS { get; set; }
        public int QTDLINHAS { get; set; }

        public Arquivo()
        {
            LINHAS = new List<ArquivoLinha>();
        }
    }
}
