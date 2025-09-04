using System;
using System.Collections.Generic;
using CVA.View.Apetit.Helpers;
using CVA.View.Apetit.View;
using Addon.CVA.View.Apetit.Helpers;

namespace CVA.View.Apetit.Controller
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
            new FileReadForm()
        };

        public AppController()
        {
            Connect();
            CreateUserFields();
            SetMenus();
            SetFilters();
            SetEvents();
        }

        private void Connect()
        {
            Company = B1Connection.Instance.Company;
            Application = B1Connection.Instance.Application;
        }

        private void CreateUserFields()
        {
            try
            {
                var userFields = new UserFields();


                #region CVA_IMPORT_LOG

                UserTables.CreateIfNotExist(FileReadForm.TableName, "[CVA] Log de Importação", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                
                userFields.CreateIfNotExist("@" + FileReadForm.TableName, "CVA_DATA", "Data Reg.", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
                userFields.CreateIfNotExist("@" + FileReadForm.TableName, "CVA_ARQUIVO", "Nome do Arquivo", 200, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
                userFields.CreateIfNotExist("@" + FileReadForm.TableName, "CVA_QTD_OK", "Cap. Diaria", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "0");
                userFields.CreateIfNotExist("@" + FileReadForm.TableName, "CVA_QTD_ERR", "Cap. Diaria", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "0");

                #endregion


                #region CVA_IMPORT_LOG_LINE

                UserTables.CreateIfNotExist(FileReadForm.ChildTableName, "[CVA] Linha Log de Importação", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                
                userFields.CreateIfNotExist("@" + FileReadForm.ChildTableName, "CVA_ERRO_MSG", "Mensagem de Erro", 250, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
                userFields.CreateIfNotExist("@" + FileReadForm.ChildTableName, "CVA_LINHA_ERRO", "Linha do arquivo", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

                #endregion


                userFields.CreateIfNotExist("OBPL", "CVA_IDSenior", "ID Integração Senior", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

                Application.StatusBar.SetText("Add-on CVA.View.Apetit pronto para uso.", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void SetMenus()
        {
            foreach (var item in formsToExecute)
            {
                item.SetMenus();
            }
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

            foreach (var item in formsToExecute)
            {
                if (ret == true)
                    item.Application_MenuEvent(Application, ref pVal, out ret);
            }

            bubbleEvent = ret;
        }

        private void Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            foreach (var item in formsToExecute)
            {
                if (ret == true)
                    item.Application_ItemEvent(Application, FormUID, ref pVal, out ret);
            }

            bubbleEvent = ret;
        }

        private void Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            foreach (var item in formsToExecute)
            {
                if (ret == true)
                    item.Application_FormDataEvent(Application, ref BusinessObjectInfo, out ret);
            }

            bubbleEvent = ret;
        }

        private void Application_AppEvent(SAPbouiCOM.BoAppEventTypes eventType)
        {
            foreach (var item in formsToExecute)
            {
                item.Application_AppEvent(Application, eventType);
            }
        }
    }
}
