using App.Repository.Services;
using System.Runtime.InteropServices;
using ui = SAPbouiCOM;


namespace App.ApplicationServices.Addon
{
    public class UserMenuService
    {

        #region Objcet Variables
        private string str_menu_description;
        private string parent_id;
        private string unique_id;
        private ui.BoMenuType type;
        private int menu_position;
        private string menu_image;
        #endregion

        #region Object Properties
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
        public string Image
        {
            get { return this.menu_image; }
            set { this.menu_image = value; }
        }
        #endregion

        #region Constructors
        public UserMenuService() { }
        public UserMenuService(string Description, string Parent, string UniqueID, ui.BoMenuType Type, int Position, string Image)
        {
            this.str_menu_description = Description;
            this.parent_id = Parent;
            this.unique_id = UniqueID;
            this.type = Type;
            this.menu_position = Position;
            this.menu_image = Image;
        }
        #endregion

        #region Object Methods
        public void Add()
        {
            try
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
                if (!oMenus.Exists(this.unique_id))
                {
                    //'Cria SubMenu 
                    oCreationPackage.Type = this.type;
                    oCreationPackage.UniqueID = this.unique_id;
                    oCreationPackage.String = this.str_menu_description;
                    oCreationPackage.Position = this.menu_position;
                    if (!string.IsNullOrEmpty(this.menu_image))
                        oCreationPackage.Image = this.menu_image;
                    oMenus.AddEx(oCreationPackage);
                }
                Marshal.ReleaseComObject(oMenuItem); if (oMenuItem != null) oMenuItem = null;
                Marshal.ReleaseComObject(oMenus); if (oMenus != null) oMenus = null;
                Marshal.ReleaseComObject(oCreationPackage); if (oCreationPackage != null) oCreationPackage = null;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
