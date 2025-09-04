using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Alessi.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class fM3201 : BaseFormParent
    {
        public fM3201(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                if (SBOApp.Application.MessageBox("Confirma o cálculo de provisões para os relatórios financeiros?", 1, "Sim", "Não") == 1)
                {
                    var bll = new ProvisoesBLL();
                    bll.Recalcular();
                }
            }

            return true;
        }
    }
}
