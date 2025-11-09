using System;
using System.Data;
using Microsoft.Data.SqlClient; // Para SQL Server
using Microsoft.Extensions.Configuration;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Enum; // tu DatabaseProvider

namespace TecWeb.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _sqlConn;
        public DatabaseProvider Provider { get; }

        public DbConnectionFactory(IConfiguration config)
        {
            _sqlConn = config.GetConnectionString("ConnectionSqlServer")
                       ?? throw new ArgumentNullException("ConnectionSqlServer no definida en appsettings.json");

            Provider = DatabaseProvider.SqlServer; // Solo SQL Server
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_sqlConn);
        }
    }
}
