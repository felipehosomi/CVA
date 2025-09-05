using CVA.AddOn.Common.Forms;
using CVA.View.EDoc.View;
using System;

namespace CVA.View.EDoc.Menu
{
    public class fMEDoc : BaseFormParent
    {
        public fMEDoc(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                FrmEDoc form = new FrmEDoc();
                form.FormID = 6010;
                form.Show("srfFiles\\fEDoc.srf");
            }

            return true;
        }
    }
}
