using System;
using SAPbouiCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class FilterHelper
    {
        private readonly SapFactory _factory;

        public FilterHelper(SapFactory factory)
        {
            _factory = factory;
        }

        public void Add(string containerUid, BoEventTypes eventType)
        {
            int pos;
            if (!Exists(eventType, out pos))
            {
                EventFilter oFilter = _factory.Filters.Add(eventType);
                oFilter.AddEx(containerUid);
                _factory.Application.SetFilter(_factory.Filters as EventFilters);
            }
            else
            {
                _factory.Filters.Item(pos).AddEx(containerUid);
                _factory.Application.SetFilter(_factory.Filters as EventFilters);
            }
        }

        public bool Exists(BoEventTypes type, out int position)
        {
            var ret = false;
            position = -1;

            try
            {
                for (var i = 0; i < _factory.Filters.Count; i++)
                {
                    var f = _factory.Filters.Item(i);
                    if (type.Equals(f.EventType))
                    {
                        ret = true;
                        position = i;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }
    }
}
