//using Addon.CVA.View.Apetit.Cardapio.Helpers;
//using CVA.View.Apetit.Cardapio.Helpers;
//using SAPbouiCOM;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Globalization;
//using System.Threading;

//namespace CVA.View.Apetit.Cardapio.View
//{
//    public class PlanejamentoCardapioForm_BAK : BaseForm
//    {
//        //Campos da tabela
//        public const string TB_IdCliente = "CVA_ID_CLIENTE";
//        public const string TB_ClienteDes = "CVA_DES_CLIENTE";
//        public const string TB_IdContrato = "CVA_ID_CONTRATO";
//        public const string TB_IDModeloCardapio = "CVA_ID_MODEL_CARD";
//        public const string TB_DesModeloCardapio = "CVA_DES_MODELO_CARD";
//        public const string TB_IdGrpServico = "CVA_GRPSERVICO";
//        public const string TB_DesGrpServico = "CVA_DES_GRPSERVICO";
//        public const string TB_Vigencia = "CVA_VIGENCIA_CONTR";
//        public const string TB_CustoPadrao = "CVA_CUSTO_PADRAO";
//        public const string TB_DataRef = "CVA_DATA_REF";
//        public const string TB_DiaSemana = "CVA_DIA_SEMANA";
//        public const string TB_QtdComensais = "CVA_QTD_COMENSAIS";
//        public const string TB_Tot_CustoMedio = "CVA_TOT_C_MEDIO";
//        public const string TB_Tot_CustoPadrao = "CVA_TOT_C_PADRAO";
//        public const string TB_Tot_VariacaoValor = "CVA_TOT_VARIACAO_V";
//        public const string TB_Tot_VariacaoPercent = "CVA_TOT_VARIACAO_P";

//        public const string CH_ModeloLinId = "CVA_MODELO_LIN_ID";
//        public const string CH_TipoPrato = "CVA_TIPO_PRATO";
//        public const string CH_DesTipoPrato = "CVA_TIPO_PRATO_DES";
//        public const string CH_Insumo = "CVA_INSUMO";
//        public const string CH_InsumoDes = "CVA_INSUMO_DES";
//        public const string CH_Percent = "CVA_PERCENT";
//        public const string CH_QTD = "CVA_QTD";
//        public const string CH_QTD_ORI = "CVA_QTD_ORI";
//        public const string CH_CustoMedio = "CVA_CUSTO_MEDIO";
//        public const string CH_Total = "CVA_TOTAL";

//        public PlanejamentoCardapioForm_BAK()
//        {
//            MatrixItens = "mtxGrps";
//            Type = "CARDPLAN";
//            TableName = "CVA_PLANEJAMENTO";
//            ChildName = "CVA_LN_PLANEJAMENTO";
//            MenuItem = Type;
//            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
//            IdToEvaluateGridEmpty = "it_CTpP";

//            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
//        }

//        public override void CreateUserFields()
//        {
//            var userFields = new Helpers.UserFields();

//            UserTables.CreateIfNotExist(TableName, "[CVA] Plan. de Cadápio", SAPbobsCOM.BoUTBTableType.bott_MasterData);
//            userFields.CreateIfNotExist("@" + TableName, TB_IdCliente, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_IdContrato, "Id Contrato", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_IDModeloCardapio, "Id Modelo", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_IdGrpServico, "Id Serviço", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_Vigencia, "Vigência", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_CustoPadrao, "Custo Padrão", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_DataRef, "Data de Refer.", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_DiaSemana, "Dia Semana", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_QtdComensais, "Qtd. Comensais", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_ClienteDes, "Descr. Cliente", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_DesModeloCardapio, "Descr. Modelo Card", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_DesGrpServico, "Descr. Grupo Ser.", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

//            userFields.CreateIfNotExist("@" + TableName, TB_Tot_CustoMedio, "Custo Médio Total", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_Tot_CustoPadrao, "Custo Total", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
//            userFields.CreateIfNotExist("@" + TableName, TB_Tot_VariacaoValor, "Variação Valor", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + TableName, TB_Tot_VariacaoPercent, "Variação %", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, SAPbobsCOM.BoYesNoEnum.tYES);

//            UserTables.CreateIfNotExist(ChildName, "[CVA] Ln Plan. de Cad.", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
//            userFields.CreateIfNotExist("@" + ChildName, CH_TipoPrato, "Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_DesTipoPrato, "Descr. Tipo de prato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_Insumo, "Id Insumo", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_InsumoDes, "Descr. Insumo", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
//            userFields.CreateIfNotExist("@" + ChildName, CH_Percent, "%", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_QTD, "Qtd.", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_QTD_ORI, "Qtd. Original", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity);
//            userFields.CreateIfNotExist("@" + ChildName, CH_CustoMedio, "Custo Médio", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_Total, "Total", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
//            userFields.CreateIfNotExist("@" + ChildName, CH_ModeloLinId, "Id Lin Model", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

//            #region UDOs 

//            if (!UserObjects.Exists(Type))
//            {
//                userFields.CreateUserObject(Type, "[CVA] Plan. de Cadápio", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
//                userFields.AddChildTableToUserObject(Type, ChildName);
//            }

//            #endregion
//        }

//        internal override void LoadDefault(Form oForm)
//        {
//            //oForm.Freeze(true);
//            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
//            CreateChooseFromList(f);
//            Filters.Add(oForm.TypeEx, BoEventTypes.et_CLICK);
//            Filters.Add(oForm.TypeEx, BoEventTypes.et_RIGHT_CLICK);
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

//        public static List<string> diasSemana = new List<string>
//        {
//            "Domingo",
//            "Segunda-Feira",
//            "Terça-Feira",
//            "Quarta-Feira",
//            "Quinta-Feira",
//            "Sexta-Feira",
//            "Sábado",
//        };

//        public bool _alreadyChanging = false;
//        public bool _validatorExecControl = false;
//        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
//        {
//            var ret = true;

//            try
//            {
//                if (pVal.FormTypeEx.Equals(Type))
//                {
//                    if (pVal.BeforeAction)
//                    {
//                        #region Contrato CHOOSE

//                        if (pVal.ItemUID.Equals("edtContrS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                        {
//                            var oForm = Application.Forms.Item(pVal.FormUID);
//                            string idContrato = ((EditText)oForm.Items.Item("edtContrS").Specific).Value.ToString();
//                            if (!string.IsNullOrEmpty(idContrato))
//                            {
//                                string code = "";
//                                string endDate = "";
//                                string idCard = "";

//                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                                rec.DoQuery($@"
//                                        SELECT 
//                                         {"BpCode".Aspas()},
//                                         {"EndDate".Aspas()},
//                                         {"U_CVA_ID_MCARDAPIO".Aspas()}
//                                        FROM {"OOAT".Aspas()}
//                                        WHERE {"Number".Aspas()} = {idContrato}");

//                                if (rec.RecordCount > 0)
//                                {
//                                    code = rec.Fields.Item("BpCode").Value.ToString();
//                                    endDate = rec.Fields.Item("EndDate").Value.ToString();
//                                    idCard = rec.Fields.Item("U_CVA_ID_MCARDAPIO").Value.ToString();
//                                }
//                                else
//                                {
//                                    Application.SetStatusBarMessage("Contrato não encontrado...");
//                                    bubbleEvent = false;
//                                    return;
//                                }

//                                if (!string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(idCard))
//                                {
//                                    DateTime dtTime;
//                                    if (DateTime.TryParseExact(endDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dtTime))
//                                        ((IEditText)oForm.Items.Item("edtContrV").Specific).Value = dtTime.ToString("yyyyMMdd");

//                                    rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                                    rec.DoQuery($@"
//                                        SELECT 
//                                         * 
//                                        FROM {"@CVA_MCARDAPIO".Aspas()} as c
//                                        WHERE c.{"Code".Aspas()} = '{idCard}'
//                                    ;");

//                                    ((IEditText)oForm.Items.Item("edtMdlS").Specific).Value = idCard;
//                                    if (rec.RecordCount > 0)
//                                    {
//                                        var edtMdlD = rec.Fields.Item("U_CVA_DESCRICAO").Value;
//                                        ((IEditText)oForm.Items.Item("edtMdlD").Specific).Value = edtMdlD.ToString();
//                                        /*
                                     

//        private void CarregarTipoPrato(string idCard)
//        {
//            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//            rec.DoQuery($@"
//                            SELECT 
//	                            ML.{"Code".Aspas()},
//                                ML.{"LineId".Aspas()},
//                                ML.{"U_CVA_TIPO_PRATO".Aspas()},
//                                ML.{"U_CVA_TIPO_PRATO_DES".Aspas()}
//                            FROM {"@CVA_MCARDAPIO".Aspas()} AS M
//                            INNER JOIN {"@CVA_LIN_MCARDAPIO".Aspas()} AS ML ON
//                                ML.{"Code".Aspas()} = M.{"Code".Aspas()}
//                            WHERE M.{"Code".Aspas()} = '{idCard}'
//            ;");

//            while (rec.EoF)
//            {

//            }
//        }    
//                                     */
//                                    }
//                                }
//                            }
//                        }

//                        #endregion

//                        #region Tipo Prato CHOOSE
//                        if (pVal.ItemUID.Equals(MatrixItens))
//                        {
//                            var oForm = Application.Forms.Item(pVal.FormUID);
//                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

//                            oForm.Items.Item("btnRLinha").Enabled = mtx.GetNextSelectedRow() >= 0;
//                        }

//                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("it_CTpP") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                        {
//                            var oForm = Application.Forms.Item(pVal.FormUID);
//                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
//                            var it_CTpP = ((IEditText)mtx.Columns.Item("it_CTpP").Cells.Item(pVal.Row).Specific).Value;

//                            SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                            rec.DoQuery($@"
//                                    SELECT 
//	                                    * 
//                                    FROM {"@CVA_LIN_MCARDAPIO".Aspas()}
//                                    WHERE {"LineId".Aspas()} = {it_CTpP}
//                                    ");

//                            if (rec.RecordCount > 0)
//                            {
//                                var name = rec.Fields.Item("U_CVA_TIPO_PRATO_DES").Value.ToString();
//                                var codePrato = rec.Fields.Item("U_CVA_TIPO_PRATO").Value.ToString();
//                                ((IEditText)mtx.Columns.Item("it_tpPrr").Cells.Item(pVal.Row).Specific).Value = name;
//                                ((IEditText)mtx.Columns.Item("it_CLM").Cells.Item(pVal.Row).Specific).Value = codePrato;
//                            }
//                            else
//                            {
//                                Application.SetStatusBarMessage("Tipo de prato não encontrado...");
//                                bubbleEvent = false;
//                                _alreadyChanging = false;
//                                return;
//                            }
//                        }

//                        #endregion

//                        #region Insumo CHOOSE

//                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_edtIn") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                        {
//                            if (!_alreadyChanging)
//                            {
//                                _alreadyChanging = true;
//                                var oForm = Application.Forms.Item(pVal.FormUID);
//                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
//                                var l_edtIn = ((IEditText)mtx.Columns.Item("l_edtIn").Cells.Item(pVal.Row).Specific).Value;
//                                var cliId = ((IEditText)oForm.Items.Item("edtCliS").Specific).Value;

//                                if (!string.IsNullOrEmpty(l_edtIn))
//                                {
//                                    var dtRefStr = ((IEditText)oForm.Items.Item("edtDtRef").Specific).Value;
//                                    var edtContrS = ((IEditText)oForm.Items.Item("edtContrS").Specific).Value;
//                                    var edtGrpS = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;
//                                    DateTime dtRef;

//                                    if (string.IsNullOrEmpty(cliId) || string.IsNullOrEmpty(edtContrS) || string.IsNullOrEmpty(edtGrpS) || !DateTime.TryParseExact(dtRefStr, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dtRef))
//                                    {
//                                        Application.MessageBox("A data de referência deve ser escolhida após o cliente, contrato e o grupo de serviço");
//                                        bubbleEvent = false;
//                                        return;
//                                    }

//                                    //Procura por itens bloquados
//                                    SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                                    rec.DoQuery($@"
//                                    SELECT 
//	                                    * 
//                                    FROM {"@CVA_BLOQUEN".Aspas()} AS B
//                                        INNER JOIN {"@CVA_LIN_BLOQUEN".Aspas()} AS LB ON
//                                            B.{ "Code".Aspas()} = LB.{ "Code".Aspas()}
//                                    WHERE LB.{ "U_CVA_ITEMCODE".Aspas()} = '{l_edtIn}'
//                                        ;
//                                    ");

//                                    if (rec.RecordCount > 0)
//                                    {
//                                        Application.MessageBox($"O insumo está bloqueado para uso.");
//                                        ((IEditText)mtx.Columns.Item("l_edtIn").Cells.Item(pVal.Row).Specific).Value = "";
//                                        bubbleEvent = false;
//                                        _alreadyChanging = false;
//                                        return;
//                                    }

//                                    var itemResults = ObterDadosItem(l_edtIn, dtRef, edtContrS, edtGrpS, cliId);

//                                    if (string.IsNullOrEmpty(itemResults.ItemName))
//                                    {
//                                        Application.SetStatusBarMessage("Insumo não encontrado...");
//                                        bubbleEvent = false;
//                                        _alreadyChanging = false;
//                                        return;
//                                    }

//                                    ((IEditText)mtx.Columns.Item("l_edtInD").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemName;
//                                    ((IEditText)mtx.Columns.Item("l_edtPer").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemPercent;
//                                    ((IEditText)oForm.Items.Item("edtDSem").Specific).Value = itemResults.DiaSemana;
//                                    ((IEditText)mtx.Columns.Item("l_edt_qtd").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemQtd.ToString();
//                                    ((IEditText)mtx.Columns.Item("l_qtdOri").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemQtd.ToString();
//                                    ((IEditText)mtx.Columns.Item("l_edt_cmed").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemCustoMedio?.ToString().Replace(",", ".");
//                                    ((IEditText)mtx.Columns.Item("l_edt_to").Cells.Item(pVal.Row).Specific).Value = itemResults.ItemTotal.ToString().Replace(",", ".");
//                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue("U_CVA_TOTAL", pVal.Row, itemResults.ItemTotal.ToString().Replace(",", "."));
//                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue("U_CVA_PERCENT", pVal.Row, itemResults.ItemPercent);
//                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue("U_CVA_QTD", pVal.Row, itemResults.ItemQtd.ToString());

//                                    AtualizarTotalizadores(oForm);
//                                }
//                                _alreadyChanging = false;
//                            }
//                        }

//                        #endregion

//                        #region data referencia VALIDATE

//                        if (pVal.ItemUID.Equals("edtDtRef") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                        {
//                            var oForm = Application.Forms.Item(pVal.FormUID);
//                            var dtRefStr = ((IEditText)oForm.Items.Item("edtDtRef").Specific).Value;
//                            DateTime dtRef;
//                            var edtContrS = ((IEditText)oForm.Items.Item("edtContrS").Specific).Value;
//                            var edtGrpS = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;

//                            if (string.IsNullOrEmpty(edtContrS) || string.IsNullOrEmpty(edtGrpS))
//                            {
//                                Application.MessageBox("A data de referência deve ser escolhida após o contrato e o grupo de serviço");
//                                bubbleEvent = false;
//                                return;
//                            }

//                            if (DateTime.TryParseExact(dtRefStr, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dtRef))
//                            {
//                                //procurar por dia sem consumo
//                                var dSemana = diasSemana[(int)dtRef.DayOfWeek];

//                                SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                                rec.DoQuery($@"
//                                    SELECT 
//	                                    * 
//                                    FROM {"@CVA_PLANEJAMENTO".Aspas()}
//                                    WHERE   { "U_CVA_DATA_REF".Aspas()}     = '{dtRefStr}'
//	                                    AND { "U_CVA_ID_CONTRATO".Aspas()}  = '{edtContrS}'
//	                                    AND { "U_CVA_GRPSERVICO".Aspas()}   = '{edtGrpS}'
//                                    ;
//                                ");

//                                if (rec.RecordCount > 0)
//                                {
//                                    Application.MessageBox($"A data de referência, para este Contrato / Serviço já esta sendo utilizada.");
//                                    ((IEditText)oForm.Items.Item("edtDtRef").Specific).Value = "";
//                                    bubbleEvent = false;
//                                    return;
//                                }

//                                rec.DoQuery($@"
//                                    SELECT 
//	                                    * 
//                                    FROM {"@CVA_CALENDSC".Aspas()} AS C
//                                        INNER JOIN {"@CVA_LIN_CALENDSC".Aspas()} AS CL ON
//                                            C.{"Code".Aspas()} = CL.{"Code".Aspas()}
//                                    WHERE C.{ "U_CVA_ID_CONTRATO".Aspas()} = {edtContrS}
//                                    AND C.{ "U_CVA_GRPSERVICO".Aspas()} = '{edtGrpS}'
//                                    AND CL.{ "U_CVA_DATA".Aspas()} = '{dtRefStr}'
//                                    ;
//                                ");

//                                if (rec.RecordCount > 0)
//                                {
//                                    var motivo = rec.Fields.Item("U_CVA_MOTIVO").Value;
//                                    Application.MessageBox($"A data de referência esta bloqueada por motivo de '{motivo}'.");
//                                    ((IEditText)oForm.Items.Item("edtDtRef").Specific).Value = "";
//                                    bubbleEvent = false;
//                                    return;
//                                }

//                                ((IEditText)oForm.Items.Item("edtDSem").Specific).Value = dSemana;

//                                rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//                                rec.DoQuery($@"
//                                    SELECT 
//	                                     SUM(lc.{"U_CVA_SEGUNDA".Aspas()}) as U_CVA_SEGUNDA
//                                        ,SUM(lc.{"U_CVA_TERCA".Aspas()}) as U_CVA_TERCA
//                                        ,SUM(lc.{"U_CVA_QUARTA".Aspas()}) as U_CVA_QUARTA
//                                        ,SUM(lc.{"U_CVA_QUINTA".Aspas()}) as U_CVA_QUINTA
//                                        ,SUM(lc.{"U_CVA_SEXTA".Aspas()}) as U_CVA_SEXTA
//                                        ,SUM(lc.{"U_CVA_SABADO".Aspas()}) as U_CVA_SABADO
//                                        ,SUM(lc.{"U_CVA_DOMINGO".Aspas()}) as U_CVA_DOMINGO
//	                                    ,{"U_CVA_C_PADRAO".Aspas()}
//                                    FROM {"@CVA_COMENSAIS".Aspas()} as c
//                                        INNER JOIN {"@CVA_LIN_COMENSAIS".Aspas()} as lc on
//                                             lc.{"Code".Aspas()} = c.{"Code".Aspas()}
//                                    WHERE    c.{"U_CVA_ID_CONTRATO".Aspas()} = {edtContrS}
//                                        AND  c.{"U_CVA_GRPSERVICO".Aspas()} = '{edtGrpS}'      
// 	                                GROUP By c.{"Code".Aspas()}, c.{"U_CVA_C_PADRAO".Aspas()}
//                                ");

//                                if (rec.RecordCount > 0)
//                                {
//                                    var edtQtdCom = 0;
//                                    var edtCustoP = rec.Fields.Item("U_CVA_C_PADRAO").Value;

//                                    switch (dtRef.DayOfWeek)
//                                    {
//                                        case DayOfWeek.Sunday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_DOMINGO").Value;
//                                            break;
//                                        case DayOfWeek.Monday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SEGUNDA").Value;
//                                            break;
//                                        case DayOfWeek.Tuesday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_TERCA").Value;
//                                            break;
//                                        case DayOfWeek.Wednesday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_QUARTA").Value;
//                                            break;
//                                        case DayOfWeek.Thursday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_QUINTA").Value;
//                                            break;
//                                        case DayOfWeek.Friday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SEXTA").Value;
//                                            break;
//                                        case DayOfWeek.Saturday:
//                                            edtQtdCom = (dynamic)rec.Fields.Item("U_CVA_SABADO").Value;
//                                            break;
//                                    }

//                                    ((IEditText)oForm.Items.Item("edtQtdCom").Specific).Value = edtQtdCom.ToString();
//                                    ((IEditText)oForm.Items.Item("edtCustoP").Specific).Value = edtCustoP.ToString().Replace(",", ".");
//                                }
//                            }
//                            else
//                            {
//                                ((IEditText)oForm.Items.Item("edtDSem").Specific).Value = "";
//                            }
//                        }

//                        #endregion
//                    }

//                    if (!pVal.BeforeAction)
//                    {
//                        var oForm = Application.Forms.Item(pVal.FormUID);

//                        var cliId = ((IEditText)oForm.Items.Item("edtCliS").Specific).Value.ToString();
//                        string idContrato = ((EditText)oForm.Items.Item("edtContrS").Specific).Value.ToString();
//                        string idGrupo = ((EditText)oForm.Items.Item("edtGrpS").Specific).Value.ToString();

//                        #region Remover Linha de Planejamento (CLICK)
                        
//                        if (pVal.ItemUID.Equals("btnRLinha") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && oForm.Items.Item("btnRLinha").Enabled)
//                        {
//                            //REMOVER LINHA SELECIONADA
//                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
//                            var index = mtx.GetNextSelectedRow();
//                            if (index >= 0)
//                            {
//                                mtx.DeleteRow(index);

//                                AtualizarTotalizadores(oForm);
//                                oForm.Mode = BoFormMode.fm_UPDATE_MODE;
//                            }
//                        }
//                        #endregion

//                        #region Mapa de Planejamento (CLICK)
//                        if (pVal.ItemUID.Equals("btnVisuMPl") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && oForm.Items.Item("btnVisuMPl").Enabled)
//                        {
//                            var menuItem = B1Connection.Instance.Application.Menus.Item("43520");
//                            if (menuItem.SubMenus.Count > 0)
//                            {
//                                for (int i = 0; i < menuItem.SubMenus.Count; i++)
//                                    if (menuItem.SubMenus.Item(i).String.Contains("[CVA] GRADE DE PLANEJAMENTO DIÁRIO DE REFEIÇÕES"))
//                                    {
//                                        menuItem.SubMenus.Item(i).Activate();
//                                        PreencherCamposReport(cliId, idContrato, idGrupo);
//                                        break;
//                                    }
//                            }
//                        }
//                        #endregion

//                        #region Grupo de Serviço CHOOSE

//                        if (pVal.ItemUID.Equals("edtGrpS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
//                        {
//                            var iChoose = (ChooseFromListEvent)pVal;
//                            DataTable dataTable = iChoose.SelectedObjects;

//                            if (dataTable != null && dataTable.Rows.Count > 0)
//                            {
//                                string itemCode = dataTable.GetValue("Code", 0).ToString();
//                                string itemName = dataTable.GetValue("U_CVA_DESCRICAO", 0).ToString();

//                                ((IEditText)oForm.Items.Item("edtGrpD").Specific).Value = itemName;
//                            }
//                        }

//                        #endregion

//                        #region Cliente CHOOSE

//                        if (pVal.ItemUID.Equals("edtCliS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
//                        {
//                            var iChoose = (ChooseFromListEvent)pVal;
//                            DataTable dataTable = iChoose.SelectedObjects;

//                            if (dataTable != null && dataTable.Rows.Count > 0)
//                            {
//                                string itemCode = dataTable.GetValue("CardCode", 0).ToString();
//                                string itemName = dataTable.GetValue("CardName", 0).ToString();

//                                ((IEditText)oForm.Items.Item("edtCliD").Specific).Value = itemName;
//                            }
//                        }

//                        #endregion

//                        #region Calculo % ou QTD
//                        var valueChanged = false;
//                        if (!_validatorExecControl && !_alreadyChanging)
//                        {
//                            _validatorExecControl = true;
//                            if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_edtPer") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                            {
//                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

//                                var str_qtdOri = ((IEditText)mtx.Columns.Item("l_qtdOri").Cells.Item(pVal.Row).Specific).Value;
//                                var str_perc = ((IEditText)mtx.Columns.Item("l_edtPer").Cells.Item(pVal.Row).Specific).Value;

//                                var qtdOri = 0d;
//                                var percNum = 0d;

//                                if (double.TryParse(str_qtdOri.Replace(".", ","), out qtdOri) && double.TryParse(str_perc.Replace(".", ","), out percNum))
//                                {
//                                    var perc = percNum / 100;
//                                    int qtd = (int)Math.Round(qtdOri * perc, MidpointRounding.AwayFromZero);
//                                    if (qtd <= 0) qtd = 1;

//                                    ((IEditText)mtx.Columns.Item("l_edt_qtd").Cells.Item(pVal.Row).Specific).Value = qtd.ToString();
//                                    valueChanged = true;
//                                }
//                            }

//                            if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_edt_qtd") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                            {
//                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

//                                var str_qtdOri = ((IEditText)mtx.Columns.Item("l_qtdOri").Cells.Item(pVal.Row).Specific).Value;
//                                var str_qtd = ((IEditText)mtx.Columns.Item("l_edt_qtd").Cells.Item(pVal.Row).Specific).Value;

//                                var qtdOri = 0d;
//                                var qtd = 0d;

//                                if (double.TryParse(str_qtdOri.Replace(".", ","), out qtdOri) && double.TryParse(str_qtd.Replace(".", ","), out qtd))
//                                {
//                                    var perc = (qtd / qtdOri);
//                                    if (perc <= 0) perc = 1;

//                                    ((IEditText)mtx.Columns.Item("l_edtPer").Cells.Item(pVal.Row).Specific).Value = perc.ToString("N2");
//                                    ((IEditText)mtx.Columns.Item("l_edt_cmed").Cells.Item(pVal.Row).Specific).Value = ((IEditText)mtx.Columns.Item("l_edt_cmed").Cells.Item(pVal.Row).Specific).Value;
//                                    valueChanged = true;
//                                }
//                            }

//                            if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_edt_cmed") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && pVal.ItemChanged)
//                            {
//                                valueChanged = true;
//                            }

//                            if (valueChanged)
//                            {
//                                valueChanged = false;

//                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

//                                var str_qtd = ((IEditText)mtx.Columns.Item("l_edt_qtd").Cells.Item(pVal.Row).Specific).Value;
//                                var str_qmed = ((IEditText)mtx.Columns.Item("l_edt_cmed").Cells.Item(pVal.Row).Specific).Value;

//                                var qmed = 0d;
//                                var qtd = 0d;

//                                if (double.TryParse(str_qmed.Replace(".", ","), out qmed) && double.TryParse(str_qtd.Replace(".", ","), out qtd))
//                                {
//                                    var total = (qtd * qmed);
//                                    var vtotal = total.ToString("N2").Replace(".", "").Replace(",", ".");

//                                    ((IEditText)mtx.Columns.Item("l_edt_to").Cells.Item(pVal.Row).Specific).Value = vtotal;
//                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue("U_CVA_TOTAL", pVal.Row, vtotal);
//                                }

//                                //Totalizadores
//                                AtualizarTotalizadores(oForm);
//                            }

//                            _validatorExecControl = false;
//                        }

//                        #endregion  
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex.HResult != -2000)
//                {
//                    Application.SetStatusBarMessage(ex.Message);
//                    ret = false;
//                }
//                _alreadyChanging = false;
//                _validatorExecControl = false;
//            }

//            bubbleEvent = ret;
//        }

//        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
//        {
//            var ret = true;
//            var oApplication = B1Connection.Instance.Application;
//            var oForm = oApplication.Forms.Item(eventInfo.FormUID);

//            if (oForm.TypeEx.Equals(TYPEEX) && eventInfo.ItemUID.Equals(MatrixItens))
//                oApplication.Menus.Item("1283").Enabled = !eventInfo.BeforeAction;


//            bubbleEvent = ret;
//        }

//        private void PreencherCamposReport(string cliId, string idContrato, string idGrupo)
//        {
//            Thread.Sleep(200);
//            var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
//            ((IEditText)oForm.Items.Item("1000003").Specific).Value = cliId;
//            ((IEditText)oForm.Items.Item("1000009").Specific).Value = idContrato;
//            ((IEditText)oForm.Items.Item("1000015").Specific).Value = idGrupo;
//            oForm.Items.Item("1").Click();
//        }

//        public override void SetFilters()
//        {
//            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);
//        }

//        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
//        {
//            var ret = true;

//            try
//            {
//                if (BusinessObjectInfo.FormTypeEx.Equals(Type) && BusinessObjectInfo.BeforeAction)
//                {
//                    var oForm = Application.Forms.ActiveForm;
//                    oForm.Items.Item("btnVisuMPl").Enabled = BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_LOAD);

//                    if (BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_LOAD))
//                    {
//                        var strVariacao = ((IEditText)oForm.Items.Item("edtVValorV").Specific).Value;
//                        var variacao = 0d;
//                        if (!string.IsNullOrEmpty(strVariacao) && double.TryParse(strVariacao, out variacao))
//                            ChangeColor(oForm, variacao);
//                    }

//                    if ((BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_UPDATE) || BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD)))
//                    {
//                        string nextCode;

//                        if (BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD) || BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_UPDATE))
//                        {
//                            nextCode = DIHelper.GetNextCode(TableName).ToString();
//                            oForm.DataSources.DBDataSources.Item("@" + TableName).SetValue("Code", 0, nextCode);
//                            oForm.DataSources.DBDataSources.Item("@" + TableName).SetValue("Name", 0, nextCode);
//                        }
//                        else
//                            nextCode = oForm.DataSources.DBDataSources.Item("@" + TableName).GetValue("Code", 0).ToString();
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

//        public override void SetMenus()
//        {
//            Helpers.Menus.Add("CVAPCARD", MenuItem, "Plan. Cardápio", 2, BoMenuType.mt_STRING);
//        }

//        public void CreateChooseFromList(Form oForm)
//        {
//            #region Tipo Prato
//            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

//            string strSql = $@"
//                                SELECT 
//                                     c.{"LineId".Aspas()}
//                                    ,c.{"U_CVA_TIPO_PRATO_DES".Aspas()}
//                                FROM {"@CVA_LIN_MCARDAPIO".Aspas()} as c
//                                WHERE c.{ "Code".Aspas()} = RTRIM(LTRIM($[$edtMdlS.0])) ;
//                            ";

//            FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Prato Planejamento", idCategoria, Type, MatrixItens, "it_CTpP");
//            #endregion

//            #region Busca insumo
//            /*
//             SELECT 
//	            *
//            FROM OITM as o
//            WHERE 
//            "ItemCode" NOT IN(SELECT 
//				            lb."U_CVA_ITEMCODE" 
//			            FROM "@CVA_BLOQUEN" as b 
//				            INNER JOIN "@CVA_LIN_BLOQUEN" AS lb on
//					            b."Code" = lb."Code"
//			            WHERE b."U_CVA_ID_CONTRATO" = 1
//            )
//            AND "U_CVA_Planejar" = 'Y'
//            AND (SELECT "U_CVA_Proteina" FROM "@CVA_TIPOPRATO" WHERE "Code" = 1) = "U_CVA_Proteina"
//            AND ("U_CVA_Proteina" = 'N' OR 
//		            ("U_CVA_Proteina" = 'Y' 
//			            AND	EXISTS(
//				            SELECT * FROM "@CVA_TABGRAMATURA" as t 
//				            WHERE t."U_CVA_GRAMATURA" = o."U_CVA_GRAMATURA" 
//				            AND t."U_CVA_TIPO_PROTEINA" = o."U_CVA_ID_TIPO_PROT"
//			            )
//		            )
//	            );
//             */

//            strSql = $@"
//                        SELECT 
//	                        o.{"ItemCode".Aspas()},
//                            o.{"ItemName".Aspas()}
//                        FROM OITM as o
//                        WHERE {"ItemCode".Aspas()} 
//                            NOT IN(SELECT lb.{"U_CVA_ITEMCODE".Aspas()}
//                                    FROM {"@CVA_BLOQUEN".Aspas()} as b
//                                        INNER JOIN {"@CVA_LIN_BLOQUEN".Aspas()} AS lb on
//                                            b.{"Code".Aspas()} = lb.{"Code".Aspas()}
//                                    WHERE b.{"U_CVA_ID_CONTRATO".Aspas()} = RTRIM(LTRIM($[$edtContrS.0]))
//                        )
//                        AND {"U_CVA_Planejar".Aspas()} = 'Y'
//                        AND(SELECT {"U_CVA_PROTEINA".Aspas()} FROM {"@CVA_TIPOPRATO".Aspas()} WHERE {"Code".Aspas()} = RTRIM(LTRIM($[$mtxGrps.it_CLM.0]))) = {"U_CVA_Proteina".Aspas()}
//                        AND({ "U_CVA_Proteina".Aspas()} = 'N' OR
//                                ({ "U_CVA_Proteina".Aspas()} = 'Y'
//                                    AND EXISTS(
//                                        SELECT * FROM { "@CVA_TABGRAMATURA".Aspas()} as t
//                                        WHERE t.{ "U_CVA_GRAMATURA".Aspas()} = o.{ "U_CVA_GRAMATURA".Aspas()}
//                                        AND t.{ "U_CVA_TIPO_PROTEINA".Aspas()} = o.{ "U_CVA_ID_TIPO_PROT".Aspas()}
//                                    )
//                                )
//                            );
//            ";

//            FormatedSearch.CreateFormattedSearches(strSql, "Busca Insumo", idCategoria, Type, MatrixItens, "l_edtIn");
//            #endregion

//            #region Busca Contrato
//            strSql = $@"
//                        SELECT 
//	                        o.{"Number".Aspas()},
//                            o.{"BpCode".Aspas()},
//                            o.{"BpName".Aspas()},
//                            o.{"StartDate".Aspas()},
//                            o.{"EndDate".Aspas()}
//                        FROM OOAT as o
//                        WHERE {"BpCode".Aspas()} = RTRIM(LTRIM($[$edtCliS.0]))
//            ;";

//            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Contrato Plan.", idCategoria, Type, "U_CVA_ID_CONTRATO", "-1");
//            #endregion
//        }


//        #region Internal Helpers

//        private void AtualizarTotalizadores(Form oForm)
//        {
//            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
//            //double mediaCustoTot = 0;
//            double total = 0;
//            var qtdCom = ((IEditText)oForm.Items.Item("edtQtdCom").Specific).Value;

//            for (int i = 1; i <= mtx.RowCount; i++)
//            {
//                var linTot = ((IEditText)mtx.Columns.Item("l_edt_to").Cells.Item(i).Specific).Value;
//                double totParsed = 0;
//                if (double.TryParse(linTot.Replace(".", ","), out totParsed))
//                    total += totParsed;
//            }

//            ((IEditText)oForm.Items.Item("edtCustoMT").Specific).Value = total.ToString();

//            var qtdC = 0;
//            var cPadrao = 0d;

//            var strcPadrao = ((IEditText)oForm.Items.Item("edtCustoP").Specific).Value;
//            if (int.TryParse(qtdCom, out qtdC) && double.TryParse(strcPadrao.Replace(".", ","), out cPadrao) && total > 0)
//            {
//                //var mediaCusto = total / qtdC;
//                //var variacao = cPadrao - mediaCusto;
//                //var perc = ((mediaCusto / cPadrao) * 100) - 100;
//                var t = ObterTotalizadores(total, qtdC, cPadrao);

//                ((IEditText)oForm.Items.Item("edtMCusto").Specific).Value = t.mediaCusto.ToString("N2").Replace(",", ".");
//                ((IEditText)oForm.Items.Item("edtVValorV").Specific).Value = t.variacao.ToString("N2").Replace(",", ".");
//                ((IEditText)oForm.Items.Item("edtVValorP").Specific).Value = t.perc.ToString("N2").Replace(",", ".");

//                ChangeColor(oForm, t.variacao);
//            }
//        }

//        public static dynamic ObterTotalizadores(double totalLinhas, int qtdComensais, double custoPadrao)
//        {
//            var mediaCusto = totalLinhas / qtdComensais;
//            var variacao = custoPadrao - mediaCusto;
//            var perc = ((mediaCusto / custoPadrao) * 100) - 100;

//            return new
//            {
//                mediaCusto,
//                variacao,
//                perc
//            };
//        }

//        private void ChangeColor(Form oForm, double variacao)
//        {
//            var color = Color.Green;

//            if (variacao < 0)
//                color = Color.Red;

//            oForm.Items.Item("edtVValorV").ForeColor = ColorTranslator.ToOle(color);
//            oForm.Items.Item("edtVValorP").ForeColor = ColorTranslator.ToOle(color);

//        }

//        public static ItemResultsBAK ObterDadosItem(string itemCode, DateTime dtRef, string idContrato, string idServico, string cliId)
//        {
//            var ret = new ItemResultsBAK
//            {
//                DiaSemana = diasSemana[(int)dtRef.DayOfWeek]
//            };

//            #region Obter Item
//            SAPbobsCOM.Recordset rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
//            rec.DoQuery($@"
//                            SELECT 
//	                            * 
//                            FROM {"OITM".Aspas()}
//                            WHERE {"ItemCode".Aspas()} = '{itemCode}'
//            ;");

//            if (rec.RecordCount > 0)
//            {
//                ret.ItemName = rec.Fields.Item("ItemName").Value.ToString();
//                ret.ItemPercent = "100";
//            }
//            #endregion

//            #region Obter Comensais
//            rec.DoQuery($@"
//                        SELECT 
//	                          SUM(lc.{"U_CVA_SEGUNDA".Aspas()}) AS U_CVA_SEGUNDA
//                            , SUM(lc.{"U_CVA_TERCA".Aspas()}) AS U_CVA_TERCA
//                            , SUM(lc.{"U_CVA_QUARTA".Aspas()}) AS U_CVA_QUARTA
//                            , SUM(lc.{"U_CVA_QUINTA".Aspas()}) AS U_CVA_QUINTA
//                            , SUM(lc.{"U_CVA_SEXTA".Aspas()}) AS U_CVA_SEXTA
//                            , SUM(lc.{"U_CVA_SABADO".Aspas()}) AS U_CVA_SABADO
//                            , SUM(lc.{"U_CVA_DOMINGO".Aspas()}) AS U_CVA_DOMINGO
//                            , C.{"U_CVA_C_PADRAO".Aspas()}
//                        FROM {"@CVA_COMENSAIS".Aspas()} as c
//                            INNER JOIN {"@CVA_LIN_COMENSAIS".Aspas()} as lc on
//                                    lc.{"Code".Aspas()} = c.{"Code".Aspas()}
//                        WHERE    c.{"U_CVA_ID_CONTRATO".Aspas()} = {idContrato}
//                            AND  c.{"U_CVA_GRPSERVICO".Aspas()} = {idServico}   
//                        GROUP BY  
//	                        {"U_CVA_C_PADRAO".Aspas()}   
//            ; ");

//            if (rec.RecordCount > 0)
//            {
//                var edtQtdCom = 0;
//                switch (dtRef.DayOfWeek)
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

//                ret.ItemQtd = edtQtdCom.ToString();
//            }
//            #endregion

//            #region Obter Custos

//            //primeiro acessa combo (Estrutura)
//            rec.DoQuery($@"
//                    SELECT 
//	                    ROUND(SUM(I1.{"Quantity".Aspas()} * I1.{ "Price".Aspas()}),2) AS { "AvgPrice".Aspas() }
//                    FROM OITT AS O 
//                    INNER JOIN ITT1 AS I1 ON
//                        I1.{"Father".Aspas()} = O.{"Code".Aspas()}
//                    WHERE
//                        O.{"Code".Aspas()} = '{itemCode}'
//                    GROUP BY
//                        O.{"Code".Aspas()}
//                ; ");

//            if (rec.RecordCount > 0)
//                ret.ItemCustoMedio = rec.Fields.Item("AvgPrice").Value.ToString();
//            else
//            {
//                //caso sem estrutura, procura estoque
//                rec.DoQuery($@"
//                    SELECT 
//	                    W.{"AvgPrice".Aspas()} AS {"AvgPrice".Aspas()}
//                    FROM OCRD AS O
//	                    INNER JOIN OBPL AS B ON
//		                    O.{"U_CVA_FILIAL".Aspas()} = B.{"BPLId".Aspas()}
//	                    INNER JOIN OITW AS W ON
//		                    B.{"DflWhs".Aspas()} = W.{"WhsCode".Aspas()}
//                    WHERE 
//		                    O.{ "CardCode".Aspas()} = '{cliId}'
//	                    AND W.{ "ItemCode".Aspas()} = '{itemCode}'
//                ; ");

//                if (rec.RecordCount > 0)
//                    ret.ItemCustoMedio = rec.Fields.Item("AvgPrice").Value.ToString();
//                else
//                {
//                    //caso sem estoque, procura ultima compra
//                    rec.DoQuery($@"
//                        SELECT
//	                        OITM.{"LastPurPrc".Aspas()} AS {"AvgPrice".Aspas()}
//                        FROM OITM
//                        WHERE {"ItemCode".Aspas()} = '{itemCode}'
//                    ; ");

//                    if (rec.RecordCount > 0)
//                        ret.ItemCustoMedio = rec.Fields.Item("AvgPrice").Value.ToString();
//                }
//            }

//            #endregion
//            return ret;
//        }
//        #endregion  
//    }

//    public class ItemResultsBAK
//    {
//        public string ItemName { get; set; }
//        public string DiaSemana { get; set; }
//        public string ItemPercent { get; set; }
//        public string ItemQtd { get; set; }
//        public double IntItemQtd
//        {
//            get
//            {
//                double parseQtd = 0;
//                double.TryParse(ItemQtd, out parseQtd);
//                return parseQtd;
//            }
//        }
//        public double IntItemCustoMedio
//        {
//            get
//            {
//                double parsePrice = 0;
//                double.TryParse(ItemCustoMedio, out parsePrice);
//                return parsePrice;
//            }
//        }
//        public string ItemCustoMedio { get; set; }
//        public string ItemTotal
//        {
//            get
//            {
//                var ret = 0d;
//                var parseQtd = IntItemQtd;
//                double parsePrice = IntItemCustoMedio;

//                if (parseQtd > 0 && parsePrice > 0)
//                    ret = parsePrice * parseQtd;

//                return ret.ToString();
//            }
//        }
//    }
//}