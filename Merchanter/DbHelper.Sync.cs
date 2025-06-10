using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

        #region Sync Methods
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
