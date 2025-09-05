using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Taxa conversão Orçamentos
    /// </summary>
    public class f2000003043 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003043()
        {
            FormCount++;
        }

        public f2000003043(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003043(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003043(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();

            ComboBox cb_Filial = Form.Items.Item("cb_Filial").Specific as ComboBox;
            cb_Filial.ValidValues.Add("0", "Todas");
            cb_Filial.AddValuesFromQuery(SQL.Filial_Get);
            cb_Filial.Select(0, BoSearchKey.psk_Index);

            ComboBox cb_Motivo = Form.Items.Item("cb_Mot").Specific as ComboBox;
            cb_Motivo.ValidValues.Add("*", "Todos");
            cb_Motivo.AddValuesFromQuery(SQL.MotivoCancelamento_Get);
            cb_Motivo.Select(0, BoSearchKey.psk_Index);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Path")
                    {
                        DialogUtil dialogUtil = new DialogUtil();
                        Path = dialogUtil.FolderBrowserDialog(Path);
                        Form.DataSources.UserDataSources.Item("ud_Path").Value = Path;
                    }
                    if (ItemEventInfo.ItemUID == "bt_Exec")
                    {
                        ComboBox cb_Filial = Form.Items.Item("cb_Filial").Specific as ComboBox;
                        ComboBox cb_Motivo = Form.Items.Item("cb_Mot").Specific as ComboBox;
                        TaxaConversaoFiltroModel filtroModel = new TaxaConversaoFiltroModel();
                        filtroModel.CodFilial = Convert.ToInt32(cb_Filial.Selected.Value);
                        filtroModel.Filial = cb_Filial.Selected.Description;
                        filtroModel.CodMotivo = cb_Motivo.Selected.Value;
                        filtroModel.DescMotivo = cb_Motivo.Selected.Description;
                        filtroModel.Diretorio = Form.DataSources.UserDataSources.Item("ud_Path").Value.Trim();

                        DateTime dataDe;
                        DateTime dataAte;
                        if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtDe").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataDe))
                        {
                            SBOApp.Application.SetStatusBarMessage("Informe a Data de");
                            return true;
                        }
                        if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtAte").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataAte))
                        {
                            SBOApp.Application.SetStatusBarMessage("Informe a Data até");
                            return true;
                        }

                        filtroModel.DataDe = dataDe;
                        filtroModel.DataAte = dataAte;

                        string erro = OrcamentoBLL.GerarPlanilha(filtroModel);
                        if (!String.IsNullOrEmpty(erro))
                        {
                            SBOApp.Application.SetStatusBarMessage(erro);
                        }
                    }
                }
            }

            return true;
        }
    }
}
