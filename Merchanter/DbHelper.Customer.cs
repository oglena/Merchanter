using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

        #region Customer
        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <returns>The customer object, or null if not found.</returns>
        public async Task<Customer?> GetCustomer(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM customer WHERE customer_id=@customer_id;";
                    Customer? c = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
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
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return c;
                }
                return null;
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
        public async Task<Customer?> GetCustomerByMail(string _email, string _password) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM customer WHERE email=@email AND password=@password;";
                    Customer? c = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("email", _email));
                    cmd.Parameters.Add(new MySqlParameter("password", _password));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
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
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return c;
                }
                return null;
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
        public async Task<List<Customer>?> GetCustomers() {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM customer;";
                    MySqlCommand cmd = new(_query, connection);
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<Customer> customers = [];
                    while (await dataReader.ReadAsync()) {
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
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return customers;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Saves the specified customer to the database and updates their information.
        /// </summary>
        /// <remarks>This method performs an update operation on the customer record in the database. If
        /// the customer does not exist,  it inserts a new record and retrieves the updated customer details. The
        /// operation is wrapped in a transaction to  ensure data consistency. Optionally, additional fields related to
        /// synchronization and password can be updated  based on the <paramref name="_WWP"/> parameter.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer to be updated or inserted.</param>
        /// <param name="_customer">An instance of the <see cref="Customer"/> class containing the customer's details to be saved.</param>
        /// <param name="_WWP">With working parameters. A boolean value indicating whether to update synchronization-related fields and the password. If <see
        /// langword="true"/>, these fields are included in the update; otherwise, they are excluded.</param>
        /// <returns>A <see cref="Customer"/> object representing the updated customer, or <see langword="null"/> if the
        /// operation fails.</returns>
        public async Task<Customer?> SaveCustomer(int _customer_id, Customer _customer, bool _WWP = true) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    object? val;
                    string _query = "START TRANSACTION;" +
                        "UPDATE customer SET customer_id=LAST_INSERT_ID(@customer_id),user_name=@user_name,person_name=@person_name,email=@email,role=@role,status=@status,product_sync_status=@product_sync_status,order_sync_status=@order_sync_status,xml_sync_status=@xml_sync_status,invoice_sync_status=@invoice_sync_status,notification_sync_status=@notification_sync_status" +
                        ",product_sync_timer=@product_sync_timer,order_sync_timer=@order_sync_timer,xml_sync_timer=@xml_sync_timer,invoice_sync_timer=@invoice_sync_timer,notification_sync_timer=@notification_sync_timer" +
                        (_WWP ? ",password=@password,is_productsync_working=@is_productsync_working,is_ordersync_working=@is_ordersync_working,is_xmlsync_working=@is_xmlsync_working,is_invoicesync_working=@is_invoicesync_working,is_notificationsync_working=@is_notificationsync_working" : "") + " WHERE customer_id=@customer_id;" +
                        "SELECT LAST_INSERT_ID();" +
                        "COMMIT;";
                    MySqlCommand cmd = new(_query, connection);
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
                    if (_WWP) {
                        cmd.Parameters.Add(new MySqlParameter("is_productsync_working", _customer.is_productsync_working));
                        cmd.Parameters.Add(new MySqlParameter("is_ordersync_working", _customer.is_ordersync_working));
                        cmd.Parameters.Add(new MySqlParameter("is_xmlsync_working", _customer.is_xmlsync_working));
                        cmd.Parameters.Add(new MySqlParameter("is_invoicesync_working", _customer.is_invoicesync_working));
                        cmd.Parameters.Add(new MySqlParameter("is_notificationsync_working", _customer.is_notificationsync_working));
                        cmd.Parameters.Add(new MySqlParameter("password", _customer.password));
                    }
                    val = await cmd.ExecuteScalarAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();

                    if (val is not null) {
                        if (int.TryParse(val.ToString(), out int inserted_id)) {
                            return await GetCustomer(inserted_id);
                        }
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
        public async Task<Customer?> CreateCustomer(Customer _customer) {
            return null;
        }
        #endregion
    }
}
