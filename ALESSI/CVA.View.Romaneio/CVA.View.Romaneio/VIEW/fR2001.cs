using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Romaneio.VIEW
{
    public class fR2001 : BaseFormParent
    {
        public fR2001(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000002001 form = new f2000002001();
                form.FormID = 2001;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
