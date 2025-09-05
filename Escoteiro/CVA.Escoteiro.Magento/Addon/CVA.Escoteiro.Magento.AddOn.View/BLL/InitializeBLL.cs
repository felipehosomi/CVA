using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using System;
using System.IO;
using System.Reflection;

namespace CVA.Escoteiro.Magento.AddOn.View
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            try
            {
                UserFieldsBLL.CreateUserFields();
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Ero ao criar campos de usuário" + ex.Message);
            }
            //try
            //{
            //    UserFieldsBLL.InsertDefaultData();
            //}
            //catch (Exception ex)
            //{
            //    SBOApp.Application.SetStatusBarMessage("Ero ao inserir dados padrões: " + ex.Message);
            //}
            //try
            //{
            //    EventFilterBLL.CreateDefaultEvents();
            //}
            //catch (Exception ex)
            //{
            //    SBOApp.Application.SetStatusBarMessage("Ero ao criar eventos padrões" + ex.Message);
            //}
            //try
            //{
            //    MenuController.LoadFromXML(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Menu\\Menu.xml");
            //}
            //catch (Exception ex)
            //{
            //    SBOApp.Application.SetStatusBarMessage("Ero ao inserir dados padrões: " + ex.Message);
            //}
        }
    }
}
