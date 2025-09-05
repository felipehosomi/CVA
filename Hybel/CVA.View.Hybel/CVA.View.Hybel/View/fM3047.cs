using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3047 : BaseFormParent
    {
        public fM3047(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003047 form = new f2000003047();
                form.FormID = 3047;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
