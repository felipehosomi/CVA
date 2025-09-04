using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.MonitorReplicacao.MODEL
{
    public class RegistrosMODEL
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Codigo { get; set; }
        public string Status { get; set; }
        public string Origem { get; set; }
        public string TipoObjeto { get; set; }
        public string Funcao { get; set; }
        public string BaseErro { get; set; }
    }
}
