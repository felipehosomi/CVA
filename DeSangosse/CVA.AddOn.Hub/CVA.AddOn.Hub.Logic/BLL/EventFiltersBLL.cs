using CVA.AddOn.Common;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class EventFiltersBLL
    {
        private static EventFilters DefaultEventFilters;

        private static EventFilters _disableEventFilters;
        private static EventFilters DisableEventFilters
        {
            get
            {
                if (_disableEventFilters == null)
                {
                    _disableEventFilters = new EventFilters();
                    _disableEventFilters.Reset();
                }

                return _disableEventFilters;
            }
        }

        public static void SetDefaultEvents()
        {
            DefaultEventFilters = new EventFilters();

            // Sempre adicionar MENU CLICK, se não os menus não abrem
            EventFilter filter = DefaultEventFilters.Add(BoEventTypes.et_MENU_CLICK);

            // Adiciona eventos que não impactam na performance
            filter = DefaultEventFilters.Add(BoEventTypes.et_CLICK);
            filter = DefaultEventFilters.Add(BoEventTypes.et_CHOOSE_FROM_LIST);
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_CLOSE);
            filter = DefaultEventFilters.Add(BoEventTypes.et_LOST_FOCUS);
            filter = DefaultEventFilters.Add(BoEventTypes.et_VALIDATE);
            filter = DefaultEventFilters.Add(BoEventTypes.et_KEY_DOWN);
            filter = DefaultEventFilters.Add(BoEventTypes.et_COMBO_SELECT);
            filter = DefaultEventFilters.Add(BoEventTypes.et_RIGHT_CLICK);

            // Data Form Events
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_DATA_ADD);
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_DATA_UPDATE);
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_DATA_LOAD);
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_DATA_DELETE);

            // Adiciona FormLoad para poder setar o filtro nos system forms
            filter = DefaultEventFilters.Add(BoEventTypes.et_FORM_LOAD);
            filter = DefaultEventFilters.Add(BoEventTypes.et_PRINT_LAYOUT_KEY);

            SBOApp.Application.SetFilter(DefaultEventFilters);
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
                if (DefaultEventFilters == null)
                {
                    SetDefaultEvents();
                }

                EventFilter filter;
                // Busca o evento na lista de eventos
                for (int i = 0; i < DefaultEventFilters.Count; i++)
                {
                    filter = DefaultEventFilters.Item(i);
                    if (filter.EventType == eventType)
                    {
                        try
                        {
                            filter.AddEx(formId);
                        }
                        catch (Exception e) { }
                        SBOApp.Application.SetFilter(DefaultEventFilters);
                        return;
                    }
                }

                // Se não encontrar o evento, adiciona
                filter = DefaultEventFilters.Add(eventType);
                if (!String.IsNullOrEmpty(formId))
                {
                    filter.AddEx(formId);
                }
                SBOApp.Application.SetFilter(DefaultEventFilters);
            }
            catch (Exception e)
            {
                SBOApp.Application.SetStatusBarMessage("Erro geral no EventFilter: " + e.Message);
            }
        }

        /// <summary>
        /// Desabilita todos os eventos
        /// </summary>
        public static void DisableEvents()
        {
            SBOApp.Application.SetFilter(DisableEventFilters);
        }
    }
}
