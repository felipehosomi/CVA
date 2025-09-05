using B1WizardBase;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.View
{
    public class Button_btnF1003 : B1Item
    {
        public Button_btnF1003()
        {
            FormType = "CVA_Separacao";
            ItemUID = "btn_ok";
        }

        [B1Listener(BoEventTypes.et_CLICK, false)]
        public virtual void OnAfterClick(ItemEvent pVal)
        {
            try
            {
                Form oForm = B1Connections.theAppl.Forms.Item(pVal.FormUID);
                f1003 objForm = new f1003();
                objForm.GerarRelatorio();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
