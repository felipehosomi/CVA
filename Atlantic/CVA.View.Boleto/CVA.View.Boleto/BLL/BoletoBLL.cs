using CVA.View.Boleto.DAO.OBOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Boleto.BLL
{
    public class BoletoBLL
    {
        public static int GetNextCode()
        {
            return BoletoDAO.GetLastCode() + 1;
        }
    }
}
