using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.Model
{
    public class ListaPostagemModel
    {        
        public string Cliente { get; set; }
        public DateTime Faturaemnto { get; set; }

        public int Serial { get; set; }
    }


    public class ComboTransp
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }
}
