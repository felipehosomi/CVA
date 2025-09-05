using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3042 : BaseFormParent
    {
        public fM3042(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003042 form = new f2000003042();
                form.FormID = 3042;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
