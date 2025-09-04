using SAPbouiCOM;
using DAL.Connection;

namespace DAL.UserInterface
{
    public class MenusDao
    {
        public static void Add(string parentId, string menuId, string description, int position, BoMenuType type,
            string imagePath = null)
        {
            var oCreationPackage =
                (MenuCreationParams)ConnectionDao.Instance.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            var oMenuItem = ConnectionDao.Instance.Application.Menus.Item(parentId);
            var oMenus = oMenuItem.SubMenus;
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
    }
}
