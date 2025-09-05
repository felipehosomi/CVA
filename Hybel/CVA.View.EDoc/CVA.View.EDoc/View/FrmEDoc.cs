using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.EDoc.BLL;
using CVA.View.EDoc.DAO;
using CVA.View.EDoc.Model;
using SAPbouiCOM;
using System;

namespace CVA.View.EDoc.View
{
    [CVA.AddOn.Common.Attributes.Form(6010)]
    public class FrmEDoc : BaseForm
    {
        Form Form;

        #region Constructor
        public FrmEDoc()
        {
            FormCount++;
        }

        public FrmEDoc(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public FrmEDoc(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public FrmEDoc(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show(string srfPath)
        {
            Form = (Form)base.Show(srfPath);
            Form.Freeze(true);

            ComboBox cb_Filial = (ComboBox)Form.Items.Item("cb_Filial").Specific;
            cb_Filial.AddValuesFromQuery(SQL.Filial_Get);
            if (cb_Filial.ValidValues.Count > 0)
            {
                cb_Filial.Select(0, BoSearchKey.psk_Index);
            }

            ComboBox cb_Versao = (ComboBox)Form.Items.Item("cb_Versao").Specific;
            cb_Versao.Select(0, BoSearchKey.psk_Index);

            ComboBox cb_Arquivo = (ComboBox)Form.Items.Item("cb_Arquivo").Specific;
            cb_Arquivo.Select(0, BoSearchKey.psk_Index);

            //DateTime dataDe = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //DateTime dataAte = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            //Form.DataSources.UserDataSources.Item("ud_DtDe").Value = dataDe.ToString("dd/MM/yyyy");
            //Form.DataSources.UserDataSources.Item("ud_DtAte").Value = dataAte.ToString("dd/MM/yyyy");

            Form.Freeze(false);
            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
                    switch (ItemEventInfo.ItemUID)
                    {
                        case "bt_Gen":
                            this.GenerateEDoc();
                            break;
                        case "bt_Dir":
                            this.SelectDirectory();
                            break;
                    }
                }
            }

            return true;
        }

        private void GenerateEDoc()
        {
            EDocFilterModel filterModel = new EDocFilterModel();
            filterModel.Filial = Convert.ToInt32(Form.DataSources.UserDataSources.Item("ud_Filial").Value);
            filterModel.Diretorio = Form.DataSources.UserDataSources.Item("ud_Dir").Value;
            filterModel.DataDe = ((EditText)Form.Items.Item("et_DtDe").Specific).Value;
            filterModel.DataAte = ((EditText)Form.Items.Item("et_DtAte").Specific).Value;
            filterModel.VersaoLayout = Form.DataSources.UserDataSources.Item("ud_Layout").Value;
            filterModel.FinalidadeArquivo = Form.DataSources.UserDataSources.Item("ud_Arquivo").Value;

            if (String.IsNullOrEmpty(filterModel.DataDe))
            {
                SBOApp.Application.SetStatusBarMessage("Data Inicial deve ser informada");
                return;
            }
            if (String.IsNullOrEmpty(filterModel.DataAte))
            {
                SBOApp.Application.SetStatusBarMessage("Data Final deve ser informada");
                return;
            }

            string error = EDocFileBLL.GenerateFile(filterModel);
            if (!String.IsNullOrEmpty(error))
            {
                SBOApp.Application.SetStatusBarMessage(error);
            }
            else
            {
                SBOApp.Application.MessageBox("Arquivo gerado com sucesso!");
            }
        }

        private void SelectDirectory()
        {
            DialogUtil dialogUtil = new DialogUtil();
            Form.DataSources.UserDataSources.Item("ud_dir").Value = dialogUtil.FolderBrowserDialog();
        }
    }
}
