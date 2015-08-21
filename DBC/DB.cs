using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public class DB : IDisposable
{
    MySqlConnection _connection;
    MySqlTransaction _transaction = null;
    public static string ConnectionString { set; get; }
    public DB(string str = "")
    {
        if (str == "")
            str = ConnectionString;

        if (str == "")
            throw new Exception("连接字符串为空");

        _connection = new MySqlConnection(str);
        _connection.Open();
    }

    public void BeginTransaction()
    {
        _transaction = _connection.BeginTransaction();
    }
    public void EndTransaction()
    {
        _transaction.Commit();
        _transaction = null;
    }

    public MySqlCommand CommandPrepare(string sql, params object[] arguments)//预处理
    {
        MySqlCommand command = _connection.CreateCommand();
        command.Transaction = _transaction;
        command.CommandText = sql;
        foreach (object o in arguments)
        {
            MySqlParameter param = new MySqlParameter();
            param.Value = o;
            command.Parameters.Add(param);
        }
        return command;
    }

    public object ExecuteScalar(string sql, params object[] parameter)//查询
    {
        return CommandPrepare(sql, parameter).ExecuteScalar();
    }
    public static object SExecuteScalar(string sql, params object[] parameter)
    {
        using (DB db = new DB())
        {
            return db.ExecuteScalar(sql, parameter);
        }
    }

    public int ExecuteNonQuery(string sql, params object[] parameter)//执行
    {
        return CommandPrepare(sql, parameter).ExecuteNonQuery();
    }
    public static int SExecuteNonQuery(string sql, params object[] parameter)
    {
        using (DB db = new DB())
        {
            return db.ExecuteNonQuery(sql, parameter);
        }
    }

    public int GetLastInsertId()
    {
        return Convert.ToInt32(ExecuteScalar("select LAST_INSERT_ID()"));
    }


    public List<object[]> ExecuteReader(string sql, params object[] parameter)
    {
        List<object[]> o = new List<object[]>();
        using (MySqlDataReader reader = CommandPrepare(sql, parameter).ExecuteReader())
        {
            int count = reader.FieldCount;
            while (reader.Read())
            {
                object[] datas = new object[count];
                for (int i = 0; i < count; i++)
                {
                    datas[i] = reader[i];
                }
                o.Add(datas);
            }
        }
        return o;
    }
    public static List<object[]> SExecuteReader(string sql, params object[] parameter)
    {
        using (DB db = new DB())
        {
            return db.ExecuteReader(sql, parameter);
        }
    }
    public int Insert(string sql, params object[] parameter)
    {
        ExecuteNonQuery(sql, parameter);
        return Convert.ToInt32(GetLastInsertId());
    }
    public static int SInsert(string sql, params object[] parameter)
    {
        using (DB db = new DB())
        {
            return db.Insert(sql, parameter);
        }
    }

    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Dispose();
        }
    }
}

