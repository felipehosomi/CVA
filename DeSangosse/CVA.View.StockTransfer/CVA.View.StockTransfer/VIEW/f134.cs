using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using CVA.AddOn.Common.Util;
using CVA.View.StockTransfer.DAO;

namespace CVA.View.StockTransfer.VIEW
{
    /// <summary>
    /// Cadastro de PN
    /// </summary>
    public class f134 : BaseForm
    {
        #region Constructor
        public f134()
        {
            FormCount++;
        }

        public f134(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f134(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f134(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
            {
                ComboBox cb_WhsTran = Form.Items.Item("cb_WhsTran").Specific as ComboBox;
                cb_WhsTran.AddValuesFromQuery(Query.Warehouse_GetThirdPartyList);
            }

            return true;
        }
    }
}
