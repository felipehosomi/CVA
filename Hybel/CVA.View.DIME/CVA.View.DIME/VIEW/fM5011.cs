using CVA.AddOn.Common.Forms;
using System;


namespace CVA.View.DIME.VIEW
{
    public class fM5011 : BaseFormParent
    {
        public fM5011(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005011 form = new f2000005011();
                form.FormID = 5011;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
