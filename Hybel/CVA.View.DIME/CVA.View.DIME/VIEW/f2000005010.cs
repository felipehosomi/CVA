using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.DIME.BLL;
using CVA.View.DIME.DAO.Resources;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.View.DIME.VIEW
{
    /// <summary>
    /// DIME - Geração do Arquivo
    /// </summary>
    public class f2000005010 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000005010()
        {
            FormCount++;
        }

        public f2000005010(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000005010(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000005010(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
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
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Dir")
                    {
                        DialogUtil dialogUtil = new DialogUtil();
                        string folder = dialogUtil.FolderBrowserDialog();
                        if (!String.IsNullOrEmpty(folder))
                        {
                            ((EditText)Form.Items.Item("et_Dir").Specific).Value = folder;
                        }
                    }
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        this.GenerateFile();
                    }
                }
            }
            return true;
        }

        private void GenerateFile()
        {
            if (Form.Mode == BoFormMode.fm_FIND_MODE)
            {
                return;
            }

            if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                SBOApp.Application.SetStatusBarMessage("Por favor, salve o registro antes de gerar o arquivo");
                return;
            }
            string error = DimeBLL.GenerateFile(((EditText)Form.Items.Item("et_Code").Specific).Value);
            if (!String.IsNullOrEmpty(error))
            {
                SBOApp.Application.SetStatusBarMessage(error);
            }
            else
            {
                SBOApp.Application.SetStatusBarMessage("Arquivo gerado com sucesso!", BoMessageTime.bmt_Medium, false);
            }
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                DBDataSource dt_DIME = Form.DataSources.DBDataSources.Item("@CVA_DIME");

                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    string period = dt_DIME.GetValue("U_Periodo", dt_DIME.Offset).Trim();
                    DateTime startDate;
                    if (!DateTime.TryParseExact($"01/{period.Trim()}", "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out startDate))
                    {
                        SBOApp.Application.MessageBox("Período deve estar no formada MM/AAAA");
                        return false;
                    }
                    dt_DIME.SetValue("U_DtDe", dt_DIME.Offset, startDate.ToString("yyyyMMdd"));
                    DateTime endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    dt_DIME.SetValue("U_DtAte", dt_DIME.Offset, endDate.ToString("yyyyMMdd"));
                }

                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    //string filial = dt_DIME.GetValue("U_Filial", dt_DIME.Offset);
                    //if (!String.IsNullOrEmpty(filial))
                    //{
                    //    if (DimeConfigBLL.Exists(Convert.ToInt32(filial)))
                    //    {
                    //        SBOApp.Application.MessageBox("Configurações já cadastradas para filial selecionada!");
                    //        return false;
                    //    }
                    //}

                    dt_DIME.SetValue("Code", dt_DIME.Offset, CrudController.GetNextCode("@CVA_DIME").PadLeft(4, '0'));
                    //((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_DIME").PadLeft(4, '0');
                }
            }

            return true;
        }
    }
}
