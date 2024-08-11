using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace Chat.DAL.Repository
{
    public abstract class BaseRepository
    {
        private const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=test;Username=postgres;Password=example";

        private readonly ILogger _logger;

        public BaseRepository(ILogger logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                await connection.QueryAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе Execute");
                throw;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                return await connection.QueryAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе QueryAsync");
                throw;
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                return await connection.QueryFirstAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе QuerySingleAsync");
                throw;
            }
        }

        protected NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(CONNECTION_STRING);
        }
    }
}
