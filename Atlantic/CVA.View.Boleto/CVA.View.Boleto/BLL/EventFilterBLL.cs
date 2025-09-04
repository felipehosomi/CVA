using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Boleto.BLL
{
    public class EventFilterBLL
    {
        public static void DisableEvents()
        {
            EventFilterController.DisableEvents();
        }

        public static void SetDefaultEvents()
        {
            EventFilterController.SetFormEvent("196", SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            EventFilterController.SetFormEvent("196", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("196", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);
            EventFilterController.SetFormEvent("196", SAPbouiCOM.BoEventTypes.et_COMBO_SELECT);
            EventFilterController.SetFormEvent("196", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
        }
    }
}
