using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Usuário
    /// </summary>
    public class f20700 : BaseFormView
    {
        #region Constructor
        public f20700()
        {
            FormCount++;
        }

        public f20700(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f20700(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f20700(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.Usuario;
            TableName = "OUSR";
            CodeColumn = "USERID";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.Usuario;
            TableName = "OUSR";
            CodeColumn = "USERID";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
