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

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Cadastro romaneio
    /// </summary>
    public class f2000002003 : BaseForm
    {
        private Form Form;
        private static bool Canceled = false;
        private static string ErrorMessage = "";

        #region Constructor
        public f2000002003()
        {
            FormCount++;
        }

        public f2000002003(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002003(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002003(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public override object Show()
        {
            Form = (Form)base.Show();
            Canceled = false;
            this.SetCflCarrierCondition();
            this.SetVehicleTypeValidValues();

            Form.Items.Item("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
            Form.Items.Item("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            Form.ReportType = ReportController.AddReportToForm("Romaneio.rpt", "Romaneio", "SAP AddOn Romaneio", "Romaneio", "2000002003", "2003");

            return Form;
        }

        public object Show(WaybillActionEnum actionEnum)
        {
            Form = (Form)this.Show();
            Form.Mode = BoFormMode.fm_FIND_MODE;
            
            switch (actionEnum)
            {
                case WaybillActionEnum.Edit:
                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_View, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("st_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("st_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("st_Status").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("st_Status").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("cb_Status").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("cb_Status").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("cb_Status").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_View, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("bt_Cancel").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    break;
                case WaybillActionEnum.Cancel:
                    Form.Items.Item("et_Date").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("et_Carrier").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("et_Comment").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("cb_Veh").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("et_Date").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("et_Carrier").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("et_Comment").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("cb_Veh").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_Cancel").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    break;
                case WaybillActionEnum.View:
                    Form.Items.Item("et_Date").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("et_Carrier").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("et_Comment").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("cb_Veh").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

                    Form.Items.Item("et_Date").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("et_Carrier").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("et_Comment").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("cb_Veh").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

                    Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_Cancel").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    break;
                default:
                    break;
            }

            return Form;
        }

        public object Show(List<InvoiceModel> invoiceList)
        {
            Form = (Form)this.Show();
            Form.Freeze(true);
            
            Form.Items.Item("bt_Cancel").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

            Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
            Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("bt_NF").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_View, BoModeVisualBehavior.mvb_False);

            Form.Items.Item("st_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
            Form.Items.Item("st_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);

            try
            {
                AddInvoices(Form, invoiceList);
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
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    SBOApp.Application.LayoutKeyEvent += new _IApplicationEvents_LayoutKeyEventEventHandler(LayoutKeyEvent);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (ItemEventInfo.ItemUID == "bt_NF")
                    {
                        DBDataSource dt_Waybill = Form.DataSources.DBDataSources.Item("@CVA_WAYBILL");
                        string status = dt_Waybill.GetValue("U_Status", dt_Waybill.Offset);
                        if (status != "2")
                        {
                            EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
                            new f2000002002().Show(et_Code.Value);
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Impossível alterar documento cancelado!");
                        }
                    }
                    if (ItemEventInfo.ItemUID == "bt_Cancel")
                    {
                        this.Cancel();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                {
                    if (ItemEventInfo.ItemUID == "et_Carrier")
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        this.SetVehicleValidValues();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "cb_Veh")
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        this.FillVehicleFields();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    SBOApp.Application.LayoutKeyEvent -= new _IApplicationEvents_LayoutKeyEventEventHandler(LayoutKeyEvent);
                }
            }
            return true;
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
                    mt_NF.FlushToDataSource();

                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_WAYBILL").PadLeft(8, '0');
                }
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (Canceled)
                    {
                        ErrorMessage = "Impossível alterar documento cancelado!";
                        return false;
                    }
                }
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    DBDataSource dt_Waybill = Form.DataSources.DBDataSources.Item("@CVA_WAYBILL");
                    string status = dt_Waybill.GetValue("U_Status", dt_Waybill.Offset);
                    Canceled = status == "2";
                }
            }
            return true;
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
                    if (ContextMenuInfo.ItemUID == "mt_NF")
                    {
                        if (ContextMenuInfo.Row >= 0)
                        {
                            this.CreateRightClickMenuItem();
                            Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
                            mt_NF.SelectRow(ContextMenuInfo.Row, true, false);
                        }
                    }
                    else
                    {
                        if (Form.Menu.Exists("R2101"))
                        {
                            Form.Menu.RemoveEx("R2101");
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region LayoutKeyEvent
        void LayoutKeyEvent(ref LayoutKeyInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Form = SBOApp.Application.Forms.ActiveForm;
            EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
            if (eventInfo.FormUID.StartsWith("f2000002003"))
            {
                eventInfo.LayoutKey = et_Code.Value; 
            }
        }
        #endregion

        #region Private Methods
        public static string GetCode(Form form)
        {
            return ((EditText)form.Items.Item("et_Code").Specific).Value;
        }

        public static void AddInvoices(Form form, List<InvoiceModel> invoiceList)
        {
            DBDataSource dt_Inv = form.DataSources.DBDataSources.Item("@CVA_WAYBILL_INV");
            Matrix mt_NF = (Matrix)form.Items.Item("mt_NF").Specific;

            
            int i = dt_Inv.Size - 1;
            foreach (var invoiceModel in invoiceList)
            {
                if (!String.IsNullOrEmpty(dt_Inv.GetValue("U_DocNum", 0)))
                {
                    dt_Inv.InsertRecord(dt_Inv.Size);
                    i++;
                }
                dt_Inv.SetValue("U_DocNum", i, invoiceModel.DocNum.ToString());
                dt_Inv.SetValue("U_Invoice", i, invoiceModel.Invoice.ToString());
                dt_Inv.SetValue("U_CardCode", i, invoiceModel.CardCode);
                dt_Inv.SetValue("U_CardName", i, invoiceModel.CardName);
                dt_Inv.SetValue("U_Date", i, invoiceModel.DocDate.ToString("yyyyMMdd"));
                dt_Inv.SetValue("U_CarrierCode", i, invoiceModel.CarrierCode);
                dt_Inv.SetValue("U_CarrierName", i, invoiceModel.CarrierName);
                dt_Inv.SetValue("U_GrossWeight", i, invoiceModel.GrossWeight.ToString().Replace(",", "."));
                dt_Inv.SetValue("U_DocTotal", i, invoiceModel.DocTotal.ToString().Replace(",", "."));
                dt_Inv.SetValue("U_State", i, invoiceModel.State);
                dt_Inv.SetValue("U_City", i, invoiceModel.City);
            }

            mt_NF.LoadFromDataSource();
        }

        private void Cancel()
        {
            DBDataSource dt_Waybill = Form.DataSources.DBDataSources.Item("@CVA_WAYBILL");
            dt_Waybill.SetValue("U_Status", dt_Waybill.Offset, "2");
            Form.Mode = BoFormMode.fm_UPDATE_MODE;
            Form.Items.Item("1").Click();
        }

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("R2101"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "R2101";
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }

        private void SetVehicleTypeValidValues()
        {
            ComboBox cb_VehType = (ComboBox)Form.Items.Item("cb_VehType").Specific;
            cb_VehType.AddValuesFromQuery(Query.VehicleType_Get, "Code", "Name");
        }

        private void SetVehicleValidValues()
        {
            DBDataSource dt_Waybill = Form.DataSources.DBDataSources.Item("@CVA_WAYBILL");
            string carrierCode = dt_Waybill.GetValue("U_Carrier", dt_Waybill.Offset);

            ComboBox cb_Vehicle = (ComboBox)Form.Items.Item("cb_Veh").Specific;
            while (cb_Vehicle.ValidValues.Count > 0)
            {
                cb_Vehicle.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }

            cb_Vehicle.AddValuesFromQuery(CarrierVehicleBLL.GetCarVehicleComboboxSql(carrierCode), "Code", "Description");
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

        private void FillVehicleFields()
        {
            DBDataSource dt_Waybill = Form.DataSources.DBDataSources.Item("@CVA_WAYBILL");
            string vehicleCode = dt_Waybill.GetValue("U_Vehicle", dt_Waybill.Offset);

            VehicleModel model = VehicleBLL.Get(vehicleCode);
            dt_Waybill.SetValue("U_VehType", dt_Waybill.Offset, model.TypeCode);
            dt_Waybill.SetValue("U_Plate", dt_Waybill.Offset, model.Plate);
            dt_Waybill.SetValue("U_Color", dt_Waybill.Offset, model.Color);
            dt_Waybill.SetValue("U_Driver", dt_Waybill.Offset, model.Driver);
        }
        #endregion
    }
}
