using SAPbouiCOM;
using SBO.Hub.Helpers;

namespace CVA.Cointer.Core.BLL
{
    class EventFilterBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterHelper.SetFormEvent("FrmConsignment", BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("FrmConsignment", BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterHelper.SetFormEvent("FrmReturnInvoice", BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("FrmReturnInvoice", BoEventTypes.et_VALIDATE);

            EventFilterHelper.SetFormEvent("179", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("179", BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterHelper.SetFormEvent("179", BoEventTypes.et_CLICK);

            EventFilterHelper.EnableEvents();
        }
    }
}
