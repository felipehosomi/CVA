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
    /// Geração de Arquivo
    /// </summary>
    public class f2000004003 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000004003()
        {
            FormCount++;
        }

        public f2000004003(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004003(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004003(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            ComboBox cb_Layout = (ComboBox)Form.Items.Item("cb_Layout").Specific;
            cb_Layout.AddValuesFromQuery(Query.FileLayout_Get);

            if (cb_Layout.ValidValues.Count > 0)
            {
                cb_Layout.Select(0, BoSearchKey.psk_Index);
            }

            ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Branch").Specific;
            cb_Branch.ValidValues.Add("0", "Todas");
            cb_Branch.AddValuesFromQuery(Query.Branch_Get);

            //ComboBox cb_Period = (ComboBox)Form.Items.Item("cb_Period").Specific;
            //cb_Period.ValidValues.Add("0", "Mensal");
            //cb_Period.ValidValues.Add("1", "1º Decêndio / 1ª Quinzena");
            //cb_Period.ValidValues.Add("2", "2º Decêndio / 2ª Quinzena");
            //cb_Period.ValidValues.Add("3", "3º Decêndio");

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
                    if (ItemEventInfo.ItemUID == "bt_File")
                    {
                        this.GenerateFile();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Dir")
                    {
                        DialogUtil dialogUtil = new DialogUtil();
                        Form.DataSources.UserDataSources.Item("ds_Dir").Value  = dialogUtil.FolderBrowserDialog(Form.DataSources.UserDataSources.Item("ds_Dir").Value);
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "cb_Layout")
                    {
                        string layout = Form.DataSources.UserDataSources.Item("ds_Layout").Value;
                        LayoutModel layoutModel = new CrudController("@CVA_LAYOUT").RetrieveModel<LayoutModel>($"Code = '{layout}'");
                        Form.DataSources.UserDataSources.Item("ds_Dir").Value = layoutModel.Directory;
                    }
                }
            }
            return true;
        }

        #region GenerateFile
        private void GenerateFile()
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
        #endregion

        private FileFilterModel GetFilterModel()
        {
            if (((ComboBox)Form.Items.Item("cb_Layout").Specific).Selected == null)
            {
                throw new Exception("Layout deve ser informado!");
            }

            string bplId = ((ComboBox)Form.Items.Item("cb_Branch").Specific).Value;
            if (String.IsNullOrEmpty(bplId))
            {
                throw new Exception("Filial deve ser informada!");
            }

            //string period = ((ComboBox)Form.Items.Item("cb_Period").Specific).Value;
            //if (String.IsNullOrEmpty(period))
            //{
            //    throw new Exception("Período deve ser informado!");
            //}

            DateTime dtFrom;
            if (!DateTime.TryParseExact(((EditText)Form.Items.Item("et_DtFrom").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtFrom))
            {
                throw new Exception("Data de deve ser informada!");
            }

            DateTime dtTo;
            if (!DateTime.TryParseExact(((EditText)Form.Items.Item("et_DtTo").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtTo))
            {
                throw new Exception("Data até deve ser informada!");
            }

            FileFilterModel filterModel = new FileFilterModel();
            filterModel.Layout = ((ComboBox)Form.Items.Item("cb_Layout").Specific).Value;
            filterModel.LayoutDesc = ((ComboBox)Form.Items.Item("cb_Layout").Specific).Selected.Description;
            filterModel.BranchId = Convert.ToInt32(bplId);
            filterModel.DateFrom = dtFrom;
            filterModel.DateTo = dtTo;
            filterModel.Directory = Form.DataSources.UserDataSources.Item("ds_Dir").Value;
            filterModel.ExcelLayout = Form.DataSources.UserDataSources.Item("ds_Excel").Value == "Y";

            return filterModel;
        }
    }
}
