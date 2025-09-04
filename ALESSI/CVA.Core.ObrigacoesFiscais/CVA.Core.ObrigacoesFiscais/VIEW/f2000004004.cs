using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.ObrigacoesFiscais.BLL;
using CVA.Core.ObrigacoesFiscais.DAO.Resources;
using CVA.Core.ObrigacoesFiscais.MODEL;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    /// <summary>
    /// GIA-ST
    /// </summary>
    public class f2000004004 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000004004()
        {
            FormCount++;
        }

        public f2000004004(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004004(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004004(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                SBOApp.Application.SetStatusBarMessage("Obrigação não suportada para versão HANA.", BoMessageTime.bmt_Short, true);
                return false;
            }
            else
            {
                Form = (Form)base.Show();
                ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Branch").Specific;
                cb_Branch.AddValuesFromQuery(Query.Branch_Get);

                ComboBox cb_Dec = (ComboBox)Form.Items.Item("cb_Dec").Specific;
                cb_Dec.AddValuesFromQuery(Query.User_Get);

                OptionBtn rb_Date = (OptionBtn)Form.Items.Item("rb_Date").Specific;
                rb_Date.GroupWith("rb_Period");

                OptionBtn rb_Period = (OptionBtn)Form.Items.Item("rb_Period").Specific;
                rb_Period.Selected = true;

                Form.ReportType = ReportController.AddReportToForm("GIA-ST.rpt", "GIA-ST", "SAP AddOn GIA-ST", "GIA-ST", "2000004004", "4004");

                //Form.Items.Item("et_Period").Enabled = true;
                //Form.Items.Item("et_DtFrom").Enabled = false;
                //Form.Items.Item("et_DtTo").Enabled = false;

                return Form;
            }
            
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {

                    if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                    {
                        SBOApp.Application.SetStatusBarMessage("Obrigação não Suportada para versão HANA.",BoMessageTime.bmt_Short,true);
                        return false;
                    }
                    else
                    {
                        SBOApp.Application.LayoutKeyEvent += new _IApplicationEvents_LayoutKeyEventEventHandler(LayoutKeyEvent);
                    }

                    
                }
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }
                //if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                //{
                //    if (ItemEventInfo.ItemUID == "rb_Period")
                //    {
                //        Form.Items.Item("et_DtVenc").Click();
                //        Form.Items.Item("et_Period").Enabled = true;
                //        Form.Items.Item("et_DtFrom").Enabled = false;
                //        Form.Items.Item("et_DtTo").Enabled = false;
                //        ((EditText)Form.Items.Item("et_DtFrom").Specific).Value = "";
                //        ((EditText)Form.Items.Item("et_DtTo").Specific).Value = "";
                //        Form.Items.Item("et_Period").Click();
                //    }
                //    if (ItemEventInfo.ItemUID == "rb_Date")
                //    {
                //        Form.Items.Item("et_DtVenc").Click();
                //        Form.Items.Item("et_Period").Enabled = false;
                //        Form.Items.Item("et_DtFrom").Enabled = true;
                //        Form.Items.Item("et_DtTo").Enabled = true;
                //        ((EditText)Form.Items.Item("et_Period").Specific).Value = "";
                //        Form.Items.Item("et_DtFrom").Click();
                //    }
                //}
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "et_Period")
                    {
                        this.ValidatePeriod();
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

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_GIA_ST").PadLeft(4, '0');
                }
            }

            return true;
        }

        private void ValidatePeriod()
        {
            DBDataSource dt_GIA = Form.DataSources.DBDataSources.Item("@CVA_GIA_ST");
            string period = dt_GIA.GetValue("U_Periodo", dt_GIA.Offset).Trim();
            DateTime dateFrom;
            if (!DateTime.TryParseExact("01/" + period, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateFrom))
            {
                return;
            }
            DateTime dateTo = new DateTime(dateFrom.Year, dateFrom.Month, DateTime.DaysInMonth(dateFrom.Year, dateFrom.Month));

            dt_GIA.SetValue("U_DtDe", dt_GIA.Offset, dateFrom.ToString("yyyyMMdd"));
            dt_GIA.SetValue("U_DtAte", dt_GIA.Offset, dateTo.ToString("yyyyMMdd"));
        }

        #region LayoutKeyEvent
        void LayoutKeyEvent(ref LayoutKeyInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Form = SBOApp.Application.Forms.ActiveForm;
            EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
            if (eventInfo.FormUID.StartsWith("f2000004004"))
            {
                eventInfo.LayoutKey = et_Code.Value;
            }
        }
        #endregion
    }
}
