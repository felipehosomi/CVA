using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVA.View.Hybel.View
{
    public class f540000988 : BaseForm
    {
        #region Constructor
        public f540000988()
        {
            FormCount++;
        }

        public f540000988(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f540000988(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f540000988(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            IsSystemForm = true;
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Simul")
                {
                    Matrix mt_Item = Form.Items.Item("38").Specific as Matrix;
                    List<ItemModel> itemList = new List<ItemModel>();
                    for (int i = 1; i <= mt_Item.RowCount; i++)
                    {
                        ItemModel itemModel = new ItemModel();
                        itemModel.ItemCode = ((EditText)mt_Item.GetCellSpecific("1", i)).Value;
                        if (!String.IsNullOrEmpty(itemModel.ItemCode.Trim()))
                        {
                            itemModel.TaxaMoeda = Convert.ToDouble(((EditText)Form.Items.Item("64").Specific).Value.Replace(".", ","));
                            itemModel.Linha = Convert.ToInt32(((EditText)mt_Item.GetCellSpecific("0", i)).Value);
                            itemModel.Imposto = ((EditText)mt_Item.GetCellSpecific("160", i)).Value;
                            itemModel.Quantidade = Convert.ToDouble(((EditText)mt_Item.GetCellSpecific("11", i)).Value.Replace(".",","));

                            string peso = ((EditText)mt_Item.GetCellSpecific("58", i)).Value;
                            peso = Regex.Replace(peso, "[^0-9,]", "").Replace(".", ",");
                            if (!String.IsNullOrEmpty(peso))
                            {
                                itemModel.Peso = Convert.ToDouble(peso);
                            }

                            string precoUnitario = ((EditText)mt_Item.GetCellSpecific("14", i)).Value;
                            precoUnitario = Regex.Replace(precoUnitario, "[^0-9,]", "").Replace(".", ",");
                            if (!String.IsNullOrEmpty(precoUnitario))
                            {
                                itemModel.PrecoUnitarioUSD = Convert.ToDouble(precoUnitario);
                            }

                            itemList.Add(itemModel);
                        }
                    }

                    if (itemList.Count > 0)
                    {
                        new f2000003038().Show(itemList);
                    }
                    else
                    {
                        SBOApp.Application.SetStatusBarMessage("Nenhum item informado!");
                    }
                }
            }
            return true;
        }
    }
}
