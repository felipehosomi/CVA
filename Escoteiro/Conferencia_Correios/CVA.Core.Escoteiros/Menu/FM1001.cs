using CVA.AddOn.Common.Forms;
using CVA.Core.Escoteiros.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Menu
{
    class FM1001 : BaseFormParent
    {
        public FM1001(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f1001 form = new f1001();
                form.FormID = 3047;
                form.Show("srfFiles\\f1001.srf");
                
            }

            return true;
        }


        
    }
}


