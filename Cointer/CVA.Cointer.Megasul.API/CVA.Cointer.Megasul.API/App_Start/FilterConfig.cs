using System.Web;
using System.Web.Mvc;

namespace CVA.Cointer.Megasul.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
