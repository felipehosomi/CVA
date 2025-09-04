using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.Core.Alessi.VIEW
{
    /// <summary>
    /// Configurações registro
    /// </summary>
    public class f2000001002 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000001002()
        {
            FormCount++;
        }

        public f2000001002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            this.AddRow();
            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
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
                }
            }

            return true;
        }

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

        private void CalculatePositionTo()
        {
            try
            {
                Form.Freeze(true);
                EventFilterController.DisableEvents();

                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;

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

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                    if (String.IsNullOrEmpty(((EditText)mt_Fields.GetCellSpecific("cl_Field", mt_Fields.RowCount)).Value))
                    {
                        mt_Fields.DeleteRow(mt_Fields.RowCount);
                    }
                    mt_Fields.FlushToDataSource();
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                    {
                        ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_DOC_MAP").PadLeft(4, '0');
                    }
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

        private void ClFieldOnValidate()
        {
            Form.Freeze(true);
            try
            {
                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                EditText et_Field = (EditText)mt_Fields.GetCellSpecific("cl_Field", ItemEventInfo.Row);

                if (!String.IsNullOrEmpty(et_Field.Value))
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

        private void AddRow()
        {
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_DOC_MAP_ITEM");
            dt_Fields.Clear();

            Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
            mt_Fields.AddRow();
        }

        #region OnClickMenuAdd
        /// <summary>
        /// Método chamado pela classe f1282, que controla o evento do Menu Add (1282)
        /// </summary>
        public static void OnClickMenuAdd(Form form)
        {
            DBDataSource dt_Fields = form.DataSources.DBDataSources.Item("@CVA_DOC_MAP_ITEM");
            dt_Fields.Clear();

            Matrix mt_Fields = (Matrix)form.Items.Item("mt_Fields").Specific;
            mt_Fields.AddRow();
        }
        #endregion
    }
}
