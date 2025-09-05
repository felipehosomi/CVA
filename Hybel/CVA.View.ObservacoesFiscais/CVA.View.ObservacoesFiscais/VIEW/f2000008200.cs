using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using CVA.AddOn.Common.Util;
using CVA.View.ObservacoesFiscais.DAO.Resources;

namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Configurações Obs Documentos
    /// </summary>
    public class f2000008200 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000008200()
        {
            FormCount++;
        }

        public f2000008200(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000008200(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000008200(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            this.AddRow();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            ((ComboBox)mt_Item.Columns.Item("cl_Usage").Cells.Item(1).Specific).AddValuesFromQuery(SQL.Usage_Get);
            ((ComboBox)mt_Item.Columns.Item("cl_UF").Cells.Item(1).Specific).AddValuesFromQuery(SQL.State_Get);
            ((ComboBox)mt_Item.Columns.Item("cl_BPL").Cells.Item(1).Specific).AddValuesFromQuery(SQL.Branch_Get);
            ((ComboBox)mt_Item.Columns.Item("cl_GroupPN").Cells.Item(1).Specific).AddValuesFromQuery(SQL.GroupBP_Get);
            ((ComboBox)mt_Item.Columns.Item("cl_ItemGr").Cells.Item(1).Specific).AddValuesFromQuery(SQL.ItemGroup_Get);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.ItemUID == "mt_Item")
                    {
                        this.ColumnOnValidate();
                    }
                    if (ItemEventInfo.ItemUID == "et_Code")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        if (mt_Item.RowCount == 0)
                        {
                            this.AddRow();
                        }
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    this.ChooseFromList();
                }
            }

            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                    mt_Item.DeleteRow(mt_Item.RowCount);
                    mt_Item.FlushToDataSource();
                }
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    this.AddRow();
                }
            }

            return true;
        }

        private void ColumnOnValidate()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                string value = String.Empty;
                try
                {
                    EditText column = (EditText)mt_Item.GetCellSpecific(ItemEventInfo.ColUID, ItemEventInfo.Row);
                    value = column.Value;
                }
                catch
                {
                    ComboBox column = (ComboBox)mt_Item.GetCellSpecific(ItemEventInfo.ColUID, ItemEventInfo.Row);
                    value = column.Value;
                }

                if (!String.IsNullOrEmpty(value))
                {
                    if (ItemEventInfo.Row == mt_Item.RowCount)
                    {
                        mt_Item.FlushToDataSource();
                        this.AddRow();
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

        #region ChooseFromList
        private void ChooseFromList()
        {
            IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
            DataTable oDataTable = oCFLEvento.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    if (ItemEventInfo.ColUID == "cl_Item")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        mt_Item.FlushToDataSource();

                        string itemCode = oDataTable.GetValue("ItemCode", 0).ToString();
                        
                        DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_OBSAUTO1");
                        ds_Item.SetValue("U_ItemCode", ItemEventInfo.Row - 1, itemCode);
                        mt_Item.LoadFromDataSource();

                        if (mt_Item.RowCount == ItemEventInfo.Row)
                        {
                            this.AddRow();
                        }
                    }
                    if (ItemEventInfo.ColUID == "cl_BP")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        mt_Item.FlushToDataSource();

                        string cardCode = oDataTable.GetValue("CardCode", 0).ToString();

                        DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_OBSAUTO1");
                        ds_Item.SetValue("U_CardCode", ItemEventInfo.Row - 1, cardCode);
                        mt_Item.LoadFromDataSource();

                        if (mt_Item.RowCount == ItemEventInfo.Row)
                        {
                            this.AddRow();
                        }
                    }
                }
            }
        }
        #endregion

        private void AddRow()
        {
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_OBSAUTO1");
            dt_Fields.Clear();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }

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
                        if (Form.Menu.Exists("M9200"))
                        {
                            Form.Menu.RemoveEx("M9200");
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("M9200"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "M9200";
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }

    }
}
