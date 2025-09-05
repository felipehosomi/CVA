using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Pedido Venda
    /// </summary>
    public class f139 : DocumentBaseView
    {
        #region Constructor
        public f139()
        {
            FormCount++;
        }

        public f139(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f139(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f139(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
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

        public static bool BlockPasting(Form form)
        {
            Matrix mt_Item = form.Items.Item("38").Specific as Matrix;
            CellPosition position = mt_Item.GetCellFocus();
            try
            {
                if (mt_Item.Columns.Item(position.ColumnIndex).UniqueID == "14")
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
            return true;
        }
        #endregion
    }
}
