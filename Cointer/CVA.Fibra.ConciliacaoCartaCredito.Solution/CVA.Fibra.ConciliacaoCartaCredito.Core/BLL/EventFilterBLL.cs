using SAPbouiCOM;
using SBO.Hub.Helpers;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.BLL
{
    public class EventFilterBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterHelper.SetFormEvent("FrmImport", BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("FrmImport", BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterHelper.EnableEvents();
        }
    }
}
