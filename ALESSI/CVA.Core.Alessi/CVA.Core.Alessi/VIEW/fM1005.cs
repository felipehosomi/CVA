using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class fM1005 : BaseFormParent
    {
        public fM1005(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001005 form = new f2000001005();
                form.FormID = 1005;
                form.SrfFolder = "srfFiles";
                form.Show();
            }
            return true;
        }
    }
}
