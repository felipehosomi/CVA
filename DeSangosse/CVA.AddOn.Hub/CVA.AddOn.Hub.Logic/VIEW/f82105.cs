using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Hub.HELPER;
using SAPbouiCOM;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f82105 : BaseForm
    {
        #region Constructor
        public f82105()
        {
            FormCount++;
        }

        public f82105(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f82105(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f82105(ContextMenuInfo contextMenuInfo)
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
                FormattedSearchUtil formattedSearchUtil = new FormattedSearchUtil();
                formattedSearchUtil.AssignFormattedSearch("CVA - Texto Imposto", DAO.Resource.Query.TextoPredefinido_Get, B1Forms.CodigoImposto, "et_ObsNF");
            }
            return true;
        }
    }
}
