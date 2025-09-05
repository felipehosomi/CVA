using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVA.AddOn.Common.Util;
//using CVA.View.MaxFlex.DAO;
using CVA.Escoteiro.Magento.AddOn.View;
using CVA.AddOn.Common.Controllers;
using CVA.View.MaxFlex.Model;
using System.Globalization;

namespace Picking.Producao.Addon.View
{
    [CVA.AddOn.Common.Attributes.Form(1100)]
    public class FrmTabelaPrecoSerie : BaseForm
    {
        Form Form;

        #region Constructor
        public FrmTabelaPrecoSerie()
        {
            FormCount++;
        }

        public FrmTabelaPrecoSerie(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public FrmTabelaPrecoSerie(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public FrmTabelaPrecoSerie(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show(string srfPath)
        {
            Form = (Form)base.Show(srfPath);
            Form.Freeze(true);
            try
            {
                OptionBtn op_Search = Form.Items.Item("op_Search").Specific as OptionBtn;
                op_Search.GroupWith("op_Update");
                op_Search.Selected = true;

                //ComboBox cb_Whs = Form.Items.Item("cb_Whs").Specific as ComboBox;
                //cb_Whs.AddValuesFromQuery(SQL.Deposito_Get);

                //ComboBox cb_Price = Form.Items.Item("cb_Price").Specific as ComboBox;
                //cb_Price.AddValuesFromQuery(SQL.ListaPreco_Get);
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }

            return Form;
        }

        public override bool ItemEvent()
        {
            Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID.StartsWith("op_"))
                    {
                        bool update = ItemEventInfo.ItemUID == "op_Update";

                        Form.Items.Item("bt_Save").Enabled = update;
                        Form.Items.Item("cb_Price").Enabled = update;
                        Form.Items.Item("et_Perc").Enabled = update;
                        if (!update)
                        {
                            Form.DataSources.UserDataSources.Item("ud_Perc").Value = "0";
                            Form.DataSources.UserDataSources.Item("ud_Price").Value = "";
                        }
                    }

                    if (ItemEventInfo.ItemUID == "bt_Exec")
                    {
                        this.Search();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Save")
                    {
                        this.Save();
                    }

                    if (ItemEventInfo.ItemUID.StartsWith("gr_Item"))
                    {
                        if (ItemEventInfo.ColUID == "#" && ItemEventInfo.Row == -1)
                        {
                            this.SelectAllNone();
                        }
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                {
                    if (ItemEventInfo.ItemUID == "gr_Item" && ItemEventInfo.ItemChanged)
                    {
                        this.Calculate();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID.StartsWith("op_"))
                    {
                        Form.Items.Item("et_Item").Click();
                    }
                }
            }

            return true;
        }

        private void SelectAllNone()
        {
            Form.Freeze(true);
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            string selected;
            if (dt_Item.GetValue("#", 0).ToString() == "Y")
            {
                selected = "N";
            }
            else
            {
                selected = "Y";
            }
            for (int i = 0; i < dt_Item.Rows.Count; i++)
            {
                dt_Item.SetValue("#", i, selected);
            }
            Form.Freeze(false);
        }

        private void Calculate()
        {
            try
            {
                Form.Freeze(true);
                DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                double preco = double.Parse(dt_Item.GetValue("Preço Original", ItemEventInfo.Row).ToString(),CultureInfo.CreateSpecificCulture("pt-BR"));
                
                if (preco > 0)
                {
                    if (ItemEventInfo.ColUID == "Desconto")
                    {
                        double desconto = double.Parse(dt_Item.GetValue("Desconto", ItemEventInfo.Row).ToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                        dt_Item.SetValue("Preço Novo", ItemEventInfo.Row, (preco - (preco * (desconto / 100))));
                    }
                    else
                    {
                        double precoNovo = double.Parse(dt_Item.GetValue("Preço Novo", ItemEventInfo.Row).ToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                        dt_Item.SetValue("Desconto", ItemEventInfo.Row, ((preco - precoNovo) / preco) * 100);
                    }
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

        private void Search()
        {
            try
            {
                Form.Freeze(true);
                string warehouse = Form.DataSources.UserDataSources.Item("ud_WhsCode").Value.Trim();
                string itemCode = Form.DataSources.UserDataSources.Item("ud_Item").Value.Trim();
                string priceList = Form.DataSources.UserDataSources.Item("ud_Price").Value.Trim();
                string discount = Form.DataSources.UserDataSources.Item("ud_Perc").Value.Trim().Replace(",", ".");

                if (String.IsNullOrEmpty(warehouse))
                {
                    SBOApp.Application.SetStatusBarMessage("Depósito deve ser informado");
                    return;
                }

                if (String.IsNullOrEmpty(itemCode))
                {
                    itemCode = "NULL";
                }
                else
                {
                    itemCode = $"'{itemCode}'";
                }
                if (String.IsNullOrEmpty(priceList))
                {
                    priceList = "0";
                }

                OptionBtn op_Search = Form.Items.Item("op_Search").Specific as OptionBtn;

                string pesquisa = op_Search.Selected ? "Y" : "N";
                if (!op_Search.Selected && priceList == "0")
                {
                    SBOApp.Application.SetStatusBarMessage("Lista de preço deve ser informada");
                    return;
                }

                string sql;
                if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                {
                    sql = $"CALL SP_CVA_SERIE_LISTAPRECO ('{warehouse}', {itemCode}, {priceList}, {discount}, '{pesquisa}')";
                }
                else
                {
                    sql = $"EXEC SP_CVA_SERIE_LISTAPRECO '{warehouse}', {itemCode}, {priceList}, {discount}, '{pesquisa}'";
                }

                Form.DataSources.DataTables.Item("dt_Item").ExecuteQuery(sql);

                Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                if (pesquisa == "Y")
                {
                    gr_Item.Columns.Item("#").Visible = false;
                    Form.Items.Item("gr_Item").Enabled = false;
                }
                else
                {
                    Form.Items.Item("gr_Item").Enabled = true;
                    gr_Item.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
                    gr_Item.Columns.Item("Cód. Item").Editable = false;
                    gr_Item.Columns.Item("Descrição").Editable = false;
                    gr_Item.Columns.Item("Item (Nr. Série)").Editable = false;
                    gr_Item.Columns.Item("Preço Original").Editable = false;
                }
                gr_Item.Columns.Item("SysNumber").Visible = false;
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

        private void Save()
        {
            //GridController gridController = new GridController();
            //TabelaPrecoBLL tabelaPrecoBLL = new TabelaPrecoBLL();

            //DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            //List<TabelaPrecoSerieModel> list = gridController.FillModelFromTableAccordingToValue<TabelaPrecoSerieModel>(dt_Item, false, "#", "Y");

            //string error = tabelaPrecoBLL.Save(list);
            //if (String.IsNullOrEmpty(error))
            //{
            //    SBOApp.Application.MessageBox("Preços atualizados com sucesso!");
            //    dt_Item.Clear();
            //}
        }
    }
}
