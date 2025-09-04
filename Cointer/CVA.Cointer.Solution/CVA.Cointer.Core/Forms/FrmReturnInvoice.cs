using CVA.Cointer.Core.BLL;
using CVA.Cointer.Core.DAO;
using CVA.Cointer.Core.Model;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Cointer.Core.Forms
{
    class FrmReturnInvoice : BaseForm
    {
        public FrmReturnInvoice()
        {
            FormCount++;
        }

        public FrmReturnInvoice(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public object Show(List<string> list)
        {
            Form form = (Form)base.Show();
            form.Freeze(true);
            form.DataSources.UserDataSources.Item("ud_Group").Value = "Y";

            string sql = String.Format(Hana.Invoice_GetPendingItems, string.Join(",", list));
            form.DataSources.DataTables.Item("dt_Invoice").ExecuteQuery(sql);

            Grid gr_Invoice = (Grid)Form.Items.Item("gr_Invoice").Specific;
            gr_Invoice.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
            gr_Invoice.Columns.Item("DocEntry").Visible = false;
            gr_Invoice.Columns.Item("DocNum").Visible = false;
            gr_Invoice.Columns.Item("CardCode").Visible = false;
            gr_Invoice.Columns.Item("LineNum").Visible = false;
            gr_Invoice.Columns.Item("TaxCode").Visible = false;
            gr_Invoice.Columns.Item("Usage").Visible = false;
            gr_Invoice.Columns.Item("WhsCode").Visible = false;
            //gr_Invoice.Columns.Item("Price").Visible = false;
            gr_Invoice.Columns.Item("RN").Visible = false;
            //gr_Invoice.Columns.Item("StockPrice").Visible = false;
            

            for (int i = 1; i < gr_Invoice.Columns.Count; i++)
            {
                gr_Invoice.Columns.Item(i).Editable = false;
            }
            gr_Invoice.Columns.Item("Quantidade").Editable = true;

            for (int i = 0; i < gr_Invoice.Columns.Count; i++)
            {
                gr_Invoice.Columns.Item(i).TitleObject.Sortable = true;
            }

            form.DataSources.UserDataSources.Item("ud_Check").Value = "Y";

            form.Freeze(false);
            return form;
        }

        public override bool ItemEvent()
        {
            try
            {
                if (!ItemEventInfo.BeforeAction)
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                    {
                        Form.Freeze(true);
                        if (ItemEventInfo.ItemUID == "bt_Gen")
                        {
                            this.Generate();
                        }
                        if (ItemEventInfo.ItemUID == "cb_Check")
                        {
                            this.SelectAllNone();
                        }
                    }
                }
                else
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                    {
                        if (ItemEventInfo.ColUID == "Quantidade" && ItemEventInfo.ItemChanged)
                        {
                            DataTable dt_Invoice = Form.DataSources.DataTables.Item("dt_Invoice");
                            if ((double)dt_Invoice.GetValue("Quantidade", ItemEventInfo.Row) > (double)dt_Invoice.GetValue("Disponível", ItemEventInfo.Row))
                            {
                                SBOApp.Application.SetStatusBarMessage("Quantidade não deve ser superior ao disponível");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                try
                {
                    Form.Freeze(false);
                }
                catch { }
            }

            return true;
        }

        private void Generate()
        {
            List<InvoiceItemModel> list = Form.DataSources.DataTables.Item("dt_Invoice").FillModelByColumnValue<InvoiceItemModel>();
            if (list.Count == 0)
            {
                SBOApp.Application.SetStatusBarMessage("Nenhum item selecionado");
                return;
            }

            ReturnInvoiceModel returnInvoiceModel = new ReturnInvoiceModel();
            if (!Form.ValidateAndFillModelByUserDataSource<ReturnInvoiceModel>(ref returnInvoiceModel))
            {
                SBOApp.Application.SetStatusBarMessage(SBOApp.StatusBarMessage);
                return;
            }

            ReturnInvoiceBLL returnInvoiceBLL = new ReturnInvoiceBLL();
            string error = returnInvoiceBLL.Generate(returnInvoiceModel, list);
            if (!String.IsNullOrEmpty(error))
            {
                SBOApp.Application.SetStatusBarMessage(error);
            }
            else
            {
                Form.Close();
            }
        }

        private void SelectAllNone()
        {
            DataTable table = Form.DataSources.DataTables.Item("dt_Invoice");
            if (table.Rows.Count > 0)
            {
                string selected = Form.DataSources.UserDataSources.Item("ud_Check").Value;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.SetValue("#", i, selected);
                }
            }
        }
    }
}