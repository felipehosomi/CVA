using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3049 : BaseFormParent
    {
        public fM3049(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003049 form = new f2000003049();
                form.FormID = 3049;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
