using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class ItensDenegadosForm : BaseForm
    {
        //Campos da tabela
        public const string TB_Contrato = "CVA_ID_CONTRATO";
        public const string TB_Descricao = "CVA_DESCRICAO";
        public const string CH_ItemCode = "CVA_ITEMCODE";
        public const string CH_ItemName = "CVA_ITEMNAME";

        public ItensDenegadosForm()
        {
            MatrixItens = "mtxGrps";
            Type = "CARDITDN";
            TableName = "CVA_BLOQUEN";
            ChildName = "CVA_LIN_BLOQUEN";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf"; 
            IdToEvaluateGridEmpty = "edt_l_codS";

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

            UserTables.CreateIfNotExist(TableName, "[CVA] Itens Denegados", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Contrato, "ID Contrato", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_Descricao, "Descrição", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            UserTables.CreateIfNotExist(ChildName, "[CVA] Linhas It. Denegados", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_ItemCode, "Código Item", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_ItemName, "Descr. Serviço", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, "[CVA] Itens Denegados", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
                        #region Serviço CHOOSE

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("edt_l_codS") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);

                            var iChoose = (ChooseFromListEvent)pVal;
                            DataTable dataTable = iChoose.SelectedObjects;

                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                string itemCode = dataTable.GetValue("ItemCode", 0).ToString();
                                string itemName = dataTable.GetValue("ItemName", 0).ToString();

                                var dtSrc = oForm.DataSources.DBDataSources.Item("@" + ChildName);
                                dtSrc.SetValue($"U_{CH_ItemCode}", pVal.Row - 1, itemCode);
                                dtSrc.SetValue($"U_{CH_ItemName}", pVal.Row - 1, itemName);
                            }
                        }

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("edt_l_codS") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            var edtContrS = ((IEditText)mtx.Columns.Item("edt_l_codS").Cells.Item(pVal.Row).Specific).Value;

                            if (string.IsNullOrEmpty(edtContrS))
                                ((IEditText)mtx.Columns.Item("edt_l_codD").Cells.Item(pVal.Row).Specific).Value = "";
                        }

                        #endregion
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
            Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Itens Denegados", 7, BoMenuType.mt_STRING);
        }
    }
}