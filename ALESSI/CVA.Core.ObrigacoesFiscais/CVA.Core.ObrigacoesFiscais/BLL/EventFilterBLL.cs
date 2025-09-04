using CVA.AddOn.Common.Controllers;
using SAPbouiCOM;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class EventFilterBLL
    {
        public static void CreateDefaultEvents()
        {
            EventFilterController.SetFormEvent("2000004000", BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000004000", BoEventTypes.et_CLICK);

            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_COMBO_SELECT);
            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_LOST_FOCUS);
            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_FORM_DATA_UPDATE);
            EventFilterController.SetFormEvent("2000004001", BoEventTypes.et_FORM_DATA_LOAD);

            EventFilterController.SetFormEvent("2000004002", BoEventTypes.et_FORM_DATA_ADD);

            EventFilterController.SetFormEvent("2000004003", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000004003", BoEventTypes.et_COMBO_SELECT);

            EventFilterController.SetFormEvent("2000004004", BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("2000004004", BoEventTypes.et_LOST_FOCUS);
            EventFilterController.SetFormEvent("2000004004", BoEventTypes.et_FORM_CLOSE);
            EventFilterController.SetFormEvent("2000004004", BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000004004", BoEventTypes.et_PRINT_LAYOUT_KEY);

            EventFilterController.SetFormEvent("2000004005", BoEventTypes.et_CLICK);

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
