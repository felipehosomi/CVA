using CVA.Tust.Import.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Tust.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            TustDAO dao = new TustDAO();
            dao.Execute();
        }
    }
}
