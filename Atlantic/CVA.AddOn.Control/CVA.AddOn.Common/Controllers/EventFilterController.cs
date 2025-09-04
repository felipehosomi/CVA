using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;

namespace CVA.AddOn.Common.Controllers
{
    public class EventFilterController
    {
        private static EventFilters events;

        public static void SetDefaultEvents()
        {
            events = new EventFilters();

            // Sempre adicionar MENU CLICK, se não os menus não abrem
            EventFilter filter = events.Add(BoEventTypes.et_MENU_CLICK);

            // Adiciona eventos que não impactam na performance
            filter = events.Add(BoEventTypes.et_CLICK);
            filter = events.Add(BoEventTypes.et_CHOOSE_FROM_LIST);
            filter = events.Add(BoEventTypes.et_FORM_CLOSE);

            //filter = events.Add(BoEventTypes.et_PRINT_LAYOUT_KEY);

            // Adiciona FormLoad para poder setar o filtro nos system forms
            filter = events.Add(BoEventTypes.et_FORM_LOAD);

            SBOApp.Application.SetFilter(events);
        }

        public static void SetDataFormDefaultEvents()
        {
            if (events == null)
            {
                SetDefaultEvents();
            }
            EventFilter filter;

            filter = events.Add(BoEventTypes.et_FORM_DATA_ADD);
            filter = events.Add(BoEventTypes.et_FORM_DATA_UPDATE);
            filter = events.Add(BoEventTypes.et_FORM_DATA_LOAD);
            filter = events.Add(BoEventTypes.et_FORM_DATA_DELETE);

            SBOApp.Application.SetFilter(events);
        }

        /// <summary>
        /// Aciona evento no Form
        /// </summary>
        /// <param name="formId">Id do Form - Exemplos: 150 / 2000002001</param>
        /// <param name="eventType">Tipo do evento</param>
        public static void SetFormEvent(string formId, BoEventTypes eventType)
        {
            try
            {
                if (events == null)
                {
                    SetDefaultEvents();
                }

                EventFilter filter;
                // Busca o evento na lista de eventos
                for (int i = 0; i < events.Count; i++)
                {
                    filter = events.Item(i);
                    if (filter.EventType == eventType)
                    {
                        try
                        {
                            filter.AddEx(formId);
                        }
                        catch (Exception e) { }
                        SBOApp.Application.SetFilter(events);
                        return;
                    }
                }

                // Se não encontrar o evento, adiciona
                filter = events.Add(eventType);
                if (!String.IsNullOrEmpty(formId))
                {
                    filter.AddEx(formId);
                }
                SBOApp.Application.SetFilter(events);
            }
            catch (Exception e)
            {
                SBOApp.Application.SetStatusBarMessage("Erro geral no EventFilter: " + e.Message);
            }
        }
    }
}
