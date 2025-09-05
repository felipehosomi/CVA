using CVA.AddOn.Common.Forms;
using CVA.Electra;
using SAPbouiCOM;
using System;

namespace CVA.View.CRCP.Menu
{
    public class fM9901 : BaseFormParent
    {
        public fM9901(MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                //f2000009901 form = new f2000009901();
                //form.FormID = 9901;
                //form.SrfFolder = "srfFiles";
                //form.Show();
            }

            return true;
        }
    }
}
