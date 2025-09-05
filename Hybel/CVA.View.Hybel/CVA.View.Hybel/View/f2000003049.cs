using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.DAO.Resources;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// DCIP
    /// </summary>
    public class f2000003049 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003049()
        {
            FormCount++;
        }

        public f2000003049(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003049(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003049(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();

            ComboBox cb_Filial = Form.Items.Item("cb_Filial").Specific as ComboBox;
            cb_Filial.AddValuesFromQuery(SQL.Filial_Get);
            cb_Filial.Select(0, BoSearchKey.psk_Index);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Gen")
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    int filial;
                    int tipo;
                    string periodoStr = Form.DataSources.UserDataSources.Item("ud_Periodo").Value;
                    string diretorio = Form.DataSources.UserDataSources.Item("ud_Dir").Value;

                    Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_Filial").Value, out filial);
                    if (filial == 0)
                    {
                        SBOApp.Application.SetStatusBarMessage("Filial deve ser informada");
                        return false;
                    }

                    Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_Tipo").Value, out tipo);
                    if (tipo == 0)
                    {
                        SBOApp.Application.SetStatusBarMessage("Tipo deve ser informado");
                        return false;
                    }

                    DateTime periodo;
                    if (!DateTime.TryParseExact("01/" + periodoStr, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out periodo))
                    {
                        SBOApp.Application.SetStatusBarMessage("Período deve estar no formato MM/AAAA");
                        return false;
                    }

                    if (String.IsNullOrEmpty(diretorio))
                    {
                        SBOApp.Application.SetStatusBarMessage("Diretório deve ser informado");
                        return false;
                    }

                    string msg = DcipBLL.GerarArquivo(diretorio, filial, tipo, periodo);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        SBOApp.Application.SetStatusBarMessage(msg);
                    }
                    else
                    {
                        SBOApp.Application.MessageBox("Arquivo gerado com sucesso!");
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Dir")
                {
                    DialogUtil dialogUtil = new DialogUtil();
                    string dir = dialogUtil.FolderBrowserDialog();
                    Form.DataSources.UserDataSources.Item("ud_Dir").Value = dir;
                }
            }

            return true;
        }
    }
}
