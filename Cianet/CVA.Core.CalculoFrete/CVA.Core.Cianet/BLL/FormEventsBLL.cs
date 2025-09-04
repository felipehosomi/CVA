using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Cianet.BLL
{
    public class FormEventsBLL
    {
        public static void SetEvents()
        {
            EventFilterController.SetFormEvent("139", SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            EventFilterController.SetFormEvent("149", SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
           
        }
    }
}
