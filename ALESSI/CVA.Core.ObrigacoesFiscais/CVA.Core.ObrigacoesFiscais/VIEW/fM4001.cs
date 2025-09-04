using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4001 : BaseFormParent
    {
        public fM4001(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004001 form = new f2000004001();
                form.FormID = 4001;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
