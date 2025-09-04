using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Romaneio.VIEW
{
    public class fR2006 : BaseFormParent
    {
        public fR2006(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000002006 form = new f2000002006();
                form.FormID = 2006;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
