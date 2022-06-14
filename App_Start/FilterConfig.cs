using System.Web;
using System.Web.Mvc;

namespace LAB05_TranDinhNguyen
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
