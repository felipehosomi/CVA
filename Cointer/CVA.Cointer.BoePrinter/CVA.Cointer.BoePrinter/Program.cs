using CVA.Cointer.BoePrinter.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Cointer.BoePrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            BoeBLL boeBLL = new BoeBLL();
            boeBLL.PrintPending();
        }
    }
}
