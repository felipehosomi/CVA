using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public static class Filters
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
                try
                {
                    var p = B1Connection.Instance.Filters.Item(pos);
                    var exists = false; 

                    foreach (var item in p)
                    {
                        if(((dynamic)item).StringValue == containerUid)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        B1Connection.Instance.Filters.Item(pos).AddEx(containerUid);
                        B1Connection.Instance.Application.SetFilter(B1Connection.Instance.Filters);
                    }
                }
                catch { }
            }
        }

        private static bool Exists(BoEventTypes type, out int position)
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

        public static Form GetForm(this Forms forms, string type)
        {
            var hasForm = false;
            foreach (Form form in forms)
            {
                if (hasForm) break;
                hasForm = form.TypeEx == type;
            }

            if (hasForm) return forms.Item(type);
            else return null;
        }
    }

    public static class Extentions
    {
        public static string Aspas(this string value)
        {
            return $"\"{value}\"";
        }
    }
}
