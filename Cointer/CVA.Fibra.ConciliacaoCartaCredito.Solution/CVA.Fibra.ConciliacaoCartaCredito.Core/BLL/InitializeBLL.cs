using SBO.Hub;
using SBO.Hub.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterBLL.SetDefaultEvents();

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
