using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBC
{
    public class Authority:DBCBase
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public Authority(int id):base(DBTables.Authority)
        {
            Initialize("id=?", id);
        }

        public Authority(string code):base(DBTables.Authority)
        {
            Initialize("code=?", code);
        }

        protected override void Initialize(string filter, params object[] args)
        {
            var sql =string.Format("select id,code,description from {0} where {1}",DBTables.Authority, filter);
            var res = DB.SExecuteReader(sql, args);
            if (res.Count == 0)
                throw new DBCNoRecordException();

            var row = res[0];
            ID = Convert.ToInt32(row[0]);
            Code = (string)row[1];
            Description = (string)row[2];
        }
    }
}
