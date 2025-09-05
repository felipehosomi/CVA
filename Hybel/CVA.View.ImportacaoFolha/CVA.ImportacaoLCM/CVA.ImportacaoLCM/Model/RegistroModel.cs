using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Model
{
   public class RegistroModel
    {
       public RegistroModel()
        {
            //this.RegistrosDetalhe = new List<DetalheModel>();
        }
       public HeadModel Header { get; set; }
       //public List<DetalheModel> RegistrosDetalhe { get; set; }
       public TrailerModel Trailer { get; set; }
    }
}
