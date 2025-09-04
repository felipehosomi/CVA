using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.Core.Alessi.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    /// <summary>
    /// CT-e's
    /// </summary>
    public class f2000003001 : BaseForm
    {
        Form Form;
        #region Constructor
        public f2000003001()
        {
            FormCount++;
        }

        public f2000003001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(int docEntry, int docNum)
        {
            Form = (Form)base.Show();
            this.SetCflNFCondition();

            Form.Items.Item("et_DocNum").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("et_DocNum").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            if (NotaEntradaFreteBLL.Exists(docEntry))
            {
                Form.Mode = BoFormMode.fm_FIND_MODE;
                ((EditText)Form.Items.Item("et_DocNum").Specific).Value = docNum.ToString();
                Form.Items.Item("1").Click();
                Form.Mode = BoFormMode.fm_UPDATE_MODE;
            }
            else
            {
                Form.Mode = BoFormMode.fm_ADD_MODE;
                ((EditText)Form.Items.Item("et_Id").Specific).Value = docEntry.ToString();
                ((EditText)Form.Items.Item("et_DocNum").Specific).Value = docNum.ToString();
                this.AddRow();
            }

            Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
            mt_NF.SetCellFocus(mt_NF.RowCount, 1);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.ItemUID == "mt_NF")
                {
                    this.OnChooseFromListNF();
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
                    Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
                    //if (String.IsNullOrEmpty(((EditText)mt_NF.GetCellSpecific("cl_DocNum", mt_NF.RowCount)).Value))
                    //{
                    //    mt_NF.DeleteRow(mt_NF.RowCount);
                    //}
                    //mt_NF.FlushToDataSource();

                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                    {
                        ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_NFE_FRETE").PadLeft(8, '0');
                    }
                }
            }
            else
            {
                //if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                //{
                //    Form = SBOApp.Application.Forms.ActiveForm;
                //    this.AddRow();
                //}
                //if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                //{
                //    Form = SBOApp.Application.Forms.ActiveForm;
                //    Form.Close();
                //}
            }

            return true;
        }

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
                        if (Form.Menu.Exists("M3101"))
                        {
                            Form.Menu.RemoveEx("M3101");
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
                if (!Form.Menu.Exists("M3101"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "M3101";
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }

        private void SetCflNFCondition()
        {
            ChooseFromList oCFL = Form.ChooseFromLists.Item("cf_NF");

            oCFL.SetConditions(null);

            Conditions oCons = oCFL.GetConditions();

            Condition oCon = oCons.Add();
            oCon.Alias = "CANCELED";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "N";

            oCFL.SetConditions(oCons);
        }

        private void AddRow()
        {
            DBDataSource ds_NF = Form.DataSources.DBDataSources.Item("@CVA_NFE_FRETE_ITEM");
            ds_NF.Clear();

            Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
            mt_NF.AddRow();
        }

        private void OnChooseFromListNF()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    Matrix mt_NF = (Matrix)Form.Items.Item("mt_NF").Specific;
                    mt_NF.FlushToDataSource();
                    int rowIndex = mt_NF.GetCellFocus().rowIndex;

                    DBDataSource ds_NF = Form.DataSources.DBDataSources.Item("@CVA_NFE_FRETE_ITEM");
                    ds_NF.SetValue("U_DocNum", rowIndex - 1, oDataTable.GetValue("DocNum", 0).ToString());
                    ds_NF.SetValue("U_Serial", rowIndex - 1, oDataTable.GetValue("Serial", 0).ToString());
                    ds_NF.SetValue("U_DocDate", rowIndex - 1, ((DateTime)oDataTable.GetValue("DocDate", 0)).ToString("yyyyMMdd"));
                    ds_NF.SetValue("U_CardCode", rowIndex - 1, oDataTable.GetValue("CardCode", 0).ToString());
                    ds_NF.SetValue("U_CardName", rowIndex - 1, oDataTable.GetValue("CardName", 0).ToString());
                    ds_NF.SetValue("U_DocTotal", rowIndex - 1, oDataTable.GetValue("DocTotal", 0).ToString().Replace(",", "."));

                    int docEntry = Convert.ToInt32(oDataTable.GetValue("DocEntry", 0));
                    string carrier = NotaSaidaBLL.GetCarrier(docEntry);
                    ds_NF.SetValue("U_ShipCode", rowIndex - 1, carrier);

                    mt_NF.LoadFromDataSource();

                    if (mt_NF.RowCount == rowIndex)
                    {
                        this.AddRow();
                    }
                }
            }
        }
    }
}
