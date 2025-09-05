using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;


namespace CVA.View.Hybel.View
{
    public class fM3031 : BaseFormParent
    {
        public fM3031(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000003031 form = new f2000003031();
                form.FormID = 3031;
                form.SrfFolder = "srfFiles";
                form.Show();

                Form Form = SBOApp.Application.Forms.ActiveForm;
                Form.Items.Item("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 1, BoModeVisualBehavior.mvb_False);
            }

            return true;
        }
    }
}
