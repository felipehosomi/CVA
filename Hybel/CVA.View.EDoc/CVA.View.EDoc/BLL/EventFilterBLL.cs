using CVA.AddOn.Common.Controllers;
using SAPbouiCOM;

namespace CVA.View.EDoc.BLL
{
    public class EventFilterBLL
    {
        public static void CreateDefaultEvents()
        {
            EventFilterController.SetFormEvent("FrmEDoc", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("FrmContribuinte", BoEventTypes.et_FORM_DATA_ADD);
            EnableEvents();
        }

        public static void EnableEvents()
        {
            EventFilterController.EnableEvents();
        }

        public static void DisableEvents()
        {
            EventFilterController.DisableEvents();
        }
    }
}
