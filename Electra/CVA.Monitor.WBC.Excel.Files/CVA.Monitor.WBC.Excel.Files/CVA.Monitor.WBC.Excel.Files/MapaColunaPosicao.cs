using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Monitor.WBC.Excel.Files
{
    public class MapaColunaPosicao
    {
        public MapaColunaPosicao(string pNomeColuna, string pPosicao)
        {
            NomeColuna = pNomeColuna;
            Posicao = pPosicao;
        }
        public string NomeColuna { get; set; }
        public string Posicao { get; set; }
    }
}
