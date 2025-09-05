using CVA.AddOn.Common.Forms;
using System;


namespace CVA.View.DIME.VIEW
{
    public class fM5010 : BaseFormParent
    {
        public fM5010(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005010 form = new f2000005010();
                form.FormID = 5010;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
