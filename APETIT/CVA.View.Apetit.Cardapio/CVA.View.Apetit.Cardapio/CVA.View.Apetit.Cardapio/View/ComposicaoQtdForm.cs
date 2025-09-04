using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.Apetit.Cardapio.View
{
    public class ComposicaoQtdForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Contrato = "CVA_ID_CONTRATO";
        public const string TB_GrpServico = "CVA_GRPSERVICO";
        public const string TB_DesGrpServico = "CVA_DES_GRPSERVICO";
        public const string TB_CustoPadrao = "CVA_C_PADRAO";
        public const string TB_QtdMinFat = "CVA_QTD_MIN_FAT";

        //Campos da Linha
        public const string CH_Servico = "CVA_SERVICO";
        public const string CH_DesServico = "CVA_DES_S";
        public const string CH_Segunda = "CVA_SEGUNDA";
        public const string CH_Terca = "CVA_TERCA";
        public const string CH_Quarta = "CVA_QUARTA";
        public const string CH_Quinta = "CVA_QUINTA";
        public const string CH_Sexta = "CVA_SEXTA";
        public const string CH_Sabado = "CVA_SABADO";
        public const string CH_Domingo = "CVA_DOMINGO";
        public const string CH_DES_Turno = "CVA_DES_TURNO";

        //grid totais
        public const string MatrixTotal = "tot_mtx";

        public ComposicaoQtdForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDCOMQ";
            TableName = "CVA_COMENSAIS";
            ChildName = "CVA_LIN_COMENSAIS";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            IdToEvaluateGridEmpty = "l_Serv";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Comp. de Quantidade", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_GrpServico, "ID Grupo Serv.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_CustoPadrao, "Custo Padrão", 8, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_QtdMinFat, "Qtd. Min Fatura", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesGrpServico, "Descr. Grupo Ser.", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Linhas do Cadastro", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_Servico, "ID Serviço", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_DesServico, "Descr. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Segunda, "Segunda", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Terca, "Terça", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Quarta, "Quarta", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Quinta, "Quinta", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Sexta, "Sexta", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Sabado, "Sábado", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Domingo, "Domingo", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_DES_Turno, "Turno desc", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, "[CVA] Comp. de Quantidade", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
                userFields.AddChildTableToUserObject(Type, ChildName);
            }

            #endregion
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
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

            if (pVal.FormTypeEx.Equals(Type))
            {
                try
                {
                    if (!pVal.BeforeAction)
                    {
                        //choose service
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

                        //choose service Grid
                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("l_Serv") && pVal.EventType == BoEventTypes.et_VALIDATE && pVal.ItemChanged)
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var code = ((IEditText)oForm.Items.Item("edtGrpS").Specific).Value;
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            var l_Serv = ((IEditText)mtx.Columns.Item("l_Serv").Cells.Item(pVal.Row).Specific).Value;
                            var codD = "";

                            if (!string.IsNullOrEmpty(l_Serv))
                            {
                                var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                rec.DoQuery($@"
                                       SELECT DISTINCT
	                                        SP.{"Code".Aspas()},
                                            SP.{ "Name".Aspas()}
                                        FROM { "@CVA_SERVICO_PLAN".Aspas()} AS SP
                                        INNER JOIN { "@CVA_LIN_GRPSERVICOS".Aspas()} AS GS ON
                                            GS.{ "U_CVA_ID_SERVICO".Aspas()} = SP.{ "Code".Aspas()}
                                        WHERE SP.{ "U_CVA_ATIVO".Aspas()} = 'Y'
                                            AND GS.{"Code".Aspas()} = '{code}'
                                            AND SP.{"Code".Aspas()} = '{l_Serv}'
                                ; ");

                                if (!rec.EoF) codD = rec.Fields.Item("Name").Value.ToString();
                            }

                            ((IEditText)mtx.Columns.Item("l_ServD").Cells.Item(pVal.Row).Specific).Value = codD;
                        }
                        
                        if (pVal.ItemUID.Equals(MatrixItens) && "l_Seg,I_Ter,I_Qua,I_Qui,I_Sex,I_Sab,I_Dom".Contains(pVal.ColUID) && !isAlreadyRunningItemEvent && pVal.EventType == BoEventTypes.et_VALIDATE && pVal.ItemChanged)
                        {
                            isAlreadyRunningItemEvent = true;
                            var oForm = Application.Forms.Item(pVal.FormUID);

                            oForm.Freeze(true);

                            LerLinhasAlterarTotaisComensais(oForm);

                            oForm.Freeze(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        var oForm = Application.Forms.GetForm(pVal.FormUID);
                        if (oForm != null) oForm.Freeze(false);

                        Application.SetStatusBarMessage(ex.Message);
                        ret = false;
                    }
                    catch (Exception exx)
                    {
                    }
                }
                finally
                {
                    isAlreadyRunningItemEvent = false;
                }
            }

            bubbleEvent = ret;
        }

        public static bool isAlreadyRunningItemEvent = false;

        public void LerLinhasAlterarTotaisComensais(Form oForm)
        {
            var turnDic = MontaDicValoresDias(oForm);
            if (turnDic.Count < 1) return;

            var gridTotais = (IGrid)oForm.Items.Item(MatrixTotal).Specific;
            //gridTotais.DataTable.Clear();
            gridTotais.DataTable.Rows.Clear();
            gridTotais.DataTable.Rows.Add(turnDic.Count);
            var i = 0;

            var days = new Dictionary<string, float>
                {
                    {"SEG", 0 },
                    {"TER", 0 },
                    {"QUA", 0 },
                    {"QUI", 0 },
                    {"SEX", 0 },
                    {"SAB", 0 },
                    {"DOM", 0 }
                };

            foreach (var item in turnDic)
            {
                gridTotais.DataTable.SetValue(0, i, item.Key);
                gridTotais.DataTable.SetValue(1, i, item.Value["SEG"].ToString());
                gridTotais.DataTable.SetValue(2, i, item.Value["TER"].ToString());
                gridTotais.DataTable.SetValue(3, i, item.Value["QUA"].ToString());
                gridTotais.DataTable.SetValue(4, i, item.Value["QUI"].ToString());
                gridTotais.DataTable.SetValue(5, i, item.Value["SEX"].ToString());
                gridTotais.DataTable.SetValue(6, i, item.Value["SAB"].ToString());
                gridTotais.DataTable.SetValue(7, i, item.Value["DOM"].ToString());

                days["SEG"] += item.Value["SEG"];
                days["TER"] += item.Value["TER"];
                days["QUA"] += item.Value["QUA"];
                days["QUI"] += item.Value["QUI"];
                days["SEX"] += item.Value["SEX"];
                days["SAB"] += item.Value["SAB"];
                days["DOM"] += item.Value["DOM"];

                i++;
            }

            gridTotais.DataTable.Rows.Add(2);
            i++;

            gridTotais.DataTable.SetValue(0, i, "Total");
            gridTotais.DataTable.SetValue(1, i, days["SEG"].ToString());
            gridTotais.DataTable.SetValue(2, i, days["TER"].ToString());
            gridTotais.DataTable.SetValue(3, i, days["QUA"].ToString());
            gridTotais.DataTable.SetValue(4, i, days["QUI"].ToString());
            gridTotais.DataTable.SetValue(5, i, days["SEX"].ToString());
            gridTotais.DataTable.SetValue(6, i, days["SAB"].ToString());
            gridTotais.DataTable.SetValue(7, i, days["DOM"].ToString());
        }

        private Dictionary<string, Dictionary<string, float>> MontaDicValoresDias(Form oForm)
        {
            var turnDic = new Dictionary<string, Dictionary<string, float>>();
            var oMatrix = (Matrix)oForm.Items.Item(MatrixItens).Specific;

            for (int i = 1; i < oMatrix.RowCount; i++)
            {
                var turno = ((EditText)oMatrix.Columns.Item(2).Cells.Item(i).Specific).Value;
                var days = new Dictionary<string, float>
                {
                    {"SEG", StringToFloat(((EditText)oMatrix.Columns.Item(4).Cells.Item(i).Specific).Value) },
                    {"TER", StringToFloat(((EditText)oMatrix.Columns.Item(5).Cells.Item(i).Specific).Value) },
                    {"QUA", StringToFloat(((EditText)oMatrix.Columns.Item(6).Cells.Item(i).Specific).Value) },
                    {"QUI", StringToFloat(((EditText)oMatrix.Columns.Item(7).Cells.Item(i).Specific).Value) },
                    {"SEX", StringToFloat(((EditText)oMatrix.Columns.Item(8).Cells.Item(i).Specific).Value) },
                    {"SAB", StringToFloat(((EditText)oMatrix.Columns.Item(9).Cells.Item(i).Specific).Value) },
                    {"DOM", StringToFloat(((EditText)oMatrix.Columns.Item(10).Cells.Item(i).Specific).Value) }
                };

                if (turnDic.ContainsKey(turno))
                    foreach (var day in days)
                        turnDic[turno][day.Key] = turnDic[turno][day.Key] + day.Value;
                else
                    turnDic.Add(turno, days);
            }

            return turnDic;
        }

        private float StringToFloat(string str)
        {
            var ret = 0f;
            if (float.TryParse(str, out ret))
                return ret;

            return 0;
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

            try
            {
                if (BusinessObjectInfo.FormTypeEx.Equals(Type))
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && !BusinessObjectInfo.BeforeAction)
                    {
                        var oForm = Application.Forms.ActiveForm;
                        //oForm.Freeze(true);

                        LerLinhasAlterarTotaisComensais(oForm);

                        //oForm.Freeze(false);
                    }
                }
            }
            catch (Exception ex) { }

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Composição de Quantidade", 6, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList(Form oForm)
        {
            #region service choose
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            //RTRIM(LTRIM($[$mtxItens.it_prd]))

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

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Serviço Composiçao", idCategoria, Type, MatrixItens, "l_Serv");
            #endregion

            #region Turno Choose

            strSql = $@"SELECT {"Name".Aspas()}  FROM {"@CVA_TURNO".Aspas()} ;";
            FormatedSearch.CreateFormattedSearches(strSql, "Busca Turno", idCategoria, Type, MatrixItens, $"I_Hor");

            #endregion


        }
    }
}