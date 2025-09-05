using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3043 : BaseFormParent
    {
        public fM3043(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003043 form = new f2000003043();
                form.FormID = 3043;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
