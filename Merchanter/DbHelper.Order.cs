using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    /// <summary>
    /// Provides helper methods for interacting with the database, specifically for managing orders, order items,
    /// billing addresses, and shipping addresses.
    /// </summary>
    /// <remarks>The <see cref="DbHelper"/> class includes methods for retrieving, inserting, updating, and
    /// deleting data related to orders and their associated entities. It is designed to facilitate database operations
    /// for customer orders, including handling order statuses, shipment barcodes, and ERP integration.</remarks>
    public partial class DbHelper {

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
                        erp_no = dataReader["erp_no"] is not null ? dataReader["erp_no"].ToString() : null,
                        is_erp_sent = dataReader["is_erp_sent"] is not null ? dataReader["is_erp_sent"].ToString().Equals("1") : false,
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
                if (order_items is not null) {
                    foreach (var item in list) {
                        item.order_items = order_items.Where(x => x.order_id == item.order_id).ToList();
                    }
                }
                else {
                    OnError("GetOrders: Order Items Not Found");
                    return null;
                }

                var billing_items = GetBillingAddresses(_customer_id);
                if (billing_items is not null) {
                    foreach (var item in list) {
                        var bi = billing_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (bi is not null) {
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
                if (shipping_items is not null) {
                    foreach (var item in list) {
                        var si = shipping_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (si is not null) {
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
                        erp_no = dataReader["erp_no"] is not null ? dataReader["erp_no"].ToString() : null,
                        is_erp_sent = dataReader["is_erp_sent"] is not null ? dataReader["is_erp_sent"].ToString().Equals("1") : false,
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
                if (order_items is not null) {
                    foreach (var item in list) {
                        item.order_items = order_items.Where(x => x.order_id == item.order_id).ToList();
                    }
                }
                else {
                    OnError("GetOrders: Order Items Not Found");
                    return null;
                }

                var billing_items = GetBillingAddresses(_customer_id);
                if (billing_items is not null) {
                    foreach (var item in list) {
                        var bi = billing_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (bi is not null) {
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
                if (shipping_items is not null) {
                    foreach (var item in list) {
                        var si = shipping_items.Where(x => x.order_id == item.order_id)?.FirstOrDefault();
                        if (si is not null) {
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
                    o.erp_no = dataReader["erp_no"] is not null ? dataReader["erp_no"].ToString() : null;
                    o.is_erp_sent = dataReader["is_erp_sent"] is not null ? dataReader["is_erp_sent"].ToString().Equals("1") : false;
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

                if (o is not null) {
                    var order_items = GetOrderItems(_customer_id, _order_id);
                    if (order_items is not null)
                        o.order_items = order_items;
                    else {
                        OnError("GetOrder: " + o.order_label + " Order Items Not Found");
                        return null;
                    }

                    var order_ba = GetBillingAddress(_customer_id, _order_id);
                    if (order_ba is not null)
                        o.billing_address = order_ba;
                    else {
                        OnError("GetOrder: " + o.order_label + " Billing Address Not Found");
                        return null;
                    }

                    var order_sa = GetShippingAddress(_customer_id, _order_id);
                    if (order_sa is not null)
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
                        tax_included = dataReader["tax_included"] is not null ? dataReader["tax_included"].ToString().Equals("1") : false,
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
                        tax_included = dataReader["tax_included"] is not null ? dataReader["tax_included"].ToString().Equals("1") : false,
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
                        is_corporate = dataReader["is_corporate"] is not null ? dataReader["is_corporate"].ToString().Equals("1") : false,
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
                    ba.is_corporate = dataReader["is_corporate"] is not null ? dataReader["is_corporate"].ToString().Equals("1") : false;
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
    }
}
