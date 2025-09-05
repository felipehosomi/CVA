using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using SAPbouiCOM;
using System;
using CVA.AddOn.Common.Util;
using CVA.View.Hybel.DAO.Resources;
using System.Globalization;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Transferência Entre Filiais - Pedido de Venda
    /// </summary>
    public class f2000003048 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003048()
        {
            FormCount++;
        }

        public f2000003048(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003048(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003048(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();

            ComboBox cb_Filial = Form.Items.Item("cb_Filial").Specific as ComboBox;
            cb_Filial.AddValuesFromQuery(SQL.Filial_Get);
            cb_Filial.Select(0, BoSearchKey.psk_Index);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "gr_Item" && ItemEventInfo.ColUID == "Gerar" && ItemEventInfo.Row == -1)
                    {
                        try
                        {
                            Form.Freeze(true);
                            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                            string newValue = String.Empty;
                            if (dt_Item.Rows.Count > 0)
                            {
                                newValue = dt_Item.GetValue("Gerar", 0).ToString() == "Y" ? "N" : "Y";
                            }
                            for (int i = 0; i < dt_Item.Rows.Count; i++)
                            {
                                dt_Item.SetValue("Gerar", i, newValue);
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

                    if (ItemEventInfo.ItemUID == "bt_Search")
                    {
                        try
                        {
                            Form.Freeze(true);
                            int filial;
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_Filial").Value, out filial);
                            string sql = PedidoVendaBLL.GetTransferenciaFiliaisSQL(filial);
                            Form.DataSources.DataTables.Item("dt_Item").ExecuteQuery(sql);
                            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                            gr_Item.Columns.Item("Gerar").Type = BoGridColumnType.gct_CheckBox;
                            for (int i = 1; i < gr_Item.Columns.Count; i++)
                            {
                                gr_Item.Columns.Item(i).Editable = false;
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
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        try
                        {
                            Form.Freeze(true);
                            int filial;
                            DateTime dataEntrega;
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_Filial").Value, out filial);
                            if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_Data").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out dataEntrega))
                            {
                                SBOApp.Application.SetStatusBarMessage("Campo Data Entrega deve ser informada!");
                                return false;
                            }

                            string tipoQtde = Form.DataSources.UserDataSources.Item("ud_Qtde").Value.Trim();
                            if (String.IsNullOrEmpty(tipoQtde))
                            {
                                SBOApp.Application.SetStatusBarMessage("Campo Quantidades deve ser informado!");
                                return false;
                            }

                            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                            string error = PedidoVendaBLL.GerarTransferenciaFiliais(filial, dataEntrega, tipoQtde, dt_Item);
                            if (!String.IsNullOrEmpty(error))
                            {
                                SBOApp.Application.SetStatusBarMessage(error);
                            }
                            else
                            {
                                SBOApp.Application.MessageBox("Pedido de Venda gerado com sucesso!");
                                dt_Item.Clear();
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

            return true;
        }
    }
}
