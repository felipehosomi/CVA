using CVA.AddOn.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.VIEW
{
    public class fM6062 : BaseFormParent
    {
        public fM6062(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f2000006062 form = new f2000006062();
                form.FormID = 6062;
                form.SrfFolder = "srfFiles";
                form.Show();
            }

            return true;
        }
    }
}
