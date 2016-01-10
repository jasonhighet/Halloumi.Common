using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Halloumi.Common.Windows.Controls
{
    public class DatabaseComboBox : ComboBox
    {
        #region Private Variables

        /// <summary>
        /// The server to connect to
        /// </summary>
        private string _server = string.Empty;

        /// <summary>
        /// The last server connected to
        /// </summary>
        private string _lastServer = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DatabaseDropDown control
        /// </summary>
        public DatabaseComboBox()
            : base()
        {
            this.DisplayMember = "DATABASE_NAME";
            this.ValueMember = "DATABASE_NAME";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the server to get the list of database from
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        [Description("Specifies the the server to get the list of database from")]
        public string Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (this.DesignMode)
                {
                    this.Text = _server;
                    this.Invalidate();
                }
                else
                {
                    this.LoadDatabaseList();
                }
            }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object DataSource
        {
            get { return base.DataSource; }
            set { base.DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the display member.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string DisplayMember
        {
            get { return base.DisplayMember; }
            set { base.DisplayMember = value; }
        }

        /// <summary>
        /// Gets or sets the value member.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set { base.ValueMember = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the database list.
        /// </summary>
        private void LoadDatabaseList()
        {
            if (_server != "")
            {
                if (_server != _lastServer)
                {
                    // set cursor to busy
                    this.FindForm().Cursor = Cursors.WaitCursor;
                    Application.DoEvents();

                    // load databases from server
                    this.DisplayMember = "DATABASE_NAME";
                    this.ValueMember = "DATABASE_NAME";
                    this.DataSource = GetDatabases(_server);

                    // set cursor to normal
                    this.FindForm().Cursor = Cursors.Default;
                }
            }
            else
            {
                this.DataSource = null;
            }
            _lastServer = _server;
        }

        /// <summary>
        /// Gets a list of databases from a server as a dataset.
        /// </summary>
        /// <param name="server">The server to get the list of databases from.</param>
        /// <returns>A list of databases as a dataset</returns>
        private static DataTable GetDatabases(string server)
        {
            // generate connection string with short timeout
            string connectionString = "Data Source=" + server + ";Timeout=5;Integrated Security=SSPI;";

            DataTable databases = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // get list of databases and sort by name
                    connection.Open();
                    databases = connection.GetSchema("DATABASES");
                    databases.DefaultView.Sort = "DATABASE_NAME";
                    connection.Close();
                }
            }
            catch
            {
                // ignore errors if unable to connect
            }

            return databases;
        }

        #endregion
    }
}
