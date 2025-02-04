using Dapper;
using Domain.Interfaces.Data.DapperConfig;
using Domain.Security;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;

namespace Data.DapperConfig
{
    public class Connection : IConnection
    {
        DatabaseConfiguration _databaseConfiguration = new DatabaseConfiguration();
        private string _connectionString = "";
        public Connection(DatabaseConfiguration databaseConfiguration)
        {
            this._databaseConfiguration = databaseConfiguration;
            _connectionString = Environment.GetEnvironmentVariable(_databaseConfiguration.Connection_String);
        }

        public async Task ExecuteCreateCommand(string sqlQuery)
        {
            try
            {
                Batteries.Init();
                using (SqliteConnection con = new SqliteConnection())
                {
                    if (String.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = _connectionString;
                    }
                    con.Open();
                    var createTableCommand = con.CreateCommand();
                    createTableCommand.CommandText = sqlQuery;
                    createTableCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Connection :: ExecuteCreateCommand " + ex.Message);
            }
        }
        public async Task<List<T>> ExecuteGetList<T>(string sqlQuery, object param)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection())
                {
                    if (String.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = _connectionString;
                    }
                    return (List<T>)await con.QueryAsync<T>(sqlQuery, param);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(">> Connection :: ExecuteGetList " + ex.Message);
            }
        }
        public async Task ExecuteQuery(string sqlQuery, object param)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection())
                {
                    if (String.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = _connectionString;
                    }
                    con.Open();

                    await con.ExecuteAsync(sqlQuery, param);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(">> Connection :: ExecuteQueryParam " + ex.Message);
            }
        }
        public async Task<T> ExecuteGet<T>(string sqlQuery, object param)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection())
                {
                    if (String.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = _connectionString;
                    }
                    con.Open();

                    return (T)await con.QueryFirstOrDefaultAsync<T>(sqlQuery,param);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(">> Connection :: ExecuteSelectParam " + ex.Message);
            }
        }
        public async Task<List<T>> ExecuteProcGetList<T>(string sqlQuery, object param)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection())
                {
                    if (String.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = _connectionString;
                    }
                    con.Open();

                    return (List<T>)await con.QueryAsync<T>(sqlQuery, param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Connection :: ExecuteProcList " + ex.Message);
            }
        }
    }
}
