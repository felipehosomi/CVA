using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.DIME.VIEW
{
    public class fM5013 : BaseFormParent
    {
        public fM5013(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005013 form = new f2000005013();
                form.FormID = 5013;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
