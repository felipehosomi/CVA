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
    /// Trasnportadora X Veículos
    /// </summary>
    public class f2000002006 : BaseForm
    {
        Form Form;

        #region Constructor
        public f2000002006()
        {
            FormCount++;
        }

        public f2000002006(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002006(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002006(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public override object Show()
        {
            Form = (Form)base.Show();
            Form.DataSources.DataTables.Item("dt_Veh").Rows.Add();
            this.SetCflCarrierCondition();
            return Form;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (ItemEventInfo.ItemUID == "et_CarCode")
                    {
                        this.OnChooseFromListCarrier();
                    }
                    if (ItemEventInfo.ItemUID == "gr_Veh")
                    {
                        this.OnChooseFromListVehicle();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "gr_Veh" && ItemEventInfo.ColUID == "Veículo")
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        this.OnColumnVehicleLostFocus();
                    }
                }
            }
            return true;
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.ActionSuccess)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    GridController gridController = new GridController();
                    List<CarrierVehicleModel> itemList = gridController.FillModelFromTable<CarrierVehicleModel>(Form.DataSources.DataTables.Item("dt_Veh"));
                    CarrierVehicleBLL carrierVehicleBLL = new CarrierVehicleBLL();
                    string carrier = Form.DataSources.DBDataSources.Item("@CVA_CARRIER").GetValue("Code", 0);

                    string msg = carrierVehicleBLL.AddList(carrier, itemList);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        SBOApp.Application.MessageBox("Erro ao salvar veículos: " + msg);
                    }
                }
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    string carrier = Form.DataSources.DBDataSources.Item("@CVA_CARRIER").GetValue("Code", 0);

                    Form.DataSources.DataTables.Item("dt_Veh").ExecuteQuery(CarrierVehicleBLL.GetCarVehicleSql(carrier));
                    Form.DataSources.DataTables.Item("dt_Veh").Rows.Add();

                    this.ConfigureGridColumns();
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
                            Grid gr_Veh = (Grid)Form.Items.Item("gr_Veh").Specific;
                            gr_Veh.Rows.SelectedRows.Add(ContextMenuInfo.Row);
                        }
                    }
                    else
                    {
                        if (Form.Menu.Exists("R2102"))
                        {
                            Form.Menu.RemoveEx("R2102");
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region OnClickMenuAdd
        /// <summary>
        /// Método chamado pela classe f1282, que controla o evento do Menu Add (1282)
        /// </summary>
        public static void OnClickMenuAdd(Form form)
        {
            form.DataSources.DataTables.Item("dt_Veh").Rows.Add();
        }
        #endregion

        #region Private Methods
        private void ConfigureGridColumns()
        {
            Grid gr_Veh = (Grid)Form.Items.Item("gr_Veh").Specific;
            
            EditTextColumn cl_Vehicle = (EditTextColumn)gr_Veh.Columns.Item("Veículo");
            cl_Vehicle.ChooseFromListAlias = "Code";
            cl_Vehicle.ChooseFromListUID = "cl_Veh";

            for (int i = 1; i < gr_Veh.Columns.Count; i++)
            {
                gr_Veh.Columns.Item(i).Editable = false;
            }
        }

        private void OnColumnVehicleLostFocus()
        {
            Form.Freeze(true);
            try
            {
                EventFilterController.DisableEvents();

                DataTable dt_Veh = Form.DataSources.DataTables.Item("dt_Veh");
                string vehicleCode = dt_Veh.GetValue("Veículo", ItemEventInfo.Row).ToString();

                if (!String.IsNullOrEmpty(vehicleCode.Trim()))
                {
                    VehicleModel vehicleModel = VehicleBLL.Get(vehicleCode);
                    dt_Veh.SetValue("Tipo", ItemEventInfo.Row, vehicleModel.TypeDesc);
                    dt_Veh.SetValue("Placa", ItemEventInfo.Row, vehicleModel.Plate);
                    dt_Veh.SetValue("Cor", ItemEventInfo.Row, vehicleModel.Color);
                    dt_Veh.SetValue("Motorista", ItemEventInfo.Row, vehicleModel.Driver);

                    if (dt_Veh.Rows.Count == ItemEventInfo.Row + 1)
                    {
                        dt_Veh.Rows.Add();
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                EventFilterController.SetDefaultEvents();
                Form.Freeze(false);
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
                        DBDataSource dt_Carrier = Form.DataSources.DBDataSources.Item("@CVA_CARRIER");
                        dt_Carrier.SetValue("Code", dt_Carrier.Offset, oDataTable.GetValue("CardCode", 0).ToString());
                        dt_Carrier.SetValue("U_CarrierName", dt_Carrier.Offset, oDataTable.GetValue("CardName", 0).ToString());
                    }
                    catch { }
                }
            }
        }

        private void OnChooseFromListVehicle()
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
                        DataTable dt_Veh = Form.DataSources.DataTables.Item("dt_Veh");
                        string vehicleCode = dt_Veh.GetValue("Veículo", ItemEventInfo.Row).ToString();

                        VehicleModel vehicleModel = VehicleBLL.Get(vehicleCode);
                        dt_Veh.SetValue("Veículo", ItemEventInfo.Row, oDataTable.GetValue("Code", 0).ToString());

                        if (Form.Mode != BoFormMode.fm_UPDATE_MODE && Form.Mode != BoFormMode.fm_ADD_MODE)
                        {
                            Form.Mode = BoFormMode.fm_UPDATE_MODE;
                        }
                    }
                    catch { }
                }
            }
        }

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("R2102"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "R2102";
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
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
