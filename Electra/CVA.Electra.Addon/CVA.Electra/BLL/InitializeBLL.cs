using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Models;

namespace CVA.View.CRCP.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            UserFieldsBLL.CreateUserFields();
            EventFilterBLL.CreateDefaultEvents();
            
            MenuController menuController = new MenuController();

            MenuModel menuModel = new MenuModel();
            menuModel.Description = "CVA - Contas a Pagar/Receber";
            menuModel.Parent = "43537";
            menuModel.Position = 9;
            menuModel.UniqueID = "M9901";
            menuModel.Type = AddOn.Common.Enums.EnumMenuType.mt_STRING;
            menuModel.Enabled = true;

            menuController.Add(menuModel);
        }
    }
}
