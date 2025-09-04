using CVA.View.Apetit.Cardapio.Helpers;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace CVA.View.Apetit.Cardapio.View
{
    public abstract class BaseForm
    {
        public string ButtonOk = "1";
        public string ButtonCancelar = "2";
        public string MatrixItens = "";
        public string Type = "";
        public string ChildName = "";
        public string TableName = "";
        public string MenuItem = "";
        public string FilePath = "";
        public string IdToEvaluateGridEmpty = "";
        public string MenuUID = "";
        internal string FORMUID = "";
        internal string TITLE = "";
        internal string TYPEEX = "";
        private List<ChooseFromListHandler> ChooseFromListHandlerList = new List<View.ChooseFromListHandler>();

        internal void ConfigureChooseFromListForNonObjectTable(string choose_table, string choose_form, string choose_mtx, string choose_field, string choose_destiny_mtx, string choose_destiny_field, string choose_objtype)
        {
            ChooseFromListHandlerList.Add(new ChooseFromListHandler(choose_table, choose_form, choose_mtx, choose_field, choose_destiny_mtx, choose_destiny_field, choose_objtype));
        }

        private Dictionary<string, Dictionary<string, bool>> navigationProperties = new Dictionary<string, Dictionary<string, bool>>();

        public abstract void CreateUserFields();

        internal abstract void LoadDefault(Form oForm);
        internal abstract void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent);
        internal abstract void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent);
        public abstract void SetFilters();
        internal abstract void FormDataEvent(Application Application, ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent);

        public abstract void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent);

        public abstract void SetMenus();

        public void Application_MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            //if (pVal.MenuUID == "1282") //ADICIONAR 
            //if (pVal.MenuUID == "1281") //PROCURAR
            //if (pVal.MenuUID == "1289") //ALTERIOR
            //if (pVal.MenuUID == "1288") //PROXIMO
            //if (pVal.MenuUID == "1290") //PRIMEIRO
            //if (pVal.MenuUID == "1291") //ULTIMO

            try
            {
                if (!pVal.BeforeAction)
                {
                    //var oForm = Application.Forms.ActiveForm; //14 batidas com exception
                    var menuUID = pVal.MenuUID;
                    if (menuUID.Equals(Type))
                    {
                        if (!string.IsNullOrEmpty(MatrixItens))
                        {
                            if (pVal.MenuUID == "1282") //ADICIONAR
                            {
                                var oForm = Application.Forms.ActiveForm;
                                AddMatrixWhiteLine(oForm);
                            }

                            foreach (var item in navigationProperties)
                            {
                                if (item.Value.ContainsKey(menuUID))
                                {
                                    var oForm = Application.Forms.ActiveForm;
                                    oForm.Items.Item(item.Key).Enabled = item.Value[menuUID];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { //VERIFICAR ERRO
            }

            MenuEvent(Application, ref pVal, out bubbleEvent);
        }

        private void AddMatrixWhiteLine(Form oForm)
        {
            var oMatrix = (Matrix)oForm.Items.Item(MatrixItens).Specific;

            oForm.DataSources.DBDataSources.Item("@" + ChildName).Clear();
            oMatrix.AddRow(1);
            oMatrix.FlushToDataSource();
        }

        public void Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (BusinessObjectInfo.FormTypeEx.Equals(Type) && !string.IsNullOrEmpty(TableName))
                {
                    if ((BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_UPDATE) || BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD)))
                    {
                        var oForm = B1Connection.Instance.Application.Forms.ActiveForm;

                        if (BusinessObjectInfo.BeforeAction)
                        {
                            oForm.Freeze(true);

                            try
                            {
                                //string nextCode;
                                var code = oForm.DataSources.DBDataSources.Item("@" + TableName).GetValue("Code", 0);

                                if (BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD) || BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_UPDATE))
                                {
                                    if (TableName == "CVA_PLANEJAMENTO" && BusinessObjectInfo.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD))
                                    {
                                        code = DIHelper.GetNextCode(TableName).ToString();
                                        oForm.DataSources.DBDataSources.Item("@" + TableName).SetValue("Code", 0, code);
                                    }

                                    oForm.DataSources.DBDataSources.Item("@" + TableName).SetValue("Name", 0, code);
                                }
                                //else
                                //    nextCode = oForm.DataSources.DBDataSources.Item("@" + TableName).GetValue("Code", 0).ToString();

                                RemoveMatrixLines(oForm);
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                            finally
                            {
                                oForm.Freeze(false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            if (ret)
                FormDataEvent(B1Connection.Instance.Application, ref BusinessObjectInfo, out ret);

            bubbleEvent = ret;
        }

        private void RemoveMatrixLines(Form oForm)
        {
            if (!string.IsNullOrEmpty(MatrixItens) && !string.IsNullOrEmpty(IdToEvaluateGridEmpty))
            {
                #region Remover Linhas em Branco da Grid
                var isRemove = false;
                Matrix oMtx = (Matrix)oForm.Items.Item(MatrixItens).Specific;
                var edtCells = oMtx.Columns.Item(IdToEvaluateGridEmpty).Cells;
                DBDataSource dbDataSources;
                dbDataSources = oForm.DataSources.DBDataSources.Item("@" + ChildName);

                for (int i = oMtx.RowCount; i > 0; i--)
                {
                    if (HasOrIsWhiteLine(oForm, i))
                    {
                        try
                        {
                            if (dbDataSources.Size > (i - 1))
                                dbDataSources.RemoveRecord(i - 1);
                        }
                        catch { }
                        isRemove = true;
                    }
                }

                //if (isRemove)
                //    oMtx.LoadFromDataSource();
                #endregion
            }
            //{
            //    #region Remover Linhas em Branco da Grid
            //    var isRemove = false;

            //    Matrix oMtx = (Matrix)oForm.Items.Item(MatrixItens).Specific;
            //    DBDataSource dbDataSources;
            //    dbDataSources = oForm.DataSources.DBDataSources.Item("@" + ChildName);

            //    for (int i = dbDataSources.Size - 1; i >= 0; i--)
            //    {
            //        var value = dbDataSources.GetValue($"U_{IdToEvaluateGridEmpty}", i).ToString();
            //        if (string.IsNullOrEmpty(value) || value == "0.0")
            //        {
            //            dbDataSources.RemoveRecord(i);
            //            isRemove = true;
            //        }
            //    }

            //    if (isRemove)
            //        oMtx.LoadFromDataSource();

            //    #endregion
            //}
        }

        private bool HasOrIsWhiteLine(Form oForm, int index = -1)
        {
            if (!string.IsNullOrEmpty(MatrixItens) && !string.IsNullOrEmpty(IdToEvaluateGridEmpty))
            {
                #region Remover Linhas em Branco da Grid

                Func<string, bool> __valueTest = (string value) =>
                {
                    return (string.IsNullOrEmpty(value) || value == "0.0");
                };

                Matrix oMtx = (Matrix)oForm.Items.Item(MatrixItens).Specific;
                var edtCells = oMtx.Columns.Item(IdToEvaluateGridEmpty).Cells;
                if (index != -1)
                {
                    var val = ((EditText)edtCells.Item(index).Specific).Value;
                    return __valueTest(val);
                }

                for (int i = oMtx.RowCount; i > 0; i--)
                {
                    var value = ((EditText)edtCells.Item(i).Specific).Value;
                    if (__valueTest(value))
                        return true;
                }

                #endregion
            }

            return false;
        }

        public void Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                ItemEvent(B1Connection.Instance.Application, FormUID, ref pVal, out bubbleEvent);

                if (pVal.EventType == BoEventTypes.et_FORM_CLOSE && pVal.FormTypeEx.Equals(Type) && !pVal.BeforeAction)
                {
                    B1Connection.Instance.Application.AppEvent -= Application_AppEvent;
                    B1Connection.Instance.Application.FormDataEvent -= Application_FormDataEvent;
                    B1Connection.Instance.Application.ItemEvent -= Application_ItemEvent;
                    B1Connection.Instance.Application.RightClickEvent -= Application_RightClickEvent;
                }
            }
            catch (Exception ex)
            {
            }

            foreach (var item in ChooseFromListHandlerList)
            {
                if (pVal.EventType == BoEventTypes.et_CLICK && pVal.FormTypeEx == item.CHOOSE_FORM && pVal.ItemUID == "1")
                {
                    try
                    {
                        var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                        var oMatrix = (IMatrix)oForm.Items.Item(item.CHOOSE_MTX).Specific;
                        int selRow = oMatrix.GetNextSelectedRow();
                        var edtNumber = (EditText)oMatrix.Columns.Item(item.CHOOSE_FIELD).Cells.Item(selRow).Specific;

                        var backForm = B1Connection.Instance.Application.Forms.Item(FORMUID);

                        var mtx = (IMatrix)backForm.Items.Item(item.CHOOSE_DESTINY_MTX).Specific;
                        var selMtxRow = mtx.GetCellFocus();
                        var edtIdContr = (EditText)mtx.Columns.Item(item.CHOOSE_DESTINY_FIELD).Cells.Item(selMtxRow.rowIndex).Specific;
                        edtIdContr.Value = edtNumber.Value;
                    }
                    catch { }
                }
            }

            try
            {
                if (pVal.FormTypeEx.Equals(Type) && !string.IsNullOrEmpty(ChildName) && !string.IsNullOrEmpty(MatrixItens))
                {
                    var oForm = B1Connection.Instance.Application.Forms.ActiveForm;

                    if (pVal.BeforeAction && (oForm.Mode == BoFormMode.fm_ADD_MODE || oForm.Mode == BoFormMode.fm_UPDATE_MODE || oForm.Mode == BoFormMode.fm_OK_MODE))
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && pVal.ItemUID == MatrixItens)
                        {
                            if (!HasOrIsWhiteLine(oForm))
                                AddMatrixWhiteLine(oForm);
                        }

                        if (pVal.EventType == BoEventTypes.et_CLICK && pVal.ItemUID == "1")
                            RemoveMatrixLines(oForm);

                        if (pVal.EventType == BoEventTypes.et_VALIDATE)
                        {
                            var i = 1;
                            var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            var lastCol = mtx.Columns.Item(mtx.Columns.Count - i);

                            while (lastCol != null && !lastCol.Editable)
                            {
                                i++;
                                lastCol = mtx.Columns.Item(mtx.Columns.Count - i);
                            }

                            if (lastCol == null) lastCol = mtx.Columns.Item(mtx.Columns.Count - 1);

                            if (lastCol.UniqueID == pVal.ColUID && pVal.EventType == BoEventTypes.et_VALIDATE)
                                if (!HasOrIsWhiteLine(oForm))
                                    AddMatrixWhiteLine(oForm);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Application_AppEvent(BoAppEventTypes eventType)
        {
            switch (eventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    if (B1Connection.Instance.Application.Menus.Exists(MenuItem)) B1Connection.Instance.Application.Menus.RemoveEx(MenuItem);
                    Environment.Exit(-1);
                    break;
            }
        }

        //public void Application_AppEvent(Application Application, BoAppEventTypes eventType)
        //{
        //    switch (eventType)
        //    {
        //        case BoAppEventTypes.aet_CompanyChanged:
        //        case BoAppEventTypes.aet_FontChanged:
        //        case BoAppEventTypes.aet_LanguageChanged:
        //        case BoAppEventTypes.aet_ServerTerminition:
        //        case BoAppEventTypes.aet_ShutDown:
        //            if (Application.Menus.Exists(MenuItem)) Application.Menus.RemoveEx(MenuItem);
        //            Environment.Exit(-1);
        //            break;
        //    }
        //}

        public void Add(string parentId, string menuId, string description, int position, BoMenuType type, string imagePath = null)
        {
            MenuCreationParams oCreationPackage =
                (MenuCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            MenuItem oMenuItem = B1Connection.Instance.Application.Menus.Item(parentId);
            SAPbouiCOM.Menus oMenus = oMenuItem.SubMenus;
            var exist = (oMenus != null) && oMenuItem.SubMenus.Exists(menuId);

            if (exist)
            {
                oMenuItem.SubMenus.RemoveEx(menuId);
                exist = false;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!exist)
            {
                oCreationPackage.Type = type;
                oCreationPackage.UniqueID = menuId;
                oCreationPackage.String = description;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = position;
                oCreationPackage.Image = imagePath;

                if (oMenus == null)
                {
                    oMenuItem.SubMenus.Add(menuId, description, type, position);
                    oMenus = oMenuItem.SubMenus;
                }

                oMenus.AddEx(oCreationPackage);
            }
        }

        public string OpenMenu(string MenuItem, string FilePath, MenuEvent pVal, out Form oForm)
        {
            oForm = null;

            try
            {
                if (pVal == null || (pVal.MenuUID.Equals(MenuItem) && !pVal.BeforeAction))
                {
                    oForm = LoadForm(FilePath);
                    if (oForm != null)
                        oForm.Visible = true;

                    Filters.Add(oForm.TypeEx, BoEventTypes.et_FORM_CLOSE);

                    B1Connection.Instance.Application.AppEvent += Application_AppEvent;
                    B1Connection.Instance.Application.FormDataEvent += Application_FormDataEvent;
                    B1Connection.Instance.Application.ItemEvent += Application_ItemEvent;
                    B1Connection.Instance.Application.RightClickEvent += Application_RightClickEvent;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal void ConfigureNavigationProperties(string UID, bool adicionar, bool procurar, bool anterior, bool proximo, bool primeiro, bool ultimo, bool alterar = false)
        {
            navigationProperties.Add(UID, new Dictionary<string, bool>
            {
                { "1282",adicionar},
                { "1281",procurar},
                { "1289",anterior},
                { "1288",proximo},
                { "1290",primeiro},
                { "1291",ultimo},
                { "ALTERAR",alterar},
            });
        }

        public Form LoadForm(string formPath)
        {
            Form frm = null;
            try
            {
                if (formPath.ToLower().Contains(".srf"))
                {
                    var oXmlDoc = new XmlDocument();
                    var oCreationPackage = (FormCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

                    oCreationPackage.UniqueID = $"{oCreationPackage.UniqueID}{Guid.NewGuid().ToString().Substring(2, 10)}";

                    oXmlDoc.Load(formPath);
                    oCreationPackage.XmlData = oXmlDoc.InnerXml;

                    frm = B1Connection.Instance.Application.Forms.AddEx(oCreationPackage);
                }
                else
                {
                    if (string.IsNullOrEmpty(MenuUID))
                    {
                        var menuUID = GetTableUID(formPath);
                        MenuUID = menuUID;
                    }

                    B1Connection.Instance.Application.Menus.Item(MenuUID).Activate();

                    frm = B1Connection.Instance.Application.Forms.ActiveForm;
                }

                foreach (var item in ChooseFromListHandlerList)
                {
                    Filters.Add(item.CHOOSE_FORM, BoEventTypes.et_CHOOSE_FROM_LIST);
                    Filters.Add(item.CHOOSE_FORM, BoEventTypes.et_B1I_SERVICE_COMPLETE);
                    Filters.Add(item.CHOOSE_FORM, BoEventTypes.et_CLICK);
                }

                if (frm != null)
                {
                    Filters.Add(frm.TypeEx, BoEventTypes.et_COMBO_SELECT);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_CHOOSE_FROM_LIST);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_PICKER_CLICKED);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_FORMAT_SEARCH_COMPLETED);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_VALIDATE);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_LOST_FOCUS);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_FORM_DATA_ADD);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_FORM_DATA_UPDATE);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_FORM_DATA_LOAD);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_ITEM_PRESSED);
                    Filters.Add(frm.TypeEx, BoEventTypes.et_MATRIX_LINK_PRESSED);

                    if (ChooseFromListHandlerList.Any())
                        CreateChooseFromList(frm);

                    FORMUID = frm.UniqueID;
                }

                TYPEEX = frm.TypeEx;

            }
            catch(Exception ex) {

            }

            if (frm == null)
                B1Connection.Instance.Application.SetStatusBarMessage("Form Nulo");
            else
                LoadDefault(frm);

            return frm;
        }

        public void CreateChooseFromList(Form oForm)
        {
            if (oForm.TypeEx.Equals(Type) || oForm.Title.Equals(TITLE))
            {
                foreach (var item in ChooseFromListHandlerList)
                {
                    var mtx = (IMatrix)oForm.Items.Item(item.CHOOSE_DESTINY_MTX).Specific;
                    var oColumn = mtx.Columns.Item(item.CHOOSE_DESTINY_FIELD);
                    var ocflCreationParam = B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams) as ChooseFromListCreationParams;

                    ocflCreationParam.MultiSelection = false;
                    ocflCreationParam.ObjectType = item.CHOOSE_OBJTYPE;
                    ocflCreationParam.UniqueID = item.CHOOSE_TABLE;

                    var oCFLs = oForm.ChooseFromLists;
                    var oCFL = oCFLs.Add(ocflCreationParam);

                    oColumn.ChooseFromListUID = item.CHOOSE_TABLE;
                    oColumn.ChooseFromListAlias = item.CHOOSE_FIELD;
                }
            }
        }

        public string GetTableUID(string tableName)
        {
            var ret = "";

            var oMenu = (MenuItem)B1Connection.Instance.Application.Menus.Item("51200");
            for (int i = 0; i < oMenu.SubMenus.Count; i++)
            {
                var menuSplit = oMenu.SubMenus.Item(i).String.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (menuSplit.Length < 2) continue;

                var menuName = menuSplit[0].Trim();

                if (menuName.Equals(tableName))
                {
                    ret = oMenu.SubMenus.Item(i).UID;
                    break;
                }
            }

            return ret;
        }
    }

    public class ChooseFromListHandler
    {
        public ChooseFromListHandler(string cHOOSE_TABLE, string cHOOSE_FORM, string cHOOSE_MTX, string cHOOSE_FIELD, string cHOOSE_DESTINY_MTX, string cHOOSE_DESTINY_FIELD, string cHOOSE_OBJTYPE)
        {
            CHOOSE_TABLE = cHOOSE_TABLE;
            CHOOSE_FORM = cHOOSE_FORM;
            CHOOSE_MTX = cHOOSE_MTX;
            CHOOSE_FIELD = cHOOSE_FIELD;
            CHOOSE_DESTINY_MTX = cHOOSE_DESTINY_MTX;
            CHOOSE_DESTINY_FIELD = cHOOSE_DESTINY_FIELD;
            CHOOSE_OBJTYPE = cHOOSE_OBJTYPE;
        }

        public string CHOOSE_TABLE { get; }
        public string CHOOSE_FORM { get; }
        public string CHOOSE_MTX { get; }
        public string CHOOSE_FIELD { get; }
        public string CHOOSE_OBJTYPE { get; }

        public string CHOOSE_DESTINY_MTX { get; }
        public string CHOOSE_DESTINY_FIELD { get; }
    }
}