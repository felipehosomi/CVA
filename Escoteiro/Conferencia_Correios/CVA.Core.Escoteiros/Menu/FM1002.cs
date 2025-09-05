using CVA.AddOn.Common.Forms;
using CVA.Core.Escoteiros.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Menu
{
    class FM1002 : BaseFormParent
    {
        public FM1002(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f1002 form = new f1002();
                form.FormID = 1002;
                form.SrfFolder = "srfFiles";
                form.Show();
                
                                
                //form.Show("srfFiles\\f1002.srf");
            }

            return true;
        }
    }

}
