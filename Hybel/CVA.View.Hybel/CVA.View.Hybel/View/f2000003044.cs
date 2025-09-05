using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Simulador de Venda - Seleção de itens de pedido
    /// </summary>
    public class f2000003044 : BaseForm
    {
        Form Form;
        private static Form FrmSimuladorVenda;
        private static List<SimuladorVendaPedidoModel> PedidoList;

        #region Constructor
        public f2000003044()
        {
            FormCount++;
        }

        public f2000003044(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003044(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003044(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(Form frmSimuladorVenda, bool buscaPedidos)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            FrmSimuladorVenda = frmSimuladorVenda;
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            if (buscaPedidos)
            {
                PedidoList = SimuladorVendaBLL.GetPedidos();
            }
            
            dt_Item.Rows.Add(PedidoList.Count);
            int i = 0;
            foreach (var item in PedidoList)
            {
                dt_Item.SetValue("Nr. Doc.", i, item.DocNum);
                dt_Item.SetValue("Data", i, item.DocDate);
                dt_Item.SetValue("Cód. Cliente", i, item.CardCode);
                dt_Item.SetValue("Nome", i, item.CardName);
                dt_Item.SetValue("Cód. Item", i, item.ItemCode);
                dt_Item.SetValue("Descrição", i, item.ItemName);
                dt_Item.SetValue("Quantidade", i, item.Quantity);
                i++;
            }
            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
            gr_Item.AutoResizeColumns();
            Form.Freeze(false);
            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "gr_Item")
                    {
                        if (ItemEventInfo.Row >= 0 && ItemEventInfo.ColUID != "")
                        {
                            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                            if (gr_Item.Rows.IsSelected(ItemEventInfo.Row))
                            {
                                gr_Item.Rows.SelectedRows.Remove(ItemEventInfo.Row);
                            }
                            else
                            {
                                gr_Item.Rows.SelectedRows.Add(ItemEventInfo.Row);
                            }
                        }
                    }

                    if (ItemEventInfo.ItemUID == "bt_AddC")
                    {
                        this.Adicionar();
                        Form.Close();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Add")
                    {
                        this.Adicionar();
                    }
                }
            }
            return true;
        }

        private void Adicionar()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Item = FrmSimuladorVenda.Items.Item("mt_Item").Specific as Matrix;
                Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                List<int> selectedRows = new List<int>();

                for (int i = 0; i < gr_Item.Rows.SelectedRows.Count; i++)
                {
                    selectedRows.Add(gr_Item.Rows.SelectedRows.Item(i, BoOrderType.ot_RowOrder));
                    string itemCode = dt_Item.GetValue("Cód. Item", gr_Item.Rows.SelectedRows.Item(i, BoOrderType.ot_RowOrder)).ToString();
                    string itemDesc = dt_Item.GetValue("Descrição", gr_Item.Rows.SelectedRows.Item(i, BoOrderType.ot_RowOrder)).ToString();
                    double quantidade = Convert.ToDouble(dt_Item.GetValue("Quantidade", gr_Item.Rows.SelectedRows.Item(i, BoOrderType.ot_RowOrder)));

                    SimuladorVendaItemModel itemModel = f2000003039.ItemList.FirstOrDefault(s => s.ItemCode == itemCode);
                    if (itemModel != null)
                    {
                        itemModel.Quantidade += quantidade;
                        ((EditText)mt_Item.GetCellSpecific("cl_Qtde", itemModel.Linha)).Value = itemModel.Quantidade.ToString().Replace(",", ".");
                    }
                    else
                    {
                        DBDataSource ds_Item = FrmSimuladorVenda.DataSources.DBDataSources.Item("@CVA_SIM_VENDA_ITEM");
                        ds_Item.Clear();

                        if (mt_Item.RowCount == 0 || !String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value.Trim()))
                        {
                            mt_Item.AddRow();
                        }
                        mt_Item.FlushToDataSource();

                        itemModel = new SimuladorVendaItemModel();
                        itemModel.ItemCode = itemCode;
                        itemModel.Linha = mt_Item.RowCount;
                        itemModel.Quantidade += quantidade;
                        f2000003039.ItemList.Add(itemModel);

                        ((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value = itemModel.ItemCode;
                        ((EditText)mt_Item.GetCellSpecific("cl_Desc", mt_Item.RowCount)).Value = itemDesc;
                        ((EditText)mt_Item.GetCellSpecific("cl_Qtde", itemModel.Linha)).Value = itemModel.Quantidade.ToString().Replace(",", ".");
                    }
                }

                for (int i = selectedRows.Count - 1; i >= 0; i--)
                {
                    PedidoList.RemoveAt(selectedRows[i]);
                    dt_Item.Rows.Remove(selectedRows[i]);
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
    }
}
