using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.FolhaPagamento.BLL;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.View.FolhaPagamento.VIEW
{
    public class f2000005001 : BaseForm
    {
        #region Constructor
        public f2000005001()
        {
            FormCount++;
        }

        public f2000005001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000005001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000005001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion


        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_Help")
                {
                    try
                    {
                        //System.Diagnostics.Process.Start(@"c:\\CVA Consultoria\Folha de Pagamento\Manual.pdf");
                        System.Diagnostics.Process.Start(@"\\192.168.1.16\CVA Consultoria\Folha de Pagamento\Manual.pdf");
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.SetStatusBarMessage("Erro ao abrir arquivo: " + ex.Message);
                    }
                }
            }

            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                if (ItemEventInfo.ItemUID == "et_Branch")
                {
                    Form _Form = SBOApp.Application.Forms.Item(ItemEventInfo.FormUID);
                    _Form.Freeze(true);


                    IChooseFromListEvent oCFLEvento = null;
                    oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
                    string sCFL_ID = null;
                    sCFL_ID = oCFLEvento.ChooseFromListUID;

                    SAPbouiCOM.ChooseFromList oCFL = null;
                    oCFL = _Form.ChooseFromLists.Item(sCFL_ID);

                    DataTable oDataTable = null;
                    oDataTable = oCFLEvento.SelectedObjects;
                    if (Convert.ToString(oDataTable.GetValue(0, 0)) != "")
                    {
                        try
                        {
                            ((EditText)_Form.Items.Item("et_Branch").Specific).Value = Convert.ToString(oDataTable.GetValue(0, 0));
                        }
                        catch { }
                    }
                    _Form.Freeze(false);
                }
            }

            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_File")
                {
                    Form _Form = SBOApp.Application.Forms.ActiveForm;
                    var file = Support.GetFileNameViaOFD("Arquivos csv|*.csv;", "C:\\", "Selecione o arquivo", true);
                    //SelectFileDialog dialog = new SelectFileDialog("C:\\", "", "Arquivos csv|*.csv;", DialogType.OPEN);
                    //dialog.Open();
                    ((EditText)_Form.Items.Item("et_File").Specific).Value = file;

                }
            }

            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_Imp")
                {
                    Form _Form = SBOApp.Application.Forms.ActiveForm;
                    try
                    {
                        _Form.Freeze(true);

                        int bplId;
                        DateTime docDate;
                        DateTime dueDate;

                        if (!Int32.TryParse(((EditText)_Form.Items.Item("et_Branch").Specific).Value, out bplId))
                        {
                            SBOApp.Application.SetStatusBarMessage("Filial deve ser informada!");
                            return false;
                        }
                        if (!DateTime.TryParseExact(((EditText)_Form.Items.Item("et_DocDate").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out docDate))
                        {
                            SBOApp.Application.SetStatusBarMessage("Data lançamento deve ser informada!");
                            return false;
                        }
                        if (!DateTime.TryParseExact(((EditText)_Form.Items.Item("et_DueDate").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dueDate))
                        {
                            SBOApp.Application.SetStatusBarMessage("Data vencimento deve ser informada!");
                            return false;
                        }
                        if (String.IsNullOrEmpty(((EditText)_Form.Items.Item("et_File").Specific).Value))
                        {
                            SBOApp.Application.SetStatusBarMessage("Arquivo deve ser informado!");
                            return false;
                        }

                        string lcmId = String.Empty;

                        FolhaPagamentoBLL folhaPagamentoBLL = new FolhaPagamentoBLL();
                        string msg = folhaPagamentoBLL.Gerar(bplId, docDate, dueDate, ((EditText)_Form.Items.Item("et_File").Specific).Value, ref lcmId);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            SBOApp.Application.SetStatusBarMessage(msg);
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Importação efetuada com sucesso! Nrº do LCM gerado: " + lcmId, BoMessageTime.bmt_Medium, false);
                        }
                    }
                    finally
                    {
                        _Form.Freeze(false);
                    }

                }
            }

            return base.ItemEvent();
        }

    }
}
