using Merchanter.Classes.Settings;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter {
    public partial class DbHelper {

        #region CORE
        private MySqlConnection connection = new();
        /// <summary>
        /// Represents the MySQL database connection.
        /// </summary>
        private MySqlConnection Connection {
            get { return connection; }
            set { connection = value; }
        }

        private string server = string.Empty;
        /// <summary>
        /// Gets or sets the MySQL server address.
        /// </summary>
        public string Server {
            get { return server; }
            set { server = value; }
        }

        private string user = string.Empty;
        /// <summary>
        /// Gets or sets the MySQL username.
        /// </summary>
        public string User {
            get { return user; }
            set { user = value; }
        }

        private string password = string.Empty;
        /// <summary>
        /// Gets or sets the MySQL password.
        /// </summary>
        public string Password {
            get { return password; }
            set { password = value; }
        }

        private int port;
        /// <summary>
        /// Gets or sets the MySQL server port.
        /// </summary>
        public int Port {
            get { return port; }
            set { port = value; }
        }

        private string database = string.Empty;
        /// <summary>
        /// Gets or sets the MySQL database name.
        /// </summary>
        public string Database {
            get { return database; }
            set { database = value; }
        }

        private string? connectionString { get; set; } = null;
        /// <summary>
        /// Gets or sets the connection state of the database.
        /// </summary>
        public System.Data.ConnectionState state { get; set; }
        public List<DBSetting> DbSettings { get; set; } = [];
        public DbHelper invoice_clone { get; set; } = null;
        public DbHelper xml_clone { get; set; } = null;
        public DbHelper notification_clone { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbHelper"/> class with the specified database connection parameters.
        /// </summary>
        /// <param name="_server">The MySQL server address.</param>
        /// <param name="_user">The MySQL username.</param>
        /// <param name="_password">The MySQL password.</param>
        /// <param name="_database">The MySQL database name.</param>
        /// <param name="_port">The MySQL server port.</param>
        /// <param name="_timeout">The connection timeout in seconds.</param>
        public DbHelper(string _server, string _user, string _password, string _database,
            int _port = 3306, int _timeout = 120) {
            try {
                Server = _server; User = _user; Password = _password;
                Database = _database; Port = _port;
                connectionString = "Server=" + Server + ";Port=" + Port.ToString() + ";" + "Database=" +
                    Database + ";" + "Uid=" + User + ";" + "Pwd=" + Password + ";CharSet=utf8;" + "Connection Timeout=" + _timeout + ";";
                Connection.ConnectionString = connectionString;
                Connection.StateChange += Connection_StateChange;
                Connection.InfoMessage += Connection_InfoMessage;
            }
            catch {
                DbHelperBase.PrintConsole("Error in DB Connection", ConsoleColor.Red);
            }
        }
        #endregion

        #region Delegates
        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e) => state = e.CurrentState;
        private void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs e) {
            foreach (var item in e.errors) {
                DbHelperBase.PrintConsole(item.Code + ":" + item.Level + ":" + item.Message, ConsoleColor.DarkYellow);
            }
        }
        public event EventHandler<string>? ErrorOccured;
        #endregion

        #region Events
        protected virtual void OnError(string e) {
            ErrorOccured?.Invoke(this, e);
        }
        #endregion

        #region Connection Methods
        /// <summary>
        /// Opens the database connection.
        /// </summary>
        /// <returns>True if the connection is successfully opened; otherwise, false.</returns>
        public async Task<bool> OpenConnection() {
            try {
                await connection.OpenAsync();
                return true;
            }
            catch (MySqlException ex) {
                switch (ex.Number) {
                    case 0:
                        OnError(ex.ToString());
                        break;

                    case 1045:
                        OnError(ex.ToString());
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        /// <returns>True if the connection is successfully closed; otherwise, false.</returns>
        public async Task<bool> CloseConnection() {
            try {
                await connection.CloseAsync();
                return true;
            }
            catch (MySqlException ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        #endregion
    }
}
