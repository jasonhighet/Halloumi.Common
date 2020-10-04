using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Helper methods for SQL access
    /// </summary>
    public static class SqlHelper
    {
        #region Private Varibles

        /// <summary>
        /// A cached dictionary of all connection strings in use
        /// </summary>
        private static Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();

        /// <summary>
        /// The name of the first connection string in the config file
        /// </summary>
        private static string _defaultConnectionName = "";

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a new sql connection object based on the specified connection name
        /// </summary>
        /// <param name="connectionName">Name of the connection in the config file.</param>
        /// <returns>A new unopened database connection</returns>
        public static SqlConnection NewConnection(string connectionName)
        {
            return new SqlConnection(GetConnectionString(connectionName));
        }

        /// <summary>
        /// Returns a new sql connection object based on the default connection name
        /// </summary>
        /// <returns>A new unopened database connection</returns>
        public static SqlConnection NewConnection()
        {
            return new SqlConnection(GetConnectionString(GetDefaultConnectionName()));
        }

        /// <summary>
        /// Gets the name of the default connection string (the first listed in the config file)
        /// </summary>
        /// <returns>The name of the default connection string</returns>
        public static string GetDefaultConnectionName()
        { 
            if(_defaultConnectionName == "")
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    _defaultConnectionName = ConfigurationManager.ConnectionStrings[0].Name;
                }
            }
            return _defaultConnectionName;
        }

        /// <summary>
        /// Returns the specified connection string based on the supplied name
        /// </summary>
        /// <param name="name">The name of the connection string.</param>
        /// <returns>The specified connection string</returns>
        public static string GetConnectionString(string name)
        {
            var connectionString = "";

            if (_connectionStrings.ContainsKey(name))
            {
                // use connection string in cache
                connectionString = _connectionStrings[name];
            }
            else
            {
                // otherwise read from config file and add to cache
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
                _connectionStrings.Add(name, connectionString);
            }
            return connectionString;
        }

        /// <summary>
        /// Executes a stored procedure and returns a data reader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>
        /// A data reader of the results of the stored procedure
        /// </returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string name, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(name, CommandType.StoredProcedure, parameters, connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes a stored procedure and returns a data reader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <returns>
        /// A data reader of the results of the stored procedure
        /// </returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string name)
        {
            return ExecuteReader(connection, name, null);
        }

        /// <summary>
        /// Executes a stored procedure and returns a dataset.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>
        /// A dataset of the results of the stored procedure
        /// </returns>
        public static DataSet ExecuteDataSet(SqlConnection connection, string name, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(name, CommandType.StoredProcedure, parameters, connection);

            // create and fill the dataset via a table adapter
            var adapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();
            adapter.Fill(dataSet);

            return dataSet;
        }

        /// <summary>
        /// Executes a stored procedure and returns a dataset.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <returns>
        /// A dataset of the results of the stored procedure
        /// </returns>
        public static DataSet ExecuteDataSet(SqlConnection connection, string name)
        {
            return ExecuteDataSet(connection, name, null);
        }

        /// <summary>
        /// Executes a stored procedure and returns a single value
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>
        /// The single value result of the stored procedure as an object
        /// </returns>
        public static object ExecuteScalar(SqlConnection connection, string name, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(name, CommandType.StoredProcedure, parameters, connection);
            return command.ExecuteScalar();
        }

        /// <summary>
        /// Executes a stored procedure and returns a single value
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <returns>
        /// The single value result of the stored procedure as an object
        /// </returns>
        public static object ExecuteScalar(SqlConnection connection, string name)
        {
            return ExecuteScalar(connection, name, null);
        }

        /// <summary>
        /// Executes a stored procedure and doesn't return any results
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        public static void ExecuteNonQuery(SqlConnection connection, string name, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(name, CommandType.StoredProcedure, parameters, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a stored procedure and doesn't return any results
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        public static void ExecuteNonQuery(SqlConnection connection, string name)
        {
            ExecuteNonQuery(connection, name, null);
        }

        /// <summary>
        /// Executes a sql statement and returns a data reader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters for the sql statement.</param>
        /// <returns>
        /// A data reader of the results of the sql statement.
        /// </returns>
        public static SqlDataReader ExecuteReaderSql(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(sql, CommandType.Text, parameters, connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes a sql statement and returns a data reader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// A data reader of the results of the sql statement.
        /// </returns>
        public static SqlDataReader ExecuteReaderSql(SqlConnection connection, string sql)
        {
            return ExecuteReaderSql(connection, sql, null);
        }

        /// <summary>
        /// Executes a sql statement and returns a dataset.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters for the sql statement.</param>
        /// <returns>
        /// A dataset of the results of the sql statement.
        /// </returns>
        public static DataSet ExecuteDataSetSql(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(sql, CommandType.Text, parameters, connection);

            // create and fill the dataset via a table adapter
            var adapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();
            adapter.Fill(dataSet);
                
            return dataSet;
        }

        /// <summary>
        /// Executes a sql statement and returns a dataset.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// A dataset of the results of the sql statement.
        /// </returns>
        public static DataSet ExecuteDataSetSql(SqlConnection connection, string sql)
        {
            return ExecuteDataSetSql(connection, sql, null);
        }

        /// <summary>
        /// Executes a sql statement and returns a single value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters for the sql statement.</param>
        /// <returns>
        /// The single value result of the sql statement as an object.
        /// </returns>
        public static object ExecuteScalarSql(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(sql, CommandType.Text, parameters, connection);
            return command.ExecuteScalar();
        }

        /// <summary>
        /// Executes a sql statement and returns a single value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// The single value result of the sql statement as an object.
        /// </returns>
        public static object ExecuteScalarSql(SqlConnection connection, string sql)
        {
            return ExecuteScalarSql(connection, sql, null);
        }

        /// <summary>
        /// Executes a sql statement that doesn't return any results.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters for the sql statement.</param>
        public static void ExecuteNonQuerySql(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            connection.Open();
            var command = CreateCommand(sql, CommandType.Text, parameters, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a sql statement that doesn't return any results.
        /// </summary>
        /// <param name="name">The sql statement to execute.</param>
        public static void ExecuteNonQuerySql(SqlConnection connection, string sql)
        {
            ExecuteNonQuerySql(connection, sql, null);
        }

        /// <summary>
        /// Executes a sql statement via a data reader which is
        /// then converted to and returned as a datatable
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters for the sql statement.</param>
        /// <returns>
        /// A data reader of the results of the sql statement.
        /// </returns>
        public static DataTable ExecuteDataTableSql(SqlConnection connection, string sql, SqlParameter[] parameters)
        {
            connection.Open();

            var command = CreateCommand(sql, CommandType.Text, parameters, connection);
            var table = new DataTable();
            using (var reader = command.ExecuteReader())
            {
                table.Load(reader);
            }

            return table;
        }

        /// <summary>
        /// Executes a sql statement via a data reader which is
        /// then converted to and returned as a datatable
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// A data reader of the results of the sql statement.
        /// </returns>
        public static DataTable ExecuteDataTableSql(SqlConnection connection, string sql)
        {
            return ExecuteDataTableSql(connection, sql, null);
        }

        /// <summary>
        /// Executes a stored procedure via a data reader which is
        /// then converted to and returned as a datatable
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>
        /// A data reader of the results of the stored procedure
        /// </returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, string name, SqlParameter[] parameters)
        {
            connection.Open();

            var command = CreateCommand(name, CommandType.StoredProcedure, parameters, connection);
            var table = new DataTable();
            using (var reader = command.ExecuteReader())
            {
                table.Load(reader);
            }

            return table;
        }

        /// <summary>
        /// Executes a stored procedure via a data reader which is
        /// then converted to and returned as a datatable
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="name">The name of the stored procedure to execute.</param>
        /// <returns>
        /// A data reader of the results of the stored procedure
        /// </returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, string name)
        {
            return ExecuteDataTable(connection, name, null);
        }

        /// <summary>
        /// Creates a new command parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <param name="direction">The parameter direction.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter Parameter(string name, SqlDbType type, object value, int size, ParameterDirection direction)
        {
            // throw error if name is empty
            if (name.Length == 0)
            {
                throw (new ArgumentException("Parameter 'name' cannot be empty"));
            }

            // append @ to the start of the name if neccessry
            if (!name.StartsWith("@"))
            {
                name = "@" + name;
            }

            var parameter = new SqlParameter(name, type, size);
            parameter.Direction = direction;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Creates a new command input parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter Parameter(string name, SqlDbType type, object value, int size)
        {
            return Parameter(name, type, value, size, ParameterDirection.Input);
        }

        /// <summary>
        /// Creates a new command input parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter Parameter(string name, SqlDbType type, object value)
        {
            return Parameter(name, type, value, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// Creates a new command output parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter OutParameter(string name, SqlDbType type, int size)
        {
            return Parameter(name, type, null, size, ParameterDirection.Output);
        }

        /// <summary>
        /// Creates a new command output parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter OutParameter(string name, SqlDbType type)
        {
            return Parameter(name, type, null, 0, ParameterDirection.Output);
        }

        /// <summary>
        /// Creates a new command return parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter ReturnParameter(string name, SqlDbType type, int size)
        {
            return Parameter(name, type, null, size, ParameterDirection.ReturnValue);
        }

        /// <summary>
        /// Creates a new command return parameter based on the specified options.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The database type of the parameter.</param>
        /// <returns>The new command parameter</returns>
        public static SqlParameter ReturnParameter(string name, SqlDbType type)
        {
            return Parameter(name, type, null, 0, ParameterDirection.ReturnValue);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates an sql command from the specified options.
        /// </summary>
        /// <param name="commandText">The text for the command.</param>
        /// <param name="commandType">The type of command.</param>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>The sql command object</returns>
        private static SqlCommand CreateCommand(string commandText, 
            CommandType commandType, 
            SqlParameter[] parameters, 
            SqlConnection connection)
        {
            var command = new SqlCommand();

            command.Connection = connection;
            command.CommandType = commandType;
            command.CommandText = commandText;
            
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        #endregion
    }
}
