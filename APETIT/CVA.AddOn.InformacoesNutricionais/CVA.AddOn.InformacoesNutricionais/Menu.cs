using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;

namespace CVA.AddOn.InformacoesNutricionais
{
    class Menu
    {
        public void AddMenuItems()
        {
            
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

    }
}
