using CVA.View.Apetit.Helpers;
using SAPbouiCOM;
using System;
using System.Xml;

namespace CVA.View.Apetit.View
{
    public abstract class BaseForm
    {
        public string ButtonOk = "1";
        public string ButtonCancelar = "2";

        internal abstract void LoadDefault(Form oForm);
        public abstract void Application_MenuEvent(SAPbouiCOM.Application Application, ref SAPbouiCOM.MenuEvent pVal, out bool bubbleEvent);
        public abstract void Application_ItemEvent(SAPbouiCOM.Application Application, string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent);
        public abstract void SetFilters();
        public abstract void Application_FormDataEvent(SAPbouiCOM.Application Application, ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent);
        public abstract void Application_AppEvent(SAPbouiCOM.Application Application, SAPbouiCOM.BoAppEventTypes eventType);
        public abstract void SetMenus();

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

        public Form LoadForm(string formPath)
        {
            var oXmlDoc = new XmlDocument();
            var oCreationPackage = (FormCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            oCreationPackage.UniqueID = $"{oCreationPackage.UniqueID}{Guid.NewGuid().ToString().Substring(2, 10)}";

            oXmlDoc.Load(formPath);
            oCreationPackage.XmlData = oXmlDoc.InnerXml;

            var frm = B1Connection.Instance.Application.Forms.AddEx(oCreationPackage);

            LoadDefault(frm);

            return frm;
        }        
    }
}