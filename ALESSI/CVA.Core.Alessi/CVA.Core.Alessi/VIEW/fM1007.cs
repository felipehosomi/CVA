using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class fM1007 : BaseFormParent
    {
        public fM1007(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001007 form = new f2000001007();
                form.FormID = 1007;
                form.SrfFolder = "srfFiles";
                form.Show();
            }
            return true;
        }
    }
}
