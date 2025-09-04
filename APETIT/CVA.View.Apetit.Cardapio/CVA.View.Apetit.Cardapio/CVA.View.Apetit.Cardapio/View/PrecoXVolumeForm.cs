using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class PrecoXVolumeForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Contrato = "CVA_ID_CONTRATO";
        public const string TB_GrpServico = "CVA_GRPSERVICO";
        public const string TB_DesGrpServico = "CVA_DES_GRPSERVICO";
        public const string TB_APartir = "CVA_APARTIR";

        //Campos da Linha
        public const string CH_QtdDe = "CVA_QTD_DE";
        public const string CH_QtdAte = "CVA_QTD_ATE";
        public const string CH_PrecoUnit = "CVA_PRECO_UNIT";
        public const string CH_Servico = "CVA_SERVICO";
        public const string CH_DesServico = "CVA_DES_SERVICO";

        public PrecoXVolumeForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDPRXV";
            TableName = "CVA_TABPRCVOL";
            ChildName = "CVA_LIN_TABPRCVOL";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\CARDPRXV.srf";
            IdToEvaluateGridEmpty = "l_QtdDe";

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

            UserTables.CreateIfNotExist(TableName, "[CVA] Preço X Volume", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_GrpServico, "ID Grupo Serv.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_APartir, "A Partir De", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesGrpServico, "Descr. Grupo Ser.", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Linhas do Cadastro", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_QtdDe, "Qtd De", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_QtdAte, "Qtd Até", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_PrecoUnit, "Preço Unitário", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_DesServico, "Descr. Ser.", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_Servico, "ID Serv.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, "[CVA] Preço X Volume", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
                //l_desSer
                if (pVal.FormTypeEx.Equals(Type))
                {
                    if (!pVal.BeforeAction)
                    {
                        #region Serviço Item CHOOSE

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_serv") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var mtx = ((Matrix)oForm.Items.Item(MatrixItens).Specific);
                            var code = ((IEditText)mtx.Columns.Item("l_serv").Cells.Item(pVal.Row).Specific).Value;
                            var edtGrpS = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;

                            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rec.DoQuery($@"
                                    SELECT DISTINCT
                                        SP.{"Code".Aspas()},
                                        SP.{ "Name".Aspas()}
                                    FROM { "@CVA_SERVICO_PLAN".Aspas()} AS SP
                                    INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} AS GS ON
                                        GS.{ "U_CVA_ID_SERVICO".Aspas()} = SP.{ "Code".Aspas()}
                                    WHERE SP.{ "U_CVA_ATIVO".Aspas()} = 'Y'
                                        AND GS.{"Code".Aspas()} =  '{edtGrpS}'
                                        AND SP.{"Code".Aspas()} =  '{code}'
                            ;");
                            string name = "";

                            if (!rec.EoF)
                            {
                                name = rec.Fields.Item("Name").Value.ToString();
                            }

                            ((IEditText)mtx.Columns.Item("l_desSer").Cells.Item(pVal.Row).Specific).Value = name;
                        }

                        #endregion

                        if (pVal.ItemUID.Equals("edtGrpS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);

                            var iChoose = (ChooseFromListEvent)pVal;
                            DataTable dataTable = iChoose.SelectedObjects;

                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                string itemCode = dataTable.GetValue("Code", 0).ToString();
                                string itemName = dataTable.GetValue("U_CVA_DESCRICAO", 0).ToString();

                                ((IEditText)oForm.Items.Item("edtGrpD").Specific).Value = itemName;
                            }
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

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Preço X Volume", 4, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList(Form oForm)
        {
            #region Busca Serviço Linha
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            var strSql = $@"
                       SELECT DISTINCT
	                        SP.{"Code".Aspas()},
                            SP.{ "Name".Aspas()}
                        FROM { "@CVA_SERVICO_PLAN".Aspas()} AS SP
                        INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} AS GS ON
                            GS.{ "U_CVA_ID_SERVICO".Aspas()} = SP.{ "Code".Aspas()}
                        WHERE SP.{ "U_CVA_ATIVO".Aspas()} = 'Y'
                            AND GS.{"Code".Aspas()} = RTRIM(LTRIM($[$edtGrpS.0]))
                ; ";

            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Serviço Preço.", idCategoria, Type, MatrixItens, "l_serv");
            #endregion
        }
    }
}