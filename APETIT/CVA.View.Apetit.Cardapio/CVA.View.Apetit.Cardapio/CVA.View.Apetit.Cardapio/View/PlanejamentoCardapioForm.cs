using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CVA.View.Apetit.Cardapio.Helpers.CrystalReport;

namespace CVA.View.Apetit.Cardapio.View
{
    public class PlanejamentoCardapioForm : BaseForm
    {
        //Campos da tabela
        public const string TB_IdCliente = "CVA_ID_CLIENTE";
        public const string TB_ClienteDes = "CVA_DES_CLIENTE";
        public const string TB_IdContrato = "CVA_ID_CONTRATO";
        public const string TB_IDModeloCardapio = "CVA_ID_MODEL_CARD";
        public const string TB_DesModeloCardapio = "CVA_DES_MODELO_CARD";
        public const string TB_Vigencia = "CVA_VIGENCIA_CONTR";
        public const string TB_DataRef = "CVA_DATA_REF";
        public const string TB_IdServico = "CVA_ID_SERVICO";
        public const string TB_DesServico = "CVA_DES_SERVICO";
        public const string TB_IdGrpServico = "CVA_ID_G_SERVICO";
        public const string TB_DesGrpServico = "CVA_DES_G_SERVICO";

        public const string CH_ModeloLinId = "CVA_MODELO_LIN_ID";
        public const string CH_PlanLinId = "CVA_PLAN_ID";
        public const string CH_TipoPrato = "CVA_TIPO_PRATO";
        public const string CH_DesTipoPrato = "CVA_TIPO_PRATO_DES";
        public const string CH_Insumo = "CVA_INSUMO";
        public const string CH_InsumoDes = "CVA_INSUMO_DES";
        public const string CH_Percent = "CVA_PERCENT";
        //public const string CH_QTD = "CVA_QTD";
        public const string CH_QTD_ORI = "CVA_QTD_ORI";
        public const string CH_CustoItem = "CVA_CUSTO_ITEM";
        //public const string CH_CustoItemPadrao = "CVA_CUSTO_ITEM_PADRAO";
        public const string CH_Total = "CVA_TOTAL";
        public const string CH_DiaSemana = "CVA_DIA_SEMANA";
        //public const string CH_QtdTurno = "CVA_ID_TURNO_QTD";

        private DadosPratoForm DadosPratoInstance = null;
        private SubstituicaoEmLote SubstituicaoEmLoteInstance = null;
        private PlanejamentoEmLote PlanejamentoEmLoteInstance = null;
        private ScreenData screenData = new ScreenData();
        private int CurrentRegistryIndex = -1;
        private List<int> CodeList = new List<int>();
        private MenuRemoveControl _menuControl = new MenuRemoveControl();

        public PlanejamentoCardapioForm()
        {
            //MatrixItens = "mtxGrps";
            Type = "CARDPLAN";
            TableName = "CVA_PLANEJAMENTO";
            ChildName = "CVA_LN_PLANEJAMENTO";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            //IdToEvaluateGridEmpty = "it_CTpP";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void CreateUserFields()
        {
            var userFields = new Helpers.UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Plan. de Cadápio", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            userFields.CreateIfNotExist("@" + TableName, TB_IdCliente, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_IdContrato, "Id Contrato", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_IDModeloCardapio, "Id Modelo", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_Vigencia, "Vigência", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_ClienteDes, "Descr. Cliente", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesModeloCardapio, "Descr. Modelo Card", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DataRef, "Data de Refer.", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_IdServico, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesServico, "Descr. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_IdGrpServico, "Id Grupo Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesGrpServico, "Descr. Grupo Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Ln Plan. de Cad.", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            userFields.CreateIfNotExist("@" + ChildName, CH_PlanLinId, "Id Planejamento", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + ChildName, CH_QtdTurno, "Id Qtd Turno", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_TipoPrato, "Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_DesTipoPrato, "Descr. Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_Insumo, "Id Insumo", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_InsumoDes, "Descr. Insumo", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_Percent, "%", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);
            //userFields.CreateIfNotExist("@" + ChildName, CH_QTD, "Qtd.", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);
            userFields.CreateIfNotExist("@" + ChildName, CH_QTD_ORI, "Qtd. Original", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);
            userFields.CreateIfNotExist("@" + ChildName, CH_CustoItem, "Custo Médio", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
            userFields.CreateIfNotExist("@" + ChildName, CH_Total, "Total", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
            userFields.CreateIfNotExist("@" + ChildName, CH_ModeloLinId, "Id Lin Model", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + ChildName, CH_DiaSemana, "Dia Semana", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + ChildName, CH_CustoItemPadrao, "Custo Padrão", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
        }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList();

            CurrentRegistryIndex = -1;
            CodeList.Clear();

            var btnTot = ((IButton)oForm.Items.Item("btnTot").Specific);
            btnTot.Caption = "Mostrar Totalizadores";
            ShowTotals(false, oForm);

            ArrowNavigationLoad(oForm);

            if (oForm.Mode == BoFormMode.fm_FIND_MODE)
            {
                //ao pesquisar bloquear todos os campos para consultar apenas pelo código do planejamento
                oForm.Items.Item("edtCliD").Enabled = false;
                oForm.Items.Item("edtContrS").Enabled = false;
                oForm.Items.Item("edtMdlS").Enabled = false;
                oForm.Items.Item("edtMdlD").Enabled = false;
                oForm.Items.Item("edtGrpS").Enabled = false;
                oForm.Items.Item("edtS").Enabled = false;
                oForm.Items.Item("edtGrpD").Enabled = false;
                oForm.Items.Item("edtD").Enabled = false;
                oForm.Items.Item("edtContrV").Enabled = false;
                oForm.Items.Item("edt_ref").Enabled = false;
                oForm.Items.Item("edtCode").Enabled = true;
                oForm.Items.Item("btnTot").Enabled = false;
                ((IEditText)oForm.Items.Item("edtCode").Specific).Value = "";
                oForm.Items.Item("edtCliS").Enabled = false;
            }
        }

        private void ArrowNavigationLoad(Form oForm)
        {
            //evita consultas adicionais
            if (!CodeList.Any())
            {
                var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rec.DoQuery($@"
                        SELECT * 
                        FROM {"@CVA_PLANEJAMENTO".Aspas()} AS P ORDER BY { "Code".Aspas() }; ");

                while (!rec.EoF)
                {
                    CodeList.Add(int.Parse(rec.Fields.Item("Code").Value.ToString()));
                    rec.MoveNext();
                }
            }

            ArrowNavigationHandler(oForm);
        }

        private void ArrowNavigationHandler(Form oForm)
        {
            if (CodeList.Count > (CurrentRegistryIndex + 1))
            {
                oForm.EnableMenu("1288", true); //proximo
                oForm.EnableMenu("1291", true); //ultimo
            }
            else
            {
                oForm.EnableMenu("1288", false); //proximo
                oForm.EnableMenu("1291", false); //ultimo
            }

            if (CurrentRegistryIndex > 0)
            {
                oForm.EnableMenu("1289", true); //anterior
                oForm.EnableMenu("1290", true); //primeiro
            }
            else
            {
                oForm.EnableMenu("1289", false); //anterior
                oForm.EnableMenu("1290", false); //primeiro
            }
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            Form f;
            var openMenu = OpenMenu(MenuItem, FilePath, pVal, out f);

            if (!string.IsNullOrEmpty(openMenu))
            {
                ret = false;
                Application.SetStatusBarMessage(openMenu);
            }

            try
            {
                if (pVal.MenuUID.Equals("REMOVE") && !string.IsNullOrEmpty(_menuControl.ColUID) && _menuControl.Row > 0 && !pVal.BeforeAction)
                {
                    //Form Planejamento
                    var oForm = Application.Forms.Item(this.FORMUID);
                    screenData.AlterarDadosETela(oForm, _menuControl.ColUID, _menuControl.Row, "", "0", "", "0");
                    AtualizaTotalizadoresTela(_menuControl.Row);
                    _menuControl.Clear();
                }

                if (pVal.MenuUID.Equals("PLANLOTE") && !string.IsNullOrEmpty(_menuControl.ColUID) && _menuControl.Row > 0 && !pVal.BeforeAction)
                {
                    var pForm = Application.Forms.ActiveForm;

                    PlanejamentoEmLoteInstance = new PlanejamentoEmLote(screenData, _menuControl.ColUID, _menuControl.Row);
                    PlanejamentoEmLoteInstance.DataChangedEvent += PlanejamentoEmLoteInstance_DataChangedEvent;
                    var oForm = PlanejamentoEmLoteInstance.LoadForm(PlanejamentoEmLoteInstance.FilePath);
                    if (oForm != null)
                        oForm.Visible = true;

                    var mtx = ((Matrix)pForm.Items.Item("mtxGrps").Specific);
                    mtx.SelectionMode = BoMatrixSelect.ms_Single;
                    mtx.DoubleClickBefore += Mtx_DoubleClickBefore;
                    for (int i = 0; i < mtx.Columns.Count; i++)
                    {
                        var column_tipoPrato = mtx.Columns.Item(i);
                        column_tipoPrato.ClickAfter += Column_tipoPrato_ClickAfter;
                    }
                }

                if (pVal.MenuUID.Equals("SUBPLAN") && !string.IsNullOrEmpty(_menuControl.ColUID) && _menuControl.Row > 0 && !pVal.BeforeAction)
                {
                    var pForm = Application.Forms.ActiveForm;


                    Form newForm;
                    SubstituicaoEmLoteInstance = new SubstituicaoEmLote(screenData, _menuControl.ColUID, _menuControl.Row);
                    SubstituicaoEmLoteInstance.OpenMenu("CARDSUBS", SubstituicaoEmLoteInstance.FilePath, null, out newForm);
                    SubstituicaoEmLoteInstance.ConfirmEvent += SubstituicaoEmLoteInstance_ConfirmEvent;
                    if (newForm != null) newForm.Visible = true;


                    //SubstituicaoEmLoteInstance = new SubstituicaoEmLote(screenData, _menuControl.ColUID, _menuControl.Row);
                    //var oForm = SubstituicaoEmLoteInstance.LoadForm(SubstituicaoEmLoteInstance.FilePath);
                    //if (oForm != null)
                    //    oForm.Visible = true;

                    var mtx = ((Matrix)pForm.Items.Item("mtxGrps").Specific);
                    mtx.SelectionMode = BoMatrixSelect.ms_Single;
                    mtx.DoubleClickBefore += Mtx_DoubleClickBefore;
                    for (int i = 0; i < mtx.Columns.Count; i++)
                    {
                        var column_tipoPrato = mtx.Columns.Item(i);
                        column_tipoPrato.ClickAfter += Column_tipoPrato_ClickAfter;
                    }
                }

                if (pVal.MenuUID.Equals("REMOVEPLAN") && !pVal.BeforeAction)
                {
                    var resp = Application.MessageBox("Deseja realmente remover o planejamento todo ?", 1, "Sim", "Cancelar");
                    if (resp == 1)
                    {
                        //apos modal de confirmação
                        //Form Planejamento
                        //var oForm = Application.Forms.ActiveForm;
                        //oForm.Freeze(true);
                        AlteracaoCardapioBanco(true, false);
                        //oForm.Freeze(false);

                        ClosePlanningOpenAnother();

                        //removido tentando acertar o bug de close do sap
                        //oForm = Application.Forms.ActiveForm;
                        //oForm.Items.Item("mtxGrps").Click();

                        bubbleEvent = false;
                        return;
                    }

                }

                if ("1282".Split(',').Contains(pVal.MenuUID) && Application.Forms.ActiveForm.TypeEx == Type && pVal.BeforeAction)
                {
                    ClosePlanningOpenAnother();

                    bubbleEvent = false;
                    return;
                }

                if ("1281".Split(',').Contains(pVal.MenuUID) && Application.Forms.ActiveForm.TypeEx == Type && pVal.BeforeAction)
                {
                    ClosePlanningOpenAnother(BoFormMode.fm_FIND_MODE);

                    bubbleEvent = false;
                    return;
                }

                //feito para evitar que ActiveForm seja validado a cada clique de menu
                if ("1288,1289,1290,1291".Split(',').Contains(pVal.MenuUID) && Application.Forms.ActiveForm.TypeEx == Type && pVal.BeforeAction)
                {
                    isLoadindData = true;
                    var oForm = B1Connection.Instance.Application.Forms.Item(this.FORMUID);
                    oForm.Freeze(true);

                    var nextIndex = CurrentRegistryIndex;
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;

                    switch (pVal.MenuUID)
                    {
                        case "1288": //proximo
                            nextIndex++;
                            break;
                        case "1289": //anterior
                            nextIndex--;
                            break;
                        case "1290": //primeiro
                            nextIndex = 0;
                            break;
                        case "1291":  //ultimo
                            nextIndex = CodeList.Count - 1;
                            break;
                    }

                    LoadPlanningData(CodeList[nextIndex]); //exception codeList zero count
                    LoadPlanningDataToScreen(oForm);

                    //caso de algum erro no loadScreen, não bagunça o index
                    CurrentRegistryIndex = nextIndex;

                    ArrowNavigationHandler(oForm);

                    var novaJanelaTask = new Task(() =>
                    {
                        var fa = B1Connection.Instance.Application.Forms.Item(this.FORMUID);
                        Thread.Sleep(700);
                        fa.Mode = BoFormMode.fm_UPDATE_MODE;
                    });

                    novaJanelaTask.Start();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    isLoadindData = false;
                    if (DIHelper.HasForm(Application, FORMUID))
                    {
                        var oForm = Application.Forms.ActiveForm;
                        oForm.Freeze(false);
                    }
                }
                catch { }
            }

            bubbleEvent = ret;
        }

        /// <summary>
        /// Método abre nova thread usar com cuidado
        /// </summary>
        /// <param name="mode"></param>
        private void ClosePlanningOpenAnother(BoFormMode mode = BoFormMode.fm_ADD_MODE)
        {
            try
            {
                var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                oForm.Close();
                screenData = new ScreenData();

                var novaJanelaTask = new Task(() =>
                {
                    Thread.Sleep(500);

                    B1Connection.Instance.Application.Menus.Item(Type).Activate();

                    Thread.Sleep(100);
                    if (mode != BoFormMode.fm_ADD_MODE)
                    {
                        oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                        oForm.Mode = mode;

                        //ao pesquisar bloquear todos os campos para consultar apenas pelo código do planejamento
                        oForm.Items.Item("edtCliD").Enabled = false;
                        oForm.Items.Item("edtContrS").Enabled = false;
                        oForm.Items.Item("edtMdlS").Enabled = false;
                        oForm.Items.Item("edtMdlD").Enabled = false;
                        oForm.Items.Item("edtGrpS").Enabled = false;
                        oForm.Items.Item("edtS").Enabled = false;
                        oForm.Items.Item("edtGrpD").Enabled = false;
                        oForm.Items.Item("edtD").Enabled = false;
                        oForm.Items.Item("edtContrV").Enabled = false;
                        oForm.Items.Item("edt_ref").Enabled = false;
                        oForm.Items.Item("edtCode").Enabled = true;
                        oForm.Items.Item("btnTot").Enabled = false;
                        ((IEditText)oForm.Items.Item("edtCode").Specific).Value = "";
                        oForm.Items.Item("edtCliS").Enabled = false;
                    }
                });
                novaJanelaTask.Start();
            }
            catch { }
        }

        private void SubstituicaoEmLoteInstance_ConfirmEvent(bool todos, DateTime de, DateTime ate, List<string> codServicos, string insumoDe, string insumoPara, string insumoParaDes)
        {
            try
            {
                //B1Connection.Instance.Application.Forms.ActiveForm.Close();

                var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                #region Query
                rec.DoQuery($@"
                    DO BEGIN

                        DECLARE _idPlanejamento	VARCHAR(12) = '9';
                        DECLARE _idContrato 	VARCHAR(12) = '1';
                        DECLARE _todosFilial 	VARCHAR(1) 	= '{(todos ? "Y" : "N")}';
                        DECLARE _de  			DATE 		= '{de.ToString("yyyyMMdd")}'; 	--YYYYMMDD
                        DECLARE _ate 			DATE 		= '{ate.ToString("yyyyMMdd")}'; 	--YYYYMMDD
                        DECLARE _insumoDE 		VARCHAR(50) = '{insumoDe}';
                        DECLARE _insumoPARA 	VARCHAR(50) = '{insumoPara}';
		                DECLARE _insumoPARADES 	VARCHAR(254) = '{insumoParaDes}';		
		
		                UPDATE {"@CVA_LN_PLANEJAMENTO".Aspas()} SET {"U_CVA_INSUMO".Aspas()} = _insumoPARA, {"U_CVA_INSUMO_DES".Aspas()} = _insumoPARADES WHERE {"Code".Aspas()} IN 
		                (
			                SELECT { "Code".Aspas()}
			                --,* 
			                FROM
			                (
				                SELECT
					                 TO_DATE(CONCAT(TO_VARCHAR(P.{ "U_CVA_DATA_REF".Aspas()},'YYYYMM'),TO_VARCHAR(TO_DATE(P1.{ "Name".Aspas()},'DD'),'DD')), 'YYYYMMDD') AS { "DataRef".Aspas()}
					                ,P1.*
				                FROM { "@CVA_LN_PLANEJAMENTO".Aspas()} AS P1	
                                INNER JOIN { "@CVA_PLANEJAMENTO".Aspas()} AS P ON
                                        P.{ "Code".Aspas()} = P1.{ "U_CVA_PLAN_ID".Aspas()}
				                WHERE 	P1.{ "U_CVA_INSUMO".Aspas()} = _insumoDE
                                    AND	P.{ "U_CVA_ID_SERVICO".Aspas()} IN ({string.Join(",", codServicos)}) --FALTA A LISTA DE SERVIÇOS
                                    AND (	
							                (_todosFilial = 'N' AND _idPlanejamento = P.{ "Code".Aspas()})
                                            OR
							                (_todosFilial = 'Y' AND P.{ "Code".Aspas()} IN 
                                                                                (
                                                                                    SELECT
                                                                                        PP.{ "Code".Aspas()}
																	                FROM { "@CVA_MCARDAPIO".Aspas()} AS M
                                                                                    INNER JOIN { "@CVA_PLANEJAMENTO".Aspas()} AS PP ON
                                                                                        PP.{ "U_CVA_ID_MODEL_CARD".Aspas()} = M.{"Code".Aspas()}
																	                WHERE M.{ "U_CVA_ID_CONTRATO".Aspas()} = _idContrato
                                                                                    AND M.{ "U_CVA_ID_SERVICO".Aspas()} IN ({string.Join(",", codServicos)})
																                )
						                )
				                )
			                ) 
			                WHERE { "DataRef".Aspas()} BETWEEN _de AND _ate
		                );
                    END
                ; ");
                #endregion

                screenData.AlterarInsumo(de, ate, codServicos, insumoDe, insumoPara, insumoParaDes);

                var oForm = B1Connection.Instance.Application.Forms.Item(this.FORMUID);
                LoadPlanningDataToScreen(oForm);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        private void PlanejamentoEmLoteInstance_DataChangedEvent()
        {
            LoadPlanningDataToScreen(B1Connection.Instance.Application.Forms.ActiveForm);
        }

        private void LoadPlanningData(int planejamentoCode)
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                SELECT * 
                FROM {"@CVA_PLANEJAMENTO".Aspas()} AS P
                LEFT JOIN {"@CVA_LN_PLANEJAMENTO".Aspas()} AS LN ON
                    P.{"Code".Aspas()} = LN.{"U_CVA_PLAN_ID".Aspas()}
                WHERE P.{"Code".Aspas()} = {planejamentoCode}
                ORDER BY LN.{"Code".Aspas()}
            ; ");

            //limpa dados carregados para tela
            screenData = new ScreenData();

            //monta tela
            if (!rec.EoF)
            {
                var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                var dtRef = ((DateTime)rec.Fields.Item("U_CVA_DATA_REF").Value).ToString("yyyyMMdd");
                var grpServico = rec.Fields.Item("U_CVA_ID_G_SERVICO").Value.ToString();
                var idServico = rec.Fields.Item("U_CVA_ID_SERVICO").Value.ToString();
                var idModelo = rec.Fields.Item("U_CVA_ID_MODEL_CARD").Value.ToString();

                //campos utilizados para montar a grid
                ((IEditText)oForm.Items.Item("edt_ref").Specific).Value = dtRef.Length == 8 ? dtRef : DateTime.Parse(dtRef).ToString("yyyyMMdd");
                ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value = grpServico;
                ((IEditText)oForm.Items.Item("edtS").Specific).Value = idServico;

                screenData.Code = planejamentoCode.ToString();
                screenData.ClienteDes = rec.Fields.Item("U_CVA_DES_CLIENTE").Value.ToString();
                screenData.DesModeloCardapio = rec.Fields.Item("U_CVA_DES_MODELO_CARD").Value.ToString();
                screenData.DesGrpServico = rec.Fields.Item("U_CVA_DES_G_SERVICO").Value.ToString();
                screenData.DesServico = rec.Fields.Item("U_CVA_DES_SERVICO").Value.ToString();
                screenData.IdCliente = rec.Fields.Item("U_CVA_ID_CLIENTE").Value.ToString();
                screenData.IdContrato = rec.Fields.Item("U_CVA_ID_CONTRATO").Value.ToString();
                screenData.IDModeloCardapio = idModelo;
                screenData.Vigencia = ((DateTime)rec.Fields.Item("U_CVA_VIGENCIA_CONTR").Value).ToString("yyyyMMdd");
                screenData.DataRef = dtRef;
                screenData.IdServico = idServico;
                screenData.IdGrpServico = grpServico;

                //NAO ALTERAR A ORDEM
                //por padrão o "montarGrid" zera o Screen data, por isto esta ordem é importante
                MontaGrid(B1Connection.Instance.Application, idModelo, true);
            }

            screenData.DayChanged += AtualizaTotalizadoresTela;

            var allTurnos = ObterTodosTurnos(planejamentoCode);
            var linhaReferente = 0;
            var modeloCorrente = "-1";

            for (int i = 0; !rec.EoF; i++)
            {
                var modeloId = rec.Fields.Item("U_CVA_MODELO_LIN_ID").Value.ToString();
                if (modeloCorrente == "-1")
                    modeloCorrente = modeloId;

                var idPlanLinha = int.Parse(rec.Fields.Item("Code").Value.ToString());

                var lineData = new LineItemData
                {
                    CustoItem = rec.Fields.Item("U_CVA_CUSTO_ITEM").Value.ToString(),
                    //CustoItemPadrao = rec.Fields.Item("U_CVA_CUSTO_ITEM_PADRAO").Value.ToString(),
                    DesTipoPrato = rec.Fields.Item("U_CVA_TIPO_PRATO_DES").Value.ToString(),
                    DiaSemana = rec.Fields.Item("U_CVA_DIA_SEMANA").Value.ToString(),
                    Insumo = rec.Fields.Item("U_CVA_INSUMO").Value.ToString(),
                    InsumoDes = rec.Fields.Item("U_CVA_INSUMO_DES").Value.ToString(),
                    ModeloLinId = modeloId,
                    Percent = rec.Fields.Item("U_CVA_PERCENT").Value.ToString(),
                    //QTD = rec.Fields.Item("U_CVA_QTD").Value.ToString(),
                    QTD_ORI = rec.Fields.Item("U_CVA_QTD_ORI").Value.ToString(),
                    TipoPrato = rec.Fields.Item("U_CVA_TIPO_PRATO").Value.ToString(),
                    Total = rec.Fields.Item("U_CVA_TOTAL").Value.ToString(),
                    QtdTurnos = allTurnos.Where(x => x.IdLinhaPlan == idPlanLinha).ToList() // CarregarTurnosQtd(rec.Fields.Item("Code").Value.ToString()) //LEAK 30 SEGUNDOS
                };

                //ja existe o modeloLinId, coluna ja foi inserida
                if (screenData.MatrixItemList.Any(x => x.Value.Any(a => a.ModeloLinId == modeloId)))
                {
                    if (modeloCorrente != modeloId)
                        linhaReferente = 0;

                    var item = screenData.MatrixItemList.FirstOrDefault(x => x.Value.Any(a => a.ModeloLinId == modeloId));
                    if (item.Value.Count > linhaReferente)
                    {
                        //até o momento os turnos vem zerado quando acontece modificação do modelo
                        if (lineData.QtdTurnos.Any())
                            item.Value[linhaReferente] = lineData;
                    }
                    else
                        item.Value.Add(lineData);
                }
                else
                {
                    var nextIndex = screenData.MatrixItemList.Count;
                    screenData.MatrixItemList.Add(nextIndex.ToString(), new List<LineItemData> { lineData });
                    linhaReferente = 0;
                }

                modeloCorrente = modeloId;
                rec.MoveNext();
                linhaReferente++;
            }
        }

        private List<QtdTurnoModel> ObterTodosTurnos(int idPlan)
        {
            var ret = new List<QtdTurnoModel>();

            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                            SELECT * FROM {"@CVA_TURNO_QTD".Aspas()}
                            WHERE {"U_CVA_PLAN_ID".Aspas()} = '{idPlan}'
                            ORDER BY {"Code".Aspas()}
            ;");

            while (!rec.EoF)
            {
                var desTurno = rec.Fields.Item("U_CVA_DES_TURNO").Value.ToString();
                var qtd = (double)rec.Fields.Item("U_CVA_QTD").Value;
                var idPlanLin = rec.Fields.Item("U_CVA_ID_LN_PLAN").Value.ToString();
                ret.Add(new QtdTurnoModel(desTurno, qtd, idPlan, int.Parse(idPlanLin)));

                rec.MoveNext();
            }

            return ret;
        }

        //private List<QtdTurnoModel> CarregarTurnosQtd(string idLinPlan)
        //{
        //    var ret = new List<QtdTurnoModel>();

        //    var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //    rec.DoQuery($@"
        //                    SELECT * FROM {"@CVA_TURNO_QTD".Aspas()}
        //                    WHERE {"U_CVA_ID_LN_PLAN".Aspas()} = '{idLinPlan}'
        //                    ORDER BY {"Code".Aspas()}
        //    ;");

        //    while (!rec.EoF)
        //    {
        //        var desTurno = rec.Fields.Item("U_CVA_DES_TURNO").Value.ToString();
        //        var qtd = (double)rec.Fields.Item("U_CVA_QTD").Value;
        //        ret.Add(new QtdTurnoModel(desTurno, qtd));

        //        rec.MoveNext();
        //    }

        //    return ret;
        //}

        private void AtualizaTotalizadoresTela(int toDay)
        {
            try
            {
                if (toDay == -1) return;

                var oForm = B1Connection.Instance.Application.Forms.Item(FORMUID);
                var btnTot = ((IButton)oForm.Items.Item("btnTot").Specific);

                if (!btnTot.Caption.ToLower().Contains("mostrar"))
                {
                    #region ESPEC - Amarelo - Mostra informações do dia Selecionado
                    var st_dia = ((IStaticText)oForm.Items.Item("st_dia").Specific);
                    var data = DateTime.ParseExact(screenData.DataRef, "yyyyMMdd", null);
                    var localData = new DateTime(data.Year, data.Month, toDay);

                    st_dia.Caption = $"Dia Selecionado {localData.ToString("dd/MM/yyyy")}";

                    var dayItens = screenData.DayItens[toDay - 1];
                    var qtdTurnos = dayItens.Sum(x => x.QtdTurnos.Sum(a => a.QTD));

                    var mprimaS = dayItens.Sum(x => x.d_Total);
                    var econsS = 0;
                    var percaptaS = qtdTurnos;
                    var padraoS = dayItens.FirstOrDefault()?.d_CustoPadrao ?? 0;
                    var saldo = padraoS - percaptaS;

                    var e_mprimaS = ((IEditText)oForm.Items.Item("e_mprimaS").Specific);
                    e_mprimaS.Value = (mprimaS).ToString("N4");

                    var e_cons_s = ((IEditText)oForm.Items.Item("e_cons_s").Specific);
                    e_cons_s.Value = (econsS).ToString("N4");// dayItens.Sum(x => x.d_CustoItem).ToString("N4");

                    var e_custo_s = ((IEditText)oForm.Items.Item("e_custo_s").Specific);
                    e_custo_s.Value = (mprimaS + econsS).ToString("N4");

                    var e_percap_s = ((IEditText)oForm.Items.Item("e_percap_s").Specific);
                    e_percap_s.Value = percaptaS == 0 ? 0d.ToString("N4") : ((mprimaS + econsS) / percaptaS).ToString("N4");

                    var e_padr_s = ((IEditText)oForm.Items.Item("e_padr_s").Specific);
                    e_padr_s.Value = (padraoS).ToString("N4");

                    var e_saldo_s = ((IEditText)oForm.Items.Item("e_saldo_s").Specific);
                    e_saldo_s.Value = (saldo).ToString("N4");
                    #endregion

                    #region ESPEC - Verde - Mostra informações acumuladas do dia 01 até o dia Selecionado
                    var st_dia_a = ((IStaticText)oForm.Items.Item("st_dia_a").Specific);

                    st_dia_a.Caption = $"Acumulado até {localData.ToString("dd/MM/yyyy")}";

                    dayItens = new List<LineItemData>();
                    for (int i = 0; i < toDay; i++)
                    {
                        dayItens.AddRange(screenData.DayItens[i]);
                    }

                    var mprimaA = dayItens.Sum(x => x.d_Total);
                    var econsA = 0;
                    var percaptaA = dayItens.Sum(x => x.QtdTurnos.Sum(a => a.QTD));
                    var padraoA = padraoS;
                    var saldoA = padraoA - percaptaA;

                    var e_mprimaA = ((IEditText)oForm.Items.Item("e_mprimaA").Specific);
                    e_mprimaA.Value = (mprimaA).ToString("N4");

                    var e_cons_a = ((IEditText)oForm.Items.Item("e_cons_a").Specific);
                    e_cons_a.Value = (econsA).ToString("N4");// dayItens.Sum(x => x.d_CustoItem).ToString("N4");

                    var e_custo_a = ((IEditText)oForm.Items.Item("e_custo_a").Specific);
                    e_custo_a.Value = (mprimaA + econsA).ToString("N4");

                    var e_percap_a = ((IEditText)oForm.Items.Item("e_percap_a").Specific);
                    e_percap_a.Value = percaptaA == 0 ? 0d.ToString("N4") : ((mprimaA + econsA) / percaptaA).ToString("N4");

                    var e_padr_a = ((IEditText)oForm.Items.Item("e_padr_a").Specific);
                    e_padr_a.Value = (padraoA).ToString("N4");

                    var e_saldo_a = ((IEditText)oForm.Items.Item("e_saldo_a").Specific);
                    e_saldo_a.Value = (saldoA).ToString("N4");
                    #endregion

                    #region ESPEC - Vermelho - Mostra informações acumuladas do mês
                    var st_dia_m = ((IStaticText)oForm.Items.Item("st_dia_m").Specific);

                    st_dia_m.Caption = $"Mês Fechado";

                    dayItens = new List<LineItemData>();
                    for (int i = 0; i < screenData.DayItens.Count; i++)
                    {
                        dayItens.AddRange(screenData.DayItens[i]);
                    }

                    var mprimaM = dayItens.Sum(x => x.d_Total);
                    var econsM = 0;
                    var percaptaM = dayItens.Sum(x => x.QtdTurnos.Sum(a => a.QTD));
                    var padraoM = padraoS;
                    var saldoM = padraoM - percaptaM;

                    var e_mprimaM = ((IEditText)oForm.Items.Item("e_mprimaM").Specific);
                    e_mprimaM.Value = (mprimaM).ToString("N4");

                    var e_cons_m = ((IEditText)oForm.Items.Item("e_cons_m").Specific);
                    e_cons_m.Value = (econsM).ToString("N4");// dayItens.Sum(x => x.d_CustoItem).ToString("N4");

                    var e_custo_m = ((IEditText)oForm.Items.Item("e_custo_m").Specific);
                    e_custo_m.Value = (mprimaM + econsM).ToString("N4");

                    var e_percap_m = ((IEditText)oForm.Items.Item("e_percap_m").Specific);
                    e_percap_m.Value = percaptaM == 0 ? 0d.ToString("N4") : ((mprimaM + econsM) / percaptaM).ToString("N4");

                    var e_padr_m = ((IEditText)oForm.Items.Item("e_padr_m").Specific);
                    e_padr_m.Value = (padraoM).ToString("N4");

                    var e_saldo_m = ((IEditText)oForm.Items.Item("e_saldo_m").Specific);
                    e_saldo_m.Value = (saldoM).ToString("N4");
                    #endregion
                }
            }
            catch { }
        }

        private bool isLoadindData = false;
        private void LoadPlanningDataToScreen(Form oForm)
        {
            ((IEditText)oForm.Items.Item("edtCliD").Specific).Value = screenData.ClienteDes;
            ((IEditText)oForm.Items.Item("edtCode").Specific).Value = screenData.Code;
            ((IEditText)oForm.Items.Item("edtContrS").Specific).Value = screenData.IdContrato;
            ((IEditText)oForm.Items.Item("edtMdlS").Specific).Value = screenData.IDModeloCardapio;
            ((IEditText)oForm.Items.Item("edtMdlD").Specific).Value = screenData.DesModeloCardapio;
            ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value = screenData.IdGrpServico;
            ((IEditText)oForm.Items.Item("edtS").Specific).Value = screenData.IdServico;
            ((IEditText)oForm.Items.Item("edtGrpD").Specific).Value = screenData.DesGrpServico;
            ((IEditText)oForm.Items.Item("edtD").Specific).Value = screenData.DesServico;
            ((IEditText)oForm.Items.Item("edtContrV").Specific).Value = screenData.Vigencia.Length == 8 ? screenData.Vigencia : screenData.Vigencia.Replace("-", "").Replace("-", "");
            ((IEditText)oForm.Items.Item("edt_ref").Specific).Value = screenData.DataRef.Length == 8 ? screenData.DataRef : DateTime.Parse(screenData.DataRef).ToString("yyyyMMdd");

            var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
            for (int i = 0; i < screenData.MatrixItemList.Count; i++)
            {
                var rows = screenData.MatrixItemList[i.ToString()];
                for (int r = 0; r < rows.Count; r++)
                {
                    try
                    {
                        var row = screenData.MatrixItemList[i.ToString()][r];

                        if (!string.IsNullOrEmpty(row?.InsumoDes))
                            ((IEditText)mtx.Columns.Item(i.ToString()).Cells.Item(r + 1).Specific).Value = $@"{row.Percent}% {row.InsumoDes}";
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            ((IEditText)oForm.Items.Item("edtCliS").Specific).Value = screenData.IdCliente;

            try
            {
                mtx.SelectionMode = BoMatrixSelect.ms_Single;
                mtx.DoubleClickBefore += Mtx_DoubleClickBefore;
                for (int i = 0; i < mtx.Columns.Count; i++)
                {
                    var column_tipoPrato = mtx.Columns.Item(i);
                    column_tipoPrato.ClickAfter += Column_tipoPrato_ClickAfter;
                }
            }
            catch { }

            //oForm.Freeze(false);
        }

        public static List<string> diasSemana = new List<string> { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", };

        public bool isAlreadyControled = false;
        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
            if (isLoadindData) return;

            try
            {
                try
                {
                    #region Tratamento da troca de tela do dadoForm, voltando o foco para a tela
                    //TODO: Aqui devemos tratar a troca de tela do dadoForm, voltando o foco para a tela
                    if (pVal.EventType == BoEventTypes.et_FORM_CLOSE && DadosPratoInstance != null && pVal.FormTypeEx.Equals(DadosPratoInstance.Type) && !pVal.Before_Action)
                    {
                        DadosPratoInstance = null;
                    }

                    if (pVal.EventType == BoEventTypes.et_FORM_DEACTIVATE && DadosPratoInstance != null && pVal.FormTypeEx.Equals(DadosPratoInstance.Type) && !pVal.Before_Action && !isAlreadyControled)
                    {
                        var dadoForm = Application.Forms.Item(DadosPratoInstance.FORMUID);
                        if (dadoForm.Visible)
                        {
                            isAlreadyControled = true;
                            dadoForm.Select();
                            isAlreadyControled = false;
                            return;
                        }
                    }
                    #endregion
                }
                catch { }

                #region botão da tela de pesquisa de insumo
                if (DadosPratoInstance != null && pVal.FormTypeEx.Equals(DadosPratoInstance.Type))
                {
                    if (pVal.ItemUID.Equals("btnConfirm"))
                    {
                        var oForm = Application.Forms.ActiveForm;
                        if (oForm.Items.Item("btnConfirm").Enabled)
                        {
                            var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
                            var selectedRowIndex = mtx.GetNextSelectedRow();

                            var itemCode = ((IEditText)mtx.Columns.Item("it_code").Cells.Item(selectedRowIndex).Specific).Value;
                            var itemName = ((IEditText)mtx.Columns.Item("it_name").Cells.Item(selectedRowIndex).Specific).Value;
                            var edtPrice = ((IEditText)mtx.Columns.Item("edtPrice").Cells.Item(selectedRowIndex).Specific).Value;
                            var edtPerc = ((IEditText)oForm.Items.Item("edtPerc").Specific).Value;
                            var localData = DadosPratoInstance;

                            oForm.Close();

                            //Form Planejamento
                            oForm = Application.Forms.Item(this.FORMUID);
                            screenData.AlterarDadosETela(oForm, localData.ColUID, localData.Row, itemCode, edtPerc, itemName, edtPrice);
                            AtualizaTotalizadoresTela(localData.Row);
                        }
                    }
                    else if (pVal.ItemUID.Equals("btnCancel"))
                    {
                        var oForm = Application.Forms.ActiveForm;
                        oForm.Close();
                    }
                }
                #endregion  

                if (pVal.FormTypeEx.Equals(Type))
                {
                    if (pVal.BeforeAction)
                    {
                        if (pVal.FormMode == (int)BoFormMode.fm_OK_MODE)
                        {
                            var oForm = Application.Forms.Item(FormUID);
                            oForm.Mode = BoFormMode.fm_UPDATE_MODE;
                        }

                        try
                        {
                            var oForm = Application.Forms.Item(FormUID);
                            oForm.Items.Item("btnVisuMPl").Enabled = oForm.Mode == BoFormMode.fm_UPDATE_MODE;
                            oForm.Items.Item("btnVisuCNT").Enabled = oForm.Mode == BoFormMode.fm_UPDATE_MODE;
                        }
                        catch { }

                        #region Contrato CHOOSE

                        if (pVal.ItemUID.Equals("edtContrS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            string idContrato = ((EditText)oForm.Items.Item("edtContrS").Specific).Value.ToString();
                            screenData.IdContrato = idContrato;

                            if (!string.IsNullOrEmpty(idContrato))
                            {
                                string endDate = "";
                                var cardCode = ((IEditText)oForm.Items.Item("edtCliS").Specific).Value;

                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                rec.DoQuery($@"
                                            SELECT 
                                             {"Number".Aspas()},
                                             {"BpCode".Aspas()},
                                             {"EndDate".Aspas()}
                                            FROM {"OOAT".Aspas()}
                                            WHERE {"Number".Aspas()} = {idContrato} 
                                            AND {"BpCode".Aspas()} = '{cardCode}'
                                    ;");

                                if (!rec.EoF)
                                {
                                    endDate = rec.Fields.Item("EndDate").Value.ToString();

                                    if (!string.IsNullOrEmpty(endDate))
                                    {
                                        DateTime dtTime = DIHelper.Format_StringToDate(endDate);
                                        ((IEditText)oForm.Items.Item("edtContrV").Specific).Value = dtTime.ToString("yyyyMMdd");
                                        screenData.Vigencia = dtTime.ToString("yyyy-MM-dd");
                                    }
                                }
                                else
                                {
                                    ((IEditText)oForm.Items.Item("edtContrV").Specific).Value = "";
                                    screenData.IdContrato = "";

                                    ClearHeaderFields(pVal);
                                }
                            }
                        }

                        #endregion

                        #region Mostrar Totais
                        if (pVal.ItemUID.Equals("btnTot") && pVal.EventType.Equals(BoEventTypes.et_CLICK))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var btnTotIt = oForm.Items.Item("btnTot");

                            if (btnTotIt.Enabled)
                            {
                                var btnTot = ((IButton)btnTotIt.Specific);

                                var isShow = btnTot.Caption.ToLower().Contains("mostrar");
                                var caption = isShow ? "Esconder Totalizadores" : "Mostrar Totalizadores";

                                ShowTotals(isShow);
                                btnTot.Caption = caption;

                                if (isShow)
                                {
                                    var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
                                    var selectedRow = mtx.GetNextSelectedRow(0, BoOrderType.ot_RowOrder);
                                    AtualizaTotalizadoresTela(selectedRow);
                                }
                            }
                        }
                        #endregion
                    }

                    if (!pVal.BeforeAction)
                    {
                        if (pVal.ItemUID == "btnVisuMPl" && pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE && pVal.EventType == BoEventTypes.et_CLICK)
                        {
                            var oForm = Application.Forms.Item(FormUID);
                            if (oForm.Items.Item("btnVisuMPl").Enabled)
                            {
                                var planId = ((IEditText)oForm.Items.Item("edtCode").Specific).Value.ToString();

                                Hashtable reportParams = new Hashtable();
                                reportParams.Add("idPlanejamento", planId); //Convert.ToString(Form.DataSources.UserDataSources.Item("ud_DtAte").Value));

                                CrystalReport crRelatorio = new CrystalReport();
                                crRelatorio.ExecuteCrystalReport("Cardapio.rpt", reportParams);
                            }
                        }

                        if (pVal.ItemUID == "btnVisuCNT" && pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE && pVal.EventType == BoEventTypes.et_CLICK)
                        {
                            var oForm = Application.Forms.Item(FormUID);
                            if (oForm.Items.Item("btnVisuCNT").Enabled)
                            {
                                var planId = ((IEditText)oForm.Items.Item("edtCode").Specific).Value.ToString();

                                Form oF;
                                var contr = new ContratadoXPlanejado(planId);
                                contr.OpenMenu(contr.Type, contr.FilePath, null, out oF);
                                if (oF != null) oF.Visible = true;
                            }
                        }

                        //search Button
                        if (pVal.ItemUID == "1" && pVal.FormMode == (int)BoFormMode.fm_FIND_MODE)
                        {
                            var oForm = Application.Forms.Item(FormUID);
                            var planCode = ((EditText)oForm.Items.Item("edtCode").Specific).Value;

                            if (string.IsNullOrEmpty(planCode))
                                Application.SetStatusBarMessage("Digite o código do planejamento primeiro.");
                            else
                            {
                                int planCodeId = 0;
                                if (!int.TryParse(planCode, out planCodeId))
                                    Application.SetStatusBarMessage("Digite o código NUMÉRICO do planejamento corretamente.");
                                else
                                {
                                    var indexOf = CodeList.IndexOf(planCodeId);
                                    if (indexOf >= 0)
                                    {
                                        CurrentRegistryIndex = indexOf - 1;
                                        Application.Menus.Item("1288").Activate();
                                    }
                                    else
                                        Application.SetStatusBarMessage("Planejamento não encontrado.");
                                }


                            }
                        }

                        //Desabilita a alteração da data referencia
                        try
                        {
                            if (pVal.FormMode == (int)BoFormMode.fm_ADD_MODE || pVal.FormMode == (int)BoFormMode.fm_EDIT_MODE || pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE || pVal.FormMode == (int)BoFormMode.fm_OK_MODE)
                            {
                                var oForm = Application.Forms.Item(FormUID);
                                var hasData = screenData != null && screenData.HasData();

                                var planId = ((IEditText)oForm.Items.Item("edtCode").Specific).Value.ToString();

                                //Caso data referencia ainda esteja ON, e está ficará OFF, mostra mensagem caso haja planejamento para estes dados de cabeçalho
                                if (oForm.Items.Item("edt_ref").Enabled && hasData && string.IsNullOrEmpty(planId))
                                    ShowAlreadyExistMessage(screenData.IdCliente, screenData.IdContrato, screenData.IdGrpServico, screenData.IdServico, screenData.RefDate);

                                if ((oForm.Items.Item("edt_ref").Enabled && hasData) || (!oForm.Items.Item("edt_ref").Enabled && !hasData))
                                {
                                    oForm.Items.Item("edt_ref").Enabled = !hasData;
                                    ((IEditText)oForm.Items.Item("edt_fake").Specific).Value = "";

                                    oForm.Items.Item("edtGrpS").Enabled = !hasData;
                                    oForm.Items.Item("edtContrS").Enabled = !hasData;
                                    oForm.Items.Item("edtS").Enabled = !hasData;
                                    oForm.Items.Item("edtCliS").Enabled = !hasData;
                                    //oForm.Items.Item("edtMdlS").Enabled = !hasData;
                                }
                            }
                        }
                        catch { }

                        #region Cliente CHOOSE

                        if (pVal.ItemUID.Equals("edtCliS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var cardCode = ((IEditText)oForm.Items.Item("edtCliS").Specific).Value;
                            var cardName = "";

                            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rec.DoQuery($@"
                                            SELECT 
                                                {"CardName".Aspas()}
                                            FROM {"OCRD".Aspas()} 
                                            WHERE {"CardCode".Aspas()} = '{cardCode}'
                            ;");

                            if (!rec.EoF)
                            {
                                cardName = rec.Fields.Item("CardName").Value.ToString();

                                screenData.IdCliente = cardCode;
                                screenData.ClienteDes = cardName;
                            }
                            else
                            {
                                screenData.IdCliente = "";
                                screenData.ClienteDes = cardName;
                                ClearHeaderFields(pVal);
                            }

                            ((IEditText)oForm.Items.Item("edtCliD").Specific).Value = cardName;
                        }

                        #endregion

                        #region Grupo Serviço CHOOSE

                        if (pVal.ItemUID.Equals("edtGrpS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var code = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;
                            var contrato = ((IEditText)oForm.Items.Item("edtContrS").Specific).Value;
                            var name = "";

                            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rec.DoQuery($@"
                                    SELECT distinct
                                        GRP.{"Code".Aspas()}
                                        ,GRP.{"U_CVA_DESCRICAO".Aspas()}
                                    FROM {"@CVA_GRPSERVICOS".Aspas()} AS GRP
                                    INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} GLIN ON
                                        GRP.{ "Code".Aspas()} = GLIN.{ "Code".Aspas()}
                                    INNER JOIN { "@CVA_MCARDAPIO".Aspas()} AS MODEL ON
                                        GLIN.{ "U_CVA_ID_SERVICO".Aspas()} = MODEL.{ "U_CVA_ID_SERVICO".Aspas()}
                                    WHERE MODEL.{"U_CVA_ID_CONTRATO".Aspas()} = '{contrato}'
                                    AND   GRP.{"Code".Aspas()} = '{code}'
                            ;");

                            if (!rec.EoF)
                            {
                                name = rec.Fields.Item("U_CVA_DESCRICAO").Value.ToString();

                                screenData.IdGrpServico = code;
                                screenData.DesGrpServico = name;
                            }
                            else
                            {
                                screenData.IdGrpServico = "";
                                screenData.DesGrpServico = name;
                                ClearHeaderFields(pVal);
                            }

                            ((IEditText)oForm.Items.Item("edtGrpD").Specific).Value = name;
                        }

                        #endregion

                        #region Serviço CHOOSE

                        if (pVal.ItemUID.Equals("edtS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var code = ((IEditText)oForm.Items.Item("edtS").Specific).Value;
                            //var edtGrpS = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;
                            //var idContrato = ((EditText)oForm.Items.Item("edtContrS").Specific).Value.ToString();

                            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                            rec.DoQuery($@"
                                SELECT distinct
	                                 MODEL.{"Code".Aspas()} as {"ModelCode".Aspas()}
	                                ,MODEL.{"U_CVA_DESCRICAO".Aspas()} as {"U_CVA_DESCRICAO".Aspas()}
	                                ,GLIN.{"U_CVA_D_SERVICO".Aspas()} as {"Name".Aspas()}
                                FROM {"@CVA_GRPSERVICOS".Aspas()} AS GRP
                                INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} GLIN ON
                                    GRP.{ "Code".Aspas()} = GLIN.{ "Code".Aspas()}
                                INNER JOIN { "@CVA_MCARDAPIO".Aspas()} AS MODEL ON
                                    GLIN.{ "U_CVA_ID_SERVICO".Aspas()} = MODEL.{ "U_CVA_ID_SERVICO".Aspas()}
                                WHERE   GRP.{"Code".Aspas()} = '{screenData.IdGrpServico}'
                                    AND MODEL.{ "U_CVA_ID_SERVICO".Aspas()} = '{code}'
                                    AND {"U_CVA_ID_CONTRATO".Aspas()} = '{screenData.IdContrato}'
                            ; ");

                            string name = "";
                            string idModelo = "";
                            string desModelo = "";

                            if (!rec.EoF)
                            {
                                name = rec.Fields.Item("Name").Value.ToString();
                                idModelo = rec.Fields.Item("ModelCode").Value.ToString();
                                desModelo = rec.Fields.Item("U_CVA_DESCRICAO").Value.ToString();

                                screenData.IDModeloCardapio = idModelo;
                                screenData.DesModeloCardapio = desModelo;

                                screenData.IdServico = code;
                                screenData.DesServico = name;
                            }
                            else
                            {
                                screenData.IDModeloCardapio = "";
                                screenData.DesModeloCardapio = desModelo;
                                screenData.IdServico = "";
                                screenData.DesServico = name;
                                ClearHeaderFields(pVal);
                            }

                            ((IEditText)oForm.Items.Item("edtMdlS").Specific).Value = idModelo;
                            ((IEditText)oForm.Items.Item("edtMdlD").Specific).Value = desModelo;
                            ((IEditText)oForm.Items.Item("edtD").Specific).Value = name;
                        }

                        #region Mudança de Moldelo, monta grid, por Data Referencia e Serviço

                        if ((pVal.ItemUID.Equals("edt_ref") || pVal.ItemUID.Equals("edtS")) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
                        {
                            try
                            {
                                var oForm = Application.Forms.Item(pVal.FormUID);
                                oForm.Freeze(true);
                                var idModelo = ((IEditText)oForm.Items.Item("edtMdlS").Specific).Value;
                                var grpServ = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;

                                if (!string.IsNullOrEmpty(idModelo))
                                {
                                    MontaGrid(Application, idModelo);

                                    try
                                    {
                                        var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
                                        mtx.SelectionMode = BoMatrixSelect.ms_Single;
                                        mtx.DoubleClickBefore += Mtx_DoubleClickBefore;
                                        screenData.DayChanged += AtualizaTotalizadoresTela;
                                    }
                                    catch { }
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {
                                try
                                {
                                    var oForm = Application.Forms.Item(pVal.FormUID);
                                    oForm.Freeze(false);
                                }
                                catch { }
                            }
                        }

                        #endregion

                        #endregion

                        #region Btn Adicionar / Alterar
                        if (pVal.ItemUID.Equals("1") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && ((pVal.FormMode == (int)BoFormMode.fm_ADD_MODE) || pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE))
                        {
                            //var oForm = Application.Forms.ActiveForm;
                            //oForm.Freeze(true);
                            AlteracaoCardapioBanco(pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE);
                            //oForm.Freeze(false);

                            ClosePlanningOpenAnother();

                            //removido tentando acertar o bug de close do sap
                            //var oForm = Application.Forms.ActiveForm;
                            //oForm.Items.Item("mtxGrps").Click();
                            bubbleEvent = false;
                            return;
                        }
                        #endregion

                        #region Menu Click Direito Control
                        if (pVal.ItemUID.Equals("mtxGrps") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && ((pVal.FormMode == (int)BoFormMode.fm_ADD_MODE) || pVal.FormMode == (int)BoFormMode.fm_UPDATE_MODE))
                        {
                            if (pVal.ColUID != "" && pVal.Row > 0)
                            {
                                _menuControl.ColUID = pVal.ColUID;
                                _menuControl.Row = pVal.Row;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.HResult != -2000)
                    {
                        Application.SetStatusBarMessage(ex.Message);
                        ret = false;
                    }

                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(false);
                }
                catch (Exception exx)
                {
                }
            }

            bubbleEvent = ret;

            //if (DadosPratoInstance != null)
            //    DadosPratoInstance.Application_ItemEvent( FormUID, ref pVal, out bubbleEvent);

            //if (SubstituicaoEmLoteInstance != null)
            //    SubstituicaoEmLoteInstance.Application_ItemEvent( FormUID, ref pVal, out bubbleEvent);

            //if (PlanejamentoEmLoteInstance != null)
            //    PlanejamentoEmLoteInstance.Application_ItemEvent( FormUID, ref pVal, out bubbleEvent);
        }

        private bool hasAlreadyCleared = false;
        private void ClearHeaderFields(ItemEvent pVal)
        {
            if (!hasAlreadyCleared)
            {
                hasAlreadyCleared = true;

                var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                var edtS = ((IEditText)oForm.Items.Item("edtS").Specific);
                var edtCliS = ((IEditText)oForm.Items.Item("edtCliS").Specific);
                var edtGrpS = ((IEditText)oForm.Items.Item("edtGrpS").Specific);
                var edtContrS = ((IEditText)oForm.Items.Item("edtContrS").Specific);

                if (!pVal.ItemUID.Equals("edtS") && !string.IsNullOrEmpty(edtS.Value))
                    edtS.Value = "";

                if (!pVal.ItemUID.Equals("edtCliS") && !string.IsNullOrEmpty(edtCliS.Value))
                    edtCliS.Value = "";

                if (!pVal.ItemUID.Equals("edtGrpS") && !string.IsNullOrEmpty(edtGrpS.Value))
                    edtGrpS.Value = "";

                if (!pVal.ItemUID.Equals("edtContrS") && !string.IsNullOrEmpty(edtContrS.Value))
                    edtContrS.Value = "";

                hasAlreadyCleared = false;
            }
        }

        private void ShowAlreadyExistMessage(string cardCode, string contrato, string grpServico, string servico, DateTime dt_ref)
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                SELECT 
	                * 
                FROM {"@CVA_PLANEJAMENTO".Aspas()}
                WHERE 	{ "U_CVA_ID_CLIENTE".Aspas()} = '{cardCode}'
                    AND	{ "U_CVA_ID_CONTRATO".Aspas()} = '{contrato}'
                    AND { "U_CVA_ID_G_SERVICO".Aspas()} = '{grpServico}'
                    AND {"U_CVA_ID_SERVICO".Aspas()} = '{servico}'
                    AND	{ "U_CVA_DATA_REF".Aspas()} = '{dt_ref.ToString("yyyyMMdd")}'
            ;");

            if (!rec.EoF)
                B1Connection.Instance.Application.MessageBox("Aviso: Já existe um planejamento para este Cliente, Contrato, Grupo, Serviço e Data Referência.");
        }

        private void ShowTotals(bool show = true, Form form = null)
        {
            var oForm = form ?? B1Connection.Instance.Application.Forms.ActiveForm;
            oForm.Items.Item("st_dia").Visible = show;
            oForm.Items.Item("l_matp_s").Visible = show;
            oForm.Items.Item("l_cons_s").Visible = show;
            oForm.Items.Item("l_ctotal_s").Visible = show;
            oForm.Items.Item("l_pcapt_s").Visible = show;
            oForm.Items.Item("l_padr_s").Visible = show;
            oForm.Items.Item("l_saldo_s").Visible = show;
            oForm.Items.Item("e_mprimaS").Visible = show;
            oForm.Items.Item("e_cons_s").Visible = show;
            oForm.Items.Item("e_custo_s").Visible = show;
            oForm.Items.Item("e_percap_s").Visible = show;
            oForm.Items.Item("e_padr_s").Visible = show;
            oForm.Items.Item("e_saldo_s").Visible = show;

            oForm.Items.Item("st_dia_a").Visible = show;
            oForm.Items.Item("l_matp_a").Visible = show;
            oForm.Items.Item("l_cons_a").Visible = show;
            oForm.Items.Item("l_ctotal_a").Visible = show;
            oForm.Items.Item("l_pcapt_a").Visible = show;
            oForm.Items.Item("l_padr_a").Visible = show;
            oForm.Items.Item("l_saldo_a").Visible = show;
            oForm.Items.Item("e_mprimaA").Visible = show;
            oForm.Items.Item("e_cons_a").Visible = show;
            oForm.Items.Item("e_custo_a").Visible = show;
            oForm.Items.Item("e_percap_a").Visible = show;
            oForm.Items.Item("e_padr_a").Visible = show;
            oForm.Items.Item("e_saldo_a").Visible = show;

            oForm.Items.Item("st_dia_m").Visible = show;
            oForm.Items.Item("l_matp_m").Visible = show;
            oForm.Items.Item("l_cons_m").Visible = show;
            oForm.Items.Item("l_ctotal_m").Visible = show;
            oForm.Items.Item("l_pcapt_m").Visible = show;
            oForm.Items.Item("l_padr_m").Visible = show;
            oForm.Items.Item("l_saldo_m").Visible = show;
            oForm.Items.Item("e_mprimaM").Visible = show;
            oForm.Items.Item("e_cons_m").Visible = show;
            oForm.Items.Item("e_custo_m").Visible = show;
            oForm.Items.Item("e_percap_m").Visible = show;
            oForm.Items.Item("e_padr_m").Visible = show;
            oForm.Items.Item("e_saldo_m").Visible = show;
        }

        private ReferenceDate CarregarQuantidadesPadrao()
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            var date = DateTime.ParseExact(screenData.DataRef, "yyyyMMdd", null);

            rec.DoQuery($@"
                DO BEGIN

	                DECLARE _idContrato VARCHAR(12) = '{screenData.IdContrato}';
	                DECLARE _grpServico VARCHAR(12) = '{screenData.IdGrpServico}';
	                DECLARE _idServico VARCHAR(12) = '{screenData.IdServico}';
	                DECLARE _dtRef VARCHAR(7) = '{date.ToString("MM/yyyy")}';
	
	                SELECT 
			            SUM(lc.{ "U_CVA_SEGUNDA".Aspas()}  ) AS { "U_CVA_SEGUNDA".Aspas()}
		  	           ,SUM(lc.{ "U_CVA_TERCA".Aspas()}   )  AS { "U_CVA_TERCA".Aspas()}  
                       ,SUM(lc.{ "U_CVA_QUARTA".Aspas()}  )  AS { "U_CVA_QUARTA".Aspas()} 
                       ,SUM(lc.{ "U_CVA_QUINTA".Aspas()}  )  AS { "U_CVA_QUINTA".Aspas()} 
                       ,SUM(lc.{ "U_CVA_SEXTA".Aspas()}   )  AS { "U_CVA_SEXTA".Aspas()}  
                       ,SUM(lc.{ "U_CVA_SABADO".Aspas()}  )  AS { "U_CVA_SABADO".Aspas()} 
                       ,SUM(lc.{ "U_CVA_DOMINGO".Aspas()} )  AS { "U_CVA_DOMINGO".Aspas()}
                       ,lc.{"U_CVA_DES_TURNO".Aspas()}   AS {"U_CVA_DES_TURNO".Aspas()}
                    , (
                        SELECT top 1 {"U_CVA_Valor".Aspas()}
                            FROM { "@CVA_CUSTO_PADRAO".Aspas()}
                        WHERE { "U_CVA_Mes".Aspas()} = _dtRef
                               AND { "U_CVA_Id_Servico".Aspas()} = _idServico
                    ) AS { "CustoPadrao".Aspas()} -- ANTIGO edtCustoP
                    FROM { "@CVA_COMENSAIS".Aspas()} as c
                        INNER JOIN { "@CVA_LIN_COMENSAIS".Aspas()} as lc on
                                lc.{ "Code".Aspas()} = c.{ "Code".Aspas()}
	                WHERE    c.{ "U_CVA_ID_CONTRATO".Aspas()} = _idContrato
                        AND  c.{ "U_CVA_GRPSERVICO".Aspas()} = _grpServico 
                        AND  lc. {"U_CVA_SERVICO".Aspas()} = _idServico
                    GROUP BY
                        { "U_CVA_C_PADRAO".Aspas()}, lc.{"U_CVA_DES_TURNO".Aspas()}
                ;
                END
            ;");

            var ret = new ReferenceDate();
            ret.Clear();

            while (!rec.EoF)
            {
                ret.CustoPadrao = double.Parse(rec.Fields.Item("CustoPadrao").Value.ToString());

                //Dictionary<string, double>
                ret.Add(double.Parse(rec.Fields.Item("U_CVA_DOMINGO").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_SEGUNDA").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_TERCA").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_QUARTA").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_QUINTA").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_SEXTA").Value.ToString()),
                    double.Parse(rec.Fields.Item("U_CVA_SABADO").Value.ToString()),
                    rec.Fields.Item("U_CVA_DES_TURNO").Value.ToString()
                );

                rec.MoveNext();
            }

            return ret;
        }

        private void AlteracaoCardapioBanco(bool deletarAnteriores, bool incluirTela = true)
        {
            screenData.ValidateData();

            var code = DIHelper.GetNextCode(TableName).ToString();
            var sData = screenData;
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            if (deletarAnteriores)
            {
                var planId = ((IEditText)B1Connection.Instance.Application.Forms.ActiveForm.Items.Item("edtCode").Specific).Value.ToString();
                code = planId;

                var delQuery = $@"DELETE FROM {"@CVA_PLANEJAMENTO".Aspas()} WHERE {"Code".Aspas()} = '{code}';";
                rec.DoQuery(delQuery);

                delQuery = $@"DELETE FROM {"@CVA_LN_PLANEJAMENTO".Aspas()} WHERE {"U_CVA_PLAN_ID".Aspas()} = '{code}';";
                rec.DoQuery(delQuery);

                delQuery = $@"DELETE FROM {"@CVA_TURNO_QTD".Aspas()} WHERE {"U_CVA_PLAN_ID".Aspas()} = '{code}';";
                rec.DoQuery(delQuery);
            }

            if (incluirTela)
            {
                var query = $@"
                            INSERT INTO {"@CVA_PLANEJAMENTO".Aspas()} 
                                VALUES(
                                     {code},
                                    '{code}',
                                    '{sData.IdCliente}',
                                    '{sData.IdContrato}',
                                    '{sData.IDModeloCardapio}',
                                    '{sData.Vigencia}',
                                    '{sData.ClienteDes}',
                                    '{sData.DesModeloCardapio}',
                                    '{sData.DataRef}',
                                    '{sData.IdServico}',
                                    '{sData.DesServico}',
                                    '{sData.IdGrpServico}',
                                    '{sData.DesGrpServico}'
            ); " + Environment.NewLine + Environment.NewLine;

                var cCode = DIHelper.GetNextCode(ChildName);

                rec.DoQuery(query);
                query = "";

                foreach (var column in sData.MatrixItemList)
                {
                    for (int i = 0; i < column.Value.Count; i++)
                    {
                        var row = column.Value[i];
                        query += $@"
                        INSERT INTO {"@CVA_LN_PLANEJAMENTO".Aspas()} 
                        VALUES(
                            {cCode + i},
                            {(i + 1).ToString().ToDbNull()},
                            {code.ToDbNull()},
                            {row.TipoPrato.ToDbNull()},
                             {row.DesTipoPrato.ToDbNull()},
                             {row.Insumo.ToDbNull()},
                             {row.InsumoDes.ToDbNull()},
                             {row.d_Percent.ToDbNull()},
                             {row.d_QTDORI.ToDbNull()},
                             {row.d_CustoItem.ToDbNull()},
                             {row.d_Total.ToDbNull()},
                            {row.ModeloLinId.ToDbNull()},
                            {row.DiaSemana.ToDbNull()},
                            null
                        );
                        " + Environment.NewLine;

                        rec.DoQuery(query);
                        query = "";
                        var turnoCode = DIHelper.GetNextCode("CVA_TURNO_QTD");

                        for (int k = 0; k < row.QtdTurnos.Count; k++)
                        {
                            var turno = row.QtdTurnos[k];
                            query += $@"
                            INSERT INTO {"@CVA_TURNO_QTD".Aspas()} 
                            VALUES(
                                { turnoCode + k},
                                {(k + 1).ToString().ToDbNull()},
                                {code.ToDbNull()},
                                {(cCode + i).ToString().ToDbNull()},
                                (select top 1 {"Code".Aspas()} from {"@CVA_TURNO".Aspas()} WHERE {"Name".Aspas()} = '{turno.Turno}'),
                                {turno.Turno.ToString().ToDbNull()},
                                {turno.QTD.ToDbNull()}                    
                            );
                            " + Environment.NewLine;
                            rec.DoQuery(query);
                            query = "";
                        }
                    }
                    cCode += column.Value.Count;
                }

                if (!string.IsNullOrEmpty(query))
                    rec.DoQuery(query);
            }
        }

        private void MontaDiaReferencia(IMatrix mtx, DataTable dataTable)
        {
            //adição da coluna
            var cln_dtDia = mtx.Columns.Add("dt_dia", BoFormItemTypes.it_EDIT);
            cln_dtDia.Editable = false;
            cln_dtDia.TitleObject.Caption = "Dia";
            cln_dtDia.Visible = true;
            cln_dtDia.RightJustified = false;
            cln_dtDia.ClickAfter += Column_tipoPrato_ClickAfter;

            //adição da coluna no dataTable
            var colDt = dataTable.Columns.Add("dt_dia", BoFieldsType.ft_Text, 50);
            cln_dtDia.DataBind.Bind("mtxGrps", "dt_dia");
        }

        private void MontaTipoPrato(IMatrix mtx, DataTable dataTable, string idModelo, bool isEdit = false)
        {
            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                            SELECT 
	                            ML.{"Code".Aspas()},
                                ML.{"LineId".Aspas()},
                                ML.{"U_CVA_TIPO_PRATO".Aspas()},
                                ML.{"U_CVA_TIPO_PRATO_DES".Aspas()}
                            FROM {"@CVA_MCARDAPIO".Aspas()} AS M
                            INNER JOIN {"@CVA_LIN_MCARDAPIO".Aspas()} AS ML ON
                                ML.{"Code".Aspas()} = M.{"Code".Aspas()}
                            WHERE M.{"Code".Aspas()} = '{idModelo}'
                            ORDER BY ML.{"LineId".Aspas()}
            ;");

            screenData.MatrixItemList.Clear();

            for (int i = 0; !rec.EoF; i++, rec.MoveNext())
            {
                var code = rec.Fields.Item("Code").Value.ToString();
                var lineId = rec.Fields.Item("LineId").Value.ToString();
                var tipoPrato = rec.Fields.Item("U_CVA_TIPO_PRATO").Value.ToString();
                var tipoPratoDesc = rec.Fields.Item("U_CVA_TIPO_PRATO_DES").Value.ToString();
                var colName = i.ToString();// Guid.NewGuid().ToString().Substring(0, 8);

                //if (!isEdit)
                //{
                screenData.MatrixItemList.Add(colName, new List<LineItemData> {
                    new LineItemData {
                        ModeloLinId = lineId,
                        TipoPrato = tipoPrato,
                        DesTipoPrato = tipoPratoDesc
                        }
                    });
                //}

                //adição da coluna
                var column_tipoPrato = mtx.Columns.Add(colName, BoFormItemTypes.it_EDIT);
                column_tipoPrato.Editable = false;
                column_tipoPrato.ClickAfter += Column_tipoPrato_ClickAfter;
                column_tipoPrato.TitleObject.Caption = tipoPratoDesc;
                column_tipoPrato.Visible = true;
                column_tipoPrato.RightJustified = false;

                //adição da coluna no dataTable
                var colDt = dataTable.Columns.Add(colName, BoFieldsType.ft_Text, 50);
                column_tipoPrato.DataBind.Bind("mtxGrps", colName);
            }
        }

        private void Column_tipoPrato_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var oForm = B1Connection.Instance.Application.Forms.Item(FORMUID);
                var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);

                mtx.SelectRow(pVal.Row, true, false);

                screenData.DiaSelecionado = pVal.Row;
            }
            catch
            {

                var oForm = B1Connection.Instance.Application.Forms.Item(FORMUID);
                var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);

                mtx.SelectRow(1, true, false);

                screenData.DiaSelecionado = 1;
            }
        }

        private void CarregarDiaReferenciaLinhas(IMatrix mtx, DataTable dataTable, DateTime refDate, bool isEdit = false)
        {
            var cln_dtDia = mtx.Columns.Item("dt_dia");

            var dt = new DateTime(refDate.Year, refDate.Month + 1, 1, 0, 0, 0);
            dt = dt.Subtract(new TimeSpan(0, 1, 0));

            //adição das linhas
            for (int i = 1; i <= dt.Day; i++)
            {
                mtx.AddRow();
                dataTable.Rows.Add();

                var dtDia = new DateTime(dt.Year, dt.Month, i);
                var oDtDia = (EditText)cln_dtDia.Cells.Item(i).Specific;
                var diaSemana = diasSemana[(int)dtDia.DayOfWeek];
                oDtDia.Value = $"{i.ToString("00")} - {diaSemana}.";

                //if (!isEdit)
                //{
                foreach (var item in screenData.MatrixItemList)
                {
                    if (i == 1)
                    {
                        item.Value[0].DiaSemana = diaSemana;
                    }
                    else
                    {
                        var reference = item.Value[0];
                        if (item.Value.Count > i && !string.IsNullOrEmpty(item.Value[i].ModeloLinId) && !string.IsNullOrEmpty(item.Value[i].TipoPrato) && !string.IsNullOrEmpty(item.Value[i].DesTipoPrato) && !string.IsNullOrEmpty(item.Value[i].DiaSemana))
                            continue;

                        item.Value.Add(new LineItemData
                        {
                            ModeloLinId = reference.ModeloLinId,
                            TipoPrato = reference.TipoPrato,
                            DesTipoPrato = reference.DesTipoPrato,
                            DiaSemana = diaSemana
                        });
                    }
                    //}
                }
            }
        }


        private void MontaGrid(Application Application, string idModelo, bool isEdit = false)
        {
            var oForm = Application.Forms.Item(FORMUID);
            var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
            DataTable dataTable = oForm.DataSources.DataTables.Item("mtxGrps");
            var refDateStr = ((IEditText)oForm.Items.Item("edt_ref").Specific).Value;

            DateTime refDate;
            var refDateParse = DateTime.TryParseExact(refDateStr, "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out refDate);
            var grpServ = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;
            var serv = ((IEditText)oForm.Items.Item("edtS").Specific).Value;

            if (mtx.Columns.Count > 0)
            {
                screenData.MatrixItemList.Clear();
                mtx.Clear();
                dataTable.Clear();

                while (mtx.Columns.Count > 0)
                {
                    mtx.Columns.Remove(0);
                }
            }

            if (refDateParse && !string.IsNullOrEmpty(grpServ) && !string.IsNullOrEmpty(serv))
            {
                screenData.DataRef = refDateStr;

                MontaDiaReferencia(mtx, dataTable);
                MontaTipoPrato(mtx, dataTable, idModelo, isEdit);
                CarregarDiaReferenciaLinhas(mtx, dataTable, refDate, isEdit);

                //if (!isEdit)
                //{
                var qtdPadroes = CarregarQuantidadesPadrao();
                screenData.AtualizaPadrao(qtdPadroes);
                //}

                mtx.AutoResizeColumns();
            }
        }

        private void Mtx_DoubleClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            var ret = true;
            BubbleEvent = ret;

            try
            {
                if (pVal.ColUID.ToLower().Equals("dt_dia")) return;
                if (DadosPratoInstance != null && DIHelper.HasForm(B1Connection.Instance.Application, DadosPratoInstance.FORMUID)) return;

                var form = B1Connection.Instance.Application.Forms.Item(FORMUID);
                var mtx = ((Matrix)form.Items.Item("mtxGrps").Specific);

                var edt_dia = ((IEditText)mtx.Columns.Item("dt_dia").Cells.Item(pVal.Row).Specific).Value;
                var diaS = edt_dia.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).First();
                var dia = int.Parse(diaS);
                var date = DateTime.ParseExact(screenData.DataRef, "yyyyMMdd", null);
                var rDate = (new DateTime(date.Year, date.Month, dia));

                Form oForm;
                DadosPratoInstance = new DadosPratoForm(pVal.ColUID, pVal.Row, screenData.IdContrato, screenData.IdCliente, screenData.IdGrpServico, rDate, screenData);
                DadosPratoInstance.OpenMenu("CARDPLIT", DadosPratoInstance.FilePath, null, out oForm);
                if (oForm != null) oForm.Visible = true;

                mtx.SelectionMode = BoMatrixSelect.ms_Single;
                //mtx.DoubleClickBefore += Mtx_DoubleClickBefore;
                for (int i = 0; i < mtx.Columns.Count; i++)
                {
                    var column_tipoPrato = mtx.Columns.Item(i);
                    column_tipoPrato.ClickAfter += Column_tipoPrato_ClickAfter;
                }

                var perc = screenData.MatrixItemList[pVal.ColUID][pVal.Row - 1].d_Percent;
                ((IEditText)oForm.Items.Item("edtPerc").Specific).Value = perc == 0 ? "100" : perc.ToString();
                ((IEditText)oForm.Items.Item("edtSearch").Specific).Value = "";
            }
            catch { }
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                var oApplication = B1Connection.Instance.Application;
                var oForm = oApplication.Forms.Item(eventInfo.FormUID);
                SAPbouiCOM.Menus omenus;
                var oCreationPackage = (MenuCreationParams)oApplication.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
                oCreationPackage.Type = BoMenuType.mt_STRING;
                var omenuitem = oApplication.Menus.Item("1280");
                omenus = omenuitem.SubMenus;

                if (omenuitem.SubMenus.Exists("772")) omenus.RemoveEx("772");       //copiar
                if (omenuitem.SubMenus.Exists("784")) omenus.RemoveEx("784");       //copiar Tabela
                if (omenuitem.SubMenus.Exists("4870")) omenus.RemoveEx("4870");     //filtrar tabela

                if (omenuitem.SubMenus.Exists("SUBPLAN")) omenus.RemoveEx("SUBPLAN");
                if (omenuitem.SubMenus.Exists("PLANLOTE")) omenus.RemoveEx("PLANLOTE");
                if (omenuitem.SubMenus.Exists("REMOVE")) omenus.RemoveEx("REMOVE");
                if (omenuitem.SubMenus.Exists("REMOVEPLAN")) omenus.RemoveEx("REMOVEPLAN");

                if (oForm.TypeEx.Equals(TYPEEX) && eventInfo.ItemUID.Equals("mtxGrps") && eventInfo.ColUID != "" && eventInfo.Row > 0)
                {
                    if (eventInfo.BeforeAction)
                    {
                        if (!omenuitem.SubMenus.Exists("PLANLOTE"))
                        {
                            oCreationPackage.UniqueID = "PLANLOTE";
                            oCreationPackage.String = "Plan. Tipo de Prato";
                            oCreationPackage.Enabled = true;
                            omenus.AddEx(oCreationPackage);
                        }

                        if (!omenuitem.SubMenus.Exists("SUBPLAN") && !string.IsNullOrEmpty(screenData?.MatrixItemList?[eventInfo.ColUID]?[eventInfo.Row - 1]?.InsumoDes))
                        {
                            oCreationPackage.UniqueID = "SUBPLAN";
                            oCreationPackage.String = "Substituir em Lote";
                            oCreationPackage.Enabled = true;
                            omenus.AddEx(oCreationPackage);
                        }

                        if (!omenuitem.SubMenus.Exists("REMOVE"))
                        {
                            oCreationPackage.UniqueID = "REMOVE";
                            oCreationPackage.String = "Remover Prato";
                            oCreationPackage.Enabled = true;
                            omenus.AddEx(oCreationPackage);
                        }
                    }
                }
                //else
                //{
                //    if (omenuitem.SubMenus.Exists("SUBPLAN")) omenus.RemoveEx("SUBPLAN");
                //    if (omenuitem.SubMenus.Exists("PLANLOTE")) omenus.RemoveEx("PLANLOTE");
                //    if (omenuitem.SubMenus.Exists("REMOVE")) omenus.RemoveEx("REMOVE");
                //}

                if (oForm.TypeEx.Equals(TYPEEX) && eventInfo.FormUID.Equals(FORMUID) && !string.IsNullOrEmpty(((EditText)oForm.Items.Item("edtCode").Specific)?.Value))
                {
                    if (eventInfo.BeforeAction)
                    {
                        if (!omenuitem.SubMenus.Exists("REMOVEPLAN"))
                        {
                            oCreationPackage.UniqueID = "REMOVEPLAN";
                            oCreationPackage.String = "Remover Planejamento";
                            oCreationPackage.Enabled = true;
                            omenus.AddEx(oCreationPackage);
                        }
                    }
                }
                //else
                //{
                //    if (omenuitem.SubMenus.Exists("REMOVEPLAN")) omenus.RemoveEx("REMOVEPLAN");
                //}
            }
            catch (Exception ex)
            {

            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(Type, BoEventTypes.et_CLICK);
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);
            Filters.Add(MenuItem, BoEventTypes.et_DOUBLE_CLICK);
            Filters.Add("CARDPLIT", BoEventTypes.et_GOT_FOCUS);
            Filters.Add("CARDPLIT", BoEventTypes.et_LOST_FOCUS);
            Filters.Add("CARDPLIT", BoEventTypes.et_FORM_CLOSE);
            Filters.Add("CARDPLIT", BoEventTypes.et_FORM_VISIBLE);
            Filters.Add("CARDPLIT", BoEventTypes.et_FORM_DEACTIVATE);
            Filters.Add("672", BoEventTypes.et_GOT_FOCUS);
            Filters.Add("672", BoEventTypes.et_LOST_FOCUS);
            Filters.Add("672", BoEventTypes.et_FORM_CLOSE);
            Filters.Add("672", BoEventTypes.et_FORM_VISIBLE);
            Filters.Add("672", BoEventTypes.et_FORM_DEACTIVATE);
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            //try
            //{
            //    if (BusinessObjectInfo.FormTypeEx.Equals(Type))
            //    {
            //        if ((BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_UPDATE) || BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD)))
            //        {
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Application.SetStatusBarMessage(ex.Message);
            //    ret = false;
            //}

            bubbleEvent = ret;

            //if (DadosPratoInstance != null)
            //    DadosPratoInstance.Application_FormDataEvent(Application, ref BusinessObjectInfo, out bubbleEvent);
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPCARD", MenuItem, "Plan. Cardápio", 2, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList()
        {

            #region Tipo Prato
            //int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            //string strSql = $@"
            //                    SELECT 
            //                         c.{"LineId".Aspas()}
            //                        ,c.{"U_CVA_TIPO_PRATO_DES".Aspas()}
            //                    FROM {"@CVA_LIN_MCARDAPIO".Aspas()} as c
            //                    WHERE c.{ "Code".Aspas()} = RTRIM(LTRIM($[$edtMdlS.0])) ;
            //                ";

            //FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Prato Planejamento", idCategoria, Type, MatrixItens, "it_CTpP");
            //#endregion

            //#region Busca insumo
            ///*
            // SELECT 
            //    *
            //FROM OITM as o
            //WHERE 
            //"ItemCode" NOT IN(SELECT 
            //                lb."U_CVA_ITEMCODE" 
            //            FROM "@CVA_BLOQUEN" as b 
            //                INNER JOIN "@CVA_LIN_BLOQUEN" AS lb on
            //                    b."Code" = lb."Code"
            //            WHERE b."U_CVA_ID_CONTRATO" = 1
            //)
            //AND "U_CVA_Planejar" = 'Y'
            //AND (SELECT "U_CVA_Proteina" FROM "@CVA_TIPOPRATO" WHERE "Code" = 1) = "U_CVA_Proteina"
            //AND ("U_CVA_Proteina" = 'N' OR 
            //        ("U_CVA_Proteina" = 'Y' 
            //            AND	EXISTS(
            //                SELECT * FROM "@CVA_TABGRAMATURA" as t 
            //                WHERE t."U_CVA_GRAMATURA" = o."U_CVA_GRAMATURA" 
            //                AND t."U_CVA_TIPO_PROTEINA" = o."U_CVA_ID_TIPO_PROT"
            //            )
            //        )
            //    );
            // */

            ////strSql = $@"
            ////            SELECT 
            ////             o.{"ItemCode".Aspas()},
            ////                o.{"ItemName".Aspas()}
            ////            FROM OITM as o
            ////            WHERE {"ItemCode".Aspas()} 
            ////                NOT IN(SELECT lb.{"U_CVA_ITEMCODE".Aspas()}
            ////                        FROM {"@CVA_BLOQUEN".Aspas()} as b
            ////                            INNER JOIN {"@CVA_LIN_BLOQUEN".Aspas()} AS lb on
            ////                                b.{"Code".Aspas()} = lb.{"Code".Aspas()}
            ////                        WHERE b.{"U_CVA_ID_CONTRATO".Aspas()} = RTRIM(LTRIM($[$edtContrS.0]))
            ////            )
            ////            AND {"U_CVA_Planejar".Aspas()} = 'Y'
            ////            AND(SELECT {"U_CVA_PROTEINA".Aspas()} FROM {"@CVA_TIPOPRATO".Aspas()} WHERE {"Code".Aspas()} = RTRIM(LTRIM($[$mtxGrps.it_CLM.0]))) = {"U_CVA_Proteina".Aspas()}
            ////            AND({ "U_CVA_Proteina".Aspas()} = 'N' OR
            ////                    ({ "U_CVA_Proteina".Aspas()} = 'Y'
            ////                        AND EXISTS(
            ////                            SELECT * FROM { "@CVA_TABGRAMATURA".Aspas()} as t
            ////                            WHERE t.{ "U_CVA_GRAMATURA".Aspas()} = o.{ "U_CVA_GRAMATURA".Aspas()}
            ////                            AND t.{ "U_CVA_TIPO_PROTEINA".Aspas()} = o.{ "U_CVA_ID_TIPO_PROT".Aspas()}
            ////                        )
            ////                    )
            ////                );
            ////";

            ////FormatedSearch.CreateFormattedSearches(strSql, "Busca Insumo", idCategoria, Type, MatrixItens, "l_edtIn");
            //#endregion

            //#region Busca Contrato
            //int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            //var strSql = $@"
            //            SELECT 
            //             o.{"Number".Aspas()},
            //                o.{"BpCode".Aspas()},
            //                o.{"BpName".Aspas()},
            //                o.{"StartDate".Aspas()},
            //                o.{"EndDate".Aspas()}
            //            FROM OOAT as o
            //            WHERE {"BpCode".Aspas()} = RTRIM(LTRIM($[$edtCliS.0]))
            //;";

            //FormatedSearch.CreateFormattedSearches(strSql, "Buscar Contrato Plan.", idCategoria, Type, "edtContrS", "-1");
            //#endregion

            //#region Busca Cliente
            //strSql = $@"
            //            SELECT 
            //             o.{"CardCode".Aspas()},
            //                o.{"CardName".Aspas()}
            //            FROM OCRD as o
            //;";

            //FormatedSearch.CreateFormattedSearches(strSql, "Buscar Cliente Plan.", idCategoria, Type, "edtCliS", "-1");
            //#endregion

            //#region Busca Grupo Serviço
            //strSql = $@"
            //    SELECT distinct
            //      GRP.{"Code".Aspas()}
            //        ,GRP.{"U_CVA_DESCRICAO".Aspas()}
            //    FROM {"@CVA_GRPSERVICOS".Aspas()} AS GRP
            //    INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} GLIN ON
            //        GRP.{ "Code".Aspas()} = GLIN.{ "Code".Aspas()}
            //    INNER JOIN { "@CVA_MCARDAPIO".Aspas()} AS MODEL ON
            //        GLIN.{ "U_CVA_ID_SERVICO".Aspas()} = MODEL.{ "U_CVA_ID_SERVICO".Aspas()}
            //    WHERE MODEL.{"U_CVA_ID_CONTRATO".Aspas()} = RTRIM(LTRIM($[$edtContrS.0]))
            //; ";

            //FormatedSearch.CreateFormattedSearches(strSql, "Buscar Grupo Serviço Plan.", idCategoria, Type, "edtGrpS", "-1");
            //#endregion

            //#region Busca Serviço

            //strSql = $@"
            //    SELECT distinct
            //     GLIN.{"U_CVA_ID_SERVICO".Aspas()} as {"Code".Aspas()}
            //    ,GLIN.{"U_CVA_D_SERVICO".Aspas()} as {"Name".Aspas()}
            //    FROM {"@CVA_GRPSERVICOS".Aspas()} AS GRP
            //    INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} GLIN ON
            //        GRP.{ "Code".Aspas()} = GLIN.{ "Code".Aspas()}
            //    INNER JOIN { "@CVA_MCARDAPIO".Aspas()} AS MODEL ON
            //        GLIN.{ "U_CVA_ID_SERVICO".Aspas()} = MODEL.{ "U_CVA_ID_SERVICO".Aspas()}
            //    WHERE GRP.{"Code".Aspas()} = RTRIM(LTRIM($[$edtGrpS.0]))
            //; ";

            //FormatedSearch.CreateFormattedSearches(strSql, "Buscar Serviço Plan.", idCategoria, Type, "edtS", "-1");
            #endregion
        }

        private void PreencherCamposReport()
        {
            var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
            var planId = ((IEditText)oForm.Items.Item("edtCode").Specific).Value.ToString();

            Thread.Sleep(200);
            ((IEditText)oForm.Items.Item("1000003").Specific).Value = planId;
            oForm.Items.Item("1").Click();
        }
    }

    public class ReferenceDate
    {
        public List<Dictionary<string, double>> ValorPorDiaSemana { get; set; }
        public double CustoPadrao { get; set; }

        public ReferenceDate()
        {
            Clear();
        }

        public void Add(double qtdDom, double qtdSeg, double qtdTer, double qtdQua, double qtdQui, double qtdSex, double qtdSab, string turno)
        {
            var lstDays = new List<double> { qtdDom, qtdSeg, qtdTer, qtdQua, qtdQui, qtdSex, qtdSab };
            for (int i = 0; i < 7; i++)
            {
                if (!ValorPorDiaSemana[i].ContainsKey(turno))
                    ValorPorDiaSemana[i].Add(turno, lstDays[i]);
                else
                    ValorPorDiaSemana[i][turno] = lstDays[i];
            }
        }

        public void Clear()
        {
            ValorPorDiaSemana = new List<Dictionary<string, double>>{
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
                new Dictionary<string, double>(),
            };
        }

        public double Sum(int weekDay)
        {
            return ValorPorDiaSemana[weekDay].Sum(x => x.Value);
        }
    }

    public class ScreenData
    {
        public string Code { get; set; }
        public string IdCliente { get; set; }
        public string ClienteDes { get; set; }
        public string IdContrato { get; set; }
        public string IDModeloCardapio { get; set; }
        public string DesModeloCardapio { get; set; }
        public string Vigencia { get; set; }
        public string DataRef { get; set; }
        public string IdGrpServico { get; set; }
        public string DesGrpServico { get; set; }
        public string IdServico { get; set; }
        public string DesServico { get; set; }
        public ReferenceDate ReferenceDate { get; set; }

        private DateTime _refDate;
        public DateTime RefDate
        {
            get
            {
                return DateTime.ParseExact(DataRef, "yyyyMMdd", null);
            }
        }

        private int _diaSelecionado = 0;
        public int DiaSelecionado
        {
            set
            {
                _diaSelecionado = value;

                AtualizaTotais();

                DayChanged?.Invoke(_diaSelecionado);
            }
        }

        //feito para atualizar a tela
        public delegate void DayChangedHandler(int toDay);

        // Declare the event.
        public event DayChangedHandler DayChanged;

        public Dictionary<string, List<LineItemData>> MatrixItemList = new Dictionary<string, List<LineItemData>>();
        public Dictionary<int, List<LineItemData>> DayItens = new Dictionary<int, List<LineItemData>>();

        internal void ValidateData()
        {
            var mess = "";

            if (string.IsNullOrEmpty(DataRef))
                mess = "Data Referência Inválida, escolha uma Data de Referência válida e tente novamente.";

            if (string.IsNullOrEmpty(IdGrpServico))
                mess = "Grupo de Serviço Inválido, escolha um Grupo de Serviço válido e tente novamente.";

            if (string.IsNullOrEmpty(IdServico))
                mess = "Serviço Inválido, escolha um Grupo de Serviço válido e tente novamente.";

            if (string.IsNullOrEmpty(IDModeloCardapio))
                mess = "Modelo Inválido, escolha um cliente com contrato e modelo válido e tente novamente.";

            if (string.IsNullOrEmpty(IdContrato))
                mess = "Contrato Inválido, escolha um contrato e tente novamente.";

            if (string.IsNullOrEmpty(IdCliente))
                mess = "Cliente Inválido, escolha um cliente e tente novamente.";

            if (!string.IsNullOrEmpty(mess))
                throw new Exception(mess);
        }

        public void AtualizaTotais()
        {
            if (!DayItens.Any())
                foreach (var row in MatrixItemList)
                {
                    for (int i = 0; i < row.Value.Count; i++)
                    {
                        if (!DayItens.ContainsKey(i)) DayItens.Add(i, new List<LineItemData>());
                        DayItens[i].Add(row.Value[i]);
                    }
                }
        }

        public void AtualizaPadrao(ReferenceDate val)
        {
            ReferenceDate = val;

            foreach (var row in MatrixItemList)
            {
                foreach (var cell in row.Value)
                {
                    var diaSemana = val.ValorPorDiaSemana[PlanejamentoCardapioForm.diasSemana.IndexOf(cell.DiaSemana)];

                    if (diaSemana.Count > 0)
                    {
                        cell.CustoPadrao = val.CustoPadrao.ToString();
                        cell.QTD_ORI = diaSemana.Sum(x => x.Value).ToString();
                        cell.QtdTurnos.AddRange(diaSemana.Select(x => new QtdTurnoModel(x.Key, x.Value, 0, 0)));
                    }
                }
            }
        }

        public void AlterarDadosETela(Form oForm, string colUID, int row, string itemCode, string edtPerc, string itemName, string edtPrice)
        {
            var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);

            MatrixItemList[colUID][row - 1].Insumo = itemCode;
            MatrixItemList[colUID][row - 1].Percent = edtPerc;
            MatrixItemList[colUID][row - 1].InsumoDes = itemName;
            MatrixItemList[colUID][row - 1].CustoOriginal = DIHelper.Format_StringToDouble(edtPrice.Replace(",", "").Replace(".", ",")).ToString();

            MatrixItemList[colUID][row - 1].AtualizarQuantidades();

            if (edtPerc == "0" || edtPerc == "")
                ((IEditText)mtx.Columns.Item(colUID).Cells.Item(row).Specific).Value = "";
            else
                ((IEditText)mtx.Columns.Item(colUID).Cells.Item(row).Specific).Value = $@"{edtPerc}% {itemName}";

            mtx.AutoResizeColumns();
        }

        internal void AlterarInsumo(DateTime de, DateTime ate, List<string> codServicos, string insumoDe, string insumoPara, string insumoParaDes)
        {
            if (codServicos.Contains(IdServico))
            {
                foreach (var col in MatrixItemList)
                    for (int i = 0; i < col.Value.Count; i++)
                    {
                        var cell = col.Value[i];
                        var dataLocal = RefDate.AddDays(-(RefDate.Day - 1)).AddDays(i);

                        if (dataLocal >= de && dataLocal <= ate)
                            if (cell.Insumo == insumoDe) cell.AlterarInsumo(insumoPara, insumoParaDes, cell.Percent);
                    }
            }
        }

        internal bool HasData() => MatrixItemList.Any(x => x.Value.Any(a => !string.IsNullOrEmpty(a.Insumo)));
    }

    public class LineItemData
    {
        public string ModeloLinId { get; set; }
        public string TipoPrato { get; set; }
        public List<QtdTurnoModel> QtdTurnos { get; set; } = new List<QtdTurnoModel>();
        //public string IdQtdTurno { get; set; }
        public string DesTipoPrato { get; set; }
        public string Insumo { get; set; }
        public string InsumoDes { get; set; }
        public string Percent { get; set; }

        public bool EhDiaValido
        {
            get
            {
                return QtdTurnos.Any(x => x.QTD > 0);
            }
        }

        /// <summary>
        /// quantidade proporcional "QTD" / "Porcentagem"
        /// </summary>
        //public string QTD { get; set; }

        /// <summary>
        /// quantidade original "100%"
        /// </summary>
        public string QTD_ORI { get; set; }

        /// <summary>
        /// custo do item proporcional "CustoOriginal" / "Porcentagem"
        /// </summary>
        public string CustoItem { get; set; }

        /// <summary>
        /// custo original do item
        /// </summary>
        public string CustoOriginal { get; set; }

        /// <summary>
        /// custo padrão do dia
        /// </summary>
        public string CustoPadrao { get; set; }

        public string Total { get; set; }
        public string DiaSemana { get; set; }


        public double d_Percent
        {
            get
            {
                return Helpers.DIHelper.Format_StringToDouble(Percent);
            }
        }

        public double d_QTDORI
        {
            get
            {
                return QtdTurnos?.Sum(x => x.QTD) ?? 0;
            }
        }

        //public double d_QTD
        //{
        //    get
        //    {
        //        return Helpers.DIHelper.Format_StringToDouble(QTD);
        //    }
        //}

        public double d_CustoItem
        {
            get
            {
                return Helpers.DIHelper.Format_StringToDouble(CustoItem);
            }
        }

        public double d_CustoOriginal
        {
            get
            {
                return Helpers.DIHelper.Format_StringToDouble(CustoOriginal);
            }
        }

        public double d_CustoPadrao
        {
            get
            {
                return Helpers.DIHelper.Format_StringToDouble(CustoPadrao);
            }
        }

        public double d_Total
        {
            get
            {
                return Helpers.DIHelper.Format_StringToDouble(Total);
            }
        }

        public void AtualizarQuantidades()
        {
            var percentCem = d_Percent / 100;
            var qtd = d_QTDORI * percentCem;

            //QTD = qtd.ToString();
            CustoItem = (d_CustoOriginal).ToString();

            Total = (qtd * d_CustoOriginal).ToString();
        }

        internal void AlterarInsumo(string insumo, string insumoDes, string percent)
        {
            Insumo = insumo;
            InsumoDes = insumoDes;
            Percent = percent;
        }
    }

    public class ItemResults
    {
        public string ItemName { get; set; }
        public string DiaSemana { get; set; }
        public string ItemPercent { get; set; }
        public string ItemQtd { get; set; }
        public double IntItemQtd
        {
            get
            {
                double parseQtd = 0;
                double.TryParse(ItemQtd, out parseQtd);
                return parseQtd;
            }
        }
        public double IntItemCustoMedio
        {
            get
            {
                double parsePrice = 0;
                double.TryParse(ItemCustoMedio, out parsePrice);
                return parsePrice;
            }
        }
        public string ItemCustoMedio { get; set; }
        public string ItemTotal
        {
            get
            {
                var ret = 0d;
                var parseQtd = IntItemQtd;
                double parsePrice = IntItemCustoMedio;

                if (parseQtd > 0 && parsePrice > 0)
                    ret = parsePrice * parseQtd;

                return ret.ToString();
            }
        }
    }

    public class MenuRemoveControl
    {
        public int Row { get; set; }
        public string ColUID { get; set; }

        internal void Clear()
        {
            Row = 0;
            ColUID = "";
        }
    }
}