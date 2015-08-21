using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBC
{
    public class Betting : DBCBase
    {
        public DateTime Time { get; set; }
        public int UserID { get; set; }
        public int ScreeningID { get; set; }
        public int Orange { get; set; }
        public int Grape { get; set; }
        public int Banana { get; set; }
        public int Watermelon { get; set; }
        public int Strawberry { get; set; }
        public int Pineapple { get; set; }
        public int Winning { get; set; }
        public int Total { get; set; }

        public Screening Screening
        {
            get
            {
                return new Screening(ScreeningID);
            }
        }

        public Betting(int id)
            : base(DBTables.Betting)
        {
            Initialize("id=?", id);
        }

        protected override void Initialize(string filter, params object[] args)
        {
            var sql = "select id,`time`,userid,screeningid,orange,grape,banana,watermelon,strawberry,pineapple,winning,total from " + DBTables.Betting + " where " + filter;
            var res = DB.SExecuteReader(sql, args);
            if (res.Count == 0)
                throw new DBCNoRecordException();

            var row = res[0];
            ID = Convert.ToInt32(row[0]);
            Time = Convert.ToDateTime(row[1]);
            UserID = Convert.ToInt32(row[2]);
            ScreeningID = Convert.ToInt32(row[3]);
            Orange = Convert.ToInt32(row[4]);
            Grape = Convert.ToInt32(row[5]);
            Banana = Convert.ToInt32(row[6]);
            Watermelon = Convert.ToInt32(row[7]);
            Strawberry = Convert.ToInt32(row[8]);
            Pineapple = Convert.ToInt32(row[9]);
            Winning = Convert.ToInt32(row[10]);
            Total = Convert.ToInt32(row[11]);
        }

        public static void Create(uint orange, uint banana, uint grape, uint watermelon, uint strawberry, uint pineapple,uint total, int screeningid, int userid)
        {
            if (total == 0)
                throw new DBCCreateErrorException("投注不能为0");

            var sql = string.Format("insert into {0}(orange,banana,grape,watermelon,strawberry,pineapple,screeningid,userid,time,total) values (?,?,?,?,?,?,?,?,?,?)", DBTables.Betting);
            DB.SExecuteNonQuery(sql, orange, banana, grape, watermelon, strawberry, pineapple, screeningid, userid, DateTime.Now,total);
        }
    }
}
