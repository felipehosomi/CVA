using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.BLL
{
    public class EventFilterBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterController.SetFormEvent("f1001", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("f1001", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("f1001", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("f1001", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("f1001", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);


            EventFilterController.SetFormEvent("f1002", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("f1002", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("f1002", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("f1002", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("f1002", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);

            EventFilterController.SetFormEvent("f1003", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("f1003", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("f1003", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("f1003", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("f1003", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);

            EventFilterController.SetFormEvent("f1004", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("f1004", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("f1004", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("f1004", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("f1004", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);

            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("133", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);

            EventFilterController.SetFormEvent("140", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterController.SetFormEvent("140", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            EventFilterController.SetFormEvent("140", SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            EventFilterController.SetFormEvent("140", SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            EventFilterController.SetFormEvent("140", SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);





            EnableEvents();
        }

        public static void EnableEvents()
        {
            EventFilterController.EnableEvents();
        }

        public static void DisableEvents()
        {
            EventFilterController.DisableEvents();
        }
    }
}
