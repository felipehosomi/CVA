using CVA.AddOn.Common.Forms;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4002 : BaseFormParent
    {
        public fM4002(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004002 form = new f2000004002();
                form.FormID = 4002;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
