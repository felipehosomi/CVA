using EnvioNfLote.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace EnvioNfLote
{
    public class AppController
    {
        private readonly B1Connection conn;
        private EventFilters oFilters;

        public AppController()
        {
            conn = B1Connection.Instance;
            oFilters = new EventFilters();

            Database.Verify();
            UI.AddMenuItem("43520", "MenuEnvioNF", "CVA - Envio de NF em lote", -1, BoMenuType.mt_POPUP, $@"{AppDomain.CurrentDomain.BaseDirectory}\Forms\1487882608_contact.jpg");
            UI.AddMenuItem("MenuEnvioNF", "MenuConfiguracaoEmail", "Configuração de e-mail para envio", 1, BoMenuType.mt_STRING);
            UI.AddMenuItem("MenuEnvioNF", "MenuEnvio", "Envio de NF", 2, BoMenuType.mt_STRING);

            UI.AddFilter(ref oFilters, "MenuConfiguracaoEmail", BoEventTypes.et_MENU_CLICK);
            UI.AddFilter(ref oFilters, "MenuEnvio", BoEventTypes.et_MENU_CLICK);
            UI.AddFilter(ref oFilters, "CONF", BoEventTypes.et_ITEM_PRESSED);
            UI.AddFilter(ref oFilters, "ENVIO", BoEventTypes.et_DOUBLE_CLICK);
            UI.AddFilter(ref oFilters, "ENVIO", BoEventTypes.et_ITEM_PRESSED);
            UI.AddFilter(ref oFilters, "ENVIO", BoEventTypes.et_MATRIX_LINK_PRESSED);
            UI.AddFilter(ref oFilters, "ENVIO", BoEventTypes.et_CHOOSE_FROM_LIST);

            conn.Application.AppEvent += Application_AppEvent;
            conn.Application.ItemEvent += Application_ItemEvent;
            conn.Application.MenuEvent += Application_MenuEvent;            

            conn.Application.StatusBar.SetText("Add-on para envio de nota em lote liberado para uso.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
        }

        private void Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if(pVal.BeforeAction && pVal.MenuUID.Equals("MenuConfiguracaoEmail"))
            {
                var oForm = UI.LoadForm($@"{AppDomain.CurrentDomain.BaseDirectory}\Forms\ConfiguracaoEmailForm.srf");
                oForm.Freeze(true);
                oForm.EnableMenu("1282", false);
                oForm.EnableMenu("1288", false);
                oForm.EnableMenu("1289", false);
                oForm.EnableMenu("1290", false);
                oForm.EnableMenu("1291", false);

                UI.MontaConfiguracaoEmailForm(ref oForm);
                
                oForm.Freeze(false);
                oForm.Visible = true;
                oForm.Freeze(true);
                if (DI.VerificaConf())
                    DI.BuscaConf(ref oForm);
                oForm.Freeze(false);
            }

            if(pVal.BeforeAction && pVal.MenuUID.Equals("MenuEnvio"))
            {
                var oForm = UI.LoadForm($@"{AppDomain.CurrentDomain.BaseDirectory}\Forms\EnvioForm.srf");
                oForm.Freeze(true);
                oForm.EnableMenu("1282", false);
                oForm.EnableMenu("1288", false);
                oForm.EnableMenu("1289", false);
                oForm.EnableMenu("1290", false);
                oForm.EnableMenu("1291", false);

                UI.MontaEnvioForm(ref oForm);

                oForm.Freeze(false);
                oForm.Visible = true;
            }
        }

        private void Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            Form oForm = conn.Application.Forms.ActiveForm;
            try
            {
                if (pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                {
                    var oCFLEvento = (IChooseFromListEvent)pVal;
                    string sCFL_ID = oCFLEvento.ChooseFromListUID;
                    
                    if (!oCFLEvento.BeforeAction)
                    {
                        DataTable oDataTable = oCFLEvento.SelectedObjects;
                        string val = null;

                        try
                        {
                            val = Convert.ToString(oDataTable.GetValue(0, 0));
                        }
                        catch { }

                        if (pVal.ItemUID == "docIni")
                            oForm.DataSources.UserDataSources.Item("dsDocIni").ValueEx = val;
                        if (pVal.ItemUID == "docFin")
                            oForm.DataSources.UserDataSources.Item("dsDocFin").ValueEx = val;

                    }
                }

                if (pVal.FormTypeEx.Equals("ENVIO") && pVal.ItemUID.Equals("btClear") && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    oForm.Freeze(true);
                    ((Matrix)oForm.Items.Item("mtxDocs").Specific).Clear();
                    ((Matrix)oForm.Items.Item("mtxDocs").Specific).LoadFromDataSourceEx(true);

                    ((EditText)oForm.Items.Item("dtIni").Specific).Value = null;
                    ((EditText)oForm.Items.Item("dtFin").Specific).Value = null;
                    ((EditText)oForm.Items.Item("docIni").Specific).Value = null;
                    ((EditText)oForm.Items.Item("docFin").Specific).Value = null;
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals("ENVIO") && pVal.ItemUID.Equals("btSend") && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    if (B1Connection.Instance.Application.MessageBox("Confirmar o envio dos e-mails?", 2, "Sim", "Não") == 1)
                    {
                        oForm.Freeze(true);

                        try
                        {
                            Matrix oMatrix = (Matrix)oForm.Items.Item("mtxDocs").Specific;
                            if (oMatrix.VisualRowCount > 0)
                            {
                                var list = new List<string>();

                                for (int i = 1; i <= oMatrix.VisualRowCount; i++)
                                {
                                    CheckBox colCheck = (CheckBox)oMatrix.GetCellSpecific("ColChk", i);

                                    if (colCheck.Checked)
                                    {
                                        var doc = ((EditText)oMatrix.GetCellSpecific("ColDoc", i)).Value.ToString();
                                        list.Add(doc);
                                    }
                                }

                                try
                                {
                                    DI.EnviaDocumentos(list);
                                    conn.Application.MessageBox("Envio concluído com sucesso.");
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        oForm.Freeze(false);
                        oForm.Close();
                    }
                }

                if (pVal.FormTypeEx.Equals("ENVIO") && pVal.ItemUID.Equals("mtxDocs") && pVal.EventType.Equals(BoEventTypes.et_DOUBLE_CLICK) && pVal.ColUID.Equals("ColChk") && pVal.Row.Equals(0) && !pVal.BeforeAction)
                {
                    oForm.Freeze(true);

                    Matrix oMatrix = (Matrix)oForm.Items.Item("mtxDocs").Specific;

                    if (oMatrix.VisualRowCount > 0)
                    {
                        for (int i = 1; i <= oMatrix.VisualRowCount; i++)
                        {
                            CheckBox colCheck = (CheckBox)oMatrix.GetCellSpecific(pVal.ColUID, i);

                            if (colCheck.Checked)
                                colCheck.Checked = false;
                            else
                                colCheck.Checked = true;
                        }
                    }

                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals("ENVIO") && pVal.ItemUID.Equals("btFind") && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var empresa = string.Empty;
                    if (((ComboBox)oForm.Items.Item("cbComp").Specific).Selected != null)
                        empresa = ((ComboBox)oForm.Items.Item("cbComp").Specific).Selected.Value;
                    else
                        throw new Exception("Seleção de empresa é obrigatória.");

                    var dataInicial = ((EditText)oForm.Items.Item("dtIni").Specific).Value;
                    var dataFinal = ((EditText)oForm.Items.Item("dtFin").Specific).Value;
                    var documentoInicial = ((EditText)oForm.Items.Item("docIni").Specific).Value;
                    var documentoFinal = ((EditText)oForm.Items.Item("docFin").Specific).Value;
                    var status = string.Empty;
                    if (((ComboBox)oForm.Items.Item("cbSt").Specific).Selected != null)
                        status = ((ComboBox)oForm.Items.Item("cbSt").Specific).Selected.Value;
                    Matrix oMatrix = (Matrix)oForm.Items.Item("mtxDocs").Specific;

                    DI.BuscaDocumentos(ref oForm, ref oMatrix, empresa, dataInicial, dataFinal, documentoInicial, documentoFinal, status);

                }

                if (pVal.FormTypeEx.Equals("CONF") && pVal.ItemUID.Equals("btSave") && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var empresa = ((ComboBox)oForm.Items.Item("cbComp").Specific).Selected.Value;
                    var email = ((EditText)oForm.Items.Item("tbMail").Specific).Value;
                    var usuario = ((EditText)oForm.Items.Item("tbUsr").Specific).Value;
                    var senha = ((EditText)oForm.Items.Item("tbPwd").Specific).Value;
                    var smtp = ((EditText)oForm.Items.Item("tbSmtp").Specific).Value;
                    var porta = ((EditText)oForm.Items.Item("tbPort").Specific).Value;
                    var ssl = ((ComboBox)oForm.Items.Item("cbSsl").Specific).Selected.Value;
                    var emailCopia = ((EditText)oForm.Items.Item("tbMailCp").Specific).Value;
                    var mensagem = ((EditText)oForm.Items.Item("tbMsg").Specific).Value;
                    var subject = ((EditText)oForm.Items.Item("tbSub").Specific).Value;

                    if (DI.VerificaConf())
                    {
                        DI.AtualizaConf(empresa, email, usuario, senha, smtp, porta, ssl, emailCopia, mensagem, subject);
                        conn.Application.MessageBox("Configurações atualizadas com sucesso!");
                    }
                    else
                    {
                        DI.InsereConf(empresa, email, usuario, senha, smtp, porta, ssl, emailCopia, mensagem, subject);
                        conn.Application.MessageBox("Configurações inseridas com sucesso!");
                    }

                    oForm.Close();
                }
            }
            catch (Exception ex)
            {
                ret = false;
                conn.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                oForm.Freeze(false);
            }

            BubbleEvent = ret;
        }

        private void Application_AppEvent(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    if (conn.Application.Menus.Exists("MenuEnvio")) conn.Application.Menus.RemoveEx("MenuEnvio");
                    if (conn.Application.Menus.Exists("MenuConfiguracaoEmail")) conn.Application.Menus.RemoveEx("MenuConfiguracaoEmail");
                    if (conn.Application.Menus.Exists("MenuEnvioNF")) conn.Application.Menus.RemoveEx("MenuEnvioNF");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
