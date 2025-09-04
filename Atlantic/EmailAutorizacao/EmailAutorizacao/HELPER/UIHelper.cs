using System;
using SAPbouiCOM;

namespace EmailAutorizacao.HELPER
{
    public class UIHelper
    {
        public static void AddFilter(ref EventFilters oFilters, string containerUID, BoEventTypes eventType)
        {
            try
            {
                int pos;
                if (!FilterExists(ref oFilters, eventType, out pos))
                {
                    var oFilter = oFilters.Add(eventType);
                    oFilter.AddEx(containerUID);
                    B1Connection.Instance.Application.SetFilter(oFilters);
                }
                else
                {
                    oFilters.Item(pos).AddEx(containerUID);
                    B1Connection.Instance.Application.SetFilter(oFilters);
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.StatusBar.SetText(ex.Message);
            }
        }

        private static bool FilterExists(ref EventFilters oFilters, BoEventTypes eventType, out int pos)
        {
            var ret = false;
            pos = -1;

            try
            {
                for (var i = 0; i < oFilters.Count; i++)
                {
                    var f = oFilters.Item(i);
                    if (eventType.Equals(f.EventType))
                    {
                        ret = true;
                        pos = i;
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
