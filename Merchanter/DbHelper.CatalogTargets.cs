using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

        #region Target Operations
        /// <summary>
        /// Gets the category targets from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<CategoryTarget>?> GetCategoryTargets(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM category_targets WHERE customer_id=@customer_id;";
                    List<CategoryTarget> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        CategoryTarget ct = new CategoryTarget {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            category_id = Convert.ToInt32(dataReader["category_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                            target_name = dataReader["target_name"].ToString(),
                            sync_status = Enum.TryParse<Target.SyncStatus>(dataReader["sync_status"].ToString(), out var status) ? status : Target.SyncStatus.Error,
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(ct);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets the category targets of category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<CategoryTarget>?> GetCategoryTargets(int _customer_id, int _category_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM category_targets WHERE category_id=@category_id AND customer_id=@customer_id;";
                    List<CategoryTarget> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("category_id", _category_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        CategoryTarget ct = new CategoryTarget {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            category_id = Convert.ToInt32(dataReader["category_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                            target_name = dataReader["target_name"].ToString(),
                            sync_status = Enum.TryParse<Target.SyncStatus>(dataReader["sync_status"].ToString(), out var status) ? status : Target.SyncStatus.Error,
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(ct);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets the product targets from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductTarget>?> GetProductTargets(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_targets WHERE customer_id=@customer_id;";
                    List<ProductTarget> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductTarget pt = new ProductTarget {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                            target_name = dataReader["target_name"].ToString(),
                            sync_status = Enum.TryParse<Target.SyncStatus>(dataReader["sync_status"].ToString(), out var status) ? status : Target.SyncStatus.Error,
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pt);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets the product targets of product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductTarget>?> GetProductTargets(int _customer_id, int _product_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_targets WHERE product_id=@product_id AND customer_id=@customer_id;";
                    List<ProductTarget> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductTarget pt = new ProductTarget {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            target_id = Convert.ToInt32(dataReader["target_id"].ToString()),
                            target_name = dataReader["target_name"].ToString(),
                            sync_status = Enum.TryParse<Target.SyncStatus>(dataReader["sync_status"].ToString(), out var status) ? status : Target.SyncStatus.Error,
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pt);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Inserts the category target relation to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">CategoryTarget</param>
        /// <returns>Error returns 'int:0'</returns>
        public async Task<int> InsertCategoryTarget(int _customer_id, CategoryTarget _target) {
            try {
                string _query = "START TRANSACTION;" +
                    "INSERT INTO category_targets (customer_id,category_id,target_id,target_name,sync_status) VALUES (@customer_id,@category_id,@target_id,@target_name,@sync_status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("category_id", _target.category_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                cmd.Parameters.Add(new MySqlParameter("sync_status", (int)_target.sync_status));

                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                object? val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int CTID))
                        return CTID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Inserts the product target relation to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">ProductTarget</param>
        /// <returns>Error returns 'int:0'</returns>
        public async Task<int> InsertProductTarget(int _customer_id, ProductTarget _target) {
            try {
                string _query = "START TRANSACTION;" +
                    "INSERT INTO product_targets (customer_id,product_id,target_id,target_name,sync_status) VALUES (@customer_id,@product_id,@target_id,@target_name,@sync_status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _target.product_id));
                cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                cmd.Parameters.Add(new MySqlParameter("sync_status", (int)_target.sync_status));

                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                object? val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int PTID))
                        return PTID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Updates the category target relation in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">CategoryTarget</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<bool> UpdateCategoryTarget(int _customer_id, CategoryTarget _target) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "UPDATE category_targets SET category_id=@category_id,target_id=@target_id,target_name=@target_name,update_date=@update_date,sync_status=@sync_status " +
                        "WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _target.id));
                    cmd.Parameters.Add(new MySqlParameter("category_id", _target.category_id));
                    cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                    cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("sync_status", (int)_target.sync_status));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Updates the product target relation in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_target">ProductTarget</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<bool> UpdateProductTarget(int _customer_id, ProductTarget _target) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "UPDATE product_targets SET product_id=@product_id,target_id=@target_id,target_name=@target_name,update_date=@update_date,sync_status=@sync_status " +
                        "WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _target.id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _target.product_id));
                    cmd.Parameters.Add(new MySqlParameter("target_id", _target.target_id));
                    cmd.Parameters.Add(new MySqlParameter("target_name", _target.target_name));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    cmd.Parameters.Add(new MySqlParameter("sync_status", (int)_target.sync_status));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Deletes the product target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteProductTarget(int _customer_id, int _product_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "DELETE FROM product_targets WHERE product_id=@product_id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Deletes the category target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteCategoryTarget(int _customer_id, int _category_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "DELETE FROM category_targets WHERE category_id=@category_id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("category_id", _category_id));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return val > 0;
                }
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
