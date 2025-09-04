using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4003 : BaseFormParent
    {
        public fM4003(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004003 form = new f2000004003();
                form.FormID = 4003;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
