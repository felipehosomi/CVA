using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4000 : BaseFormParent
    {
        public fM4000(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004000 form = new f2000004000();
                form.FormID = 4000;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
