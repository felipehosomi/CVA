//using B1WizardBase;
using CVA.AddOn.Common.Forms;
using CVA.Core.Escoteiros.View;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Menu
{
    class FM1003 : BaseFormParent
    {

        public FM1003(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                f1003 form = new f1003();
                form.FormID = 1003;
                form.SrfFolder = "srfFiles";
                form.Show();


                //form.Show("srfFiles\\f1002.srf");
            }

            return true;
        }

        //public FM1003()
        //{
        //    MenuUID = "FM1003";
        //    LoadXml("srfFiles\\f1002.srf");
        //}

        //[B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        //public virtual void OnAfterMenuClick(MenuEvent pVal)
        //{
        //    // ADD YOUR ACTION CODE HERE ...
        //    LoadForm();
        //    f1003.CarregaFormulario();
        //}

    }

}
