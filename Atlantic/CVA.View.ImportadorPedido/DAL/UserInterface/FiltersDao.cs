using System;
using SAPbouiCOM;
using DAL.Connection;

namespace DAL.UserInterface
{
    public class FiltersDao
    {
        public static void Add(string containerUid, BoEventTypes eventType)
        {
            int pos;
            if (!Exists(eventType, out pos))
            {
                var oFilter = ConnectionDao.Instance.Filters.Add(eventType);
                oFilter.AddEx(containerUid);
                ConnectionDao.Instance.Application.SetFilter(ConnectionDao.Instance.Filters);
            }
            else
            {
                ConnectionDao.Instance.Filters.Item(pos).AddEx(containerUid);
                ConnectionDao.Instance.Application.SetFilter(ConnectionDao.Instance.Filters);
            }
        }

        public static bool Exists(BoEventTypes type, out int position)
        {
            var ret = false;
            position = -1;

            try
            {
                for (var i = 0; i < ConnectionDao.Instance.Filters.Count; i++)
                {
                    var f = ConnectionDao.Instance.Filters.Item(i);
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
