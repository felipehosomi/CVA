using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class ModeloDeCardapioForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Descricao = "CVA_DESCRICAO";
        public const string TB_MCardapio = "CVA_ID_MCARDAPIO";
        public const string TB_IdServico = "CVA_ID_SERVICO";
        public const string TB_DesServico = "CVA_DES_SERVICO";
        public const string TB_IdContrato = "CVA_ID_CONTRATO";
        public const string TB_DesContrato = "CVA_DES_CONTRATO";

        public const string CH_TipoPrato = "CVA_TIPO_PRATO";
        public const string CH_DesTipoPrato = "CVA_TIPO_PRATO_DES";

        public ModeloDeCardapioForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDMDLC";
            TableName = "CVA_MCARDAPIO";
            ChildName = "CVA_LIN_MCARDAPIO";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            IdToEvaluateGridEmpty = "it_CTpP";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new Helpers.UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Modelos de Cadápio", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Descricao, "Descrição", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_MCardapio, "Modelo Cardapio", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_IdServico, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesServico, "Descr. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_IdContrato, "Id Contrato", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesContrato, "Descr. Contrato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

            UserTables.CreateIfNotExist(ChildName, "[CVA] ln Modelos de Cadápio", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_TipoPrato, "Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_DesTipoPrato, "Descr. Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

            #region UDOs

            if (!UserObjects.Exists(Type)) //if (!UserObjects.ExistsThenRemove(Type)) //usado para limpar o objeto do SAP, ISTO APAGA OS REGISTROS
            {
                userFields.CreateUserObject(Type, "[CVA] Modelos de Cadápio", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
                userFields.AddChildTableToUserObject(Type, ChildName);
            }

            #endregion
        }
         
        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList();
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

                        #region Contrato CHOOSE

                        if (pVal.ItemUID.Equals("edtCSer") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            string idContrato = ((EditText)oForm.Items.Item("edtCSer").Specific).Value.ToString();

                            if (!string.IsNullOrEmpty(idContrato))
                            {
                                string bpName = "";

                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                rec.DoQuery($@"
                                            SELECT 
                                             {"Number".Aspas()},
                                             {"BpName".Aspas()}
                                            FROM {"OOAT".Aspas()}
                                            WHERE {"Number".Aspas()} = {idContrato} 
                                    ;");

                                if (rec.RecordCount > 0)
                                {
                                    bpName = rec.Fields.Item("BpName").Value.ToString();
                                    ((IEditText)oForm.Items.Item("edtCSerD").Specific).Value = bpName;
                                }
                                else
                                {
                                    Application.SetStatusBarMessage("Contrato não encontrado...");
                                    bubbleEvent = false;
                                    return;
                                }
                            }
                        }

                        #endregion

                        #region Serviço CHOOSE

                        if (pVal.ItemUID.Equals("edtSer") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var code = ((IEditText)oForm.Items.Item("edtSer").Specific).Value;

                            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rec.DoQuery($@"
                                SELECT Distinct
	                                    SP.{"Code".Aspas()} 
	                                ,SP.{ "Name".Aspas()}
                                FROM {"@CVA_SERVICO_PLAN".Aspas()} SP
                                INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} AS GRP ON
                                    SP.{ "Code".Aspas()} = GRP.{ "U_CVA_ID_SERVICO".Aspas()}	
                                WHERE 	{ "U_CVA_ATIVO".Aspas()} = 'Y'
                                    AND	SP.{"Code".Aspas()}  =  '{code}'
                            ;");
                            string name = "";

                            if (!rec.EoF) name = rec.Fields.Item("Name").Value.ToString();

                            ((IEditText)oForm.Items.Item("edtSerD").Specific).Value = name;
                        }

                        #endregion

                        #region Tipo Prato CHOOSE

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("it_CTpP") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            var it_CTpP = ((IEditText)mtx.Columns.Item("it_CTpP").Cells.Item(pVal.Row).Specific).Value;

                            if (!string.IsNullOrEmpty(it_CTpP))
                            {
                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                rec.DoQuery($@"
                                    SELECT 
	                                    * 
                                    FROM {"@CVA_TIPOPRATO".Aspas()}
                                    WHERE {"Code".Aspas()} = {it_CTpP}
                                    ;");

                                if (rec.RecordCount > 0)
                                {
                                    var name = rec.Fields.Item("Name").Value.ToString();

                                    ((IEditText)mtx.Columns.Item("it_tpPrr").Cells.Item(pVal.Row).Specific).Value = name;

                                    //workaround para problema de não carregar demais linhas, colocar direto no dataSource
                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_TipoPrato}", pVal.Row, it_CTpP);
                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_DesTipoPrato}", pVal.Row, name);
                                }
                            }else if(pVal.Row < oForm.DataSources.DBDataSources.Item("@" + ChildName).Size)
                            {
                                oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_TipoPrato}", pVal.Row, "");
                                oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_DesTipoPrato}", pVal.Row, "");
                            }
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
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPCONFIG", MenuItem, "Modelos de Cardápio", 3, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList()
        {
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            string strSql = $@"SELECT * FROM {"@CVA_TIPOPRATO".Aspas()};";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Prato Modelo", idCategoria, Type, MatrixItens, "it_CTpP");
            
            #region Busca Serviço
            strSql = $@"
                        SELECT Distinct
	                         SP.{"Code".Aspas()} 
	                        ,SP.{ "Name".Aspas()}
                        FROM {"@CVA_SERVICO_PLAN".Aspas()} SP
                        INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} AS GRP ON
                            SP.{ "Code".Aspas()} = GRP.{ "U_CVA_ID_SERVICO".Aspas()}	
                        WHERE 	{ "U_CVA_ATIVO".Aspas()} = 'Y'
            ; ";

            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Serviço Model Plan.", idCategoria, Type, "edtSer", "-1");
            #endregion

            #region Busca Contrato
            idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            strSql = $@"
                SELECT 
	                o.{"Number".Aspas()},
                    o.{"BpCode".Aspas()},
                    o.{"BpName".Aspas()},
                    o.{"StartDate".Aspas()},
                    o.{"EndDate".Aspas()}
                FROM OOAT as o
            ;";

            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Contrato Model Plan.", idCategoria, Type, "edtCSer", "-1");
            #endregion
        }
    }
}