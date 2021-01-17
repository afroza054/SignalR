using System.Web;
using System.Web.Mvc;

namespace Connection_hub_Persistent_Work_01
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
