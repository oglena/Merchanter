using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {

    /// <summary>
    /// Provides methods for managing MySQL database connections.
    /// Includes opening, closing, and handling connection state changes.
    /// </summary>
    public partial class DbHelper {

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
                MySqlCommand cmd = new(_query, connection);
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
    }
}