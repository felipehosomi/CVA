using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.BLL
{
    public class RecursoBLL
    {
        private RecursoDAO _RecursoDAO { get; set; }

        public RecursoBLL(RecursoDAO recursoDAO)
        {
            _RecursoDAO = recursoDAO;
        }

        public List<Recurso> GetRecursoFixo(string itemCode)
        {
            return _RecursoDAO.GetRecursoFixo(itemCode);
        }
    }
}
