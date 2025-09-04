using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Romaneio.VIEW
{
    public class fR2005 : BaseFormParent
    {
        public fR2005(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000002005 form = new f2000002005();
                form.FormID = 2005;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
