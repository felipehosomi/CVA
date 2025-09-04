using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Conta bancária empresa
    /// </summary>
    public class f60701 : BaseFormView
    {
        #region Constructor
        public f60701()
        {
            FormCount++;
        }

        public f60701(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f60701(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f60701(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.ContaBancariaEmpresa;
            TableName = "DSC1";
            CodeColumn = "AbsEntry";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.ContaBancariaEmpresa;
            TableName = "DSC1";
            CodeColumn = "AbsEntry";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
