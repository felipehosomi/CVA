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
    /// RESTGO
    /// </summary>
    public class f2000004005 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000004005()
        {
            FormCount++;
        }

        public f2000004005(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004005(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004005(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
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
                cb_Branch.ValidValues.Add("0", "Todas");
                cb_Branch.AddValuesFromQuery(Query.Branch_Get);

                return Form;
            }
            
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
                    if (ItemEventInfo.ItemUID == "bt_Exec")
                    {
                        this.Execute();
                    }
                    //if (ItemEventInfo.ItemUID == "bt_File")
                    //{
                    //    this.GenerateFile();
                    //}
                }
            }
            return true;
        }

        #region Execute
        private void Execute()
        {
            FileFilterModel filterModel = this.GetFilterModel();

            string sql = ISSRetidoBLL.GetSQL(filterModel);

            DataTable dt_Data = Form.DataSources.DataTables.Item("dt_Data");
            dt_Data.ExecuteQuery(sql);

            Grid gr_Data = Form.Items.Item("gr_Data").Specific as Grid;
            EditTextColumn cl_Total = (EditTextColumn)gr_Data.Columns.Item("Vlr. Total");
            cl_Total.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            EditTextColumn cl_BC = (EditTextColumn)gr_Data.Columns.Item("Base Cálculo");
            cl_BC.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            EditTextColumn cl_ISS = (EditTextColumn)gr_Data.Columns.Item("ISS Retido");
            cl_ISS.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

        }
        #endregion

        #region GenerateFile
        private void GenerateFile()
        {
            DialogUtil dialogUtil = new DialogUtil();
            string folder = dialogUtil.FolderBrowserDialog();

            if (!String.IsNullOrEmpty(folder))
            {
                FileFilterModel filterModel = this.GetFilterModel();
                FileBLL fileBLL = new FileBLL();
                string error = fileBLL.GenerateFile(filterModel);
                if (String.IsNullOrEmpty(error))
                {
                    SBOApp.Application.MessageBox("Arquivo gerado com sucesso!");
                }
                else
                {
                    SBOApp.Application.MessageBox(error);
                }
            }
        }
        #endregion

        private FileFilterModel GetFilterModel()
        {
            string type = ((ComboBox)Form.Items.Item("cb_Type").Specific).Value;
            if (String.IsNullOrEmpty(type))
            {
                throw new Exception("Tipo deve ser informado!");
            }

            string bplId = ((ComboBox)Form.Items.Item("cb_Branch").Specific).Value;
            if (String.IsNullOrEmpty(bplId))
            {
                throw new Exception("Filial deve ser informada!");
            }

            string period = ((EditText)Form.Items.Item("et_Period").Specific).Value;
            DateTime periodDate;
            if (!DateTime.TryParseExact("01/" + period, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out periodDate))
            {
                throw new Exception("Período deve estar no formato MM/AAAA");
            }

            FileFilterModel filterModel = new FileFilterModel();
            filterModel.Layout = type;
            filterModel.BranchId = Convert.ToInt32(bplId);
            filterModel.DateFrom = periodDate;
            filterModel.DateTo = new DateTime(periodDate.Year, periodDate.Month, DateTime.DaysInMonth(periodDate.Year, periodDate.Month));

            return filterModel;
        }
    }
}
