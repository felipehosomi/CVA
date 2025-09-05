using CVA.AddOn.Common.Controllers;

namespace CVA.View.Hybel.BLL
{
    public class EventFilterBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterController.SetFormEvent("25", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);

            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);

            EventFilterController.SetFormEvent("134", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("134", SAPbouiCOM.BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("134", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("134", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);

            EventFilterController.SetFormEvent("139", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("139", SAPbouiCOM.BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("139", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("139", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);

            EventFilterController.SetFormEvent("141", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("141", SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterController.SetFormEvent("149", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("149", SAPbouiCOM.BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("149", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("149", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);

            EventFilterController.SetFormEvent("81", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("81", SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterController.SetFormEvent("540000988", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("540000988", SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);
            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);
            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("2000003031", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);

            EventFilterController.SetFormEvent("2000003033", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003032", SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK);

            EventFilterController.SetFormEvent("2000003033", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("2000003033", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000003033", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);
            EventFilterController.SetFormEvent("2000003033", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);

            EventFilterController.SetFormEvent("2000003034", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("2000003034", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000003034", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);
            EventFilterController.SetFormEvent("2000003034", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);

            EventFilterController.SetFormEvent("2000003036", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);

            EventFilterController.SetFormEvent("2000003037", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003037", SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK);
            EventFilterController.SetFormEvent("2000003037", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterController.SetFormEvent("2000003038", SAPbouiCOM.BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("2000003038", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("2000003038", SAPbouiCOM.BoEventTypes.et_COMBO_SELECT);
            EventFilterController.SetFormEvent("2000003038", SAPbouiCOM.BoEventTypes.et_FORM_CLOSE);
            EventFilterController.SetFormEvent("2000003038", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);

            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_FORM_RESIZE);
            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);
            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK);
            EventFilterController.SetFormEvent("2000003039", SAPbouiCOM.BoEventTypes.et_KEY_DOWN);

            EventFilterController.SetFormEvent("2000003040", SAPbouiCOM.BoEventTypes.et_FORM_RESIZE);

            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK);
            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_FORM_RESIZE);
            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_FORM_CLOSE);
            EventFilterController.SetFormEvent("2000003041", SAPbouiCOM.BoEventTypes.et_PRINT_LAYOUT_KEY);

            EventFilterController.SetFormEvent("2000003042", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003042", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterController.SetFormEvent("2000003043", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003044", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003045", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003045", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("2000003046", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003046", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterController.SetFormEvent("2000003047", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("2000003047", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("2000003047", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);

            EventFilterController.SetFormEvent("2000003048", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("2000003049", SAPbouiCOM.BoEventTypes.et_CLICK);

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
