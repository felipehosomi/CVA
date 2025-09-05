using CVA.AddOn.Common.Controllers;
using SAPbouiCOM;

namespace CVA.Escoteiro.Magento.AddOn.View
{
    class EventFilterBLL
    {
        public static void CreateDefaultEvents()
        {
            EventFilterController.SetFormEvent("FrmTabelaPrecoSerie", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("FrmTabelaPrecoSerie", BoEventTypes.et_VALIDATE);
            EventFilterController.SetFormEvent("FrmPesquisasSatisfacao", BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("FrmPesquisasSatisfacao", BoEventTypes.et_VALIDATE);

            //EventFilterController.SetFormEvent("60100", BoEventTypes.et_FORM_LOAD);
            //EventFilterController.SetFormEvent("60100", BoEventTypes.et_FORM_DATA_LOAD);
            //EventFilterController.SetFormEvent("60100", BoEventTypes.et_FORM_DATA_ADD);
            //EventFilterController.SetFormEvent("60100", BoEventTypes.et_FORM_DATA_UPDATE);
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
