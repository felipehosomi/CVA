using DAL.Connection;
using SAPbouiCOM;

namespace CONTROLLER
{
    public class ItemEventController
    {
        #region ItemEvents
        public static void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Form form = null;
            if (pVal.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                try
                {
                    form = ConnectionDao.Instance.Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                }
                catch
                {
                    return;
                }
            }

            if (FormUID == "10001")
            {
                ImportadorPedidoController.ItemEvents(FormUID, ref pVal, out BubbleEvent, form);
            }
            if (FormUID == "10002")
            {
                GeradorNFController.ItemEvents(FormUID, ref pVal, out BubbleEvent, form);
            }
            if (FormUID == "10003")
            {
                CancelaDocController.ItemEvents(FormUID, ref pVal, out BubbleEvent, form);
            }
        }
        #endregion
    }
}
