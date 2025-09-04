using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public class Menus
    {
        public static void Add(string parentId, string menuId, string description, int position, BoMenuType type,
            string imagePath = null)
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
    }
}
