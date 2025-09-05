using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Hybel.View
{
    public class fM3048 : BaseFormParent
    {
        public fM3048(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003048 form = new f2000003048();
                form.FormID = 3048;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
