using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class Filters
    {
        public static void Add(string containerUid, BoEventTypes eventType)
        {
            int pos;
            if (!Exists(eventType, out pos))
            {
                EventFilter oFilter = B1Connection.Instance.Filters.Add(eventType);
                oFilter.AddEx(containerUid);
                B1Connection.Instance.Application.SetFilter(B1Connection.Instance.Filters);
            }
            else
            {
                B1Connection.Instance.Filters.Item(pos).AddEx(containerUid);
                B1Connection.Instance.Application.SetFilter(B1Connection.Instance.Filters);
            }
        }

        public static bool Exists(BoEventTypes type, out int position)
        {
            var ret = false;
            position = -1;

            try
            {
                for (var i = 0; i < B1Connection.Instance.Filters.Count; i++)
                {
                    var f = B1Connection.Instance.Filters.Item(i);
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
