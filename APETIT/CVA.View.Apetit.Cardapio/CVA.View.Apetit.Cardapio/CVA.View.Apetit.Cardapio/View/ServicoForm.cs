using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.Apetit.Cardapio.View
{
    public class ServicoForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Ativo = "CVA_ATIVO";

        public ServicoForm()
        {
            Type = "CARDSERV";
            TableName = "CVA_SERVICO_PLAN";
            MenuItem = Type;
            FilePath = TableName;
            //MatrixItens = "mtxGrps";
            //ChildName = "CVA_LIN_GRPSERVICOS"; 
            //IdToEvaluateGridEmpty = "U_CVA_ID_SERVICO";

            //ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();
            UserTables.CreateIfNotExist(TableName, "[CVA] Serviços", SAPbobsCOM.BoUTBTableType.bott_NoObject);
            userFields.CreateIfNotExist("@" + TableName, TB_Ativo, "ATIVO", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
        }

        internal override void LoadDefault(Form oForm)
        {
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
            Helpers.Menus.Add("CVAPCONFIG", Type, "Serviços", 1, BoMenuType.mt_STRING);
        }
    }
}