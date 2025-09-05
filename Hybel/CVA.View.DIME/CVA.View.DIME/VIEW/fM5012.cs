using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.DIME.VIEW
{
    public class fM5012 : BaseFormParent
    {
        public fM5012(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005012 form = new f2000005012();
                form.FormID = 5012;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
