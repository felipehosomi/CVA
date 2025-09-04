using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class DiasSemConsumoForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Contrato = "CVA_ID_CONTRATO";
        public const string TB_GrpServico = "CVA_GRPSERVICO";
        public const string TB_DesGrpServico = "CVA_DES_GRPSERVICO";

        //Campos da Linha
        public const string CH_Motivo = "CVA_MOTIVO";
        public const string CH_Data = "CVA_DATA";

        public DiasSemConsumoForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDCALE";
            TableName = "CVA_CALENDSC";
            ChildName = "CVA_LIN_CALENDSC";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            IdToEvaluateGridEmpty = "l_data";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Dias sem Consumo", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_GrpServico, "ID Grupo Serv.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_DesGrpServico, "Descr. Grupo Ser.", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Linhas Dias sem Consumo", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_Motivo, "Motivo", 250, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_Data, "Data", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, "[CVA] Dias sem Consumo", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
                userFields.AddChildTableToUserObject(Type, ChildName);
            }

            #endregion
        }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            //oForm.Freeze(false);
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            Form oForm;
            var openMenu = OpenMenu(MenuItem, FilePath, pVal, out oForm);

            if (!string.IsNullOrEmpty(openMenu))
            {
                ret = false;
                Application.SetStatusBarMessage(openMenu);
            }

            bubbleEvent = ret;
        }
        
        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.FormTypeEx.Equals(Type))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.ItemUID.Equals("edtGrpS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);

                            var iChoose = (ChooseFromListEvent)pVal;
                            DataTable dataTable = iChoose.SelectedObjects;

                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                string itemCode = dataTable.GetValue("Code", 0).ToString();
                                string itemName = dataTable.GetValue("U_CVA_DESCRICAO", 0).ToString();

                                ((IEditText)oForm.Items.Item("edtGrpD").Specific).Value = itemName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

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
            Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Dias Sem Consumo", 8, BoMenuType.mt_STRING);
        }
    }
}