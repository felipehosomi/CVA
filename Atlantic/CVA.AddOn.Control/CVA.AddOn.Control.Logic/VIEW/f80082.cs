using CVA.AddOn.Common;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Utilização
    /// </summary>
    public class f80082 : BaseFormView
    {
        #region Constructor
        public f80082()
        {
            FormCount++;
        }

        public f80082(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f80082(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f80082(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.Utilizacao;
            TableName = "OUSG";
            CodeColumn = "ID";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            if (ItemEventInfo.BeforeAction)
            {
                if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                {
                    switch (ItemEventInfo.EventType)
                    {
                        case BoEventTypes.et_KEY_DOWN:
                        case BoEventTypes.et_CLICK:
                            if (ItemEventInfo.ItemUID == "3" && ItemEventInfo.ColUID != "FreeChrgBP")
                            {
                                return false;
                            }
                            break;
                    }
                }
            }


            return base.ItemEvent();
        }

        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.Utilizacao;
            TableName = "OUSG";
            CodeColumn = "ID";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Replicadora)
            {
                return base.FormDataEvent();
            }
            return true;
        }
        #endregion
    }
}
