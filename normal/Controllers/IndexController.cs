using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace normal.Controllers
{
    public class IndexController : Controller
    {
        DBC.User GetCurrentUser()
        {
            if (Session["userid"] != null)
            {
                var id = (int)Session["userid"];
                return new DBC.User(id);
            }
            return null;
        }

        void GetScreeningsList()
        {
            var list = new List<DBC.Screening>();
            var sql = string.Format("select id from {0} where start>? and end<?", DBTables.Screening);
            var res = DB.SExecuteReader(sql, DateTime.Today, DateTime.Now);
            foreach (var item in res)
            {
                var id = Convert.ToInt32(item[0]);
                var s = new DBC.Screening(id);
                list.Add(s);
            }
            ViewBag.screeningsList = list;
        }

        void GetBettingsList()
        {
            var user = GetCurrentUser();
            if (user == null || user.Role != Enums.Roles.Normal)
                return;

            var list = new List<DBC.Betting>();
            var sql = string.Format("select id from {0} where userid=? order by id desc limit 20", DBTables.Betting);
            var res = DB.SExecuteReader(sql, user.ID);
            foreach (var item in res)
            {
                var id = Convert.ToInt32(item[0]);
                var betting = new DBC.Betting(id);
                list.Add(betting);
            }
            ViewBag.user = user;
            ViewBag.bettingsList = list;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "竞猜";
            GetScreeningsList();
            GetBettingsList();
            return View();
        }

        public ActionResult Reflesh()
        {
            GetScreeningsList();
            GetBettingsList();
            return PartialView("main");
        }

        public ActionResult Draw()
        {
            var sql = string.Format("select result from {0} where end<? order by end desc limit 1", DBTables.Screening);
            var res = DB.SExecuteScalar(sql, DateTime.Now);
            var result = Enums.Fruits.None;
            if (res != null)
            {
                result = (Enums.Fruits)Convert.ToByte(res);
            }
            return Content(result.ToString());
        }

        public ActionResult Betting(uint orange = 0, uint banana = 0, uint grape = 0, uint strawberry = 0, uint pineapple = 0,  uint watermelon = 0)
        {
            var user = GetCurrentUser();
            if (user == null)
                return HttpNotFound();

            var now=DateTime.Now;

            DBC.Screening screening=null;
            try
            {
                screening = new DBC.Screening(now);
            }
            catch
            {
                return Content("还没有到投注时间");
            }
            var diff=screening.End-now;
            if (diff.TotalSeconds <= 30)
            {
                return Content("开奖前30秒不能再投注");
            }

            var total = orange + banana + grape + strawberry + pineapple + watermelon;
            if (total == 0)
                return Content("总投注额不能为0");

            if(user.Points<total)
                return Content("积分不足，请先充值");

            user.Points -= total;
            DBC.Betting.Create(orange, banana, grape, watermelon, strawberry, pineapple, total,screening.ID,user.ID);

            return Content("ok");
        }
    }
}
