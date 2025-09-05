using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Hub.Logic.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f10016 : BaseForm
    {
        #region Constructor
        public f10016()
        {
            FormCount++;
        }

        public f10016(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f10016(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f10016(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        return ValidaLote();
                    }
                }
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "7")
                    {
                        return ValidaLote();
                    }
                }
            }
            return true;
        }

        private bool ValidaLote()
        {
            Matrix mt_Pedido = Form.Items.Item("7").Specific as Matrix;
            int selectedRow = 0;
            while (selectedRow >= 0)
            {
                selectedRow = mt_Pedido.GetNextSelectedRow(selectedRow);
                if (selectedRow < 0)
                {
                    break;
                }
                string docNum = ((EditText)mt_Pedido.GetCellSpecific("DocNum", selectedRow)).Value;
                PedidoVendaBLL pedidoVendaBLL = new PedidoVendaBLL();
                string msg = pedidoVendaBLL.ValidaLotes(docNum);
                if (!String.IsNullOrEmpty(msg))
                {
                    msg = $"Pedido {docNum}{Environment.NewLine}{msg}";
                    SBOApp.Application.MessageBox(msg);
                    return false;
                }
            }
            return true;
        }
    }
}
