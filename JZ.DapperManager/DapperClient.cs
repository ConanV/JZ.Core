using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.DapperManager
{
    public class DapperClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClient(IOptionsMonitor<ConnectionConfig> config)
        {
            CurrentConnectionConfig = config.CurrentValue;
        }

        public DapperClient(ConnectionConfig config) { CurrentConnectionConfig = config; }

        IDbConnection _connection = null;
        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    //case DbStoreType.MySql:
                    //    _connection = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    //case DbStoreType.Sqlite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    case DbStoreType.SqlServer:
                        _connection = new System.Data.SqlClient.SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    //case DbStoreType.Oracle:
                    //    _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }

        /// <summary>
        /// 查询SQL得到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        //public virtual List<T> Query<T>(string strSQL)
        //{
        //    using (IDbConnection conn = Connection)
        //    {
        //        return Query<T>(strSQL, null);
        //    }
        //}
        /// <summary>
        /// 查询SQL得到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="param">查询参数model</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSQL, object? param = null)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSQL, param).AsList();
            }
        }

        public virtual List<TReturn> Query<TFirst, TSecond, TReturn>(string strSQL, Func<TFirst, TSecond, TReturn> map, string splitOn, object? param = null)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<TFirst, TSecond, TReturn>(strSQL, map, param, splitOn: splitOn).AsList();
            }
        }

        /// <summary>
        /// 异步查询SQL得到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        //public virtual async Task<List<T>> QueryAsync<T>(string strSQL)
        //{
        //    return await QueryAsync<T>(strSQL, null);
        //}
        /// <summary>
        /// 异步查询SQL得到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="param">查询参数model</param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAsync<T>(string strSQL, object? param = null)
        {
            using (IDbConnection conn = Connection)
            {
                if (param == null)
                {
                    var res = await conn.QueryAsync<T>(strSQL);
                    return res.ToList<T>();
                }
                else
                {
                    var res = await conn.QueryAsync<T>(strSQL, param);
                    return res.ToList<T>();
                }
            }
        }

        /// <summary>
        /// 执行SQL返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSQL)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSQL).FirstOrDefault<T>();
            }
        }
        /// <summary>
        /// 异步执行SQL返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string strSQL)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSQL);
                return res.FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="param">查询参数model</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSQL, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSQL, param).FirstOrDefault<T>();
            }
        }
        /// <summary>
        /// 异步执行SQL返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="param">查询参数model</param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string strSQL, object param)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSQL, param);
                return res.FirstOrDefault<T>();
            }
        }
        /// <summary>
        /// 执行SQL返回影响的条数
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="param">查询参数model</param>
        /// <returns></returns>
        public virtual int Excute(string strSQL, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Execute(strSQL, param);
            }
        }
        /// <summary>
        /// 执行存储过程返回影响的条数
        /// </summary>
        /// <param name="strProcedure">存储过程名称</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Execute(strProcedure, null, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
