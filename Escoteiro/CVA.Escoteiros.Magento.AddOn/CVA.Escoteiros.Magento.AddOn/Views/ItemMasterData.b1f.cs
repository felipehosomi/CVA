using System;
using System.Linq;
using SAPbouiCOM.Framework;
using System.Xml;
using SAPbouiCOM;
using Application = SAPbouiCOM.Framework.Application;
using CVA.Escoteiros.Magento.AddOn.Controller;
using CVA.Escoteiros.Magento.AddOn.Model.Magento;

namespace CVA.Escoteiros.Magento.AddOn.Views
{
    [FormAttribute("150", "Views/ItemMasterData.b1f")]
    class ItemMasterData : SystemFormBase
    {
        private static bool _afterDataLoad;

        public ItemMasterData()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtCats = ((SAPbouiCOM.Matrix)(this.GetItem("mtCats").Specific));
            this.mtCats.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.mtCats_PressedAfter);
            this.mtSelCats = ((SAPbouiCOM.Matrix)(this.GetItem("mtSelCats").Specific));
            this.mtSelCats.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.mtSelCats_PressedAfter);
            this.stCats = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.stSelCats = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.etAux = ((SAPbouiCOM.EditText)(this.GetItem("etAux").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += SBO_Application_MenuEvent;

            SetCategoriesMatrix();
            LoadNewElements();

            stCats.Item.TextStyle = 4;
            stSelCats.Item.TextStyle = 4;

            fdCats = ((SAPbouiCOM.Folder)(GetItem("fdCats").Specific));
            fdCats.PressedAfter += fdCats_PressedAfter;
            fdCats.Item.Left = (UIAPIRawForm.Items.Item("27").Left + UIAPIRawForm.Items.Item("27").Width) - 10;

            if (!_afterDataLoad) return;

            // Realiza o carregamento das categorias selecionadas no OnCustomInitialize
            // quando acesso ao cadastro do item é realizado através de um linked button
            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtCatsBak = UIAPIRawForm.DataSources.DataTables.Item("dtCatsBak");
            var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");
            var oitm = UIAPIRawForm.DataSources.DBDataSources.Item("OITM");

            mtCats.FlushToDataSource();
            mtSelCats.FlushToDataSource();

            dtSelCats.Rows.Clear();

            dtCategories.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, dtCatsBak.SerializeAsXML(BoDataTableXmlSelect.dxs_All));

            ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtSelCats, oitm.GetValue("ItemCode", oitm.Offset));

            mtCats.LoadFromDataSourceEx();
            mtSelCats.LoadFromDataSourceEx();
            
            _afterDataLoad = false;
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
                        case "1282":
                            {
                                var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
                                var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");
                                var oitm = UIAPIRawForm.DataSources.DBDataSources.Item("OITM");

                                if (pVal.MenuUID == "ReloadCategories") ItemMasterDataController.Categories = null;

                                SetCategoriesMatrix();

                                if (!String.IsNullOrEmpty(oitm.GetValue("ItemCode", oitm.Offset)))
                                {
                                    ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtSelCats, oitm.GetValue("ItemCode", oitm.Offset));
                                    mtCats.LoadFromDataSourceEx();
                                    mtSelCats.LoadFromDataSourceEx();
                                }

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

        private void LoadNewElements()
        {
            var appPath = Environment.CurrentDirectory;
            if (!appPath.EndsWith(@"\")) appPath += @"\";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(string.Format(@"{0}xml\ItemMasterData.xml", appPath));

            if (xmlDoc.DocumentElement != null)
            {
                var selectSingleNode = xmlDoc.DocumentElement.SelectSingleNode("/Application/forms/action/form/@uid");

                if (selectSingleNode != null)
                {
                    selectSingleNode.Value = UIAPIRawForm.UniqueID;
                }
            }

            var innerXml = xmlDoc.InnerXml;

            Application.SBO_Application.LoadBatchActions(ref innerXml);
        }

        private void SetCategoriesMatrix()
        {
            var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
            var dtCatsBak = UIAPIRawForm.DataSources.DataTables.Item("dtCatsBak");

            dtCategories.Rows.Clear();

            // Caso as categorias ainda não tenham sido obtidas na API do Magento
            if (ItemMasterDataController.Categories == null)
            {
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

        private void fdCats_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.PaneLevel = 20;
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            fdCats.Item.Left = (UIAPIRawForm.Items.Item("27").Left + UIAPIRawForm.Items.Item("27").Width) - 10;

            mtSelCats.Item.Left = UIAPIRawForm.Width / 2 + 15;
            mtSelCats.Item.Width = UIAPIRawForm.Width - mtSelCats.Item.Left - 40;
            mtCats.Item.Width = mtSelCats.Item.Left - 20;
            stSelCats.Item.Left = mtSelCats.Item.Left;

            mtCats.AutoResizeColumns();
            mtSelCats.AutoResizeColumns();
        }

        private void mtCats_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Select")
            {
                mtCats.FlushToDataSource();

                var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
                var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");

                if (dtCategories.GetValue("Select", pVal.Row - 1).ToString() == "Y")
                {
                    dtSelCats.Rows.Add();

                    //dtSelCats.SetValue("LineNum", dtSelCats.Rows.Count - 1, dtSelCats.Rows.Count);
                    dtSelCats.SetValue("Select", dtSelCats.Rows.Count - 1, "Y");
                    dtSelCats.SetValue("ID", dtSelCats.Rows.Count - 1, dtCategories.GetValue("ID", pVal.Row - 1));
                    dtSelCats.SetValue("Name", dtSelCats.Rows.Count - 1, dtCategories.GetValue("Name", pVal.Row - 1).ToString().Replace("▼", "").TrimStart('\u00A0').TrimStart(' '));

                    UIAPIRawForm.Freeze(true);
                    mtSelCats.LoadFromDataSourceEx();
                    UIAPIRawForm.Freeze(false);
                }
                else
                {
                    for (var i = 0; i < mtSelCats.RowCount; i++)
                    {
                        if (dtSelCats.GetValue("ID", i).ToString() != dtCategories.GetValue("ID", pVal.Row - 1).ToString()) continue;

                        dtSelCats.Rows.Remove(i);

                        UIAPIRawForm.Freeze(true);
                        mtSelCats.LoadFromDataSourceEx();
                        UIAPIRawForm.Freeze(false);
                    }
                }
            }
        }

        private void mtSelCats_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Select")
            {
                try
                {
                    mtSelCats.FlushToDataSource();

                    var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
                    var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");

                    if (dtSelCats.GetValue("Select", pVal.Row - 1).ToString() == "N")
                    {
                        for (var i = 0; i < mtCats.RowCount; i++)
                        {
                            if (dtCategories.GetValue("ID", i).ToString() != dtSelCats.GetValue("ID", pVal.Row - 1).ToString()) continue;

                            UIAPIRawForm.ActiveItem = "etAux";
                            dtCategories.SetValue("Select", i, "N");

                            dtSelCats.Rows.Remove(pVal.Row - 1);

                            UIAPIRawForm.Freeze(true);
                            mtCats.LoadFromDataSourceEx();
                            mtSelCats.LoadFromDataSourceEx();
                            UIAPIRawForm.Freeze(false);

                            return;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pVal.ObjectKey);
            var itemCode = xmlDocument.GetElementsByTagName("ItemCode")[0].InnerXml;
            var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");
            mtSelCats.FlushToDataSource();

            if (ItemMasterDataController.SetItemCategories(dtSelCats, itemCode) != 0)
            {
                Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), BoMessageTime.bmt_Short);
            }
        }

        private void Form_DataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pVal.ObjectKey);
            var itemCode = xmlDocument.GetElementsByTagName("ItemCode")[0].InnerXml;
            var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");
            mtSelCats.FlushToDataSource();

            ItemMasterDataController.RemoveSelectedItemCategories(itemCode);

            if (ItemMasterDataController.SetItemCategories(dtSelCats, itemCode) != 0)
            {
                Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), BoMessageTime.bmt_Short);
            }
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            try
            {
                var dtCategories = UIAPIRawForm.DataSources.DataTables.Item("dtCategories");
                var dtCatsBak = UIAPIRawForm.DataSources.DataTables.Item("dtCatsBak");
                var dtSelCats = UIAPIRawForm.DataSources.DataTables.Item("dtSelCats");
                var oitm = UIAPIRawForm.DataSources.DBDataSources.Item("OITM");

                mtCats.FlushToDataSource();
                mtSelCats.FlushToDataSource();

                dtSelCats.Rows.Clear();

                dtCategories.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, dtCatsBak.SerializeAsXML(BoDataTableXmlSelect.dxs_All));

                ItemMasterDataController.GetSelectedItemCategories(dtCategories, dtSelCats, oitm.GetValue("ItemCode", oitm.Offset));

                mtCats.LoadFromDataSourceEx();
                mtSelCats.LoadFromDataSourceEx();
            }
            catch
            {
                _afterDataLoad = true;
            }
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

        private EditText etAux;

        private Folder fdCats;

        private Matrix mtCats;
        private Matrix mtSelCats;

        private StaticText stCats;
        private StaticText stSelCats;
    }
}
