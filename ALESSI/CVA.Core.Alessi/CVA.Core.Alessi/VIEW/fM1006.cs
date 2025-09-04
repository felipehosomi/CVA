using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class fM1006 : BaseFormParent
    {
        public fM1006(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000001006 form = new f2000001006();
                form.FormID = 1006;
                form.SrfFolder = "srfFiles";
                form.Show();
            }
            return true;
        }
    }
}
