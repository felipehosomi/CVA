namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Cotação
    /// </summary>
    public class f149 : DocumentBaseView
    {
        #region Constructor
        public f149()
        {
            FormCount++;
        }

        public f149(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f149(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f149(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "14")
                {
                    if (ItemEventInfo.CharPressed != 9)
                    {
                        if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_KEY_DOWN)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
