using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBC
{
    public class DBCException : Exception
    {
        public DBCException(string msg) : base(msg) { }
    }

    public class DBCNoRecordException : DBCException
    {
        public DBCNoRecordException() : base("无该记录") { }
    }

    public class DBCConflictException : DBCException
    {
        public DBCConflictException() : base("已经有该记录") { }
    }

    public class DBCCreateErrorException:DBCException
    {
        public DBCCreateErrorException() : base("创建失败") { }
        public DBCCreateErrorException(string msg) : base(msg) { }
    }
}
