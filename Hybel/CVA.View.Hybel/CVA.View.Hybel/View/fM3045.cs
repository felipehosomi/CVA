using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3045 : BaseFormParent
    {
        public fM3045(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003045 form = new f2000003045();
                form.FormID = 3045;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
