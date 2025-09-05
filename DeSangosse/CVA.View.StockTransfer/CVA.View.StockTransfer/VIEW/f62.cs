using CVA.AddOn.Common.Forms;
using CVA.View.StockTransfer.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.VIEW
{
    public class f62 : BaseForm
    {
        #region Constructor
        public f62()
        {
            FormCount++;
        }

        public f62(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f62(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f62(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region
        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.ActionSuccess)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    DBDataSource ds_OWHS = Form.DataSources.DBDataSources.Item("OWHS");
                    string transferOut = ds_OWHS.GetValue("U_CVA_Transfer_Out", ds_OWHS.Offset);
                    if (transferOut == "Y")
                    {
                        WarehouseBLL.UpdateWhsTransferDefault(ds_OWHS.GetValue("WhsCode", ds_OWHS.Offset));
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
