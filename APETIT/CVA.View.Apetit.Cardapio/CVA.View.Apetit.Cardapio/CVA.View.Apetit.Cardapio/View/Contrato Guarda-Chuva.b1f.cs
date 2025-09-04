using System;
using SAPbouiCOM.Framework;
using SAPbouiCOM;
using SAPbobsCOM;
using CVA.View.Apetit.Cardapio.Helpers;
using Addon.CVA.View.Apetit.Cardapio.Helpers;

namespace CVA.View.Apetit.Cardapio.View
{
    [FormAttribute("1250000100", "View/Contrato Guarda-Chuva.b1f")]
    class f_1250000100 : SystemFormBase
    {
        private IForm oForm;

        public f_1250000100()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.oForm = this.UIAPIRawForm;
            this.tabCustoPadrao = ((SAPbouiCOM.Folder)(this.GetItem("tab_cPadr").Specific));
            SAPbouiCOM.Item tabTransRecorrente = this.oForm.Items.Item("1320000072");

            this.tabCustoPadrao.Item.Top = tabTransRecorrente.Top;
            this.tabCustoPadrao.Item.Height = tabTransRecorrente.Height;
            this.tabCustoPadrao.Item.Width = tabTransRecorrente.Width;
            this.tabCustoPadrao.Item.Left = (tabTransRecorrente.Left + tabTransRecorrente.Width);
            this.tabCustoPadrao.Item.AffectsFormMode = false;
            this.tabCustoPadrao.GroupWith("1320000072");
            this.tabCustoPadrao.PressedAfter += this.TaCustoPadrao_PressedAfter;

            this.edtFilial = ((SAPbouiCOM.EditText)(this.GetItem("edt_Filial").Specific));
            this.mtxCustoPadrao = ((SAPbouiCOM.Matrix)(this.GetItem("mtx_cPadr").Specific));
            mtxCustoPadrao.AutoResizeColumns();
            this.mtxCustoPadrao.ClickAfter += MtxCustoPadrao_DoubleClickAfter;
            this.mtxCustoPadrao.ValidateBefore += MtxCustoPadrao_ValidateBefore;
            this.OnCustomInitialize();

            AddEmpyGridLine();
        }

        private void MtxCustoPadrao_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            AddEmpyGridLine();
        }

        void AddEmpyGridLine()
        {
            if (mtxCustoPadrao.RowCount == 0)
                mtxCustoPadrao.AddRow();
        }

        protected override void OnFormResizeAfter(SBOItemEventArg pVal)
        {
            mtxCustoPadrao.AutoResizeColumns();
        }

        private void MtxCustoPadrao_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "it_sid" && pVal.ItemChanged)
            {
                var codeServico = ((IEditText)mtxCustoPadrao.Columns.Item("it_sid").Cells.Item(pVal.Row).Specific).Value;
                var rec = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                rec.DoQuery($@"
                    SELECT Distinct
	                      SP.{"Code".Aspas()} 
                        , SP.{ "Name".Aspas()}
                    FROM { "@CVA_SERVICO_PLAN".Aspas()} SP
                    WHERE SP.{"Code".Aspas()} = '{codeServico}'
                ;");

                var nameServico = "";
                if (!rec.EoF)
                    nameServico = rec.Fields.Item("Name").Value.ToString();

                ((IEditText)mtxCustoPadrao.Columns.Item("it_sdes").Cells.Item(pVal.Row).Specific).Value = nameServico;
            }

            if (pVal.ColUID == "it_valor" && mtxCustoPadrao.RowCount == pVal.Row)
            {
                mtxCustoPadrao.AddRow();
            }
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            base.OnFormActivateAfter(pVal);

            if (pVal.FormMode == (int)BoFormMode.fm_ADD_MODE || pVal.FormMode == (int)BoFormMode.fm_EDIT_MODE || pVal.FormMode == (int)BoFormMode.fm_OK_MODE)
            {
                mtxCustoPadrao.AddRow();
            }
            else
            {
                mtxCustoPadrao.FlushToDataSource();
            }
        }

        protected override void OnFormLoadAfter(SBOItemEventArg pVal)
        {
            base.OnFormLoadAfter(pVal);

            if (pVal.FormMode == (int)BoFormMode.fm_ADD_MODE || pVal.FormMode == (int)BoFormMode.fm_EDIT_MODE || pVal.FormMode == (int)BoFormMode.fm_OK_MODE)
            {
                mtxCustoPadrao.AddRow();
            }
            else
            {
                mtxCustoPadrao.FlushToDataSource();
            }
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {

            if (oForm.Mode == BoFormMode.fm_ADD_MODE || oForm.Mode == BoFormMode.fm_EDIT_MODE || oForm.Mode == BoFormMode.fm_OK_MODE)
            {
                if (mtxCustoPadrao.RowCount > 0) mtxCustoPadrao.Clear();
                 
                var number = oForm.DataSources.DBDataSources.Item("OOAT").GetValue("Number", 0);
                var rec = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                rec.DoQuery($@"SELECT * FROM {"@CVA_CUSTO_PADRAO".Aspas()} WHERE { "U_CVA_Contrato".Aspas() } = '{number}';");

                var i = 1;
                while (!rec.EoF)
                {
                    mtxCustoPadrao.AddRow();

                    ((EditText)mtxCustoPadrao.Columns.Item("#").Cells.Item(i).Specific).Value = rec.Fields.Item("U_CVA_Contrato").Value.ToString();
                    ((EditText)mtxCustoPadrao.Columns.Item("it_mes").Cells.Item(i).Specific).Value = rec.Fields.Item("U_CVA_Mes").Value.ToString();
                    ((EditText)mtxCustoPadrao.Columns.Item("it_valor").Cells.Item(i).Specific).Value = rec.Fields.Item("U_CVA_Valor").Value.ToString();
                    ((EditText)mtxCustoPadrao.Columns.Item("it_sid").Cells.Item(i).Specific).Value = rec.Fields.Item("U_CVA_Id_Servico").Value.ToString();
                    ((EditText)mtxCustoPadrao.Columns.Item("it_sdes").Cells.Item(i).Specific).Value = rec.Fields.Item("U_CVA_Des_Servico").Value.ToString();

                    rec.MoveNext();
                    i++;
                }

                //mtxCustoPadrao.AddRow();
            }
            else
            {
                mtxCustoPadrao.Clear();
            }

            base.OnFormDataLoadAfter(ref pVal);
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            if (mtxCustoPadrao.RowCount > 0)
            {
                var number = oForm.DataSources.DBDataSources.Item("OOAT").GetValue("Number", 0);

                var rec = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                rec.DoQuery($@"DELETE FROM {"@CVA_CUSTO_PADRAO".Aspas()} WHERE { "U_CVA_Contrato".Aspas() } = '{number}';");

                for (int i = 1; i <= mtxCustoPadrao.RowCount; i++)
                {
                    var mes = ((EditText)mtxCustoPadrao.Columns.Item("it_mes").Cells.Item(i).Specific).Value;
                    var valor = ((EditText)mtxCustoPadrao.Columns.Item("it_valor").Cells.Item(i).Specific).Value;
                    var serId = ((EditText)mtxCustoPadrao.Columns.Item("it_sid").Cells.Item(i).Specific).Value;
                    var serDes = ((EditText)mtxCustoPadrao.Columns.Item("it_sdes").Cells.Item(i).Specific).Value;

                    var dt = DateTime.Now;
                    var val = 0d;

                    if (string.IsNullOrEmpty(valor) || !double.TryParse(valor, out val)) continue;
                    if (string.IsNullOrEmpty(mes) || !DateTime.TryParseExact(mes, "MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out dt)) continue;

                    var code = DIHelper.GetNextCode("CVA_CUSTO_PADRAO").ToString();

                    rec.DoQuery($@"INSERT INTO {"@CVA_CUSTO_PADRAO".Aspas()} VALUES('{code}', '{code}', '{mes}', '{number}', '{val.ToString().Replace(",", ".")}', '{serId}', '{serDes}');");
                }
            }

            base.OnFormDataUpdateBefore(ref pVal, out BubbleEvent);
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            base.OnFormDataAddBefore(ref pVal, out BubbleEvent);
        }

        private void TaCustoPadrao_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            oForm.PaneLevel = 2030;
        }


        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            CreateChooseFromList(this.oForm);
        }

        public void CreateChooseFromList(IForm oForm)
        {
            #region Busca Serviço
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            var strSql = $@"
                        SELECT Distinct
	                         SP.{"Code".Aspas()} 
	                        ,SP.{ "Name".Aspas()}
                        FROM {"@CVA_SERVICO_PLAN".Aspas()} SP
            ; ";

            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Serviço Contrato.", idCategoria, "1250000100", "mtx_cPadr", "ud_sID");
            #endregion

        }

        private SAPbouiCOM.Folder tabCustoPadrao;
        private SAPbouiCOM.EditText edtFilial;
        private Matrix mtxCustoPadrao;
    }
}
