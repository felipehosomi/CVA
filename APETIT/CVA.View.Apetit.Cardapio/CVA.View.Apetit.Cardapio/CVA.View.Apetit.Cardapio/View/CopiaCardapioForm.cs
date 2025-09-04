//using Addon.CVA.View.Apetit.Cardapio.Helpers;
//using CVA.View.Apetit.Cardapio.Helpers;
//using CVA.View.Apetit.Cardapio.Model;
//using SAPbouiCOM;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;

//namespace CVA.View.Apetit.Cardapio.View
//{
//    public class CopiaCardapioForm : BaseForm
//    {
//        //Campos da tabela
//        public const string TB_Descricao = "CVA_DESCRICAO";
//        public const string CH_TipoPrato = "CVA_TIPO_PRATO";
//        public const string CH_DesTipoPrato = "CVA_TIPO_PRATO_DES";

//        public CopiaCardapioForm()
//        {
//            Type = "CARDCOPY";
//            MenuItem = Type;
//            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
//        }

//        public override void CreateUserFields()
//        {
//        }

//        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
//        {
//            var ret = true;
//            bubbleEvent = ret;
//        }

//        internal override void LoadDefault(Form oForm)
//        {
//            Filters.Add(oForm.TypeEx, BoEventTypes.et_CLICK);
//            //oForm.Freeze(true);
//            //var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
//            //CreateChooseFromList(f);
//            //oForm.Freeze(false);
//        }

//        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
//        {
//            var ret = true;
//            var openMenu = OpenMenu(MenuItem, FilePath, pVal);

//            if (!string.IsNullOrEmpty(openMenu))
//            {
//                ret = false;
//                Application.SetStatusBarMessage(openMenu);
//            }

//            bubbleEvent = ret;
//        }

//        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
//        {
//            var ret = true;

//            try
//            {
//                if (pVal.FormTypeEx.Equals(TYPEEX))
//                {
//                    if (!pVal.BeforeAction)
//                    {
//                        if (pVal.ItemUID.Equals("btnGerar") && pVal.EventType.Equals(BoEventTypes.et_CLICK))
//                        {
//                            var oForm = Application.Forms.Item(pVal.FormUID);

//                            try
//                            {
//                                oForm.Freeze(true);
//                                if(GerarCopia(Application))
//                                    B1Connection.Instance.Application.MessageBox($"Dados Copiados com sucesso !");
//                            }
//                            catch (Exception ex)
//                            {
//                                Application.MessageBox(ex.Message);
//                            }finally
//                            {
//                                oForm.Freeze(false);
//                            }
//                        }
//                    }

//                    if (pVal.BeforeAction)
//                    {
//                        var oForm = Application.Forms.Item(pVal.FormUID);

//                        var edtDCtrS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtDCtrS").Specific).Value);
//                        var edtDCliS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtDCliS").Specific).Value);
//                        var edtDDataS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtDDataS").Specific).Value);
//                        var edtPDataSA = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtPDataSA").Specific).Value);
//                        var edtPCtrS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtPCtrS").Specific).Value);
//                        var edtPCliS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtPCliS").Specific).Value);
//                        var edtPDataS = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtPDataS").Specific).Value);
//                        var edtDDataSA = !string.IsNullOrEmpty(((IEditText)oForm.Items.Item("edtDDataSA").Specific).Value);
//                        var enabledGerar = edtDCtrS &
//                                           edtDCliS &
//                                           edtDDataS &
//                                           edtPCtrS &
//                                           edtPCliS &
//                                           edtDDataSA &
//                                           edtPDataSA &
//                                           edtPDataS;

//                        oForm.Items.Item("btnGerar").Enabled = enabledGerar;

//                        #region Contrato CHOOSE DE

//                        if ((pVal.ItemUID.Equals("edtDCtrS") || pVal.ItemUID.Equals("edtPCtrS")) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                        {
//                            var edtCrt = pVal.ItemUID.Equals("edtDCtrS") ? "edtDCtrS" : "edtPCtrS";
//                            var edtCliS = pVal.ItemUID.Equals("edtDCtrS") ? "edtDCliS" : "edtPCliS";
//                            var edtCliD = pVal.ItemUID.Equals("edtDCtrS") ? "edtDCliD" : "edtPCliD";
//                            string code = "";
//                            string name = "";

//                            string idContrato = ((EditText)oForm.Items.Item(edtCrt).Specific).Value.ToString();
//                            if (!string.IsNullOrEmpty(idContrato))
//                            {
//                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

//                                rec.DoQuery($@"
//                                        SELECT 
//                                          {"BpCode".Aspas()}
//                                         ,{"BpName".Aspas()}
//                                        FROM {"OOAT".Aspas()}
//                                        WHERE {"Number".Aspas()} = {idContrato}");

//                                if (rec.RecordCount > 0)
//                                {
//                                    code = rec.Fields.Item("BpCode").Value.ToString();
//                                    name = rec.Fields.Item("BpName").Value.ToString();
//                                }

//                                if (!string.IsNullOrEmpty(code))
//                                {
//                                    ((IEditText)oForm.Items.Item(edtCliS).Specific).Value = code.ToString();
//                                    ((IEditText)oForm.Items.Item(edtCliD).Specific).Value = name.ToString();
//                                }
//                            }
//                        }

//                        #endregion

//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Application.SetStatusBarMessage(ex.Message);
//                ret = false;
//            }

//            bubbleEvent = ret;
//        }

//        private bool GerarCopia(Application Application)
//        {
//            var oForm = Application.Forms.ActiveForm;
//            var diDate = DIHelper.Format_StringToDate(((EditText)oForm.Items.Item("edtDDataS").Specific).Value.ToString());

//            Application.SetStatusBarMessage("Validando dados de contrato...", BoMessageTime.bmt_Medium, false);

//            #region Dados Contrato
//            var ContratoDe = new
//            {
//                ID = ((EditText)oForm.Items.Item("edtDCtrS").Specific).Value.ToString(),
//                CLIENTE = ((EditText)oForm.Items.Item("edtDCliS").Specific).Value.ToString(),
//                DE = DIHelper.Format_StringToDate(((EditText)oForm.Items.Item("edtDDataS").Specific).Value),
//                ATE = DIHelper.Format_StringToDate(((EditText)oForm.Items.Item("edtDDataSA").Specific).Value)
//            };

//            var ContratoAte = new
//            {
//                ID = ((EditText)oForm.Items.Item("edtPCtrS").Specific).Value.ToString(),
//                CLIENTE = ((EditText)oForm.Items.Item("edtPCliS").Specific).Value.ToString(),
//                DE = DIHelper.Format_StringToDate(((EditText)oForm.Items.Item("edtPDataS").Specific).Value),
//                ATE = DIHelper.Format_StringToDate(((EditText)oForm.Items.Item("edtPDataSA").Specific).Value)
//            };
//            #endregion

//            #region Valida Contrato De - Até (dados de entrada)
//            //valida dia da semana, devem ser identicos
//            if (ContratoDe.DE.DayOfWeek != ContratoAte.DE.DayOfWeek)
//                throw new Exception($"O dia da semana da data de origem difere do dia da semana da data de destino, verifique e tente novamente.");

//            //valida se a quantidade de dias é igual
//            if (ContratoDe.ATE.Subtract(ContratoDe.DE).TotalDays != ContratoAte.ATE.Subtract(ContratoAte.DE).TotalDays)
//                throw new Exception($"A quantidade de dias de origem difere da quantidade de dias de destino, verifique e tente novamente.");

//            //verificar vigencia
//            var verificaDe = VerificaVigenciaContrato(ContratoDe.ID, ContratoDe.DE.ToString("yyyyMMdd"), ContratoDe.ATE.ToString("yyyyMMdd"));
//            var verificaAte = VerificaVigenciaContrato(ContratoAte.ID, ContratoAte.DE.ToString("yyyyMMdd"), ContratoAte.ATE.ToString("yyyyMMdd"));
//            if (!verificaDe || !verificaAte)
//                throw new Exception($"O contrato de {(verificaDe ? "origem" : "destino")} está fora de vigência, verifique e tente novamente.");
//            #endregion

//            #region Copiar Planejamento

//            //Obter dados do planejamento ORIGEM
//            var dadosPlanejamento = ObterDadosPlanejamento(ContratoDe.ID, ContratoDe.DE.ToString("yyyyMMdd"), ContratoDe.ATE.ToString("yyyyMMdd"));

//            //Obter modelo do planejamento DESTINO
//            var modeloLinhasDestino = ObterModeloLinhas(ContratoAte.ID);
//            var planejamentosAdicionar = new List<PlanejamentoData>();

//            //irá rodar os dias (dia a dia)
//            for (int i = 0; i <= ContratoDe.ATE.Subtract(ContratoDe.DE).TotalDays; i++)
//            {
//                var diaCorrenteDE = ContratoDe.DE.AddDays(i);
//                var diaCorrenteATE = ContratoAte.DE.AddDays(i);

//                //geralmente é 1 só, porem feito para funcionar com varios plan. no mesmo dia
//                var planejamentosDoDia = dadosPlanejamento.Where(x => x.U_CVA_DATA_REF.ToString("yyyyMMdd") == diaCorrenteDE.ToString("yyyyMMdd"));
//                var linhasToRemove = new List<PlanejamentoDataLinha>();

//                foreach (var plan in planejamentosDoDia)
//                {
//                    var modelosDestinoCopy = modeloLinhasDestino.ToList();
//                    var copy = plan.Copy();
//                    copy.U_CVA_DATA_REF = diaCorrenteATE;
//                    copy.U_CVA_ID_CONTRATO = ContratoAte.ID;
//                    copy.U_CVA_ID_CLIENTE = ContratoAte.CLIENTE;
//                    copy.U_CVA_ID_MODEL_CARD = modeloLinhasDestino.FirstOrDefault().Code;
//                    copy.U_CVA_DES_MODELO_CARD = modeloLinhasDestino.FirstOrDefault().U_CVA_DES_MODELO_CARD;

//                    foreach (var linhaPlan in copy.PlanejamentoLinhas)
//                    {
//                        var modeloOrigem = copy.ModeloLinhas.FirstOrDefault(x => x.LineId.ToString() == linhaPlan.U_CVA_MODELO_LIN_ID);
//                        var modeloSel = modelosDestinoCopy.FirstOrDefault(x => x.U_CVA_TIPO_PRATO == modeloOrigem.U_CVA_TIPO_PRATO);
//                        if (modeloSel == null)
//                        {
//                            linhasToRemove.Add(linhaPlan);
//                            continue;
//                        }

//                        modelosDestinoCopy.Remove(modeloSel);
//                    }

//                    foreach (var item in linhasToRemove)
//                        copy.PlanejamentoLinhas.Remove(item);

//                    planejamentosAdicionar.Add(copy);
//                }
//            }

//            return AdicionarPlanejamentosAoDestino(planejamentosAdicionar);

//            #endregion
//        }

//        private bool AdicionarPlanejamentosAoDestino(List<PlanejamentoData> planejamentosAdicionar)
//        {
//            var logCount = 1;
//            SAPbobsCOM.GeneralData oChild;
//            SAPbobsCOM.GeneralDataCollection oChildren;

//            var oCompService = B1Connection.Instance.Company.GetCompanyService();
//            B1Connection.Instance.Company.StartTransaction();

//            try
//            {
//                var nextCode = DIHelper.GetNextCode("CVA_PLANEJAMENTO");

//                foreach (var item in planejamentosAdicionar)
//                {
//                    B1Connection.Instance.Application.SetStatusBarMessage($"Copiando dados do contrato ({logCount}/{planejamentosAdicionar.Count})...", BoMessageTime.bmt_Medium, false);
//                    logCount++;

//                    var oGeneralService = oCompService.GetGeneralService("CARDPLAN");
//                    var oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

//                    var dadosAlterados = ObterDadosCopiarContrato(item.U_CVA_ID_CONTRATO, item.U_CVA_ID_CLIENTE, item.U_CVA_DATA_REF, item.U_CVA_GRPSERVICO);
//                    if (!dadosAlterados.Any())
//                        continue;

//                    var cabDados = dadosAlterados.FirstOrDefault();

//                    //Setting Data to Master Data Table Fields
//                    oGeneralData.SetProperty("Code", nextCode.ToString());
//                    oGeneralData.SetProperty("Name", nextCode.ToString());
//                    oGeneralData.SetProperty("U_CVA_ID_CLIENTE", item.U_CVA_ID_CLIENTE);
//                    oGeneralData.SetProperty("U_CVA_ID_CONTRATO", item.U_CVA_ID_CONTRATO);
//                    oGeneralData.SetProperty("U_CVA_ID_MODEL_CARD", item.U_CVA_ID_MODEL_CARD);
//                    oGeneralData.SetProperty("U_CVA_GRPSERVICO", item.U_CVA_GRPSERVICO);
//                    oGeneralData.SetProperty("U_CVA_VIGENCIA_CONTR", item.U_CVA_VIGENCIA_CONTR);

//                    oGeneralData.SetProperty("U_CVA_CUSTO_PADRAO", cabDados.U_CVA_CUSTO_PADRAO);

//                    oGeneralData.SetProperty("U_CVA_DATA_REF", item.U_CVA_DATA_REF);
//                    oGeneralData.SetProperty("U_CVA_DIA_SEMANA", item.U_CVA_DIA_SEMANA);
//                    oGeneralData.SetProperty("U_CVA_DES_CLIENTE", item.U_CVA_DES_CLIENTE);
//                    oGeneralData.SetProperty("U_CVA_DES_MODELO_CARD", item.U_CVA_DES_MODELO_CARD);
//                    oGeneralData.SetProperty("U_CVA_DES_GRPSERVICO", item.U_CVA_DES_GRPSERVICO);
//                    //oGeneralData.SetProperty("U_CVA_TOT_C", item.U_CVA_TOT_C);

//                    //Setting Data to Child Table Fields
//                    oChildren = oGeneralData.Child("CVA_LN_PLANEJAMENTO");

//                    double total = 0;
//                    double qtdComensais = 0d;
//                    var hasLine = false;

//                    foreach (var linha in item.PlanejamentoLinhas)
//                    {
//                        //var linDados = dadosAlterados.FirstOrDefault(x => x.U_CVA_INSUMO == linha.U_CVA_INSUMO);
//                        //if (linDados == null)
//                        //    continue;

//                        //qtdComensais += linDados.U_CVA_QTD_COMENSAIS;
//                        hasLine = true;
//                        oChild = oChildren.Add();
//                        oChild.SetProperty("U_CVA_TIPO_PRATO", linha.U_CVA_TIPO_PRATO);
//                        oChild.SetProperty("U_CVA_TIPO_PRATO_DES", linha.U_CVA_TIPO_PRATO_DES);
//                        oChild.SetProperty("U_CVA_INSUMO", linha.U_CVA_INSUMO);
//                        oChild.SetProperty("U_CVA_MODELO_LIN_ID", linha.U_CVA_MODELO_LIN_ID);

//                        var itemResults = PlanejamentoCardapioForm_BAK.ObterDadosItem(linha.U_CVA_INSUMO, item.U_CVA_DATA_REF, item.U_CVA_ID_CONTRATO, item.U_CVA_GRPSERVICO, item.U_CVA_ID_CLIENTE);
//                        var qtdOriginal = itemResults.IntItemQtd;
//                        qtdComensais = qtdOriginal;
//                        var percent = linha.U_CVA_PERCENT;
//                        var qtd = qtdOriginal * (percent / 100);
//                        var cMedio = itemResults.IntItemCustoMedio;
//                        var linTot = cMedio * qtd;

//                        oChild.SetProperty("U_CVA_INSUMO_DES", itemResults.ItemName);
//                        oChild.SetProperty("U_CVA_PERCENT", percent);

//                        //TODO: Calcular campos
//                        oChild.SetProperty("U_CVA_QTD_ORI", qtdOriginal.ToString());
//                        oChild.SetProperty("U_CVA_QTD", qtd.ToString());
//                        oChild.SetProperty("U_CVA_CUSTO_MEDIO", cMedio);
//                        oChild.SetProperty("U_CVA_TOTAL", linTot);

//                        total += linTot;
//                    }

//                    if (!hasLine)
//                        throw new Exception("Não foi encontrado nenhum item relativo ao modelo / grupo do contrato de destino");

//                    oGeneralData.SetProperty("U_CVA_QTD_COMENSAIS", qtdComensais.ToString());
//                    var t = PlanejamentoCardapioForm_BAK.ObterTotalizadores(total, (int)qtdComensais, cabDados.U_CVA_CUSTO_PADRAO);

//                    //TODO: Calcular os totais
//                    oGeneralData.SetProperty("U_CVA_TOT_C_MEDIO", t.mediaCusto);
//                    oGeneralData.SetProperty("U_CVA_TOT_C_PADRAO", total);
//                    oGeneralData.SetProperty("U_CVA_TOT_VARIACAO_V", t.variacao);
//                    oGeneralData.SetProperty("U_CVA_TOT_VARIACAO_P", t.perc);

//                    //Attempt to Add the Record
//                    oGeneralService.Add(oGeneralData);
//                    nextCode++;
//                }

//                if (B1Connection.Instance.Company.InTransaction)                
//                    B1Connection.Instance.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                if (B1Connection.Instance.Company.InTransaction)
//                    B1Connection.Instance.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
//            }
//        }

//        private List<dynamic> ObterDadosCopiarContrato(string idContrato, string idCliente, DateTime dateRef, string idServico)
//        {
//            SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//            List<dynamic> ret = new List<dynamic>();

//            rec.DoQuery($@"
//                    SELECT
//	                      LC.{"U_CVA_SERVICO".Aspas()} AS {"U_CVA_INSUMO".Aspas()}
//                        , C.{ "U_CVA_C_PADRAO".Aspas()}
//                        , LC.{ "U_CVA_SEGUNDA".Aspas()}
//                        , LC.{ "U_CVA_TERCA".Aspas()}
//                        , LC.{ "U_CVA_QUARTA".Aspas()}
//                        , LC.{ "U_CVA_QUINTA".Aspas()}
//                        , LC.{ "U_CVA_SEXTA".Aspas()}
//                        , LC.{ "U_CVA_SABADO".Aspas()}
//                        , LC.{ "U_CVA_DOMINGO".Aspas()}
//                    FROM {"@CVA_COMENSAIS".Aspas()} AS C
//                    INNER JOIN { "@CVA_LIN_COMENSAIS".Aspas()} AS LC ON
//		                    C.{ "Code".Aspas()} = LC.{ "Code".Aspas()}
//                    WHERE 
//		                    C.{ "U_CVA_ID_CONTRATO".Aspas()} = {idContrato}
//	                    AND	C.{ "U_CVA_GRPSERVICO".Aspas()} = '{idServico}'
//            ");

//            if (rec.RecordCount > 0)
//            {
//                #region qtdComensais e Custo padrao
//                var edtQtdCom = 0;
//                var edtCustoP = rec.Fields.Item("U_CVA_C_PADRAO").Value;

//                switch (dateRef.DayOfWeek)
//                {
//                    case DayOfWeek.Sunday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_DOMINGO").Value;
//                        break;
//                    case DayOfWeek.Monday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SEGUNDA").Value;
//                        break;
//                    case DayOfWeek.Tuesday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_TERCA").Value;
//                        break;
//                    case DayOfWeek.Wednesday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_QUARTA").Value;
//                        break;
//                    case DayOfWeek.Thursday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_QUINTA").Value;
//                        break;
//                    case DayOfWeek.Friday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SEXTA").Value;
//                        break;
//                    case DayOfWeek.Saturday:
//                        edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SABADO").Value;
//                        break;
//                }
//                #endregion

//                while (!rec.EoF)
//                {
//                    ret.Add(new
//                    {
//                        U_CVA_CUSTO_PADRAO = edtCustoP,
//                        U_CVA_QTD_COMENSAIS = edtQtdCom,
//                        U_CVA_INSUMO = rec.Fields.Item("U_CVA_INSUMO").Value.ToString(),
//                    });

//                    rec.MoveNext();
//                }
//            }

//            return ret;
//        }

//        /// <param name="contratoId"></param>
//        /// <param name="de">YYYYMMDD</param>
//        /// <param name="ate">YYYYMMDD</param>
//        /// <returns></returns>
//        private bool VerificaVigenciaContrato(string contratoId, string de, string ate)
//        {
//            SAPbobsCOM.Recordset rec = (dynamic)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

//            rec.DoQuery($@"
//                    SELECT 
//                        {"BpCode".Aspas()}
//                        ,{"BpName".Aspas()}
//                    FROM {"OOAT".Aspas()}
//                    WHERE {"Number".Aspas()} = {contratoId}
//                    AND TO_DATE('{de}', 'YYYYMMDD') between {"StartDate".Aspas()} AND {"EndDate".Aspas()}
//                    AND TO_DATE('{ate}', 'YYYYMMDD') between {"StartDate".Aspas()} AND {"EndDate".Aspas()}
//            ");

//            return rec.RecordCount > 0;
//        }

//        private List<PlanejamentoData> ObterDadosPlanejamento(string contratoId, string de, string ate)
//        {
//            var ret = new List<PlanejamentoData>();

//            SAPbobsCOM.Recordset rec = (dynamic)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

//            rec.DoQuery($@"
//                    SELECT 
//		                 P.{"Code".Aspas()}
//                        , P.{"U_CVA_ID_CLIENTE".Aspas()}
//                        , P.{"U_CVA_ID_CONTRATO".Aspas()}
//                        , P.{"U_CVA_ID_MODEL_CARD".Aspas()}
//                        , P.{"U_CVA_GRPSERVICO".Aspas()}
//                        , P.{"U_CVA_VIGENCIA_CONTR".Aspas()}
//                        , P.{"U_CVA_CUSTO_PADRAO".Aspas()}
//                        , P.{"U_CVA_DATA_REF".Aspas()}
//                        , P.{ "U_CVA_DIA_SEMANA".Aspas()}
//                        , P.{ "U_CVA_QTD_COMENSAIS".Aspas()}
//                        , P.{ "U_CVA_DES_CLIENTE".Aspas()}
//                        , P.{ "U_CVA_DES_MODELO_CARD".Aspas()}
//                        , P.{ "U_CVA_DES_GRPSERVICO".Aspas()}
//                        , P.{ "U_CVA_TOT_C_MEDIO".Aspas()}
//                        , P.{ "U_CVA_TOT_C_PADRAO".Aspas()}
//                        , P.{ "U_CVA_TOT_VARIACAO_V".Aspas()}
//                        , P.{ "U_CVA_TOT_VARIACAO_P".Aspas()}
//                        , P1.{ "Code".Aspas()} AS { "LINE_CODE".Aspas()}
//                        , P1.{ "LineId".Aspas()}
//                        , P1.{ "U_CVA_TIPO_PRATO".Aspas()}
//                        , P1.{ "U_CVA_TIPO_PRATO_DES".Aspas()}
//                        , P1.{ "U_CVA_INSUMO".Aspas()}
//                        , P1.{ "U_CVA_PERCENT".Aspas()}
//                        , P1.{ "U_CVA_QTD".Aspas()}
//                        , P1.{ "U_CVA_CUSTO_MEDIO".Aspas()}
//                        , P1.{ "U_CVA_TOTAL".Aspas()}
//                        , P1.{ "U_CVA_INSUMO_DES".Aspas()}
//                        , P1.{ "U_CVA_QTD_ORI".Aspas()}
//                        , P1.{ "U_CVA_MODELO_LIN_ID".Aspas()}

//                    FROM    {"@CVA_PLANEJAMENTO".Aspas()} AS P
//                        INNER JOIN {"@CVA_LN_PLANEJAMENTO".Aspas()} AS P1 ON
//                            P1.{"Code".Aspas()} = P.{"Code".Aspas()}
//                    WHERE   P.{ "U_CVA_ID_CONTRATO".Aspas()} = {contratoId}
//                        AND P.{ "U_CVA_DATA_REF".Aspas()} BETWEEN '{de}' AND '{ate}'
//                    ORDER BY P.{ "U_CVA_DATA_REF".Aspas()}
//            ;");


//            while (!rec.EoF)
//                ret.Add(new PlanejamentoData(rec));

//            foreach (var plan in ret)
//                plan.ModeloLinhas = ObterModeloLinhas(plan.U_CVA_ID_CONTRATO);

//            return ret;
//        }

//        private List<ModeloLinha> ObterModeloLinhas(string idContrato)
//        {
//            var ret = new List<ModeloLinha>();
//            SAPbobsCOM.Recordset rec = (dynamic)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

//            rec.DoQuery($@"
//                    SELECT * FROM {"@CVA_LIN_MCARDAPIO".Aspas()} AS L INNER JOIN OOAT AS O ON O.{"U_CVA_ID_MCARDAPIO".Aspas()} = L.{"Code".Aspas()} WHERE O.{"Number".Aspas()} = '{idContrato}'
//                ;");

//            while (!rec.EoF)
//            {
//                ret.Add(new ModeloLinha(rec));
//                rec.MoveNext();
//            }

//            return ret;
//        }

//        public override void SetFilters()
//        {
//            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);
//        }

//        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
//        {
//            var ret = true;

//            bubbleEvent = ret;
//        }

//        public override void SetMenus()
//        {
//            Helpers.Menus.Add("CVAPCARD", MenuItem, "Replicação de Cardápio", 3, BoMenuType.mt_STRING);
//        }
//    }
//}