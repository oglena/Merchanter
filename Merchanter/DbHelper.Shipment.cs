using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

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
                        is_shipped = dataReader["is_shipped"] is not null ? dataReader["is_shipped"].ToString() == "1" ? true : false : false,
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
                    s.is_shipped = dataReader["is_shipped"] is not null ? dataReader["is_shipped"].ToString() == "1" ? true : false : false;
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
    }
}
