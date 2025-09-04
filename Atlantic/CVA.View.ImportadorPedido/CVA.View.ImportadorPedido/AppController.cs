using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL.Connection;
using DAL.UserData;
using DAL.UserInterface;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using SAPbobsCOM;
using MODEL;
using CONTROLLER;

namespace CVA.View.ImportadorPedido
{
    public class AppController
    {
        private Application Application { get; set; }
        private Company Company { get; set; }
        private EventFilters Filters { get; set; }

        #region Constructor
        public AppController()
        {
            var conn = ConnectionDao.Instance;
            Application = conn.Application;
            Company = conn.Company;
            Filters = conn.Filters;

            string error = UserFieldsBlo.VerifyData();
            if (!String.IsNullOrEmpty(error))
            {
                Application.SetStatusBarMessage(error);
            }

            MenuBlo.SetMenus();
            EventFilterBlo.SetFilters();
            SetEvents();            
        }

        private void SetEvents()
        {
            Application.AppEvent += AppEvents;
            Application.MenuEvent += MenuEvents;
            Application.ItemEvent += ItemEvents;
        }
        #endregion

        #region ItemEvents
        private void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            ItemEventController.ItemEvents(FormUID, ref pVal, out BubbleEvent);
        }
        #endregion

        #region MenuEvents
        private void MenuEvents(ref MenuEvent pVal, out bool BubbleEvent)
        {
            MenuEventController.MenuEvents(ref pVal, out BubbleEvent);
        }
        #endregion

        #region AppEvents
        private void AppEvents(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    if (Application.Menus.Exists("IMPNF")) Application.Menus.RemoveEx("IMPNF");
                    if (Application.Menus.Exists("IMPPED")) Application.Menus.RemoveEx("IMPPED");
                    if (Application.Menus.Exists("CANCELDOC")) Application.Menus.RemoveEx("CANCELDOC");
                    if (Application.Menus.Exists("CVA")) Application.Menus.RemoveEx("CVA");
                    Environment.Exit(-1);
                    break;
            }
        }
        #endregion
    }
}
