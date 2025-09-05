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
    public class f81 : BaseForm
    {
        #region Constructor
        public f81()
        {
            FormCount++;
        }

        public f81(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f81(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f81(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.ItemUID == "11")
                {
                    Matrix mt_Pedido = Form.Items.Item("10").Specific as Matrix;
                    for (int i = 1; i <= mt_Pedido.RowCount; i++)
                    {
                        if (((CheckBox)mt_Pedido.GetCellSpecific("1", i)).Checked)
                        {
                            string docNum = ((EditText)mt_Pedido.GetCellSpecific("11", i)).Value;
                            PedidoVendaBLL pedidoVendaBLL = new PedidoVendaBLL();
                            string msg = pedidoVendaBLL.ValidaLotes(docNum);
                            if (!String.IsNullOrEmpty(msg))
                            {
                                msg = $"Pedido {docNum}{Environment.NewLine}{msg}";
                                SBOApp.Application.MessageBox(msg);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
