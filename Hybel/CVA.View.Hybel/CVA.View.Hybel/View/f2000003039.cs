using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Simulador Vendas
    /// </summary>
    public class f2000003039 : BaseForm
    {
        private Form Form;
        public static List<SimuladorVendaItemModel> ItemList;
        public static bool BuscaPedidos;

        #region Constructor
        public f2000003039()
        {
            FormCount++;
        }

        public f2000003039(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003039(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003039(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public override object Show()
        {
            BuscaPedidos = true;
            ItemList = new List<SimuladorVendaItemModel>();
            Form = (Form)base.Show();
            Form.Freeze(true);

            Form.Items.Item("1").Visible = true;
            if (!SimuladorVendaBLL.Exists())
            {
                Form.Mode = BoFormMode.fm_ADD_MODE;
                ((EditText)Form.Items.Item("et_Code").Specific).Value = SBOApp.Company.UserName;

                Form.Items.Item("1").Click();
            }

            Form.Mode = BoFormMode.fm_FIND_MODE;
            Form.Items.Item("et_Code").Visible = true;
            ((EditText)Form.Items.Item("et_Code").Specific).Value = SBOApp.Company.UserName;
            Form.Items.Item("1").Click();

            Form.Items.Item("et_Code").Visible = false;
            Form.Items.Item("1").Visible = false;
            this.LimpaTela();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.Columns.Item(0).Width = 0;

            Form.Freeze(false);
            return Form;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_RESIZE)
                {
                    try
                    {
                        Form.Freeze(true);
                        Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                        if (Form.Width <= 800)
                        {
                            mt_Item.Item.Width = 430;
                            mt_Item.AutoResizeColumns();
                            mt_Item.Columns.Item(0).Width = 0;
                        }
                        mt_Item.Item.Height = 100;
                    }
                    catch { }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    switch (ItemEventInfo.ItemUID)
                    {
                        case "bt_AddIt":
                            this.AdicionaItem();
                            break;
                        case "bt_AddPed":
                            this.AdicionaPedido();
                            break;
                        case "bt_Gen":
                            this.Gerar();
                            break;
                        case "bt_Rest":
                            BuscaPedidos = true;
                            this.LimpaTela();
                            break;
                        case "bt_Demo":
                            this.Demonstrativo();
                            break;
                        case "bt_Cons":
                            this.Consulta();
                            break;
                        case "gr_Item":
                            Grid gr_Item = (Grid)Form.Items.Item("gr_Item").Specific;
                            if (ItemEventInfo.Row >= 0)
                            {
                                gr_Item.Rows.SelectedRows.Add(ItemEventInfo.Row);
                            }
                            break;
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    if (ItemEventInfo.ItemUID == "et_Item" || ItemEventInfo.ItemUID == "et_Desc")
                    {
                        this.OnItemChooseFromList();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 13)
                    {
                        switch (ItemEventInfo.ItemUID)
                        {
                            case "et_Item":
                            case "et_Desc":
                            case "et_Qtde":
                                this.AdicionaItem();
                                break;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Gerar
        private void Gerar()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                if (mt_Item.RowCount == 0 || String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value.Trim()))
                {
                    SBOApp.Application.SetStatusBarMessage("Nenhum item informado!");
                    return;
                }
                if (Form.Mode != BoFormMode.fm_UPDATE_MODE)
                {
                    Form.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                Form.Items.Item("1").Visible = true;
                Form.Items.Item("1").Click();
                Form.Items.Item("1").Visible = false;

                DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                string code = ((EditText)Form.Items.Item("et_Code").Specific).Value;
                dt_Item.ExecuteQuery(SimuladorVendaBLL.GetSimulacaoSQL(code));

                Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                gr_Item.RowHeaders.Width = 10;
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
        #endregion

        #region AdicionaPedido
        private void AdicionaPedido()
        {
            new f2000003044().Show(Form, BuscaPedidos);
            BuscaPedidos = false;
        }
        #endregion

        #region AdicionaItem
        private void AdicionaItem()
        {
            if (!String.IsNullOrEmpty(((EditText)Form.Items.Item("et_Item").Specific).Value.Trim()))
            {
                Form.Freeze(true);
                try
                {
                    double qtde;
                    double.TryParse(((EditText)Form.Items.Item("et_Qtde").Specific).Value, out qtde);
                    if (qtde == 0)
                    {
                        int retorno = SBOApp.Application.MessageBox("Quantidade informada é zero, deseja continuar?", 1, "Sim", "Não");
                        if (retorno != 1)
                        {
                            Form.Items.Item("et_Qtde").Click();
                            return;
                        }
                    }
                    Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                    SimuladorVendaItemModel itemModel = ItemList.FirstOrDefault(i => i.ItemCode == ((EditText)Form.Items.Item("et_Item").Specific).Value);
                    if (itemModel != null)
                    {
                        int retorno = SBOApp.Application.MessageBox("Item já informado, deseja adicionar a quantidade informada?", 1, "Sim", "Não");
                        if (retorno != 1)
                        {
                            Form.Items.Item("et_Item").Click();
                            return;
                        }
                        itemModel.Quantidade += Convert.ToDouble(((EditText)Form.Items.Item("et_Qtde").Specific).Value.Replace(".", ","));

                        ((EditText)mt_Item.GetCellSpecific("cl_Qtde", itemModel.Linha)).Value = itemModel.Quantidade.ToString().Replace(",", ".");
                    }
                    else
                    {
                        string itemName = ((EditText)Form.Items.Item("et_Desc").Specific).Value.Trim();
                        if (String.IsNullOrEmpty(itemName))
                        {
                            itemName = ItemBLL.GetItemName(((EditText)Form.Items.Item("et_Item").Specific).Value);
                        }
                        if (String.IsNullOrEmpty(itemName.Trim()))
                        {
                            SBOApp.Application.SetStatusBarMessage("Item não encontrado!");
                            Form.Items.Item("et_Item").Click();
                            return;
                        }

                        DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_SIM_VENDA_ITEM");
                        ds_Item.Clear();

                        if (mt_Item.RowCount == 0 || !String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value.Trim()))
                        {
                            mt_Item.AddRow();
                        }
                        mt_Item.FlushToDataSource();

                        itemModel = new SimuladorVendaItemModel();
                        itemModel.ItemCode = ((EditText)Form.Items.Item("et_Item").Specific).Value;
                        itemModel.Linha = mt_Item.RowCount;
                        itemModel.Quantidade += Convert.ToDouble(((EditText)Form.Items.Item("et_Qtde").Specific).Value.Replace(".", ","));
                        ItemList.Add(itemModel);

                        ((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value = itemModel.ItemCode;
                        ((EditText)mt_Item.GetCellSpecific("cl_Desc", mt_Item.RowCount)).Value = itemName;
                        ((EditText)mt_Item.GetCellSpecific("cl_Qtde", itemModel.Linha)).Value = itemModel.Quantidade.ToString().Replace(",", ".");
                    }
                    this.LimpaCampos();
                    Form.Items.Item("et_Item").Click();
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
            else
            {
                SBOApp.Application.SetStatusBarMessage("Informe o código do item!");
            }
        }
        #endregion

        #region Limpar
        private void LimpaCampos()
        {
            Form.DataSources.UserDataSources.Item("ud_Item").Value = String.Empty;
            Form.DataSources.UserDataSources.Item("ud_Desc").Value = String.Empty;
            Form.DataSources.UserDataSources.Item("ud_Qtde").Value = "0";
            Form.DataSources.UserDataSources.Item("ud_Pedido").Value = String.Empty;
        }

        private void LimpaTela()
        {
            this.LimpaCampos();
            Form.DataSources.DataTables.Item("dt_Item").Clear();

            DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_SIM_VENDA_ITEM");
            ds_Item.Clear();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.LoadFromDataSource();
        }
        #endregion

        #region Demonstrativo/Consulta
        private ItemModel FillItemModel()
        {
            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
            if (gr_Item.Rows.SelectedRows != null && gr_Item.Rows.SelectedRows.Count > 0)
            {
                int rowIndex = gr_Item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
                DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                ItemModel itemModel = new ItemModel();
                itemModel.ItemCode = dt_Item.GetValue("Código", rowIndex).ToString();
                itemModel.ItemName = dt_Item.GetValue("Produto", rowIndex).ToString();
                itemModel.EstoqueFisico = Convert.ToDouble(dt_Item.GetValue("Est Fís", rowIndex));
                itemModel.EstoqueEncomendado = Convert.ToDouble(dt_Item.GetValue("Est Enc", rowIndex));
                itemModel.EstoqueReservado = Convert.ToDouble(dt_Item.GetValue("Est Res", rowIndex));
                itemModel.EstoqueDisponivel = Convert.ToDouble(dt_Item.GetValue("Est Dis", rowIndex));
                itemModel.EstoqueMinimo = Convert.ToDouble(dt_Item.GetValue("Est Mín", rowIndex));

                return itemModel;
            }
            return null;
        }

        private void Demonstrativo()
        {
            ItemModel itemModel = this.FillItemModel();
            if (itemModel != null)
            {
                new f2000003040().Show(itemModel);
            }
        }

        private void Consulta()
        {
            ItemModel itemModel = this.FillItemModel();
            if (itemModel != null)
            {
                new f2000003041().Show(itemModel);
            }
        }
        #endregion

        #region OnItemChooseFromList
        private void OnItemChooseFromList()
        {
            var oCFLEvento = (IChooseFromListEvent)ItemEventInfo;
            string sCFL_ID = oCFLEvento.ChooseFromListUID;

            if (!oCFLEvento.BeforeAction)
            {
                DataTable oDataTable = oCFLEvento.SelectedObjects;
                string itemCode = null;
                string itemName = null;

                try
                {
                    itemCode = Convert.ToString(oDataTable.GetValue("ItemCode", 0));
                    itemName = Convert.ToString(oDataTable.GetValue("ItemName", 0));

                    Form.DataSources.UserDataSources.Item("ud_Item").Value = itemCode;
                    Form.DataSources.UserDataSources.Item("ud_Desc").Value = itemName;
                }
                catch { }
            }
        }
        #endregion

        #region RightClickEvent
        public override bool RightClickEvent()
        {
            Form = SBOApp.Application.Forms.ActiveForm;
            if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE || Form.Mode == BoFormMode.fm_OK_MODE)
            {
                if (ContextMenuInfo.BeforeAction && ContextMenuInfo.EventType == BoEventTypes.et_RIGHT_CLICK)
                {
                    if (ContextMenuInfo.ItemUID == "mt_Item")
                    {
                        if (ContextMenuInfo.Row >= 0)
                        {
                            this.CreateRightClickMenuItem();
                            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                            mt_Item.SelectRow(ContextMenuInfo.Row, true, false);
                        }
                    }
                    else
                    {
                        // Remove o menu, se não ele aparecerá sempre
                        if (Form.Menu.Exists("M3939"))
                        {
                            Form.Menu.RemoveEx("M3939");
                        }
                    }
                }
            }
            return true;
        }

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("M3939"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "M3939"; // Classe a ser chamada
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }
        #endregion
    }
}
