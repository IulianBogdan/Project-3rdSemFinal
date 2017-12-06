using System.Web;
using System.Web.Mvc;

namespace Sem3FinalProject_Code
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
