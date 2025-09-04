using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class GrupoDeServicoForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Descricao = "CVA_DESCRICAO";
        public const string CH_IdAgrupamento = "CVA_ID_AGRUP";
        public const string CH_IdServico = "CVA_ID_SERVICO";
        public const string CH_DescricaoServico = "CVA_D_SERVICO";
        public const string CH_Turno = "CVA_TURNO";
        public const string CH_Valor = "CVA_VALOR";

        public GrupoDeServicoForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDGRPS";
            TableName = "CVA_GRPSERVICOS";
            ChildName = "CVA_LIN_GRPSERVICOS";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\CARDGRPS.srf";
            IdToEvaluateGridEmpty = "edt_l_codS";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Grupo de Serviços", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Descricao, "Descrição", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Linhas do Cadastro", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_IdAgrupamento, "Id Agrupamento", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_IdServico, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_DescricaoServico, "Descr. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_Turno, "Turno", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_Valor, "Valor R$", 12, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, "[CVA] Grupo de Serviços", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
                userFields.AddChildTableToUserObject(Type, ChildName);
            }

            #endregion
        }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList(f);
            //oForm.Freeze(false);
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            Form oForm;
            var openMenu = OpenMenu(MenuItem, FilePath, pVal, out oForm);

            if (!string.IsNullOrEmpty(openMenu))
            {
                ret = false;
                Application.SetStatusBarMessage(openMenu);
            }

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.FormTypeEx.Equals(Type))
                {
                    if (!pVal.BeforeAction)
                    {
                        #region Serviço CHOOSE

                        //if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("edt_l_codS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                        //{
                        //    var oForm = Application.Forms.Item(pVal.FormUID);
                        //    var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

                        //    var iChoose = (ChooseFromListEvent)pVal;
                        //    DataTable dataTable = iChoose.SelectedObjects;

                        //    if (dataTable != null && dataTable.Rows.Count > 0)
                        //    {
                        //        string itemCode = dataTable.GetValue("Code", 0).ToString();
                        //        string itemName = dataTable.GetValue("Name", 0).ToString();

                        //        var dtSrc = oForm.DataSources.DBDataSources.Item("@" + ChildName);
                        //        dtSrc.SetValue($"U_{CH_IdServico}", pVal.Row - 1, itemCode);
                        //        dtSrc.SetValue($"U_{CH_DescricaoServico}", pVal.Row - 1, itemName);
                        //    }
                        //}

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("edt_l_codS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            var edtContrS = ((IEditText)mtx.Columns.Item("edt_l_codS").Cells.Item(pVal.Row).Specific).Value;
                            var codD = "";

                            if (!string.IsNullOrEmpty(edtContrS))
                            {
                                var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                rec.DoQuery($@"
                                            SELECT * 
                                            FROM {"@CVA_SERVICO_PLAN".Aspas()} AS P
                                            WHERE P.{"Code".Aspas()} = {edtContrS}
                                            ORDER BY P.{"Code".Aspas()}
                                ; ");

                                if(!rec.EoF)
                                {
                                    var name = rec.Fields.Item("Name").Value.ToString();
                                    codD = name;
                                }
                            }

                            ((IEditText)mtx.Columns.Item("edt_l_codD").Cells.Item(pVal.Row).Specific).Value = codD;
                        }

                        #endregion
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

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPCONFIG", MenuItem, "Grupo de Serviços", 0, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList(Form oForm)
        {
            #region turno choose
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            var strSql = $@"SELECT {"Name".Aspas()} FROM {"@CVA_TURNO".Aspas()} ;";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Turno Servico", idCategoria, Type, MatrixItens, $"l_Turno");
            #endregion

            #region service choose
            strSql = $@"SELECT {"Code".Aspas()}, {"Name".Aspas()} FROM {"@CVA_SERVICO_PLAN".Aspas()} WHERE {"U_CVA_ATIVO".Aspas()} = 'Y' ;";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Servico Planjamento", idCategoria, Type, MatrixItens, $"edt_l_codS");
            #endregion

            #region agrupamento service choose

            strSql = $@"
                        SELECT 
                             {"Code".Aspas()}, 
                             {"Name".Aspas()}
                        FROM {"@CVA_AGRUP_SERVICO".Aspas()} ;
                    ";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Agrupamento", idCategoria, Type, MatrixItens, "l_Agrp");
            #endregion
        }
    }
}