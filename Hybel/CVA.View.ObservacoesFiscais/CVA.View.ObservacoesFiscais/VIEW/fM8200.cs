using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.ObservacoesFiscais.VIEW
{
    public class fM8200 : BaseFormParent
    {
        public fM8200(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000008200 form = new f2000008200();
                form.FormID = 8200;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
