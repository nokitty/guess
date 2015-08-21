using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace MakeScreening
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            CreateScreening(DateTime.Now);

            while (true)
            {
                var now = DateTime.Now;               
                //每天01：00：00生成当天所有场次
                if (now.Hour == 1 && now.Minute == 0 && now.Second == 0)
                {
                    CreateScreening(now);
                }

                //设置开奖结果
                if (now.Second == 59 && now.Minute % 10 == 9)
                {
                    SetResult(now);
                }

                //线程休息一秒钟
                var delta = 1000 - DateTime.Now.Millisecond;

                //Debug.WriteLine("开始时间：{0}，毫秒数为：{1}，设置的时间间隔为：{2}", now.ToLongTimeString(), now.Millisecond,delta);

                Thread.Sleep(delta);
            }
        }

        static void CreateScreening(DateTime now)
        {
            try
            {
                new DBC.Screening(new DateTime(now.Year, now.Month, now.Day, 13, 0, 1));
                return;
            }
            catch
            {
                var start = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
                //每天70场
                for (int i = 0; i < 70; i++)
                {
                    var end = start.AddMinutes(10);
                    DBC.Screening.Create(start, end);
                    start = end;
                }
            }
        }

        static void SetResult(DateTime now)
        {
            try
            {
                var screening = new DBC.Screening(now);
                //看看有没有预设当场结果
                if (screening.Preset != Enums.Fruits.None)
                {
                    screening.Result = screening.Preset;
                }
                //统计当场各种水果的数量
                else
                {
                    var list = new List<FruitCount>();
                    var sql = string.Format("select sum(orange),sum(banana),sum(grape),sum(pineapple),sum(strawberry),sum(watermelon) from {0} where screeningid=?", DBTables.Betting);
                    var res = DB.SExecuteReader(sql, screening.ID);
                    var row = res[0];
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Orange, Count = Utility.ToInt32(row[0]) });
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Banana, Count = Utility.ToInt32(row[1]) });
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Grape, Count = Utility.ToInt32(row[2]) });
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Pineapple, Count = Utility.ToInt32(row[3]) });
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Strawberry, Count = Utility.ToInt32(row[4]) });
                    list.Add(new FruitCount() { Fruit = Enums.Fruits.Watermelon, Count = Utility.ToInt32(row[5]) });

                    var min= list.Min(i => i.Count);
                    var same=list.Where(i => i.Count == min).ToList();
                    var rand = new Random();
                    screening.Result = same[rand.Next(same.Count)].Fruit;
                }

                //更新投注记录中的中奖额
                {
                    var sql = string.Format("update {0} set winning={1}*5 where screeningid=?", DBTables.Betting,screening.Result.ToString());
                    DB.SExecuteNonQuery(sql, screening.ID);
                }

                //更新中奖用户积分
                {
                    var sql = string.Format("update (select userid, winning from {0},{1} where screeningid=? and user.id=userid and user.role=?) as w, {0},user set points=points+w.winning where w.userid=user.id;", DBTables.Betting,DBTables.User);
                    DB.SExecuteNonQuery(sql,screening.ID, Enums.Roles.Normal);
                }
            }
            catch { }
        }
    }
}
