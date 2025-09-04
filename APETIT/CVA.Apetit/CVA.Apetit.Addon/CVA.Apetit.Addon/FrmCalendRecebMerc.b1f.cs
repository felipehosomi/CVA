using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace CVA.Apetit.Addon
{
    [FormAttribute("CVA.Apetit.Addon.FrmCalendarioRecebMercadorias", "FrmCalendRecebMerc.b1f")]
    class FrmCalendRecebMerc : UserFormBase
    {
        public FrmCalendRecebMerc()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFilial").Specific));
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_2").Specific));
            this.Matrix0.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.Matrix0_ValidateBefore);
            this.Matrix0.PressedBefore += new SAPbouiCOM._IMatrixEvents_PressedBeforeEventHandler(this.Matrix0_PressedBefore);
            this.Matrix0.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix0_KeyDownBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("txtFilial").Specific));
            this.EditText0.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.EditText0_KeyDownAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("txtDocEnt").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataUpdateBefore += new DataUpdateBeforeHandler(this.Form_DataUpdateBefore);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Carregando formulário"), SAPbouiCOM.BoMessageTime.bmt_Short, false);

            Form oForm = (Form)this.UIAPIRawForm;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                Class.FilialService.PreencherCombo("cbFilial", UIAPIRawForm, false);

                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;

                //Class.FormService.InserirLinhaMtx(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");
                Class.FormService.AdicionarLinha(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");

                this.UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                oForm.Freeze(false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Ocorreu um erro inesperado. Detalhes: {0}", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;

        private void EditText0_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            throw new System.NotImplementedException();

        }

        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                string sFilial = ComboBox0.Selected.Description;
                EditText0.Value = sFilial;

                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;
                if (oMatrix.RowCount == 0)
                    Class.FormService.AdicionarLinha(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        private void Matrix0_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //Class.FormService.InserirLinhaMtx(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");

        }

        private void Matrix0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //Class.FormService.InserirLinhaMtx(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");

        }

        private void Matrix0_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            string diaSemana = "";
            DateTime data;
            int iDiaSemana;

            BubbleEvent = true;

            try
            {
                Form oForm = Class.Conexao.sbo_application.Forms.Item(pVal.FormUID);
                Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;

                EditText oEdit = (EditText)oMatrix.Columns.Item("cData").Cells.Item(pVal.Row).Specific;
                EditText oEdit2 = (EditText)oMatrix.Columns.Item("cDiaSemana").Cells.Item(pVal.Row).Specific;


                if (!string.IsNullOrEmpty(oEdit.Value))
                {
                    data = DateTime.ParseExact(oEdit.Value.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    iDiaSemana = (int)data.DayOfWeek;
                    if (iDiaSemana == 0)
                        diaSemana = "Domingo";
                    else if (iDiaSemana == 1)
                        diaSemana = "Segunda";
                    else if (iDiaSemana == 2)
                        diaSemana = "Terça";
                    else if (iDiaSemana == 3)
                        diaSemana = "Quarta";
                    else if (iDiaSemana == 4)
                        diaSemana = "Quinta";
                    else if (iDiaSemana == 5)
                        diaSemana = "Sexta";
                    else if (iDiaSemana == 6)
                        diaSemana = "Sábado";

                    Class.FormService.AdicionarLinha(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");
                    if (oForm.Mode == BoFormMode.fm_OK_MODE) oForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                oEdit2.Value = diaSemana;
            }
            catch { }
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //throw new System.NotImplementedException();

            try
            {
                //Form oForm = (Form)this.UIAPIRawForm;
                //oForm.EnableMenu("1292", true);
                //oForm.EnableMenu("1293", true);
            }
            catch
            {
            }

            //return true;


        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            string filial = "";

            BubbleEvent = true;

            Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;

            if (oMatrix.RowCount > 0)
            {
                for (int i = oMatrix.RowCount; i >= 1; i--)
                {
                    EditText oEdit = (EditText)oMatrix.Columns.Item("cData").Cells.Item(i).Specific;
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

            try { filial = this.ComboBox0.Selected.Value.Trim(); } catch { }
            if (string.IsNullOrEmpty(filial))
            {
                Class.Conexao.sbo_application.MessageBox("Filial não selecionada");
                BubbleEvent = false;
            }
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            Matrix oMatrix = (Matrix)this.UIAPIRawForm.Items.Item("Item_2").Specific;

            for (int i = oMatrix.RowCount; i >= 1; i--)
            {
                EditText oEdit = (EditText)oMatrix.Columns.Item("cData").Cells.Item(i).Specific;
                if (string.IsNullOrEmpty(oEdit.Value))
                {
                    oMatrix.DeleteRow(i);
                }
            }
            oMatrix.FlushToDataSource();
            oMatrix.LoadFromDataSource();
        }

        private Button Button2;

        private void Button2_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Class.FormService.AdicionarLinha(UIAPIRawForm, "Item_2", "@CVA_CAR_CAL1");
        }
    }
}
