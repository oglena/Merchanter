using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

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
    }
}
