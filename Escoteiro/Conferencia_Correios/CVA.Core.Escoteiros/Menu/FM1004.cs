//using B1WizardBase;
using CVA.AddOn.Common.Forms;
using CVA.Core.Escoteiros.View;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Menu
{
    class FM1004 : BaseFormParent
    {

        public FM1004(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f1004 form = new f1004();
                form.FormID = 1004;
                form.SrfFolder = "srfFiles";
                form.Show();
            }
            return true;
        }
    }
}
