using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace guess.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            if (Session["user"] != null)
                return Redirect("~/");

            return View();
        }

        [HttpPost]
        public ActionResult Index(int id, string password, string remember)
        {
            try
            {
                var user = new DBC.User(id);

                if (user.Role != Enums.Roles.Administrator)
                    throw new Exception("权限不足");
                
                if (user.Password != password.ToSHA256String())
                    throw new Exception("密码错误");

                Session["user"] = user;

                if (string.IsNullOrEmpty(remember) == false)
                {
                    HttpCookie cookie = new HttpCookie("login");
                    var val = Guid.NewGuid().ToString("d");
                    var expire = DateTime.Now.AddDays(15);

                    DB.SExecuteNonQuery("insert into " + DBTables.UserLoginCookie + "(userid,value,expire) values (?,?,?)", user.ID, val, expire);

                    cookie.Value = val;
                    cookie.Expires = expire;
                    cookie.HttpOnly = true;

                    Response.SetCookie(cookie);
                }
                return Redirect("~/");
            }
            catch
            {
                ViewBag.loginFail = true;
                return View();
            }

        }
    }
}
