using CVA.AddOn.Common.Forms;
using CVA.View.EDoc.View;
using System;

namespace CVA.View.EDoc.Menu
{
    public class fMContribuinte : BaseFormParent
    {
        public fMContribuinte(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                FrmContribuinte form = new FrmContribuinte();
                form.FormID = 6011;
                form.Show("srfFiles\\fContribuinte.srf");
            }

            return true;
        }
    }
}
