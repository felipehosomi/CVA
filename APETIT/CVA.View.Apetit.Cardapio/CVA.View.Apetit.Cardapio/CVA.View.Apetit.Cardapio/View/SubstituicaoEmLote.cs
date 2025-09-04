using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVA.View.Apetit.Cardapio.View
{
    public class SubstituicaoEmLote : BaseForm
    {
        public List<QtdTurnoModel> QtdTurnoModels = new List<QtdTurnoModel>();
        ScreenData ScreenData;
        string COLUID;
        int Row;

        // Declare the delegate (if using non-generic pattern).
        public delegate void Confirm(bool todos, DateTime de, DateTime ate, List<string> codServicos, string insumoDe, string insumoPara, string insumoParaDes);

        // Declare the event.
        public event Confirm ConfirmEvent;

        public SubstituicaoEmLote(ScreenData screenData, string coluid, int row)
        {
            //if (matrixItemList != null) QtdTurnoModels = matrixItemList[coluid][row].QtdTurnos;

            COLUID = coluid;
            Row = row;
            ScreenData = screenData;
            Type = "CARDSUBS";
            TableName = "CVA_CARDSUBS";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
        }

        public override void CreateUserFields()
        {
            //var userFields = new UserFields();

            //UserTables.CreateIfNotExist(TableName, "[CVA] Dados Prato Qtd. Turno", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            //userFields.CreateIfNotExist("@" + TableName, TB_IdPlan, "ID Plan.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + TableName, TB_IdLinhaPlan, "ID Linha Plan.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + TableName, TB_IdTurno, "ID Turno", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + TableName, TB_DesTurno, "Descr. Turno", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            //userFields.CreateIfNotExist("@" + TableName, TB_Qtd, "Quantidade Turno", 12, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        internal override void LoadDefault(Form oForm)
        {
            oForm = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            Filters.Add(oForm.TypeEx, BoEventTypes.et_ALL_EVENTS);
            CreateChooseFromList();

            try
            {
                #region Tab Substituição

                #region Radio Planejamento

                rad_all = (OptionBtn)oForm.Items.Item("rad_all").Specific;
                rad_only = (OptionBtn)oForm.Items.Item("rad_only").Specific;

                #endregion

                edt_de = (EditText)oForm.Items.Item("edt_de").Specific;
                edt_it_d = (EditText)oForm.Items.Item("edt_it_d").Specific;
                edt_itd_d = (EditText)oForm.Items.Item("edt_itd_d").Specific;
                edt_it_a = (EditText)oForm.Items.Item("edt_it_a").Specific;
                edt_itd_a = (EditText)oForm.Items.Item("edt_itd_a").Specific;
                edt_ate = (EditText)oForm.Items.Item("edt_ate").Specific;
                btnConSub = (Button)oForm.Items.Item("btnConSub").Specific;
                mtxServ = (Matrix)oForm.Items.Item("mtxServ").Specific;

                rad_only.GroupWith("rad_all");
                rad_all.Selected = true;
                oForm.Items.Item("btnConSub").Enabled = false;
                btnConSub.PressedAfter += BtnConSub_PressedAfter;


                edt_it_d.Value = ScreenData.MatrixItemList[COLUID][Row - 1].Insumo;
                edt_itd_d.Value = ScreenData.MatrixItemList[COLUID][Row - 1].InsumoDes;
                edt_it_a.ValidateAfter += Edt_it_a_ValidateAfter;

                oForm.DataSources.DataTables.Item("DT_S").ExecuteQuery($@"
                    SELECT  
                         'N'                    AS {"sel".Aspas()}
                     , S.{"Code".Aspas()}    AS {"codS".Aspas()}
                        , S.{ "Name".Aspas()}   AS {"desS".Aspas()}
                    FROM { "@CVA_SERVICO_PLAN".Aspas()} AS S
                    WHERE S.{ "U_CVA_ATIVO".Aspas()} = 'Y'
                ");

                mtxServ.LoadFromDataSourceEx(true);

                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        private void BtnConSub_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var oForm = B1Connection.Instance.Application.Forms.ActiveForm;

                if (oForm.Items.Item("btnConSub").Enabled)
                {
                    var codServicos = new List<string>();

                    for (int i = 1; i <= mtxServ.RowCount; i++)
                    {
                        var check = (CheckBox)mtxServ.Columns.Item(1).Cells.Item(i).Specific;
                        if (check.Checked)
                        {
                            var edtCodServico = (EditText)mtxServ.Columns.Item(2).Cells.Item(i).Specific;
                            codServicos.Add(edtCodServico.Value);
                        }
                    }

                    var radAll = rad_all.Selected;
                    var de = DIHelper.Format_StringToDate(edt_de.Value);
                    var ate = DIHelper.Format_StringToDate(edt_ate.Value);
                    var itde = edt_it_d.Value;
                    var itpara = edt_it_a.Value;
                    var itparaDes = edt_itd_a.Value;

                    oForm.Close();

                    ConfirmEvent?.Invoke(radAll,de, ate, codServicos, itde, itpara, itparaDes);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Edt_it_a_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (string.IsNullOrEmpty(edt_it_a.Value)) return;

            var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                    SELECT DISTINCT
                        { "ItemName".Aspas()}
                    FROM { "OITM".Aspas()} 
                    WHERE { "ItemCode".Aspas()} = '{edt_it_a.Value}'
            ;");

            string name = "";
            if (!rec.EoF) name = rec.Fields.Item("ItemName").Value.ToString();
            edt_itd_a.Value = name;
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            //var openMenu = OpenMenu(MenuItem, FilePath, pVal);

            //if (!string.IsNullOrEmpty(openMenu))
            //{
            //    ret = false;
            //    Application.SetStatusBarMessage(openMenu);
            //}

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            if (pVal.FormTypeEx.Equals(TYPEEX))
            {
                try
                {
                    if (!pVal.BeforeAction)
                    {
                        if (DIHelper.HasForm(Application, FORMUID))
                        {
                            var oForm = Application.Forms.Item(FORMUID);
                            var hasOneSelected = false;

                            for (int i = 1; i <= mtxServ.RowCount; i++)
                            {
                                var check = (CheckBox)mtxServ.Columns.Item(1).Cells.Item(i).Specific;
                                if (check.Checked) hasOneSelected = true;
                            }

                            var enabled = !string.IsNullOrEmpty(edt_de.Value)
                                       && !string.IsNullOrEmpty(edt_ate.Value)
                                       && !string.IsNullOrEmpty(edt_it_a.Value)
                                       && !string.IsNullOrEmpty(edt_itd_a.Value)
                                       && hasOneSelected;

                            var btn = oForm.Items.Item("btnConSub").Enabled = enabled;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var oForm = Application.Forms.GetForm(pVal.FormTypeEx);
                    if (oForm != null) oForm.Freeze(false);

                    //Application.SetStatusBarMessage(ex.Message);
                    //ret = false;
                }
            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (BusinessObjectInfo.FormTypeEx.Equals(TYPEEX))
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && !BusinessObjectInfo.BeforeAction)
                    {
                        //var oForm = Application.Forms.ActiveForm;
                        //oForm.Freeze(true);

                        //LerLinhasAlterarTotaisComensais(oForm);

                        //oForm.Freeze(false);
                    }
                }
            }
            catch (Exception ex) { }

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            //Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Composição de Quantidade", 6, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList()
        {
            #region Insumo
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            #region query
            string strSql = $@"  
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
                                    O.{ "CardCode".Aspas()} = '{ScreenData.IdCliente}'
                            GROUP BY
                                W.{ "ItemCode".Aspas()}
                        ) AS TB) AS T ON 
                        T.{ "ItemCode".Aspas()} = OIT.{ "ItemCode".Aspas()}
                    WHERE   OIT.{"ItemCode".Aspas()} NOT IN (
                            SELECT 
	                            L.{"U_CVA_ITEMCODE".Aspas()} 
                            FROM { "@CVA_BLOQUEN".Aspas()} AS B
                            INNER JOIN { "@CVA_LIN_BLOQUEN".Aspas()} AS L ON
                                B.{ "Code".Aspas()} = L.{ "Code".Aspas()}
                            WHERE B.{ "U_CVA_ID_CONTRATO".Aspas()} = '{ScreenData.IdContrato}'
                
                            UNION
                
                            SELECT 
                                IT.{"Father".Aspas()} as {"U_CVA_ITEMCODE".Aspas()}
                            FROM  { "@CVA_BLOQUEN".Aspas()} AS B
                            INNER JOIN  { "@CVA_LIN_BLOQUEN".Aspas()} AS L ON
                                B. { "Code".Aspas()} = L.{ "Code".Aspas()}
                            INNER JOIN ITT1 AS IT ON
                	            IT.{ "Code".Aspas()} = L.{ "U_CVA_ITEMCODE".Aspas()}                 
                            WHERE B. { "U_CVA_ID_CONTRATO".Aspas()} = '{ScreenData.IdContrato}'
                        )
            ;";
            #endregion

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Insumo Sub.", idCategoria, TYPEEX, "edt_it_a", "-1");
            #endregion
        }





        OptionBtn rad_all;
        OptionBtn rad_only;
        EditText edt_de;
        EditText edt_ate;
        EditText edt_it_d;
        EditText edt_itd_d;
        EditText edt_it_a;
        EditText edt_itd_a;

        Button btnConSub;
        Matrix mtxServ;
    }
}