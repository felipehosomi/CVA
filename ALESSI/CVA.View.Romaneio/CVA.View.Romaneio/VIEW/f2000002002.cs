using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.Romaneio.BLL;
using CVA.View.Romaneio.DAO.Resources;
using CVA.View.Romaneio.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Filtro NF's
    /// </summary>
    public class f2000002002 : BaseForm
    {
        private Form Form;
        private static string WaybillCode = String.Empty;

        #region Constructor
        public f2000002002()
        {
            FormCount++;
        }

        public f2000002002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public object Show(string waybillCode = "", BoFormMode formMode = BoFormMode.fm_ADD_MODE)
        {
            Form = (Form)base.Show();
            Form.Mode = formMode;
            this.SetCflBranchCondition();
            this.SetCflCarrierCondition();

            WaybillCode = waybillCode;
            if (!String.IsNullOrEmpty(WaybillCode))
            {
                ((Button)Form.Items.Item("bt_Next").Specific).Caption = "OK";
            }

            ComboBox cb_State = (ComboBox)Form.Items.Item("cb_State").Specific;
            cb_State.AddValuesFromQuery(Query.State_Get, "Code", "Name");

            return Form;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    switch (ItemEventInfo.ItemUID)
                    {
                        case "bt_Search":
                            this.Search();
                            break;
                        case "bt_Next":
                            this.AddInvoices();
                            break;
                        case "gr_NF":
                            if (ItemEventInfo.Row == -1 && ItemEventInfo.ColUID == "Selecionar")
                            {
                                this.SelectAllNone();
                            }
                            break;
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (ItemEventInfo.ItemUID == "et_CflBP")
                    {
                        this.OnChooseFromListBranch();
                    }
                    if (ItemEventInfo.ItemUID == "et_Carrier")
                    {
                        this.OnChooseFromListCarrier();
                    }
                }
            }
            return true;
        }
        #endregion

        #region Private Methods
        private void SelectAllNone()
        {
            Form.Freeze(true);
            DataTable dt_NF = Form.DataSources.DataTables.Item("dt_NF");
            string selected;
            if (dt_NF.GetValue("Selecionar", 0).ToString() == "Y")
            {
                selected = "N";
            }
            else
            {
                selected = "Y";
            }
            for (int i = 0; i < dt_NF.Rows.Count; i++)
            {
                dt_NF.SetValue("Selecionar", i, selected);
            }
            Form.Freeze(false);
        }

        private void AddInvoices()
        {
            GridController gridController = new GridController();
            DataTable dt_NF = Form.DataSources.DataTables.Item("dt_NF");

            List<InvoiceModel> invoiceList = gridController.FillModelFromTableAccordingToValue<InvoiceModel>(dt_NF, false, "Selecionar", "Y");
            if (invoiceList.Count == 0)
            {
                SBOApp.Application.SetStatusBarMessage("Nenhuma NF selecionada!");
                return;
            }
            if (String.IsNullOrEmpty(WaybillCode))
            {
                new f2000002003().Show(invoiceList);
            }
            else
            {
                for (int i = 0; i < SBOApp.Application.Forms.Count; i++)
                {
                    Form frmWaybill = SBOApp.Application.Forms.Item(i);
                    if (frmWaybill.Type == 2000002003)
                    {
                        if (WaybillCode == f2000002003.GetCode(frmWaybill))
                        {
                            if (frmWaybill.Mode != BoFormMode.fm_UPDATE_MODE)
                            {
                                frmWaybill.Mode = BoFormMode.fm_UPDATE_MODE;
                            }
                            f2000002003.AddInvoices(frmWaybill, invoiceList);
                            break;
                        }
                    }
                }
            }
            Form.Close();
        }

        private void Search()
        {
            Form.Freeze(true);
            try
            {
                ValidationController validationController = new ValidationController();
                InvoiceFilterModel filterModel = validationController.FillModel<InvoiceFilterModel>(this.Form);
                string sql = InvoiceBLL.GetQuery(filterModel);

                Form.DataSources.DataTables.Item("dt_NF").ExecuteQuery(sql);

                Grid gr_NF = (Grid)Form.Items.Item("gr_NF").Specific;
                gr_NF.Columns.Item("Selecionar").Type = BoGridColumnType.gct_CheckBox;
                for (int i = 1; i < gr_NF.Columns.Count; i++)
                {
                    gr_NF.Columns.Item(i).Editable = false;
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

        private void OnChooseFromListBranch()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    EditText et_Branch = (EditText)Form.Items.Item("et_Branch").Specific;
                    et_Branch.Value = String.Empty;
                    string values = String.Empty;
                    for (int i = 0; i < oDataTable.Rows.Count; i++)
                    {
                        int bplId = Convert.ToInt32(oDataTable.GetValue("BPLId", i));
                        values += ", " + bplId;
                    }
                    values = values.Substring(2);
                    try
                    {
                        et_Branch.Value = values;
                    }
                    catch { }
                }
            }
        }


        private void OnChooseFromListCarrier()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    try
                    {
                        EditText et_Carrier = (EditText)Form.Items.Item("et_Carrier").Specific;
                        et_Carrier.Value = oDataTable.GetValue("CardCode", 0).ToString();
                    }
                    catch { }
                }
            }
        }

        private void SetCflBranchCondition()
        {
            ChooseFromList oCFL = Form.ChooseFromLists.Item("cf_Branch");

            oCFL.SetConditions(null);

            Conditions oCons = oCFL.GetConditions();

            Condition oCon = oCons.Add();
            oCon.Alias = "Disabled";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "N";

            oCFL.SetConditions(oCons);
        }

        private void SetCflCarrierCondition()
        {
            ChooseFromList oCFL = Form.ChooseFromLists.Item("cf_BP");

            oCFL.SetConditions(null);

            Conditions oCons = oCFL.GetConditions();

            Condition oCon = oCons.Add();
            oCon.Alias = "U_arkabportaltransp";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "Y";

            oCFL.SetConditions(oCons);
        }
        #endregion
    }
}
