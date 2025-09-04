using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.Alessi.VIEW
{
    public class fM1002 : BaseFormParent
    {
        public fM1002(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001002 form = new f2000001002();
                form.FormID = 1002;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
