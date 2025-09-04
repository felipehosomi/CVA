
using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class IncidenciaDeProteinaForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Contrato = "CVA_ID_CONTRATO";
        public const string TB_TpProteina = "CVA_TIPO_PROTEINA";
        public const string TB_DesContrato = "CVA_DES_CONTRATO";
        public const string TB_TpProteinaDES = "CVA_DES_TP_PROT";
        public const string TB_Gramatura = "CVA_GRAMATURA";
        public const string TB_Incidencia = "CVA_INCIDENCIA";

        public IncidenciaDeProteinaForm()
        {
            Type = "CARDINCP";
            TableName = "CVA_TABGRAMATURA";
            MenuItem = Type;
            FilePath = TableName;
            TITLE = "[CVA] Inc. Prot. Gramatura";
            //MatrixItens = "mtxGrps";
            //ChildName = "CVA_LIN_GRPSERVICOS";
            //IdToEvaluateGridEmpty = "U_CVA_ID_SERVICO";

            //ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
            ConfigureChooseFromListForNonObjectTable("OOAT", "1250010024", "7", "Number", "3", $"U_{TB_Contrato}", "1250000025");
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, TITLE, SAPbobsCOM.BoUTBTableType.bott_NoObject);
            userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesContrato, "Descr. Contrato", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_TpProteina, "Tipo de Proteína", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_TpProteinaDES, "Descr. Tipo de Proteína", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_Gramatura, "Gramatura", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Measurement, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_Incidencia, "Incidência", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
        }

        internal override void LoadDefault(Form oForm)
        {
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList(f);
            var mtx = (IMatrix)oForm.Items.Item("3").Specific;
            mtx.Columns.Item("U_CVA_DES_CONTRATO").Editable = false;
            mtx.Columns.Item("U_CVA_DES_TP_PROT").Editable = false;
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            Form oForm;
            var openMenu = OpenMenu(MenuItem, FilePath, pVal, out oForm);

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            if (pVal.FormUID.Equals(FORMUID))
            {
                if (!pVal.BeforeAction)
                {
                    #region Contrato CHOOSE

                    if (pVal.ItemUID.Equals("3") && pVal.ColUID.Equals("U_CVA_ID_CONTRATO") && pVal.EventType == BoEventTypes.et_VALIDATE && pVal.ItemChanged)
                    {
                        var oForm = Application.Forms.Item(pVal.FormUID);
                        var mtx = (IMatrix)oForm.Items.Item("3").Specific;
                        var code = ((IEditText)mtx.Columns.Item("U_CVA_ID_CONTRATO").Cells.Item(pVal.Row).Specific).Value;

                        var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        rec.DoQuery($@"
                                SELECT 
                                    OO.{"Number".Aspas()}, 
                                    OB.{"BPLName".Aspas()} 
                                FROM OOAT AS OO
                                INNER JOIN OBPL AS OB ON 
                                    OO.{ "U_CVA_FILIAL".Aspas()} = OB.{ "BPLId".Aspas()}
                                WHERE {"Number".Aspas()} = '{code}'
                        ;");

                        if (!rec.EoF)
                        {
                            var item = mtx.Columns.Item("U_CVA_DES_CONTRATO");
                            item.Editable = true;
                            var specific = ((IEditText)item.Cells.Item(pVal.Row).Specific);
                            specific.Value = rec.Fields.Item("BPLName").Value.ToString();
                            mtx.SetCellFocus(pVal.Row, 4);
                            item.Editable = false;
                        }
                    }

                    #endregion

                    #region Tipo de Proteina CHOOSE

                    if (pVal.ItemUID.Equals("3") && pVal.ColUID.Equals("U_CVA_TIPO_PROTEINA") && pVal.EventType == BoEventTypes.et_VALIDATE && pVal.ItemChanged)
                    {
                        var oForm = Application.Forms.Item(pVal.FormUID);
                        var mtx = (IMatrix)oForm.Items.Item("3").Specific;
                        var code = ((IEditText)mtx.Columns.Item("U_CVA_TIPO_PROTEINA").Cells.Item(pVal.Row).Specific).Value;

                        var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        rec.DoQuery($@"
                            SELECT 
                                {"Code".Aspas()}, 
                                {"Name".Aspas()} 
                            FROM {"@CVA_TIPOPROTEINA".Aspas()}                             
                            WHERE {"Code".Aspas()} = '{code}'
                        ;");

                        if (!rec.EoF)
                        {
                            var item = mtx.Columns.Item("U_CVA_DES_TP_PROT");
                            item.Editable = true;
                            var specific = ((IEditText)item.Cells.Item(pVal.Row).Specific);
                            specific.Value = rec.Fields.Item("Name").Value.ToString();
                            mtx.SetCellFocus(pVal.Row, 4);
                            item.Editable = false;
                        }
                    }

                    #endregion

                }
            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPDADOSC", Type, "Incidência Prot. Gramatura", 5, BoMenuType.mt_STRING);
        }


        public void CreateChooseFromList(Form oForm)
        {
            #region Tipo de Proteina
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            string strSql = $@"SELECT * FROM {"@CVA_TIPOPROTEINA".Aspas()} ;";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Proteína Incid.", idCategoria, oForm.TypeEx, "3", $"U_{TB_TpProteina}");
            #endregion

            //#region Contrato
            //strSql = $@"SELECT 
            //                OO.{"Number".Aspas()}, 
            //                OB.{"BPLName".Aspas()} 
            //            FROM OOAT AS OO
            //            INNER JOIN OBPL AS OB ON 
            //                OO.{ "U_CVA_FILIAL".Aspas()} = OB.{ "BPLId".Aspas()}
            //; ";

            //FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Proteína Incid.", idCategoria, oForm.TypeEx, "3", $"U_{TB_TpProteina}");
            //#endregion
        }
    }
}