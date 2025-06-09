using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {

    /// <summary>
    /// Provides methods for managing MySQL database connections.
    /// Includes opening, closing, and handling connection state changes.
    /// </summary>
    public partial class DbHelper {

        #region Admin
        /// <summary>
        /// Gets the admin from the database
        /// </summary>
        /// <param name="_name">Admin Username</param>
        /// <param name="_password">Admin Password</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Admin?> GetAdmin(string _name, string _password) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM admin WHERE name=@name AND password=@password;";
                    Admin? a = null;
                    MySqlCommand cmd = new(_query, connection);
                    //cmd.Parameters.Add( new MySqlParameter( "id", _admin_id ) );
                    cmd.Parameters.Add(new MySqlParameter("name", _name));
                    cmd.Parameters.Add(new MySqlParameter("password", _password));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        a = new Admin {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            name = dataReader["name"].ToString(),
                            password = dataReader["password"].ToString(),
                            type = Convert.ToInt32(dataReader["type"].ToString())
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return a;
                }
                return null;
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
        public async Task<bool> LogToServer(string? _thread_id, string _title, string _message, int _customer_id, string _worker = "general") {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string query = "INSERT INTO log (thread_id,title,message,worker,customer_id) VALUES (@thread_id,@title,@message,@worker,@customer_id);";
                    MySqlCommand cmd = new(query, connection);
                    cmd.Parameters.Add(new MySqlParameter() { ParameterName = "thread_id", Value = _thread_id });
                    cmd.Parameters.Add(new MySqlParameter() { ParameterName = "title", Value = _title });
                    cmd.Parameters.Add(new MySqlParameter() { ParameterName = "message", Value = _message });
                    cmd.Parameters.Add(new MySqlParameter() { ParameterName = "worker", Value = _worker });
                    cmd.Parameters.Add(new MySqlParameter() { ParameterName = "customer_id", Value = _customer_id });
                    int value = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    DbHelperBase.PrintConsole("log|" + _title + ":" + _message, ConsoleColor.Yellow);
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                DbHelperBase.PrintConsole("LOG ERROR GG - " + ex.ToString(), ConsoleColor.Red);
                return false;
            }
        }

        /// <summary>
        /// Gets the last logs from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Log>?> GetLastLogs(int _customer_id, ApiFilter _filters) {
            try {
                List<Log> list = [];
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Log));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
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
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets log count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public async Task<int> GetLogsCount(int _customer_id, ApiFilter _filters) {
            try {
                string _query = "SELECT COUNT(*) FROM log WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Log), true);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    object? val = await cmd.ExecuteScalarAsync();
                    if (val is null) return -1;
                    _ = int.TryParse(val.ToString(), out int total_count);
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return total_count;
                }
                return -1;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Notification
        /// <summary>
        /// Inserts the notifications to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_notifications">Notifications</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> InsertNotification(int _customer_id, Notification _notification) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "INSERT INTO notifications (customer_id,type,order_label,product_sku,xproduct_barcode,invoice_no,notification_content) VALUES (@customer_id,@type,@order_label,@product_sku,@xproduct_barcode,@invoice_no,@notification_content);";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("type", _notification.type));
                    cmd.Parameters.Add(new MySqlParameter("is_notification_sent", _notification.is_notification_sent));
                    cmd.Parameters.Add(new MySqlParameter("notification_content", _notification.notification_content));
                    cmd.Parameters.Add(new MySqlParameter("order_label", _notification.order_label));
                    cmd.Parameters.Add(new MySqlParameter("product_sku", _notification.product_sku));
                    cmd.Parameters.Add(new MySqlParameter("xproduct_barcode", _notification.xproduct_barcode));
                    cmd.Parameters.Add(new MySqlParameter("invoice_no", _notification.invoice_no));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
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
        /// Updates the notifications to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_notifications">Notifications</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> UpdateNotification(int _customer_id, Notification _notification) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "UPDATE notifications SET notification_content=@notification_content,is_notification_sent=@is_notification_sent,notification_date=@notification_date WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _notification.id));
                    cmd.Parameters.Add(new MySqlParameter("notification_content", _notification.notification_content));
                    cmd.Parameters.Add(new MySqlParameter("is_notification_sent", _notification.is_notification_sent));
                    cmd.Parameters.Add(new MySqlParameter("notification_date", DateTime.Now));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
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
        /// Gets the notifications from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_is_notification_sent">Is Notification Sent</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Notification>?> GetNotifications(int _customer_id, bool? _is_notification_sent, ApiFilter _filters) {
            try {
                List<Notification> list = [];
                string _query = (!_is_notification_sent.HasValue ? "SELECT * FROM notifications WHERE customer_id=@customer_id" :
                "SELECT * FROM notifications WHERE is_notification_sent = " + (_is_notification_sent.Value ? "1" : "0") + " AND customer_id=@customer_id");
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Notification));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
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
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
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
                        is_active = dataReader["is_active"] is not null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
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
    }
}