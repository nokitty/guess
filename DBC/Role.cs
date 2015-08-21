using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBC
{
    public class Role:DBCBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Role(int id):base(DBTables.Role)
        {
            Initialize("id=?", id);
        }

        protected override void Initialize(string filter, params object[] args)
        {
            var sql = string.Format("select id,name,description from {0} where {1}", DBTables.Role, filter);
            var res = DB.SExecuteReader(sql, args);
            if (res.Count == 0)
                throw new DBCNoRecordException();

            var row = res[0];
            ID = Convert.ToInt32(row[0]);
            Name = (string)row[1];
            Description = (string)row[2];
        }
    }
}
