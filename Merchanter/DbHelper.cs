using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using Attribute = Merchanter.Classes.Attribute;

namespace Merchanter {
    public class DbHelper {

        #region CORE
        private MySqlConnection connection = new();
        public MySqlConnection Connection {
            get { return connection; }
            set { connection = value; }
        }

        private string server = string.Empty;
        public string Server {
            get { return server; }
            set { server = value; }
        }

        private string user = string.Empty;
        public string User {
            get { return user; }
            set { user = value; }
        }

        private string password = string.Empty;
        public string Password {
            get { return password; }
            set { password = value; }
        }

        private int port;
        public int Port {
            get { return port; }
            set { port = value; }
        }

        private string database = string.Empty;
        public string Database {
            get { return database; }
            set { database = value; }
        }

        private string? connectionString { get; set; } = null;
        public System.Data.ConnectionState state { get; set; }
        public List<DBSetting> DbSettings { get; set; } = [];
        public DbHelper invoice_clone { get; set; }
        public DbHelper xml_clone { get; set; }
        public DbHelper notification_clone { get; set; }

        public DbHelper(string _server, string _user, string _password, string _database, int _port = 3306, int _timeout = 120) {
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
                PrintConsole("Error in DB Connection", ConsoleColor.Red);
            }
        }
        #endregion

        #region Delegates
        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e) => state = e.CurrentState;
        private void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs e) {
            foreach (var item in e.errors) {
                PrintConsole(item.Code + ":" + item.Level + ":" + item.Message, ConsoleColor.DarkYellow);
            }
        }
        public event EventHandler<string> ErrorOccured;
        #endregion

        #region Events
        protected virtual void OnError(string e) {
            ErrorOccured?.Invoke(this, e);
        }
        #endregion

        #region Connection Methods
        public bool OpenConnection() {
            try {
                connection.Open();
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

        public bool CloseConnection() {
            try {
                connection.Close();
                return true;
            }
            catch (MySqlException ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Customer
        /// <summary>
        /// Gets the customer from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Customer? GetCustomer(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM customer WHERE customer_id=@customer_id";
                Customer? c = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    c = new Customer {
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        user_name = dataReader["user_name"].ToString(),
                        person_name = dataReader["person_name"].ToString(),
                        email = dataReader["email"].ToString(),
                        password = dataReader["password"].ToString(),
                        role = (Customer.Role)Convert.ToInt32(dataReader["role"].ToString()),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"]?.ToString())),
                        product_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["product_sync_status"].ToString())),
                        order_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["order_sync_status"].ToString())),
                        xml_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_sync_status"].ToString())),
                        invoice_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["invoice_sync_status"].ToString())),
                        notification_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["notification_sync_status"].ToString())),
                        product_sync_timer = Convert.ToInt32(dataReader["product_sync_timer"].ToString()),
                        order_sync_timer = Convert.ToInt32(dataReader["order_sync_timer"].ToString()),
                        xml_sync_timer = Convert.ToInt32(dataReader["xml_sync_timer"].ToString()),
                        invoice_sync_timer = Convert.ToInt32(dataReader["invoice_sync_timer"].ToString()),
                        notification_sync_timer = Convert.ToInt32(dataReader["notification_sync_timer"].ToString()),
                        last_product_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_product_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_product_sync_date"].ToString()) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_order_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_order_sync_date"].ToString()) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_xml_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_xml_sync_date"].ToString()) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_invoice_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_invoice_sync_date"].ToString()) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_notification_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_notification_sync_date"].ToString()) : null,
                        is_productsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_productsync_working"].ToString())),
                        is_ordersync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_ordersync_working"].ToString())),
                        is_xmlsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xmlsync_working"].ToString())),
                        is_invoicesync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_invoicesync_working"].ToString())),
                        is_notificationsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notificationsync_working"].ToString())),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the customer from the database
        /// </summary>
        /// <param name="_username">Username</param>
        /// <param name="_password">Password</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Customer? GetCustomer(string _username, string _password) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM customer WHERE user_name=@user_name AND password=@password;";
                Customer? c = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("user_name", _username));
                cmd.Parameters.Add(new MySqlParameter("password", _password));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    c = new Customer {
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        user_name = dataReader["user_name"].ToString(),
                        person_name = dataReader["person_name"].ToString(),
                        email = dataReader["email"].ToString(),
                        password = dataReader["password"].ToString(),
                        role = (Customer.Role)Convert.ToInt32(dataReader["role"].ToString()),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                        product_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["product_sync_status"].ToString())),
                        order_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["order_sync_status"].ToString())),
                        xml_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_sync_status"].ToString())),
                        invoice_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["invoice_sync_status"].ToString())),
                        notification_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["notification_sync_status"].ToString())),
                        product_sync_timer = Convert.ToInt32(dataReader["product_sync_timer"].ToString()),
                        order_sync_timer = Convert.ToInt32(dataReader["order_sync_timer"].ToString()),
                        xml_sync_timer = Convert.ToInt32(dataReader["xml_sync_timer"].ToString()),
                        invoice_sync_timer = Convert.ToInt32(dataReader["invoice_sync_timer"].ToString()),
                        notification_sync_timer = Convert.ToInt32(dataReader["notification_sync_timer"].ToString()),
                        last_product_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_product_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_product_sync_date"].ToString()) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_order_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_order_sync_date"].ToString()) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_xml_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_xml_sync_date"].ToString()) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_invoice_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_invoice_sync_date"].ToString()) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_notification_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_notification_sync_date"].ToString()) : null,
                        is_productsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_productsync_working"].ToString())),
                        is_ordersync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_ordersync_working"].ToString())),
                        is_xmlsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xmlsync_working"].ToString())),
                        is_invoicesync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_invoicesync_working"].ToString())),
                        is_notificationsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notificationsync_working"].ToString())),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the customer from the database
        /// </summary>
        /// <param name="_email">E-mail</param>
        /// <param name="_password">Password</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Customer? GetCustomerByMail(string _email, string _password) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM customer WHERE email=@email AND password=@password;";
                Customer? c = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("email", _email));
                cmd.Parameters.Add(new MySqlParameter("password", _password));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    c = new Customer {
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        user_name = dataReader["user_name"].ToString(),
                        person_name = dataReader["person_name"].ToString(),
                        email = dataReader["email"].ToString(),
                        password = dataReader["password"].ToString(),
                        role = (Customer.Role)Convert.ToInt32(dataReader["role"].ToString()),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                        product_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["product_sync_status"].ToString())),
                        order_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["order_sync_status"].ToString())),
                        xml_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_sync_status"].ToString())),
                        invoice_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["invoice_sync_status"].ToString())),
                        notification_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["notification_sync_status"].ToString())),
                        product_sync_timer = Convert.ToInt32(dataReader["product_sync_timer"].ToString()),
                        order_sync_timer = Convert.ToInt32(dataReader["order_sync_timer"].ToString()),
                        xml_sync_timer = Convert.ToInt32(dataReader["xml_sync_timer"].ToString()),
                        invoice_sync_timer = Convert.ToInt32(dataReader["invoice_sync_timer"].ToString()),
                        notification_sync_timer = Convert.ToInt32(dataReader["notification_sync_timer"].ToString()),
                        last_product_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_product_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_product_sync_date"].ToString()) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_order_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_order_sync_date"].ToString()) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_xml_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_xml_sync_date"].ToString()) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_invoice_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_invoice_sync_date"].ToString()) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_notification_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_notification_sync_date"].ToString()) : null,
                        is_productsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_productsync_working"].ToString())),
                        is_ordersync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_ordersync_working"].ToString())),
                        is_xmlsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xmlsync_working"].ToString())),
                        is_invoicesync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_invoicesync_working"].ToString())),
                        is_notificationsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notificationsync_working"].ToString())),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the customers from the database
        /// </summary>
        /// <returns>[Error] returns 'null'</returns>
        public List<Customer> GetCustomers() {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM customer";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                List<Customer> customers = [];
                while (dataReader.Read()) {
                    customers.Add(new Customer {
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        user_name = dataReader["user_name"].ToString(),
                        person_name = dataReader["person_name"].ToString(),
                        email = dataReader["email"].ToString(),
                        password = dataReader["password"].ToString(),
                        role = (Customer.Role)Convert.ToInt32(dataReader["role"].ToString()),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                        product_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["product_sync_status"].ToString())),
                        order_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["order_sync_status"].ToString())),
                        xml_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_sync_status"].ToString())),
                        invoice_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["invoice_sync_status"].ToString())),
                        notification_sync_status = Convert.ToBoolean(Convert.ToInt32(dataReader["notification_sync_status"].ToString())),
                        product_sync_timer = Convert.ToInt32(dataReader["product_sync_timer"].ToString()),
                        order_sync_timer = Convert.ToInt32(dataReader["order_sync_timer"].ToString()),
                        xml_sync_timer = Convert.ToInt32(dataReader["xml_sync_timer"].ToString()),
                        invoice_sync_timer = Convert.ToInt32(dataReader["invoice_sync_timer"].ToString()),
                        notification_sync_timer = Convert.ToInt32(dataReader["notification_sync_timer"].ToString()),
                        last_product_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_product_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_product_sync_date"].ToString()) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_order_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_order_sync_date"].ToString()) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_xml_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_xml_sync_date"].ToString()) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_invoice_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_invoice_sync_date"].ToString()) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace(dataReader["last_notification_sync_date"].ToString()) ? Convert.ToDateTime(dataReader["last_notification_sync_date"].ToString()) : null,
                        is_productsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_productsync_working"].ToString())),
                        is_ordersync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_ordersync_working"].ToString())),
                        is_xmlsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xmlsync_working"].ToString())),
                        is_invoicesync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_invoicesync_working"].ToString())),
                        is_notificationsync_working = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notificationsync_working"].ToString())),
                    });
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return customers;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Saves the customer to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_customer">Customer</param>
        /// <param name="_with_parameters">With working parameters</param>
        /// <returns>[Error] returns 'null'</returns>
        public Customer? SaveCustomer(int _customer_id, Customer _customer, bool _with_working_parameters = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                object val; int inserted_id;
                string _query = "START TRANSACTION;" +
                    "UPDATE customer SET customer_id=LAST_INSERT_ID(@customer_id),user_name=@user_name,person_name=@person_name,email=@email,role=@role,status=@status,product_sync_status=@product_sync_status,order_sync_status=@order_sync_status,xml_sync_status=@xml_sync_status,invoice_sync_status=@invoice_sync_status,notification_sync_status=@notification_sync_status" +
                    ",product_sync_timer=@product_sync_timer,order_sync_timer=@order_sync_timer,xml_sync_timer=@xml_sync_timer,invoice_sync_timer=@invoice_sync_timer,notification_sync_timer=@notification_sync_timer" +
                    (_with_working_parameters ? ",password=@password,is_productsync_working=@is_productsync_working,is_ordersync_working=@is_ordersync_working,is_xmlsync_working=@is_xmlsync_working,is_invoicesync_working=@is_invoicesync_working,is_notificationsync_working=@is_notificationsync_working" : "") + " WHERE customer_id=@customer_id;" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("user_name", _customer.user_name));
                cmd.Parameters.Add(new MySqlParameter("person_name", _customer.person_name));
                cmd.Parameters.Add(new MySqlParameter("email", _customer.email));
                cmd.Parameters.Add(new MySqlParameter("role", (int)_customer.role));
                cmd.Parameters.Add(new MySqlParameter("status", _customer.status));
                cmd.Parameters.Add(new MySqlParameter("product_sync_status", _customer.product_sync_status));
                cmd.Parameters.Add(new MySqlParameter("order_sync_status", _customer.order_sync_status));
                cmd.Parameters.Add(new MySqlParameter("xml_sync_status", _customer.xml_sync_status));
                cmd.Parameters.Add(new MySqlParameter("invoice_sync_status", _customer.invoice_sync_status));
                cmd.Parameters.Add(new MySqlParameter("notification_sync_status", _customer.notification_sync_status));
                cmd.Parameters.Add(new MySqlParameter("product_sync_timer", _customer.product_sync_timer));
                cmd.Parameters.Add(new MySqlParameter("order_sync_timer", _customer.order_sync_timer));
                cmd.Parameters.Add(new MySqlParameter("xml_sync_timer", _customer.xml_sync_timer));
                cmd.Parameters.Add(new MySqlParameter("invoice_sync_timer", _customer.invoice_sync_timer));
                cmd.Parameters.Add(new MySqlParameter("notification_sync_timer", _customer.notification_sync_timer));
                if (_with_working_parameters) {
                    cmd.Parameters.Add(new MySqlParameter("is_productsync_working", _customer.is_productsync_working));
                    cmd.Parameters.Add(new MySqlParameter("is_ordersync_working", _customer.is_ordersync_working));
                    cmd.Parameters.Add(new MySqlParameter("is_xmlsync_working", _customer.is_xmlsync_working));
                    cmd.Parameters.Add(new MySqlParameter("is_invoicesync_working", _customer.is_invoicesync_working));
                    cmd.Parameters.Add(new MySqlParameter("is_notificationsync_working", _customer.is_notificationsync_working));
                    cmd.Parameters.Add(new MySqlParameter("password", _customer.password));
                }
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val != null) {
                    if (int.TryParse(val.ToString(), out inserted_id)) {
                        return GetCustomer(inserted_id);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Creates a new customer in the database
        /// </summary>
        /// <param name="_new_customer">New Customer</param>
        /// <returns></returns>
        public Customer? CreateCustomer(Customer _new_customer) {
            return null;
        }
        #endregion

        #region Admin
        /// <summary>
        /// Gets the admin from the database
        /// </summary>
        /// <param name="_name">Admin Username</param>
        /// <param name="_password">Admin Password</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Admin? GetAdmin(string _name, string _password) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                //string _query = "SELECT * FROM admin WHERE name=@name AND password=@password AND id=@id";
                string _query = "SELECT * FROM admin WHERE name=@name AND password=@password";
                Admin? a = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                //cmd.Parameters.Add( new MySqlParameter( "id", _admin_id ) );
                cmd.Parameters.Add(new MySqlParameter("name", _name));
                cmd.Parameters.Add(new MySqlParameter("password", _password));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    a = new Admin {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        name = dataReader["name"].ToString(),
                        password = dataReader["password"].ToString(),
                        type = Convert.ToInt32(dataReader["type"].ToString())
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return a;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Log
        /// <summary>
        /// Logs the message to the server
        /// </summary>
        /// <param name="_thread_id">Thread ID</param>
        /// <param name="_title">Title</param>
        /// <param name="_message">Message</param>
        /// <param name="_worker">Worker</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool LogToServer(string? _thread_id, string _title, string _message, int _customer_id, string _worker = "general") {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string query = "INSERT INTO log (thread_id,title,message,worker,customer_id) VALUES (@thread_id,@title,@message,@worker,@customer_id);";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.Add(new MySqlParameter() { ParameterName = "thread_id", Value = _thread_id });
                cmd.Parameters.Add(new MySqlParameter() { ParameterName = "title", Value = _title });
                cmd.Parameters.Add(new MySqlParameter() { ParameterName = "message", Value = _message });
                cmd.Parameters.Add(new MySqlParameter() { ParameterName = "worker", Value = _worker });
                cmd.Parameters.Add(new MySqlParameter() { ParameterName = "customer_id", Value = _customer_id });
                int value = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                PrintConsole("log|" + _title + ":" + _message, ConsoleColor.Yellow);
                return true;
            }
            catch (Exception ex) {
                PrintConsole("LOG ERROR GG - " + ex.ToString(), ConsoleColor.Red);
                return false;
            }
        }

        /// <summary>
        /// Gets the last logs from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_items_per_page">Items per page</param>
        /// <param name="_current_page_index">Current page index</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Log> GetLastLogs(int _customer_id, int _items_per_page = 20, int _current_page_index = 0) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id ORDER BY id DESC LIMIT @start,@end";
                List<Log> list = new List<Log>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _items_per_page * _current_page_index));
                cmd.Parameters.Add(new MySqlParameter("end", _items_per_page));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Log l = new Log {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        worker = dataReader["worker"]?.ToString() ?? string.Empty,
                        title = dataReader["title"].ToString() ?? string.Empty,
                        message = dataReader["message"]?.ToString() ?? string.Empty,
                        thread_id = dataReader["thread_id"]?.ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(l);
                }
                dataReader.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the last logs from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Filters</param>
        /// <param name="_items_per_page">Items per page</param>
        /// <param name="_current_page_index">Current page index</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Log> GetLastLogs(int _customer_id, Dictionary<string, string?> _filters, int _items_per_page = 20, int _current_page_index = 0) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id";
                string? filtered_worker = _filters.ContainsKey("worker") ? _filters["worker"] : null;
                string? filtered_title = _filters.ContainsKey("title") ? _filters["title"] : null;
                string? filtered_message = _filters.ContainsKey("message") ? _filters["message"] : null;
                string? filtered_date = _filters.ContainsKey("date") ? _filters["date"] : null;
                List<Log> list = [];
                if (filtered_worker != null || filtered_title != null || filtered_message != null || filtered_date != null) {
                    _query += !string.IsNullOrWhiteSpace(filtered_worker) && filtered_worker != "0" ? " AND worker='" + filtered_worker + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace(filtered_title) && filtered_title != "0" ? " AND title='" + filtered_title + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace(filtered_message) && filtered_message != "0" ? " AND message LIKE '%" + filtered_message + "%'" : string.Empty;
                    //_query += !string.IsNullOrWhiteSpace( filtered_date ) && filtered_worker != "0" ? " AND update_date='" + filtered_date + "'" : string.Empty; TODO: Date filter in GetLastLogs
                    _query += " ORDER BY id DESC LIMIT @start,@end";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("start", _items_per_page * _current_page_index));
                    cmd.Parameters.Add(new MySqlParameter("end", _items_per_page));
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {
                        Log l = new() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            worker = dataReader["worker"]?.ToString() ?? string.Empty,
                            title = dataReader["title"].ToString() ?? string.Empty,
                            message = dataReader["message"]?.ToString() ?? string.Empty,
                            thread_id = dataReader["thread_id"]?.ToString(),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        };
                        list.Add(l);
                    }
                    dataReader.Close();
                }
                else {
                    list = GetLastLogs(_customer_id, _items_per_page, _current_page_index);
                }
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the last logs from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Log> GetLastLogs(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                if (_filters.Sort != null)
                    _query += " ORDER BY " + _filters.Sort.Field + " " + _filters.Sort.Direction + " LIMIT @start,@end;";
                else {
                    _filters.Sort = new Sort() { Field = "update_date", Direction = "DESC" };
                    _query += " ORDER BY update_date DESC LIMIT @start,@end;";
                }
                List<Log> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _filters.Pager.ItemsPerPage * _filters.Pager.CurrentPageIndex));
                cmd.Parameters.Add(new MySqlParameter("end", _filters.Pager.ItemsPerPage));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Log l = new() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        worker = dataReader["worker"]?.ToString() ?? string.Empty,
                        title = dataReader["title"].ToString() ?? string.Empty,
                        message = dataReader["message"]?.ToString() ?? string.Empty,
                        thread_id = dataReader["thread_id"]?.ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(l);
                }
                dataReader.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets log count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetLogsCount(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM log WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets log count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Filters</param>
        /// <param name="_items_per_page">Items Per Page</param>
        /// <param name="_current_page_index">Current Page Index</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetLogsCount(int _customer_id, Dictionary<string, string?> _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM log WHERE customer_id=@customer_id";
                string? filtered_worker = _filters.ContainsKey("worker") ? _filters["worker"] : null;
                string? filtered_title = _filters.ContainsKey("title") ? _filters["title"] : null;
                string? filtered_message = _filters.ContainsKey("message") ? _filters["message"] : null;
                string? filtered_date = _filters.ContainsKey("date") ? _filters["date"] : null;
                int total_count = 0;
                if (filtered_worker != null || filtered_title != null || filtered_message != null || filtered_date != null) {
                    _query += !string.IsNullOrWhiteSpace(filtered_worker) && filtered_worker != "0" ? " AND worker='" + filtered_worker + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace(filtered_title) && filtered_title != "0" ? " AND title='" + filtered_title + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace(filtered_message) && filtered_message != "0" ? " AND message LIKE '%" + filtered_message + "%'" : string.Empty;
                    //_query += !string.IsNullOrWhiteSpace( filtered_date ) && filtered_worker != "0" ? " AND update_date='" + filtered_date + "'" : string.Empty; TODO: Date filter in GetLastLogs
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    int.TryParse(cmd.ExecuteScalar().ToString(), out total_count);
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets log count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetLogsCount(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };
                string _query = "SELECT COUNT(*) FROM log WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                int total_count = 0;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                int.TryParse(cmd.ExecuteScalar().ToString(), out total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Loads the settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsMerchanter LoadSettings(int _customer_id) {
            try {
                DbSettings = this.GetSettings(_customer_id);
                Helper.global = new SettingsMerchanter(_customer_id);
                var customer = GetCustomer(_customer_id);
                if (customer != null) { Helper.global.customer = customer; }
                else { PrintConsole("Customer not found!", ConsoleColor.Red); return null; }

                #region Core Settings
                Helper.global.platforms = LoadPlatforms();
                Helper.global.works = LoadWorks();
                Helper.global.integrations = LoadIntegrations(_customer_id);
                #endregion

                #region Customer Settings
                Helper.global.settings = GetCustomerSettings(_customer_id);
                Helper.global.product = GetProductSettings(_customer_id);
                Helper.global.invoice = GetInvoiceSettings(_customer_id);
                Helper.global.order = GetOrderSettings(_customer_id);
                Helper.global.shipment = GetShipmentSettings(_customer_id);
                Helper.global.order_statuses = LoadOrderStatuses(_customer_id);
                Helper.global.payment_methods = LoadPaymentMethods(_customer_id);
                Helper.global.shipment_methods = LoadShipmentMethods(_customer_id);
                Helper.global.sync_mappings = GetCustomerSyncMappings(_customer_id);
                #endregion

                #region Integration Settings
                //TODO: Could check if integration exists
                Helper.global.entegra = GetEntegraSettings(_customer_id);
                Helper.global.netsis = GetNetsisSettings(_customer_id);
                Helper.global.magento = GetMagentoSettings(_customer_id);
                Helper.global.n11 = GetN11Settings(_customer_id);
                Helper.global.hb = GetHBSettings(_customer_id);
                Helper.global.ty = GetTYSettings(_customer_id);
                Helper.global.ank_erp = GetAnkERPSettings(_customer_id);
                Helper.global.ideasoft = GetIdeasoftSettings(_customer_id);
                Helper.global.google = GetGoogleSettings(_customer_id);
                #endregion

                Helper.global.erp_invoice_ftp_username = DbSettings.Where(x => x.name == "erp_invoice_ftp_username").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.erp_invoice_ftp_password = DbSettings.Where(x => x.name == "erp_invoice_ftp_password").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.erp_invoice_ftp_url = DbSettings.Where(x => x.name == "erp_invoice_ftp_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_bayikodu = DbSettings.Where(x => x.name == "xml_bogazici_bayikodu").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_email = DbSettings.Where(x => x.name == "xml_bogazici_email").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_sifre = DbSettings.Where(x => x.name == "xml_bogazici_sifre").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_fsp_url = DbSettings.Where(x => x.name == "xml_fsp_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_koyuncu_url = DbSettings.Where(x => x.name == "xml_koyuncu_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_oksid_url = DbSettings.Where(x => x.name == "xml_oksid_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_penta_base_url = DbSettings.Where(x => x.name == "xml_penta_base_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_penta_customerid = DbSettings.Where(x => x.name == "xml_penta_customerid").FirstOrDefault()?.value ?? string.Empty;


                #region Decyrption
                if (Helper.global.shipment != null && !string.IsNullOrWhiteSpace(Helper.global.shipment.yurtici_kargo_password))
                    Helper.global.shipment.yurtici_kargo_password = DBSetting.Decrypt(Helper.global.shipment.yurtici_kargo_password);
                if (Helper.global.shipment != null && !string.IsNullOrWhiteSpace(Helper.global.shipment.mng_kargo_password))
                    Helper.global.shipment.mng_kargo_password = DBSetting.Decrypt(Helper.global.shipment.mng_kargo_password);
                if (Helper.global.shipment != null && !string.IsNullOrWhiteSpace(Helper.global.shipment.mng_kargo_client_secret))
                    Helper.global.shipment.mng_kargo_client_secret = DBSetting.Decrypt(Helper.global.shipment.mng_kargo_client_secret);
                if (Helper.global.magento != null && !string.IsNullOrWhiteSpace(Helper.global.magento.token))
                    Helper.global.magento.token = DBSetting.Decrypt(Helper.global.magento.token);
                if (Helper.global.entegra != null && !string.IsNullOrWhiteSpace(Helper.global.entegra.api_password))
                    Helper.global.entegra.api_password = DBSetting.Decrypt(Helper.global.entegra.api_password);
                if (Helper.global.netsis != null && !string.IsNullOrWhiteSpace(Helper.global.netsis.netopenx_password))
                    Helper.global.netsis.netopenx_password = DBSetting.Decrypt(Helper.global.netsis.netopenx_password);
                if (Helper.global.netsis != null && !string.IsNullOrWhiteSpace(Helper.global.netsis.dbpassword))
                    Helper.global.netsis.dbpassword = DBSetting.Decrypt(Helper.global.netsis.dbpassword);
                if (Helper.global.invoice != null && !string.IsNullOrWhiteSpace(Helper.global.invoice.erp_invoice_ftp_password))
                    Helper.global.invoice.erp_invoice_ftp_password = DBSetting.Decrypt(Helper.global.invoice.erp_invoice_ftp_password);
                if (Helper.global.n11 != null && !string.IsNullOrWhiteSpace(Helper.global.n11.appsecret))
                    Helper.global.n11.appsecret = DBSetting.Decrypt(Helper.global.n11.appsecret);
                if (Helper.global.hb != null && !string.IsNullOrWhiteSpace(Helper.global.hb.token))
                    Helper.global.hb.token = DBSetting.Decrypt(Helper.global.hb.token);
                if (Helper.global.hb != null && !string.IsNullOrWhiteSpace(Helper.global.hb.password))
                    Helper.global.hb.password = DBSetting.Decrypt(Helper.global.hb.password);
                if (Helper.global.ty != null && !string.IsNullOrWhiteSpace(Helper.global.ty.api_secret))
                    Helper.global.ty.api_secret = DBSetting.Decrypt(Helper.global.ty.api_secret);
                if (Helper.global.ank_erp != null && !string.IsNullOrWhiteSpace(Helper.global.ank_erp.password))
                    Helper.global.ank_erp.password = DBSetting.Decrypt(Helper.global.ank_erp.password);
                if (Helper.global.ideasoft != null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.client_secret))
                    Helper.global.ideasoft.client_secret = DBSetting.Decrypt(Helper.global.ideasoft.client_secret);
                if (Helper.global.ideasoft != null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.refresh_token))
                    Helper.global.ideasoft.refresh_token = DBSetting.Decrypt(Helper.global.ideasoft.refresh_token);
                if (Helper.global.ideasoft != null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.access_token))
                    Helper.global.ideasoft.access_token = DBSetting.Decrypt(Helper.global.ideasoft.access_token);
                if (Helper.global.google != null && !string.IsNullOrWhiteSpace(Helper.global.google.oauth2_clientsecret))
                    Helper.global.google.oauth2_clientsecret = DBSetting.Decrypt(Helper.global.google.oauth2_clientsecret);
                #endregion

                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local"))) {
                    PrintConsole(Helper.global.settings.company_name + " local enabled!!!", ConsoleColor.Yellow);
                    //Console.Beep();
                    if (_customer_id == 1) {
                        if (Helper.global?.netsis != null && Helper.global?.entegra != null) {
                            Helper.global.netsis.rest_url = "http://85.106.8.239:7070/";
                            Helper.global.entegra.api_url = "http://85.106.8.239:5421/";
                        }
                    }
                }

                return Helper.global;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                Helper.global = null;
                return null;
            }
        }

        public List<ActiveIntegration>? LoadActiveIntegrations(int _customer_id) {
            try {
                var integrations = LoadIntegrations(_customer_id);
                var platforms = LoadPlatforms();
                var works = LoadWorks();
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM active_integrations WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<ActiveIntegration> list = [];
                        while (dataReader.Read()) {
                            list.Add(new ActiveIntegration() {
                                customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                                work = works.Where(x => x.id == Convert.ToInt32(dataReader["WID"].ToString())).FirstOrDefault(),
                                platform = platforms.Where(x => x.id == Convert.ToInt32(dataReader["PID"].ToString())).FirstOrDefault(),
                                integration = integrations.Where(x => x.id == Convert.ToInt32(dataReader["IID"].ToString())).FirstOrDefault(),
                                platform_name = dataReader["platform_name"].ToString(),
                                work_name = dataReader["work_name"].ToString(),
                                platform_work_type = dataReader["platform_work_type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                                available_platform_types = dataReader["available_platform_types"] is string availableTypesStr ? [.. availableTypesStr.Split(',').Select(type => Enum.TryParse(type, out Merchanter.Classes.Platform.PlatformType platformType) ? platformType : default)] : [],
                                platform_status = Convert.ToBoolean(Convert.ToInt32(dataReader["platform_status"].ToString())),
                                work_type = dataReader["work_type"] is string workType1Str && Enum.TryParse(workType1Str, out Merchanter.Classes.Work.WorkType workType1) ? workType1 : default,
                                work_direction = dataReader["work_direction"] is string workDirectionStr && Enum.TryParse(workDirectionStr, out Merchanter.Classes.Work.WorkDirection workDirection) ? workDirection : default,
                                work_status = Convert.ToBoolean(Convert.ToInt32(dataReader["work_status"].ToString())),
                                version = dataReader["version"].ToString(),
                                integration_status = Convert.ToBoolean(Convert.ToInt32(dataReader["integration_status"].ToString())),
                            });
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open) this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets DB settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<DBSetting> GetSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM db_settings WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<DBSetting> list = new List<DBSetting>();
                        while (dataReader.Read()) {
                            list.Add(new DBSetting() {
                                id = Convert.ToInt32(dataReader["id"].ToString()),
                                customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                                name = dataReader["name"].ToString(),
                                value = dataReader["value"].ToString(),
                                group_name = dataReader["group_name"].ToString(),
                                description = dataReader["description"].ToString(),
                                update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                            });
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets setting value from the database
        /// </summary>
        /// <param name="_setting">Setting</param>
        /// <param name="_filter">Filter</param>
        /// <returns>[No data] returns 'null'</returns>
        public static string? GetSettingValue(string _setting, string _filter) {
            var temp = _setting.Split('|');
            foreach (var item in temp) {
                if (item.Split('=')[0] == _filter)
                    return item.Split('=')[1];
            }
            return temp[0].Split('=')[1];
        }

        #region Save Functions
        /// <summary>
        /// Saves the setting to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_name">Setting Name</param>
        /// <param name="_value">Setting Value</param>
        /// <returns>[Error] returns 'false'</returns>
        public bool SaveSetting(int _customer_id, string _name, string _value) {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "UPDATE db_settings SET value=@value, update_date=@update_date WHERE customer_id=@customer_id AND name=@name";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("name", _name));
                        cmd.Parameters.Add(new MySqlParameter("value", _value));
                        cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));

                        int val = cmd.ExecuteNonQuery();

                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();

                        return val > 0;
                    }
                return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the customer settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings General</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveCustomerSettings(int _customer_id, SettingsGeneral _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings SET company_name=@company_name,rate_TL=@rate_TL,rate_USD=@rate_USD,rate_EUR=@rate_EUR WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("company_name", _settings.company_name));
                cmd.Parameters.Add(new MySqlParameter("rate_TL", _settings.rate_TL));
                cmd.Parameters.Add(new MySqlParameter("rate_USD", _settings.rate_USD));
                cmd.Parameters.Add(new MySqlParameter("rate_EUR", _settings.rate_EUR));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the customer settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Invoice</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveInvoiceSettings(int _customer_id, SettingsInvoice _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_invoice SET daysto_invoicesync=@daysto_invoicesync,erp_invoice_ftp_username=@erp_invoice_ftp_username,erp_invoice_ftp_password=@erp_invoice_ftp_password,erp_invoice_ftp_url=@erp_invoice_ftp_url WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("daysto_invoicesync", _settings.daysto_invoicesync));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_username", _settings.erp_invoice_ftp_username));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_password", !string.IsNullOrWhiteSpace(_settings.erp_invoice_ftp_password) ? DBSetting.Encrypt(_settings.erp_invoice_ftp_password) : null));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_url", _settings.erp_invoice_ftp_url));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the product settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Product</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveProductSettings(int _customer_id, SettingsProduct _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_product SET default_brand=@default_brand,customer_root_category_id=@customer_root_category_id,product_list_filter_source_products=@product_list_filter_source_products,is_barcode_required=@is_barcode_required,xml_qty_addictive_enable=@xml_qty_addictive_enable WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("default_brand", _settings.default_brand));
                cmd.Parameters.Add(new MySqlParameter("customer_root_category_id", _settings.customer_root_category_id));
                cmd.Parameters.Add(new MySqlParameter("product_list_filter_source_products", _settings.product_list_filter_source_products));
                cmd.Parameters.Add(new MySqlParameter("is_barcode_required", _settings.is_barcode_required));
                cmd.Parameters.Add(new MySqlParameter("xml_qty_addictive_enable", _settings.xml_qty_addictive_enable));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the entegra settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Entegra</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveEntegraSettings(int _customer_id, SettingsEntegra _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_entegra SET api_url=@api_url,api_username=@api_username,api_password=@api_password WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("api_url", _settings.api_url));
                cmd.Parameters.Add(new MySqlParameter("api_username", _settings.api_username));
                cmd.Parameters.Add(new MySqlParameter("api_password", !string.IsNullOrWhiteSpace(_settings.api_password) ? DBSetting.Encrypt(_settings.api_password) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the shipment settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Shipment</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveShipmentSettings(int _customer_id, SettingsShipment _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_shipment SET yurtici_kargo=@yurtici_kargo,mng_kargo=@mng_kargo,aras_kargo=@aras_kargo,yurtici_kargo_user_name=@yurtici_kargo_user_name,yurtici_kargo_password=@yurtici_kargo_password,mng_kargo_customer_number=@mng_kargo_customer_number,mng_kargo_password=@mng_kargo_password,mng_kargo_client_id=@mng_kargo_client_id,mng_kargo_client_secret=@mng_kargo_client_secret WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo", _settings.yurtici_kargo));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo", _settings.mng_kargo));
                cmd.Parameters.Add(new MySqlParameter("aras_kargo", _settings.aras_kargo));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo_user_name", _settings.yurtici_kargo_user_name));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo_password", !string.IsNullOrWhiteSpace(_settings.yurtici_kargo_password) ? DBSetting.Encrypt(_settings.yurtici_kargo_password) : null));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_customer_number", _settings.mng_kargo_customer_number));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_password", !string.IsNullOrWhiteSpace(_settings.mng_kargo_password) ? DBSetting.Encrypt(_settings.mng_kargo_password) : null));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_client_id", _settings.mng_kargo_client_id));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_client_secret", !string.IsNullOrWhiteSpace(_settings.mng_kargo_client_secret) ? DBSetting.Encrypt(_settings.mng_kargo_client_secret) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the N11 settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings N11</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveN11Settings(int _customer_id, SettingsN11 _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_n11 SET appkey=@appkey,appsecret=@appsecret WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("appkey", _settings.appkey));
                cmd.Parameters.Add(new MySqlParameter("appsecret", !string.IsNullOrWhiteSpace(_settings.appsecret) ? DBSetting.Encrypt(_settings.appsecret) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the HB settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings HB</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveHBSettings(int _customer_id, SettingsHB _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_hb SET merchant_id=@merchant_id,token=@token,user_name=@user_name,password=@password WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("merchant_id", _settings.merchant_id));
                cmd.Parameters.Add(new MySqlParameter("token", !string.IsNullOrWhiteSpace(_settings.token) ? DBSetting.Encrypt(_settings.token) : null));
                cmd.Parameters.Add(new MySqlParameter("user_name", _settings.user_name));
                cmd.Parameters.Add(new MySqlParameter("password", !string.IsNullOrWhiteSpace(_settings.password) ? DBSetting.Encrypt(_settings.password) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the TY settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings TY</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveTYSettings(int _customer_id, SettingsTY _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ty SET seller_id=@seller_id,api_key=@api_key,api_secret=@api_secret WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("seller_id", _settings.seller_id));
                cmd.Parameters.Add(new MySqlParameter("api_key", _settings.api_key));
                cmd.Parameters.Add(new MySqlParameter("api_secret", !string.IsNullOrWhiteSpace(_settings.api_secret) ? DBSetting.Encrypt(_settings.api_secret) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the Ankara ERP settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings ANK_ERP</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveAnkERPSettings(int _customer_id, SettingsAnkaraErp _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ank_erp SET company_code=@company_code,user_name=@user_name,password=@password,work_year=@work_year,url=@url WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("company_code", _settings.company_code));
                cmd.Parameters.Add(new MySqlParameter("user_name", _settings.user_name));
                cmd.Parameters.Add(new MySqlParameter("work_year", _settings.work_year));
                cmd.Parameters.Add(new MySqlParameter("url", _settings.url));
                cmd.Parameters.Add(new MySqlParameter("password", !string.IsNullOrWhiteSpace(_settings.password) ? DBSetting.Encrypt(_settings.password) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the IDEASOFT settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings IDEASOFT</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveIdeasoftSettings(int _customer_id, SettingsIdeasoft _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ideasoft SET store_url=@store_url,client_id=@client_id,client_secret=@client_secret,refresh_token=@refresh_token WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("store_url", _settings.store_url));
                cmd.Parameters.Add(new MySqlParameter("client_id", _settings.client_id));
                cmd.Parameters.Add(new MySqlParameter("client_secret", !string.IsNullOrWhiteSpace(_settings.client_secret) ? DBSetting.Encrypt(_settings.client_secret) : null));
                cmd.Parameters.Add(new MySqlParameter("refresh_token", !string.IsNullOrWhiteSpace(_settings.refresh_token) ? DBSetting.Encrypt(_settings.refresh_token) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the Google settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Google</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveGoogleSettings(int _customer_id, SettingsGoogle _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_google SET email=@email,oauth2_clientid=@oauth2_clientid,oauth2_clientsecret=@oauth2_clientsecret,sender_name=@sender_name,is_enabled=@is_enabled WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("email", _settings.email));
                cmd.Parameters.Add(new MySqlParameter("oauth2_clientid", _settings.oauth2_clientid));
                cmd.Parameters.Add(new MySqlParameter("sender_name", _settings.sender_name));
                cmd.Parameters.Add(new MySqlParameter("is_enabled", _settings.is_enabled));
                cmd.Parameters.Add(new MySqlParameter("oauth2_clientsecret", !string.IsNullOrWhiteSpace(_settings.oauth2_clientsecret) ? DBSetting.Encrypt(_settings.oauth2_clientsecret) : null));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the magento settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Magento</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveMagentoSettings(int _customer_id, SettingsMagento _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_magento SET base_url=@base_url,token=@token,root_category_id=@root_category_id,order_processing_comment=@order_processing_comment,barcode_attribute_code=@barcode_attribute_code,brand_attribute_code=@brand_attribute_code,is_xml_enabled_attribute_code=@is_xml_enabled_attribute_code,xml_sources_attribute_code=@xml_sources_attribute_code,customer_tc_no_attribute_code=@customer_tc_no_attribute_code,customer_firma_ismi_attribute_code=@customer_firma_ismi_attribute_code,customer_firma_vergidairesi_attribute_code=@customer_firma_vergidairesi_attribute_code,customer_firma_vergino_attribute_code=@customer_firma_vergino_attribute_code WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("base_url", _settings.base_url));
                cmd.Parameters.Add(new MySqlParameter("token", !string.IsNullOrWhiteSpace(_settings.token) ? DBSetting.Encrypt(_settings.token) : null));
                cmd.Parameters.Add(new MySqlParameter("root_category_id", _settings.root_category_id));
                cmd.Parameters.Add(new MySqlParameter("order_processing_comment", _settings.order_processing_comment));
                cmd.Parameters.Add(new MySqlParameter("barcode_attribute_code", _settings.barcode_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("brand_attribute_code", _settings.brand_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("is_xml_enabled_attribute_code", _settings.is_xml_enabled_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("xml_sources_attribute_code", _settings.xml_sources_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_tc_no_attribute_code", _settings.customer_tc_no_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_ismi_attribute_code", _settings.customer_firma_ismi_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_vergidairesi_attribute_code", _settings.customer_firma_vergidairesi_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_vergino_attribute_code", _settings.customer_firma_vergino_attribute_code));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the order settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Order</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveOrderSettings(int _customer_id, SettingsOrder _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_order SET daysto_ordersync=@daysto_ordersync,siparis_kargo_sku=@siparis_kargo_sku,siparis_taksitkomisyon_sku=@siparis_taksitkomisyon_sku,is_rewrite_siparis=@is_rewrite_siparis,siparis_kdvdahilmi=@siparis_kdvdahilmi WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("daysto_ordersync", _settings.daysto_ordersync));
                cmd.Parameters.Add(new MySqlParameter("siparis_kargo_sku", _settings.siparis_kargo_sku));
                cmd.Parameters.Add(new MySqlParameter("siparis_taksitkomisyon_sku", _settings.siparis_taksitkomisyon_sku));
                cmd.Parameters.Add(new MySqlParameter("is_rewrite_siparis", _settings.is_rewrite_siparis));
                cmd.Parameters.Add(new MySqlParameter("siparis_kdvdahilmi", _settings.siparis_kdvdahilmi));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the netsis settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Netsis</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveNetsisSettings(int _customer_id, SettingsNetsis _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_netsis SET rest_url=@rest_url,netopenx_user=@netopenx_user,netopenx_password=@netopenx_password,dbname=@dbname,dbuser=@dbuser,dbpassword=@dbpassword,belgeonek_musterisiparisi=@belgeonek_musterisiparisi,siparis_carionek=@siparis_carionek,cari_siparis_grupkodu=@cari_siparis_grupkodu,sipari_caritip=@sipari_caritip,siparis_muhasebekodu=@siparis_muhasebekodu,siparis_subekodu=@siparis_subekodu,siparis_depokodu=@siparis_depokodu,ebelge_dizayn_earsiv=@ebelge_dizayn_earsiv,ebelge_dizayn_efatura=@ebelge_dizayn_efatura,ebelge_klasorismi=@ebelge_klasorismi,efatura_belge_onek=@efatura_belge_onek,earsiv_belge_onek=@earsiv_belge_onek,fatura_cari_gruplari=@fatura_cari_gruplari,siparis_kod2=@siparis_kod2,siparis_cyedek1=@siparis_cyedek1,siparis_ekack15=@siparis_ekack15,siparis_ekack10=@siparis_ekack10,siparis_ekack11=@siparis_ekack11,siparis_ekack4=@siparis_ekack4 WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("rest_url", _settings.rest_url));
                cmd.Parameters.Add(new MySqlParameter("netopenx_user", _settings.netopenx_user));
                cmd.Parameters.Add(new MySqlParameter("netopenx_password", !string.IsNullOrWhiteSpace(_settings.netopenx_password) ? DBSetting.Encrypt(_settings.netopenx_password) : null));
                cmd.Parameters.Add(new MySqlParameter("dbname", _settings.dbname));
                cmd.Parameters.Add(new MySqlParameter("dbuser", _settings.dbuser)); //TODO: Add encrypt
                cmd.Parameters.Add(new MySqlParameter("dbpassword", !string.IsNullOrWhiteSpace(_settings.dbpassword) ? DBSetting.Encrypt(_settings.dbpassword) : null));
                cmd.Parameters.Add(new MySqlParameter("belgeonek_musterisiparisi", _settings.belgeonek_musterisiparisi));
                cmd.Parameters.Add(new MySqlParameter("siparis_carionek", _settings.siparis_carionek));
                cmd.Parameters.Add(new MySqlParameter("cari_siparis_grupkodu", _settings.cari_siparis_grupkodu));
                cmd.Parameters.Add(new MySqlParameter("sipari_caritip", _settings.sipari_caritip));
                cmd.Parameters.Add(new MySqlParameter("siparis_muhasebekodu", _settings.siparis_muhasebekodu));
                cmd.Parameters.Add(new MySqlParameter("siparis_subekodu", _settings.siparis_subekodu));
                cmd.Parameters.Add(new MySqlParameter("siparis_depokodu", _settings.siparis_depokodu));
                cmd.Parameters.Add(new MySqlParameter("ebelge_dizayn_earsiv", _settings.ebelge_dizayn_earsiv));
                cmd.Parameters.Add(new MySqlParameter("ebelge_dizayn_efatura", _settings.ebelge_dizayn_efatura));
                cmd.Parameters.Add(new MySqlParameter("ebelge_klasorismi", _settings.ebelge_klasorismi));
                cmd.Parameters.Add(new MySqlParameter("efatura_belge_onek", _settings.efatura_belge_onek));
                cmd.Parameters.Add(new MySqlParameter("earsiv_belge_onek", _settings.earsiv_belge_onek));
                cmd.Parameters.Add(new MySqlParameter("fatura_cari_gruplari", _settings.fatura_cari_gruplari));
                cmd.Parameters.Add(new MySqlParameter("siparis_kod2", _settings.siparis_kod2));
                cmd.Parameters.Add(new MySqlParameter("siparis_cyedek1", _settings.siparis_cyedek1));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack15", _settings.siparis_ekack15));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack10", _settings.siparis_ekack10));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack11", _settings.siparis_ekack11));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack4", _settings.siparis_ekack4));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves the work sources to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Work Sources</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SaveCustomerIntegrations(int _customer_id, List<Integration> _settings) {
            try {
                int val = 0;
                foreach (Integration item in _settings) {
                    string _query = "UPDATE integrations SET work_id=@work_id,is_active=@is_active WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("work_id", item.work.id));
                    cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Get Functions
        /// <summary>
        /// Gets the settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsGeneral GetCustomerSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings WHERE customer_id=@customer_id";
                SettingsGeneral? cs = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    cs = new SettingsGeneral {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        company_name = dataReader["company_name"].ToString(),
                        rate_TL = Convert.ToDecimal(dataReader["rate_TL"].ToString()),
                        rate_USD = Convert.ToDecimal(dataReader["rate_USD"].ToString()),
                        rate_EUR = Convert.ToDecimal(dataReader["rate_EUR"].ToString())
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return cs;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the invoice settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsInvoice GetInvoiceSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_invoice WHERE customer_id=@customer_id";
                SettingsInvoice? inv_s = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    inv_s = new SettingsInvoice {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        daysto_invoicesync = Convert.ToInt32(dataReader["daysto_invoicesync"].ToString()),
                        erp_invoice_ftp_password = dataReader["erp_invoice_ftp_password"].ToString(),
                        erp_invoice_ftp_url = dataReader["erp_invoice_ftp_url"].ToString(),
                        erp_invoice_ftp_username = dataReader["erp_invoice_ftp_username"].ToString()
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return inv_s;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsProduct GetProductSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_product WHERE customer_id=@customer_id";
                SettingsProduct? ps = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    ps = new SettingsProduct {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        default_brand = dataReader["default_brand"].ToString(),
                        customer_root_category_id = Convert.ToInt32(dataReader["customer_root_category_id"].ToString()),
                        product_list_filter_source_products = Convert.ToBoolean(Convert.ToInt32(dataReader["product_list_filter_source_products"].ToString())),
                        is_barcode_required = Convert.ToBoolean(Convert.ToInt32(dataReader["is_barcode_required"].ToString())),
                        xml_qty_addictive_enable = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_qty_addictive_enable"].ToString()))
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return ps;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the entegra settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsEntegra GetEntegraSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_entegra WHERE customer_id=@customer_id";
                SettingsEntegra? es = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    es = new SettingsEntegra {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        api_url = dataReader["api_url"].ToString(),
                        api_username = dataReader["api_username"].ToString(),
                        api_password = dataReader["api_password"].ToString()
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return es;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the magento settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsMagento GetMagentoSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_magento WHERE customer_id=@customer_id";
                SettingsMagento? ms = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    ms = new SettingsMagento {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        base_url = dataReader["base_url"].ToString(),
                        token = dataReader["token"].ToString(),
                        root_category_id = Convert.ToInt32(dataReader["root_category_id"].ToString()),
                        brand_attribute_code = dataReader["brand_attribute_code"].ToString(),
                        barcode_attribute_code = dataReader["barcode_attribute_code"].ToString(),
                        is_xml_enabled_attribute_code = dataReader["is_xml_enabled_attribute_code"].ToString(),
                        xml_sources_attribute_code = dataReader["xml_sources_attribute_code"].ToString(),
                        customer_tc_no_attribute_code = dataReader["customer_tc_no_attribute_code"].ToString(),
                        customer_firma_ismi_attribute_code = dataReader["customer_firma_ismi_attribute_code"].ToString(),
                        customer_firma_vergidairesi_attribute_code = dataReader["customer_firma_vergidairesi_attribute_code"].ToString(),
                        customer_firma_vergino_attribute_code = dataReader["customer_firma_vergino_attribute_code"].ToString(),
                        order_processing_comment = dataReader["order_processing_comment"].ToString()
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return ms;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the order settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsOrder GetOrderSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_order WHERE customer_id=@customer_id";
                SettingsOrder? os = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    os = new SettingsOrder {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        daysto_ordersync = Convert.ToInt32(dataReader["daysto_ordersync"].ToString()),
                        siparis_kargo_sku = dataReader["siparis_kargo_sku"].ToString(),
                        siparis_taksitkomisyon_sku = dataReader["siparis_taksitkomisyon_sku"].ToString(),
                        is_rewrite_siparis = Convert.ToBoolean(Convert.ToInt32(dataReader["is_rewrite_siparis"].ToString())),
                        siparis_kdvdahilmi = Convert.ToBoolean(Convert.ToInt32(dataReader["siparis_kdvdahilmi"].ToString())),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return os;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the google settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsGoogle GetGoogleSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_google WHERE customer_id=@customer_id";
                SettingsGoogle? gs = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    gs = new SettingsGoogle {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        email = dataReader["email"].ToString(),
                        oauth2_clientid = dataReader["oauth2_clientid"].ToString(),
                        oauth2_clientsecret = dataReader["oauth2_clientsecret"].ToString(),
                        sender_name = dataReader["sender_name"].ToString(),
                        is_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_enabled"].ToString())),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return gs;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the netsis settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsNetsis GetNetsisSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_netsis WHERE customer_id=@customer_id";
                SettingsNetsis? ns = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    ns = new SettingsNetsis {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        netopenx_user = dataReader["netopenx_user"].ToString(),
                        netopenx_password = dataReader["netopenx_password"].ToString(),
                        dbname = dataReader["dbname"].ToString(),
                        dbuser = dataReader["dbuser"].ToString(),
                        dbpassword = dataReader["dbpassword"].ToString(),
                        rest_url = dataReader["rest_url"].ToString(),
                        belgeonek_musterisiparisi = dataReader["belgeonek_musterisiparisi"].ToString(),
                        siparis_carionek = dataReader["siparis_carionek"].ToString(),
                        cari_siparis_grupkodu = dataReader["cari_siparis_grupkodu"].ToString(),
                        sipari_caritip = dataReader["sipari_caritip"].ToString(),
                        siparis_muhasebekodu = dataReader["siparis_muhasebekodu"].ToString(),
                        siparis_subekodu = Convert.ToInt32(dataReader["siparis_subekodu"].ToString()),
                        siparis_depokodu = Convert.ToInt32(dataReader["siparis_depokodu"].ToString()),
                        ebelge_dizayn_earsiv = dataReader["ebelge_dizayn_earsiv"].ToString(),
                        ebelge_dizayn_efatura = dataReader["ebelge_dizayn_efatura"].ToString(),
                        ebelge_klasorismi = dataReader["ebelge_klasorismi"].ToString(),
                        efatura_belge_onek = dataReader["efatura_belge_onek"].ToString(),
                        earsiv_belge_onek = dataReader["earsiv_belge_onek"].ToString(),
                        fatura_cari_gruplari = dataReader["fatura_cari_gruplari"].ToString(),
                        siparis_kod2 = dataReader["siparis_kod2"].ToString(),
                        siparis_cyedek1 = dataReader["siparis_cyedek1"].ToString(),
                        siparis_ekack15 = dataReader["siparis_ekack15"].ToString(),
                        siparis_ekack10 = dataReader["siparis_ekack10"].ToString(),
                        siparis_ekack11 = dataReader["siparis_ekack11"].ToString(),
                        siparis_ekack4 = dataReader["siparis_ekack4"].ToString()
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return ns;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the shipment settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsShipment GetShipmentSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_shipment WHERE customer_id=@customer_id";
                SettingsShipment? ss = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    ss = new SettingsShipment {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        yurtici_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["yurtici_kargo"].ToString())),
                        mng_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["mng_kargo"].ToString())),
                        aras_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["aras_kargo"].ToString())),
                        yurtici_kargo_user_name = dataReader["yurtici_kargo_user_name"].ToString(),
                        yurtici_kargo_password = dataReader["yurtici_kargo_password"].ToString(),
                        mng_kargo_customer_number = dataReader["mng_kargo_customer_number"].ToString(),
                        mng_kargo_password = dataReader["mng_kargo_password"].ToString(),
                        mng_kargo_client_id = dataReader["mng_kargo_client_id"].ToString(),
                        mng_kargo_client_secret = dataReader["mng_kargo_client_secret"].ToString(),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return ss;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the N11 settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsN11 GetN11Settings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_n11 WHERE customer_id=@customer_id";
                SettingsN11? sn11 = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    sn11 = new SettingsN11 {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        appkey = dataReader["appkey"].ToString(),
                        appsecret = dataReader["appsecret"].ToString(),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return sn11;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the HB settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsHB GetHBSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_hb WHERE customer_id=@customer_id";
                SettingsHB? shb = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    shb = new SettingsHB {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        merchant_id = dataReader["merchant_id"].ToString(),
                        token = dataReader["token"].ToString(),
                        user_name = dataReader["user_name"].ToString(),
                        password = dataReader["password"].ToString(),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return shb;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the TY settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsTY GetTYSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_ty WHERE customer_id=@customer_id";
                SettingsTY? sty = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    sty = new SettingsTY {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        seller_id = dataReader["seller_id"].ToString(),
                        api_key = dataReader["api_key"].ToString(),
                        api_secret = dataReader["api_secret"].ToString(),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return sty;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the ANKARA ERP settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsAnkaraErp GetAnkERPSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_ank_erp WHERE customer_id=@customer_id";
                SettingsAnkaraErp? ank = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    ank = new SettingsAnkaraErp {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        company_code = dataReader["company_code"].ToString(),
                        user_name = dataReader["user_name"].ToString(),
                        password = dataReader["password"].ToString(),
                        work_year = dataReader["work_year"].ToString(),
                        url = dataReader["url"].ToString(),
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return ank;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the IDEASOFT settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public SettingsIdeasoft GetIdeasoftSettings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM settings_ideasoft WHERE customer_id=@customer_id";
                SettingsIdeasoft? idea = null;
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    idea = new SettingsIdeasoft {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        store_url = dataReader["store_url"].ToString(),
                        client_id = dataReader["client_id"].ToString(),
                        client_secret = dataReader["client_secret"].ToString(),
                        refresh_token = dataReader["refresh_token"].ToString(),
                        access_token = dataReader["access_token"].ToString(),
                        update_date = !string.IsNullOrWhiteSpace(dataReader["update_date"].ToString()) ? Convert.ToDateTime(dataReader["update_date"].ToString()) : null
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return idea;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Load Functions
        /// <summary>
        /// Gets order statuses from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<OrderStatus> LoadOrderStatuses(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM order_statuses WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<OrderStatus> list = [];
                        while (dataReader.Read()) {
                            list.Add(new OrderStatus(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                 Convert.ToInt32(dataReader["customer_id"].ToString()),
                                 dataReader["status_name"].ToString(),
                                 dataReader["status_code"].ToString(),
                                 dataReader["platform"].ToString(),
                                 dataReader["platform_status_code"].ToString(),
                                 Convert.ToBoolean(Convert.ToInt32(dataReader["sync_status"].ToString())),
                                 Convert.ToBoolean(Convert.ToInt32(dataReader["process_status"].ToString()))
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets payment methods from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<PaymentMethod> LoadPaymentMethods(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM payment_methods WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<PaymentMethod> list = [];
                        while (dataReader.Read()) {
                            list.Add(new PaymentMethod(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                Convert.ToInt32(dataReader["customer_id"].ToString()),
                                dataReader["payment_name"].ToString(),
                                dataReader["payment_code"].ToString(),
                                dataReader["platform"].ToString(),
                                dataReader["platform_payment_code"].ToString()
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets shipment methods from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ShipmentMethod> LoadShipmentMethods(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM shipment_methods WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<ShipmentMethod> list = [];
                        while (dataReader.Read()) {
                            list.Add(new ShipmentMethod(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                Convert.ToInt32(dataReader["customer_id"].ToString()),
                                dataReader["shipment_name"].ToString(),
                                dataReader["shipment_code"].ToString(),
                                dataReader["platform"].ToString(),
                                dataReader["platform_shipment_code"].ToString()
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets customer integrations from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Integration> LoadIntegrations(int _customer_id) {
            try {
                var works = LoadWorks();
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM integrations WHERE customer_id=@customer_id;";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<Integration> list = new List<Integration>();
                        while (dataReader.Read()) {
                            list.Add(new Integration(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                Convert.ToInt32(dataReader["customer_id"].ToString()),
                                Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                                works.FirstOrDefault(x => x.id == Convert.ToInt32(dataReader["work_id"].ToString()))
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets platforms from the database
        /// </summary>
        /// <returns>[Error] returns 'null'</returns>
        public List<Platform> LoadPlatforms() {
            try {
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM platforms;";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<Platform> list = [];
                        while (dataReader.Read()) {
                            list.Add(new Platform(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                dataReader["name"].ToString(),
                                dataReader["work_type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                                dataReader["available_types"] is string availableTypesStr ? [.. availableTypesStr.Split(',').Select(type => Enum.TryParse(type, out Merchanter.Classes.Platform.PlatformType platformType) ? platformType : default)] : [],
                                Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                                Convert.ToDateTime(dataReader["update_date"].ToString()),
                                dataReader["image"].ToString()
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets works from the database
        /// </summary>
        /// <returns>[Error] returns 'null'</returns>
        public List<Work> LoadWorks() {
            try {
                var platforms = LoadPlatforms();
                if (this.state != System.Data.ConnectionState.Open)
                    if (this.OpenConnection()) {
                        string _query = "SELECT * FROM works;";
                        MySqlCommand cmd = new MySqlCommand(_query, Connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<Work> list = [];
                        while (dataReader.Read()) {
                            list.Add(new Work(
                                Convert.ToInt32(dataReader["id"].ToString()),
                                platforms.FirstOrDefault(x => x.id == Convert.ToInt32(dataReader["platform_id"].ToString())),
                                dataReader["name"].ToString(),
                                dataReader["type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                                dataReader["direction"] is string workDirectionStr && Enum.TryParse(workDirectionStr, out Merchanter.Classes.Work.WorkDirection workDirection) ? workDirection : default,
                                Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                                dataReader["version"].ToString()
                            ));
                        }
                        dataReader.Close();
                        if (state == System.Data.ConnectionState.Open)
                            this.CloseConnection();
                        return list;
                    }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion
        #endregion

        #region Notification
        /// <summary>
        /// Inserts the notifications to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_notifications">Notifications</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertNotifications(int _customer_id, List<Notification> _notifications) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                foreach (Notification item in _notifications) {
                    string _query = "INSERT INTO notifications (customer_id,type,order_label,product_sku,xproduct_barcode,invoice_no,notification_content) VALUES (@customer_id,@type,@order_label,@product_sku,@xproduct_barcode,@invoice_no,@notification_content)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("type", item.type));
                    cmd.Parameters.Add(new MySqlParameter("is_notification_sent", item.is_notification_sent));
                    cmd.Parameters.Add(new MySqlParameter("notification_content", item.notification_content));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("product_sku", item.product_sku));
                    cmd.Parameters.Add(new MySqlParameter("xproduct_barcode", item.xproduct_barcode));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", item.invoice_no));
                    val += cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the notifications to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_notifications">Notifications</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateNotifications(int _customer_id, List<Notification> _notifications) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                foreach (Notification item in _notifications) {
                    string _query = "UPDATE notifications SET notification_content=@notification_content,is_notification_sent=@is_notification_sent,notification_date=@notification_date WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("notification_content", item.notification_content));
                    cmd.Parameters.Add(new MySqlParameter("is_notification_sent", item.is_notification_sent));
                    cmd.Parameters.Add(new MySqlParameter("notification_date", DateTime.Now));
                    val += cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the notifications from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_is_notification_sent">Is Notification Sent</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Notification> GetNotifications(int _customer_id, bool? _is_notification_sent) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = (!_is_notification_sent.HasValue ? "SELECT * FROM notifications WHERE customer_id=@customer_id" :
                    "SELECT * FROM notifications WHERE is_notification_sent = " + (_is_notification_sent.Value ? "1" : "0") + " AND customer_id=@customer_id");
                List<Notification> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Notification n = new Notification {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        type = (Notification.NotificationTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        order_label = dataReader["order_label"].ToString(),
                        product_sku = dataReader["product_sku"].ToString(),
                        xproduct_barcode = dataReader["xproduct_barcode"].ToString(),
                        invoice_no = dataReader["invoice_no"].ToString(),
                        notification_content = dataReader["notification_content"].ToString(),
                        is_notification_sent = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notification_sent"].ToString())),
                        create_date = Convert.ToDateTime(dataReader["create_date"].ToString()),
                        notification_date = !string.IsNullOrWhiteSpace(dataReader["notification_date"].ToString()) ? Convert.ToDateTime(dataReader["notification_date"].ToString()) : null,
                    };
                    list.Add(n);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the notifications from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_is_notification_sent">Is Notification Sent</param>
        /// <param name="_items_per_page">Items Per Page</param>
        /// <param name="_current_page_index">Current Page Index</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Notification> GetNotifications(int _customer_id, bool? _is_notification_sent, int _items_per_page = 20, int _current_page_index = 0) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = (!_is_notification_sent.HasValue ? "SELECT * FROM notifications WHERE customer_id=@customer_id ORDER BY id DESC LIMIT @start,@end" :
                    "SELECT * FROM notifications WHERE is_notification_sent = " + (_is_notification_sent.Value ? "1" : "0") + " AND customer_id=@customer_id ORDER BY id DESC LIMIT @start,@end");
                List<Notification> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _items_per_page * (_current_page_index)));
                cmd.Parameters.Add(new MySqlParameter("end", _items_per_page));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Notification n = new Notification {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        type = (Notification.NotificationTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        order_label = dataReader["order_label"].ToString(),
                        product_sku = dataReader["product_sku"].ToString(),
                        xproduct_barcode = dataReader["xproduct_barcode"].ToString(),
                        invoice_no = dataReader["invoice_no"].ToString(),
                        notification_content = dataReader["notification_content"].ToString(),
                        is_notification_sent = Convert.ToBoolean(Convert.ToInt32(dataReader["is_notification_sent"].ToString())),
                        create_date = Convert.ToDateTime(dataReader["create_date"].ToString()),
                        notification_date = !string.IsNullOrWhiteSpace(dataReader["notification_date"].ToString()) ? Convert.ToDateTime(dataReader["notification_date"].ToString()) : null,
                    };
                    list.Add(n);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Mapping
        /// <summary>
        /// Gets the sync mappings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<SyncMapping> GetCustomerSyncMappings(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM sync_mapping WHERE customer_id=@customer_id";
                List<SyncMapping> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    SyncMapping sm = new SyncMapping {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        attribute_source = dataReader["attribute_source"].ToString(),
                        attribute_type = dataReader["attribute_type"].ToString(),
                        variable_type = dataReader["variable_type"].ToString(),
                        product_attribute = dataReader["product_attribute"].ToString(),
                        work_source = dataReader["work_source"].ToString(),
                        source_attribute = dataReader["source_attribute"].ToString(),
                        regex = dataReader["regex"].ToString(),
                        is_active = dataReader["is_active"] != null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(sm);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Catalog

        #region Product
        /// <summary>
        /// Gets product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Product? GetProduct(int _customer_id, int _id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM products WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Product? p = null;
                if (dataReader.Read()) {
                    p = new Product {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                        name = dataReader["name"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        price = decimal.Parse(dataReader["price"].ToString()),
                        special_price = decimal.Parse(dataReader["special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString()))
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (p != null && !string.IsNullOrWhiteSpace(p.sku)) {
                    var ext = GetProductExt(_customer_id, p.sku);
                    if (ext != null) {
                        p.extension = ext;
                    }
                    else {
                        OnError("GetProduct: " + p.sku + " - Product Extension Not Found");
                        return null;
                    }

                    var pas = GetProductAttributes(_customer_id, p.id);
                    if (pas != null) {
                        p.attributes = [.. pas];
                    }
                    else {
                        OnError("GetProduct: " + p.sku + " - Product Attributes Not Found");
                        return null;
                    }

                    var ss = GetProductSources(_customer_id, p.sku);
                    if (ss != null && ss.Count > 0) {
                        p.sources = [.. ss];
                    }
                    else {
                        OnError("GetProduct: " + p.sku + " - Product Source Not Found");
                        return null;
                    }

                    var images = GetProductImages(_customer_id, p.sku);
                    if (images != null) {
                        p.images = [.. images];
                    }
                    else {
                        OnError("GetProduct: " + p.sku + " - Product Images Not Found");
                        return null;
                    }
                }

                return p;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Product? GetProductBySku(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM products WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Product? p = null;
                if (dataReader.Read()) {
                    p = new Product {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                        name = dataReader["name"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        price = decimal.Parse(dataReader["price"].ToString()),
                        special_price = decimal.Parse(dataReader["special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString()))
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (p != null && !string.IsNullOrWhiteSpace(p.sku)) {
                    var ext = GetProductExt(_customer_id, p.sku);
                    if (ext != null) {
                        p.extension = ext;
                    }
                    else {
                        OnError("GetProductBySku: " + p.sku + " - Product Extension Not Found");
                        return null;
                    }

                    var pas = GetProductAttributes(_customer_id, p.id);
                    if (pas != null) {
                        p.attributes = [.. pas];
                    }
                    else {
                        OnError("GetProduct: " + p.sku + " - Product Attributes Not Found");
                        return null;
                    }

                    var ss = GetProductSources(_customer_id, p.sku);
                    if (ss != null && ss.Count > 0) {
                        p.sources = [.. ss];
                    }
                    else {
                        OnError("GetProductBySku: " + p.sku + " - Product Source Not Found");
                        return null;
                    }

                    var imgs = GetProductImages(_customer_id, p.id);
                    if (imgs != null) {
                        p.images = [.. imgs];
                    }
                    else {
                        OnError("GetProductBySku: " + p.sku + " - Product Images Not Found");
                        return null;
                    }
                }

                return p;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brands"> Brands</param>
        /// <param name="_categories"> Categories</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> GetProducts(int _customer_id, out List<Brand> _brands, out List<Category> _categories,
            out List<ProductAttribute> _product_attributes, out List<ProductImage> _product_images) {
            _brands = []; _categories = []; _product_attributes = []; _product_images = []; List<ProductSource>? _product_other_sources = [];
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT p.id AS p_id, p.customer_id AS p_customer_id, p.source_product_id AS p_source_product_id, p.sources AS p_sources, p.update_date AS p_update_date, p.sku AS p_sku, p.type AS p_type, p.total_qty AS p_total_qty, p.name AS p_name, p.barcode AS p_barcode, p.price AS p_price, p.special_price AS p_special_price, p.custom_price AS p_custom_price, p.currency AS p_currency, p.tax AS p_tax, p.tax_included AS p_tax_included, pe.id AS pe_id, pe.brand_id AS pe_brand_id, pe.category_ids AS pe_category_ids, pe.is_xml_enabled AS pe_is_xml_enabled, pe.xml_sources AS pe_xml_sources, pe.update_date AS pe_update_date, ps.id AS ps_id, ps.name AS ps_name, ps.is_active AS ps_is_active, ps.qty AS ps_qty, ps.update_date AS ps_update_date, pe.weight AS pe_weight, pe.volume AS pe_volume, pe.is_enabled AS pe_is_enabled, pe.description AS pe_description " +
                    "FROM products AS p INNER JOIN products_ext AS pe ON p.sku = pe.sku " +
                    "INNER JOIN product_sources AS ps ON p.sku = ps.sku AND ps.name = SUBSTRING_INDEX(p.sources, ',', 1) " +
                    "WHERE p.customer_id=@customer_id;";
                List<Product> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["p_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["p_customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["p_source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["p_update_date"].ToString()),
                        sku = dataReader["p_sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["p_type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["p_total_qty"].ToString()),
                        name = dataReader["p_name"].ToString(),
                        barcode = dataReader["p_barcode"].ToString(),
                        price = decimal.Parse(dataReader["p_price"].ToString()),
                        special_price = decimal.Parse(dataReader["p_special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["p_custom_price"].ToString()),
                        currency = dataReader["p_currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["p_tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["p_tax_included"].ToString()))
                    };
                    p.extension = new ProductExtension() {
                        id = Convert.ToInt32(dataReader["pe_id"].ToString()),
                        customer_id = p.customer_id,
                        is_enabled = dataReader["pe_is_enabled"].ToString() == "1",
                        sku = p.sku,
                        barcode = p.barcode,
                        brand_id = Convert.ToInt32(dataReader["pe_brand_id"].ToString()),
                        category_ids = dataReader["pe_category_ids"].ToString(),
                        is_xml_enabled = dataReader["pe_is_xml_enabled"].ToString() == "1",
                        xml_sources = dataReader["pe_xml_sources"]?.ToString()?.Split(','),
                        weight = Convert.ToDecimal(dataReader["pe_weight"].ToString()),
                        volume = Convert.ToDecimal(dataReader["pe_volume"].ToString()),
                        description = dataReader["pe_description"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["pe_update_date"].ToString())
                    };
                    p.sources = [new ProductSource(_customer_id,
                        Convert.ToInt32(dataReader["ps_id"].ToString()),
                        dataReader["ps_name"].ToString(),
                        p.sku, p.barcode, Convert.ToInt32(dataReader["ps_qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["ps_is_active"].ToString())),
                        Convert.ToDateTime(dataReader["ps_update_date"].ToString())
                    )];
                    p.attributes = [];
                    p.images = [];
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                _brands = GetBrands(_customer_id);
                _categories = GetCategories(_customer_id);
                _product_attributes = GetProductAttributes(_customer_id);
                _product_images = GetProductImages(_customer_id);

                // Attach brands and categories
                for (int i = 0; i < list.Count; i++) {
                    if (list[i].extension.brand_id > 0)
                        list[i].extension.brand = _brands.First(x => x.id == list[i].extension.brand_id);
                    if (!string.IsNullOrWhiteSpace(list[i].extension.category_ids))
                        list[i].extension.categories = [.. _categories.Where(x => list[i].extension.category_ids.Split(",").Contains(x.id.ToString()))];
                }

                // Load other product sources
                var ai = LoadActiveIntegrations(_customer_id);
                if (ai is null || ai.Count == 0) {
                    OnError("GetProducts: Active Integrations Not Found");
                    return [];
                }
                var main_source = ai.FirstOrDefault(x => x.work_status && x.work_type == Work.WorkType.PRODUCT && x.work_direction == Work.WorkDirection.MAIN_SOURCE);
                if (main_source is not null) {
                    _product_other_sources = GetProductOtherSources(_customer_id, main_source.work_name);
                    if (_product_other_sources != null && _product_other_sources.Count > 0) {
                        foreach (var item in list) {
                            var selected_product_source = _product_other_sources?.FindAll(x => x.sku == item.sku);
                            if (selected_product_source != null && selected_product_source.Count > 0)
                                item.sources.AddRange(selected_product_source);
                        }
                    }
                }

                //Attach product attributes
                if (_product_attributes != null && _product_attributes.Count > 0) {
                    foreach (var item in list) {
                        item.attributes = [.. _product_attributes.Where(x => x.product_id == item.id)];
                    }
                }

                //Attach product images
                if (_product_images != null && _product_images.Count > 0) {
                    foreach (var item in list) {
                        item.images = [.. _product_images.Where(x => x.product_id == item.id)];
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return [];
            }
        }

        /// <summary>
        /// Gets the products from the database with filters
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> GetProducts(int _customer_id, ApiFilter _filters) {
            try {
                List<Brand> brands = GetBrands(_customer_id);
                List<Category> categories = GetCategories(_customer_id);
                List<ProductSource>? product_other_sources = [];
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };

                string _query = "SELECT p.id AS p_id, p.customer_id AS p_customer_id, p.source_product_id AS p_source_product_id, p.sources AS p_sources, p.update_date AS p_update_date, p.sku AS p_sku, p.type AS p_type, p.total_qty AS p_total_qty, p.name AS p_name, p.barcode AS p_barcode, p.price AS p_price, p.special_price AS p_special_price, p.custom_price AS p_custom_price, p.currency AS p_currency, p.tax AS p_tax, p.tax_included AS p_tax_included, pe.id AS pe_id, pe.brand_id AS pe_brand_id, pe.category_ids AS pe_category_ids, pe.is_xml_enabled AS pe_is_xml_enabled, pe.xml_sources AS pe_xml_sources, pe.update_date AS pe_update_date, ps.id AS ps_id, ps.name AS ps_name, ps.is_active AS ps_is_active, ps.qty AS ps_qty, ps.update_date AS ps_update_date, pe.weight AS pe_weight, pe.volume AS pe_volume, pe.is_enabled AS pe_is_enabled, pe.description AS pe_description " +
                    "FROM products AS p INNER JOIN products_ext AS pe ON p.sku = pe.sku " +
                    "INNER JOIN product_sources AS ps ON p.sku = ps.sku AND ps.name = SUBSTRING_INDEX(p.sources, ',', 1) " +
                    "WHERE p.customer_id=@customer_id";

                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                if (_filters.Sort != null)
                    _query += " ORDER BY " + _filters.Sort.Field + " " + _filters.Sort.Direction + " LIMIT @start,@end;";
                else {
                    _filters.Sort = new Sort() { Field = "p.id", Direction = "DESC" };
                    _query += " ORDER BY p.id DESC LIMIT @start,@end;";
                }

                if (state != System.Data.ConnectionState.Open) connection.Open();
                List<Product> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _filters.Pager.ItemsPerPage * _filters.Pager.CurrentPageIndex));
                cmd.Parameters.Add(new MySqlParameter("end", _filters.Pager.ItemsPerPage));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["p_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["p_customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["p_source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["p_update_date"].ToString()),
                        sku = dataReader["p_sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["p_type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["p_total_qty"].ToString()),
                        name = dataReader["p_name"].ToString(),
                        barcode = dataReader["p_barcode"].ToString(),
                        price = decimal.Parse(dataReader["p_price"].ToString()),
                        special_price = decimal.Parse(dataReader["p_special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["p_custom_price"].ToString()),
                        currency = dataReader["p_currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["p_tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["p_tax_included"].ToString()))
                    };
                    p.extension = new ProductExtension() {
                        id = Convert.ToInt32(dataReader["pe_id"].ToString()),
                        customer_id = p.customer_id,
                        is_enabled = dataReader["pe_is_enabled"].ToString() == "1",
                        sku = p.sku,
                        barcode = p.barcode,
                        brand_id = Convert.ToInt32(dataReader["pe_brand_id"].ToString()),
                        category_ids = dataReader["pe_category_ids"].ToString(),
                        is_xml_enabled = dataReader["pe_is_xml_enabled"].ToString() == "1",
                        xml_sources = dataReader["pe_xml_sources"]?.ToString()?.Split(','),
                        weight = Convert.ToDecimal(dataReader["pe_weight"].ToString()),
                        volume = Convert.ToDecimal(dataReader["pe_volume"].ToString()),
                        description = dataReader["pe_description"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["pe_update_date"].ToString())
                    };
                    p.sources = [new ProductSource(_customer_id,
                        Convert.ToInt32(dataReader["ps_id"].ToString()),
                        dataReader["ps_name"].ToString(),
                        p.sku, p.barcode, Convert.ToInt32(dataReader["ps_qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["ps_is_active"].ToString())),
                        Convert.ToDateTime(dataReader["ps_update_date"].ToString())

                    )];
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                // Attach brands and categories
                for (int i = 0; i < list.Count; i++) {
                    if (list[i].extension.brand_id > 0)
                        list[i].extension.brand = brands.FirstOrDefault(x => x.id == list[i].extension.brand_id);
                    if (!string.IsNullOrWhiteSpace(list[i].extension.category_ids))
                        list[i].extension.categories = categories?.Where(x => list[i].extension.category_ids.Split(",").Contains(x.id.ToString())).ToList();
                }

                // Load other product sources
                var ai = LoadActiveIntegrations(_customer_id);
                if (ai == null || ai.Count == 0) {
                    OnError("GetProducts: Active Integrations Not Found");
                    return [];
                }
                var main_source = ai.FirstOrDefault(x => x.work_status && x.work_type == Work.WorkType.PRODUCT && x.work_direction == Work.WorkDirection.MAIN_SOURCE);
                if (main_source != null)
                    product_other_sources = GetProductOtherSources(_customer_id, main_source.work_name);
                if (product_other_sources != null && product_other_sources.Count > 0) {
                    foreach (var item in list) {
                        var selected_product_source = product_other_sources?.FindAll(x => x.sku == item.sku);
                        if (selected_product_source != null && selected_product_source.Count > 0)
                            item.sources.AddRange(selected_product_source);
                    }
                }

                // Get product attributes
                var attrs = GetProductAttributes(_customer_id);
                if (attrs != null && attrs.Count > 0) {
                    foreach (var item in list) {
                        item.attributes = [.. attrs.Where(x => x.product_id == item.id)];
                    }
                }

                // Get product images
                var images = GetProductImages(_customer_id);
                if (images != null && images.Count > 0) {
                    foreach (var item in list) {
                        item.images = [.. images.Where(x => x.product_id == item.id)];
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_keyword">Keyword</param>
        /// <param name="_with_ext">Get with extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> SearchProducts(int _customer_id, string[] _skus, bool _with_attr = false, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string skus = string.Empty;
                for (int i = 0; i < _skus.Length; i++) {
                    if (i == 0)
                        skus += "@sku" + i.ToString();
                    else
                        skus += "," + "@sku" + i.ToString();
                }
                string _query = "SELECT * FROM products WHERE sku IN (" + skus + ") AND customer_id=@customer_id ORDER BY id DESC";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                for (int i = 0; i < _skus.Length; i++) {
                    cmd.Parameters.Add(new MySqlParameter("sku" + i.ToString(), _skus[i]));
                }
                //cmd.Parameters.Add(new MySqlParameter("keyword", string.Format("%{0}%", _keyword)));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                        name = dataReader["name"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        price = decimal.Parse(dataReader["price"].ToString()),
                        special_price = decimal.Parse(dataReader["special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString())),
                    };
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (_with_ext) {
                    var exts = GetProductExts(_customer_id);
                    foreach (var item in list) {
                        var selected_ext = exts?.Where(x => x.sku == item.sku).FirstOrDefault();
                        if (selected_ext != null)
                            item.extension = selected_ext;
                        else {
                            OnError("SearchProducts: " + item.sku + " - Product Extension Not Found");
                            return null;
                        }
                    }
                }

                if (_with_attr) {
                    var attrs = GetProductAttributes(_customer_id);
                    if (attrs != null) {
                        foreach (var item in list) {
                            item.attributes = [.. attrs.Where(x => x.product_id == item.id)];
                        }
                    }
                    else {
                        OnError("GetProducts: Product Attributes Not Found");
                        return null;
                    }
                }

                var product_sources = GetProductSources(_customer_id);
                foreach (var item in list) {
                    var selected_product_source = product_sources?.Where(x => x.sku == item.sku).ToList();
                    if (selected_product_source != null)
                        item.sources = selected_product_source;
                    else {
                        OnError("SearchProducts: " + item.sku + " - Product Source Not Found");
                        return null;
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_keyword">Keyword</param>
        /// <param name="_with_ext">Get with extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> SearchProducts(int _customer_id, string _keyword, bool _with_attr = false, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM products WHERE (sku LIKE @keyword OR name LIKE @keyword OR barcode LIKE @keyword) AND customer_id=@customer_id ORDER BY id DESC";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("keyword", string.Format("%{0}%", _keyword)));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                        name = dataReader["name"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        price = decimal.Parse(dataReader["price"].ToString()),
                        special_price = decimal.Parse(dataReader["special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString())),
                    };
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (_with_ext) {
                    var exts = GetProductExts(_customer_id);
                    foreach (var item in list) {
                        var selected_ext = exts?.Where(x => x.sku == item.sku).FirstOrDefault();
                        if (selected_ext != null)
                            item.extension = selected_ext;
                        else {
                            OnError("SearchProducts: " + item.sku + " - Product Extension Not Found");
                            return null;
                        }
                    }
                }

                if (_with_attr) {
                    var attrs = GetProductAttributes(_customer_id);
                    if (attrs != null) {
                        foreach (var item in list) {
                            item.attributes = attrs.Where(x => x.product_id == item.id).ToList();
                        }
                    }
                    else {
                        OnError("GetProducts: Product Attributes Not Found");
                        return null;
                    }
                }

                var product_sources = GetProductSources(_customer_id);
                foreach (var item in list) {
                    var selected_product_source = product_sources?.Where(x => x.sku == item.sku).ToList();
                    if (selected_product_source != null)
                        item.sources = selected_product_source;
                    else {
                        OnError("SearchProducts: " + item.sku + " - Product Source Not Found");
                        return null;
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the products to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_products">Products</param>
        /// <param name="_with_ext">Insert with extension</param>
        public int InsertProducts(int _customer_id, List<Product> _products) {
            try {
                UInt64 inserted_id = 0;
                foreach (Product item in _products) {
                    if (this.IsProductExists(_customer_id, item.sku)) {
                        OnError("InsertProducts: " + item.sku + " - Product Already Exists");
                        return 0;
                    }

                    string _query = "START TRANSACTION;" +
                        "INSERT INTO products (customer_id,source_product_id,sku,type,barcode,total_qty,price,special_price,custom_price,currency,tax,tax_included,name,sources) VALUES (@customer_id,@source_product_id,@sku,@type,@barcode,@total_qty,@price,@special_price,@custom_price,@currency,@tax,@tax_included,@name,@sources);" +
                        "SELECT LAST_INSERT_ID();" +
                        "COMMIT;";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("source_product_id", item.source_product_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                    cmd.Parameters.Add(new MySqlParameter("type", item.type));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("total_qty", item.sources.Where(x => x.is_active).Sum(x => x.qty)));
                    cmd.Parameters.Add(new MySqlParameter("price", item.price));
                    cmd.Parameters.Add(new MySqlParameter("special_price", item.special_price));
                    cmd.Parameters.Add(new MySqlParameter("custom_price", item.custom_price));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    cmd.Parameters.Add(new MySqlParameter("tax", item.tax));
                    cmd.Parameters.Add(new MySqlParameter("tax_included", item.tax_included));
                    cmd.Parameters.Add(new MySqlParameter("name", item.name));
                    cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", item.sources.Where(x => x.is_active).Select(x => x.name))));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    inserted_id += (UInt64)cmd.ExecuteScalar();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (!InsertProductExt(_customer_id, item.extension)) {
                        OnError("InsertProducts: " + item.sku + " - Product Extension Insert Error");
                        return 0;
                    }

                    if (!UpdateProductSources(_customer_id, item.sources, item.sku)) {
                        OnError("InsertProducts: " + item.sku + " - Product Source Insert Error");
                        return 0;
                    }

                    if (item.attributes != null && item.attributes.Count > 0) {
                        if (!UpdateProductAttributes(_customer_id, item.attributes, (int)inserted_id)) {
                            OnError("InsertProducts: " + item.sku + " - Product Attributes Insert Error");
                            return 0;
                        }
                    }

                    if (item.images != null && item.images.Count > 0) {
                        if (!UpdateProductImages(_customer_id, item.images, (int)inserted_id)) {
                            OnError("InsertProducts: " + item.sku + " - Product Images Insert Error");
                            return 0;
                        }
                    }
                }

                if (inserted_id > 0)
                    return (int)inserted_id;
                else return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return 0;
            }
        }

        public Product? InsertProduct(int _customer_id, Product _product) {
            try {
                if (this.IsProductExists(_customer_id, _product.sku)) {
                    OnError("InsertProducts: " + _product.sku + " - Product Already Exists");
                    return null;
                }

                UInt64 inserted_id = 0;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO products (customer_id,source_product_id,sku,type,barcode,total_qty,price,special_price,custom_price,currency,tax,tax_included,name,sources) VALUES (@customer_id,@source_product_id,@sku,@type,@barcode,@total_qty,@price,@special_price,@custom_price,@currency,@tax,@tax_included,@name,@sources);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("source_product_id", _product.source_product_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _product.sku));
                cmd.Parameters.Add(new MySqlParameter("type", _product.type));
                cmd.Parameters.Add(new MySqlParameter("barcode", _product.barcode));
                cmd.Parameters.Add(new MySqlParameter("total_qty", _product.sources.Where(x => x.is_active).Sum(x => x.qty)));
                cmd.Parameters.Add(new MySqlParameter("price", _product.price));
                cmd.Parameters.Add(new MySqlParameter("special_price", _product.special_price));
                cmd.Parameters.Add(new MySqlParameter("custom_price", _product.custom_price));
                cmd.Parameters.Add(new MySqlParameter("currency", _product.currency));
                cmd.Parameters.Add(new MySqlParameter("tax", _product.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _product.tax_included));
                cmd.Parameters.Add(new MySqlParameter("name", _product.name));
                cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", _product.sources.Where(x => x.is_active).Select(x => x.name))));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                inserted_id = (UInt64)cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (!InsertProductExt(_customer_id, _product.extension)) {
                    OnError("InsertProducts: " + _product.sku + " - Product Extension Insert Error");
                    return null;
                }

                if (!UpdateProductSources(_customer_id, _product.sources, _product.sku)) {
                    OnError("InsertProducts: " + _product.sku + " - Product Source Insert Error");
                    return null;
                }

                if (_product.attributes != null && _product.attributes.Count > 0) {
                    if (!UpdateProductAttributes(_customer_id, _product.attributes, (int)inserted_id)) {
                        OnError("InsertProducts: " + _product.sku + " - Product Attributes Insert Error");
                        return null;
                    }
                }

                if (_product.images != null && _product.images.Count > 0) {
                    if (!UpdateProductImages(_customer_id, _product.images, (int)inserted_id)) {
                        OnError("InsertProducts: " + _product.sku + " - Product Images Insert Error");
                        return null;
                    }
                }

                if (inserted_id > 0) {
                    return GetProduct(_customer_id, (int)inserted_id);
                }
                else return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the products in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_products">Products</param>
        /// <param name="_with_ext">Update with extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateProducts(int _customer_id, List<Product> _products) {
            try {
                int val = 0;
                foreach (Product item in _products) {
                    string _query = "UPDATE products SET type=@type,barcode=@barcode,total_qty=@total_qty,price=@price,special_price=@special_price,custom_price=@custom_price,currency=@currency,tax=@tax,tax_included=@tax_included,update_date=@update_date,name=@name,sources=@sources,source_product_id=@source_product_id WHERE id=@id AND sku=@sku AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("source_product_id", item.source_product_id));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                    cmd.Parameters.Add(new MySqlParameter("type", item.type));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("price", item.price));
                    cmd.Parameters.Add(new MySqlParameter("special_price", item.special_price));
                    cmd.Parameters.Add(new MySqlParameter("custom_price", item.custom_price));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    cmd.Parameters.Add(new MySqlParameter("tax", item.tax));
                    cmd.Parameters.Add(new MySqlParameter("tax_included", item.tax_included));
                    cmd.Parameters.Add(new MySqlParameter("name", item.name));
                    cmd.Parameters.Add(new MySqlParameter("total_qty", item.sources.Where(x => x.is_active).Sum(x => x.qty)));
                    cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", item.sources.Where(x => x.is_active).Select(x => x.name).ToArray())));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (!UpdateProductExt(_customer_id, item.extension)) {
                        OnError("UpdateProducts: " + item.sku + " - Product Extension Update Error");
                        return false;
                    }

                    //DeleteProductSources(_customer_id, item.sku.ToString());
                    if (!UpdateProductSources(_customer_id, item.sources, item.sku)) {
                        OnError("UpdateProducts: " + item.sku + " - Product Source Insert Error");
                        return false;
                    }

                    if (item.attributes != null && item.attributes.Count > 0) {
                        if (!UpdateProductAttributes(_customer_id, item.attributes, item.id)) {
                            OnError("UpdateProducts: " + item.sku + " - Product Attributes Update Error");
                            return false;
                        }
                    }

                    if (item.images != null && item.images.Count > 0) {
                        if (!UpdateProductImages(_customer_id, item.images, item.id)) {
                            OnError("UpdateProducts: " + item.sku + " - Product Images Insert Error");
                            return false;
                        }
                    }
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        public Product? UpdateProduct(int _customer_id, Product _product) {
            try {
                string _query = "UPDATE products SET type=@type,barcode=@barcode,total_qty=@total_qty,price=@price,special_price=@special_price,custom_price=@custom_price,currency=@currency,tax=@tax,tax_included=@tax_included,update_date=@update_date,name=@name,sources=@sources,source_product_id=@source_product_id WHERE id=@id AND sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("id", _product.id));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("source_product_id", _product.source_product_id));
                cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                cmd.Parameters.Add(new MySqlParameter("sku", _product.sku));
                cmd.Parameters.Add(new MySqlParameter("type", _product.type));
                cmd.Parameters.Add(new MySqlParameter("barcode", _product.barcode));
                cmd.Parameters.Add(new MySqlParameter("price", _product.price));
                cmd.Parameters.Add(new MySqlParameter("special_price", _product.special_price));
                cmd.Parameters.Add(new MySqlParameter("custom_price", _product.custom_price));
                cmd.Parameters.Add(new MySqlParameter("currency", _product.currency));
                cmd.Parameters.Add(new MySqlParameter("tax", _product.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _product.tax_included));
                cmd.Parameters.Add(new MySqlParameter("name", _product.name));
                cmd.Parameters.Add(new MySqlParameter("total_qty", _product.sources.Where(x => x.is_active).Sum(x => x.qty)));
                cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", _product.sources.Where(x => x.is_active).Select(x => x.name).ToArray())));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val == 0) {
                    OnError("UpdateProduct: " + _product.sku + " - Product Not Found");
                    return null;
                }

                if (!UpdateProductExt(_customer_id, _product.extension)) {
                    OnError("UpdateProduct: " + _product.sku + " - Product Extension Update Error");
                    return null;
                }

                //if (!UpdateProductSources(_customer_id, _product.sources, _product.sku)) {
                //    OnError("UpdateProduct: " + _product.sku + " - Product Sources Update Error");
                //    return null;
                //}

                if (_product.attributes != null && _product.attributes.Count > 0) {
                    if (!UpdateProductAttributes(_customer_id, _product.attributes, _product.id)) {
                        OnError("UpdateProduct: " + _product.sku + " - Product Attributes Update Error");
                        return null;
                    }
                }

                var deleted_images = GetProductImages(_customer_id, _product.id).Except(_product.images).ToList();
                foreach (var ditem in deleted_images) {
                    if (!DeleteProductImage(_customer_id, ditem)) {
                        OnError("UpdateProduct: " + _product.sku + " - Product Image Delete Error");
                        return null;
                    }
                }
                if (_product.images != null && _product.images.Count > 0) {
                    if (!UpdateProductImages(_customer_id, _product.images, _product.id)) {
                        OnError("UpdateProduct: " + _product.sku + " - Product Images Update Error");
                        return null;
                    }
                }
                return GetProductBySku(_customer_id, _product.sku);
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets XML enabled products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <param name="_with_ext">Get with extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> GetXMLEnabledProducts(int _customer_id, bool _val = true, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT p.id,p.customer_id,p.source_product_id,p.update_date,p.sku,p.type,p.total_qty,p.name,p.barcode,p.price,p.special_price,p.custom_price,p.currency,p.tax,p.tax_included " +
                    "FROM products_ext AS ext INNER JOIN products AS p ON ext.sku=p.sku WHERE ext.is_xml_enabled=@is_xml_enabled AND ext.customer_id=@customer_id";
                List<Product> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _val ? 1 : 0));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                        name = dataReader["name"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        price = decimal.Parse(dataReader["price"].ToString()),
                        special_price = decimal.Parse(dataReader["special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString())),
                    };
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (_with_ext) {
                    var exts = GetProductExts(_customer_id);
                    foreach (var item in list) {
                        var selected_ext = exts?.Where(x => x.sku == item.sku).FirstOrDefault();
                        if (selected_ext != null)
                            item.extension = selected_ext;
                        else {
                            OnError("GetXMLEnabledProducts: " + item.sku + " - Product Extension Not Found");
                            return null;
                        }
                    }
                }

                var product_sources = GetProductSources(_customer_id);
                foreach (var item in list) {
                    var selected_product_source = product_sources?.Where(x => x.sku == item.sku).ToList();
                    if (selected_product_source != null)
                        item.sources = selected_product_source;
                    else {
                        OnError("GetXMLEnabledProducts: " + item.sku + " - Product Source Not Found");
                        return null;
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        public bool IsProductExtExists(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM products_ext WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        public bool IsProductExists(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM products WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets product count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetProductsCount(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) " +
                    "FROM products AS p INNER JOIN products_ext AS pe ON p.sku = pe.sku " +
                    "INNER JOIN product_sources AS ps ON p.sku = ps.sku AND ps.name = SUBSTRING_INDEX(p.sources, ',', 1) " +
                    "WHERE p.customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null) {
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        }
                        else {
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                        }
                    }
                }
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets product count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetProductsCount(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM products WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Updates XML status by product barcode in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_barcode">Barcode</param>
        /// <param name="_is_xml_enabled">Is XML Enabled</param>
        /// <param name="_xml_sources">XML Sources</param>
        /// <param name="_with_ext">Update with extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateXMLStatusByProductBarcode(int _customer_id, string _barcode, bool _is_xml_enabled, string[]? _xml_sources, bool _with_ext = true) {
            try {
                int val = 0;
                if (!string.IsNullOrWhiteSpace(_barcode)) {
                    string _query = "UPDATE products_ext SET is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources WHERE barcode=@barcode AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("barcode", _barcode));
                    cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _is_xml_enabled ? 1 : 0));
                    cmd.Parameters.Add(new MySqlParameter("xml_sources", _xml_sources != null ? string.Join(",", _xml_sources) : null));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (val > 0)
                        return true;
                    else return false;
                }
                return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Product Extension
        /// <summary>
        /// Gets the product extension from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public ProductExtension? GetProductExt(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM products_ext WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                ProductExtension? px = null;
                if (dataReader.Read()) {
                    px = new ProductExtension();
                    px.id = Convert.ToInt32(dataReader["id"].ToString());
                    px.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    px.brand_id = Convert.ToInt32(dataReader["brand_id"].ToString());
                    px.category_ids = dataReader["category_ids"].ToString();
                    px.sku = dataReader["sku"].ToString();
                    px.barcode = dataReader["barcode"].ToString();
                    px.is_xml_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xml_enabled"].ToString()));
                    px.xml_sources = dataReader["xml_sources"]?.ToString()?.Split(',');
                    px.description = dataReader["description"].ToString();
                    px.weight = decimal.Parse(dataReader["weight"].ToString());
                    px.volume = decimal.Parse(dataReader["volume"].ToString());
                    px.is_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_enabled"].ToString()));
                    px.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (px != null && px.brand_id > 0) {
                    var b = GetBrand(_customer_id, px.brand_id);
                    if (b != null) {
                        px.brand = b;
                    }
                    else {
                        OnError("GetProductExt: " + px.sku + " - Product Brand Not Found");
                        return null;
                    }

                    var cats = GetProductCategories(_customer_id, _sku);
                    if (cats != null && cats.Count > 0) {
                        px.categories = [.. cats];
                    }
                    else {
                        OnError("GetProductExt: " + px.sku + " - Product Categories Not Found");
                        return null;
                    }
                }

                return px;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product extensions from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductExtension> GetProductExts(int _customer_id, ref List<Brand> brands, ref List<Category> categories) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string _query = "SELECT * FROM products_ext WHERE customer_id=@customer_id";
                List<ProductExtension> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductExtension s = new ProductExtension {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        brand_id = Convert.ToInt32(dataReader["brand_id"].ToString()),
                        category_ids = dataReader["category_ids"].ToString(),
                        sku = dataReader["sku"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        is_xml_enabled = dataReader["is_xml_enabled"] != null && dataReader["is_xml_enabled"].ToString() == "1",
                        xml_sources = dataReader["xml_sources"]?.ToString()?.Split(','),
                        description = dataReader["description"].ToString(),
                        weight = decimal.Parse(dataReader["weight"].ToString()),
                        volume = decimal.Parse(dataReader["volume"].ToString()),
                        is_enabled = dataReader["is_enabled"] != null && dataReader["is_enabled"].ToString() == "1",
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                //brands = GetBrands(_customer_id);
                //categories = GetCategories(_customer_id);
                if (brands == null || categories == null) {
                    OnError("GetProductExts: Brands or Categories Not Found");
                    return null;
                }

                foreach (var item in list) {
                    var b = brands.FirstOrDefault(x => x.id == item.brand_id);
                    if (b != null) {
                        item.brand = b;
                    }
                    else {
                        OnError("GetProductExts: " + item.sku + " - Product Brand Not Found");
                        return null;
                    }

                    var c = categories.Where(x => item.category_ids.Split(',').Contains(x.id.ToString())).ToList();
                    if (c != null && c.Count > 0) {
                        item.categories = [.. c];
                    }
                    else {
                        OnError("GetProductExts: " + item.sku + " - Product Categories Not Found");
                        return null;
                    }
                }
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product extensions from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductExtension> GetProductExts(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM products_ext WHERE customer_id=@customer_id";
                List<ProductExtension> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductExtension s = new ProductExtension {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        brand_id = Convert.ToInt32(dataReader["brand_id"].ToString()),
                        category_ids = dataReader["category_ids"].ToString(),
                        sku = dataReader["sku"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        is_xml_enabled = dataReader["is_xml_enabled"] != null && dataReader["is_xml_enabled"].ToString() == "1",
                        xml_sources = dataReader["xml_sources"]?.ToString()?.Split(','),
                        description = dataReader["description"].ToString(),
                        weight = decimal.Parse(dataReader["weight"].ToString()),
                        volume = decimal.Parse(dataReader["volume"].ToString()),
                        is_enabled = dataReader["is_enabled"] != null && dataReader["is_enabled"].ToString() == "1",
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                var brands = GetBrands(_customer_id);
                var categories = GetCategories(_customer_id);
                if (brands == null || categories == null) {
                    OnError("GetProductExts: Brands or Categories Not Found");
                    return null;
                }

                foreach (var item in list) {
                    var b = brands.FirstOrDefault(x => x.id == item.brand_id);
                    if (b != null) {
                        item.brand = b;
                    }
                    else {
                        OnError("GetProductExts: " + item.sku + " - Product Brand Not Found");
                        return null;
                    }

                    var c = categories.Where(x => item.category_ids.Split(',').Contains(x.id.ToString())).ToList();
                    if (c != null && c.Count > 0) {
                        item.categories = [.. c];
                    }
                    else {
                        OnError("GetProductExts: " + item.sku + " - Product Categories Not Found");
                        return null;
                    }
                }
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the product extension to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertProductExt(int _customer_id, ProductExtension _source) {
            try {
                if (this.IsProductExtExists(_customer_id, _source.sku)) {
                    OnError("Sku is already in table.");
                    return false;
                }

                int val = 0;
                string _query = "INSERT INTO products_ext (customer_id,brand_id,category_ids,sku,barcode,weight,volume,description) VALUES " +
                    "(@customer_id,@brand_id,@category_ids,@sku,@barcode,@weight,@volume,@description)"; //,@is_xml_enabled,@xml_sources
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_id", _source.brand_id));
                cmd.Parameters.Add(new MySqlParameter("category_ids", _source.category_ids));
                cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode));
                cmd.Parameters.Add(new MySqlParameter("weight", _source.weight));
                cmd.Parameters.Add(new MySqlParameter("volume", _source.volume));
                cmd.Parameters.Add(new MySqlParameter("description", _source.description));
                //cmd.Parameters.Add( new MySqlParameter( "is_xml_enabled", _source.is_xml_enabled ) );
                //cmd.Parameters.Add( new MySqlParameter( "xml_sources", _source.xml_sources != null ? string.Join( ",", _source.xml_sources ) : null ) );
                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the product extension in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateProductExt(int _customer_id, ProductExtension _source) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "UPDATE products_ext SET brand_id=@brand_id,category_ids=@category_ids,barcode=@barcode,is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources,update_date=@update_date,weight=@weight,volume=@volume,description=@description,is_enabled=@is_enabled " +
                    "WHERE sku=@sku AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                cmd.Parameters.Add(new MySqlParameter("brand_id", _source.brand_id));
                cmd.Parameters.Add(new MySqlParameter("category_ids", _source.category_ids));
                cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode));
                cmd.Parameters.Add(new MySqlParameter("xml_sources", (_source.xml_sources != null && _source.xml_sources.Length > 0) ? string.Join(",", _source.xml_sources) : string.Empty));
                cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _source.is_xml_enabled));
                cmd.Parameters.Add(new MySqlParameter("weight", _source.weight));
                cmd.Parameters.Add(new MySqlParameter("volume", _source.volume));
                cmd.Parameters.Add(new MySqlParameter("description", _source.description));
                cmd.Parameters.Add(new MySqlParameter("is_enabled", _source.is_enabled));
                cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the product extension from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteProductExt(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM products_ext WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Product Source
        /// <summary>
        /// Updates the product sources in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sources">Product Sources</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateProductSources(int _customer_id, List<ProductSource> _sources, string _sku) {
            try {
                if (_sources.Count == 0) {
                    OnError("UpdateProductSources: No sources found");
                    return false;
                }
                UInt64 val = 0;
                var existed_sources = GetProductSources(_customer_id, _sku);
                foreach (var item in _sources) {
                    var existed_source = existed_sources.FirstOrDefault(x => x.name == item.name && x.sku == item.sku);
                    if (existed_source != null) {
                        if (existed_source.qty != item.qty || existed_source.is_active != item.is_active || existed_source.barcode != item.barcode) {
                            string _query = "UPDATE product_sources SET barcode=@barcode,qty=@qty,is_active=@is_active,update_date=@update_date WHERE sku=@sku AND name=@name AND customer_id=@customer_id;";
                            MySqlCommand cmd = new MySqlCommand(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("name", item.name));
                            cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                            cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                            cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                            cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) connection.Open();
                            val = (UInt64)cmd.ExecuteNonQuery();
                            if (state == System.Data.ConnectionState.Open) connection.Close();
                        }
                        else val++;
                    }
                    else {
                        string _query = "INSERT INTO product_sources (customer_id,name,sku,barcode,qty,is_active) VALUES (@customer_id,@name,@sku,@barcode,@qty,@is_active);";
                        MySqlCommand cmd = new MySqlCommand(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("name", item.name));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                        cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                        cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                        if (state != System.Data.ConnectionState.Open) connection.Open();
                        val = (UInt64)cmd.ExecuteNonQuery();
                        if (state == System.Data.ConnectionState.Open) connection.Close();
                    }
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the product sources from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductSource>? GetProductSources(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM product_sources WHERE customer_id=@customer_id";
                List<ProductSource> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductSource s = new ProductSource(
                        _customer_id,
                        Convert.ToInt32(dataReader["id"].ToString()),
                        dataReader["name"].ToString(),
                        dataReader["sku"].ToString(),
                        dataReader["barcode"].ToString(),
                        Convert.ToInt32(dataReader["qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                        Convert.ToDateTime(dataReader["update_date"].ToString())
                    );

                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product sources from the database except the main source
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductSource>? GetProductOtherSources(int _customer_id, string _main_source) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_sources WHERE name <> @name AND customer_id=@customer_id;";
                List<ProductSource> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("name", _main_source));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductSource s = new ProductSource(
                        _customer_id,
                        Convert.ToInt32(dataReader["id"].ToString()),
                        dataReader["name"].ToString(),
                        dataReader["sku"].ToString(),
                        dataReader["barcode"].ToString(),
                        Convert.ToInt32(dataReader["qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                        Convert.ToDateTime(dataReader["update_date"].ToString())
                    );

                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product sources from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductSource> GetProductSources(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                List<ProductSource> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductSource s = new ProductSource(
                        _customer_id,
                        Convert.ToInt32(dataReader["id"].ToString()),
                        dataReader["name"].ToString(),
                        dataReader["sku"].ToString(),
                        dataReader["barcode"].ToString(),
                        Convert.ToInt32(dataReader["qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                        Convert.ToDateTime(dataReader["update_date"].ToString())
                    );
                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the product source to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Source</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertProductSource(int _customer_id, ProductSource _source) {
            try {
                int val = 0;
                string _query = "INSERT INTO product_sources (customer_id,name,sku,barcode,qty,is_active) VALUES (@customer_id,@name,@sku,@barcode,@qty,@is_active)";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("name", _source.name));
                cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode));
                cmd.Parameters.Add(new MySqlParameter("qty", _source.qty));
                cmd.Parameters.Add(new MySqlParameter("is_active", _source.is_active));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the product source from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        [Obsolete]
        public bool DeleteProductSources(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Product Image
        /// <summary>
        /// Inserts the product image to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_image">ProductImage</param>
        /// <returns>Error returns 'int:0'</returns>
        public int InsertProductImage(int _customer_id, ProductImage _image) {
            try {
                object val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO product_images (customer_id,product_id,sku,type,image_name,image_url,image_base64,is_default) VALUES (@customer_id,@product_id,@sku,@type,@image_name,@image_url,@image_base64,@is_default);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _image.product_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _image.sku));
                cmd.Parameters.Add(new MySqlParameter("type", (int)_image.type));
                cmd.Parameters.Add(new MySqlParameter("image_name", _image.image_name));
                cmd.Parameters.Add(new MySqlParameter("image_url", _image.image_url));
                cmd.Parameters.Add(new MySqlParameter("image_base64", _image.image_base64));
                cmd.Parameters.Add(new MySqlParameter("is_default", _image.is_default ? 1 : 0));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out int PIID))
                        return PIID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Updates the product image in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_image">ProductImage</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public bool UpdateProductImages(int _customer_id, List<ProductImage> _images, int _product_id) {
            try {
                UInt64 val = 0;
                var existed_images = GetProductImages(_customer_id, _product_id);
                foreach (var item in _images) {
                    var existed_image = existed_images.FirstOrDefault(x => x.image_name == item.image_name && x.sku == item.sku);
                    if (existed_image != null) {
                        if (existed_image.image_url != item.image_url || existed_image.is_default != item.is_default || existed_image.type != item.type) {
                            string _query = "UPDATE product_images SET type=@type,image_url=@image_url,image_base64=@image_base64,is_default=@is_default,update_date=@update_date WHERE sku=@sku AND product_id=@product_id AND image_name=@image_name AND customer_id=@customer_id;";
                            MySqlCommand cmd = new MySqlCommand(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                            cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                            cmd.Parameters.Add(new MySqlParameter("type", item.type));
                            cmd.Parameters.Add(new MySqlParameter("image_name", item.image_name));
                            cmd.Parameters.Add(new MySqlParameter("image_url", item.image_url));
                            cmd.Parameters.Add(new MySqlParameter("image_base64", item.image_base64));
                            cmd.Parameters.Add(new MySqlParameter("is_default", item.is_default));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) connection.Open();
                            val += (UInt64)cmd.ExecuteNonQuery();
                            if (state == System.Data.ConnectionState.Open) connection.Close();
                        }
                        else val++;
                    }
                    else {
                        string _query = "START TRANSACTION;" +
                            "INSERT INTO product_images (customer_id,product_id,sku,type,image_name,image_url,image_base64,is_default) VALUES (@customer_id,@product_id,@sku,@type,@image_name,@image_url,@image_base64,@is_default);" +
                            "SELECT LAST_INSERT_ID();" +
                            "COMMIT;";
                        MySqlCommand cmd = new MySqlCommand(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("type", item.type));
                        cmd.Parameters.Add(new MySqlParameter("image_name", item.image_name));
                        cmd.Parameters.Add(new MySqlParameter("image_url", item.image_url));
                        cmd.Parameters.Add(new MySqlParameter("image_base64", item.image_base64));
                        cmd.Parameters.Add(new MySqlParameter("is_default", item.is_default));
                        if (state != System.Data.ConnectionState.Open) connection.Open();
                        val += (UInt64)cmd.ExecuteScalar();
                        if (state == System.Data.ConnectionState.Open) connection.Close();
                    }
                }
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the product images from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductImage> GetProductImages(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id;";
                List<ProductImage> list = new List<ProductImage>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.CommandTimeout = 600;
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductImage pi = new ProductImage {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        image_name = dataReader["image_name"].ToString(),
                        image_url = dataReader["image_url"].ToString(),
                        image_base64 = dataReader["image_base64"].ToString(),
                        is_default = dataReader["is_default"] != null && dataReader["is_default"].ToString() == "1",
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(pi);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product images by sku from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductImage> GetProductImages(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id AND sku=@sku;";
                List<ProductImage> list = new List<ProductImage>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductImage pi = new ProductImage {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        image_name = dataReader["image_name"].ToString(),
                        image_url = dataReader["image_url"].ToString(),
                        image_base64 = dataReader["image_base64"].ToString(),
                        is_default = dataReader["is_default"] != null && dataReader["is_default"].ToString() == "1",
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(pi);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product images by product_id from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductImage> GetProductImages(int _customer_id, int _product_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id AND product_id=@product_id;";
                List<ProductImage> list = new List<ProductImage>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductImage pi = new ProductImage {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                        image_name = dataReader["image_name"].ToString(),
                        image_url = dataReader["image_url"].ToString(),
                        image_base64 = dataReader["image_base64"].ToString(),
                        is_default = dataReader["is_default"] != null && dataReader["is_default"].ToString() == "1",
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(pi);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the product image target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_image">Product Image</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteProductImage(int _customer_id, ProductImage _product_image) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "DELETE FROM product_images WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _product_image.id));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Category and Product Target Operations
        /// <summary>
        /// Gets the category targets from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<CategoryTarget> GetCategoryTargets(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM category_targets WHERE customer_id=@customer_id";
                List<CategoryTarget> list = new List<CategoryTarget>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    CategoryTarget ct = new CategoryTarget {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        category_id = Convert.ToInt32(dataReader["category_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                        target_name = dataReader["target_name"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(ct);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product targets from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductTarget> GetProductTargets(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_targets WHERE customer_id=@customer_id";
                List<ProductTarget> list = new List<ProductTarget>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductTarget pt = new ProductTarget {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                        target_name = dataReader["target_name"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(pt);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the category target relation to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">CategoryTarget</param>
        /// <returns>Error returns 'int:0'</returns>
        public int InsertCategoryTarget(int _customer_id, CategoryTarget _target) {
            try {
                object val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO category_targets (customer_id,category_id,target_id,target_name) VALUES (@customer_id,@category_id,@target_id,@target_name);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("category_id", _target.category_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out int CTID))
                        return CTID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Inserts the product target relation to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">ProductTarget</param>
        /// <returns>Error returns 'int:0'</returns>
        public int InsertProductTarget(int _customer_id, ProductTarget _target) {
            try {
                object val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO product_targets (customer_id,product_id,target_id,target_name) VALUES (@customer_id,@product_id,@target_id,@target_name);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _target.product_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));

                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out int PTID))
                        return PTID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Updates the category target relation in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">CategoryTarget</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public bool UpdateCategoryTarget(int _customer_id, CategoryTarget _target) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "UPDATE category_targets SET category_id=@category_id,target_id=@target_id,target_name=@target_name,update_date=@update_date " +
                    "WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _target.id));
                cmd.Parameters.Add(new MySqlParameter("category_id", _target.category_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the product target relation in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">ProductTarget</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public bool UpdateProductTarget(int _customer_id, ProductTarget _target) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "UPDATE product_targets SET product_id=@product_id,target_id=@target_id,target_name=@target_name,update_date=@update_date " +
                    "WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _target.id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _target.product_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the product target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteProductTarget(int _customer_id, int _product_id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM product_targets WHERE product_id=@product_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Attribute
        /// <summary>
        /// Gets the attribute from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_name">Attribute Name</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Attribute? GetAttribute(int _customer_id, string _name) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM attributes WHERE attribute_name=@attribute_name AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("attribute_name", _name));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Attribute? a = null;
                if (dataReader.Read()) {
                    a = new Attribute();
                    a.id = Convert.ToInt32(dataReader["id"].ToString());
                    a.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    a.attribute_name = dataReader["attribute_name"].ToString();
                    a.attribute_title = dataReader["attribute_title"].ToString();
                    a.type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString());
                    a.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return a;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the attribute from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attribute_id">Attribute ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Attribute? GetAttribute(int _customer_id, int _attribute_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM attributes WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _attribute_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Attribute? a = null;
                if (dataReader.Read()) {
                    a = new Attribute();
                    a.id = Convert.ToInt32(dataReader["id"].ToString());
                    a.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    a.attribute_name = dataReader["attribute_name"].ToString();
                    a.attribute_title = dataReader["attribute_title"].ToString();
                    a.type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString());
                    a.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return a;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the attribute options from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attribute_id">Attribute ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<AttributeOption> GetAttributeOptions(int _customer_id, int _attribute_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM attribute_options WHERE attribute_id=@attribute_id AND customer_id=@customer_id";
                List<AttributeOption> list = new List<AttributeOption>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("attribute_id", _attribute_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    AttributeOption ao = new AttributeOption {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                        option_name = dataReader["option_name"].ToString(),
                        option_value = dataReader["option_value"].ToString(),
                    };
                    list.Add(ao);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the attribute options from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_option_ids">Option ID's</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<AttributeOption> GetAttributeOptions(int _customer_id, string _option_ids) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = string.Format("SELECT * FROM attribute_options WHERE id IN ({0}) AND customer_id=@customer_id", _option_ids);
                List<AttributeOption> list = new List<AttributeOption>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                //cmd.Parameters.Add( new MySqlParameter( "option_ids", _option_ids ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    AttributeOption ao = new AttributeOption {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                        option_name = dataReader["option_name"].ToString(),
                        option_value = dataReader["option_value"].ToString(),
                    };
                    list.Add(ao);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product attributes from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_with_ext">Get with Extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductAttribute> GetProductAttributes(int _customer_id, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_attributes WHERE customer_id=@customer_id";
                List<ProductAttribute> list = new List<ProductAttribute>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductAttribute pa = new ProductAttribute {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                        type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString()),
                        value = dataReader["value"].ToString(),
                        option_ids = dataReader["option_ids"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(pa);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (_with_ext) {
                    foreach (var item in list) {
                        var attr = GetAttribute(_customer_id, item.attribute_id);
                        if (attr != null) {
                            item.attribute = attr;
                        }
                        else {
                            OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Not Found");
                            return null;
                        }

                        if (item.option_ids != null && item.option_ids.Length > 0) {
                            var options = GetAttributeOptions(_customer_id, item.option_ids);
                            if (options != null && options.Count > 0) {
                                item.options = [.. options];
                            }
                            else {
                                OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Options Not Found");
                                return null;
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product attributes from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <param name="_with_ext">Get with Extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ProductAttribute> GetProductAttributes(int _customer_id, int _product_id, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM product_attributes WHERE product_id=@product_id AND customer_id=@customer_id";
                List<ProductAttribute> list = new List<ProductAttribute>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    ProductAttribute pa = new ProductAttribute {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                        type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString()),
                        value = dataReader["value"].ToString(),
                        option_ids = dataReader["option_ids"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(pa);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (_with_ext) {
                    foreach (var item in list) {
                        var attr = GetAttribute(_customer_id, item.attribute_id);
                        if (attr != null) {
                            item.attribute = attr;
                        }
                        else {
                            OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Not Found");
                            return null;
                        }

                        if (item.option_ids != null && item.option_ids.Length > 0) {
                            var options = GetAttributeOptions(_customer_id, item.option_ids);
                            if (options != null && options.Count > 0) {
                                item.options = [.. options];
                            }
                            else {
                                OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Options Not Found");
                                return null;
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the product attributes in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attrs">Product Attributes</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateProductAttributes(int _customer_id, List<ProductAttribute> _attrs, int _product_id) {
            try {
                UInt64 val = 0;
                foreach (var item in _attrs) {
                    if (item.id > 0) {
                        string _query = "UPDATE product_attributes SET product_id=@product_id,value=@value,type=@type,option_ids=@option_ids,update_date=@update_date WHERE sku=@sku AND attribute_id=@attribute_id AND customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", item.product_id));
                        cmd.Parameters.Add(new MySqlParameter("attribute_id", item.attribute_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("type", item.type));
                        cmd.Parameters.Add(new MySqlParameter("value", item.value));
                        cmd.Parameters.Add(new MySqlParameter("option_ids", item.option_ids));
                        cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                        if (state != System.Data.ConnectionState.Open) connection.Open();
                        val += (UInt64)cmd.ExecuteNonQuery();
                        if (state == System.Data.ConnectionState.Open) connection.Close();
                    }
                    else {
                        string _query = "START TRANSACTION;" +
                            "INSERT INTO product_attributes (customer_id,product_id,sku,attribute_id,type,value,option_ids) VALUES (@customer_id,@product_id,@sku,@attribute_id,@type,@value,@option_ids);" +
                            "SELECT LAST_INSERT_ID();" +
                            "COMMIT;";
                        MySqlCommand cmd = new MySqlCommand(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                        cmd.Parameters.Add(new MySqlParameter("attribute_id", item.attribute_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("type", (int)item.type));
                        cmd.Parameters.Add(new MySqlParameter("value", item.value));
                        cmd.Parameters.Add(new MySqlParameter("option_ids", item.option_ids));
                        if (state != System.Data.ConnectionState.Open) connection.Open();
                        val += (UInt64)cmd.ExecuteScalar();
                        if (state == System.Data.ConnectionState.Open) connection.Close();
                    }
                }
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Brand
        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Brand ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Brand? GetBrand(int _customer_id, int _id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM brands " +
                    "WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if (dataReader.Read()) {
                    b = new Brand();
                    b.id = Convert.ToInt32(dataReader["id"].ToString());
                    b.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    b.brand_name = dataReader["brand_name"].ToString();
                    b.status = dataReader["status"] != null ? dataReader["status"].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return b;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the default brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public Brand GetDefaultBrand(int _customer_id) {
            try {
                if (Helper.global == null) return null;
                var default_brand = GetBrandByName(_customer_id, Helper.global.product.default_brand);
                if (default_brand == null) {
                    var inserted_default_brand = InsertBrand(_customer_id, new Brand() { customer_id = _customer_id, brand_name = Helper.global.product.default_brand, status = true });
                    if (inserted_default_brand != null) {
                        return inserted_default_brand;
                    }
                    else {
                        return new Brand() { customer_id = _customer_id, brand_name = Helper.global.product.default_brand, status = true };
                    }
                }
                else {
                    return default_brand;
                }
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brands from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Brand> GetBrands(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM brands WHERE customer_id=@customer_id";
                List<Brand> list = new List<Brand>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Brand b = new Brand {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        brand_name = dataReader["brand_name"].ToString(),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                    };
                    list.Add(b);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brands from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Brand> GetBrands(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };
                string _query = "SELECT * FROM brands WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                if (_filters.Sort != null)
                    _query += " ORDER BY " + _filters.Sort.Field + " " + _filters.Sort.Direction + " LIMIT @start,@end;";
                else {
                    _filters.Sort = new Sort() { Field = "id", Direction = "DESC" };
                    _query += " ORDER BY id DESC LIMIT @start,@end;";
                }
                List<Brand> list = new List<Brand>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _filters.Pager.ItemsPerPage * _filters.Pager.CurrentPageIndex));
                cmd.Parameters.Add(new MySqlParameter("end", _filters.Pager.ItemsPerPage));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Brand b = new Brand {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        brand_name = dataReader["brand_name"].ToString(),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                    };
                    list.Add(b);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Brand? GetBrand(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT b.id AS 'BID', b.brand_name,b.status AS 'brand_status',b.customer_id AS 'customer' FROM products_ext AS pex INNER JOIN brands AS b ON pex.brand_id=b.id " +
                    "WHERE pex.sku=@sku AND b.customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if (dataReader.Read()) {
                    b = new Brand();
                    b.id = Convert.ToInt32(dataReader["BID"].ToString());
                    b.customer_id = Convert.ToInt32(dataReader["customer"].ToString());
                    b.brand_name = dataReader["brand_name"].ToString();
                    b.status = dataReader["brand_status"] != null ? dataReader["brand_status"].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return b;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_name">Brand Name</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Brand? GetBrandByName(int _customer_id, string _name) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM brands " +
                    "WHERE LOWER(brand_name)=LOWER(@brand_name) AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_name", _name));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if (dataReader.Read()) {
                    b = new Brand();
                    b.id = Convert.ToInt32(dataReader["id"].ToString());
                    b.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    b.brand_name = dataReader["brand_name"].ToString();
                    b.status = dataReader["status"] != null ? dataReader["status"].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                return b;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the brand to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand">Brand</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Brand? InsertBrand(int _customer_id, Brand _brand) {
            try {
                object val; int inserted_id;
                var existed_brand = GetBrandByName(_customer_id, _brand.brand_name);
                if (existed_brand != null) {
                    return existed_brand;
                }
                string _query = "START TRANSACTION;" +
                    "INSERT INTO brands (customer_id,brand_name,status) VALUES (@customer_id,@brand_name,@status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_name", _brand.brand_name));
                cmd.Parameters.Add(new MySqlParameter("status", _brand.status));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out inserted_id)) {
                        return GetBrand(_customer_id, inserted_id);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the brand to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand">Brand</param>
        /// <param name="return_bid">Returning Brand ID</param>
        /// <returns>Error returns 'int:0'</returns>
        public int InsertBrand(int _customer_id, Brand _brand, bool return_bid) {
            try {
                var existed_brand = GetBrandByName(_customer_id, _brand.brand_name);
                if (existed_brand != null) {
                    return existed_brand.id;
                }
                object val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO brands (customer_id,brand_name,status) VALUES (@customer_id,@brand_name,@status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_name", _brand.brand_name));
                cmd.Parameters.Add(new MySqlParameter("status", _brand.status));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out int BID))
                        return BID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Updates the brand in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand">Brand</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public Brand? UpdateBrand(int _customer_id, Brand _brand) {
            try {
                var existed_brand = GetBrandByName(_customer_id, _brand.brand_name);
                if (existed_brand != null && existed_brand.id != _brand.id) {
                    return null;
                }
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "UPDATE brands SET brand_name=@brand_name,status=@status " +
                    "WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _brand.id));
                cmd.Parameters.Add(new MySqlParameter("brand_name", _brand.brand_name));
                cmd.Parameters.Add(new MySqlParameter("status", _brand.status));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return _brand;
                else return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand_id">Brand ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteBrand(int _customer_id, int _brand_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "DELETE FROM brands WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _brand_id));
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets brand count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetBrandsCount(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM brands WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets brand count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetBrandsCount(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM brands WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Category
        /// <summary>
        /// Gets the root category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_parent_id">Send '1' for system_category_root_id</param>
        /// <returns>No data and Error returns 'null'</returns>
        public Category GetRootCategory(int _customer_id, int _parent_id = 1) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM categories WHERE parent_id=@parent_id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("parent_id", _parent_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Category c = null;
                if (dataReader.Read()) {
                    c = new Category {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                        category_name = dataReader["category_name"].ToString(),
                        is_active = dataReader["is_active"] != null && dataReader["is_active"].ToString() == "1",
                        source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                    };
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                //TODO: Check for want to insert a new root category
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Category> GetProductCategories(int _customer_id, string _sku) {
            try {
                List<Category> categories = [];
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string? category_ids = string.Empty;
                {
                    string _query = "SELECT category_ids FROM products_ext " +
                      "WHERE customer_id=@customer_id AND sku=@sku";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    category_ids = cmd.ExecuteScalar().ToString();
                }

                if (category_ids != null) {
                    string _query = string.Format("SELECT * FROM categories " +
                        "WHERE customer_id=@customer_id AND id IN ({0})", category_ids);
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {
                        Category c = new() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] != null && (dataReader["is_active"].ToString() == "1"),
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                        categories.Add(c);
                    }
                    dataReader.Close();

                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                return categories;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Category> GetCategories(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM categories " +
                    "WHERE customer_id=@customer_id ";
                List<Category> categories = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Category c = new() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                        category_name = dataReader["category_name"].ToString(),
                        is_active = dataReader["is_active"] != null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
                        source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                    };
                    categories.Add(c);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                return categories;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Category> GetCategories(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };
                string _query = "SELECT * FROM categories WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                if (_filters.Sort != null)
                    _query += " ORDER BY " + _filters.Sort.Field + " " + _filters.Sort.Direction + " LIMIT @start,@end;";
                else {
                    _filters.Sort = new Sort() { Field = "id", Direction = "DESC" };
                    _query += " ORDER BY id DESC LIMIT @start,@end;";
                }
                List<Category> categories = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("start", _filters.Pager.ItemsPerPage * _filters.Pager.CurrentPageIndex));
                cmd.Parameters.Add(new MySqlParameter("end", _filters.Pager.ItemsPerPage));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Category c = new() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                        category_name = dataReader["category_name"].ToString(),
                        is_active = dataReader["is_active"] != null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
                        source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                    };
                    categories.Add(c);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                return categories;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Category ID</param>
        /// <returns>No data and Error returns 'null'</returns>
        public Category? GetCategory(int _customer_id, int _id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM categories " +
                    "WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Category? c = null;
                if (dataReader.Read()) {
                    c = new Category {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                        category_name = dataReader["category_name"].ToString(),
                        is_active = dataReader["is_active"] != null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
                        source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                    };
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Category Name</param>
        /// <returns>No data and Error returns 'null'</returns>
        public Category? GetCategoryByName(int _customer_id, string _category_name) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM categories " +
                    "WHERE category_name=@category_name AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("category_name", _category_name));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Category? c = null;
                if (dataReader.Read()) {
                    c = new Category();
                    c.id = Convert.ToInt32(dataReader["id"].ToString());
                    c.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    c.parent_id = Convert.ToInt32(dataReader["parent_id"].ToString());
                    c.category_name = dataReader["category_name"].ToString();
                    c.is_active = dataReader["is_active"] != null ? dataReader["is_active"].ToString() == "1" ? true : false : false;
                    c.source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString());
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                return c;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the category to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Category? InsertCategory(int _customer_id, Category _category) {
            try {
                object val; int inserted_id;
                var existed_category = GetCategoryByName(_customer_id, _category.category_name);
                if (existed_category != null && existed_category.parent_id == _category.parent_id) {
                    return null;
                }
                string _query = "START TRANSACTION;" +
                    "INSERT INTO categories (customer_id,parent_id,category_name,is_active,source_category_id) VALUES (@customer_id,@parent_id,@category_name,@is_active,@source_category_id);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("parent_id", _category.parent_id));
                cmd.Parameters.Add(new MySqlParameter("category_name", _category.category_name));
                cmd.Parameters.Add(new MySqlParameter("is_active", _category.is_active));
                cmd.Parameters.Add(new MySqlParameter("source_category_id", _category.source_category_id));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                val = cmd.ExecuteScalar();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val != null) {
                    if (int.TryParse(val.ToString(), out inserted_id)) {
                        return GetCategory(_customer_id, inserted_id);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the category in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public Category? UpdateCategory(int _customer_id, Category _category) {
            try {
                //var existed_category = GetCategoryByName(_customer_id, _category.category_name);
                //if (existed_category != null && existed_category.parent_id == _category.parent_id) {
                //    return null;
                //}
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "UPDATE categories SET category_name=@category_name,is_active=@is_active,parent_id=@parent_id,source_category_id=@source_category_id " +
                    "WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _category.id));
                cmd.Parameters.Add(new MySqlParameter("parent_id", _category.parent_id));
                cmd.Parameters.Add(new MySqlParameter("category_name", _category.category_name));
                cmd.Parameters.Add(new MySqlParameter("is_active", _category.is_active));
                cmd.Parameters.Add(new MySqlParameter("source_category_id", _category.source_category_id));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return _category;
                else return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteCategory(int _customer_id, int _category_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = 0;
                string _query = "DELETE FROM categories WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _category_id));
                val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets category count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetCategoryCount(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM categories WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets category count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public int GetCategoryCount(int _customer_id, ApiFilter _filters) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT COUNT(*) FROM categories WHERE customer_id=@customer_id";
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is null)
                            _query += $" AND {filter.Field} {filter.Operator} NULL";
                        else
                            _query += $" AND {filter.Field} {filter.Operator} @{filter.Field}";
                    }
                }
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (_filters.Filters != null && _filters.Filters.Count > 0) {
                    foreach (var filter in _filters.Filters) {
                        if (filter.Value is not null)
                            cmd.Parameters.Add(new MySqlParameter(filter.Field, filter.Value));
                    }
                }
                int.TryParse(cmd.ExecuteScalar().ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #endregion

        #region XML Source
        /// <summary>
        /// Gets the XML Products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<XProduct> GetXProducts(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE customer_id=@customer_id";
                List<XProduct> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    XProduct xp = new XProduct {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        barcode = dataReader["barcode"].ToString(),
                        qty = Convert.ToInt32(dataReader["qty"].ToString()),
                        source_sku = dataReader["source_sku"].ToString(),
                        source_brand = dataReader["source_brand"].ToString(),
                        source_product_group = dataReader["source_product_group"].ToString(),
                        xml_source = dataReader["xml_source"].ToString(),
                        price1 = decimal.Parse(dataReader["price1"].ToString()),
                        price2 = decimal.Parse(dataReader["price2"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        is_infosent = Convert.ToBoolean(Convert.ToInt32(dataReader["is_infosent"].ToString())),
                        is_active = Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString()))
                    };

                    list.Add(xp);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the XML Products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_xml_source">XML Source</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<XProduct> GetXProducts(int _customer_id, string _xml_source) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE xml_source=@xml_source AND customer_id=@customer_id";
                List<XProduct> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("xml_source", _xml_source));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    XProduct xp = new XProduct {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        barcode = dataReader["barcode"].ToString(),
                        qty = Convert.ToInt32(dataReader["qty"].ToString()),
                        source_sku = dataReader["source_sku"].ToString(),
                        source_brand = dataReader["source_brand"].ToString(),
                        source_product_group = dataReader["source_product_group"].ToString(),
                        xml_source = dataReader["xml_source"].ToString(),
                        price1 = decimal.Parse(dataReader["price1"].ToString()),
                        price2 = decimal.Parse(dataReader["price2"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        is_infosent = Convert.ToBoolean(Convert.ToInt32(dataReader["is_infosent"].ToString())),
                        is_active = Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString()))
                    };

                    list.Add(xp);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the XML Product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_barcode">Barcode</param>
        /// <returns>No data and Error returns 'null'</returns>
        public XProduct? GetXProductByBarcode(int _customer_id, string _barcode) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE barcode=@barcode AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("barcode", _barcode));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                XProduct? xp = null;
                if (dataReader.Read()) {
                    xp = new XProduct {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        barcode = dataReader["barcode"].ToString(),
                        qty = Convert.ToInt32(dataReader["qty"].ToString()),
                        source_sku = dataReader["source_sku"].ToString(),
                        source_brand = dataReader["source_brand"].ToString(),
                        source_product_group = dataReader["source_product_group"].ToString(),
                        xml_source = dataReader["xml_source"].ToString(),
                        price1 = decimal.Parse(dataReader["price1"].ToString()),
                        price2 = decimal.Parse(dataReader["price2"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        is_infosent = Convert.ToBoolean(Convert.ToInt32(dataReader["is_infosent"].ToString())),
                        is_active = Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString()))
                    };
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return xp;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the XML Products to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_products">XML Products</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertXProducts(int _customer_id, List<XProduct> _products) {
            try {
                int val = 0;
                foreach (XProduct item in _products) {
                    string _query = "INSERT INTO xml_products (customer_id,barcode,source_sku,source_brand,source_product_group,xml_source,qty,price1,price2,currency) VALUES " +
                        "(@customer_id,@barcode,@source_sku,@source_brand,@source_product_group,@xml_source,@qty,@price1,@price2,@currency)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("source_sku", item.source_sku));
                    cmd.Parameters.Add(new MySqlParameter("source_brand", item.source_brand));
                    cmd.Parameters.Add(new MySqlParameter("source_product_group", item.source_product_group));
                    cmd.Parameters.Add(new MySqlParameter("xml_source", item.xml_source));
                    cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                    cmd.Parameters.Add(new MySqlParameter("price1", item.price1));
                    cmd.Parameters.Add(new MySqlParameter("price2", item.price2));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    //cmd.Parameters.Add( new MySqlParameter( "is_active", item.is_active ) );
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the XML Products in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_products">XML Products</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateXProducts(int _customer_id, List<XProduct> _products) {
            try {
                int val = 0;
                foreach (XProduct item in _products) {
                    string _query = "UPDATE xml_products SET source_sku=@source_sku,source_brand=@source_brand,source_product_group=@source_product_group,xml_source=@xml_source,qty=@qty,price1=@price1,price2=@price2,currency=@currency,is_infosent=@is_infosent,is_active=@is_active,update_date=@update_date WHERE id=@id AND barcode=@barcode AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("source_sku", item.source_sku));
                    cmd.Parameters.Add(new MySqlParameter("source_brand", item.source_brand));
                    cmd.Parameters.Add(new MySqlParameter("source_product_group", item.source_product_group));
                    cmd.Parameters.Add(new MySqlParameter("xml_source", item.xml_source));
                    cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                    cmd.Parameters.Add(new MySqlParameter("price1", item.price1));
                    cmd.Parameters.Add(new MySqlParameter("price2", item.price2));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("is_infosent", item.is_infosent));
                    cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the XML Product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">XML Product ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteXProduct(int _customer_id, int _id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM xml_products WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("id", _id));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Shipment
        /// <summary>
        /// Gets the shipments from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Shipment> GetShipments(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM shipments WHERE customer_id=@customer_id";
                List<Shipment> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Shipment s = new Shipment {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        order_label = dataReader["order_label"].ToString(),
                        shipment_method = dataReader["shipment_method"].ToString(),
                        order_source = dataReader["order_source"].ToString(),
                        barcode = dataReader["barcode"].ToString(),
                        is_shipped = dataReader["is_shipped"] != null ? dataReader["is_shipped"].ToString() == "1" ? true : false : false,
                        tracking_number = dataReader["tracking_number"].ToString(),
                        order_date = Convert.ToDateTime(dataReader["order_date"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        shipment_date = !string.IsNullOrWhiteSpace(dataReader["shipment_date"].ToString()) ? Convert.ToDateTime(dataReader["shipment_date"].ToString()) : null,
                    };
                    list.Add(s);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the shipment from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        public Shipment? GetShipment(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string _query = "SELECT * FROM shipments WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Shipment? s = null;
                if (dataReader.Read()) {
                    s = new Shipment();
                    s.id = Convert.ToInt32(dataReader["id"].ToString());
                    s.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    s.order_id = Convert.ToInt32(dataReader["order_id"].ToString());
                    s.order_label = dataReader["order_label"].ToString();
                    s.shipment_method = dataReader["shipment_method"].ToString();
                    s.order_source = dataReader["order_source"].ToString();
                    s.barcode = dataReader["barcode"].ToString();
                    s.is_shipped = dataReader["is_shipped"] != null ? dataReader["is_shipped"].ToString() == "1" ? true : false : false;
                    s.tracking_number = dataReader["tracking_number"].ToString();
                    s.order_date = Convert.ToDateTime(dataReader["order_date"].ToString());
                    s.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                    s.shipment_date = !string.IsNullOrWhiteSpace(dataReader["shipment_date"].ToString()) ? Convert.ToDateTime(dataReader["shipment_date"].ToString()) : null;
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return s;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the shipments to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_shipments">Shipments</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertShipments(int _customer_id, List<Shipment> _shipments) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                foreach (Shipment item in _shipments) {
                    string _query = "INSERT INTO shipments (customer_id,order_id,order_label,order_source,barcode,is_shipped,tracking_number,shipment_date,order_date,shipment_method) VALUES (@customer_id,@order_id,@order_label,@order_source,@barcode,@is_shipped,@tracking_number,@shipment_date,@order_date,@shipment_method)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("order_source", item.order_source));
                    cmd.Parameters.Add(new MySqlParameter("shipment_method", item.shipment_method));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("is_shipped", item.is_shipped));
                    cmd.Parameters.Add(new MySqlParameter("tracking_number", item.tracking_number));
                    cmd.Parameters.Add(new MySqlParameter("shipment_date", item.shipment_date));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    val += cmd.ExecuteNonQuery();
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the shipments in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_shipments">Shipments</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateShipments(int _customer_id, List<Shipment> _shipments) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                foreach (Shipment item in _shipments) {
                    string _query = "UPDATE shipments SET order_label=@order_label,order_source=@order_source,barcode=@barcode,is_shipped=@is_shipped,tracking_number=@tracking_number,shipment_date=@shipment_date,order_date=@order_date,update_date=@update_date,shipment_method=@shipment_method WHERE order_id=@order_id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("order_source", item.order_source));
                    cmd.Parameters.Add(new MySqlParameter("shipment_method", item.shipment_method));
                    cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                    cmd.Parameters.Add(new MySqlParameter("is_shipped", item.is_shipped));
                    cmd.Parameters.Add(new MySqlParameter("tracking_number", item.tracking_number));
                    cmd.Parameters.Add(new MySqlParameter("shipment_date", item.shipment_date));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    val += cmd.ExecuteNonQuery();
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the shipment as shipped in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_label">Order Label</param>
        /// <param name="_tracking_numbers">Tracking Numbers</param>
        /// <param name="_value">True or False</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetShipped(int _customer_id, string _order_label, string _tracking_numbers, bool _value = true) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE shipments SET is_shipped=" + (_value ? "1" : "0") + ",shipment_date=@shipment_date,tracking_number=@tracking_number WHERE order_label=@order_label AND customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("shipment_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("tracking_number", _tracking_numbers));
                    cmd.Parameters.Add(new MySqlParameter("order_label", _order_label));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Invoice
        /// <summary>
        /// Gets the invoices from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_with_ext">Get with extension</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Invoice> GetInvoices(int _customer_id, bool _with_ext = true) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM invoices WHERE customer_id=@customer_id";
                List<Invoice> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Invoice inv = new Invoice {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = dataReader["order_id"].ToString(),
                        order_label = dataReader["order_label"].ToString(),
                        erp_customer_code = dataReader["erp_customer_code"].ToString(),
                        erp_customer_group = dataReader["erp_customer_group"].ToString(),
                        is_belge_created = dataReader["is_belge_created"] != null && dataReader["is_belge_created"].ToString() == "1",
                        erp_no = dataReader["erp_no"].ToString(),
                        invoice_no = dataReader["invoice_no"].ToString(),
                        belge_url = dataReader["belge_url"].ToString(),
                        gib_fatura_no = dataReader["gib_fatura_no"].ToString(),
                        order_date = Convert.ToDateTime(dataReader["order_date"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(inv);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (_with_ext) {
                    var items = GetInvoiceItems(_customer_id);
                    if (items != null) {
                        foreach (var item in list) {
                            item.items = items.Where(x => x.erp_no == item.erp_no).ToList();
                        }
                    }
                    else {
                        OnError("GetInvoices: Invoice Items Not Found");
                        return null;
                    }
                }
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the invoice from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_erp_no">ERP No</param>
        /// <returns>No data and Error returns 'null'</returns>
        public Invoice? GetInvoice(int _customer_id, string _erp_no) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM invoices WHERE erp_no=@erp_no AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _erp_no));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Invoice? inv = null;
                if (dataReader.Read()) {
                    inv = new Invoice();
                    inv.id = Convert.ToInt32(dataReader["id"].ToString());
                    inv.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    inv.order_id = dataReader["order_id"].ToString();
                    inv.order_label = dataReader["order_label"].ToString();
                    inv.is_belge_created = dataReader["is_belge_created"] != null && (dataReader["is_belge_created"].ToString() == "1");
                    inv.erp_customer_code = dataReader["erp_customer_code"].ToString();
                    inv.erp_customer_group = dataReader["erp_customer_group"].ToString();
                    inv.erp_no = dataReader["erp_no"].ToString();
                    inv.invoice_no = dataReader["invoice_no"].ToString();
                    inv.belge_url = dataReader["belge_url"].ToString();
                    inv.gib_fatura_no = dataReader["gib_fatura_no"].ToString();
                    inv.order_date = Convert.ToDateTime(dataReader["order_date"].ToString());
                    inv.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (inv != null) {
                    var i = GetInvoiceItems(_customer_id, _erp_no);
                    if (i != null)
                        inv.items = i;
                    else {
                        OnError("GetInvoice: " + inv.erp_no + " - Invoice Items Not Found");
                        return null;
                    }
                }
                return inv;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the invoices to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_invoices">Invoices</param>
        /// <param name="_with_items">Insert with items</param>
        /// <returns>[Error] returns 'false'</returns>
        public bool InsertInvoices(int _customer_id, List<Invoice> _invoices, bool _with_items = true) {
            try {
                int val = 0;
                foreach (Invoice item in _invoices) {
                    string _query = "INSERT INTO invoices (customer_id,order_id,order_label,erp_customer_code,erp_customer_group,erp_no,invoice_no,gib_fatura_no,order_date) VALUES " +
                        "(@customer_id,@order_id,@order_label,@erp_customer_code,@erp_customer_group,@erp_no,@invoice_no,@gib_fatura_no,@order_date)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("erp_customer_code", item.erp_customer_code));
                    cmd.Parameters.Add(new MySqlParameter("erp_customer_group", item.erp_customer_group));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", item.invoice_no));
                    cmd.Parameters.Add(new MySqlParameter("gib_fatura_no", item.gib_fatura_no));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (_with_items) {
                        if (item.items != null && item.items.Count > 0) {
                            if (!string.IsNullOrWhiteSpace(item.erp_no)) {
                                if (!InsertInvoiceItems(_customer_id, item.items)) {
                                    OnError("InsertInvoices: " + item.erp_no + " Invoice Items Not Inserted");
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the invoices in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_invoices">Invoices</param>
        /// <param name="_with_ext">Update with extension</param>
        /// <returns>[Error] returns 'false'</returns>
        public bool UpdateInvoices(int _customer_id, List<Invoice> _invoices, bool _with_ext = true) {
            try {
                int val = 0;
                foreach (Invoice item in _invoices) {
                    string _query = "UPDATE invoices SET order_label=@order_label,erp_customer_code=@erp_customer_code,erp_customer_group=@erp_customer_group,invoice_no=@invoice_no,gib_fatura_no=@gib_fatura_no,order_date=@order_date,update_date=@update_date WHERE id=@id AND erp_no=@erp_no AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("erp_customer_code", item.erp_customer_code));
                    cmd.Parameters.Add(new MySqlParameter("erp_customer_group", item.erp_customer_group));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", item.invoice_no));
                    cmd.Parameters.Add(new MySqlParameter("gib_fatura_no", item.gib_fatura_no));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (_with_ext) {
                        if (item.items != null && item.items.Count > 0) {
                            if (!UpdateInvoiceItems(_customer_id, item.items)) {
                                OnError("UpdateInvoices: " + item.erp_no + " Invoice Items Not Updated");
                                return false;
                            }
                        }
                    }
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the invoice items from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<InvoiceItem> GetInvoiceItems(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM invoice_items WHERE customer_id=@customer_id";
                List<InvoiceItem> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    InvoiceItem ii = new InvoiceItem {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        invoice_no = dataReader["invoice_no"].ToString(),
                        erp_no = dataReader["erp_no"].ToString(),
                        sku = dataReader["sku"].ToString(),
                        qty = Convert.ToInt32(dataReader["qty"].ToString()),
                        serials = dataReader["serials"].ToString().Split(','),
                        create_date = Convert.ToDateTime(dataReader["create_date"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(ii);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the invoice items from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_erp_no">ERP No</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<InvoiceItem> GetInvoiceItems(int _customer_id, string _erp_no) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM invoice_items WHERE erp_no=@erp_no AND customer_id=@customer_id";
                List<InvoiceItem> list = new List<InvoiceItem>();
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("erp_no", _erp_no));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    InvoiceItem ii = new InvoiceItem {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        invoice_no = dataReader["invoice_no"].ToString(),
                        erp_no = dataReader["erp_no"].ToString(),
                        sku = dataReader["sku"].ToString(),
                        qty = Convert.ToInt32(dataReader["qty"].ToString()),
                        serials = dataReader["xml_sources"].ToString().Split(','),
                        create_date = Convert.ToDateTime(dataReader["create_date"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                    };
                    list.Add(ii);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the invoice items to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_items">Invoice Items</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertInvoiceItems(int _customer_id, List<InvoiceItem> _items) {
            try {
                int val = 0;
                foreach (var item in _items) {
                    string _query = "INSERT INTO invoice_items (customer_id,invoice_no,erp_no,sku,qty,serials) VALUES " +
                        "(@customer_id,@invoice_no,@erp_no,@sku,@qty,@serials)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", item.invoice_no));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                    cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                    cmd.Parameters.Add(new MySqlParameter("serials", string.Join(",", item.serials)));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val = cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the invoice items in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_items">Invoice Items</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateInvoiceItems(int _customer_id, List<InvoiceItem> _items) {
            try {
                int val = 0;
                foreach (InvoiceItem item in _items) {
                    string _query = "UPDATE invoice_items SET invoice_no=@invoice_no,sku=@sku,qty=@qty,serials=@serials,update_date=@update_date " +
                        "WHERE id=@id AND erp_no=@erp_no AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("id", item.id));
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", item.invoice_no));
                    cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                    cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                    cmd.Parameters.Add(new MySqlParameter("serials", string.Join(",", item.serials)));
                    cmd.Parameters.Add(new MySqlParameter("update_date", item.update_date));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the invoice as created in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_invoice_no">Invoice No</param>
        /// <param name="_fullpath">Full Path</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetInvoiceCreated(int _customer_id, string _invoice_no, string _fullpath = "") {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string query = "UPDATE invoices SET is_belge_created=1,belge_url=@belge_url WHERE invoice_no=@invoice_no AND customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", _invoice_no));
                    cmd.Parameters.Add(new MySqlParameter("belge_url", _fullpath));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Order
        /// <summary>
        /// Gets the orders from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Order> GetOrders(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM orders WHERE customer_id=@customer_id";
                List<Order> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Order o = new Order {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        email = dataReader["email"].ToString(),
                        firstname = dataReader["firstname"].ToString(),
                        lastname = dataReader["lastname"].ToString(),
                        order_label = dataReader["order_label"].ToString(),
                        order_source = dataReader["order_source"].ToString(),
                        payment_method = dataReader["payment_method"].ToString(),
                        shipment_method = dataReader["shipment_method"].ToString(),
                        comment = dataReader["comment"].ToString(),
                        order_shipping_barcode = dataReader["order_shipping_barcode"].ToString(),
                        erp_no = dataReader["erp_no"] != null ? dataReader["erp_no"].ToString() : null,
                        is_erp_sent = dataReader["is_erp_sent"] != null ? dataReader["is_erp_sent"].ToString().Equals("1") : false,
                        grand_total = float.Parse(dataReader["grand_total"].ToString()),
                        subtotal = float.Parse(dataReader["subtotal"].ToString()),
                        discount_amount = float.Parse(dataReader["discount_amount"].ToString()),
                        installment_amount = float.Parse(dataReader["installment_amount"].ToString()),
                        shipment_amount = float.Parse(dataReader["shipment_amount"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        order_status = dataReader["order_status"].ToString(),
                        order_date = !string.IsNullOrWhiteSpace(dataReader["order_date"].ToString()) ? Convert.ToDateTime(dataReader["order_date"].ToString()) : DateTime.Now,
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(o);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                var order_items = GetOrderItems(_customer_id);
                if (order_items != null) {
                    foreach (var item in list) {
                        item.order_items = order_items.Where(x => x.order_id == item.order_id).ToList();
                    }
                }
                else {
                    OnError("GetOrders: Order Items Not Found");
                    return null;
                }

                var billing_items = GetBillingAddresses(_customer_id);
                if (billing_items != null) {
                    foreach (var item in list) {
                        var bi = billing_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (bi != null) {
                            item.billing_address = bi;
                        }
                        else {
                            OnError("GetOrders: " + item.order_label + " Billing Address Not Found");
                            return null;
                        }
                    }
                }
                else {
                    OnError("GetOrders: Billing Addresses Not Found");
                    return null;
                }

                var shipping_items = GetShippingAddresses(_customer_id);
                if (shipping_items != null) {
                    foreach (var item in list) {
                        var si = shipping_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (si != null) {
                            item.shipping_address = si;
                        }
                        else {
                            OnError("GetOrders: " + item.order_label + " Shipping Address Not Found");
                            return null;
                        }
                    }
                }
                else {
                    OnError("GetOrders: Shipping Addresses Not Found");
                    return null;
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the orders from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_statuses">Order Statuses</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Order> GetOrders(int _customer_id, string[] _order_statuses) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM orders WHERE customer_id=@customer_id";
                string _query_ext = "order_status IN (";
                if (_order_statuses.Length > 0) {
                    foreach (var item in _order_statuses) {
                        _query_ext += "'" + item + "',";
                    }
                    _query_ext = _query_ext.Remove(_query_ext.Length - 1, 1) + ")";
                    _query = "SELECT * FROM orders WHERE " + _query_ext + " AND customer_id=@customer_id";
                }
                List<Order> list = [];
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    Order o = new Order {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        email = dataReader["email"].ToString(),
                        firstname = dataReader["firstname"].ToString(),
                        lastname = dataReader["lastname"].ToString(),
                        order_label = dataReader["order_label"].ToString(),
                        order_source = dataReader["order_source"].ToString(),
                        payment_method = dataReader["payment_method"].ToString(),
                        shipment_method = dataReader["shipment_method"].ToString(),
                        comment = dataReader["comment"].ToString(),
                        order_shipping_barcode = dataReader["order_shipping_barcode"].ToString(),
                        erp_no = dataReader["erp_no"] != null ? dataReader["erp_no"].ToString() : null,
                        is_erp_sent = dataReader["is_erp_sent"] != null ? dataReader["is_erp_sent"].ToString().Equals("1") : false,
                        grand_total = float.Parse(dataReader["grand_total"].ToString()),
                        subtotal = float.Parse(dataReader["subtotal"].ToString()),
                        discount_amount = float.Parse(dataReader["discount_amount"].ToString()),
                        installment_amount = float.Parse(dataReader["installment_amount"].ToString()),
                        shipment_amount = float.Parse(dataReader["shipment_amount"].ToString()),
                        currency = dataReader["currency"].ToString(),
                        order_status = dataReader["order_status"].ToString(),
                        order_date = !string.IsNullOrWhiteSpace(dataReader["order_date"].ToString()) ? Convert.ToDateTime(dataReader["order_date"].ToString()) : DateTime.Now,
                        update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                    };
                    list.Add(o);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                var order_items = GetOrderItems(_customer_id);
                if (order_items != null) {
                    foreach (var item in list) {
                        item.order_items = order_items.Where(x => x.order_id == item.order_id).ToList();
                    }
                }
                else {
                    OnError("GetOrders: Order Items Not Found");
                    return null;
                }

                var billing_items = GetBillingAddresses(_customer_id);
                if (billing_items != null) {
                    foreach (var item in list) {
                        var bi = billing_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (bi != null) {
                            item.billing_address = bi;
                        }
                        else {
                            OnError("GetOrders: " + item.order_label + " Billing Address Not Found");
                            return null;
                        }
                    }
                }
                else {
                    OnError("GetOrders: Billing Addresses Not Found");
                    return null;
                }

                var shipping_items = GetShippingAddresses(_customer_id);
                if (shipping_items != null) {
                    foreach (var item in list) {
                        var si = shipping_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (si != null) {
                            item.shipping_address = si;
                        }
                        else {
                            OnError("GetOrders: " + item.order_label + " Shipping Address Not Found");
                            return null;
                        }
                    }
                }
                else {
                    OnError("GetOrders: Shipping Addresses Not Found");
                    return null;
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the order from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public Order? GetOrder(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM orders WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Order? o = null;
                if (dataReader.Read()) {
                    o = new Order();
                    o.id = Convert.ToInt32(dataReader["id"].ToString());
                    o.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    o.order_id = Convert.ToInt32(dataReader["order_id"].ToString());
                    o.email = dataReader["email"].ToString();
                    o.firstname = dataReader["firstname"].ToString();
                    o.lastname = dataReader["lastname"].ToString();
                    o.order_label = dataReader["order_label"].ToString();
                    o.order_source = dataReader["order_source"].ToString();
                    o.payment_method = dataReader["payment_method"].ToString();
                    o.shipment_method = dataReader["shipment_method"].ToString();
                    o.comment = dataReader["comment"].ToString();
                    o.order_shipping_barcode = dataReader["order_shipping_barcode"].ToString();
                    o.erp_no = dataReader["erp_no"] != null ? dataReader["erp_no"].ToString() : null;
                    o.is_erp_sent = dataReader["is_erp_sent"] != null ? dataReader["is_erp_sent"].ToString().Equals("1") : false;
                    o.grand_total = float.Parse(dataReader["grand_total"].ToString());
                    o.subtotal = float.Parse(dataReader["subtotal"].ToString());
                    o.discount_amount = float.Parse(dataReader["discount_amount"].ToString());
                    o.installment_amount = float.Parse(dataReader["installment_amount"].ToString());
                    o.shipment_amount = float.Parse(dataReader["shipment_amount"].ToString());
                    o.currency = dataReader["currency"].ToString();
                    o.order_status = dataReader["order_status"].ToString();
                    o.order_date = !string.IsNullOrWhiteSpace(dataReader["order_date"].ToString()) ? Convert.ToDateTime(dataReader["order_date"].ToString()) : DateTime.Now;
                    o.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (o != null) {
                    var order_items = GetOrderItems(_customer_id, _order_id);
                    if (order_items != null)
                        o.order_items = order_items;
                    else {
                        OnError("GetOrder: " + o.order_label + " Order Items Not Found");
                        return null;
                    }

                    var order_ba = GetBillingAddress(_customer_id, _order_id);
                    if (order_ba != null)
                        o.billing_address = order_ba;
                    else {
                        OnError("GetOrder: " + o.order_label + " Billing Address Not Found");
                        return null;
                    }

                    var order_sa = GetShippingAddress(_customer_id, _order_id);
                    if (order_sa != null)
                        o.shipping_address = order_sa;
                    else {
                        OnError("GetOrder: " + o.order_label + " Shipping Address Not Found");
                        return null;
                    }
                }

                return o;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the orders to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_orders">Orders</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertOrders(int _customer_id, List<Order> _orders) {
            try {
                int val = 0;
                foreach (Order item in _orders) {
                    string _query = "INSERT INTO orders (customer_id,order_id,email,firstname,lastname,order_label,order_source,payment_method,shipment_method,comment,grand_total,subtotal,discount_amount,installment_amount,shipment_amount,order_status,order_date,currency,order_shipping_barcode) VALUES (@customer_id,@order_id,@email,@firstname,@lastname,@order_label,@order_source,@payment_method,@shipment_method,@comment,@grand_total,@subtotal,@discount_amount,@installment_amount,@shipment_amount,@order_status,@order_date,@currency,@order_shipping_barcode)";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    cmd.Parameters.Add(new MySqlParameter("email", item.email));
                    cmd.Parameters.Add(new MySqlParameter("firstname", item.firstname));
                    cmd.Parameters.Add(new MySqlParameter("lastname", item.lastname));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("order_source", item.order_source));
                    cmd.Parameters.Add(new MySqlParameter("payment_method", item.payment_method));
                    cmd.Parameters.Add(new MySqlParameter("shipment_method", item.shipment_method));
                    cmd.Parameters.Add(new MySqlParameter("comment", item.comment));
                    cmd.Parameters.Add(new MySqlParameter("order_shipping_barcode", item.order_shipping_barcode));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    cmd.Parameters.Add(new MySqlParameter("is_erp_sent", item.is_erp_sent));
                    cmd.Parameters.Add(new MySqlParameter("grand_total", item.grand_total));
                    cmd.Parameters.Add(new MySqlParameter("subtotal", item.subtotal));
                    cmd.Parameters.Add(new MySqlParameter("discount_amount", item.discount_amount));
                    cmd.Parameters.Add(new MySqlParameter("installment_amount", item.installment_amount));
                    cmd.Parameters.Add(new MySqlParameter("shipment_amount", item.shipment_amount));
                    cmd.Parameters.Add(new MySqlParameter("order_status", item.order_status));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (val > 0) {
                        foreach (var order_item in item.order_items) {
                            if (!InsertOrderItem(_customer_id, order_item)) {
                                OnError("InsertOrders: " + item.order_label + " Order Item Not Inserted");
                                return false;
                            }
                        }

                        if (!InsertOrderBillingAddress(_customer_id, item.billing_address)) {
                            OnError("InsertOrders: " + item.order_label + " Billing Address Not Inserted");
                            return false;
                        }

                        if (!InsertOrderShippingAddress(_customer_id, item.shipping_address)) {
                            OnError("InsertOrders: " + item.order_label + " Shipping Address Not Inserted");
                            return false;
                        }
                    }
                    else {
                        OnError("InsertOrders: " + item.order_label + " Order Not Inserted");
                        return false;
                    }
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the orders in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_orders">Orders</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateOrders(int _customer_id, List<Order> _orders) {
            try {
                int val = 0;
                foreach (Order item in _orders) {
                    string _query = "UPDATE orders SET order_label=@order_label,email=@email,firstname=@firstname,lastname=@lastname,order_source=@order_source,payment_method=@payment_method,shipment_method=@shipment_method,comment=@comment,grand_total=@grand_total,subtotal=@subtotal,discount_amount=@discount_amount,installment_amount=@installment_amount,shipment_amount=@shipment_amount,order_status=@order_status,order_date=@order_date,update_date=@update_date,currency=@currency WHERE order_id=@order_id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("email", item.email));
                    cmd.Parameters.Add(new MySqlParameter("firstname", item.firstname));
                    cmd.Parameters.Add(new MySqlParameter("lastname", item.lastname));
                    cmd.Parameters.Add(new MySqlParameter("order_label", item.order_label));
                    cmd.Parameters.Add(new MySqlParameter("order_source", item.order_source));
                    cmd.Parameters.Add(new MySqlParameter("payment_method", item.payment_method));
                    cmd.Parameters.Add(new MySqlParameter("shipment_method", item.shipment_method));
                    cmd.Parameters.Add(new MySqlParameter("comment", item.comment));
                    //cmd.Parameters.Add(new MySqlParameter("order_shipping_barcode", item.order_shipping_barcode));
                    //cmd.Parameters.Add(new MySqlParameter("erp_no", item.erp_no));
                    //cmd.Parameters.Add(new MySqlParameter("is_erp_sent", item.is_erp_sent));
                    cmd.Parameters.Add(new MySqlParameter("grand_total", item.grand_total));
                    cmd.Parameters.Add(new MySqlParameter("subtotal", item.subtotal));
                    cmd.Parameters.Add(new MySqlParameter("discount_amount", item.discount_amount));
                    cmd.Parameters.Add(new MySqlParameter("installment_amount", item.installment_amount));
                    cmd.Parameters.Add(new MySqlParameter("shipment_amount", item.shipment_amount));
                    cmd.Parameters.Add(new MySqlParameter("order_status", item.order_status));
                    cmd.Parameters.Add(new MySqlParameter("order_date", item.order_date));
                    cmd.Parameters.Add(new MySqlParameter("order_id", item.order_id));
                    cmd.Parameters.Add(new MySqlParameter("currency", item.currency));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (val > 0) {
                        foreach (var order_item in item.order_items) {
                            if (!UpdateOrderItem(_customer_id, order_item)) {
                                OnError("UpdateOrders: " + item.order_label + " Order Item Not Updated");
                                return false;
                            }
                        }

                        if (!UpdateOrderBillingAddress(_customer_id, item.billing_address)) {
                            OnError("UpdateOrders: " + item.order_label + " Billing Address Not Updated");
                            return false;
                        }

                        if (!UpdateOrderShippingAddress(_customer_id, item.shipping_address)) {
                            OnError("UpdateOrders: " + item.order_label + " Shipping Address Not Updated");
                            return false;
                        }
                    }
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the order items from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<OrderItem> GetOrderItems(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string _query = "SELECT * FROM order_items WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<OrderItem> list = [];
                while (dataReader.Read()) {
                    OrderItem oi = new OrderItem() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        order_item_id = Convert.ToInt32(dataReader["order_item_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        parent_sku = dataReader["parent_sku"].ToString(),
                        price = float.Parse(dataReader["price"].ToString()),
                        tax_amount = float.Parse(dataReader["tax_amount"].ToString()),
                        qty_ordered = Convert.ToInt32(dataReader["qty_ordered"].ToString()),
                        qty_invoiced = Convert.ToInt32(dataReader["qty_invoiced"].ToString()),
                        qty_cancelled = Convert.ToInt32(dataReader["qty_cancelled"].ToString()),
                        qty_refunded = Convert.ToInt32(dataReader["qty_refunded"].ToString()),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = dataReader["tax_included"] != null ? dataReader["tax_included"].ToString().Equals("1") : false,
                    };
                    list.Add(oi);
                }
                dataReader.Close();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the order items from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<OrderItem> GetOrderItems(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM order_items WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<OrderItem> list = [];
                while (dataReader.Read()) {
                    OrderItem oi = new OrderItem() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        order_item_id = Convert.ToInt32(dataReader["order_item_id"].ToString()),
                        sku = dataReader["sku"].ToString(),
                        parent_sku = dataReader["parent_sku"].ToString(),
                        price = float.Parse(dataReader["price"].ToString()),
                        tax_amount = float.Parse(dataReader["tax_amount"].ToString()),
                        qty_ordered = Convert.ToInt32(dataReader["qty_ordered"].ToString()),
                        qty_invoiced = Convert.ToInt32(dataReader["qty_invoiced"].ToString()),
                        qty_cancelled = Convert.ToInt32(dataReader["qty_cancelled"].ToString()),
                        qty_refunded = Convert.ToInt32(dataReader["qty_refunded"].ToString()),
                        tax = Convert.ToInt32(dataReader["tax"].ToString()),
                        tax_included = dataReader["tax_included"] != null ? dataReader["tax_included"].ToString().Equals("1") : false,
                    };
                    list.Add(oi);
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the order item to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_item">Order Item</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertOrderItem(int _customer_id, OrderItem _order_item) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_items (customer_id,order_id,order_item_id,sku,parent_sku,price,tax_amount,qty_ordered,qty_invoiced,qty_cancelled,qty_refunded,tax,tax_included) VALUES (@customer_id,@order_id,@order_item_id,@sku,@parent_sku,@price,@tax_amount,@qty_ordered,@qty_invoiced,@qty_cancelled,@qty_refunded,@tax,@tax_included)";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_item.order_id));
                cmd.Parameters.Add(new MySqlParameter("order_item_id", _order_item.order_item_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _order_item.sku));
                cmd.Parameters.Add(new MySqlParameter("parent_sku", _order_item.parent_sku));
                cmd.Parameters.Add(new MySqlParameter("price", _order_item.price));
                cmd.Parameters.Add(new MySqlParameter("tax_amount", _order_item.tax_amount));
                cmd.Parameters.Add(new MySqlParameter("qty_ordered", _order_item.qty_ordered));
                cmd.Parameters.Add(new MySqlParameter("qty_invoiced", _order_item.qty_invoiced));
                cmd.Parameters.Add(new MySqlParameter("qty_cancelled", _order_item.qty_cancelled));
                cmd.Parameters.Add(new MySqlParameter("qty_refunded", _order_item.qty_refunded));
                cmd.Parameters.Add(new MySqlParameter("tax", _order_item.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _order_item.tax_included));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the order item in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_item">Order Item</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateOrderItem(int _customer_id, OrderItem _order_item) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "UPDATE order_items SET sku=@sku,parent_sku=@parent_sku,price=@price,tax_amount=@tax_amount,qty_ordered=@qty_ordered,qty_invoiced=@qty_invoiced,qty_cancelled=@qty_cancelled,qty_refunded=@qty_refunded,tax=@tax,tax_included=@tax_included WHERE order_item_id=@order_item_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_item.order_id));
                cmd.Parameters.Add(new MySqlParameter("order_item_id", _order_item.order_item_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _order_item.sku));
                cmd.Parameters.Add(new MySqlParameter("parent_sku", _order_item.parent_sku));
                cmd.Parameters.Add(new MySqlParameter("price", _order_item.price));
                cmd.Parameters.Add(new MySqlParameter("tax_amount", _order_item.tax_amount));
                cmd.Parameters.Add(new MySqlParameter("qty_ordered", _order_item.qty_ordered));
                cmd.Parameters.Add(new MySqlParameter("qty_invoiced", _order_item.qty_invoiced));
                cmd.Parameters.Add(new MySqlParameter("qty_cancelled", _order_item.qty_cancelled));
                cmd.Parameters.Add(new MySqlParameter("qty_refunded", _order_item.qty_refunded));
                cmd.Parameters.Add(new MySqlParameter("tax", _order_item.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _order_item.tax_included));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the order items from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool DeleteOrderItems(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "DELETE FROM order_items WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the billing addresses from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<BillingAddress> GetBillingAddresses(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM order_billing_addresses WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<BillingAddress> list = [];
                while (dataReader.Read()) {
                    BillingAddress ba = new BillingAddress() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        billing_id = Convert.ToInt32(dataReader["billing_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        firstname = dataReader["firstname"].ToString(),
                        lastname = dataReader["lastname"].ToString(),
                        telephone = dataReader["telephone"].ToString(),
                        street = dataReader["street"].ToString(),
                        region = dataReader["region"].ToString(),
                        city = dataReader["city"].ToString(),
                        is_corporate = dataReader["is_corporate"] != null ? dataReader["is_corporate"].ToString().Equals("1") : false,
                        firma_ismi = dataReader["firma_ismi"].ToString(),
                        firma_vergidairesi = dataReader["firma_vergidairesi"].ToString(),
                        firma_vergino = dataReader["firma_vergino"].ToString(),
                        tc_no = dataReader["tc_no"].ToString()
                    };
                    list.Add(ba);
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the billing address from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public BillingAddress? GetBillingAddress(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM order_billing_addresses WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                BillingAddress? ba = null;
                if (dataReader.Read()) {
                    ba = new BillingAddress();
                    ba.id = Convert.ToInt32(dataReader["id"].ToString());
                    ba.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    ba.billing_id = Convert.ToInt32(dataReader["billing_id"].ToString());
                    ba.order_id = Convert.ToInt32(dataReader["order_id"].ToString());
                    ba.firstname = dataReader["firstname"].ToString();
                    ba.lastname = dataReader["lastname"].ToString();
                    ba.telephone = dataReader["telephone"].ToString();
                    ba.street = dataReader["street"].ToString();
                    ba.region = dataReader["region"].ToString();
                    ba.city = dataReader["city"].ToString();
                    ba.is_corporate = dataReader["is_corporate"] != null ? dataReader["is_corporate"].ToString().Equals("1") : false;
                    ba.firma_ismi = dataReader["firma_ismi"].ToString();
                    ba.firma_vergidairesi = dataReader["firma_vergidairesi"].ToString();
                    ba.firma_vergino = dataReader["firma_vergino"].ToString();
                    ba.tc_no = dataReader["tc_no"].ToString();
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return ba;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the billing address to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_billing_address">Billing Address</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertOrderBillingAddress(int _customer_id, BillingAddress _billing_address) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_billing_addresses (customer_id,billing_id,order_id,firstname,lastname,telephone,street,region,city,is_corporate,firma_ismi,firma_vergidairesi,firma_vergino,tc_no) VALUES (@customer_id,@billing_id,@order_id,@firstname,@lastname,@telephone,@street,@region,@city,@is_corporate,@firma_ismi,@firma_vergidairesi,@firma_vergino,@tc_no)";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("billing_id", _billing_address.billing_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _billing_address.order_id));
                cmd.Parameters.Add(new MySqlParameter("firstname", _billing_address.firstname));
                cmd.Parameters.Add(new MySqlParameter("lastname", _billing_address.lastname));
                cmd.Parameters.Add(new MySqlParameter("telephone", _billing_address.telephone));
                cmd.Parameters.Add(new MySqlParameter("street", _billing_address.street));
                cmd.Parameters.Add(new MySqlParameter("region", _billing_address.region));
                cmd.Parameters.Add(new MySqlParameter("city", _billing_address.city));
                cmd.Parameters.Add(new MySqlParameter("is_corporate", _billing_address.is_corporate));
                cmd.Parameters.Add(new MySqlParameter("firma_ismi", _billing_address.firma_ismi));
                cmd.Parameters.Add(new MySqlParameter("firma_vergidairesi", _billing_address.firma_vergidairesi));
                cmd.Parameters.Add(new MySqlParameter("firma_vergino", _billing_address.firma_vergino));
                cmd.Parameters.Add(new MySqlParameter("tc_no", _billing_address.tc_no));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the billing address in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_billing_address">Billing Address</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateOrderBillingAddress(int _customer_id, BillingAddress _billing_address) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "UPDATE order_billing_addresses SET firstname=@firstname,lastname=@lastname,telephone=@telephone,street=@street,region=@region,city=@city,is_corporate=@is_corporate,firma_ismi=@firma_ismi,firma_vergidairesi=@firma_vergidairesi,firma_vergino=@firma_vergino,tc_no=@tc_no WHERE billing_id=@billing_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("billing_id", _billing_address.billing_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _billing_address.order_id));
                cmd.Parameters.Add(new MySqlParameter("firstname", _billing_address.firstname));
                cmd.Parameters.Add(new MySqlParameter("lastname", _billing_address.lastname));
                cmd.Parameters.Add(new MySqlParameter("telephone", _billing_address.telephone));
                cmd.Parameters.Add(new MySqlParameter("street", _billing_address.street));
                cmd.Parameters.Add(new MySqlParameter("region", _billing_address.region));
                cmd.Parameters.Add(new MySqlParameter("city", _billing_address.city));
                cmd.Parameters.Add(new MySqlParameter("is_corporate", _billing_address.is_corporate));
                cmd.Parameters.Add(new MySqlParameter("firma_ismi", _billing_address.firma_ismi));
                cmd.Parameters.Add(new MySqlParameter("firma_vergidairesi", _billing_address.firma_vergidairesi));
                cmd.Parameters.Add(new MySqlParameter("firma_vergino", _billing_address.firma_vergino));
                cmd.Parameters.Add(new MySqlParameter("tc_no", _billing_address.tc_no));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the shipping addresses from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<ShippingAddress> GetShippingAddresses(int _customer_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM order_shipping_addresses WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<ShippingAddress> list = [];
                while (dataReader.Read()) {
                    ShippingAddress sa = new ShippingAddress() {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        shipping_id = Convert.ToInt32(dataReader["shipping_id"].ToString()),
                        order_id = Convert.ToInt32(dataReader["order_id"].ToString()),
                        firstname = dataReader["firstname"].ToString(),
                        lastname = dataReader["lastname"].ToString(),
                        telephone = dataReader["telephone"].ToString(),
                        street = dataReader["street"].ToString(),
                        region = dataReader["region"].ToString(),
                        city = dataReader["city"].ToString()
                    };
                    list.Add(sa);
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the shipping address from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public ShippingAddress? GetShippingAddress(int _customer_id, int _order_id) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string _query = "SELECT * FROM order_shipping_addresses WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                ShippingAddress? sa = null;
                if (dataReader.Read()) {
                    sa = new ShippingAddress();
                    sa.id = Convert.ToInt32(dataReader["id"].ToString());
                    sa.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                    sa.shipping_id = Convert.ToInt32(dataReader["shipping_id"].ToString());
                    sa.order_id = Convert.ToInt32(dataReader["order_id"].ToString());
                    sa.firstname = dataReader["firstname"].ToString();
                    sa.lastname = dataReader["lastname"].ToString();
                    sa.telephone = dataReader["telephone"].ToString();
                    sa.street = dataReader["street"].ToString();
                    sa.region = dataReader["region"].ToString();
                    sa.city = dataReader["city"].ToString();
                }

                if (state == System.Data.ConnectionState.Open) connection.Close();

                return sa;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the shipping address to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_shipping_address">Shipping Address</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertOrderShippingAddress(int _customer_id, ShippingAddress _shipping_address) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_shipping_addresses (shipping_id,customer_id,order_id,firstname,lastname,telephone,street,region,city) VALUES (@shipping_id,@customer_id,@order_id,@firstname,@lastname,@telephone,@street,@region,@city)";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("shipping_id", _shipping_address.shipping_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _shipping_address.order_id));
                cmd.Parameters.Add(new MySqlParameter("firstname", _shipping_address.firstname));
                cmd.Parameters.Add(new MySqlParameter("lastname", _shipping_address.lastname));
                cmd.Parameters.Add(new MySqlParameter("telephone", _shipping_address.telephone));
                cmd.Parameters.Add(new MySqlParameter("street", _shipping_address.street));
                cmd.Parameters.Add(new MySqlParameter("region", _shipping_address.region));
                cmd.Parameters.Add(new MySqlParameter("city", _shipping_address.city));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the shipping address in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_shipping_address">Shipping Address</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateOrderShippingAddress(int _customer_id, ShippingAddress _shipping_address) {
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();

                int val = 0;
                string _query = "UPDATE order_shipping_addresses SET firstname=@firstname,lastname=@lastname,telephone=@telephone,street=@street,region=@region,city=@city WHERE shipping_id=@shipping_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("shipping_id", _shipping_address.shipping_id));
                cmd.Parameters.Add(new MySqlParameter("order_id", _shipping_address.order_id));
                cmd.Parameters.Add(new MySqlParameter("firstname", _shipping_address.firstname));
                cmd.Parameters.Add(new MySqlParameter("lastname", _shipping_address.lastname));
                cmd.Parameters.Add(new MySqlParameter("telephone", _shipping_address.telephone));
                cmd.Parameters.Add(new MySqlParameter("street", _shipping_address.street));
                cmd.Parameters.Add(new MySqlParameter("region", _shipping_address.region));
                cmd.Parameters.Add(new MySqlParameter("city", _shipping_address.city));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the order as processed
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <param name="_erp_no">ERP No</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetOrderProcess(int _customer_id, int _order_id, string _erp_no) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string query = "UPDATE orders SET is_erp_sent=@is_erp_sent,erp_no=@erp_no WHERE order_id=@order_id AND customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                    cmd.Parameters.Add(new MySqlParameter("is_erp_sent", !string.IsNullOrWhiteSpace(_erp_no)));
                    cmd.Parameters.Add(new MySqlParameter("erp_no", _erp_no));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the order status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order">Order</param>
        /// <param name="_status">Status</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetOrderStatus(int _customer_id, Order _order, string _status) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string query = "UPDATE orders SET order_status=@order_status WHERE order_id=@order_id AND customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", _order.order_id));
                    cmd.Parameters.Add(new MySqlParameter("order_status", _status));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();
                if (_status == "HAZIRLANIYOR") {
                    foreach (var item in _order.order_items) {
                        item.qty_invoiced = item.qty_ordered - item.qty_refunded - item.qty_cancelled;
                        UpdateOrderItem(_customer_id, item);
                    }
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the order shipment barcode
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_order_id">Order ID</param>
        /// <param name="_order_shipping_barcode">Order Shipping Barcode</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetOrderShipmentBarcode(int _customer_id, int _order_id, string _order_shipping_barcode) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open) connection.Open();

                string query = "UPDATE orders SET order_shipping_barcode=@order_shipping_barcode WHERE order_id=@order_id AND customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("order_id", _order_id));
                    cmd.Parameters.Add(new MySqlParameter("order_shipping_barcode", _order_shipping_barcode));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open) connection.Close();

                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion


        #region Is_Working Methods
        /// <summary>
        /// Sets the product sync working status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetProductSyncWorking(int _customer_id, bool _val) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET is_productsync_working=@is_productsync_working WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("is_productsync_working", _val));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the order sync working status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetOrderSyncWorking(int _customer_id, bool _val) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET is_ordersync_working=@is_ordersync_working WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("is_ordersync_working", _val));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the xml sync working status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetXmlSyncWorking(int _customer_id, bool _val) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET is_xmlsync_working=@is_xmlsync_working WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("is_xmlsync_working", _val));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the invoice sync working status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetInvoiceSyncWorking(int _customer_id, bool _val) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET is_invoicesync_working=@is_invoicesync_working WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("is_invoicesync_working", _val));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the notification sync working status
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetNotificationSyncWorking(int _customer_id, bool _val) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET is_notificationsync_working=@is_notificationsync_working WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("is_notificationsync_working", _val));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the product sync date
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetProductSyncDate(int _customer_id) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET last_product_sync_date=@last_product_sync_date WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("last_product_sync_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the order sync date
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetOrderSyncDate(int _customer_id) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET last_order_sync_date=@last_order_sync_date WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("last_order_sync_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the xml sync date
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetXmlSyncDate(int _customer_id) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET last_xml_sync_date=@last_xml_sync_date WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("last_xml_sync_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the invoice sync date
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetInvoiceSyncDate(int _customer_id) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET last_invoice_sync_date=@last_invoice_sync_date WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("last_invoice_sync_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets the notification sync date
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool SetNotificationSyncDate(int _customer_id) {
            try {
                int value = 0;
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();
                string query = "UPDATE customer SET last_notification_sync_date=@last_notification_sync_date WHERE customer_id=@customer_id";
                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("last_notification_sync_date", DateTime.Now));
                    value = cmd.ExecuteNonQuery();
                }
                if (state == System.Data.ConnectionState.Open)
                    connection.Close();
                if (value > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Helper Methods
        private static void PrintConsole(string value, ConsoleColor _color) {
            Console.ForegroundColor = _color;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }
        #endregion
    }
}