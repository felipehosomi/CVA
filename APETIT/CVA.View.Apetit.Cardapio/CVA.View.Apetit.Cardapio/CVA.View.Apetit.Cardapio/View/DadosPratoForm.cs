using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.Apetit.Cardapio.View
{
    public class DadosPratoForm : BaseForm
    {
        public string ColUID;
        public string IdContrato;
        public string TipoPrato;
        public string IdCliente;
        public string IdGrpServico;
        public DateTime DtRef;
        public int Row;

        private DadosPratoQtdTurnoForm DadosPratoQtdTurnoInstance = null;
        private ScreenData ScreenData;
        private LineItemData LineItemData;

        public DadosPratoForm(string colUID, int row, string idContrato, string idCliente, string idGrpServico, DateTime dtRef, ScreenData screenData)
        {
            ColUID = colUID;
            Row = row;
            IdContrato = idContrato;
            IdCliente = idCliente;
            IdGrpServico = idGrpServico;
            DtRef = dtRef;
            ScreenData = screenData;

            //MatrixItemList vem com COLUID menor pois a coluna não existe
            LineItemData = ScreenData.MatrixItemList[ColUID][Row - 1];

            TipoPrato = LineItemData.TipoPrato;
            //MatrixItens = "mtxGrps";
            Type = "CARDPLIT";
            //TableName = "CVA_PLANEJAMENTO";
            //ChildName = "CVA_LN_PLANEJAMENTO";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            //IdToEvaluateGridEmpty = "it_CTpP";
        }

        public override void CreateUserFields() { }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            Filters.Add(f.TypeEx, BoEventTypes.et_CLICK);
            Filters.Add(f.TypeEx, BoEventTypes.et_DOUBLE_CLICK);
            Filters.Add(f.TypeEx, BoEventTypes.et_KEY_DOWN);
            Filters.Add(f.TypeEx, BoEventTypes.et_RIGHT_CLICK);

            ((IEditText)f.Items.Item("edtRef").Specific).Value = DtRef.ToString("yyyyMMdd");
            //((EditText)f.Items.Item("edtSearch").Specific).KeyDownAfter += EdtSearch_KeyDownAfter;
            //((Button)f.Items.Item("bt_search").Specific).ClickAfter += Bt_search_ClickAfter;
            //((Button)f.Items.Item("bt_qtdT").Specific).ClickAfter += bt_qtdT_ClickAfter;
            //((Button)f.Items.Item("bt_rec").Specific).ClickAfter += Bt_rec_ClickAfter;

            //var mtx = ((Matrix)f.Items.Item("mtxGrps").Specific);
            //mtx.SelectionMode = BoMatrixSelect.ms_Auto;
            //mtx.DoubleClickAfter += Mtx_DoubleClickAfter;

            CarregaDadosGrid(f);

            //oForm.Freeze(false);
        }

        private void bt_qtdT_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            Qtd_Click();
        }

        private void Qtd_Click()
        {
            var f = B1Connection.Instance.Application.Forms.ActiveForm;
            f.Visible = false;

            DadosPratoQtdTurnoInstance = new DadosPratoQtdTurnoForm(ScreenData.MatrixItemList, ColUID, Row - 1);

            Form oForm;
            DadosPratoQtdTurnoInstance.OpenMenu("CARDITTR", DadosPratoQtdTurnoInstance.FilePath, null, out oForm);
            if (oForm != null)
            {
                oForm.Visible = true;
            }
        }

        private void Mtx_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            MtxConfirm();
        }

        private void MtxConfirm()
        {
            var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
            var btnConfirm = oForm.Items.Item("btnConfirm");
            if (btnConfirm.Enabled) btnConfirm.Click();
        }

        private void Bt_rec_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            RecClick();
        }

        private void RecClick()
        {
            var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
            var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);
            var selectedRowIndex = mtx.GetNextSelectedRow();

            if (selectedRowIndex >= 0)
            {
                mtx.GetLineData(selectedRowIndex);

                var itemCode = ((IEditText)mtx.Columns.Item("it_code").Cells.Item(selectedRowIndex).Specific).Value.ToString();

                var f = B1Connection.Instance.Application.Forms.ActiveForm;
                f.Visible = false;

                var secForm = B1Connection.Instance.Application.OpenForm(BoFormObjectEnum.fo_ProductTree, "", itemCode);
            }
        }

        private void Bt_search_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            CarregaDadosGrid(B1Connection.Instance.Application.Forms.ActiveForm);
        }

        private void EdtSearch_KeyDownAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.CharPressed == 13)
                Bt_search_ClickAfter(sboObject, pVal);
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

        public static List<string> diasSemana = new List<string> { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", };

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                #region Tratamento da troca de tela do dadoForm, voltando o foco para a tela
                if (pVal.EventType == BoEventTypes.et_FORM_CLOSE && ((DadosPratoQtdTurnoInstance != null && pVal.FormTypeEx.Equals(DadosPratoQtdTurnoInstance.TYPEEX)) || pVal.FormTypeEx.Equals("672")) && !pVal.Before_Action)
                {
                    DadosPratoQtdTurnoInstance = null;
                    var f = Application.Forms.Item(FORMUID);
                    f.Visible = true;
                }

                if (pVal.EventType == BoEventTypes.et_FORM_DEACTIVATE && DadosPratoQtdTurnoInstance != null && pVal.FormTypeEx.Equals(DadosPratoQtdTurnoInstance.TYPEEX) && !pVal.Before_Action)
                {
                    var dadoForm = Application.Forms.Item(DadosPratoQtdTurnoInstance.FORMUID);
                    if (dadoForm.Visible) dadoForm.Select();
                }

                if (pVal.EventType == BoEventTypes.et_FORM_DEACTIVATE && pVal.FormTypeEx.Equals("672") && !pVal.Before_Action)
                {
                    var dadoForm = Application.Forms.Item("F_66");
                    if (dadoForm.Visible) dadoForm.Select();
                }
                #endregion

                if (pVal.FormTypeEx.Equals(Type) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    if (oForm.TypeEx.Equals(Type))
                    {
                        //evento de keydown do enter
                        if (pVal.EventType == BoEventTypes.et_KEY_DOWN && pVal.CharPressed == 13)
                            CarregaDadosGrid(B1Connection.Instance.Application.Forms.ActiveForm);

                        //botão pesquisar
                        if (pVal.EventType == BoEventTypes.et_CLICK && pVal.ItemUID == "bt_search")
                            CarregaDadosGrid(B1Connection.Instance.Application.Forms.ActiveForm);

                        //botão qtd
                        if (pVal.EventType == BoEventTypes.et_CLICK && pVal.ItemUID == "bt_qtdT")
                            Qtd_Click();

                        //botão Rec
                        if (pVal.EventType == BoEventTypes.et_CLICK && pVal.ItemUID == "bt_rec")
                            RecClick();

                        var mtx = ((Matrix)oForm.Items.Item("mtxGrps").Specific);

                        if (pVal.ItemUID.Equals("mtxGrps") && (pVal.ColUID.Equals("it_name") || pVal.ColUID.Equals("edtPrice")))
                        {
                            var row = pVal.Row > 0 ? pVal.Row : 1;

                            if (mtx.RowCount < row) { bubbleEvent = false; return; }

                            mtx.SelectRow(row, true, false);

                            if (pVal.EventType == BoEventTypes.et_DOUBLE_CLICK)
                            {
                                MtxConfirm();
                                bubbleEvent = true;
                                return;
                            }
                        }

                        var selectedRowIndex = mtx.GetNextSelectedRow();
                        var btnConfirm = oForm.Items.Item("btnConfirm");
                        var bt_rec = oForm.Items.Item("bt_rec");

                        if (selectedRowIndex >= 0)
                        {
                            mtx.GetLineData(selectedRowIndex);

                            var itemCode = ((IEditText)mtx.Columns.Item("it_code").Cells.Item(selectedRowIndex).Specific).Value.ToString();
                            var edtPerc = ((IEditText)oForm.Items.Item("edtPerc").Specific).Value.ToString();

                            btnConfirm.Enabled = !string.IsNullOrEmpty(itemCode) && !string.IsNullOrEmpty(edtPerc);
                        }
                        else btnConfirm.Enabled = false;

                        bt_rec.Enabled = btnConfirm.Enabled;
                    }
                }
            }
            catch (Exception ex)
            {
                //Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            //if (DadosPratoQtdTurnoInstance != null)
            //    DadosPratoQtdTurnoInstance.Application_ItemEvent(FormUID, ref pVal, out bubbleEvent);

            bubbleEvent = ret;
        }

        private void CarregaDadosGrid(Form oForm)
        {
            var edtSearch = ((IEditText)oForm.Items.Item("edtSearch").Specific).Value;
            var edtRef = ((IEditText)oForm.Items.Item("edtRef").Specific).Value;
            var mtx = ((IMatrix)oForm.Items.Item("mtxGrps").Specific);
            if (string.IsNullOrEmpty(edtRef)) return;

            var dtRef = DateTime.ParseExact(edtRef, "yyyyMMdd", null);

            //usar edtRef
            DataTable dataTable = oForm.DataSources.DataTables.Item("mtxGrps");

            #region query

            dataTable.ExecuteQuery($@"                
                DO BEGIN

	                DECLARE _diaReferencia VARCHAR(12) = '{(int)dtRef.DayOfWeek}';
	                DECLARE _idContrato VARCHAR(12) = '{IdContrato}';
	                DECLARE _grpServico VARCHAR(12) = '{IdGrpServico}';
	                DECLARE _clientId VARCHAR(12) = '{IdCliente}';
                    DECLARE _search VARCHAR(254) = '{edtSearch}';
                    DECLARE _idTipoPrato VARCHAR(254) = '{TipoPrato}';
         
                    /*
                    01.01.01.095.00
                    01.05.03.140.00
                    01.40.06.001.00
                    01.01.07.001.00
                    */

                    SELECT 		   
                        OIT.{"ItemCode".Aspas()} as {"ItemCode".Aspas()}
                       ,{"ItemName".Aspas()} as {"ItemName".Aspas()}
                       ,T.{"AvgPrice".Aspas()} AS {"AvgPrice".Aspas()}          
                    FROM OITM AS OIT
                    INNER JOIN OITT AS IT ON
                        IT.{"Code".Aspas()} = OIT.{"ItemCode".Aspas()}
                    INNER JOIN (SELECT
                            { "ItemCode".Aspas()},
                            IFNULL(TB.{ "AvgPrice".Aspas()},0)  AS { "AvgPrice".Aspas()}
                            FROM (
	                            SELECT
                                    { "ItemCode".Aspas()} AS { "ItemCode".Aspas()},
                                    SUM(OITM.{ "LastPurPrc".Aspas()}) AS { "AvgPrice".Aspas()}
	                            FROM OITM
	                            GROUP BY { "ItemCode".Aspas()}
	
	                            UNION
	
	                            SELECT 
	                                O.{ "Code".Aspas()} AS { "ItemCode".Aspas()},
                                    ROUND(SUM(I1.{ "Quantity".Aspas()} * I1. { "Price".Aspas()}),2) AS  { "AvgPrice".Aspas()}
	                            FROM OITT AS O 
	                            INNER JOIN ITT1 AS I1 ON
		                            I1.{ "Father".Aspas()} = O.{ "Code".Aspas()}
	                            GROUP BY
                                    O.{ "Code".Aspas()}
	
	                            UNION
	
	                            SELECT 
	                                W.{ "ItemCode".Aspas()} as { "ItemCode".Aspas()},
                                    SUM(W.{ "AvgPrice".Aspas()})
                                FROM OCRD AS O
                                    INNER JOIN OBPL AS B ON
                                        O.{ "U_CVA_FILIAL".Aspas()} = B.{ "BPLId".Aspas()}
                                    INNER JOIN OITW AS W ON
                                        B.{ "DflWhs".Aspas()} = W.{ "WhsCode".Aspas()}
                                WHERE 
                                        O.{ "CardCode".Aspas()} = _clientId
                                GROUP BY
                                    W.{ "ItemCode".Aspas()}
                            ) AS TB) AS T ON 
                            T.{ "ItemCode".Aspas()} = OIT.{ "ItemCode".Aspas()}
                        WHERE 
        	                    (LOWER(OIT.{ "ItemName".Aspas()}) LIKE LOWER(CONCAT('%', CONCAT(_search,'%')) )
                            OR	LOWER(OIT.{ "ItemCode".Aspas()}) LIKE LOWER(CONCAT('%', CONCAT(_search,'%')) ))
                            AND OIT.{"ItemCode".Aspas()} NOT IN (
                                SELECT 
	                                L.{"U_CVA_ITEMCODE".Aspas()} 
                                FROM { "@CVA_BLOQUEN".Aspas()} AS B
                                INNER JOIN { "@CVA_LIN_BLOQUEN".Aspas()} AS L ON
                                    B.{ "Code".Aspas()} = L.{ "Code".Aspas()}
                                WHERE B.{ "U_CVA_ID_CONTRATO".Aspas()} = _idContrato
                
                                UNION
                
                                SELECT 
                                    IT.{"Father".Aspas()} as {"U_CVA_ITEMCODE".Aspas()}
                                FROM  { "@CVA_BLOQUEN".Aspas()} AS B
                                INNER JOIN  { "@CVA_LIN_BLOQUEN".Aspas()} AS L ON
                                    B. { "Code".Aspas()} = L.{ "Code".Aspas()}
                                INNER JOIN ITT1 AS IT ON
                	                IT.{ "Code".Aspas()} = L.{ "U_CVA_ITEMCODE".Aspas()}                 
                                WHERE B. { "U_CVA_ID_CONTRATO".Aspas()} = _idContrato
                            )
                        --AND OIT.{"U_CVA_Familia".Aspas()} = (SELECT TOP 1 F.{"Name".Aspas()} FROM {"@CVA_TIPOPRATO".Aspas()} AS T INNER JOIN {"@CVA_FAMILIA".Aspas()} AS F ON T.{"U_CVA_FAMILIA".Aspas()} = F.{"Code".Aspas()}  WHERE T.{"Code".Aspas()} = _idTipoPrato)
                        --AND OIT.{"U_CVA_Subfamilia".Aspas()} = (SELECT TOP 1 F.{ "Name".Aspas()} FROM { "@CVA_TIPOPRATO".Aspas()} AS T INNER JOIN { "@CVA_SUBFAMILA".Aspas()} AS F ON T.{ "U_CVA_SUB_FAMILIA".Aspas()} = F.{ "Code".Aspas()}  WHERE T.{ "Code".Aspas()} = _idTipoPrato)
                    ;
                END
            ;");
            #endregion

            mtx.LoadFromDataSource();
            mtx.AutoResizeColumns();
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent) => bubbleEvent = true;

        public override void SetFilters() { }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent) => bubbleEvent = true;

        public override void SetMenus() { }
    }
}