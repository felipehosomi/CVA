using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.DIME.BLL;
using CVA.View.DIME.DAO.Resources;
using SAPbouiCOM;
using System;

namespace CVA.View.DIME.VIEW
{
    /// <summary>
    /// Quadro 12
    /// </summary>
    public class f2000005013 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000005013()
        {
            FormCount++;
        }

        public f2000005013(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000005013(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000005013(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Filial").Specific;
            cb_Branch.AddValuesFromQuery(SQL.Branch_Get);
           
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
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "et_Period")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        if (mt_Item.RowCount == 0)
                        {
                            this.AddRow();
                        }
                    }
                    if (ItemEventInfo.ItemUID == "mt_Item")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        if (ItemEventInfo.Row == mt_Item.RowCount)
                        {
                            if (!String.IsNullOrEmpty(((ComboBox)mt_Item.GetCellSpecific("cl_Origem", ItemEventInfo.Row)).Value))
                            {
                                this.AddRow();
                            }
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
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    DBDataSource dt_DIME = Form.DataSources.DBDataSources.Item("@CVA_DIME_12");

                    string filial = dt_DIME.GetValue("U_Filial", dt_DIME.Offset);
                    string period = dt_DIME.GetValue("U_Periodo", dt_DIME.Offset).Trim();
                    DateTime date;
                    if (!DateTime.TryParseExact($"01/{period.Trim()}", "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date))
                    {
                        SBOApp.Application.MessageBox("Período deve estar no formada MM/AAAA");
                        return false;
                    }

                    if (!String.IsNullOrEmpty(filial) && !String.IsNullOrEmpty(period))
                    {
                        if (CrudController.ExecuteScalar(String.Format(SQL.DimeQuadro12_Get, period, filial)) != null)
                        {
                            SBOApp.Application.MessageBox("Configurações já cadastradas para filial e periodo informados!");
                            return false;
                        }
                    }

                    dt_DIME.SetValue("Code", dt_DIME.Offset, CrudController.GetNextCode("@CVA_DIME_12").PadLeft(4, '0'));
                }
            }

            return true;
        }

        private void AddRow()
        {
            DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("@CVA_DIME_12_ITEM");
            dt_Item.Clear();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }
    }
}
