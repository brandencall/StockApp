using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration config;

        public SqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }

        public List<T> LoadData<T, U>(string sqlStatement,
                                      U paramters,
                                      string connectionStringName,
                                      bool isStroredProcedure = false)
        {
            string? connectionString = config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (isStroredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, paramters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                     T paramters,
                                     string connectionStringName,
                                     bool isStroredProcedure = false)
        {
            string? connectionString = config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (isStroredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, paramters, commandType: commandType);
            }
        }
    }
}
