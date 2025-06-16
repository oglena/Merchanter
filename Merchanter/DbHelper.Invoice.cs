using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    /// <summary>
    /// Provides helper methods for interacting with the database, specifically for managing invoices and invoice items.
    /// </summary>
    /// <remarks>The <see cref="DbHelper"/> class includes methods for retrieving, inserting, updating, and
    /// managing invoices and their associated items. It is designed to facilitate database operations related to
    /// invoices, such as fetching invoice data, updating invoice statuses, and handling invoice items. Ensure that the
    /// database connection is properly configured and open before using these methods.</remarks>
    public partial class DbHelper {

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
                        is_belge_created = dataReader["is_belge_created"] is not null && dataReader["is_belge_created"].ToString() == "1",
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
                    if (items is not null) {
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
                    inv.is_belge_created = dataReader["is_belge_created"] is not null && (dataReader["is_belge_created"].ToString() == "1");
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
                if (inv is not null) {
                    var i = GetInvoiceItems(_customer_id, _erp_no);
                    if (i is not null)
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
                        if (item.items is not null && item.items.Count > 0) {
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
                        if (item.items is not null && item.items.Count > 0) {
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
    }
}
