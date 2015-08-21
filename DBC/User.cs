using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBC
{
    public class User : DBCBase
    {
        public string Tel { get; set; }
        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = (string)SetAttribute("password", value, _password);
            }
        }
        public Enums.Roles Role { set; get; }
        string _remark;
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = (string)SetAttribute("remark", value, _remark);
            }
        }
        public int Parent { get; set; }
        uint _points;
        public uint Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = (uint)SetAttribute("points", value, _points);
            }
        }

        public User(int id)
            : base(DBTables.User)
        {
            Initialize("id=?", id);
        }
        public User(string tel, string password)
            : base(DBTables.User)
        {
            Initialize("tel=? and password=?", tel, password);
        }
        protected override void Initialize(string filter, params object[] args)
        {
            var sql = "select id,tel,password,role,remark,parent,points from " + DBTables.User + " where " + filter;
            var res = DB.SExecuteReader(sql, args);
            if (res.Count == 0)
                throw new DBCNoRecordException();

            var row = res[0];
            ID = Convert.ToInt32(row[0]);
            Tel = (string)row[1];
            _password = (string)row[2];
            Role = (Enums.Roles)row[3];
            _remark = (string)row[4];
            Parent = Convert.ToInt32(row[5]);
            _points = Convert.ToUInt32(row[6]);
        }
        public static User Create(string tel, string password, Enums.Roles role, int parent, string remark)
        {
            tel = tel.Trim();
            var sql = string.Format("insert into {0} (tel,password,role,parent,remark) values (?,?,?,?,?)", DBTables.User);
            var id = DB.SInsert(sql, tel, password, role, parent, remark);
            return new User(id);
        }
    }
}
