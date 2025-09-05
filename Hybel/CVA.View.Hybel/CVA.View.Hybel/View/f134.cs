using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Parceiro de Negócio
    /// </summary>
    public class f134 : BaseForm
    {
        #region Constructor
        public f134()
        {
            FormCount++;
        }

        public f134(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f134(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f134(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ChooseFromList
        private void ChooseFromList()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    string cardCode = oDataTable.GetValue("CardCode", 0).ToString();
                }
            }
        }
        #endregion
    }
}
