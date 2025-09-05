using EDB_Solution.Controller;
using EDB_Solution.Model.Magento;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Linq;
using Application = SAPbouiCOM.Framework.Application;

namespace EDB_Solution.Views
{
    [FormAttribute("CVA.Escoteiros.Magento.AddOn.Views.BatchItemCategoryUpdater", "Views/BatchItemCategoryUpdater.b1f")]
    class BatchItemCategoryUpdater : UserFormBase
    {
        public BatchItemCategoryUpdater()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtCats = ((SAPbouiCOM.Matrix)(this.GetItem("mtCats").Specific));
            this.stCats = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.btOK = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btOK.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btOK_PressedBefore);
            this.btCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtItems").Specific));
            this.mtItems.DoubleClickAfter += new SAPbouiCOM._IMatrixEvents_DoubleClickAfterEventHandler(this.mtItems_DoubleClickAfter);
            this.mtItems.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.mtItems_PressedAfter);
            this.mtItems.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtItems_ChooseFromListAfter);
            this.stItems = ((SAPbouiCOM.StaticText)(this.GetItem("Item_13").Specific));
            this.ckAllCats = ((SAPbouiCOM.CheckBox)(this.GetItem("ckAllCats").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += SBO_Application_MenuEvent;

            UIAPIRawForm.EnableMenu("1281", false);
            UIAPIRawForm.EnableMenu("1282", false);

            stCats.Item.TextStyle = 4;
            stItems.Item.TextStyle = 4;

            var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");
            dtItems.Rows.Add();
            dtItems.Rows.Add();
            dtItems.Rows.Remove(1);
            mtItems.LoadFromDataSourceEx();

            SetCategoriesMatrix();
        }

        private void SBO_Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        case "ReloadCategories":
                            {
                                var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
                                var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");

                                ItemMasterDataController.Categories = null;
                                SetCategoriesMatrix();

                                for (var i = 0; i < dtItems.Rows.Count - 1; i++)
                                {
                                    if (dtItems.GetValue("Select", i).ToString() == "N") continue;

                                    ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtItems.GetValue("ItemCode", i).ToString(), i + 1);
                                }

                                mtCats.LoadFromDataSourceEx();

                                if (pVal.MenuUID == "ReloadCategories")
                                {
                                    Application.SBO_Application.StatusBar.SetText("Categorias do e-commerce recarregadas.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtCats.Item.Left = UIAPIRawForm.Width / 2 - 15;
            mtCats.Item.Width = UIAPIRawForm.Width - mtCats.Item.Left - 10;
            mtItems.Item.Width = mtCats.Item.Left - 10;
            stCats.Item.Left = mtCats.Item.Left;
            ckAllCats.Item.Left = mtCats.Item.Left - 2;

            mtItems.AutoResizeColumns();
            mtCats.AutoResizeColumns();
        }

        private void mtItems_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");
            dtItems.Rows.Add(dataTable.Rows.Count);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtItems.SetValue("LineNum", (pVal.Row - 1) + i, pVal.Row + i);
                dtItems.SetValue("ItemCode", (pVal.Row - 1) + i, dataTable.GetValue("ItemCode", i));
                dtItems.SetValue("ItemName", (pVal.Row - 1) + i, dataTable.GetValue("ItemName", i));
            }

            mtItems.LoadFromDataSourceEx();
            mtCats.Columns.Item("Select").Editable = true;
        }

        private void SetCategoriesMatrix()
        {
            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtCatsBak = UIAPIRawForm.DataSources.DataTables.Item("dtCatsBak");

            dtCategories.Rows.Clear();

            // Caso as categorias ainda não tenham sido obtidas na API do Magento
            if (ItemMasterDataController.Categories == null)
            {
                Application.SBO_Application.StatusBar.SetText("Obtendo as categorias do e-commerce, aguarde.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                // Obtém as categorias existentes no Magento e as armazena em um objeto static
                ItemMasterDataController.GetCategories();

                if (ItemMasterDataController.Categories.children_data == null)
                {
                    Application.SBO_Application.StatusBar.SetText("Não foi possível obter as categorias do e-commerce por falha de comunicação.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    return;
                }

                foreach (var category in ItemMasterDataController.Categories.children_data.Where(x => x.is_active))
                {
                    SetCategoriesMatrix(dtCategories, category);
                }

                dtCatsBak.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, dtCategories.SerializeAsXML(BoDataTableXmlSelect.dxs_All));
            }
            else if (dtCatsBak.IsEmpty)
            {
                foreach (var category in ItemMasterDataController.Categories.children_data.Where(x => x.is_active))
                {
                    SetCategoriesMatrix(dtCategories, category);
                }

                dtCatsBak.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, dtCategories.SerializeAsXML(BoDataTableXmlSelect.dxs_All));
            }

            dtCategories.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, dtCatsBak.SerializeAsXML(BoDataTableXmlSelect.dxs_All));

            UIAPIRawForm.Freeze(true);
            mtCats.LoadFromDataSourceEx();
            mtCats.AutoResizeColumns();
            UIAPIRawForm.Freeze(false);
        }

        private void SetCategoriesMatrix(DataTable dtCategories, CategoryModel category)
        {
            dtCategories.Rows.Add();

            dtCategories.SetValue("LineNum", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
            dtCategories.SetValue("ID", dtCategories.Rows.Count - 1, category.id);
            // Para que o SAP não faça um trim automático nas células da matrix, deve-se utilizar o espaço sem quebra (U+00A0), inserido ao pressionar as teclas Atl + 0160
            dtCategories.SetValue("Name", dtCategories.Rows.Count - 1, new string('\u00A0', category.level == 2 ? 1 : 15 * (category.level - 2)) + (category.children_data.Count == 0 ? "\u00A0\u00A0\u00A0\u00A0\u00A0" : "▼  ") + category.name);

            foreach (var children in category.children_data.Where(x => x.is_active))
            {
                SetCategoriesMatrix(dtCategories, children);
            }
        }

        private void mtItems_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID != "Select" || pVal.Row == 0) return;

            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");

            mtCats.FlushToDataSource();
            mtItems.FlushToDataSource();

            ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtItems.GetValue("ItemCode", pVal.Row - 1).ToString(), pVal.Row, dtItems.GetValue("Select", pVal.Row - 1).ToString());

            UIAPIRawForm.Freeze(true);
            mtCats.LoadFromDataSourceEx();
            UIAPIRawForm.Freeze(false);
        }

        private void mtItems_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID != "Select" || pVal.Row != 0) return;

            mtItems.FlushToDataSource();

            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");
            var select = dtItems.GetValue("Select", 0).ToString() == "Y";

            for (var i = 0; i < dtItems.Rows.Count - 1; i++)
            {
                if (dtItems.GetValue("Select", i).ToString() == (select ? "N" : "Y")) continue;

                dtItems.SetValue("Select", i, select ? "N" : "Y");
                ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtItems.GetValue("ItemCode", i).ToString(), i + 1, select ? "N" : "Y");
            }

            UIAPIRawForm.Freeze(true);
            mtCats.LoadFromDataSourceEx();
            mtItems.LoadFromDataSourceEx();
            UIAPIRawForm.Freeze(false);
        }

        private void btOK_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (UIAPIRawForm.Mode != BoFormMode.fm_UPDATE_MODE) return;

            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");
            var setAllCategories = UIAPIRawForm.DataSources.UserDataSources.Item("AllCats").ValueEx == "Y";

            mtItems.FlushToDataSource();
            mtCats.FlushToDataSource();

            for (var i = 0; i < dtItems.Rows.Count - 1; i++)
            {
                if (dtItems.GetValue("Select", i).ToString() != "Y") continue;

                var itemCode = dtItems.GetValue("ItemCode", i).ToString();
                var lineNum = int.Parse(dtItems.GetValue("LineNum", i).ToString());

                // Remove todas as categorias às quais o item está vinculado
                ItemMasterDataController.RemoveSelectedItemCategories(itemCode);

                // Realiza a vinculação das categorias ao item
                if (ItemMasterDataController.SetItemCategories(dtCategories, itemCode, lineNum, setAllCategories) != 0)
                {
                    Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), BoMessageTime.bmt_Short);
                    continue;
                }

                // Altera o campo U_CVA_Integrated para Não para posteriormente ser integrado ao Magento
                ItemMasterDataController.SetItemToIntegrate(itemCode);
            }

            // Obtém novamente as categorias dos itens selecionados
            for (var i = 0; i < dtItems.Rows.Count - 1; i++)
            {
                ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtItems.GetValue("ItemCode", i).ToString(), i + 1, dtItems.GetValue("Select", i).ToString());
            }

            UIAPIRawForm.Freeze(true);
            mtCats.LoadFromDataSourceEx();
            UIAPIRawForm.Freeze(false);
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            switch (eventInfo.ItemUID)
            {
                case "mtCats":
                    if (eventInfo.Row == -1) return;
                    Application.SBO_Application.Menus.Item("1280").SubMenus.Item("ReloadCategories").Enabled = true;
                    break;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            switch (eventInfo.ItemUID)
            {
                case "mtCats":
                    Application.SBO_Application.Menus.Item("1280").SubMenus.Item("ReloadCategories").Enabled = false;
                    break;
            }
        }

        private SAPbouiCOM.CheckBox ckAllCats;
        private SAPbouiCOM.Matrix mtCats;
        private SAPbouiCOM.StaticText stCats;
        private SAPbouiCOM.Button btOK;
        private SAPbouiCOM.Button btCancel;
        private SAPbouiCOM.Matrix mtItems;
        private SAPbouiCOM.StaticText stItems;
    }
}
