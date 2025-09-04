using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.Apetit.Cardapio.View
{
    public class TiposDePratoForm : BaseForm
    {
        //Campos da tabela
        /// <summary>
        /// Y -> SIM
        /// N -> Não
        /// </summary>
        public const string TB_Proteina = "CVA_PROTEINA";
        public const string TB_Familia = "CVA_FAMILIA";
        public const string TB_FamiliaDes = "CVA_D_FAMILIA";
        public const string TB_SubFamilia = "CVA_SUB_FAMILIA";
        public const string TB_SubFamiliaDes = "CVA_D_SUB_FAMILIA";

        public TiposDePratoForm()
        {
            Type = "CARDTPPR";
            TableName = "CVA_TIPOPRATO";
            MenuItem = Type;
            FilePath = TableName;
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();
            UserTables.CreateIfNotExist(TableName, "[CVA] Tipos de Prato", SAPbobsCOM.BoUTBTableType.bott_NoObject);
            userFields.CreateIfNotExist("@" + TableName, TB_Familia, "ID Familia", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_FamiliaDes, "Desc Familia", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_SubFamilia, "ID Sub-Familia", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_SubFamiliaDes, "Desc Sub-Familia", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_Proteina, "Proteína ?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
        }

        internal override void LoadDefault(Form oForm)
        {
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList();
            var mtx = (IMatrix)oForm.Items.Item("3").Specific;
            mtx.Columns.Item("U_CVA_D_FAMILIA").Editable = false;
            mtx.Columns.Item("U_CVA_D_SUB_FAMILIA").Editable = false;
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

            if (FormUID.Equals(FORMUID))
            {
                #region Id Familia

                if (pVal.ColUID.Equals("U_CVA_FAMILIA") && pVal.EventType == BoEventTypes.et_VALIDATE && !pVal.BeforeAction && pVal.ItemChanged)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var mtx = (IMatrix)oForm.Items.Item("3").Specific;
                    var codeFamily = ((IEditText)mtx.Columns.Item("U_CVA_FAMILIA").Cells.Item(pVal.Row).Specific).Value;
                    var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    rec.DoQuery($@"
                        SELECT 
                             c.{"Code".Aspas()}
                            ,c.{"Name".Aspas()}
                        FROM {"@CVA_FAMILIA".Aspas()} as c
                        WHERE c.{ "Code".Aspas()} = '{codeFamily}'
                    ;");

                    if (!rec.EoF)
                    {
                        var item = mtx.Columns.Item("U_CVA_D_FAMILIA");
                        item.Editable = true;
                        var specific = ((IEditText)item.Cells.Item(pVal.Row).Specific);
                        specific.Value = rec.Fields.Item("Name").Value.ToString();
                        mtx.SetCellFocus(pVal.Row, 4);
                        item.Editable = false;
                    }
                }

                #endregion

                #region Id SubFamilia

                if (pVal.ColUID.Equals("U_CVA_SUB_FAMILIA") && pVal.EventType == BoEventTypes.et_VALIDATE && !pVal.BeforeAction && pVal.ItemChanged)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var mtx = (IMatrix)oForm.Items.Item("3").Specific;
                    var codeFamily = ((IEditText)mtx.Columns.Item("U_CVA_FAMILIA").Cells.Item(pVal.Row).Specific).Value;
                    var codeSubFamily = ((IEditText)mtx.Columns.Item("U_CVA_SUB_FAMILIA").Cells.Item(pVal.Row).Specific).Value;
                    var rec = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    rec.DoQuery($@"
                        SELECT 
                             c.{"Code".Aspas()}
                            ,c.{"Name".Aspas()}
                        FROM {"@CVA_SUBFAMILA".Aspas()} as c
                        WHERE   c.{ "U_CVA_Familia".Aspas()} = '{codeFamily}'
                            AND c.{ "Code".Aspas()} = '{codeSubFamily}'
                    ;");


                    if (!rec.EoF)
                    {
                        var item = mtx.Columns.Item("U_CVA_D_SUB_FAMILIA");
                        item.Editable = true;

                        var specific = ((IEditText)item.Cells.Item(pVal.Row).Specific);
                        try { specific.Value = rec.Fields.Item("Name").Value.ToString(); }
                        catch { specific.Value = rec.Fields.Item("Name").Value.ToString().Substring(0, 15); }

                        mtx.SetCellFocus(pVal.Row, 6);
                        item.Editable = false;
                    }
                }

                #endregion
            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);

            Filters.Add(Type, BoEventTypes.et_COMBO_SELECT);
            Filters.Add(Type, BoEventTypes.et_CHOOSE_FROM_LIST);
            Filters.Add(Type, BoEventTypes.et_FORMAT_SEARCH_COMPLETED);
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

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPCONFIG", "CARDTPPR", "Tipos de Prato", 2, SAPbouiCOM.BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList()
        {
            #region Busca Familia
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            string strSql = $@"
                                SELECT 
                                     c.{"Code".Aspas()}
                                    ,c.{"Name".Aspas()}
                                FROM {"@CVA_FAMILIA".Aspas()} as c ;
            ";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Familia produto", idCategoria, TYPEEX, "3", "U_CVA_FAMILIA");//CVA_FAMILIA
            #endregion

            #region Busca Sub Familia
            strSql = $@"
                        SELECT 
                             c.{"Code".Aspas()}
                            ,c.{"Name".Aspas()}
                        FROM {"@CVA_SUBFAMILA".Aspas()} as c 
                        WHERE c.{ "U_CVA_Familia".Aspas()} = RTRIM(LTRIM($[$3.U_CVA_FAMILIA.0])) 
            ;";

            FormatedSearch.CreateFormattedSearches(strSql, "Busca Sub-Familia produto", idCategoria, TYPEEX, "3", "U_CVA_SUB_FAMILIA");//CVA_SUB_FAMILIA
            #endregion
        }
    }
}