using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4005 : BaseFormParent
    {
        public fM4005(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004005 form = new f2000004005();
                form.FormID = 4005;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
