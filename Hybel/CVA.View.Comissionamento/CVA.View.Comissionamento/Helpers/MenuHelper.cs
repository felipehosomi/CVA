using SAPbouiCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class MenuHelper
    {
        private readonly SapFactory _factory;

        public MenuHelper(SapFactory factory)
        {
            _factory = factory;
        }

        public void Add(string parentId, string menuId, string description, int position, BoMenuType type,
            string imagePath = null)
        {
            MenuCreationParams oCreationPackage =
                (MenuCreationParams)_factory.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            MenuItem oMenuItem = _factory.Application.Menus.Item(parentId);
            Menus oMenus = oMenuItem.SubMenus;
            var exist = (oMenus != null) && oMenuItem.SubMenus.Exists(menuId);

            if (exist)
            {
                oMenuItem.SubMenus.RemoveEx(menuId);
                exist = false;
            }

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
    }
}
