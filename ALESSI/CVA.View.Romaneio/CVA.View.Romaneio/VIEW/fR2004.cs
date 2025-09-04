using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Romaneio.VIEW
{
    public class fR2004 : BaseFormParent
    {
        public fR2004(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000002004 form = new f2000002004();
                form.FormID = 2004;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
