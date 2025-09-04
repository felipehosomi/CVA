using System;
using System.Collections.Generic;
using CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.View;
using Addon.CVA.View.Apetit.Cardapio.Helpers;
using System.IO;

namespace CVA.View.Apetit.Cardapio.Controller
{
    public class AppController
    {
        private SAPbobsCOM.Company Company { get; set; }
        private SAPbouiCOM.Application Application { get; set; }

        //Adicionar os Forms a Executar
        private List<BaseForm> formsToExecute = new List<BaseForm>
        {
            //1//new DadosPratoQtdTurnoForm(new List<QtdTurnoModel>()),
            //new DadosPratoQtdTurnoForm(null, "",0),
            new ServicoForm(),
            new AgrupamentoDeServicoForm(),
            new GrupoDeServicoForm(),
            new TipoProteinaForm(),
            new TiposDePratoForm(),
            new ModeloDeCardapioForm(),
            new PrecoXVolumeForm(),
            new IncidenciaDeProteinaForm(),
            new ComposicaoQtdForm(),
            new ItensDenegadosForm(),
            new DiasSemConsumoForm(),
            new PlanejamentoCardapioForm(),
            //new CopiaCardapioForm(),
            new TurnoForm(),
        };

        #region AutomatedEvents
        public AppController()
        {
            try
            {
                Connect();
                CreateUserFields();
                CreateFormateds();
                SetMenus();
                SetFilters();
                SetEvents();
            }
            catch (Exception ex)
            {
                Application.MessageBox($@"ocorreu um erro no Addon. {ex.Message}");
            }

            //DIHelper.AddReport("Cardapio", "[CVA] GRADE DE PLANEJAMENTO DIÁRIO DE REFEIÇÕES", "43520");

            Application.StatusBar.SetText("Add-on Planejamento de Cardápios pronto para uso.", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        private void Connect()
        {
            Company = B1Connection.Instance.Company;
            Application = B1Connection.Instance.Application;
        }

        private void SetEvents()
        {
            //Application.AppEvent += Application_AppEvent;
            //Application.FormDataEvent += Application_FormDataEvent;
            //Application.ItemEvent += Application_ItemEvent;
            Application.MenuEvent += Application_MenuEvent;
            //Application.RightClickEvent += Application_RightClickEvent;
        }

        private void SetFilters()
        {
            Filters.Add("1250000100", SAPbouiCOM.BoEventTypes.et_ALL_EVENTS);

            foreach (var item in formsToExecute)
            {
                Filters.Add(item.Type, SAPbouiCOM.BoEventTypes.et_CLICK);
                Filters.Add(item.Type, SAPbouiCOM.BoEventTypes.et_GOT_FOCUS);
                Filters.Add(item.Type, SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK);
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

        //private void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        //{
        //    var ret = true;

        //    foreach (var item in formsToExecute)
        //    {
        //        if (ret == true)
        //            item.Application_RightClickEvent(eventInfo, out ret);
        //    }

        //    bubbleEvent = ret;
        //}

        //private void Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        //{
        //    var ret = true;

        //    foreach (var item in formsToExecute)
        //    {
        //        if (ret == true)
        //        {
        //            item.Application_ItemEvent(Application, FormUID, ref pVal, out ret);
        //        }
        //    }

        //    bubbleEvent = ret;
        //}

        //private void Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        //{
        //    var ret = true;

        //    foreach (var item in formsToExecute)
        //    {
        //        if (ret == true)
        //        {
        //            item.Application_FormDataEvent(Application, ref BusinessObjectInfo, out ret);
        //        }
        //    }

        //    bubbleEvent = ret;
        //}

        //private void Application_AppEvent(SAPbouiCOM.BoAppEventTypes eventType)
        //{
        //    foreach (var item in formsToExecute)
        //    {
        //        item.Application_AppEvent(Application, eventType);
        //    }
        //}
        #endregion

        private void CreateChooseFromListOITM()
        {
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            string strSql = $@"SELECT * FROM {"@CVA_TIPOPROTEINA".Aspas()} ;";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Proteína OITM", idCategoria, "150", "CVA_ID_TIPO_PROT", "-1");
        }

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

                userFields.CreateIfNotExist("OOAT", "CVA_FILIAL", "Filial Cardapio", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("OCRD", "CVA_FILIAL", "Filial de Planejamento", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

                //userFields.CreateIfNotExist("OITM", "CVA_Proteina", "Proteína ?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                userFields.CreateIfNotExist("OITM", "CVA_Planejar", "Faz Planejamento ?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                userFields.CreateIfNotExist("OITM", "CVA_ID_TIPO_PROT", "Tipo de Proteína", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("OITM", "CVA_GRAMATURA", "Gramatura", 6, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Measurement);
                userFields.CreateIfNotExist("OITM", "CVA_MODO_PREPARO", "Modo de Preparo", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("IGE1", "CVA_TpAjuste", "Tipo de Saída", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "1", new Dictionary<object, object> { { "1", "Consumo Extra" }, { "2", "Perda de Material" } });

                #region Campos Apontamento

                UserTables.CreateIfNotExist("CVA_MOTIVO_APO", "CVA: Motivo para Apontamento", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                UserTables.CreateIfNotExist("CVA_MOTIVO_REPO", "CVA: Motivo para Reposição", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);

                userFields.CreateIfNotExist("WOR1", "CVA_SubMotivo", "Substituto", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("WOR1", "CVA_Substituto", "Substituto - Motivo", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("WOR1", "CVA_SubJust", "Substituto - Justificativa", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("OWOR", "CVA_APO_ZERO", "Apontamento Zerado?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                userFields.CreateIfNotExist("OWOR", "CVA_SERVICO", "Serviço", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("OBPL", "CVA_Codigo2PN", "Selecionar PN", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

                UserTables.CreateIfNotExist("CVA_APTO_TERCEIROS", "CVA: Apontamento Terceiros", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "FILIAL", "Filial", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "DATA", "Data", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "TURNO", "Turno", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "SERVICO", "Serviço", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "QTYPLAN", "qtd planejado", 6, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS", "QTYREF", "qtd Refeições", 6, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);

                UserTables.CreateIfNotExist("CVA_APTO_TERCEIROS1", "CVA: Apontamento Terceiros1", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS1", "CARDCODE", "CardCode", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS1", "CARDNAME", "CardName", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_APTO_TERCEIROS1", "QTYREF", "qtd Refeições", 6, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);
                userFields.CreateIfNotExist("OCRD", "CVA_TERCEIROS", "Terceiros ?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "N", "Não" }, { "Y", "Sim" } });

                #endregion

                #region UDOs

                if (!UserObjects.Exists("CVA_APTO_TERCEIROS"))
                {
                    userFields.CreateUserObject("CVA_APTO_TERCEIROS", "CVA: Apontamento Terceiros", "CVA_APTO_TERCEIROS", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                    userFields.AddChildTableToUserObject("CVA_APTO_TERCEIROS", "CVA_APTO_TERCEIROS1");
                }

                #endregion

                #endregion

                #region B1 SystemForm UserFields
                UserTables.CreateIfNotExist("CVA_CUSTO_PADRAO", "[CVA] Custo Padrao", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                userFields.CreateIfNotExist("@CVA_CUSTO_PADRAO", "CVA_Mes", "Mês", 7, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_CUSTO_PADRAO", "CVA_Contrato", "Id Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_CUSTO_PADRAO", "CVA_Valor", "Valor", 12, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
                userFields.CreateIfNotExist("@CVA_CUSTO_PADRAO", "CVA_Id_Servico", "Id Serviço", 7, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                userFields.CreateIfNotExist("@CVA_CUSTO_PADRAO", "CVA_Des_Servico", "Desc. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
                #endregion
            }
            catch (Exception ex)
            {
                Application.StatusBar.SetText(ex.Message);
                //throw ex;
            }
        }

        private void CreateFormateds()
        {
            //#region Formated OOAT
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            //string strSql = $@"SELECT {"Code".Aspas()}, {"U_CVA_DESCRICAO".Aspas()} FROM {"@CVA_MCARDAPIO".Aspas()};";
            //FormatedSearch.CreateFormattedSearches(strSql, "Busca Modelo Cardapio", idCategoria, "1250000100", "U_CVA_ID_MCARDAPIO", "-1");
            //#endregion

            #region Formated OOAT
            var strSql = $@"SELECT {"BPLId".Aspas()}, {"BPLName".Aspas()} FROM {"OBPL".Aspas()};";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Filial Cardapio", idCategoria, "1250000100", "edt_Filial", "-1");
            #endregion

            #region Formated OITM
            strSql = $@"SELECT {"Code".Aspas()}, {"Name".Aspas()} FROM {"@CVA_TIPOPROTEINA".Aspas()};";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Tp Proteina OITM", idCategoria, "150", "U_CVA_ID_TIPO_PROT", "-1");
            #endregion

            #region Formated OWHS
            strSql = $@"SELECT {"BPLId".Aspas()}, {"BPLName".Aspas()} FROM {"OBPL".Aspas()};";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Filial Parceiro", idCategoria, "134", "U_CVA_FILIAL", "-1");
            #endregion
        }

        private void SetMenus()
        {
            //var local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "1495154259_bank-building.jpg");

            Menus.Add("43520", "CVAPCARD", "Planejamento de Cardápio", -1, SAPbouiCOM.BoMenuType.mt_POPUP);
            Menus.Add("CVAPCARD", "CVAPCONFIG", "Configurações Gerais", 0, SAPbouiCOM.BoMenuType.mt_POPUP);
            Menus.Add("CVAPCARD", "CVAPDADOSC", "Dados Contratuais", 1, SAPbouiCOM.BoMenuType.mt_POPUP);

            foreach (var item in formsToExecute)
            {
                item.SetMenus();
            }
        }
    }
}
