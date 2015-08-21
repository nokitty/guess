using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authorize
{
    public class RoleCheck : AuthorizeAttribute
    {
        Enums.Roles _role;

        public RoleCheck(Enums.Roles role)
        {
            _role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.Session["user"] as DBC.User;
            if (user == null)
                return false;
            return user.Role == _role;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectResult("~/login");
            }
            else
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }

}