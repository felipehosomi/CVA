using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3033 : BaseFormParent
    {
        public fM3033(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003033 form = new f2000003033();
                form.FormID = 3033;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
