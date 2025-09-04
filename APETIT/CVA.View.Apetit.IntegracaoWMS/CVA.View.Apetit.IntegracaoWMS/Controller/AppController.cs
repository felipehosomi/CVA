using System;
using System.Collections.Generic;
using CVA.View.Apetit.IntegracaoWMS.Helpers;
using CVA.View.Apetit.IntegracaoWMS.View;
using Addon.CVA.View.Apetit.IntegracaoWMS.Helpers;

namespace CVA.View.Apetit.IntegracaoWMS.Controller
{
    public class AppController
    {
        private SAPbobsCOM.Company Company { get; set; }
        private SAPbouiCOM.Application Application { get; set; }
        private bool pickerClicked = false;
        private bool added = false;
        private SAPbouiCOM.Grid grid;

        //Adicionar os Forms a Executar
        private List<BaseForm> formsToExecute = new List<BaseForm>
        {
            new RotaEntregaForm(),
            new GerarArquivoForm(),
        };

        #region AutomatedEvents
        public AppController()
        {
            Connect();
            CreateUserFields();
            SetMenus();
            SetFilters();
            SetEvents();

            Application.StatusBar.SetText("Add-on Integrador WMS pronto para uso.", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        private void Connect()
        {
            Company = B1Connection.Instance.Company;
            Application = B1Connection.Instance.Application;
        }

        private void SetEvents()
        {
            Application.AppEvent += Application_AppEvent;
            Application.FormDataEvent += Application_FormDataEvent;
            Application.ItemEvent += Application_ItemEvent;
            Application.MenuEvent += Application_MenuEvent;
        }

        private void SetFilters()
        {
            foreach (var item in formsToExecute)
            {
                item.SetFilters();
            }
        }

        private void Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                foreach (var item in formsToExecute)
                {
                    if (ret == true)
                        item.Application_MenuEvent(Application, ref pVal, out ret);
                }

            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.MessageBox(ex.Message);
            }

            bubbleEvent = ret;
        }

        private void Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            foreach (var item in formsToExecute)
            {
                if (ret == true)
                {
                    item.Application_ItemEvent(Application, FormUID, ref pVal, out ret);
                }
            }

            bubbleEvent = ret;
        }

        private void Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;
            try
            {
                foreach (var item in formsToExecute)
                {
                    if (ret == true)
                    {
                        item.Application_FormDataEvent(Application, ref BusinessObjectInfo, out ret);
                    }
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.MessageBox(ex.Message);
            }

            bubbleEvent = ret;
        }

        private void Application_AppEvent(SAPbouiCOM.BoAppEventTypes eventType)
        {
            try
            {

                foreach (var item in formsToExecute)
                {
                    item.Application_AppEvent(Application, eventType);
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.MessageBox(ex.Message);
            }
        }
        #endregion

        private void CreateUserFields()
        {
            try
            {
                var userFields = new UserFields();

                foreach (var item in formsToExecute)
                {
                    try
                    {
                        item.CreateUserFields();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                #region UserFields

                userFields.CreateIfNotExist("OCRD", "CVA_DflWhs", "Depósito padrão do terceiro", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("RDR1", "CVA_IntegradoOK", "Integrado ?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });

                UserTables.CreateIfNotExist("CVA_WMS_INTEGRACAO", "[CVA] Integração WMS ENTRADA", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                userFields.CreateIfNotExist("@CVA_WMS_INTEGRACAO", "CVA_Key", "Chave da tabela princ.", 50, SAPbobsCOM.BoFieldTypes.db_Alpha);
                userFields.CreateIfNotExist("@CVA_WMS_INTEGRACAO", "CVA_ObjType", "Tipo do Objeto a ser Importado", 10, SAPbobsCOM.BoFieldTypes.db_Alpha);
                userFields.CreateIfNotExist("@CVA_WMS_INTEGRACAO", "CVA_Integrado", "Integrado ?", 1, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "0", new Dictionary<object, object> { { 0, "Criado" }, { 1, "Integrado" }, { 2, "Erro" } });
                userFields.CreateIfNotExist("@CVA_WMS_INTEGRACAO", "CVA_IntegradoEm", "Erro em", 10, SAPbobsCOM.BoFieldTypes.db_Date);
                userFields.CreateIfNotExist("@CVA_WMS_INTEGRACAO", "CVA_IntegraErro", "Integrado em", 250, SAPbobsCOM.BoFieldTypes.db_Alpha);

                #region Formated Rota
                int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
                string strSql = $@"SELECT {"Code".Aspas()}, {"U_CVA_DESCRICAO".Aspas()} FROM {"@CVA_ROTAENTREGA".Aspas()};";
                FormatedSearch.CreateFormattedSearches(strSql, "Busca Rota Transaferencia", idCategoria, "60004", "edtDCtrS", "-1");
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.MessageBox(ex.Message);
            }
        }

        private void SetMenus()
        {
            try
            {
                Menus.Add("43520", "INTEGWMS", "[CVA] Integração WMS", -1, SAPbouiCOM.BoMenuType.mt_POPUP, $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\1495154259_bank-building.jpg");

                foreach (var item in formsToExecute)
                {
                    item.SetMenus();
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.MessageBox(ex.Message);
            }
        }
    }
}
