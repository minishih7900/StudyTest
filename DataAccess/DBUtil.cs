using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DBUtil
    {
        public int CommandTimeout = 0;
        private SqlConnection conn = null;

        private DBUtil(string connectionString)
        {
            if (conn == null)
            {
                conn = new SqlConnection(connectionString);
            }
        }
        /// <summary>
        /// 取得DB_Dapper實例
        /// </summary>
        /// <returns></returns>
        public static DBUtil CreateInstance(EConnectionString ec = EConnectionString.MainConnection)
        {
            var cstr = ConnectionStringHelper.GetConnectionString(ec);
            return (cstr != null) ? new DBUtil(cstr) : null;
        }

        /// <summary>
        /// 取得特定DB之Sql connection 
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public static SqlConnection Connection(EConnectionString ec = EConnectionString.MainConnection)
        {
            var cstr = ConnectionStringHelper.GetConnectionString(ec);
            return (cstr != null) ? new SqlConnection(cstr) : null;
        }

        /// <summary>
        /// 執行SQL查詢語句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL查詢語句</param>
        /// <param name="param">SQL查詢語句中所包含之@參數集合</param>
        /// <returns>SQL語句執行後之結果List</returns>
        public List<T> QueryList<T>(string sql, Dictionary<String, Object> param)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var list = conn.Query<T>(sql, param, commandType: CommandType.Text).ToList<T>();
                return list;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<T> QueryList<T>(string sql, object param)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var list = conn.Query<T>(sql, param, commandType: CommandType.Text).ToList<T>();
                return list;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 執行SQL指令(不適用於SQL查詢-新增修改使用)
        /// </summary>
        /// <typeparam name="T">傳入domain類別</typeparam>
        /// <param name="sql">SQL string指令</param>
        /// <param name="entity">類別實例(或param dictionary參數物件)</param>
        /// <returns>受影響筆數</returns>
        public int NonQuerySQL<T>(string sql, T entity)
        {
            int result = -1;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                result = conn.Execute(sql, entity, commandType: CommandType.Text);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public T ExecuteScalarSQL<T>(string sql, object param)
        {
            T result = default(T);
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                result = conn.ExecuteScalar<T>(sql, param, commandType: CommandType.Text);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
