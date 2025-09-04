using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Grupo Item
    /// </summary>
    public class f63 : BaseFormView
    {
        #region Constructor
        public f63()
        {
            FormCount++;
        }

        public f63(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;    
        }

        public f63(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f63(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.GrupoItem;
            TableName = "OITB";
            CodeColumn = "ItmsGrpCod";

            //CodeField = "6";
            //FocusField = "10002024";

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.GrupoItem;
            TableName = "OITB";
            CodeColumn = "ItmsGrpCod";

            //CodeField = "6";
            //FocusField = "10002024";

            return base.FormDataEvent();
        }
        #endregion
    }
}
