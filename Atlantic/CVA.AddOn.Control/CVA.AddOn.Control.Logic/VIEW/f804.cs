using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Plano de Contas
    /// </summary>
    public class f804 : BaseFormView
    {
        #region Constructor
        public f804()
        {
            FormCount++;
        }

        public f804(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f804(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f804(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.PlanoContas;
            TableName = "OACT";
            CodeColumn = "AcctCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "43")
                {
                    if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                    {
                        f805.AcctCode = ((EditText)Form.Items.Item("13").Specific).Value;
                    }
                }
            }

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.PlanoContas;
            TableName = "OACT";
            CodeColumn = "AcctCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
