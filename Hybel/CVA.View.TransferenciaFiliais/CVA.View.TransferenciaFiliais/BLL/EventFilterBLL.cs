using CVA.AddOn.Common.Controllers;

namespace CVA.View.TransferenciaFiliais.BLL
{
    public class EventFilterBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);

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
