using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace CVA.Apetit.Addon
{
    [FormAttribute("CVA.Apetit.Addon.FrmConfiguracao", "FrmConfiguracao.b1f")]
    class FrmConfiguracao : UserFormBase
    {
        public FrmConfiguracao()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_0").Specific));
            this.Matrix0.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.Matrix0_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btSalvar").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Matrix Matrix0;

        private void OnCustomInitialize()
        {
            string sql;
            int tot, linha;

            try
            {
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Depósitos"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("cDepPad", this.UIAPIRawForm, "Item_0", false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Parceiros de Negócio"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("cCD", this.UIAPIRawForm, "Item_0", false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Usages de Saída"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("uUsgTran", this.UIAPIRawForm, "Item_0", false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Usages de Entrada"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("uUsgRet", this.UIAPIRawForm, "Item_0", false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Usages de Saída"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("uUsgRem", this.UIAPIRawForm, "Item_0", false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo Listas de Preços"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                Class.FilialService.PreencherComboMatriz("cPrice", this.UIAPIRawForm, "Item_0", false);

                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT 
	T0.""BPLId""
	,T0.""BPLName""
	,IFNULL(T2.""CardName"", '') AS ""CardName2PN""
	,IFNULL(T1.""Code"", 0) AS ""Code""
	,IFNULL(T1.""U_BPLId"", 0) AS ""U_BPLId""
	,IFNULL(T1.""U_CardCodePN"", '') AS ""U_CardCodePN""
	,IFNULL(T1.""U_WhsCode"", '') AS ""U_WhsCode""
	,IFNULL(T1.""U_BPLId_CD"", 2) AS ""U_BPLId_CD""
	,IFNULL(T1.""U_UsgTransf"", 16) AS ""U_UsgTransf""
	,IFNULL(T1.""U_UsgRetorno"", 21) AS ""U_UsgRetorno""
	,IFNULL(T1.""U_UsgRemessa"", 34) AS ""U_UsgRemessa""
	,IFNULL(T1.""U_PrecoUnit"", -1) AS ""U_PrecoUnit""
	,IFNULL(T1.""U_DiasPlanej"", 2) AS ""U_DiasPlanej""
FROM OBPL T0
    LEFT JOIN ""@CVA_CAR_CONFIG"" T1 ON T1.""U_BPLId"" = T0.""BPLId""
	LEFT JOIN OCRD T2 ON T2.""CardCode"" = T1.""U_CardCodePN""
ORDER BY T0.""BPLId""
");
                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    //this.UIAPIRawForm.Freeze(true);
                    this.Matrix0.Clear();
                    tot = oRec.RecordCount;
                    linha = 1;

                    oRec.MoveFirst();
                    for (int i = 0; i < oRec.RecordCount; i++)
                    {
                        Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Preenchendo linha " + linha.ToString() + " de " + tot.ToString() ), SAPbouiCOM.BoMessageTime.bmt_Short, false);

                        this.Matrix0.AddRow();

                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cBPLId").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("BPLId").Value.ToString();
                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cBPLName").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("BPLName").Value.ToString();
                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cPN").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("U_CardCodePN").Value.ToString();

                        string s = oRec.Fields.Item("U_WhsCode").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "cDepPad", s);
                        s = oRec.Fields.Item("U_BPLId_CD").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "cCD", s);
                        s = oRec.Fields.Item("U_UsgTransf").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "uUsgTran", s);
                        s = oRec.Fields.Item("U_UsgRetorno").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "uUsgRet", s);
                         s = oRec.Fields.Item("U_UsgRemessa").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "uUsgRem", s);
                        s = oRec.Fields.Item("U_PrecoUnit").Value.ToString();
                        this.Matrix0.SetCellWithoutValidation(i + 1, "cPrice", s);

                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cNomePN").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("CardName2PN").Value.ToString();
                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cDiasSeg").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("U_DiasPlanej").Value.ToString();
                        ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cCode").Cells.Item(i + 1).Specific).Value = oRec.Fields.Item("Code").Value.ToString();

                        oRec.MoveNext();
                        linha++;
                    }

                    this.Matrix0.Columns.Item("cCode").Visible = false;
                    this.Matrix0.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto;
                    this.Matrix0.SelectRow(1, true, false);

                    //this.UIAPIRawForm.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private void Matrix0_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
            SAPbouiCOM.SBOChooseFromListEventArg ChooseEvents = ((SAPbouiCOM.SBOChooseFromListEventArg)(pVal));
            SAPbouiCOM.EditText oEdit1;
            SAPbouiCOM.EditText oEdit2;

            try
            {
                string coluna = pVal.ColUID;
                string chave = ChooseEvents.SelectedObjects.GetValue(0, 0).ToString();
                string descricao = ChooseEvents.SelectedObjects.GetValue(1, 0).ToString();

                if (coluna == "cPN")
                {
                    oEdit1 = (SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cPN").Cells.Item(pVal.Row).Specific;
                    oEdit2 = (SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cNomePN").Cells.Item(pVal.Row).Specific;

                    try { oEdit1.Value = chave; } catch { }
                    try { oEdit2.Value = descricao; } catch { }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string s;
            int code, linha, tot;

            try
            {
                tot = this.Matrix0.VisualRowCount;
                linha = 1;

                this.GetItem("btSalvar").Enabled = false;
                //Class.Conexao.oCompany.StartTransaction();

                for (int i = 1; i <= this.Matrix0.VisualRowCount; i++)
                {
                    Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Salvando registro " + linha.ToString() + " de " + tot.ToString()), SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    s = ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cCode").Cells.Item(i).Specific).Value;
                    Int32.TryParse(s, out code);

                    SalvarLinhaMatriz(code, i);
                    linha++;
                }

                //Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                this.UIAPIRawForm.Close();
            }
            catch (Exception ex)
            {
                //if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Class.Conexao.sbo_application.MessageBox(ex.Message);
                this.GetItem("btSalvar").Enabled = true;
            }
        }

        private void SalvarLinhaMatriz(int code, int linha)
        {
            string sql, cardCodePN, whsCode, s;
            int bplId, bplIdCD, usgTransf, usgRetorno, usgRemessa, precoUnit, diasPlanej;
            bool insert = code == 0 ? true : false;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                s = ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cBPLId").Cells.Item(linha).Specific).Value;
                Int32.TryParse(s, out bplId);

                cardCodePN = ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cPN").Cells.Item(linha).Specific).Value;

                try { whsCode = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("cDepPad").Cells.Item(linha).Specific).Selected.Value; } catch { whsCode = ""; }

                try { s = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("cCD").Cells.Item(linha).Specific).Selected.Value; } catch { s = ""; }
                Int32.TryParse(s, out bplIdCD);

                try { s = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("uUsgTran").Cells.Item(linha).Specific).Selected.Value; } catch { s = ""; }
                Int32.TryParse(s, out usgTransf);

                try { s = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("uUsgRet").Cells.Item(linha).Specific).Selected.Value; } catch { s = ""; }
                Int32.TryParse(s, out usgRetorno);

                try { s = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("uUsgRem").Cells.Item(linha).Specific).Selected.Value; } catch { s = ""; }
                Int32.TryParse(s, out usgRemessa);

                try { s = ((SAPbouiCOM.ComboBox)this.Matrix0.Columns.Item("cPrice").Cells.Item(linha).Specific).Selected.Value; } catch { s = ""; }
                Int32.TryParse(s, out precoUnit);

                s = ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("cDiasSeg").Cells.Item(linha).Specific).Value;
                Int32.TryParse(s, out diasPlanej);

                if (insert)
                {
                    sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONFIG"" (
""Code"", ""Name"", ""U_BPLId"", ""U_CardCodePN"", ""U_WhsCode"", ""U_BPLId_CD"", ""U_UsgTransf"", ""U_UsgRetorno"", ""U_UsgRemessa"", ""U_PrecoUnit"", ""U_DiasPlanej"" ) VALUES (
{0}     , '{1}'   , {2}        , '{3}'           , '{4}'        , {5}           , {6}            , {7}             , {8}             , {9}            , {10} )",
bplId   , bplId   , bplId      , cardCodePN      , whsCode      , bplIdCD       , usgTransf      , usgRetorno      , usgRemessa      , precoUnit      , diasPlanej);
                }
                else
                {
                    sql = string.Format(@"
UPDATE ""@CVA_CAR_CONFIG"" SET
    ""U_CardCodePN"" = '{0}'
    ,""U_WhsCode"" = '{1}'
    ,""U_BPLId_CD"" = {2}
    ,""U_UsgTransf"" = {3}
    ,""U_UsgRetorno"" = {4}
    ,""U_UsgRemessa"" = {5}
    ,""U_PrecoUnit"" = {6}
    ,""U_DiasPlanej"" = {7}
WHERE ""Code"" = {8}
", cardCodePN, whsCode, bplIdCD, usgTransf, usgRetorno, usgRemessa, precoUnit, diasPlanej, code);
                }

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}
