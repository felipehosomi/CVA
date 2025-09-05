using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3036 : BaseFormParent
    {
        public fM3036(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003036 form = new f2000003036();
                form.FormID = 3036;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
