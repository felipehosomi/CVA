using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbouiCOM;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing.Chart;

namespace CVA.Apetit.Addon
{
    [FormAttribute("CVA.Apetit.Addon.FrmConsolidacoes", "FrmConsolidacao.b1f")]
    class FrmConsolidacao : UserFormBase
    {
        private bool processado;
        private System.Data.DataTable oDT_Planejamento;         // Tabela de Planejamento
        private System.Data.DataTable oDT_LN_Planejamento;      // Tabela de Linhas de Planejamento
        private System.Data.DataTable oDT_OPs;                  // Tabela de OP's
        private System.Data.DataTable oDT_Grid;                 // Tabela de Grid na Tela   
        private List<int> filiais;
        private int filial;
        private DateTime dtIni, dtFim;

        public FrmConsolidacao()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_1").Specific));
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFilial").Specific));
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.ComboBox1 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbReco").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("proc").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("filtrar").Specific));
            this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("btnAgrupa").Specific));
            this.Button4 = ((SAPbouiCOM.Button)(this.GetItem("btnDados").Specific));
            this.Button4.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button4_ClickBefore);
            this.Button5 = ((SAPbouiCOM.Button)(this.GetItem("btnReco").Specific));
            this.Button5.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button5_ClickBefore);
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_7").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_8").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("txtFilial").Specific));
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("prodIni").Specific));
            this.EditText3.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.EditText3_ChooseFromListAfter);
            this.EditText4 = ((SAPbouiCOM.EditText)(this.GetItem("prodFim").Specific));
            this.EditText4.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.EditText4_ChooseFromListAfter);
            this.EditText5 = ((SAPbouiCOM.EditText)(this.GetItem("txtProdIni").Specific));
            this.EditText6 = ((SAPbouiCOM.EditText)(this.GetItem("txtProdFim").Specific));
            this.EditText7 = ((SAPbouiCOM.EditText)(this.GetItem("txtVigen").Specific));
            this.Button6 = ((SAPbouiCOM.Button)(this.GetItem("btnGrafico").Specific));
            this.Button6.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button6_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.ComboBox ComboBox1;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.EditText EditText3;
        private SAPbouiCOM.EditText EditText4;
        private SAPbouiCOM.EditText EditText5;
        private SAPbouiCOM.EditText EditText6;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.Button Button4;
        private SAPbouiCOM.Button Button5;

        //==================================================================================================================================//
        private void OnCustomInitialize()
        //==================================================================================================================================//
        {
            this.GetItem("Matrix").Visible = false;

            Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Carregando formulário"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
            Class.FilialService.PreencherCombo("cbFilial", UIAPIRawForm, false);

            //this.GetItem("btnGrafico").Visible = false;
            //this.GetItem("Item_11").Visible = false;

            this.Grid0.DataTable = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            this.EditText0.DataBind.SetBound(true, "", "UD_0");
            this.EditText1.DataBind.SetBound(true, "", "UD_1");
            //Matrix0.Columns.Item("cQuant").DataBind.SetBound(true, "", "UD_2");
            //Matrix0.Columns.Item("cQuant").DataBind.SetBound(true, "@CVA_CAR_CONF1", "U_PrecoUnit");

            processado = false;
        }

        //==================================================================================================================================//
        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        //==================================================================================================================================//
        {
            string sFilial;

            try
            {
                sFilial = ComboBox0.Selected.Description;
                EditText2.Value = sFilial;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        //==================================================================================================================================//
        private void Button2_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;

            string msg = "";

            try
            {
                msg = ValidarFiltros();

                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                oDT_Planejamento = Class.Geral.TabelaPlanejamento(filial, dtIni, dtFim);
                oDT_LN_Planejamento = Class.Geral.TabelaLnPlanejamento(filial, dtIni, dtFim);
                oDT_OPs = Class.Geral.TabelaOPs(filial, dtIni, dtFim);
                oDT_Grid = Class.Conexao.ExecuteSqlDataTable(Class.Geral.TabelaGridTela(filial, dtIni, dtFim));
                this.Grid0.DataTable.ExecuteQuery(Class.Geral.TabelaGridTela(filial, dtIni, dtFim));
            }
            catch (Exception ex)
            {
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }

        //==================================================================================================================================//
        private string ValidarFiltros()
        //==================================================================================================================================//
        {
            string dataDe, dataAte, msg = "", s;
            int n;

            try
            {
                try { s = this.ComboBox0.Selected.Value.Trim(); } catch { s = ""; }
                Int32.TryParse(s, out n);

                dataDe = this.EditText0.Value.Trim();
                dataAte = this.EditText1.Value.Trim();

                if (n == 0)
                    msg += "Nenhuma filial selecionada" + Environment.NewLine;

                if ((string.IsNullOrEmpty(dataDe)) || (string.IsNullOrEmpty(dataAte)))
                    msg += "Data inicial ou final não preenchida" + Environment.NewLine;

                if (string.IsNullOrEmpty(msg))
                    if ((Convert.ToDateTime(dataDe.Substring(0, 4) + "-" + dataDe.Substring(4, 2) + "-" + dataDe.Substring(6, 2))) > Convert.ToDateTime(dataAte.Substring(0, 4) + "-" + dataAte.Substring(4, 2) + "-" + dataAte.Substring(6, 2)))
                        msg += "Data inicial posterior à data final" + Environment.NewLine;

                if (string.IsNullOrEmpty(msg))
                {
                    filial = n;
                    dtIni = Convert.ToDateTime(dataDe.Substring(0, 4) + "-" + dataDe.Substring(4, 2) + "-" + dataDe.Substring(6, 2));
                    dtFim = Convert.ToDateTime(dataAte.Substring(0, 4) + "-" + dataAte.Substring(4, 2) + "-" + dataAte.Substring(6, 2));
                }
                else
                {
                    filial = 0;
                }

            }
            catch (Exception ex)
            {
                msg += ex.Message + Environment.NewLine;
            }

            return msg;
        }

        //==================================================================================================================================//
        private void Button0_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;

            if (this.Grid0.DataTable.Rows.Count == 0)
                Class.Conexao.sbo_application.MessageBox("Nenhuma linha para processamento");
            else
            {
                if (Class.Conexao.sbo_application.MessageBox($"Confirma processamento das {this.Grid0.DataTable.Rows.Count} previsões listadas?", 2, "Sim", "Não") == 1)
                {
                    ProcessaGrid();
                }
                else
                    Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Processamento não confirmado"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
        }

        //==================================================================================================================================//
        private void ProcessaGrid()
        //==================================================================================================================================//
        {
            int iErro = 0, lote;
            string sql, dataDe, dataAte, now, itemCodeIni, itemCodeFim, msg = "";
            double quant;

            try
            {
                msg = ValidarConsolidacao();

                if (string.IsNullOrEmpty(msg))
                {
                    Class.Conexao.oCompany.StartTransaction();

                    lote = Class.Geral.RetornaLote(filial, dtIni, dtFim, "", "");

                    Class.Geral.SubirPlanejamento(lote, oDT_Planejamento);
                    Class.Geral.SubirLnPlanejamento(lote, oDT_LN_Planejamento);
                    Class.Geral.SubirOPPlanejamento(lote, oDT_OPs);
                    Class.Geral.Subir_LN_OPPlanejamento(lote);



                    /*

                    try { xfilial = this.ComboBox0.Selected.Value.Trim(); } catch { }

                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //SelectedRows oSel = this.Grid0.Rows.SelectedRows;


                if (string.IsNullOrEmpty(msg))
                {
                    Class.Conexao.oCompany.StartTransaction();

                    dataDe = this.EditText0.Value.Trim();
                    dataAte = this.EditText1.Value.Trim();
                    now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    itemCodeIni = this.EditText3.Value.Trim();
                    itemCodeFim = this.EditText4.Value.Trim();

                    sql = $@"select ifnull(max(""Code""), 0) + 1 AS ""Lote"" FROM ""@CVA_CAR_LOTE""";
                    oRec.DoQuery(sql);
                    lote = Convert.ToInt32(oRec.Fields.Item("Lote").Value);

                    sql = string.Format(@"
                            INSERT INTO ""@CVA_CAR_LOTE"" (
                                ""Code"", ""Name"", ""U_Lote"", ""U_ID_Filial"", ""U_CreateDate"", ""U_DataDe"", ""U_DataAte"", ""U_ProdutoIni"", ""U_ProdutoFim"") VALUES (
                                '{0}'   , '{1}'   , {2}       , '{3}'          , '{4}'           , '{5}'       , '{6}'        , '{7}'           , '{8}')",
                                 lote, lote, lote, xfilial, now, dataDe, dataAte, itemCodeIni, itemCodeFim);

                    oRec.DoQuery(sql);

                    sql = $@"select ifnull(max(""Code""), 0) + 1 AS ""Max"" FROM ""@CVA_CAR_CONSOL""";
                    oRec.DoQuery(sql);
                    int maxCode = Convert.ToInt32(oRec.Fields.Item("Max").Value);

                    for (int i = 0; i < this.Grid0.DataTable.Rows.Count; i++)
                    {
                        DateTime data = Convert.ToDateTime(Grid0.DataTable.GetValue(0, i).ToString());
                        string sData = data.ToString("yyyy-MM-dd");
                        data = Convert.ToDateTime(Grid0.DataTable.GetValue(2, i).ToString());
                        string sDataOrig = data.ToString("yyyy-MM-dd");
                        string itemCode = Grid0.DataTable.GetValue(6, i).ToString();
                        string origem = Grid0.DataTable.GetValue(9, i).ToString();
                        string idTurno = "";
                        string idServico = "";

                        string sQuant = Grid0.DataTable.GetValue(8, i).ToString().Replace(',', '.');
                        quant = Convert.ToDouble(sQuant, CultureInfo.InvariantCulture);

                        string cancelado = "";

                        if (quant > 0)
                        {
                            sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"", ""U_Data"", ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          , '{3}'     , '{4}'         , {5}        , '{6}'          , {7}         , '{8}'    , '{9}'         , '{10}'          , '{11}'      , {12}      , '{13}'       , '{14}'         , '{15}')",
maxCode, maxCode, xfilial, sData, itemCode, sQuant, cancelado, 0, "", sDataOrig, now, origem, lote, idTurno, idServico, "PC");

                            oRec.DoQuery(sql);
                            maxCode++;

                            if (origem == "CD")
                            {
                                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"", ""U_Data"", ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          , '{3}'     , '{4}'         , {5}        , '{6}'          , {7}         , '{8}'    , '{9}'         , '{10}'          , '{11}'      , {12}      , '{13}'       , '{14}'         , '{15}')",
maxCode, maxCode, xfilial, sData, itemCode, sQuant, cancelado, 0, "", sDataOrig, now, origem, lote, idTurno, idServico, "PV");

                                oRec.DoQuery(sql);
                                maxCode++;
                            }
                        }
                    }
                    */


                    /*
                    foreach (System.Data.DataRow row in oDT_OP_Aberto.Rows)
                    {
                        //oCon.CondVal = row["ItmsGrpCod"].ToString();
                        DateTime data = Convert.ToDateTime(row["Data"].ToString());
                        string sData = data.ToString("yyyy-MM-dd");
                        data = Convert.ToDateTime(row["DataPlanej"].ToString());
                        string sDataOrig = data.ToString("yyyy-MM-dd");
                        string itemCode = row["ItemCode"].ToString();
                        string origem = "FI";
                        string idTurno = row["IDTurno"].ToString();
                        string idServico = row["IDServico"].ToString();

                        string sQuant = row["Quant"].ToString().Replace(',', '.');
                        quant = Convert.ToDouble(sQuant, CultureInfo.InvariantCulture);

                        string cancelado = "";

                        if (quant > 0)
                        {
                            sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"",  ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          ,  '{3}'         , {4}        , '{5}'          , {6}         , '{7}'    , '{8}'         , '{9}'           , '{10}'      , {11}      , '{12}'       , '{13}'         , '{14}')",
maxCode , maxCode , xfilial         ,  itemCode      , sQuant     , cancelado      , 0           , ""       , sDataOrig     , now             , origem      , lote      , idTurno      , idServico      , "OP");

                            oRec.DoQuery(sql);
                            maxCode++;
                        }
                    }
                    */



                    //SetarPlanejamentoLido(lote);

                    Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                    //InserePrevisao(xfilial, lote);
                    //InsereOP(xfilial, lote);
                    //InserePedVenda(xfilial, lote);


                    //this.GetItem("proc").Enabled = false;
                    //processado = true;

                    //AtualizarGrid(lote);
                    Class.Conexao.sbo_application.MessageBox("Processamento concluído");

                




                }
                else
                    throw new Exception(msg);

                

            }
            catch (Exception ex)
            {
                if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }

        //==================================================================================================================================//
        private string ValidarConsolidacao()
        //==================================================================================================================================//
        {
            string msg = "", warehouse, sql, bplName = "", cardCode, s, itemCode;
            int cont;

            try
            {
                sql = string.Format(@"SELECT COUNT(1) FROM OBPL WHERE ""BPLId"" = {0} ", filial);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                if (cont == 0)
                    msg = "Filial de código (BPLId) " + filial + " não encontrada no SAP";
                else
                {
                    sql = string.Format(@"SELECT ""BPLName"" FROM OBPL WHERE ""BPLId"" = {0} ", filial);
                    bplName = Class.Conexao.ExecuteSqlScalar(sql).ToString();

                    sql = string.Format(@"SELECT IFNULL(""DflWhs"", '') FROM OBPL  WHERE ""BPLId"" = {0} ", filial);
                    warehouse = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                    if (string.IsNullOrEmpty(warehouse))
                        msg = "Depósito padrão não configurado para a filial " + bplName;
                }

                if (string.IsNullOrEmpty(msg))
                {
                    sql = string.Format(@"
SELECT COUNT(1)
FROM ""@CVA_CAR_CONFIG"" 
WHERE ""U_BPLId"" = {0}
    AND IFNULL(""U_CardCodePN"", '') <> ''
", filial);
                    cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                    if (cont == 0)
                        msg = "Filial " + bplName + " ainda não configurada em 'Configuração de Parâmetros' do addon";
                }

                if (string.IsNullOrEmpty(msg))
                {
                    for (int i = 0; i < this.Grid0.DataTable.Rows.Count; i++)
                    {
                        itemCode = Grid0.DataTable.GetValue(6, i).ToString();
                        s = Grid0.DataTable.GetValue(3, i).ToString();
                        if (string.IsNullOrEmpty(s))
                            msg = "Sem data de entrega calculada porque a categoria do item " + itemCode + " não está definida.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msg;
        }











        // |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||










































        //==================================================================================================================================//
        private void Button0_ClickBeforeX(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;

            if (processado)
                return;

            if (this.Grid0.DataTable.Rows.Count == 0)
                Class.Conexao.sbo_application.MessageBox("Nenhuma linha para processamento");
            else
            {
                if (Class.Conexao.sbo_application.MessageBox($"Confirma processamento das {this.Grid0.DataTable.Rows.Count} previsões listadas? (Este processo é irreversível)", 2, "Sim", "Não") == 1)
                {
                    ProcessaGrid();
                }
                else
                    Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Processamento não confirmado"), SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
        }

        //==================================================================================================================================//
        private void ProcessaGridX()
        //==================================================================================================================================//
        {
            int iErro = 0, lote;
            string sql, dataDe, dataAte, xfilial = "", now, itemCodeIni, itemCodeFim, msg = "";
            double quant;

            try
            {
                try { xfilial = this.ComboBox0.Selected.Value.Trim(); } catch { }

                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //SelectedRows oSel = this.Grid0.Rows.SelectedRows;

                msg = ValidarConsolidacao(xfilial);

                if (string.IsNullOrEmpty(msg))
                {
                    Class.Conexao.oCompany.StartTransaction();

                    dataDe = this.EditText0.Value.Trim();
                    dataAte = this.EditText1.Value.Trim();
                    now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    itemCodeIni = this.EditText3.Value.Trim();
                    itemCodeFim = this.EditText4.Value.Trim();

                    sql = $@"select ifnull(max(""Code""), 0) + 1 AS ""Lote"" FROM ""@CVA_CAR_LOTE""";
                    oRec.DoQuery(sql);
                    lote = Convert.ToInt32(oRec.Fields.Item("Lote").Value);

                    sql = string.Format(@"
                            INSERT INTO ""@CVA_CAR_LOTE"" (
                                ""Code"", ""Name"", ""U_Lote"", ""U_ID_Filial"", ""U_CreateDate"", ""U_DataDe"", ""U_DataAte"", ""U_ProdutoIni"", ""U_ProdutoFim"") VALUES (
                                '{0}'   , '{1}'   , {2}       , '{3}'          , '{4}'           , '{5}'       , '{6}'        , '{7}'           , '{8}')",
                                 lote   , lote    , lote      , xfilial         , now             , dataDe      , dataAte      , itemCodeIni     , itemCodeFim);

                    oRec.DoQuery(sql);

                    sql = $@"select ifnull(max(""Code""), 0) + 1 AS ""Max"" FROM ""@CVA_CAR_CONSOL""";
                    oRec.DoQuery(sql);
                    int maxCode = Convert.ToInt32(oRec.Fields.Item("Max").Value);

                    for (int i = 0; i < this.Grid0.DataTable.Rows.Count; i++)
                    {
                        DateTime data = Convert.ToDateTime(Grid0.DataTable.GetValue(0, i).ToString());
                        string sData = data.ToString("yyyy-MM-dd");
                        data = Convert.ToDateTime(Grid0.DataTable.GetValue(2, i).ToString());
                        string sDataOrig = data.ToString("yyyy-MM-dd");
                        string itemCode = Grid0.DataTable.GetValue(6, i).ToString();
                        string origem = Grid0.DataTable.GetValue(9, i).ToString();
                        string idTurno = "";
                        string idServico = "";

                        string sQuant = Grid0.DataTable.GetValue(8, i).ToString().Replace(',', '.');
                        quant = Convert.ToDouble(sQuant, CultureInfo.InvariantCulture);

                        string cancelado = "";

                        if (quant > 0)
                        {
                            sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"", ""U_Data"", ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          , '{3}'     , '{4}'         , {5}        , '{6}'          , {7}         , '{8}'    , '{9}'         , '{10}'          , '{11}'      , {12}      , '{13}'       , '{14}'         , '{15}')",
maxCode , maxCode , xfilial         , sData     , itemCode      , sQuant     , cancelado      , 0           , ""       , sDataOrig     , now             , origem      , lote      , idTurno      , idServico      , "PC");

                            oRec.DoQuery(sql);
                            maxCode++;

                            if (origem == "CD")
                            {
                                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"", ""U_Data"", ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          , '{3}'     , '{4}'         , {5}        , '{6}'          , {7}         , '{8}'    , '{9}'         , '{10}'          , '{11}'      , {12}      , '{13}'       , '{14}'         , '{15}')",
maxCode , maxCode , xfilial         , sData     , itemCode      , sQuant     , cancelado      , 0           , ""       , sDataOrig     , now             , origem      , lote      , idTurno      , idServico      , "PV");

                                oRec.DoQuery(sql);
                                maxCode++;
                            }
                        }
                    }

                    /*
                    foreach (System.Data.DataRow row in oDT_OP_Aberto.Rows)
                    {
                        //oCon.CondVal = row["ItmsGrpCod"].ToString();
                        DateTime data = Convert.ToDateTime(row["Data"].ToString());
                        string sData = data.ToString("yyyy-MM-dd");
                        data = Convert.ToDateTime(row["DataPlanej"].ToString());
                        string sDataOrig = data.ToString("yyyy-MM-dd");
                        string itemCode = row["ItemCode"].ToString();
                        string origem = "FI";
                        string idTurno = row["IDTurno"].ToString();
                        string idServico = row["IDServico"].ToString();

                        string sQuant = row["Quant"].ToString().Replace(',', '.');
                        quant = Convert.ToDouble(sQuant, CultureInfo.InvariantCulture);

                        string cancelado = "";

                        if (quant > 0)
                        {
                            sql = string.Format(@"
INSERT INTO ""@CVA_CAR_CONSOL"" (
""Code"", ""Name"", ""U_ID_Filial"",  ""U_ItemCode"", ""U_Quant"", ""U_Cancelado"", ""U_Status"", ""U_Msg"", ""U_DataOrig"", ""U_CreateDate"", ""U_Origem"", ""U_Lote"", ""U_IdTurno"", ""U_IdServico"", ""U_Tipo"") VALUES (
'{0}'   , '{1}'   , '{2}'          ,  '{3}'         , {4}        , '{5}'          , {6}         , '{7}'    , '{8}'         , '{9}'           , '{10}'      , {11}      , '{12}'       , '{13}'         , '{14}')",
maxCode , maxCode , xfilial         ,  itemCode      , sQuant     , cancelado      , 0           , ""       , sDataOrig     , now             , origem      , lote      , idTurno      , idServico      , "OP");

                            oRec.DoQuery(sql);
                            maxCode++;
                        }
                    }
                    */

                    SetarPlanejamentoLido(lote);

                    Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                    InserePrevisao(xfilial, lote);
                    InsereOP(xfilial, lote);
                    InserePedVenda(xfilial, lote);


                    this.GetItem("proc").Enabled = false;
                    processado = true;

                    AtualizarGrid(lote);
                    Class.Conexao.sbo_application.MessageBox("Processamento concluído");
                }
                else
                    throw new Exception(msg);

            }
            catch (Exception ex)
            {
                if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }

        //==================================================================================================================================//
        private void SetarPlanejamentoLido(int lote)
        //==================================================================================================================================//
        {
            string sql = "", msg;
            int code;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (System.Data.DataRow row in oDT_Planejamento.Rows)
                {
                    Int32.TryParse(row["Code"].ToString(), out code);   //CVA_TST_PLAN1

                    //sql = string.Format(@"UPDATE ""@CVA_TST_PLAN1"" SET ""U_CVA_LOTE_CONSOL"" = {0} WHERE ""Code"" = {1} 
                    sql = string.Format(@"UPDATE ""@CVA_LN_PLANEJAMENTO"" SET ""U_CVA_LOTE_CONSOL"" = {0} WHERE ""Code"" = {1} 
", lote, code);
                    oRec.DoQuery(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private string RetornaCodFilialCD(string xfilial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // ************************** TROCAR @CVA_CAR_CONF PARA @CVA_CAR_CONFIG
                sql = string.Format(@"
SELECT TOP 1 IFNULL(T1.""U_BPLId"", '') AS ""U_BPLId""
FROM ""@CVA_CAR_CONF"" T0
    INNER JOIN ""@CVA_CAR_CONF1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
WHERE T0.""U_BPLId"" = {0}
", xfilial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("U_BPLId").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }

        //==================================================================================================================================//
        private string RetornaCodDepPadrao(string xfilial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT IFNULL(""DflWhs"", '') AS ""DflWhs"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", xfilial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("DflWhs").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }


        //==================================================================================================================================//
        private void InserePrevisao(string xfilial, int lote)
        //==================================================================================================================================//
        {
            string sql, sFilial, code, name, depositoPadrao = "", sErrMsg, newDocEntry, origem, bplName, bplIdCD;
            int iErro = 0, iErrCode, j;
            DateTime dataPrevIni, dataPrevFim, dataMin, dataMax;
            System.Data.DataTable oDT;

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                bplIdCD = RetornaCodFilialCD(xfilial);

                sFilial = xfilial.PadLeft(4, '0');
                dataPrevIni = DateTime.ParseExact(this.EditText0.Value.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                dataPrevFim = DateTime.ParseExact(this.EditText1.Value.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                sql = string.Format(@"
SELECT * 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, "PC");
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                if (oDT.Rows.Count > 0)
                {
                    SAPbobsCOM.SalesForecast oSalesForecast = (SAPbobsCOM.SalesForecast)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesForecast);
                    SAPbobsCOM.ProductionOrders oOP = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                    bplName = RetornaNomeFilial(xfilial);
                    dataMin = RetornaData("Min", lote, "PC");
                    dataMax = RetornaData("Max", lote, "PC");
                    code = lote.ToString().PadLeft(6, '0') + "_PC";
                    name = "PC - FILIAL " + bplName + " " + dataPrevIni.ToString("dd/MM/yyyy") + " até " + dataPrevFim.ToString("dd/MM/yyyy");

                    oSalesForecast.ForecastCode = code;
                    oSalesForecast.ForecastName = name;
                    oSalesForecast.View = SAPbobsCOM.ForecastViewTypeEnum.fvtDaily;
                    oSalesForecast.ForecastStartDate = dataMin;
                    oSalesForecast.ForecastEndDate = dataMax;
                    oSalesForecast.UserFields.Fields.Item("U_BPLId").Value = xfilial;

                    SAPbobsCOM.SalesForecast_Lines oSalesForecastLine = oSalesForecast.Lines;

                    j = 0;
                    foreach (System.Data.DataRow linha in oDT.Rows)
                    {
                        origem = linha["U_Origem"].ToString();
                        if (origem == "FI")
                            depositoPadrao = RetornaCodDepPadrao(xfilial);
                        else
                            depositoPadrao = RetornaCodDepPadrao(bplIdCD);

                        if (j > 0)
                            oSalesForecastLine.Add();
                        oSalesForecastLine.ForecastedDay = Convert.ToDateTime(linha["U_Data"].ToString());
                        oSalesForecastLine.ItemNo = linha["U_ItemCode"].ToString();
                        oSalesForecastLine.Quantity = Convert.ToDouble(linha["U_Quant"].ToString());
                        oSalesForecastLine.Warehouse = depositoPadrao;
                        j++;
                    }
                    iErro = oSalesForecast.Add();
                    if (iErro != 0)
                    {
                        Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                        AtualizaTabela2("0", lote, "PC", 2, sErrMsg);
                    }
                    else
                    {
                        newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                        AtualizaTabela2(newDocEntry, lote, "PC", 1, "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private string RetornaNomeFilial(string xfilial)
        //==================================================================================================================================//
        {
            string s = "", sql;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT ""BPLName"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", xfilial);
                oRec.DoQuery(sql);
                if (oRec.RecordCount > 0)
                    s = oRec.Fields.Item("BPLName").Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        //==================================================================================================================================//
        private void InsereOP(string xfilial, int lote)
        //==================================================================================================================================//
        {
            string sql, depositoPadrao = "", newDocEntry, sErrMsg;
            int erro, iErrCode, j;
            System.Data.DataTable oDT, dtLinhas;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT * 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, "OP");
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                sql = string.Format(@"SELECT ""DflWhs"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", xfilial);
                oRec.DoQuery(sql);
                if (oRec.RecordCount > 0)
                    depositoPadrao = oRec.Fields.Item("DflWhs").Value.ToString();

                foreach (System.Data.DataRow linha in oDT.Rows)
                {
                    SAPbobsCOM.ProductionOrders oOP = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                    oOP.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotStandard;
                    oOP.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned;
                    oOP.ItemNo = linha["U_ItemCode"].ToString();
                    oOP.PlannedQuantity = Convert.ToDouble(linha["U_Quant"].ToString());
                    oOP.Warehouse = depositoPadrao;
                    oOP.PostingDate = DateTime.Today;
                    oOP.StartDate = Convert.ToDateTime(linha["U_DataOrig"].ToString());
                    oOP.DueDate = Convert.ToDateTime(linha["U_DataOrig"].ToString());
                    oOP.UserFields.Fields.Item("U_CVA_Turno").Value = linha["U_IdTurno"].ToString();
                    oOP.UserFields.Fields.Item("U_CVA_SERVICO").Value = linha["U_IdServico"].ToString();

                    sql = string.Format(@"
SELECT
    T4.""Code"" AS ""Prato""
	,T5.""Code"" AS ""ItemCode""
	,T6.""ItemName""
	,(T5.""Quantity"" / T4.""PlAvgSize"") AS ""Unit""
	,(T5.""Quantity"" / T4.""PlAvgSize"") * {1} AS ""Quant""
FROM ""OITT"" T4
    INNER JOIN ""ITT1"" T5 ON T5.""Father"" = T4.""Code""
    INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T5.""Code""
WHERE T4.""Code"" = '{0}'
", oOP.ItemNo, oOP.PlannedQuantity);

                    dtLinhas = Class.Conexao.ExecuteSqlDataTable(sql);
                    j = 0;
                    foreach (System.Data.DataRow linha1 in dtLinhas.Rows)
                    {
                        if (j > 0)
                            oOP.Lines.Add();

                        oOP.Lines.ItemNo = linha1["ItemCode"].ToString();
                        oOP.Lines.BaseQuantity = Convert.ToDouble(linha1["Unit"].ToString());
                        //oOP.Lines.DistributionRule = "";
                        //oOP.Lines.EndDate = oOP.DueDate;
                        oOP.Lines.ItemType = SAPbobsCOM.ProductionItemType.pit_Item;
                        oOP.Lines.PlannedQuantity = oOP.PlannedQuantity * oOP.Lines.BaseQuantity;
                        oOP.Lines.ProductionOrderIssueType = SAPbobsCOM.BoIssueMethod.im_Manual;
                        //oOP.Lines.StartDate = oOP.StartDate;
                        oOP.Lines.Warehouse = depositoPadrao;

                        j++;
                    }
                    erro = oOP.Add();
                    if (erro != 0)
                    {
                        Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                        AtualizaTabelaOP("0", linha["Code"].ToString(), 2, sErrMsg);
                    }
                    else
                    {
                        newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                        AtualizaTabelaOP(newDocEntry, linha["Code"].ToString(), 1, "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void InserePedVenda(string xfilial, int lote)
        //==================================================================================================================================//
        {
            string sql, sFilial, code, name, depositoPadrao = "", sErrMsg, newDocEntry, origem, aux, sData, cardCode = "", usage = "", itemCode, bplName = "";
            int iErro = 0, iErrCode, j, tipoPreco = 0, iLinha;
            double quant;
            DateTime dataPrevIni, dataPrevFim, dataMin, dataMax, data;
            System.Data.DataTable oDT;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                SAPbobsCOM.Recordset oRec1 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // ************************** TROCAR @CVA_CAR_CONF PARA @CVA_CAR_CONFIG
                // Buscar as configurações de PN, usage, etc para esta filial
                sql = string.Format(@"
SELECT TOP 1 T1.*, T2.""DflWhs"" AS ""DepPadrao"", T2.""BPLName""
FROM ""@CVA_CAR_CONF"" T0
    INNER JOIN ""@CVA_CAR_CONF1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
    LEFT JOIN ""OBPL"" T2 ON T2.""BPLId"" = T0.""U_BPLId""
WHERE T0.""U_BPLId"" = {0}
", xfilial);
                oRec1.DoQuery(sql);

                if (oRec1.RecordCount > 0)
                {
                    //cardCode = oRec1.Fields.Item("U_CardCodePN").Value.ToString(); -> esta informação vem direto da tabela de filial
                    tipoPreco = Convert.ToInt32(oRec1.Fields.Item("U_PrecoUnit").Value.ToString());
                    //sFilialOrigem = oRec1.Fields.Item("U_BPLId").Value.ToString(); 
                    //depositoPadrao = oRec1.Fields.Item("DepPadrao").Value.ToString();
                    depositoPadrao = oRec1.Fields.Item("U_WhsCode").Value.ToString();
                    usage = oRec1.Fields.Item("U_UsgTransf").Value.ToString();
                    bplName = oRec1.Fields.Item("BPLName").Value.ToString();
                }

                sql = string.Format(@"SELECT IFNULL(""U_CVA_Codigo2PN"", '') AS ""CardCode"" FROM OBPL WHERE ""BPLId"" = {0} ", xfilial);
                oRec1.DoQuery(sql);

                if (oRec1.RecordCount > 0)
                    cardCode = oRec1.Fields.Item("CardCode").Value.ToString();

                // Determinar quantos Pedidos de Venda agrupando por data de entrega
//                sql = string.Format(@"
//SELECT ""U_Lote"", ""U_Data""
//FROM ""@CVA_CAR_CONSOL"" 
//WHERE ""U_Lote"" = {0}
//    AND ""U_Tipo"" = 'PC' 
//    AND ""U_Origem"" = 'CD' 
//GROUP BY ""U_Lote"", ""U_Data""
//", lote);
                sql = string.Format(@"
SELECT ""U_Lote"", ""U_Data""
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}
    AND ""U_Tipo"" = 'PV' 
GROUP BY ""U_Lote"", ""U_Data""
", lote);
                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    oRec.MoveFirst();
                    // Para cada data de entrega, criar um Pedido de Venda com todos os itens
                    for (int i = 0; i < oRec.RecordCount; i++)
                    {
                        SAPbobsCOM.Documents oOrders = (SAPbobsCOM.Documents)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                        aux = oRec.Fields.Item("U_Data").Value.ToString();
                        data = Convert.ToDateTime(aux.Substring(6, 4) + "-" + aux.Substring(3, 2) + "-" + aux.Substring(0, 2));
                        sData = data.ToString("yyyy-MM-dd");

                        oOrders.CardCode = cardCode;
                        oOrders.DocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOrders.DocDueDate = data;
                        oOrders.TaxDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOrders.BPL_IDAssignedToInvoice = 2;   // Convert.ToInt32(filial);
                        //oOrders.BPL_IDAssignedToInvoice = Convert.ToInt32(filial);
                        //oOrders.BPL_IDAssignedToInvoice = Convert.ToInt32(sFilialOrigem);
                        oOrders.Comments = "Consolidação de Planejamento - Transferência - CD para FILIAL " + bplName;
                        iLinha = 0;

                        sql = string.Format(@"
SELECT *
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = 'PV'  
    AND ""U_Data"" = '{1}'
", lote, sData);
                        oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                        foreach (System.Data.DataRow linha in oDT.Rows)
                        {
                            itemCode = linha["U_ItemCode"].ToString();
                            quant = Convert.ToDouble(linha["U_Quant"].ToString());

                            if (iLinha > 0)
                                oOrders.Lines.Add();
                            oOrders.Lines.ItemCode = itemCode;
                            oOrders.Lines.ShipDate = data;
                            //oOrders.Lines.UoMCode = "";     -> read only
                            oOrders.Lines.Quantity = quant;
                            //oOrders.Lines.WarehouseCode = "";
                            oOrders.Lines.Price = RetornaPreco(tipoPreco, itemCode);
                            //oOrders.Lines.Price = 10.5;
                            oOrders.Lines.Usage = usage;
                            oOrders.Lines.WarehouseCode = depositoPadrao;
                            //oOrders.Lines.
                            iLinha++;
                        }
                        iErro = oOrders.Add();

                        if (iErro != 0)
                        {
                            Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                            //AtualizaTabelaPV(string newDocEntry, int lote, DateTime data, int erro, string msgErro)
                            AtualizaTabelaPV("0", lote, data, 2, sErrMsg);
                        }
                        else
                        {
                            newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                            AtualizaTabelaPV(newDocEntry, lote, data, 1, "");
                        }
                        oRec.MoveNext();
                    }
                }                        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //==================================================================================================================================//
        private void AtualizarGrid(int lote)
        //==================================================================================================================================//
        {
            string sql;
            
            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT
    T0.""Code""
	,T0.""U_Lote"" AS ""Lote""
	,T0.""U_ID_Filial"" AS ""Filial""
	,T1.""BPLName""
    ,T0.""U_Tipo"" AS ""Tipo""
	,T0.""U_Data"" AS ""Data""
	,T0.""U_DataOrig"" AS ""DataOrig""
	,T0.""U_ItemCode"" AS ""ItemCode""
	,T2.""ItemName""
	,T0.""U_Quant"" AS ""Quant""
	,T0.""U_Origem"" AS ""Origem""
	,T3.""Name"" AS ""Turno""
	,T4.""Name"" AS ""Servico""	
	,T0.""U_DocEntryPC"" AS ""Previsao""
	,T0.""U_DocEntryPV"" AS ""Ped Venda""
	,T0.""U_DocEntryOP"" AS ""OP""
	,T0.""U_Cancelado"" AS ""Cancelado""
	,T0.""U_Status"" AS ""Status""
	,T0.""U_Msg"" AS ""Msg""
FROM ""@CVA_CAR_CONSOL"" T0
	INNER JOIN ""OBPL"" T1 ON T1.""BPLId"" = T0.""U_ID_Filial""
	INNER JOIN ""OITM"" T2 ON T2.""ItemCode"" = T0.""U_ItemCode""
	LEFT JOIN ""@CVA_TURNO"" T3 ON T3.""Code"" = T0.""U_IdTurno""	
	LEFT JOIN ""@CVA_SERVICO_PLAN"" T4 ON T4.""Code"" = T0.""U_IdServico""
WHERE ""U_Lote"" = {0} 
", lote);

                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    this.Grid0.DataTable.Clear();
                    this.Grid0.DataTable.ExecuteQuery(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private DateTime RetornaData(string opcao, int lote, string tipo)
        //==================================================================================================================================//
        {
            DateTime dtAux = DateTime.MinValue;
            string sql, aux;

            if (opcao == "Min")
                sql = string.Format(@"
SELECT MIN(""U_Data"") 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, tipo);
            else
                sql = string.Format(@"
SELECT MAX(""U_Data"") 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, tipo);

            aux = Class.Conexao.ExecuteSqlScalar(sql).ToString();
            DateTime.TryParse(aux, out dtAux);

            return dtAux;
        }

        //==================================================================================================================================//
        private double RetornaPreco(int tipoPreco, string itemCode)
        //==================================================================================================================================//
        {
            string sql;
            double preco = 0;

            SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            sql = string.Format(@"SELECT IFNULL(""LastPurPrc"", 0) AS ""LastPurPrc"", IFNULL(""AvgPrice"", 0) AS ""AvgPrice"" FROM OITM WHERE ""ItemCode"" = '{0}'", itemCode);
            oRec.DoQuery(sql);

            if (oRec.RecordCount > 0)
            {
                string s = oRec.Fields.Item("AvgPrice").Value.ToString();

                if (tipoPreco == 1)
                    preco = Convert.ToDouble(oRec.Fields.Item("LastPurPrc").Value.ToString(), CultureInfo.InvariantCulture);
                else
                    preco = Convert.ToDouble(oRec.Fields.Item("AvgPrice").Value.ToString(), CultureInfo.InvariantCulture);
            }

            return preco;
        }


        //==================================================================================================================================//
        private void AtualizaTabela2(string newDocEntry, int lote, string tipo, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                //sData = data.ToString("yyyy-MM-dd");
                msg = msgErro.Replace("'", "");

                //if (tipo == "CD")
                //{
                    sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryPC"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""U_Lote"" = {3} 
    AND ""U_Tipo"" = '{4}'  
", newDocEntry, erro, msg, lote, tipo);
                //}

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void AtualizaTabelaOP(string newDocEntry, string code, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", sData, msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                msg = msgErro.Replace("'", "");

                sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryOP"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""Code"" = '{3}' 
", newDocEntry, erro, msg, code);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void AtualizaTabelaPV(string newDocEntry, int lote, DateTime data, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", sData, msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                msg = msgErro.Replace("'", "");
                sData = data.ToString("yyyy-MM-dd");

                sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryPV"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""U_Lote"" = {3} 
    AND ""U_Tipo"" = 'PV'
    AND ""U_Data"" = '{4}'  
", newDocEntry, erro, msg, lote, sData);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void EditText3_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        //==================================================================================================================================//
        {
            Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
            try
            {
                SBOChooseFromListEventArg ChooseEvents = ((SBOChooseFromListEventArg)(pVal));
                string chave = ChooseEvents.SelectedObjects.GetValue(0, 0).ToString();
                string descricao = ChooseEvents.SelectedObjects.GetValue(1, 0).ToString();

                try { EditText3.Value = chave; } catch { }
                try { EditText5.Value = descricao; } catch { }
            }
            catch (Exception ex)
            {

            }
        }

        //==================================================================================================================================//
        private void EditText4_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        //==================================================================================================================================//
        {
            Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
            try
            {
                SBOChooseFromListEventArg ChooseEvents = ((SBOChooseFromListEventArg)(pVal));
                string chave = ChooseEvents.SelectedObjects.GetValue(0, 0).ToString();
                string descricao = ChooseEvents.SelectedObjects.GetValue(1, 0).ToString();

                try { EditText4.Value = chave; } catch { }
                try {  EditText6.Value = descricao; } catch { }
            }
            catch (Exception ex)
            {

            }
        }

        //==================================================================================================================================//
        private void Button2_ClickBeforeX(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;

            string msg = "", s = "";
            int xfilial;

            try
            {
                try { s = this.ComboBox0.Selected.Value.Trim(); } catch { }
                Int32.TryParse(s, out xfilial);

                filiais = new List<int>();
                if (!string.IsNullOrEmpty(s))
                    filiais.Add(xfilial);

                //msg = ValidarFiltros(filiais);
                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                Class.Conexao.oCompany.StartTransaction();
                foreach (int n in filiais)
                {
                    //Carga(n);
                }
                Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

            }
            catch (Exception ex)
            {
                if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }

        }


        //==================================================================================================================================//
        private void CargaX(int xfilial)
        //==================================================================================================================================//
        {
            string dataDe, dataAte, sql, code;
            System.Data.DataTable table;

            try
            {
                /*
                dataDe = this.EditText0.Value.Trim();
                dataAte = this.EditText1.Value.Trim();

                table = Class.Geral.TabelaPlanejamento(xfilial, dataDe, dataAte);
                if (table.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow linha in table.Rows)
                    {
                        code = linha["Code"].ToString();

                        if (!Class.Geral.PlanejamentoCarregado("@CVA_CAR_PLAN", code))
                            Class.Geral.SubirPlanejamento(linha);
                    }
                }

                table = Class.Geral.TabelaLnPlanejamento(xfilial, dataDe, dataAte);
                if (table.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow linha in table.Rows)
                    {
                        code = linha["Code"].ToString();

                        if (!Class.Geral.PlanejamentoCarregado("@CVA_CAR_PLAN1", code))
                            Class.Geral.SubirLnPlanejamento(xfilial, linha);

                        if (!Class.Geral.PlanejamentoCarregado("@CVA_CAR_OP", code))
                            Class.Geral.SubirOPPlanejamento(xfilial, code);

                    }
                }
                */


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void Button2_ClickBefore_Old(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;
            string dataDe, dataAte, xfilial = "", sql, msg = "", itemCodeIni, itemCodeFim;
            int linha;

            Form oForm = (Form)this.UIAPIRawForm;

            try
            {
                dataDe = this.EditText0.Value.Trim();
                dataAte = this.EditText1.Value.Trim();
                itemCodeIni = this.EditText3.Value.Trim();
                itemCodeFim = this.EditText4.Value.Trim();

                try { xfilial = this.ComboBox0.Selected.Value.Trim(); } catch { }
                if (string.IsNullOrEmpty(xfilial))
                    msg += "Nenhuma filial selecionada" + Environment.NewLine;

                if ((string.IsNullOrEmpty(dataDe)) || (string.IsNullOrEmpty(dataAte)))
                    msg += "Data inicial ou final não preenchida" + Environment.NewLine;

                if (string.IsNullOrEmpty(msg))
                    if ((Convert.ToDateTime(dataDe.Substring(0, 4) + "-" + dataDe.Substring(4, 2) + "-" + dataDe.Substring(6, 2))) > Convert.ToDateTime(dataAte.Substring(0, 4) + "-" + dataAte.Substring(4, 2) + "-" + dataAte.Substring(6, 2)))
                        msg += "Data inicial posterior à data final" + Environment.NewLine;

                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                //oForm.Freeze(true);
                //this.UIAPIRawForm.Freeze(true);

                /*
                sql = string.Format(@"
SELECT 
	""Filial"" 
	,MAX(""BPLName"") AS ""BPLName""
	,""Data"" AS ""Data""
	,MAX(""DiasPlanej"") AS ""DiasPlanej""
	,MIN(""DataPlanej"") AS ""DataPlanej""
	,""ItemCode"" AS ""ItemCode""
	,MAX(""ItemName"") AS ""ItemName""
	,SUM(""Quant"") AS ""Quant""
	,MAX(""Destino"") AS ""Destino""
FROM (SELECT
	T8.""BPLId"" AS ""Filial""
	,T8.""BPLName"" AS ""BPLName""
	,T11.""BPLId"" AS ""FilOrigCod""
	,T11.""BPLName"" AS ""FilOrigNam""
	,T0.""U_CVA_DATA_REF"" AS ""DataPlanej""
	,IFNULL(T8.""U_CVA_DiasPlanej"", 0) AS ""DiasPlanej""
	,(	SELECT MAX(T21.""U_CVA_Data"")
		FROM ""@CVA_CAR_CAL"" T20 
			LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
		WHERE T20.""U_BPLId"" = T8.""BPLId""
			AND T21.""U_CVA_Data"" <= ADD_DAYS(T0.""U_CVA_DATA_REF"", T8.""U_CVA_DiasPlanej"" * -1)
	) AS ""Data""
	,T5.""Code"" AS ""ItemCode""
	,T6.""ItemName"" AS ""ItemName""
	,(T5.""Quantity"" / T4.""PlAvgSize"") * T1.""U_CVA_QTD"" AS ""Quant""
	,CASE
		WHEN IFNULL(T9.""DocEntry"", 0) > 0  THEN 'PC'
		WHEN (IFNULL(T9.""DocEntry"", 0) = 0 AND T6.""PrcrmntMtd"" = 'M')  THEN 'PA'
		WHEN (IFNULL(T9.""DocEntry"", 0) = 0 AND T6.""PrcrmntMtd"" <> 'M')  THEN 'CD'
		ELSE ''
	END AS ""Destino""	
FROM ""@CVA_PLANEJAMENTO"" T0
	INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""Code"" = T0.""Code""	
	INNER JOIN ""OITT"" T4 ON T4.""Code"" = T1.""U_CVA_INSUMO""
	INNER JOIN ""ITT1"" T5 ON T5.""Father"" = T4.""Code""
	INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T5.""Code""
	LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
	LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T2.""U_CVA_FILIAL""
	LEFT JOIN ""OBPL"" T11 ON T11.""DflWhs"" = T4.""ToWH""
	LEFT JOIN ""@CVA_CAR_INS"" T7 ON T7.""U_BPLId"" = T8.""BPLId""
	LEFT JOIN ""@CVA_CAR_INS1"" T9 ON T9.""DocEntry"" = T7.""DocEntry"" 
		AND (T9.""U_CVA_ItmsGrpCod"" = T6.""ItmsGrpCod"" OR T9.""U_CVA_ItemCode"" = T6.""ItemCode"")
WHERE T8.""BPLId"" = {0} AND T0.""U_CVA_DATA_REF"" >= '{1}' AND T0.""U_CVA_DATA_REF"" <= '{2}'

UNION ALL

SELECT
	T8.""BPLId""
	,T8.""BPLName""
	,T11.""BPLId"" AS ""FilialOrig""
	,T11.""BPLName"" AS ""FilOrigNam""
	,T0.""U_CVA_DATA_REF""
	,IFNULL(T8.""U_CVA_DiasPlanej"", 0)
	,(	SELECT MAX(T21.""U_CVA_Data"")
		FROM ""@CVA_CAR_CAL"" T20 
			LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
		WHERE T20.""U_BPLId"" = T8.""BPLId""
			AND T21.""U_CVA_Data"" <= ADD_DAYS(T0.""U_CVA_DATA_REF"", T8.""U_CVA_DiasPlanej"" * -1)
	)
	,T6.""ItemCode""
	,T6.""ItemName""
	,T1.""U_CVA_QTD""
	,CASE
		WHEN IFNULL(T9.""DocEntry"", 0) > 0  THEN 'PC'
		WHEN (IFNULL(T9.""DocEntry"", 0) = 0 AND T6.""PrcrmntMtd"" = 'M')  THEN 'PA'
		WHEN (IFNULL(T9.""DocEntry"", 0) = 0 AND T6.""PrcrmntMtd"" <> 'M')  THEN 'CD'
		ELSE ''
	END
FROM ""@CVA_PLANEJAMENTO"" T0
	INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""Code"" = T0.""Code""	
	LEFT JOIN ""OITT"" T4 ON T4.""Code"" = T1.""U_CVA_INSUMO""
	INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T1.""U_CVA_INSUMO""
	LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
	LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T2.""U_CVA_FILIAL""
	LEFT JOIN ""OBPL"" T11 ON T11.""DflWhs"" = T4.""ToWH""
	LEFT JOIN ""@CVA_CAR_INS"" T7 ON T7.""U_BPLId"" = T8.""BPLId""
	LEFT JOIN ""@CVA_CAR_INS1"" T9 ON T9.""DocEntry"" = T7.""DocEntry"" 
		AND (T9.""U_CVA_ItmsGrpCod"" = T6.""ItmsGrpCod"" OR T9.""U_CVA_ItemCode"" = T6.""ItemCode"")
WHERE T4.""Code"" IS NULL AND T8.""BPLId"" = {0} AND T0.""U_CVA_DATA_REF"" >= '{1}' AND T0.""U_CVA_DATA_REF"" <= '{2}'
)
GROUP BY ""Filial"", ""Data"" ,""ItemCode""
 ", filial, dataDe, dataAte);
 */

                sql = string.Format(@"
SELECT 
	""Data"" AS ""Data Entrega""
	,MAX(""DiasPlanej"") AS ""Dias Segurança""
	,MIN(""DataPlanej"") AS ""Data Prim Consumo""
    ,MAX(""Categoria"") AS ""Categoria""
    ,MAX(""Familia"") AS ""Família""
    ,MAX(""SubFamilia"") AS ""SubFamília""
    ,""ItemCode"" AS ""Código Item""
	,MAX(""ItemName"") AS ""Descrição""
	,SUM(""Quant"") AS ""Quantidade""
	,MAX(""Origem"") AS ""Origem""
FROM (
	SELECT
		T8.""BPLId"" AS ""Filial""
		,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
		,IFNULL(T8.""U_CVA_DiasPlanej"", 0) AS ""DiasPlanej""
		,(	SELECT MAX(T21.""U_CVA_Data"")
			FROM ""@CVA_CAR_CAL"" T20 
				LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
			WHERE T20.""U_BPLId"" = T8.""BPLId""
				AND T20.""U_Categoria"" = T6.""U_CVA_Categoria""
                AND T21.""U_CVA_Data"" <= ADD_DAYS(ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1), IFNULL(T8.""U_CVA_DiasPlanej"", 0) * -1)
		) AS ""Data""
		,T1.""U_CVA_INSUMO""
		,T6.""ItemCode"" AS ""ItemCode""
		,T6.""ItemName"" AS ""ItemName""
		,(T5.""Quantity"" / T4.""PlAvgSize"") * T1.""U_CVA_QTD"" AS ""QuantTot""
		,T9.""DocEntry""
		,CASE
			WHEN IFNULL(T9.""DocEntry"", 0) > 0 THEN 'FI'
			WHEN IFNULL(T9.""DocEntry"", 0) = 0 THEN 'CD'
			ELSE ''
		END AS ""Origem""
		,T16.""Code"" AS ""IdServico""
		,T16.""Name"" AS ""Servico""
		,T15.""Code"" AS ""IdTurno""
		,T14.""U_CVA_DES_TURNO"" AS ""Turno""
        ,T6.""U_CVA_Familia"" AS ""Familia""
        ,T6.""U_CVA_Subfamilia"" AS ""SubFamilia""
		,T18.""Descr"" AS ""Categoria""
		,CASE WEEKDAY((	ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) ))
            WHEN 6 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_DOMINGO""
			WHEN 0 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_SEGUNDA""
			WHEN 1 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_TERCA""
			WHEN 2 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_QUARTA""
			WHEN 3 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_QUINTA""
			WHEN 4 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_SEXTA""
			WHEN 5 THEN (T5.""Quantity"" / T4.""PlAvgSize"") * T14.""U_CVA_SABADO""
			ELSE 0  
		END	AS ""Quant""                
    FROM ""@CVA_PLANEJAMENTO"" T0
		INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
		INNER JOIN ""OITT"" T4 ON T4.""Code"" = T1.""U_CVA_INSUMO""
		INNER JOIN ""ITT1"" T5 ON T5.""Father"" = T4.""Code""
		INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T5.""Code""
		INNER JOIN ""OITM"" T3 ON T3.""ItemCode"" = T1.""U_CVA_INSUMO""
		LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
		LEFT JOIN ""OOAT"" T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
		LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
		LEFT JOIN ""@CVA_CAR_INS"" T7 ON T7.""U_BPLId"" = T8.""BPLId""
		LEFT JOIN ""@CVA_CAR_INS1"" T9 ON T9.""DocEntry"" = T7.""DocEntry"" 
			AND (
                    ((T9.""U_CVA_ItmsGrpCod"" = T6.""ItmsGrpCod"") AND (IFNULL(T9.""U_CVA_ItemCode"", '') = ''))
                    OR (IFNULL(T9.""U_CVA_ItemCode"", '') = T6.""ItemCode"")
                    OR (IFNULL(T9.""U_CVA_Familia"", '') = T6.""U_CVA_Familia"")
                    OR (IFNULL(T9.""U_CVA_SFamilia"", '') = T6.""U_CVA_Subfamilia"")
                )
		LEFT JOIN ""@CVA_COMENSAIS"" T13 ON T13.""U_CVA_ID_CONTRATO"" = T0.""U_CVA_ID_CONTRATO"" AND T13.""U_CVA_GRPSERVICO"" = T0.""U_CVA_ID_G_SERVICO""
		LEFT JOIN ""@CVA_LIN_COMENSAIS"" T14 ON T14.""Code"" = T13.""Code"" AND T14.""U_CVA_SERVICO"" = T0.""U_CVA_ID_SERVICO""
		LEFT JOIN ""@CVA_TURNO"" T15 ON T15.""Name"" = T14.""U_CVA_DES_TURNO""
		LEFT JOIN ""@CVA_SERVICO_PLAN"" T16 ON T16.""Code"" = T0.""U_CVA_ID_SERVICO"" 
		LEFT JOIN ""CUFD"" T17 ON T17.""TableID"" = 'OITM' AND T17.""AliasID"" = 'CVA_Categoria' 
        LEFT JOIN ""UFD1"" T18 ON T18.""TableID"" = T17.""TableID"" AND T18.""FieldID"" = T17.""FieldID"" AND T18.""FldValue"" = T6.""U_CVA_Categoria""
    WHERE
        T8.""BPLId"" = {0}
		AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'  
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
		AND ('{3}' = '' OR T3.""ItemCode"" >= '{3}')
		AND ('{4}' = '' OR T3.""ItemCode"" <= '{4}')
	ORDER BY T1.""U_CVA_INSUMO""
	)
GROUP BY ""Filial"", ""Data"" ,""ItemCode""	
ORDER BY ""ItemCode""	
", xfilial, dataDe, dataAte, itemCodeIni, itemCodeFim);
                //""@CVA_PLANEJAMENTO""  ""@CVA_LN_PLANEJAMENTO""
                //""@CVA_TST_PLAN""  ""@CVA_TST_PLAN1""

                this.Grid0.DataTable.ExecuteQuery(sql);

                sql = string.Format(@"
SELECT 
	MAX(""Data"") AS ""Data""
	,MAX(""DiasPlanej"") AS ""DiasPlanej""
	,""DataPlanej"" AS ""DataPlanej""
	,""IdServico""
	,MAX(""Servico"") AS ""Servico""
	,""IdTurno""
	,MAX(""Turno"") AS ""Turno""
	,""ItemCode"" AS ""ItemCode""
	,MAX(""ItemName"") AS ""ItemName""
	,SUM(""Quant"") AS ""Quant""
FROM (
	SELECT
		T8.""BPLId"" AS ""Filial""
		,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
		,IFNULL(T8.""U_CVA_DiasPlanej"", 0) AS ""DiasPlanej""
		,(	SELECT MAX(T21.""U_CVA_Data"")
			FROM ""@CVA_CAR_CAL"" T20 
				LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
			WHERE T20.""U_BPLId"" = T8.""BPLId""
				AND T20.""U_Categoria"" = T3.""U_CVA_Categoria""
				AND T21.""U_CVA_Data"" <= ADD_DAYS(ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1), IFNULL(T8.""U_CVA_DiasPlanej"", 0) * -1)
		) AS ""Data""
		,T16.""Code"" AS ""IdServico""
		,T16.""Name"" AS ""Servico""
		,T15.""Code"" AS ""IdTurno""
		,T14.""U_CVA_DES_TURNO"" AS ""Turno""
		,T3.""ItemCode"" AS ""ItemCode""
		,T3.""ItemName"" AS ""ItemName""
		,T1.""U_CVA_QTD"" AS ""Quant""
	FROM ""@CVA_PLANEJAMENTO"" AS T0
		INNER JOIN ""@CVA_LN_PLANEJAMENTO"" AS T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
		INNER JOIN ""OITM"" T3 ON T3.""ItemCode"" = T1.""U_CVA_INSUMO""
		LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
		LEFT JOIN ""OOAT"" T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
		LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
		LEFT JOIN ""@CVA_COMENSAIS"" T13 ON T13.""U_CVA_ID_CONTRATO"" = T0.""U_CVA_ID_CONTRATO"" AND T13.""U_CVA_GRPSERVICO"" = T0.""U_CVA_ID_G_SERVICO""
		LEFT JOIN ""@CVA_LIN_COMENSAIS"" T14 ON T14.""Code"" = T13.""Code"" AND T14.""U_CVA_SERVICO"" = T0.""U_CVA_ID_SERVICO""
		LEFT JOIN ""@CVA_TURNO"" T15 ON T15.""Name"" = T14.""U_CVA_DES_TURNO""
		LEFT JOIN ""@CVA_SERVICO_PLAN"" T16 ON T16.""Code"" = T0.""U_CVA_ID_SERVICO"" 
	WHERE 
		T8.""BPLId"" = {0}
		AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'  
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
		AND ('{3}' = '' OR T3.""ItemCode"" >= '{3}')
		AND ('{4}' = '' OR T3.""ItemCode"" <= '{4}')
	)
GROUP BY ""Filial"", ""DataPlanej"" ,""ItemCode"", ""IdServico"", ""IdTurno""
		", xfilial, dataDe, dataAte, itemCodeIni, itemCodeFim);

                //oDT_OP_Aberto = Class.Conexao.ExecuteSqlDataTable(sql);

                sql = string.Format(@"
    SELECT
        T1.""Code""
		,T8.""BPLId"" AS ""Filial""
		,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
		,IFNULL(T8.""U_CVA_DiasPlanej"", 0) AS ""DiasPlanej""
		,(SELECT MAX(T21.""U_CVA_Data"")
            FROM ""@CVA_CAR_CAL"" T20
                LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
            WHERE T20.""U_BPLId"" = T8.""BPLId""
                AND T20.""U_Categoria"" = T3.""U_CVA_Categoria""
                AND T21.""U_CVA_Data"" <= ADD_DAYS(ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1), IFNULL(T8.""U_CVA_DiasPlanej"", 0) * -1)
		) AS ""Data""
		,T16.""Code"" AS ""IdServico""
		,T16.""Name"" AS ""Servico""
		,T3.""ItemCode"" AS ""ItemCode""
		,T3.""ItemName"" AS ""ItemName""
		,T1.""U_CVA_QTD"" AS ""Quant""
    FROM ""@CVA_PLANEJAMENTO"" AS T0
        INNER JOIN ""@CVA_LN_PLANEJAMENTO"" AS T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
        INNER JOIN ""OITM"" T3 ON T3.""ItemCode"" = T1.""U_CVA_INSUMO""
        LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
        LEFT JOIN ""OOAT"" T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
        LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
        LEFT JOIN ""@CVA_SERVICO_PLAN"" T16 ON T16.""Code"" = T0.""U_CVA_ID_SERVICO""
    WHERE
        T8.""BPLId"" = {0}
        AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
        AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'
        AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
        AND('{3}' = '' OR T3.""ItemCode"" >= '{3}')
        AND('{4}' = '' OR T3.""ItemCode"" <= '{4}')
		", xfilial, dataDe, dataAte, itemCodeIni, itemCodeFim);

                oDT_Planejamento = Class.Conexao.ExecuteSqlDataTable(sql);


                /*
                sql = string.Format(@"
SELECT 
	""Filial"" 
	,MAX(""BPLName"") AS ""BPLName""
	,""Data"" AS ""Data""
	,MAX(""DiasPlanej"") AS ""DiasPlanej""
	,MIN(""DataPlanej"") AS ""DataPlanej""
	,""ItemCode"" AS ""ItemCode""
	,MAX(""ItemName"") AS ""ItemName""
	,SUM(""Quant"") AS ""Quant""
FROM (
	SELECT
		T8.""BPLId"" AS ""Filial""
		,T8.""BPLName"" AS ""BPLName""
		,T0.""U_Data"" AS ""DataPlanej""
		,IFNULL(T8.""U_CVA_DiasPlanej"", 0) AS ""DiasPlanej""
		,(	SELECT MAX(T21.""U_CVA_Data"")
			FROM ""@CVA_CAR_CAL"" T20 
				LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
			WHERE T20.""U_BPLId"" = T8.""BPLId""
				AND T21.""U_CVA_Data"" <= ADD_DAYS(T0.""U_Data"", IFNULL(T8.""U_CVA_DiasPlanej"", 0) * -1)
		) AS ""Data""
		,T3.""ItemCode"" AS ""ItemCode""
		,T3.""ItemName"" AS ""ItemName""
		,T0.""U_Quant"" AS ""Quant""
	FROM ""@CVA_CAR_TESTE"" T0
		INNER JOIN ""OITM"" T3 ON T3.""ItemCode"" = T0.""U_ItemCode""
		LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CardCode""
		LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T0.""U_BPLId""
	WHERE 
		T8.""BPLId"" = {0}
		AND T0.""U_Data"" >= '{1}' 
		AND T0.""U_Data"" <= '{2}'
		AND ('{3}' = '' OR T3.""ItemCode"" >= '{3}')
		AND ('{4}' = '' OR T3.""ItemCode"" <= '{4}')
	)
GROUP BY ""Filial"", ""Data"" ,""ItemCode"";
", filial, dataDe, dataAte, itemCodeIni, itemCodeFim);

                oDT_OP_Agrupado = Class.Conexao.ExecuteSqlDataTable(sql);
                */

                /*
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(sql);

                //Matrix0.Clear();
                oRec.MoveFirst();
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    linha = i + 1;
                    Matrix0.FlushToDataSource();

                    Matrix0.AddRow();
                    ((EditText)Matrix0.Columns.Item("cFilialGr").Cells.Item(linha).Specific).Value = oRec.Fields.Item("Filial").Value.ToString();
                    ((EditText)Matrix0.Columns.Item("cDeposito").Cells.Item(linha).Specific).Value = oRec.Fields.Item("BPLName").Value.ToString();
                    ((EditText)Matrix0.Columns.Item("cDtEntrega").Cells.Item(linha).Specific).Value = oRec.Fields.Item("Data").Value.ToString();
                    ((EditText)Matrix0.Columns.Item("cItemCode").Cells.Item(linha).Specific).Value = oRec.Fields.Item("ItemCode").Value.ToString();
                    ((EditText)Matrix0.Columns.Item("cInsumo").Cells.Item(linha).Specific).Value = oRec.Fields.Item("ItemName").Value.ToString();
                    ((EditText)Matrix0.Columns.Item("cQuant").Cells.Item(linha).Specific).Value = oRec.Fields.Item("Quant").Value.ToString();
                    //oRec.Fields.Item("AbsId").Value.ToString();
                    //oMatrix.LoadFromDataSource();
                    oRec.MoveNext();
                }
                //Matrix0.AutoResizeColumns();
                //oForm.Freeze(false);
                //this.UIAPIRawForm.Freeze(false);
                */

                processado = false;
                this.UIAPIRawForm.Refresh();
            }
            catch (Exception ex)
            {
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }


        //==================================================================================================================================//
        private void Button4_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            BubbleEvent = true;
            //FormInput activeForm = new FormInput();
            //activeForm.ShowDialog();
        }

        //==================================================================================================================================//
        private void Button5_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            string sql, chave = "", warehouse, aux, msg = "";
            int absEntry, absID_OFCT;
            DateTime data;

            BubbleEvent = true;

            try
            {
                try { chave = ComboBox1.Selected.Value.ToString(); } catch { }
                if (chave == "")
                {
                    Class.Conexao.sbo_application.MessageBox("Nenhuma recomendação selecionada");
                    return;
                }

                sql = string.Format(@"SELECT ""PurchReq"" FROM OMSN WHERE ""MsnCode"" = '{0}'", chave);
                aux = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                if (aux != "Y")
                {
                    Class.Conexao.sbo_application.MessageBox("Recomendação já processada");
                    return;
                }

                sql = string.Format(@"SELECT ""FCTAbs"" FROM OMSN WHERE ""MsnCode"" = '{0}'", chave);
                Int32.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out absID_OFCT);

                sql = string.Format(@"SELECT ""AbsEntry"" FROM OMSN WHERE ""MsnCode"" = '{0}'", chave);
                Int32.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out absEntry);

                sql = string.Format(@"
SELECT ""DueDate"", ""Warehouse""
FROM ORCM
WHERE ""OrderType"" = 'R'
    AND ""ObjAbs"" = {0}
GROUP BY ""DueDate"", ""Warehouse""
", absEntry);

                System.Data.DataTable oDT = Class.Conexao.ExecuteSqlDataTable(sql);
                if (oDT.Rows.Count > 0)
                {
                    msg = ValidarRecomendacao(oDT);

                    if (string.IsNullOrEmpty(msg))
                    {
                        Class.Conexao.oCompany.StartTransaction();
                        foreach (System.Data.DataRow linha in oDT.Rows)
                        {
                            data = Convert.ToDateTime(linha["DueDate"].ToString());
                            warehouse = linha["Warehouse"].ToString();

                            InserePedCompra(absEntry, data, warehouse, chave, absID_OFCT);
                        }
                        SetarPurchReq(absEntry);
                        Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }
                    else
                        throw new Exception(msg);
                }
                Class.Conexao.sbo_application.MessageBox("Recomendações processadas");
            }
            catch (Exception ex)
            {
                if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }

        //==================================================================================================================================//
        private string ValidarRecomendacao(System.Data.DataTable oDT)
        //==================================================================================================================================//
        {
            string msg = "", warehouse, sql;
            int cont;

            foreach (System.Data.DataRow linha in oDT.Rows)
            {
                warehouse = linha["Warehouse"].ToString();

                sql = string.Format(@"SELECT COUNT(1) FROM OBPL WHERE ""DflWhs"" = '{0}' ", warehouse);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                if (cont == 0)
                    msg = "Depósito " + warehouse + " não está 'amarrado' a nenhuma filial";
                else if (cont > 1)
                    msg = "Depósito " + warehouse + " 'amarrado' a mais de uma filial";

            }

            return msg;
        }

        //==================================================================================================================================//
        private string ValidarConsolidacao(string xfilial)
        //==================================================================================================================================//
        {
            string msg = "", warehouse, sql, bplName = "", cardCode;
            int nFilial, cont;

            Int32.TryParse(xfilial, out nFilial);
            if (nFilial == 0)
                msg = xfilial + " não é um número válido";

            if (string.IsNullOrEmpty(msg))
            {
                sql = string.Format(@"SELECT COUNT(1) FROM OBPL WHERE ""BPLId"" = {0} ", nFilial);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                if (cont == 0)
                    msg = "Filial de código (BPLId) " + xfilial + " não encontrada no SAP";
                else
                {
                    sql = string.Format(@"SELECT ""BPLName"" FROM OBPL WHERE ""BPLId"" = {0} ", nFilial);
                    bplName = Class.Conexao.ExecuteSqlScalar(sql).ToString();

                    sql = string.Format(@"SELECT IFNULL(""DflWhs"", '') FROM OBPL  WHERE ""BPLId"" = {0} ", nFilial);
                    warehouse = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                    if (string.IsNullOrEmpty(warehouse))
                        msg = "Depósito padrão não configurado para a filial " + bplName;
                }
            }

            if (string.IsNullOrEmpty(msg))
            {
                // ************************** TROCAR @CVA_CAR_CONF PARA @CVA_CAR_CONFIG
                sql = string.Format(@"
SELECT COUNT(1)
FROM ""@CVA_CAR_CONF"" T0
    INNER JOIN ""@CVA_CAR_CONF1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
WHERE T0.""U_BPLId"" = {0}
", xfilial);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                if (cont == 0)
                    msg = "Filial " + bplName + " ainda não configurada em 'Configuração de Parâmetros' do addon";
            }

            if (string.IsNullOrEmpty(msg))
            {
                sql = string.Format(@"SELECT IFNULL(""U_CVA_Codigo2PN"", '') AS ""CardCode"" FROM OBPL  WHERE ""BPLId"" = {0} ", nFilial);
                cardCode = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                if (string.IsNullOrEmpty(cardCode))
                    msg = "Configuração do código do cliente para a filial " + bplName + " em branco";
            }

            return msg;
        }

        //==================================================================================================================================//
        private void SetarPurchReq(int absEntry)
        //==================================================================================================================================//
        {
            string sql;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"UPDATE OMSN SET ""PurchReq"" = 'N' WHERE ""AbsEntry"" = {0} ", absEntry);
                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void InserePedCompra(int absEntry, DateTime data, string warehouse, string chave, int absID_OFCT)
        //==================================================================================================================================//
        {
            string sql, sData, aux, sErrMsg, newDocEntry;
            int bplId, i = 0, erro, iErrCode;
            DateTime docDate;

            try
            {
                sData = data.ToString("yyyy-MM-dd");

                sql = string.Format(@"SELECT ""BPLId"" FROM OBPL WHERE ""DflWhs"" = '{0}' ", warehouse);
                aux = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                Int32.TryParse(aux, out bplId);

                if (bplId == 0)
                    throw new Exception ("Filial para o depósito " + warehouse + " não identificada");

               sql = string.Format(@"
SELECT ""DocEntry"", ""ItemCode"", ""DueDate"", ""ReleasDate"", ""DocDate"", ""Quantity"", ""Warehouse""
FROM ORCM
WHERE ""OrderType"" = 'R'
    AND ""ObjAbs"" = {0}
    AND ""DueDate"" = '{1}'
    AND ""Warehouse"" = '{2}'
", absEntry, sData, warehouse);

                System.Data.DataTable oDT = Class.Conexao.ExecuteSqlDataTable(sql);
                if (oDT.Rows.Count > 0)
                {
                    docDate = Convert.ToDateTime(oDT.Rows[0]["DocDate"].ToString());
                    SAPbobsCOM.Documents oOrders = (SAPbobsCOM.Documents)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);

                    oOrders.BPL_IDAssignedToInvoice = bplId;
                    oOrders.DocDate = docDate;
                    oOrders.DocDueDate = data;
                    oOrders.TaxDate = docDate;
                    oOrders.RequriedDate = data;
                    oOrders.Comments = "Consolidação de Planejamento - Recomendação " + chave;
                    oOrders.UserFields.Fields.Item("U_IdPrevisao").Value = absID_OFCT.ToString();

                    foreach (System.Data.DataRow linha in oDT.Rows)
                    {
                        if (i > 0)
                            oOrders.Lines.Add();

                        oOrders.Lines.ItemCode = linha["ItemCode"].ToString();
                        oOrders.Lines.RequiredDate = data;
                        oOrders.Lines.Quantity = Convert.ToDouble(linha["Quantity"].ToString());
                        oOrders.Lines.WarehouseCode = warehouse;

                        i++;
                    }
                    erro = oOrders.Add();

                    if (erro != 0)
                    {
                        Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                        throw new Exception(sErrMsg);
                    }
                    else
                    {
                        newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EditText EditText7;
        private Button Button6;

        //==================================================================================================================================//
        private void Button6_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            DateTime dataRef = new DateTime(2020, 4, 1);
            string msg = "";

            BubbleEvent = true;

            Grafico grafico = new Grafico();
            msg = grafico.GerarGrafico(dataRef, 0, 0, 0);

            if (!string.IsNullOrEmpty(msg))
                Class.Conexao.sbo_application.MessageBox(msg);
            else
                Class.Conexao.sbo_application.MessageBox("Processamento concluído");
        }




        //==================================================================================================================================//
        private void Button6_ClickBefore_old(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        //==================================================================================================================================//
        {
            string arqOut = "";

            BubbleEvent = true;

            
            try
            {
                this.Button6.Caption = "Processando......";
                this.GetItem("btnGrafico").Enabled = false;
                this.GetItem("btnGrafico").Refresh();

                arqOut = "Planilha.xlsx";
                FileInfo newFile = new FileInfo(arqOut);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(arqOut);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    //txtMsg.Text = "Exportando os dados para a planilha"; txtMsg.Refresh();
                    CriarPlanilha(package);

                    package.Save();
                }
                Class.Conexao.sbo_application.MessageBox("Processamento concluído");
            }
            catch (Exception ex)
            {
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
            finally
            {
                this.Button6.Caption = "Gráfico";
                this.GetItem("btnGrafico").Enabled = true;
                this.GetItem("btnGrafico").Refresh();
            }
        }

        //==================================================================================================================================//
        private void CriarPlanilha(ExcelPackage package)
        //==================================================================================================================================//
        {
            int numLinha, dia;
            string aba1 = "Planilha", vigencia;
            double soma;

            try
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add(aba1);
                //FormataPlanilhaInicial(worksheet, caso, numMeses);
                numLinha = 3;
                vigencia = "202004";     //this.EditText7.Value;

                System.Data.DataTable oDT = RetornaDtGrafico(vigencia);
               
                if (oDT.Rows.Count > 0)
                {
                    sheet.Cells[numLinha-1, 1].Value = "Dia";
                    sheet.Cells[numLinha-1, 2].Value = "Padrão";
                    sheet.Cells[numLinha-1, 3].Value = "Planejado";

                    foreach (System.Data.DataRow linha in oDT.Rows)
                    {
                        dia = Convert.ToDateTime(linha["DataPlanej"].ToString()).Day;
                        Double.TryParse(linha["Custo"].ToString(), out soma);

                        sheet.Cells[numLinha, 1].Value = dia;
                        if (soma > 0)
                        {
                            sheet.Cells[numLinha, 2].Value = (soma - 1) > 0 ? (soma - 1) : 0;
                            sheet.Cells[numLinha, 3].Value = soma;
                        }
                        numLinha++;
                    }
                }

                //var range = sheet.Cells["A3"].LoadFromText(Utils.GetFileInfo(csvDir, "Sample9-1.txt", false), format, TableStyles.Medium27, true);
                var range = sheet.Cells["A2:C33"];
                //var range = sheet.Cells[1, 1, 1, 5]

                //var tbl = sheet.Tables[0];

                var dateStyle = package.Workbook.Styles.CreateNamedStyle("TableDate");
                dateStyle.Style.Numberformat.Format = "YYYY-MM";
                var numStyleDia = package.Workbook.Styles.CreateNamedStyle("TableDIa");
                numStyleDia.Style.Numberformat.Format = "#,##0";
                var numStyleCusto = package.Workbook.Styles.CreateNamedStyle("TableCusto");
                numStyleCusto.Style.Numberformat.Format = "#,##0.00";



                var tbl = sheet.Tables.Add(range.Offset(0, 0, range.End.Row - range.Start.Row + 1, range.End.Column - range.Start.Column + 1), "Table");

                tbl.ShowTotal = true;
                tbl.Columns[0].TotalsRowLabel = "Média";
                tbl.Columns[0].DataCellStyleName = "TableDIa";           //"TableDate";
                tbl.Columns[1].TotalsRowFunction = RowFunctions.Average;    //      RowFunctions.Sum;
                tbl.Columns[1].DataCellStyleName = "TableCusto";
                tbl.Columns[2].TotalsRowFunction = RowFunctions.Average;
                tbl.Columns[2].DataCellStyleName = "TableCusto";

                var chart = sheet.Drawings.AddChart("chart1", eChartType.ColumnClustered3D);
                chart.Title.Text = "Custo de Projeção do Cardápio para FEV/2020";
                chart.XAxis.Title.Text = "Dia";
                chart.YAxis.Title.Text = "R$";

                //Column3D -> not implemented
                chart.SetPosition(10, 330);
                chart.SetSize(800, 600);

                //Create one series for each column...
                for (int col = 1; col < 3; col++)
                {
                    var ser = chart.Series.Add(range.Offset(1, col, range.End.Row - 1, 1), range.Offset(1, 0, range.End.Row - 1, 1));
                    ser.HeaderAddress = range.Offset(0, col, 1, 1);
                }

                chart.Style = eChartStyle.Style26;      //eChartStyle.Style27;
                sheet.View.ShowGridLines = false;
                sheet.Calculate();
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();







                /*
                //Add a Line series
                var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineStacked);
                chartType2.UseSecondaryAxis = true;
                var serie3 = chartType2.Series.Add(range.Offset(1, 2, range.End.Row - 1, 1), range.Offset(1, 0, range.End.Row - 1, 1));
                serie3.Header = "Items in stock";

                //By default the secondary XAxis is not visible, but we want to show it...
                chartType2.XAxis.Deleted = false;
                chartType2.XAxis.TickLabelPosition = eTickLabelPosition.High;

                //Set the max value for the Y axis...
                chartType2.YAxis.MaxValue = 50;

                chart.Style = eChartStyle.Style26;
                sheet.View.ShowGridLines = false;
                sheet.Calculate();
                */

                /*
                ExcelPackage pck = new ExcelPackage();
                ExcelRange r1, r2;

                var sheet1 = pck.Workbook.Worksheets.Add("data_sheet");
                var sheet2 = pck.Workbook.Worksheets.Add("chart_sheet");
                var chart = (OfficeOpenXml.Drawing.Chart.ExcelBarChart)sheet2.Drawings.AddChart("some_name", OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
                chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Right;
                chart.Legend.Add();
                chart.SetPosition(1, 0, 1, 0);
                chart.SetSize(600, 400);
                chart.DataLabel.ShowValue = true;

                r1 = sheet1.Cells["A3:A10"];
                r2 = sheet1.Cells["B3:B10"];
                chart.Series.Add(r2, r1);

                chart.Style = OfficeOpenXml.Drawing.Chart.eChartStyle.Style21;
                chart.Title.Text = "Some title";
                chart.XAxis.Title.Text = "X axis name";
                chart.YAxis.Title.Text = "Y axis name";
                */




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private System.Data.DataTable RetornaDtGrafico(string vigencia)
        //==================================================================================================================================//
        {
            string sql, sDataIni, sDataFim;
            DateTime dtAux;
            System.Data.DataTable oDT = null;

            try
            {
                sDataIni = vigencia + "01";
                dtAux = DateTime.ParseExact(sDataIni, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                dtAux = dtAux.AddMonths(1);
                sDataFim = dtAux.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT 
	ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
    , SUM(T1.""U_CVA_CUSTO_ITEM"") AS ""Custo""
FROM ""@CVA_PLANEJAMENTO"" T0
    INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T0.""Code"" = T1.""U_CVA_PLAN_ID""
WHERE ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{0}'
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) < '{1}'
GROUP BY ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1)
", sDataIni, sDataFim);
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oDT;
        }


        //==================================================================================================================================//
        private string ValidarFiltrosX(List<int> filiais)
        //==================================================================================================================================//
        {
            string dataDe, dataAte, msg = "";

            try
            {
                dataDe = this.EditText0.Value.Trim();
                dataAte = this.EditText1.Value.Trim();

                if (filiais.Count == 0)
                    msg += "Nenhuma filial selecionada" + Environment.NewLine;

                if ((string.IsNullOrEmpty(dataDe)) || (string.IsNullOrEmpty(dataAte)))
                    msg += "Data inicial ou final não preenchida" + Environment.NewLine;

                if (string.IsNullOrEmpty(msg))
                    if ((Convert.ToDateTime(dataDe.Substring(0, 4) + "-" + dataDe.Substring(4, 2) + "-" + dataDe.Substring(6, 2))) > Convert.ToDateTime(dataAte.Substring(0, 4) + "-" + dataAte.Substring(4, 2) + "-" + dataAte.Substring(6, 2)))
                        msg += "Data inicial posterior à data final" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                msg += ex.Message + Environment.NewLine;
            }

            return msg;
        }



        /*
        private void Button1_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            string dataDe = this.EditText0.Value;
            string dataAte = this.EditText1.Value;
            string parametro = "";
            if (!string.IsNullOrEmpty(dataDe) && !string.IsNullOrEmpty(dataAte))
            {
                parametro = $@" c.""DueDate"" >='{dataDe}'AND c.""DueDate"" <='{dataAte}'
                                AND";
            }

            string usuarioLogado = Conexao.oCompany.UserName;
            string sql = $@"SELECT DISTINCT
            this.Grid0.DataTable.ExecuteQuery(sql);
            SAPbouiCOM.EditTextColumn oCol = (EditTextColumn)this.Grid0.Columns.Item(0);
            oCol.LinkedObjectType = "13";

            oCol = (EditTextColumn)this.Grid0.Columns.Item(5);
            oCol.LinkedObjectType = "2";

        }

        private void Button2_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Conexao.sbo_application.MessageBox("Cancelar selecionados?", 2, "Sim", "Não") == 1)
            {
                var formCancelar = new ValeGasCancelar();
                formCancelar.Show();

                var form = Conexao.sbo_application.Forms.Item(pVal.FormUID);
                var grid = (Grid)form.Items.Item("grd").Specific;
                ValeGasCancelar.GridVale = grid;
               
            }
        }


        */

    }
}
