using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f25 : BaseForm
    {
        private Form form = null;

        #region Constructor
        public f25()
        {
            FormCount++;
        }

        public f25(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f25(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f25(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show()

        public object Show()
        {
            return null;
        }

        public object Show(string[] args)
        {
            try
            {
                form = SBOApp.Application.OpenForm(BoFormObjectEnum.fo_SerialNumbersForItems, "", args[0]);
            }
            catch
            {
                form = SBOApp.Application.OpenForm(BoFormObjectEnum.fo_ItemBatchNumbers, "", args[0]);
            }
            return form;
        }
        #endregion

        public override bool ItemEvent()
        {
            IsSystemForm = true;
            base.ItemEvent();

            if(ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "1" && ItemEventInfo.BeforeAction && ItemEventInfo.FormMode == 2)
            {
                var Matrix = (Matrix)Form.Items.Item("3").Specific;

                for (var i = 1; i <= Matrix.VisualRowCount; i++)
                {
                    var pedido = ((EditText)Matrix.GetCellSpecific("4", i)).Value.ToString().Replace(".", ",");
                    var efetuado = ((EditText)Matrix.GetCellSpecific("5", i)).Value.ToString().Replace(".", ",");
                    var dPedido = decimal.Parse(pedido);
                    var dEfetuado = decimal.Parse(efetuado);

                    if (dEfetuado < dPedido)
                    {
                        SBOApp.Application.MessageBox($"Linha {i}: Favor selecionar a quantidade total de lotes do item.");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
