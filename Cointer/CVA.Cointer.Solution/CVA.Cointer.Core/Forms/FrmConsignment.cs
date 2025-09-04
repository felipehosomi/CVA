using CVA.Cointer.Core.DAO;
using CVA.Cointer.Core.Model;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Cointer.Core.Forms
{
    class FrmConsignment : BaseForm
    {
        public FrmConsignment()
        {
            FormCount++;
        }

        public FrmConsignment(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public override object Show()
        {
            Form form = (Form)base.Show();

            ChooseFromList cf_BPCode = Form.ChooseFromLists.Item("cf_BPCode");
            var oCons = cf_BPCode.GetConditions();
            var oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "C";
            cf_BPCode.SetConditions(oCons);

            ChooseFromList cf_BPName = Form.ChooseFromLists.Item("cf_BPName");
            oCons = cf_BPName.GetConditions();
            oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "C";
            cf_BPName.SetConditions(oCons);

            form.DataSources.UserDataSources.Item("ud_Check").Value = "Y";

            return form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                try
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                    {
                        Form.Freeze(true);
                        if (ItemEventInfo.ItemUID == "bt_Search")
                        {
                            this.Search();
                        }
                        if (ItemEventInfo.ItemUID == "bt_Next")
                        {
                            this.Next();
                        }
                        if (ItemEventInfo.ItemUID == "cb_Check")
                        {
                            this.SelectAllNone();
                        }
                    }
                    else if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                    {
                        var cflEvent = (IChooseFromListEvent)ItemEventInfo;
                        var cflId = cflEvent.ChooseFromListUID;

                        var dataTable = cflEvent.SelectedObjects;
                        if (dataTable != null)
                        {
                            Form.DataSources.UserDataSources.Item("ud_BPCode").Value = dataTable.GetValue("CardCode", 0).ToString();
                            Form.DataSources.UserDataSources.Item("ud_BPName").Value = dataTable.GetValue("CardName", 0).ToString();
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

            return true;
        }

        private void Search()
        {
            string cardCode = Form.DataSources.UserDataSources.Item("ud_BPCode").Value;
            DateTime dateFrom;
            DateTime dateTo;

            string invoice = Form.DataSources.UserDataSources.Item("ud_Inv").Value;

            if (String.IsNullOrEmpty(cardCode))
            {
                SBOApp.Application.SetStatusBarMessage("Cliente deve ser informado");
                return;
            }
            if (String.IsNullOrEmpty(invoice))
            {
                invoice = "0";
            }

            if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtFrom").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateFrom))
            {
                SBOApp.Application.SetStatusBarMessage("Data inicial deve ser informada");
                return;
            }
            if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtTo").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTo))
            {
                SBOApp.Application.SetStatusBarMessage("Data final deve ser informada");
                return;
            }

            string sql = String.Format(Hana.Invoice_Get, cardCode, invoice, dateFrom.ToString("yyyyMMdd"), dateTo.ToString("yyyyMMdd"));
            Form.DataSources.DataTables.Item("dt_Invoice").ExecuteQuery(sql);

            Grid gr_Invoice = (Grid)Form.Items.Item("gr_Invoice").Specific;
            gr_Invoice.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;

            EditTextColumn cl_Id = (EditTextColumn)gr_Invoice.Columns.Item("Id Doc");
            cl_Id.LinkedObjectType = "13";

            for (int i = 1; i < gr_Invoice.Columns.Count; i++)
            {
                gr_Invoice.Columns.Item(i).Editable = false;
            }

            gr_Invoice.AutoResizeColumns();

            for (int i = 0; i < gr_Invoice.Columns.Count; i++)
            {
                gr_Invoice.Columns.Item(i).TitleObject.Sortable = true;
            }
        }

        private void Next()
        {
            List<string> list = Form.DataSources.DataTables.Item("dt_Invoice").FillStringListByColumnValue("Id Doc", "#", "Y");
            if (list.Count == 0)
            {
                SBOApp.Application.SetStatusBarMessage("Nenhuma NF selecionada");
                return;
            }

            FrmReturnInvoice frmReturnInvoice = new FrmReturnInvoice();
            frmReturnInvoice.Show(list);
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
