using System;
using System.Collections.Generic;

namespace JZ.DapperManager
{
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }
        public DbStoreType DbType { get; set; }
    }

    public enum DbStoreType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3
    }

    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }
}
