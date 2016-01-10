using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Halloumi.Common.Helpers
{
    public static class ConnectionStringHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server)
        {
            return NewConnectionString(server, "", "", "", 0);
        }

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <param name="timeout">The timeout for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server, int timeout)
        {
            return NewConnectionString(server, "", "", "", timeout);
        }

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <param name="database">The database name for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server, string database)
        {
            return NewConnectionString(server, database, "", "", 0);
        }

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <param name="database">The database name for the connection string.</param>
        /// <param name="timeout">The timeout for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server, string database, int timeout)
        {
            return NewConnectionString(server, database, "", "", timeout);
        }

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <param name="database">The database name for the connection string.</param>
        /// <param name="username">The username for the connection string.</param>
        /// <param name="password">The password for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server, string database, string username, string password)
        {
            return NewConnectionString(server, database, username, password, 0);
        }

        /// <summary>
        /// Creates a connection string from the specified options.
        /// </summary>
        /// <param name="server">The server name for the connection string.</param>
        /// <param name="database">The database name for the connection string.</param>
        /// <param name="username">The username for the connection string.</param>
        /// <param name="password">The password for the connection string.</param>
        /// <param name="timeout">The timeout for the connection string.</param>
        /// <returns>The created connection string</returns>
        public static string NewConnectionString(string server, string database, string username, string password, int timeout)
        {
            string connectionString = string.Empty;

            if (server == string.Empty)
            {
                server = ".";
            }
            connectionString += "Data Source=" + server + ";";

            if (database == string.Empty)
            {
                database = "Master";
            }
            connectionString += "Initial Catalog=" + database + ";";

            if (timeout > 0)
            {
                connectionString += "Timeout=" + timeout + ";";
            }

            if (username != string.Empty)
            {
                connectionString += "UID=" + username + ";";
                if (password != string.Empty)
                {
                    connectionString += "PWD=" + password + ";";
                }

            }
            else
            {
                connectionString += "Integrated Security=SSPI;";
            }

            return connectionString;
        }

        /// <summary>
        /// Tests a connection string by opening and closing a connection to the database
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if successful</returns>
        public static bool TestConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    
        #endregion
    }
}
