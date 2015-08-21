using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace guess.Controllers
{
    [Authorize.RoleCheck(Enums.Roles.Normal)]
    public class xxtestController : Controller
    {
        #region 常量
        int _itemsPerPage = 20;
        #endregion

        int ToInt32(object value)
        {
            if (value.GetType().Equals(typeof(DBNull)))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }
        DBC.User GetCurrentUser()
        {
            return Session["user"] as DBC.User;
        }

        List<object> GetUserOverviewList(List<DBC.User> userList)
        {
            var now = DateTime.Now;
            var todayBegin = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var todayEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            var list = new List<object>();
            foreach (var u in userList)
            {
                //日投注额
                var dayBetting = 0;
                {
                    var sql = "select sum(total) from betting where time>? and time<? and userid=?";
                    dayBetting = ToInt32(DB.SExecuteScalar(sql, todayBegin, todayEnd, u.ID));
                }
                //日获奖额
                var dayWinning = 0;
                {
                    var sql = "select sum(winning) from betting where time>? and time<? and userid=?";
                    dayWinning = ToInt32(DB.SExecuteScalar(sql, todayBegin, todayEnd, u.ID));
                }
                //累计投注额
                var totalBetting = 0;
                {
                    var sql = "select sum(total) from betting where  userid=?";
                    totalBetting = ToInt32(DB.SExecuteScalar(sql, u.ID));
                }
                //累计获奖额
                var totalWinning = 0;
                {
                    var sql = "select sum(winning) from betting where userid=?";
                    totalWinning = ToInt32(DB.SExecuteScalar(sql, u.ID));
                }

                dynamic o = new System.Dynamic.ExpandoObject();
                o.user = u;
                o.dayBetting = dayBetting;
                o.dayWinning = dayWinning;
                o.totalBetting = totalBetting;
                o.totalWinning = totalWinning;
                list.Add(o);
            }
            return list;
        }
        List<object> GetBettingOverviewList(List<DBC.Betting> bettingList)
        {
            var list = new List<object>();

            foreach (var betting in bettingList)
            {
                try
                {
                    var s = new DBC.Screening(betting.ScreeningID);
                    var result = s.Result;

                    dynamic o = new System.Dynamic.ExpandoObject();
                    o.betting = betting;
                    o.result = result;
                    list.Add(o);
                }
                catch { }
            }

            return list;
        }

        #region 基本信息
        public ActionResult Index()
        {
            ViewBag.Title = "概况";
            ViewBag.Title2 = "概况";
            ViewBag.Page = "概况";

            ViewBag.current = GetCurrentUser();

            return View();
        }

        #endregion
        #region 投注模块
        public ActionResult Betting(int? userid, int p = 0)
        {
            try
            {
                ViewBag.Title = "投注记录";
                ViewBag.Title2 = "投注记录";
                ViewBag.Page = "投注";

                var currentUser = Session["user"] as DBC.User;
                var pages = 1;
                var sqlCount = "";
                var sqlList = "";
                var sqlCountArgs = new List<object>();
                var sqlListArgs = new List<object>();

                var current = GetCurrentUser();
                sqlCount = string.Format("select count(*) from {0} where {0}.userid=?", DBTables.Betting, DBTables.User);
                sqlCountArgs.Add(current.ID);

                sqlList = string.Format("select id from {0} where {0}.userid=?  order by time desc limit ?,?", DBTables.Betting, DBTables.User);
                sqlListArgs.Add(current.ID);

                //添加分页参数
                sqlListArgs.Add(p * _itemsPerPage);
                sqlListArgs.Add(_itemsPerPage);

                var totalCount = Convert.ToInt32(DB.SExecuteScalar(sqlCount, sqlCountArgs.ToArray()));
                pages = (int)Math.Ceiling(totalCount * 1.0 / _itemsPerPage);
                var res = DB.SExecuteReader(sqlList, sqlListArgs.ToArray());

                var bettingList = new List<DBC.Betting>();
                foreach (var item in res)
                {
                    //数据记录不完整时跳过
                    try
                    {
                        var id = Convert.ToInt32(item[0]);
                        var betting = new DBC.Betting(id);
                        bettingList.Add(betting);
                    }
                    catch { }
                }

                var pagination = new Pagination();
                pagination.Pages = pages;
                pagination.Current = p;
                pagination.BaseUrl = "/home/betting";
                ViewBag.pagination = pagination;

                ViewBag.list = GetBettingOverviewList(bettingList);
            }
            catch
            {
                ViewBag.errorText = "未查询到任何记录";
            }

            return View();
        }

        public ActionResult BettingAdd()
        {
            ViewBag.Title = "新投注";
            ViewBag.Title2 = "新投注";
            ViewBag.Page = "投注";
            return View();
        }
        [HttpPost]
        public ActionResult BettingAdd(uint orange, uint banana, uint grape, uint pineapple, uint strawberry, uint watermenon)
        {
            var now = DateTime.Now;
            try
            {
                var screening = new DBC.Screening(now);
                var diff = screening.End - now;
                if (diff.TotalSeconds <= 30)
                {
                    return Content("开奖前30秒禁止投注");
                }
                else
                {
                    var current = GetCurrentUser();
                    var total = orange + banana + grape + watermenon + strawberry + pineapple;
                    if (total == 0)
                        return Content("总投注额不能为0");

                    if (current.Points < total)
                        return Content("积分不足，请充值");

                    current.Points = current.Points- total;
                    DBC.Betting.Create(orange, banana, grape, watermenon, strawberry, pineapple, total, screening.ID, current.ID);
                }
                return Redirect("~/home/betting");

            }
            catch
            {
                return Content("还没有到竞猜时间");
            }
        }
        #endregion
        #region 修改密码
        public ActionResult ResetPw()
        {
            ViewBag.Title = "修改密码";
            ViewBag.Title2 = "修改密码";
            ViewBag.Page = "修改密码";
            return View();
        }
        [HttpPost]
        public ActionResult ResetPw(string old, string newp)
        {
            ViewBag.Title = "修改密码";
            ViewBag.Title2 = "修改密码";
            ViewBag.Page = "修改密码";
            if (old == null)
                old = "";
            if (newp == null)
                newp = "";
            old = old.ToSHA256String();
            var user = Session["user"] as DBC.User;
            if (user.Password != old)
            {
                ViewBag.error = true;
            }
            else
            {
                ViewBag.success = true;
                user.Password = newp.ToSHA256String();
            }
            return View();
        }
        #endregion
    }
}
