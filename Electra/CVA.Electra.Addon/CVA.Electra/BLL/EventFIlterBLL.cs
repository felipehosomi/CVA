using CVA.AddOn.Common.Controllers;
using SAPbouiCOM;

namespace CVA.View.CRCP.BLL
{
    public class EventFilterBLL
    {
        public static void CreateDefaultEvents()
        {
            EventFilterController.SetFormEvent("2000009901", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000009901", BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("2000009901", BoEventTypes.et_DOUBLE_CLICK);
            EventFilterController.SetFormEvent("2000009901", BoEventTypes.et_COMBO_SELECT);
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
