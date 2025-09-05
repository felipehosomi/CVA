using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// DE-PARA Concorrente X Itens
    /// </summary>
    public class f2000003031 : BaseForm
    {
        private Form Form;
        
        #region Constructor
        public f2000003031()
        {
            FormCount++;
        }

        public f2000003031(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003031(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003031(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            Form.DataBrowser.BrowseBy = "et_Code";
            Form.Items.Item("bt_Search").Visible = false;

            Form.Items.Item("et_Code").Enabled = true;
            Form.Items.Item("mt_Item").Enabled = true;

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();

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
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "mt_Item")
                    {
                        switch (ItemEventInfo.ColUID)
                        {
                            case "cl_Item":
                                this.ClItemOnValidate();
                                break;
                        }
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
                    if (ItemEventInfo.ColUID == "cl_Item" || ItemEventInfo.ColUID == "cl_Name")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        mt_Item.FlushToDataSource();

                        string itemCode = oDataTable.GetValue("ItemCode", 0).ToString();
                        string itemName = oDataTable.GetValue("ItemName", 0).ToString();

                        DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_CONC_ITEM");
                        ds_Item.SetValue("U_ItemCode", ItemEventInfo.Row - 1, itemCode);
                        ds_Item.SetValue("U_ItemName", ItemEventInfo.Row - 1, itemName);
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

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                    if (String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Item", mt_Item.RowCount)).Value))
                    {
                        mt_Item.DeleteRow(mt_Item.RowCount);
                    }
                    mt_Item.FlushToDataSource();
                }
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    this.AddRow();
                }
            }

            return true;
        }

        private void ClItemOnValidate()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                EditText cl_Item = (EditText)mt_Item.GetCellSpecific("cl_Item", ItemEventInfo.Row);

                if (!String.IsNullOrEmpty(cl_Item.Value))
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

        private void AddRow()
        {
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_CONC_ITEM");
            dt_Fields.Clear();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }

        #region OnClickMenuAdd
        /// <summary>
        /// Método chamado pela classe f1282, que controla o evento do Menu Add (1282)
        /// </summary>
        public static void OnClickMenuAdd(Form form)
        {
            DBDataSource dt_Fields = form.DataSources.DBDataSources.Item("@CVA_CONC_ITEM");
            dt_Fields.Clear();

            Matrix mt_Item = (Matrix)form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }
        #endregion
    }
}
