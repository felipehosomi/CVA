using CVA.AddOn.Common.Forms;
using System;


namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    public class fM4004 : BaseFormParent
    {
        public fM4004(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000004004 form = new f2000004004();
                form.FormID = 4004;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
