using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.FolhaPagamento.VIEW
{
    public class fM5001 : BaseFormParent
    {
        public fM5001(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005001 form = new f2000005001();
                form.FormID = 5001;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
