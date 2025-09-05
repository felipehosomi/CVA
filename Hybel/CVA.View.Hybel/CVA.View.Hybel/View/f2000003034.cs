using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Aplicação
    /// </summary>
    public class f2000003034 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000003034()
        {
            FormCount++;
        }

        public f2000003034(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003034(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003034(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

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
                        switch (ItemEventInfo.ColUID)
                        {
                            case "cl_Tipo":
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
            }

            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
                    if (String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Tipo", mt_Item.RowCount)).Value))
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
                EditText cl_Tipo = (EditText)mt_Item.GetCellSpecific("cl_Tipo", ItemEventInfo.Row);

                if (!String.IsNullOrEmpty(cl_Tipo.Value))
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
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_MAQUINA");
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
            DBDataSource dt_Fields = form.DataSources.DBDataSources.Item("@CVA_MAQUINA");
            dt_Fields.Clear();

            Matrix mt_Item = (Matrix)form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }
        #endregion
    }
}
