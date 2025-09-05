using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3046 : BaseFormParent
    {
        public fM3046(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003046 form = new f2000003046();
                form.FormID = 3046;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
