using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class AgrupamentoDeServicoForm : BaseForm
    {
        //Campos da tabela
        public const string TB_ItemCode = "CVA_ITEMCODE";

        public AgrupamentoDeServicoForm()
        {
            Type = "CARDAGDS"; 
            TableName = "CVA_AGRUP_SERVICO";
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
            UserTables.CreateIfNotExist(TableName, "[CVA] Agrupamento de Serviços", SAPbobsCOM.BoUTBTableType.bott_NoObject);
            userFields.CreateIfNotExist("@" + TableName, TB_ItemCode, "Item Faturamento", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
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
            
            bubbleEvent = ret;
        }
        
        public override void SetMenus()
        {
            Helpers.Menus.Add("CVAPCONFIG", Type, "Agrupamento de Serviços", 1, BoMenuType.mt_STRING);
        }

        public void CreateChooseFromList(Form oForm)
        {
            #region Busca OITM
            int idCategoria = FormatedSearch.CreateCategory("Addon Apetit");

            var strSql = $@"
                SELECT 
	                o.{"ItemCode".Aspas()},
                    o.{"ItemName".Aspas()}
                FROM OITM as o
                INNER JOIN OITB AS b ON
                    o.{"ItmsGrpCod".Aspas()} = b.{"ItmsGrpCod".Aspas()}
                WHERE   {"ItmsGrpNam".Aspas()} = 'FATURAMENTO'
            ; ";

            FormatedSearch.CreateFormattedSearches(strSql, "Buscar Item Fat.", idCategoria, oForm.TypeEx, "3", "U_CVA_ITEMCODE");
            #endregion
        }

    }
}