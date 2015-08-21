using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBC
{
    abstract public class DBCBase
    {
        public int ID { get; set; }
        protected string _tableName;
        public DBCBase(string tableName)
        {
            _tableName = tableName;
        }

        public object SetAttribute(string name, object newValue, object oldValue)
        {
            if (newValue == oldValue)
                return oldValue;

            var sql = "update " + _tableName + " set " + name + "=? where id=?";
            DB.SExecuteNonQuery(sql, newValue, ID);
            return newValue;
        }

        protected abstract void Initialize(string filter, params object[] args);
        
        public virtual void Delete()
        {
            var sql = "delete from " + _tableName + " where id=?";
            DB.SExecuteNonQuery(sql, ID);
        }
    }
}