using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace top.Controllers
{
    [Authorize.RoleCheck(Enums.Roles.Administrator)]
    public class HomeController : Controller
    {
        #region 常量
        int _itemsPerPage = 20;
        #endregion

        #region 工具函数
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
                    var sql = string.Format("select sum(total) from {0},{1} where {1}.id={0}.userid and time>? and time<? and ({0}.userid=? or {1}.parent=?)", DBTables.Betting, DBTables.User);
                    dayBetting =Utility.ToInt32(DB.SExecuteScalar(sql, todayBegin, todayEnd, u.ID, u.ID));
                }
                //日获奖额
                var dayWinning = 0;
                {
                    var sql = string.Format("select sum(winning) from {0},{1} where {1}.id={0}.userid and time>? and time<? and ({0}.userid=? or {1}.parent=?)", DBTables.Betting, DBTables.User);
                    dayWinning = Utility.ToInt32(DB.SExecuteScalar(sql, todayBegin, todayEnd, u.ID, u.ID));
                }
                //累计投注额
                var totalBetting = 0;
                {
                    var sql = string.Format("select sum(total) from {0},{1} where {1}.id={0}.userid and ({0}.userid=? or {1}.parent=?)", DBTables.Betting, DBTables.User);
                    totalBetting = Utility.ToInt32(DB.SExecuteScalar(sql, u.ID, u.ID));
                }
                //累计获奖额
                var totalWinning = 0;
                {
                    var sql = string.Format("select sum(winning) from {0},{1} where {1}.id={0}.userid and ({0}.userid=? or {1}.parent=?)", DBTables.Betting, DBTables.User);
                    totalWinning = Utility.ToInt32(DB.SExecuteScalar(sql, u.ID, u.ID));
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

        #endregion

        #region 概况
        public ActionResult Index()
        {
            ViewBag.Title = "概况";
            ViewBag.Title2 = "概况";
            ViewBag.Page = "概况";

            var totalUser = 0;
            var agentUser = 0;
            var dayBetting = 0;
            var dayWinning = 0;
            var now = DateTime.Now;
            var dayBegin = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var dayEnd = dayBegin.AddDays(1);

            //总用户数
            {
                var sql = string.Format("select count(*) from {0} ", DBTables.User);
                totalUser = Utility.ToInt32(DB.SExecuteScalar(sql));
            }
            //代理数
            {
                var sql = string.Format("select count(*) from {0} where role=?", DBTables.User);
                agentUser = Utility.ToInt32(DB.SExecuteScalar(sql, Enums.Roles.Agent));
            }
            //当天投注额
            {
                var sql = string.Format("select sum(total) from {0} where time<? and time>?", DBTables.Betting);
                dayBetting = Utility.ToInt32(DB.SExecuteScalar(sql, dayEnd, dayBegin));
            }
            //当天中奖
            {
                var sql = string.Format("select sum(winning) from {0} where time<? and time>?", DBTables.Betting);
                dayWinning = Utility.ToInt32(DB.SExecuteScalar(sql, dayEnd, dayBegin));
            }

            ViewBag.totalUser = totalUser;
            ViewBag.agentUser = agentUser;
            ViewBag.dayBetting = dayBetting;
            ViewBag.dayWinning = dayWinning;
            return View();
        }
        #endregion

        #region 开奖
        public ActionResult Drawing()
        {
            ViewBag.Title = "开奖";
            ViewBag.Title2 = "开奖";
            ViewBag.Page = "开奖";

            try
            {
                var current = new DBC.Screening(DateTime.Now);
                ViewBag.current = current;
            }
            catch { }

            var list = new List<DBC.Screening>();
            {
                var now = DateTime.Now;
                var dayBegin = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var dayEnd = dayBegin.AddDays(1);
                var sql = string.Format("select id from {0} where start>? and start<? and result!=? order by id desc", DBTables.Screening);
                var res = DB.SExecuteReader(sql, dayBegin, dayEnd, Enums.Fruits.None);
                foreach (var item in res)
                {
                    var id = Convert.ToInt32(item[0]);
                    var s = new DBC.Screening(id);
                    list.Add(s);
                }
            }            
            ViewBag.list = list;

            return View();
        }

        [HttpPost]
        public ActionResult DrawingPreset(int id,Enums.Fruits preset)
        {
            var s = new DBC.Screening(id);
            s.Preset = preset;
            return Redirect("~/home/drawing");
        }
        #endregion

        #region 用户模块
        public ActionResult Users(int p = 0)
        {
            ViewBag.Title = "用户";
            ViewBag.Title2 = "用户";
            ViewBag.Page = "用户";

            var currentUser = Session["user"] as DBC.User;
            var pages = 1;

            var sql1 = string.Format("select count(*) from {0} where parent=?", DBTables.User);
            var totalCount = Convert.ToInt32(DB.SExecuteScalar(sql1, currentUser.ID));
            pages = (int)Math.Ceiling(totalCount * 1.0 / _itemsPerPage);

            var sql2 = string.Format("select id from {0} where parent=? limit ?,?", DBTables.User);
            var res = DB.SExecuteReader(sql2, currentUser.ID, p * _itemsPerPage, _itemsPerPage);

            var userList = new List<DBC.User>();
            foreach (var item in res)
            {
                var id = Convert.ToInt32(item[0]);
                var u = new DBC.User(id);
                userList.Add(u);
            }

            ViewBag.list = GetUserOverviewList(userList);

            var pagination = new Pagination();
            pagination.Pages = pages;
            pagination.Current = p;
            pagination.BaseUrl = "/home/users";
            ViewBag.pagination = pagination;

            return View();
        }

        //添加新用户
        public ActionResult UserAdd()
        {
            ViewBag.Title = "添加新用户";
            ViewBag.Title2 = "添加新用户";
            ViewBag.Page = "用户";
            return View();
        }
        [HttpPost]
        public ActionResult UserAdd(string tel, string password, string remark)
        {
            var user = Session["user"] as DBC.User;
            DBC.User.Create(tel, password.ToSHA256String(), Enums.Roles.Agent, user.ID, remark);
            return Redirect("~/home/users");
        }
        //查找指定用户
        [HttpPost]
        public ActionResult UserFind(int id)
        {
            var currentUser = Session["user"] as DBC.User;

            ViewBag.Title = "查找用户";
            ViewBag.Title2 = "查找用户";
            ViewBag.Page = "用户";
            try
            {
                var user = new DBC.User(id);
                if (user.Parent != currentUser.ID)
                    throw new Exception("无权查看该用户资料");
                ViewBag.list = GetUserOverviewList(new List<DBC.User>() { user });
            }
            catch
            {
                ViewBag.errorText = "该用户id不存在";
            }
            return View("users");
        }

        //删除用户,功能暂时不实现，问题多多
        //public ActionResult UserDelete(int id)
        //{
        //    try
        //    {
        //        var user = new DBC.User(id);
        //        if (user.Parent == _currentUser.ID)
        //            user.Delete();
        //    }
        //    catch { }
        //    return Redirect("~/home/users");
        //}



        //重置用户密码
        public ActionResult UserResetPw(int id)
        {
            var user = new DBC.User(id);
            if (user.Parent == GetCurrentUser().ID)
                user.Password = "123456".ToSHA256String();

            return Redirect("~/home/users");
        }

        //充值
        public ActionResult UserRecharge(int id)
        {
            try
            {
                var currentUser = Session["user"] as DBC.User;
                ViewBag.Title = "用户充值";
                ViewBag.Title2 = "用户充值";
                ViewBag.Page = "用户";

                var user = new DBC.User(id);
                if (user.Parent != GetCurrentUser().ID)
                    throw new Exception("无权充值");

                ViewBag.user = user;
            }
            catch
            {
                ViewBag.errorText = "找不到该用户";
            }
            return View();
        }
        [HttpPost]
        public ActionResult UserRecharge(int id, int count)
        {
            try
            {
                var current = GetCurrentUser();
                var user = new DBC.User(id);
                if (user.Parent != current.ID)
                    return Content("未找到该用户");

                if (count >= 0)
                {
                    if (current.Points < count)
                        return Content("余额不足，无法完成充值");

                    current.Points -= (uint)count;
                    user.Points += (uint)count;
                }
                else
                {
                    count = -1 * count;
                    if (user.Points < count)
                    {
                        return Content("提现额大于积分，提现失败");
                    }

                    user.Points -= (uint)count;
                }
            }
            catch { }
            return Redirect("~/home/users");
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

        #region 投注模块
        public ActionResult Betting(int? userid, DateTime? beginDate,DateTime? endDate, int p = 0)
        {
            try
            {
                ViewBag.Title = "投注记录";
                ViewBag.Title2 = "投注记录";
                ViewBag.Page = "投注";
                
                if (beginDate == null)
                    beginDate = DateTime.Today.AddDays(-7);
                if (endDate == null)
                    endDate = DateTime.Today;

                var currentUser = Session["user"] as DBC.User;
                var pages = 1;
                var sqlCount = "";
                var sqlList = "";
                var sqlSum= "";
                var sqlCountArgs = new List<object>();
                var sqlListArgs = new List<object>();
                var sqlSumArgs = new List<object>();

                //查看指定用户（包括其下属帐号）的投注记录
                if (userid != null)
                {
                    var user = new DBC.User(userid.Value);
                    if (user.Parent != GetCurrentUser().ID)
                        throw new Exception("无权查看");

                    sqlCount = string.Format("select count(*) from {0},{1} where {0}.userid={1}.id and ({1}.id=? or {1}.parent=?) and time>? and time<?", DBTables.Betting, DBTables.User);
                    sqlCountArgs.Add(user.ID);
                    sqlCountArgs.Add(user.ID);
                    sqlCountArgs.Add(beginDate.Value);
                    sqlCountArgs.Add(endDate.Value.AddDays(1));

                    sqlList = string.Format("select {0}.id from {0},{1} where {0}.userid={1}.id and ({1}.id=? or {1}.parent=?) and time>? and time<? order by time desc limit ?,?", DBTables.Betting, DBTables.User);
                    sqlListArgs.Add(user.ID);
                    sqlListArgs.Add(user.ID);
                    sqlListArgs.Add(beginDate.Value);
                    sqlListArgs.Add(endDate.Value.AddDays(1));

                    sqlSum = string.Format("select sum(total),sum(winning) from {0},{1} where {0}.userid={1}.id and ({1}.id=? or {1}.parent=?) and time>? and time<?", DBTables.Betting, DBTables.User);
                    sqlSumArgs.Add(user.ID);
                    sqlSumArgs.Add(user.ID);
                    sqlSumArgs.Add(beginDate.Value);
                    sqlSumArgs.Add(endDate.Value.AddDays(1));
                }
                //查看全部投注记录
                else
                {
                    sqlCount = string.Format("select count(*) from {0} where time>? and time<?", DBTables.Betting);
                    sqlCountArgs.Add(beginDate.Value);
                    sqlCountArgs.Add(endDate.Value.AddDays(1));

                    sqlList = string.Format("select id from {0} where time>? and time<? order by time desc limit ?,?", DBTables.Betting);
                    sqlListArgs.Add(beginDate.Value);
                    sqlListArgs.Add(endDate.Value.AddDays(1));

                    sqlSum = string.Format("select sum(total),sum(winning) from {0} where time>? and time<?", DBTables.Betting);
                    sqlSumArgs.Add(beginDate.Value);
                    sqlSumArgs.Add(endDate.Value.AddDays(1));
                }

                //添加分页参数
                sqlListArgs.Add(p * _itemsPerPage);
                sqlListArgs.Add(_itemsPerPage);
                //获取总记录数
                var totalCount = Convert.ToInt32(DB.SExecuteScalar(sqlCount, sqlCountArgs.ToArray()));
                //计算分页数
                pages = (int)Math.Ceiling(totalCount * 1.0 / _itemsPerPage);
                //获取下注记录
                var res = DB.SExecuteReader(sqlList, sqlListArgs.ToArray());
                var list = new List<object>();
                foreach (var item in res)
                {
                    //数据记录不完整时跳过
                    try
                    {
                        var id = Convert.ToInt32(item[0]);
                        var b = new DBC.Betting(id);
                        var s = new DBC.Screening(b.ScreeningID);

                        var result = s.Result;

                        dynamic o = new System.Dynamic.ExpandoObject();
                        o.betting = b;
                        o.result = result;
                        list.Add(o);
                    }
                    catch { }
                }
                //获取总投注和总中奖
                var res2 = DB.SExecuteReader(sqlSum, sqlSumArgs.ToArray());
                var totalBetting = Utility.ToInt32(res2[0][0]);
                var totalWinning = Utility.ToInt32(res2[0][1]);

                //设置分页记录
                var pagination = new Pagination();
                pagination.Pages = pages;
                pagination.Current = p;
                pagination.BaseUrl = string.Format("/home/betting?userid={0}&beginDate={1}&endDate={2}", userid, beginDate.Value.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));


                ViewBag.pagination = pagination;
                ViewBag.list = list;
                ViewBag.totalBetting = totalBetting;
                ViewBag.totalWinning = totalWinning;
                ViewBag.beginDate = beginDate.Value;
                ViewBag.endDate = endDate.Value;
                ViewBag.userid = userid;
            }
            catch
            {
                ViewBag.errorText = "未查询到任何记录";
            }

            return View();
        }

        #endregion

    }
}
