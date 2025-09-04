using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class ProvisoesBLL
    {
        public void Recalcular()
        {
            RecalcularOCFL();
            RecalcularORCL();
            RecalcularORCP();
            RecalcularORCR();
        }

        private void RecalcularOCFL()
        {
            var lst = CrudB1Controller.FillStringList("SELECT Code FROM OCFL");
        }

        private void RecalcularORCL()
        {
            var lst = CrudB1Controller.FillStringList("SELECT Code FROM ORCL WHERE Status = 'N'");
        }

        private void RecalcularORCP()
        {
            var lst = CrudB1Controller.FillStringList("SELECT Code FROM ORCP WHERE IsRemoved = 'N'");
        }

        private void RecalcularORCR()
        {
            var lst = CrudB1Controller.FillStringList("SELECT Code FROM ORCR");
        }
    }
}
