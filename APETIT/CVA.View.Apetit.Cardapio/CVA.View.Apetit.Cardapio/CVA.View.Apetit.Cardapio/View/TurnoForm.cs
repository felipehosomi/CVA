using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class TurnoForm : BaseForm
    {
        //Campos da tabela

        public TurnoForm()
        {
            Type = "CARDTURN";
            TableName = "CVA_TURNO";
            MenuItem = Type;
            FilePath = TableName;
            TITLE = "[CVA] Turnos";
            //MatrixItens = "mtxGrps";
            //ChildName = "CVA_LIN_GRPSERVICOS";
            //IdToEvaluateGridEmpty = "U_CVA_ID_SERVICO";

            //ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
            //ConfigureChooseFromListForNonObjectTable("OOAT", "1250010024", "7", "Number", "3", $"U_{TB_Contrato}", "1250000025");
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
            //userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            //userFields.CreateIfNotExist("@" + TableName, TB_TpProteina, "Tipo de Proteína", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            //userFields.CreateIfNotExist("@" + TableName, TB_Gramatura, "Gramatura", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Measurement, SAPbobsCOM.BoYesNoEnum.tYES);
            //userFields.CreateIfNotExist("@" + TableName, TB_Incidencia, "Incidência", 5, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_Quantity, SAPbobsCOM.BoYesNoEnum.tYES);
        }

        internal override void LoadDefault(Form oForm)
        {
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            CreateChooseFromList(f);
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
            Helpers.Menus.Add("CVAPCONFIG", Type, "Turnos", 6, BoMenuType.mt_STRING);
        }


        public void CreateChooseFromList(Form oForm)
        {
            //int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");
            //string strSql = $@"SELECT * FROM {"@CVA_TIPOPROTEINA".Aspas()} ;";

            //FormatedSearch.CreateFormattedSearches(strSql, "Busca Tipo Proteína Incid.", idCategoria, oForm.TypeEx, "3", $"U_{TB_TpProteina}");
        }
    }
}