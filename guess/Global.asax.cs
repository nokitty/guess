using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace guess
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DB.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["db"].ConnectionString;

        }

        protected void Session_Start()
        {
            var cookie = Request.Cookies["login"];
            if (cookie != null)
            {
                var val = cookie.Value;
                var sql = "select userid from " + DBTables.UserLoginCookie + " where value=? and expire>?";
                var res = DB.SExecuteScalar(sql, val, DateTime.Now);
                if (res != null)
                {
                    var userid = Convert.ToInt32(res);
                    Session["user"] = new DBC.User(userid);
                }
            }
        }

    }
}