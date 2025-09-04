using CVA.AddOn.Common.Forms;
using System;

namespace CVA.View.Dctf.VIEW
{
    public class fD5050 : BaseFormParent
    {
        public fD5050(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000005050 form = new f2000005050();
                form.FormID = 5050;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
