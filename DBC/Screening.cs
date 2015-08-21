using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;

namespace DBC
{
    public class Screening : DBCBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        Fruits _result;
        public Fruits Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = (Fruits)SetAttribute("result", value, _result);
            }
        }

        Fruits _preset;
        public Fruits Preset
        {
            get
            {
                return _preset;
            }
            set
            {
                _preset = (Fruits)SetAttribute("preset", value, _preset);
            }
        }

        public Screening(int id)
            : base(DBTables.Screening)
        {
            Initialize("id=?", id);
        }

        public Screening(DateTime time)
            : base(DBTables.Screening)
        {
            Initialize("end>? and start<?", time, time);
        }

        /// <summary>
        /// 获取统计数值
        /// </summary>
        /// <returns></returns>
        public int GetCount(string field)
        {
            var sql = string.Format("select sum({1}) from {0} where screeningid=?", DBTables.Betting,field);
            return Utility.ToInt32(DB.SExecuteScalar(sql, ID));
        }

        protected override void Initialize(string filter, params object[] args)
        {
            var sql = "select id,result,start,end,preset from " + DBTables.Screening + " where " + filter;
            var res = DB.SExecuteReader(sql, args);
            if (res.Count == 0)
                throw new DBCNoRecordException();

            var row = res[0];
            ID = Convert.ToInt32(row[0]);
            Result = (Fruits)Convert.ToByte(row[1]);
            Start = Convert.ToDateTime(row[2]);
            End = Convert.ToDateTime(row[3]);
            _preset = (Fruits)Convert.ToByte(row[4]);
        }

        public static void Create(DateTime start, DateTime end)
        {
            var sql = string.Format("insert into {0}(start,end) values (?,?)", DBTables.Screening);
            DB.SExecuteNonQuery(sql, start, end);
        }
    }
}
