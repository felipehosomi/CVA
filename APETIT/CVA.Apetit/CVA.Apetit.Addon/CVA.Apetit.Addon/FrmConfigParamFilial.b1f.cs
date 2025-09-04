using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace CVA.Apetit.Addon
{
    [FormAttribute("CVA.Apetit.Addon.Form1", "FrmConfigParamFilial.b1f")]
    class FrmConfigParamFilial : UserFormBase
    {
        public FrmConfigParamFilial()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("mtxDados").Specific));
            this.Matrix0.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.Matrix0_ChooseFromListAfter);
            this.Matrix0.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix0_KeyDownBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFilial").Specific));
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("txtFilial").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("txtDocEnt").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new DataAddBeforeHandler(this.Form_DataAddBefore);

        }

        private SAPbouiCOM.Matrix Matrix0;

        private void OnCustomInitialize()
        {
            string sql;

            Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Carregando formulário"), SAPbouiCOM.BoMessageTime.bmt_Short, false);

            Form oForm = (Form)this.UIAPIRawForm;

            try
            {
                //oForm.Freeze(true);
                this.UIAPIRawForm.Freeze(true);

                Class.FilialService.PreencherCombo("cbFilial", UIAPIRawForm, false);
                //SAPbouiCOM.Form oForm = (SAPbouiCOM.Form)Class.Conexao.sbo_application.Forms.ActiveForm;

                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT ""WhsCode"", ""WhsName"" FROM OWHS ORDER BY ""WhsCode"" ");
                oRec.DoQuery(sql);
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    string whsCode = oRec.Fields.Item("WhsCode").Value.ToString();
                    string whsName = oRec.Fields.Item("WhsName").Value.ToString();
                    ((SAPbouiCOM.Matrix)(this.UIAPIRawForm.Items.Item("mtxDados").Specific)).Columns.Item("cDepPad").ValidValues.Add(whsCode, whsName);
                    oRec.MoveNext();
                }

                sql = string.Format(@"SELECT ""BPLId"", ""BPLName"" FROM OBPL ORDER BY ""BPLId"" ");
                oRec.DoQuery(sql);
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    string whsCode = oRec.Fields.Item("BPLId").Value.ToString();
                    string whsName = oRec.Fields.Item("BPLName").Value.ToString();
                    ((SAPbouiCOM.Matrix)(this.UIAPIRawForm.Items.Item("mtxDados").Specific)).Columns.Item("cCD").ValidValues.Add(whsCode, whsName);
                    oRec.MoveNext();
                }

                sql = string.Format(@"SELECT ""ID"", ""Usage"" FROM OUSG WHERE UPPER(""Usage"") LIKE 'S%' ORDER BY ""ID"" ");
                oRec.DoQuery(sql);
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    string whsCode = oRec.Fields.Item("ID").Value.ToString();
                    string whsName = oRec.Fields.Item("Usage").Value.ToString();
                    ((SAPbouiCOM.Matrix)(this.UIAPIRawForm.Items.Item("mtxDados").Specific)).Columns.Item("uUsgTran").ValidValues.Add(whsCode, whsName);
                    oRec.MoveNext();
                }

                sql = string.Format(@"SELECT ""ID"", ""Usage"" FROM OUSG WHERE UPPER(""Usage"") LIKE 'E%' ORDER BY ""ID"" ");
                oRec.DoQuery(sql);
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    string whsCode = oRec.Fields.Item("ID").Value.ToString();
                    string whsName = oRec.Fields.Item("Usage").Value.ToString();
                    ((SAPbouiCOM.Matrix)(this.UIAPIRawForm.Items.Item("mtxDados").Specific)).Columns.Item("uUsgRet").ValidValues.Add(whsCode, whsName);
                    oRec.MoveNext();
                }

                sql = string.Format(@"SELECT ""ID"", ""Usage"" FROM OUSG WHERE UPPER(""Usage"") LIKE 'S%' ORDER BY ""ID"" ");
                oRec.DoQuery(sql);
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    string whsCode = oRec.Fields.Item("ID").Value.ToString();
                    string whsName = oRec.Fields.Item("Usage").Value.ToString();
                    ((SAPbouiCOM.Matrix)(this.UIAPIRawForm.Items.Item("mtxDados").Specific)).Columns.Item("uUsgRem").ValidValues.Add(whsCode, whsName);
                    oRec.MoveNext();
                }




                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                //Class.FormService.AdicionarLinha(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");
                //Class.FormService.RemoverLinha(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");

                //Class.FormService.InserirLinhaMtx(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");
                Class.FormService.AdicionarLinha(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");

                //buscaFilial(oForm);

                //Util.NovoRegistro(oForm);
                oForm.Freeze(false);
            }
            catch (Exception ex)
            {
                oForm.Freeze(false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Ocorreu um erro inesperado. Detalhes: {0}", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }


        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText0;

        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                string sFilial = ComboBox0.Selected.Description;
                EditText0.Value = sFilial;

                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("mtxDados").Specific;
                if (oMatrix.RowCount == 0)
                    Class.FormService.AdicionarLinha(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("mtxDados").Specific;

            if (oMatrix.RowCount > 0)
            {
                //for (int i = 1; i <= oMatrix.RowCount; i++)
                for (int i = oMatrix.RowCount; i >= 1; i--)
                {
                    EditText oEdit = (EditText)oMatrix.Columns.Item("cPN").Cells.Item(i).Specific;
                    if (string.IsNullOrEmpty(oEdit.Value))
                    {
                        oMatrix.DeleteRow(i);
                    }
                }
                oMatrix.FlushToDataSource();
                oMatrix.LoadFromDataSource();
            }

            if (oMatrix.RowCount == 0)
            {
                Class.Conexao.sbo_application.MessageBox("Grid sem linhas");
                BubbleEvent = false;
            }
        }

        private EditText EditText1;

        private void Matrix0_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //throw new System.NotImplementedException();
            //Class.FormService.InserirLinhaMtx(UIAPIRawForm, "mtxDados", "@CVA_CAR_CONF1");

        }

        private void Matrix0_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            //throw new System.NotImplementedException();
            Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
            try
            {
                EditText oEdit;
                string coluna = pVal.ColUID;

                SBOChooseFromListEventArg ChooseEvents = ((SBOChooseFromListEventArg)(pVal));
                string selecionado = ChooseEvents.SelectedObjects.GetValue(0, 0).ToString();

                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("mtxDados").Specific;

                //if (coluna == "cPA")
                //    oEdit = (EditText)oMatrix.Columns.Item("cPA").Cells.Item(pVal.Row).Specific;
                //else
                    oEdit = (EditText)oMatrix.Columns.Item("cPN").Cells.Item(pVal.Row).Specific;

                if (oForm.Mode == BoFormMode.fm_OK_MODE) oForm.Mode = BoFormMode.fm_UPDATE_MODE;
                oEdit.Value = selecionado;

            }
            catch (Exception ex)
            {

            }
        }
    }
}