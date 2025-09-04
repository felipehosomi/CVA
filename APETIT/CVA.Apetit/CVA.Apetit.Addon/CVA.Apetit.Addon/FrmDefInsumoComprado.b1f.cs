using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace CVA.Apetit.Addon
{
    [FormAttribute("CVA.Apetit.Addon.FrmDefinicaoInsumosComprados", "FrmDefInsumoComprado.b1f")]
    class FrmDefInsumoComprado : UserFormBase
    {
        public FrmDefInsumoComprado()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFilial").Specific));
            this.ComboBox0.ComboSelectBefore += new SAPbouiCOM._IComboBoxEvents_ComboSelectBeforeEventHandler(this.ComboBox0_ComboSelectBefore);
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_2").Specific));
            this.Matrix0.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.Matrix0_ClickBefore);
            this.Matrix0.MatrixLoadAfter += new SAPbouiCOM._IMatrixEvents_MatrixLoadAfterEventHandler(this.Matrix0_MatrixLoadAfter);
            this.Matrix0.ChooseFromListBefore += new SAPbouiCOM._IMatrixEvents_ChooseFromListBeforeEventHandler(this.Matrix0_ChooseFromListBefore);
            this.Matrix0.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.Matrix0_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("txtFilial").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("txtDocEnt").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("NovaLinha").Specific));
            this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.Matrix0.SelectionMode = BoMatrixSelect.ms_Single;
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private Button Button2;

        private void OnCustomInitialize()
        {
            string sql;

            Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Carregando formulário"), SAPbouiCOM.BoMessageTime.bmt_Short, false);

            Form oForm = (Form)this.UIAPIRawForm;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                this.UIAPIRawForm.Menu.Add("AddLineFrmDefInsumo", "Adicionar Linha", SAPbouiCOM.BoMenuType.mt_STRING, 0);
                this.UIAPIRawForm.Menu.Add("DelLineFrmDefInsumo", "Remover Linha", SAPbouiCOM.BoMenuType.mt_STRING, 0);

                Class.FilialService.PreencherCombo("cbFilial", UIAPIRawForm, false);
                Class.FilialService.PreencherComboMatriz("cItemGroup", UIAPIRawForm, "Item_2", false);
                Class.FilialService.PreencherComboMatriz("cFamilia", UIAPIRawForm, "Item_2", false);
                Class.FilialService.PreencherComboMatriz("cSFamilia", UIAPIRawForm, "Item_2", false);


                
                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                this.UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                oForm.Freeze(false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Ocorreu um erro inesperado. Detalhes: {0}", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            string sql;
            int cont;

            try
            {
                string sFilial = ComboBox0.Selected.Description;
                EditText0.Value = sFilial;
                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;
                oMatrix.Clear();

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    sql = string.Format(@"SELECT COUNT(1) FROM ""@CVA_CAR_INS"" WHERE ""U_BPLId"" = '{0}' ", ComboBox0.Selected.Value);
                    cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                    if (cont > 0)
                    {
                        Class.Conexao.sbo_application.MessageBox("Filial já cadastrada");
                        EditText0.Value = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        private void Matrix0_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            string chave, descricao;

            Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
            try
            {
                EditText oEdit, oEdit1;
                string coluna = pVal.ColUID;

                SBOChooseFromListEventArg ChooseEvents = ((SBOChooseFromListEventArg)(pVal));
                chave = ChooseEvents.SelectedObjects.GetValue(0, 0).ToString();
                descricao = ChooseEvents.SelectedObjects.GetValue(1, 0).ToString();

                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;

                oEdit = (EditText)oMatrix.Columns.Item("cItem").Cells.Item(pVal.Row).Specific;
                oEdit1 = (EditText)oMatrix.Columns.Item("cDescricao").Cells.Item(pVal.Row).Specific;

                try { oEdit.Value = chave; } catch { }
                try { oEdit1.Value = descricao; } catch { }

                if (oForm.Mode == BoFormMode.fm_OK_MODE) oForm.Mode = BoFormMode.fm_UPDATE_MODE;
            }
            catch (Exception ex)
            {

            }
        }

        private void Button2_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var oMatrix = (SAPbouiCOM.Matrix)UIAPIRawForm.Items.Item("Item_2").Specific;
            UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CAR_INS1").InsertRecord(UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CAR_INS1").Size);
        }

        public static void AddLineFrmDefInsumo()
        {
            string sql;
            int cont = 0;


            if(string.IsNullOrEmpty(Class.Events.FormUID))
                Class.Events.FormUID = "ConfIns";

            var oForm = Class.Conexao.sbo_application.Forms.Item(Class.Events.FormUID);

            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                string sFilial = ((SAPbouiCOM.ComboBox)oForm.Items.Item("cbFilial").Specific).Value;

                if(!string.IsNullOrEmpty(sFilial))
                {
                    sql = $@"SELECT COUNT(1) FROM ""@CVA_CAR_INS"" WHERE ""U_BPLId"" = '{sFilial}'";
                    cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                }
                else
                {
                    Class.Conexao.sbo_application.MessageBox("Favor selecionar uma filial");
                    return;
                }
            }

            if (cont > 0)
            {
                Class.Conexao.sbo_application.MessageBox("Selecionar outra filial");
            }
            else
            {
                var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("Item_2").Specific;
                oForm.Freeze(true);
                oMatrix.FlushToDataSource();
                oForm.DataSources.DBDataSources.Item("@CVA_CAR_INS1").InsertRecord(oForm.DataSources.DBDataSources.Item("@CVA_CAR_INS1").Size);
                oMatrix.LoadFromDataSource();
                ColunaDescricao(oForm);
                oForm.Freeze(false);

            }
        }

        public static void RemoveMatrixLines()
        {
            if (string.IsNullOrEmpty(Class.Events.FormUID))
                Class.Events.FormUID = "ConfIns";

            var oForm = Class.Conexao.sbo_application.Forms.Item(Class.Events.FormUID);

            if (oForm != null)
            {
                var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("Item_2").Specific;

                oForm.Freeze(true);              
                
                #region Remover Linhas em Branco da Grid
                var isRemove = false;
                var dbDataSources = oForm.DataSources.DBDataSources.Item("@CVA_CAR_INS1");
                
                for (int i = oMatrix.RowCount; i > 0; i--)
                {
                    if (oMatrix.IsRowSelected(i))
                    {
                        try { dbDataSources.RemoveRecord(i - 1); } catch { }
                        //oMatrix.DeleteRow(i);
                        isRemove = true;
                    }
                }

                if (isRemove)
                {
                    oMatrix.LoadFromDataSource();

                    string sDocEntry = ((SAPbouiCOM.EditText)oForm.Items.Item("txtDocEnt").Specific).Value;

                    if (string.IsNullOrEmpty(sDocEntry))
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                    else
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                       
                }
                    
                #endregion

                oForm.Freeze(false);

                if (!isRemove)
                    Class.Conexao.sbo_application.MessageBox("Favor selecionar uma linha para remoção!");
            }
           
        }

        private void Matrix0_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            string s1 = "";

            try
            {
                SAPbouiCOM.Conditions oCons;
                SAPbouiCOM.Condition oCon;
                SAPbouiCOM.Conditions oEmptyConds = new SAPbouiCOM.Conditions();
                SAPbouiCOM.ChooseFromList oCFL = this.UIAPIRawForm.ChooseFromLists.Item("CFL_1");
                oCFL.SetConditions(oEmptyConds);
                oCons = oCFL.GetConditions();

                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;
                try { s1 = ((ComboBox)oMatrix.Columns.Item("cItemGroup").Cells.Item(pVal.Row).Specific).Selected.Value.ToString(); } catch { }

                if(!string.IsNullOrEmpty(s1))
                {
                    oCon = oCons.Add();
                    oCon.Alias = "ItmsGrpCod";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = s1;
                    oCFL.SetConditions(oCons);
                }
                
            }
            catch
            { }
        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            string msg = "";

            BubbleEvent = ValidarDados(pVal, ref msg);
            if (!string.IsNullOrEmpty(msg))
                Class.Conexao.sbo_application.MessageBox(msg);
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            string msg = "";

            BubbleEvent = ValidarDados(pVal, ref msg);
            if (!string.IsNullOrEmpty(msg))
                Class.Conexao.sbo_application.MessageBox(msg);
        }

        private bool ValidarDados(BusinessObjectInfo pVal, ref string msg)
        {
            bool retorno = true;
            string filial = "", s1 = "", s2 = "", s3 = "", s4 = "", sql;
            int cont;

            msg = "";

            if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                sql = string.Format(@"SELECT COUNT(1) FROM ""@CVA_CAR_INS"" WHERE ""U_BPLId"" = '{0}' ", ComboBox0.Selected.Value);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                if (cont > 0)
                {
                    msg = "Filial já cadastrada";
                    retorno = false;
                }
            }

            Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;

            if ((msg == "") && (oMatrix.RowCount > 0))
            {
                for (int i = oMatrix.RowCount; i >= 1; i--)
                {
                    try { s1 = ((ComboBox)oMatrix.Columns.Item("cItemGroup").Cells.Item(i).Specific).Selected.Description; } catch { }
                    try { s2 = ((EditText)oMatrix.Columns.Item("cItem").Cells.Item(i).Specific).Value; } catch { }
                    try { s3 = ((ComboBox)oMatrix.Columns.Item("cFamilia").Cells.Item(i).Specific).Selected.Description; } catch { }
                    try { s4 = ((ComboBox)oMatrix.Columns.Item("cSFamilia").Cells.Item(i).Specific).Selected.Description; } catch { }

                    if ((string.IsNullOrEmpty(s1)) && (string.IsNullOrEmpty(s2)) && (string.IsNullOrEmpty(s3)) && (string.IsNullOrEmpty(s4)))
                    {
                        oMatrix.DeleteRow(i);
                    }
                }
                oMatrix.FlushToDataSource();
                oMatrix.LoadFromDataSource();
            }

            if ((msg == "") && (oMatrix.RowCount == 0))
            {
                msg = "Grid sem linhas";
                retorno = false;
            }

            try { filial = this.ComboBox0.Selected.Value.Trim(); } catch { }
            if ((msg == "") && (string.IsNullOrEmpty(filial)))
            {
                msg = "Filial não selecionada";
                retorno = false;
            }

            return retorno;
        }

        private void Matrix0_MatrixLoadAfter(object sboObject, SBOItemEventArg pVal)
        {
            Form oForm = (Form)this.UIAPIRawForm;
            ColunaDescricao(oForm);
        }

        private static void ColunaDescricao(Form oForm)
        {
            EditText oEdit, oEdit1;
            string descricao, sql;

            Matrix oMatrix = (Matrix)oForm.Items.Item("Item_2").Specific;

            if (oMatrix.RowCount > 0)
            {
                for (int i = oMatrix.RowCount; i >= 1; i--)
                {
                    oEdit = (EditText)oMatrix.Columns.Item("cItem").Cells.Item(i).Specific;
                    oEdit1 = (EditText)oMatrix.Columns.Item("cDescricao").Cells.Item(i).Specific;

                    if (!string.IsNullOrEmpty(oEdit.Value))
                    {
                        sql = string.Format(@"SELECT ""ItemName"" FROM OITM WHERE ""ItemCode"" = '{0}' ", oEdit.Value);
                        descricao = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                        try { oEdit1.Value = descricao; } catch { }
                    }
                    else
                        try { oEdit1.Value = ""; } catch { }
                }
            }

        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Class.Events.Row = eventInfo.Row;
            Class.Events.ItemUID = eventInfo.ItemUID;
            Class.Events.FormUID = eventInfo.FormUID;
            Class.Events.ColUID = eventInfo.ColUID;
        }

        private void ComboBox0_ComboSelectBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            Form oForm = (Form)this.UIAPIRawForm;
            if ((oForm.Mode == BoFormMode.fm_OK_MODE) || (oForm.Mode == BoFormMode.fm_UPDATE_MODE))
            {
                BubbleEvent = false;
            }
        }

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
            EditText0.Value = "";
        }

        
        private void Matrix0_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string chave = "";

            /*
            if (pVal.ColUID == "cSFamilia")
            {
                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;
                try
                {
                    chave = ((ComboBox)oMatrix.Columns.Item("cFamilia").Cells.Item(pVal.Row).Specific).Selected.Value.ToString();
                }
                catch (Exception ex)
                {
                    //Class.Conexao.sbo_application.MessageBox(ex.Message);
                }

                PreencherComboMatrizSubFamilia("cSFamilia", UIAPIRawForm, "Item_2", chave, false);
            }
           */

        }

        private void Button0_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //throw new System.NotImplementedException();

        }

        /*
        private void PreencherComboMatrizSubFamilia(string comboUID, SAPbouiCOM.IForm form, string MatrizID, string chave, bool opcaoTodas = false)
        {
            string sql = "";

            try
            {
                if (comboUID == "cSFamilia")
                {
                    sql = string.Format(@"
SELECT ""Code"" AS ""Chave"", ""Name"" AS ""Descricao"" 
FROM ""@CVA_SUBFAMILA"" 
WHERE ""U_CVA_Familia"" = '{0}' 
ORDER BY ""Code"" 
", chave);
                }

                for (int i = ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Count; i >= 0; i--)
                    try { ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Remove(i); } catch { }

                //for (int i = 0; i <= ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Count; i++)
                //    try { ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Remove(i); } catch { }


                if (!string.IsNullOrEmpty(sql))
                {
                    var dt = Class.Conexao.ExecuteSqlDataTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string whsCode = dt.Rows[i]["Chave"].ToString();
                        string whsName = dt.Rows[i]["Descricao"].ToString();
                        ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Add(whsCode, whsName);
                    }

                    if (opcaoTodas)
                    {
                        ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Add("0", "Todas");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        */

    }
}
