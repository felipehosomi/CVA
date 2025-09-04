using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.Alessi.VIEW
{
    public class fM3002 : BaseFormParent
    {
        public fM3002(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003002 form = new f2000003002();
                form.FormID = 3002;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
