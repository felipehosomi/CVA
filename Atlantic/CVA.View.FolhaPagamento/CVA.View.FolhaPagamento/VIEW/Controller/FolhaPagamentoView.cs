using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using System.Globalization;
using CVA.View.FolhaPagamento.BLL;

namespace CVA.View.FolhaPagamento.VIEW.Controller
{
    #region Properties
    public partial class FolhaPagamentoView
    {
        private SAPbouiCOM.Application _application { get; set; }

        private FolhaPagamentoBLL _folhaPagamentoBLL { get; set; }

        public EditText et_DocDate { get; set; }
        public EditText et_DueDate { get; set; }
        public EditText et_Branch { get; set; }
        public EditText et_File { get; set; }

        public Button bt_Imp { get; set; }
        public Button bt_File { get; set; }
        public Button bt_Help { get; set; }
    }
    #endregion

    [FormAttribute("CVAFolhaPagamento", "CVA.View.FolhaPagamento.VIEW.Form.FolhaPagamento.srf")]
    [MenuEvent(UniqueUID = "CVAFolhaPagamento")]
    public partial class FolhaPagamentoView : DoverUserFormBase
    {
        public FolhaPagamentoView(SAPbouiCOM.Application application, FolhaPagamentoBLL folhaPagamentoBLL)
        {
            _application = application;
            _folhaPagamentoBLL = folhaPagamentoBLL;

            OnInitializeCustomComponents();
            OnInitializeCustomEvents();
        }

        private void OnInitializeCustomComponents()
        {
            bt_Imp = this.GetItem("bt_Imp").Specific as Button;
            bt_File = this.GetItem("bt_File").Specific as Button;
            bt_Help = this.GetItem("bt_Help").Specific as Button;

            et_DocDate = this.GetItem("et_DocDate").Specific as EditText;
            et_DueDate = this.GetItem("et_DueDate").Specific as EditText;
            et_Branch = this.GetItem("et_Branch").Specific as EditText;
            et_File = this.GetItem("et_File").Specific as EditText;
        }

        private void OnInitializeCustomEvents()
        {
            bt_Imp.ClickAfter += Bt_Imp_ClickAfter;
            bt_File.ClickAfter += Bt_File_ClickAfter;
            bt_Help.ClickAfter += Bt_Help_ClickAfter;

            et_Branch.ChooseFromListAfter += Et_Branch_ChooseFromListAfter;
        }

        protected internal virtual void Bt_Help_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                //System.Diagnostics.Process.Start(@"c:\\CVA Consultoria\Folha de Pagamento\Manual.pdf");
                System.Diagnostics.Process.Start(@"\\192.168.1.16\CVA Consultoria\Folha de Pagamento\Manual.pdf");
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("Erro ao abrir arquivo: " + ex.Message);
            }
        }

        protected internal virtual void Et_Branch_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            SBOChooseFromListEventArg chooseFromListEvent = ((SAPbouiCOM.SBOChooseFromListEventArg)(pVal));

            if (chooseFromListEvent.SelectedObjects != null)
            {
                try
                {
                    et_Branch.Value = chooseFromListEvent.SelectedObjects.GetValue(0, 0).ToString();
                }
                catch { }
            }
            this.UIAPIRawForm.Freeze(false);
        }

        protected internal virtual void Bt_File_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            SelectFileDialog dialog = new SelectFileDialog("C:\\", "", "Arquivos csv|*.csv;", DialogType.OPEN);
            dialog.Open();
            et_File.Value = dialog.SelectedFile;
        }

        protected internal virtual void Bt_Imp_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                int bplId;
                DateTime docDate;
                DateTime dueDate;

                if (!Int32.TryParse(et_Branch.Value, out bplId))
                {
                    _application.SetStatusBarMessage("Filial deve ser informada!");
                    return;
                }
                if (!DateTime.TryParseExact(et_DocDate.Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out docDate))
                {
                    _application.SetStatusBarMessage("Data lançamento deve ser informada!");
                    return;
                }
                if (!DateTime.TryParseExact(et_DueDate.Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dueDate))
                {
                    _application.SetStatusBarMessage("Data vencimento deve ser informada!");
                    return;
                }
                if (String.IsNullOrEmpty(et_File.Value))
                {
                    _application.SetStatusBarMessage("Arquivo deve ser informado!");
                    return;
                }

                string lcmId = String.Empty;
                string msg = _folhaPagamentoBLL.Gerar(bplId, docDate, dueDate, et_File.Value, ref lcmId);
                if (!String.IsNullOrEmpty(msg))
                {
                    _application.SetStatusBarMessage(msg);
                }
                else
                {
                    _application.SetStatusBarMessage("Importação efetuada com sucesso! Nrº do LCM gerado: " + lcmId, BoMessageTime.bmt_Medium, false);
                }
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
    }
}
