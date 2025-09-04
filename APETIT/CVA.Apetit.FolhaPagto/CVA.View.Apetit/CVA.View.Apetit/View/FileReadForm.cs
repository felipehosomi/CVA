using Addon.CVA.View.Apetit.Helpers;
using CVA.View.Apetit.Helpers;
using CVA.View.Apetit.Model;
using SAPbouiCOM;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;

namespace CVA.View.Apetit.View
{
    public class FileReadForm : BaseForm
    {
        public static string TableName = "CVA_IMPORT_LOG";
        public static string ChildTableName = "CVA_IMPORT_LOG_LINE";
        public static string ButtonSelecionarArquivo = "btnSel";
        public static string ButtonImportar = "btnImport";
        public static string TxtArquivo = "txtArq";
        public static string Type = "CVAIMPORT";
        public static string MenuItem = "MENU_CVAIMPORT";

        public string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_file_reader.srf";

        private WindowsIdentity _user = WindowsIdentity.GetCurrent();

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            //oForm.Freeze(false);
        }

        public override void Application_MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.MenuUID.Equals(MenuItem) && !pVal.BeforeAction)
                {
                    var oForm = LoadForm(FilePath);
                    oForm.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        public override void Application_ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.FormTypeEx.Equals(Type))
                {
                    var oForm = Application.Forms.ActiveForm;
                    var filePath = ((EditText)oForm.Items.Item(TxtArquivo).Specific).Value;
                    oForm.Items.Item(ButtonImportar).Enabled = !string.IsNullOrEmpty(filePath);

                    if (pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED))
                    {
                        if (!pVal.BeforeAction)
                        {
                            #region [CVA] Importar Arquivo

                            //clicou no "Selecionar Arquivo"
                            if (pVal.ItemUID.Equals(ButtonSelecionarArquivo))
                            {
                                LoadFile(oForm);
                            }

                            // clicou botão importar
                            if (pVal.ItemUID.Equals(ButtonImportar))
                            {
                                try
                                {
                                    oForm.Freeze(true);
                                    var fileContent = System.IO.File.OpenText(filePath).ReadToEnd();
                                    var lstImport = ImportSeniorModel.ToList(fileContent);

                                    GerarPreLanctos(lstImport, filePath);
                                }
                                catch (Exception ex)
                                {
                                    Application.MessageBox(ex.Message);
                                    bubbleEvent = false;
                                    return;
                                }
                                finally
                                {
                                    oForm.Freeze(false);
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);

            Filters.Add(Type, BoEventTypes.et_COMBO_SELECT);
            Filters.Add(Type, BoEventTypes.et_CHOOSE_FROM_LIST);
            Filters.Add(Type, BoEventTypes.et_PICKER_CLICKED);
            Filters.Add(Type, BoEventTypes.et_VALIDATE);
            Filters.Add(Type, BoEventTypes.et_LOST_FOCUS);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_ADD);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_UPDATE);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_LOAD);
            Filters.Add(Type, BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(Type, BoEventTypes.et_MATRIX_LINK_PRESSED);
        }

        public override void Application_FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void Application_AppEvent(Application Application, BoAppEventTypes eventType)
        {
            switch (eventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    if (Application.Menus.Exists(MenuItem)) Application.Menus.RemoveEx(MenuItem);
                    Environment.Exit(-1);
                    break;
            }
        }

        public override void SetMenus()
        {
            //Helpers.Menus.Add("43520", "CVA", "CVA - Apetit", -1, BoMenuType.mt_POPUP);

            //position 14 antes das pastinhas, 18 depois de tudo
            Helpers.Menus.Add("1536", MenuItem, "CVA Importar Folha de Pagamento", 14, BoMenuType.mt_STRING);
        }


        #region Funçoes Praticas

        [STAThread]
        private void LoadFile(Form oForm)
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                try
                {
                    oForm.Freeze(true);
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog.Filter = "Dat Files (*.DAT)|*.DAT";
                    
                    System.Diagnostics.Process[] myProcs = System.Diagnostics.Process.GetProcessesByName("SAP Business One");
                                        
                    var currentSessionID = System.Diagnostics.Process.GetCurrentProcess().SessionId;

                    if (myProcs.Length > 0)
                    {
                        for (int i = 0; i <= myProcs.Length - 1; i++)
                        {
                            if (myProcs[i].SessionId == currentSessionID)
                            {
                                WindowWrapper myWindow = new WindowWrapper(myProcs[i].MainWindowHandle);
                                System.Windows.Forms.DialogResult dr = openFileDialog.ShowDialog(myWindow);
                                if (dr == System.Windows.Forms.DialogResult.OK)
                                {
                                    string filePath = openFileDialog.FileName;

                                    ((EditText)oForm.Items.Item(TxtArquivo).Specific).Value = filePath;

                                    oForm.Items.Item(ButtonImportar).Enabled = true;
                                }
                                return;
                            }
                            
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    oForm.Freeze(false);
                }
            });// Kick off a new thread
            t.IsBackground = true;
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }

        private void GerarPreLanctos(List<ImportSeniorModel> lstImport, string filePath)
        {
            var agrupadoFiliais = lstImport.GroupBy(x => x.Filial);

            var obtd = (SAPbobsCOM.JournalVouchers)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
            var fileName = System.IO.Path.GetFileName(filePath);

            //Inicia Log do arquivo
            var logTableCode = SaveFileLog(fileName);

            var hasError = false;
            var countOk = 0;
            var countError = 0;

            foreach (var itens in agrupadoFiliais)
            {
                B1Connection.Instance.Application.SetStatusBarMessage($"Importando Folha de Pagto {(countOk + countError)}/{lstImport.Count}", BoMessageTime.bmt_Short, false);

                var cabItem = itens.FirstOrDefault();

                obtd.JournalEntries.ReferenceDate = cabItem.DtLancto;
                obtd.JournalEntries.DueDate = cabItem.DtLancto;
                obtd.JournalEntries.Memo = cabItem.Historico.Substring(0, 50);
                obtd.JournalEntries.Reference2 = cabItem.NumLote;

                foreach (var item in itens)
                {
                    try
                    {
                        //definindo conta de débito ou crédito
                        bool? isDebitChoose = null;

                        if (!string.IsNullOrEmpty(item.CodRedDebito)) isDebitChoose = true;
                        else if (!string.IsNullOrEmpty(item.CodRedCredito)) isDebitChoose = false;
                        if (!isDebitChoose.HasValue) throw new Exception($"Erro ao definir Débito / Crédito, nenhum valor definido para leitura ");

                        var isDebit = isDebitChoose.Value;
                        var acc = isDebit ? item.CodRedDebito : item.CodRedCredito;
                        var accntntCod = GenerateAccAccount(acc);
                        var filialSenior = isDebit ? item.Filial : item.Filial_2;

                        //Obter o código da OACT pelo accntntCod
                        var acctCode = GetAccCode(accntntCod, isDebit, acc);
                        if (string.IsNullOrEmpty(acctCode))
                            throw new Exception($"Erro ao buscar AcctCode ({accntntCod}) pelo AccntntCod ('{acc}') via '{(isDebit ? "Débito" : "Crédito")}'");

                        //obter código da filial pelo SeniorId
                        var BPLId = GetBPLId(filialSenior);
                        int iBPLId = 0;
                        if (string.IsNullOrEmpty(BPLId) || !int.TryParse(BPLId, out iBPLId))
                            throw new Exception($"Erro ao buscar BPLId pelo Id da Filial Senior = '{filialSenior}'");

                        //definir valores de credito e debito
                        var creditValue = isDebit ? 0 : item.Valor;
                        var debitValue = !isDebit ? 0 : item.Valor;

                        obtd.JournalEntries.Lines.AccountCode = acctCode;
                        obtd.JournalEntries.Lines.BPLID = iBPLId;
                        obtd.JournalEntries.Lines.Credit = creditValue;
                        obtd.JournalEntries.Lines.Debit = debitValue;
                        obtd.JournalEntries.Lines.CostingCode4 = "M0000052";// isDebit ? item.CentroCusto : item.CentroCusto_2;
                        obtd.JournalEntries.Lines.Add();

                        countOk++;
                    }
                    catch (Exception ex)
                    {
                        hasError = true;
                        countError++;

                        LogLine(item.NumeroLinha, ex.Message, logTableCode);

                        continue;
                    }
                }

                obtd.JournalEntries.Add();
            }

            //faz Update da table de erro com as quantidades
            SaveFileLog(countOk, countError, logTableCode);

            var ret = obtd.Add();
            if (ret != 0)
            {
                string err;
                int code;

                B1Connection.Instance.Company.GetLastError(out code, out err);
                B1Connection.Instance.Application.SetStatusBarMessage(err);
            }

            if (hasError)
                B1Connection.Instance.Application.SetStatusBarMessage("Identificamos alguns erros ao inserir registros, verifique a tabela de Log.");

            B1Connection.Instance.Application.MessageBox("Importação finalizada");

        }

        private string GetAccCode(string accntntCod, bool isDebit, string accWithoutFormat)
        {
            SAPbobsCOM.Recordset recSet = B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            if (string.IsNullOrEmpty(accWithoutFormat)) return string.Empty;
            recSet.DoQuery($"SELECT {"AcctCode".Aspas()} AS {"AcctCode".Aspas()} FROM OACT WHERE {"AccntntCod".Aspas()} = '{(accWithoutFormat)}'");

            //if (string.IsNullOrEmpty(accntntCod)) return string.Empty;
            //recSet.DoQuery($"SELECT {"AcctCode".Aspas()} AS {"AcctCode".Aspas()} FROM OACT WHERE {"AcctCode".Aspas()} = '{(accntntCod)}'");

            if (recSet.EoF) return string.Empty;

            var acctCode = recSet.Fields.Item("AcctCode").Value;
            return acctCode;
        }

        private string GetBPLId(string seniorId)
        {
            SAPbobsCOM.Recordset recSet = B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery($"SELECT {"BPLId".Aspas()} FROM OBPL WHERE {"U_CVA_IDSenior".Aspas()} = '{seniorId}'");

            if (recSet.EoF) return string.Empty;

            var BPLId = recSet.Fields.Item("BPLId").Value.ToString();
            return BPLId;
        }
        #endregion

        #region Funções de tabela

        //Retorna o  código do arquivo criado
        /// <summary>
        /// usado para fazer UPDATE na tabela de log
        /// </summary>
        private void SaveFileLog(int ok, int err, string code)
        {
            SaveFileLog(string.Empty, ok, err, code);
        }

        //Retorna o  código do arquivo criado
        private string SaveFileLog(string fileName, int ok = 0, int err = 0, string code = "")
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            var isNew = string.IsNullOrEmpty(code);
            var nxCode = code;

            //Mensagem de existencia do arquivo
            //obtem o proximo código pela tabela caso necessário
            if (isNew)
            {
                rec.DoQuery($"SELECT COUNT(*) as {"Count".Aspas()} FROM {TableName.PreppendSymbol().Aspas()} WHERE {"U_CVA_ARQUIVO".Aspas()} = '{fileName}'");
                var hasFile = rec.Fields.Item("Count").Value > 0;
                if (hasFile)
                {
                    //1 = sim, 2 = não
                    var ret = B1Connection.Instance.Application.MessageBox("Este arquivo ja foi importado, deseja importar novamente ?", 1, "Sim", "Não");
                    if (ret == 2) throw new Exception("Processo Cancelado");
                }

                nxCode = rec.GetNextCode(TableName);

                rec.DoQuery($"INSERT INTO {TableName.PreppendSymbol().Aspas()} VALUES('{nxCode}', '{nxCode}', CURRENT_DATE, '{fileName}', {ok}, {err});");
            }
            else rec.DoQuery($"UPDATE {TableName.PreppendSymbol().Aspas()} SET {"U_CVA_QTD_OK".Aspas()} = {ok}, {"U_CVA_QTD_ERR".Aspas()} = {err} WHERE {"Code".Aspas()} = '{nxCode}'");

            return nxCode;
        }

        private void LogLine(int linhaErro, string error, string code)
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            //obtem o proximo código pela tabela caso necessário
            var nxCode = rec.GetNextCode(ChildTableName);

            rec.DoQuery($"INSERT INTO {ChildTableName.PreppendSymbol().Aspas()} VALUES('{nxCode}', {nxCode}, 'CODE {code} {error.Replace('\'', '-').RemoveDiacritics()}', {linhaErro})");
        }
        #endregion

        private string GenerateAccAccount(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 8) return string.Empty;
            return $"{value.Substring(0, 1)}.{value.Substring(1, 1)}.{value.Substring(2, 1)}.{value.Substring(3, 2)}.{value.Substring(5, 3)}";
        }
    }

    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {

        private IntPtr _hwnd;

        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }

        
}

