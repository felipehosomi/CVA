using SBO.Hub;
using SBO.Hub.Helpers;
using System;
using System.Windows.Forms;

namespace CVA.Cointer.Core.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterBLL.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();

            try
            {
                MenuHelper.LoadFromXML($"{Application.StartupPath}\\Menu\\Menu.xml");
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage($"Erro ao criar menu: {ex.Message}");
            }
        }
    }
}
