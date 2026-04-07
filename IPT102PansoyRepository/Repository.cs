using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework
{
    public class Repository
    {
        private readonly IConfiguration _configuration;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveDataAsync(
            string connectionName,
            string storedProcedureName,
            DynamicParameters parameters)
        {
            var connectionString = _configuration.GetConnectionString(connectionName);

            using IDbConnection connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(
                storedProcedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(
            string connectionName,
            string storedProcedureName,
            DynamicParameters? parameters = null) where T : class
        {
            var connectionString = _configuration.GetConnectionString(connectionName);

            using IDbConnection connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<T>(
                storedProcedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}