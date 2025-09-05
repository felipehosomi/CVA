using System.Web;
using System.Web.Mvc;

namespace CVA.Web.Hybel.Producao
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
