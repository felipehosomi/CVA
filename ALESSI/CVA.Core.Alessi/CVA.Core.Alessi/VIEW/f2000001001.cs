using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Models;
using CVA.Core.Alessi.BLL;
using CVA.Core.Alessi.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using CVA.AddOn.Common.Util;

namespace CVA.Core.Alessi.VIEW
{
    /// <summary>
    /// Importação do arquivo
    /// </summary>
    public class f2000001001 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000001001()
        {
            FormCount++;
        }

        public f2000001001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            this.FillComboboxes();
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
                    if (ItemEventInfo.ItemUID == "bt_Import")
                    {
                        this.DoImport();
                    }
                    if (ItemEventInfo.ItemUID == "bt_File")
                    {
                        this.SelectFile();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "cb_ObjType")
                    {
                        this.FillComboBoxLayout();
                    }
                }
            }
            return true;
        }

        private void FillComboboxes()
        {
            ComboBox cb_ObjType = (ComboBox)Form.Items.Item("cb_ObjType").Specific;
            cb_ObjType.AddValidValues("@CVA_DOC_MAP", "ObjType");

            ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Branch").Specific;

            BusinessPlaceController bpController = new BusinessPlaceController();
            List<BusinessPlaceModel> bpList = bpController.GetActiveBPList();
            foreach (var item in bpList)
            {
                cb_Branch.ValidValues.Add(item.BPlId.ToString(), item.BPlName);
            }
        }

        private void FillComboBoxLayout()
        {
            string objType = ((ComboBox)Form.Items.Item("cb_ObjType").Specific).Value;
            if (!String.IsNullOrEmpty(objType))
            {
                ComboBox cb_Layout = (ComboBox)Form.Items.Item("cb_Layout").Specific;

                DocumentMappingBLL documentMappingBLL = new DocumentMappingBLL();
                List<string> list = documentMappingBLL.GetLayouts(Convert.ToInt32(objType));

                while (cb_Layout.ValidValues.Count > 0)
                {
                    cb_Layout.ValidValues.Remove(0, BoSearchKey.psk_Index);
                }

                int id = 1;
                foreach (var item in list)
                {
                    cb_Layout.ValidValues.Add(id.ToString(), item);
                    id++;
                }
            }
        }

        private void SelectFile()
        {
            DialogUtil dialogUtil = new DialogUtil();
            string file = dialogUtil.OpenFileDialog();
            if (!String.IsNullOrEmpty(file))
            {
                ((EditText)Form.Items.Item("et_File").Specific).Value = file;
            }
        }

        private void DoImport()
        {
            string objType = ((ComboBox)Form.Items.Item("cb_ObjType").Specific).Value;
            if (String.IsNullOrEmpty(objType))
            {
                SBOApp.Application.SetStatusBarMessage("Tipo documento deve ser informado!");
                return;
            }

            if (((ComboBox)Form.Items.Item("cb_Layout").Specific).Selected == null)
            {
                SBOApp.Application.SetStatusBarMessage("Layout deve ser informado!");
                return;
            }
            string layout = ((ComboBox)Form.Items.Item("cb_Layout").Specific).Selected.Description;

            string errorHandler = ((ComboBox)Form.Items.Item("cb_Error").Specific).Value;
            if (String.IsNullOrEmpty(errorHandler))
            {
                SBOApp.Application.SetStatusBarMessage("Tratamento de erro deve ser informado!");
                return;
            }

            string bplId = ((ComboBox)Form.Items.Item("cb_Branch").Specific).Value;
            if (String.IsNullOrEmpty(bplId))
            {
                SBOApp.Application.SetStatusBarMessage("Filial deve ser informada!");
                return;
            }

            string file = ((EditText)Form.Items.Item("et_File").Specific).Value;
            if (String.IsNullOrEmpty(bplId))
            {
                SBOApp.Application.SetStatusBarMessage("Arquivo deve ser informado!");
                return;
            }

            Form.Freeze(true);
            try
            {
                ImportParametersModel parametersModel = new ImportParametersModel();
                parametersModel.ObjType = Convert.ToInt32(objType);
                parametersModel.ErrorHandler = Convert.ToInt32(errorHandler);
                parametersModel.BPlId = Convert.ToInt32(bplId);
                parametersModel.FilePath = file;
                parametersModel.Layout = layout;

                DocumentImportBLL importBLL = new DocumentImportBLL();
                List<ImportLogModel> logList = importBLL.DoImport(parametersModel);
                DataTable dt_Log = Form.DataSources.DataTables.Item("dt_Log");
                dt_Log.Rows.Clear();
                dt_Log.Rows.Add(logList.Count);
                int i = 0;
                foreach (var item in logList)
                {
                    dt_Log.SetValue("Line", i, item.Line);
                    dt_Log.SetValue("Description", i, item.Description);
                    i++;
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
    }
}

