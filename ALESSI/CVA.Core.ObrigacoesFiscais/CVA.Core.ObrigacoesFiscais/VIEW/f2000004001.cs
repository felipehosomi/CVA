using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.ObrigacoesFiscais.BLL;
using CVA.Core.ObrigacoesFiscais.DAO.Resources;
using CVA.Core.ObrigacoesFiscais.MODEL;
using SAPbouiCOM;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    /// <summary>
    /// Configurações de geração de arquivo
    /// </summary>
    public class f2000004001 : BaseForm
    {
        private Form Form;
        private static string ObjectName = String.Empty;
        private static LayoutModel LayoutModel;

        #region Constructor
        public f2000004001()
        {
            FormCount++;
        }

        public f2000004001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            ObjectName = String.Empty;

            Form.EnableMenu("1292", true);
            Form.EnableMenu("1293", true);

            ComboBox cb_Layout = Form.Items.Item("cb_Layout").Specific as ComboBox;
            cb_Layout.AddValuesFromQuery(Query.FileLayout_Get);

            Form.Items.Item("bt_Link").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
            Form.Items.Item("cb_Child").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);

            this.AddRow();
            return Form;
        }

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Link")
                {
                    {
                        ComboBox cb_Child = Form.Items.Item("cb_Child").Specific as ComboBox;
                        if (!String.IsNullOrEmpty(cb_Child.Value.Trim()))
                        {
                            f2000004006 frmLink = new f2000004006();
                            frmLink.Show(((EditText)Form.Items.Item("et_Code").Specific).Value, ((ComboBox)Form.Items.Item("cb_Type").Specific).Value, ((EditText)Form.Items.Item("et_ObjName").Specific).Value, cb_Child.Value);
                        }
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "cb_Layout")
                    {
                        ComboBox cb_Layout = Form.Items.Item("cb_Layout").Specific as ComboBox;
                        LayoutModel = new CrudController("@CVA_LAYOUT").RetrieveModel<LayoutModel>($"Code = {cb_Layout.Value}");
                    }
                    if (ItemEventInfo.ItemUID == "cb_Type")
                    {
                        this.FillFieldNameComboBox();
                    }
                    if (ItemEventInfo.ItemUID == "mt_Fields" && ItemEventInfo.ColUID == "cl_Type")
                    {
                        this.ClTypeOnComboSelect();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "et_ObjName")
                    {
                        EditText et_ObjName = (EditText)Form.Items.Item("et_ObjName").Specific;
                        if (ObjectName.ToLower() != et_ObjName.Value.ToLower())
                        {
                            ObjectName = et_ObjName.Value.ToLower();
                            this.FillFieldNameComboBox();
                        }
                    }
                    if (ItemEventInfo.ItemUID == "mt_Fields")
                    {
                        switch (ItemEventInfo.ColUID)
                        {
                            case "cl_Field":
                                this.ClFieldOnValidate();
                                break;
                            case "cl_To":
                                this.CalculateSize();
                                break;
                            case "cl_Size":
                                this.CalculatePositionTo();
                                break;
                        }
                    }
                    if (ItemEventInfo.ItemUID == "et_Layout")
                    {
                        Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                        if (mt_Fields.RowCount == 0)
                        {
                            this.AddRow();
                        }
                    }
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
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                    try
                    {
                        if (String.IsNullOrEmpty(((ComboBox)mt_Fields.GetCellSpecific("cl_Field", mt_Fields.VisualRowCount)).Value))
                        {
                            mt_Fields.DeleteRow(mt_Fields.VisualRowCount);
                        }
                        mt_Fields.FlushToDataSource();
                        if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                        {
                            ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_FILE_MAP").PadLeft(4, '0');
                        }
                    }
                    catch { }
                }
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    Form.Freeze(true);
                    this.AddRow();
                    DBDataSource dt_File = Form.DataSources.DBDataSources.Item("@CVA_FILE_MAP");
                    ComboBox cb_Layout = Form.Items.Item("cb_Layout").Specific as ComboBox;

                    if (ObjectName.ToLower() != dt_File.GetValue("U_ObjName", dt_File.Offset).ToLower())
                    {
                        ObjectName = dt_File.GetValue("U_ObjName", dt_File.Offset);
                        FillFieldNameComboBox();

                        LayoutModel = new CrudController("@CVA_LAYOUT").RetrieveModel<LayoutModel>($"Code = {cb_Layout.Value}");
                    }

                    ComboBox cb_Child = Form.Items.Item("cb_Child").Specific as ComboBox;
                    while (cb_Child.ValidValues.Count > 0)
                    {
                        cb_Child.ValidValues.Remove(0, BoSearchKey.psk_Index);
                    }

                    string sql = String.Format(Query.FileMapping_GetForLink, ((EditText)Form.Items.Item("et_Code").Specific).Value, cb_Layout.Value);
                    cb_Child.AddValuesFromQuery(sql);
                    Form.Freeze(false);
                }
            }

            return true;
        }
        #endregion

        #region CalculateSize
        private void CalculateSize()
        {
            try
            {
                Form.Freeze(true);
                EventFilterController.DisableEvents();

                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;

                EditText et_From = (EditText)mt_Fields.GetCellSpecific("cl_From", ItemEventInfo.Row);
                EditText et_To = (EditText)mt_Fields.GetCellSpecific("cl_To", ItemEventInfo.Row);

                int positionFrom;
                int positionTo;

                Int32.TryParse(et_From.Value, out positionFrom);
                Int32.TryParse(et_To.Value, out positionTo);
                if (positionFrom > 0 && positionTo > 0)
                {
                    EditText et_Size = (EditText)mt_Fields.GetCellSpecific("cl_Size", ItemEventInfo.Row);
                    et_Size.Value = (positionTo - positionFrom).ToString();
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
        #endregion

        #region CalculatePositionTo
        private void CalculatePositionTo()
        {
            try
            {
                Form.Freeze(true);
                EventFilterController.DisableEvents();

                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                if (mt_Fields.RowCount == 0)
                {
                    return;
                }

                EditText et_From = (EditText)mt_Fields.GetCellSpecific("cl_From", ItemEventInfo.Row);
                EditText et_Size = (EditText)mt_Fields.GetCellSpecific("cl_Size", ItemEventInfo.Row);

                int positionFrom;
                int size;

                Int32.TryParse(et_From.Value, out positionFrom);
                Int32.TryParse(et_Size.Value, out size);
                if (positionFrom > 0 && size > 0)
                {
                    EditText et_To = (EditText)mt_Fields.GetCellSpecific("cl_To", ItemEventInfo.Row);
                    et_To.Value = (positionFrom + size).ToString();
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
        #endregion

        private void ClTypeOnComboSelect()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                ComboBox cl_Type = (ComboBox)mt_Fields.GetCellSpecific("cl_Type", ItemEventInfo.Row);

                int type;
                if (Int32.TryParse(cl_Type.Value, out type))
                {
                    mt_Fields.FlushToDataSource();
                    DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_FILE_MAP_ITEM");

                    EditText cl_Format = (EditText)mt_Fields.GetCellSpecific("cl_Format", ItemEventInfo.Row);
                    EditText cl_Dec = (EditText)mt_Fields.GetCellSpecific("cl_Dec", ItemEventInfo.Row);

                    if (type == (int)FieldTypeEnum.Date)
                    {
                        ds_Item.SetValue("U_Format", ItemEventInfo.Row - 1, LayoutModel.DateFormat);
                        ds_Item.SetValue("U_Decimal", ItemEventInfo.Row - 1, String.Empty);
                    }
                    else if (type == (int)FieldTypeEnum.Decimal)
                    {
                        ds_Item.SetValue("U_Decimal", ItemEventInfo.Row - 1, LayoutModel.DecimalQuantity.ToString());
                        ds_Item.SetValue("U_Format", ItemEventInfo.Row - 1, String.Empty);

                        cl_Dec.Value = LayoutModel.DecimalQuantity.ToString();
                        cl_Format.Value = String.Empty;
                    }
                    else
                    {
                        ds_Item.SetValue("U_Format", ItemEventInfo.Row - 1, String.Empty);
                        ds_Item.SetValue("U_Decimal", ItemEventInfo.Row - 1, String.Empty);
                    }
                    mt_Fields.LoadFromDataSource();
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

        #region ClFieldOnValidate
        private void ClFieldOnValidate()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                ComboBox cb_Field = (ComboBox)mt_Fields.GetCellSpecific("cl_Field", ItemEventInfo.Row);

                if (!String.IsNullOrEmpty(cb_Field.Value))
                {
                    if (ItemEventInfo.Row == mt_Fields.RowCount)
                    {
                        mt_Fields.FlushToDataSource();
                        this.AddRow();
                    }
                    // Se possuir mais uma linha, faz o cálculo da posição inicial
                    if (ItemEventInfo.Row > 1)
                    {
                        EditText et_From = (EditText)mt_Fields.GetCellSpecific("cl_From", ItemEventInfo.Row);
                        if (String.IsNullOrEmpty(et_From.Value))
                        {
                            int positionTo;
                            EditText et_To = (EditText)mt_Fields.GetCellSpecific("cl_To", ItemEventInfo.Row - 1);
                            Int32.TryParse(et_To.Value, out positionTo);
                            if (positionTo > 0)
                            {
                                et_From.Value = positionTo.ToString();
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
                Form.Freeze(false);
            }
        }
        #endregion

        #region FillFieldNameComboBox
        private void FillFieldNameComboBox()
        {
            if (String.IsNullOrEmpty(ObjectName))
            {
                return;
            }
            ComboBox cb_Type = (ComboBox)Form.Items.Item("cb_Type").Specific;
            string objType = cb_Type.Value;
            if (String.IsNullOrEmpty(objType))
            {
                return;
            }

            Form.Freeze(true);
            try
            {
                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                ComboBox cl_Field = (ComboBox)mt_Fields.Columns.Item("cl_Field").Cells.Item(1).Specific;
                //cl_Field.DisplayType = BoComboDisplayType.cdt_Description;

                string sql = FileBLL.GetSQL(objType, ObjectName);
                DataTable dt_Object = Form.DataSources.DataTables.Item("dt_Object");
                dt_Object.ExecuteQuery(sql);

                while (cl_Field.ValidValues.Count > 0)
                {
                    cl_Field.ValidValues.Remove(0, BoSearchKey.psk_Index);
                }
                for (int i = 0; i < dt_Object.Columns.Count; i++)
                {
                    cl_Field.ValidValues.Add(dt_Object.Columns.Item(i).Name, dt_Object.Columns.Item(i).Name);
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro ao buscar campos: " + ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
        #endregion

        #region AddRow
        private void AddRow()
        {
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_FILE_MAP_ITEM");
            dt_Fields.Clear();

            Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
            mt_Fields.AddRow();
        }
        #endregion

        #region OnClickMenuAdd
        /// <summary>
        /// Método chamado pela classe f1282, que controla o evento do Menu Add (1282)
        /// </summary>
        public static void OnClickMenuAdd(Form form)
        {
            DBDataSource dt_Fields = form.DataSources.DBDataSources.Item("@CVA_FILE_MAP_ITEM");
            dt_Fields.Clear();

            Matrix mt_Fields = (Matrix)form.Items.Item("mt_Fields").Specific;
            mt_Fields.AddRow();
        }
        #endregion
    }
}
