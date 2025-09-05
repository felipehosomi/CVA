
using ui = SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Repository.Services;

namespace App.ApplicationServices
{
    public class CreationUserMenu
    {
        #region Objcet Variables
        private string str_menu_description;
        private string parent_id;
        private string unique_id;
        private SAPbouiCOM.BoMenuType type;
        private int menu_position;
        private string _image;

        #endregion

        #region Object Properties
        public string Image
        {
            get { return this._image; }
            set { this._image = value; }
        }

        public string Description
        {
            get { return this.str_menu_description; }
            set { this.str_menu_description = value; }
        }
        public string Parent
        {
            get { return this.parent_id; }
            set { this.parent_id = value; }
        }
        public string UniqueID
        {
            get { return this.unique_id; }
            set { this.unique_id = value; }
        }
        public ui.BoMenuType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public int Position
        {
            get { return this.menu_position; }
            set { this.menu_position = value; }
        }
        #endregion

        #region Constructors
        public CreationUserMenu() { }
        public CreationUserMenu(string Description, string Parent, string UniqueID, ui.BoMenuType Type, int Position, string Image)
        {
            this.str_menu_description = Description;
            this.parent_id = Parent;
            this.unique_id = UniqueID;
            this.type = Type;
            this.menu_position = Position;
            this._image = Image;

        }
        #endregion

        #region Object Methods
        public void Add()
        {
            SAPbouiCOM.MenuItem oMenuItem;
            SAPbouiCOM.Menus oMenus;
            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            // 'Seta Objeto Menu

            oMenus = AddonService.uiApplication.Menus;
            oMenuItem = AddonService.uiApplication.Menus.Item(this.parent_id);

            oMenus = oMenuItem.SubMenus;
            // 'Seta Objeto Menu Creatrion Parameters;
            oCreationPackage = (ui.MenuCreationParams)AddonService.uiApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
            if (oMenus.Exists(this.unique_id))
            {
                Remove();
            }

            //'Cria SubMenu 
            if (this._image != "")
            {
                string appPath = System.Windows.Forms.Application.StartupPath;
                if (!appPath.EndsWith(@"\")) appPath += @"\";
                oCreationPackage.Image = appPath + this._image;
            }
            oCreationPackage.Type = this.type;
            oCreationPackage.UniqueID = this.unique_id;
            oCreationPackage.String = this.str_menu_description;
            oCreationPackage.Position = this.menu_position;
            oMenus.AddEx(oCreationPackage);

            oMenuItem.Release();
            oMenus.Release();
            oCreationPackage.Release();
        }

        public void Remove()
        {
            SAPbouiCOM.MenuItem oMenuItem;
            SAPbouiCOM.Menus oMenus;

            oMenus = AddonService.uiApplication.Menus;
            oMenuItem = AddonService.uiApplication.Menus.Item(this.unique_id);

            if (oMenus.Exists(this.unique_id))
                oMenus.RemoveEx(this.unique_id);

            oMenuItem.Release();
            oMenus.Release();
        }
        #endregion
    }
}
