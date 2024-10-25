using Merchanter.Classes;
using Merchanter.Responses;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public class QP_MySQLHelper {
        public static MySqlConnection connection { get; set; } = null;
        private static string server { get; set; } = Constants.qpmysql_server;
        private static string uid { get; set; } = Constants.qpmysql_uid;
        private static string password { get; set; } = Constants.qpmysql_password;
        private static string database { get; set; } = Constants.qpmysql_database;
        private static string port { get; set; } = Constants.qpmysql_port;
        public static System.Data.ConnectionState state { get; set; }

        private static void Connection_StateChange( object sender, System.Data.StateChangeEventArgs e ) => state = e.CurrentState;

        private static void PrepareConnection() {
            string connectionString = string.Empty;
            server = server; uid = uid; password = password; database = database;
            connectionString = "Server=" + server + ";Port=" + port + ";" + "Database=" +
            database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";";
            connection = new MySqlConnection( connectionString );
            connection.StateChange += Connection_StateChange;
        }

        private bool OpenConnection() {
            try {
                connection.Open();
                return true;
            }
            catch( MySqlException ex ) {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch( ex.Number ) {
                    case 0:
                        Log.It( ex );
                        break;

                    case 1045:
                        Log.It( ex );
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection() {
            try {
                connection.Close();
                return true;
            }
            catch( MySqlException ex ) {
                Log.It( ex );
                return false;
            }
        }

        public static List<M2_ProductSetGroupAttributes>? GetProductSetGroupAttributes( int _attribute_set_id, int _attribute_set_group_id ) {
            try {
                PrepareConnection();
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                string _query = "SELECT * FROM eav_entity_attribute AS eea WHERE eea.attribute_set_id = @attribute_set_id AND eea.attribute_group_id = @attribute_group_id;";
                List<M2_ProductSetGroupAttributes> list = new List<M2_ProductSetGroupAttributes>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "attribute_set_id", _attribute_set_id ) );
                cmd.Parameters.Add( new MySqlParameter( "attribute_group_id", _attribute_set_group_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    list.Add( new M2_ProductSetGroupAttributes() {
                        entity_attribute_id = int.Parse( dataReader[ "entity_attribute_id" ].ToString() ),
                        entity_type_id = int.Parse( dataReader[ "entity_type_id" ].ToString() ),
                        attribute_set_id = int.Parse( dataReader[ "attribute_set_id" ].ToString() ),
                        attribute_group_id = int.Parse( dataReader[ "attribute_group_id" ].ToString() ),
                        attribute_id = int.Parse( dataReader[ "attribute_id" ].ToString() ),
                        sort_order = int.Parse( dataReader[ "sort_order" ].ToString() )
                    } );
                }
                dataReader.Close();

                return list;
            }
            catch {
                return null;
            } finally {
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
            }
        }

        public static bool? QP_UpdateCustomBundlePC( string _product_id, decimal _custom_price ) {
            try {
                using( QP_MySQLHelper.connection = new MySqlConnection() ) {
                    QP_MySQLHelper.PrepareConnection();
                    if( state != System.Data.ConnectionState.Open )
                        connection.Open();
                    string query = "UPDATE qptest.catalog_product_bundle_selection SET cpc_price = " + _custom_price.ToString().Replace( ',', '.' ) + " where product_id = " + _product_id + ";";
                    MySqlCommand cmd = new MySqlCommand( query, connection );
                    int value = cmd.ExecuteNonQuery();
                    if( value > 0 )
                        return true;
                    else
                        return false;
                }
            }
            catch {
                return null;
            } finally {
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
            }
        }

        public static bool QP_UpdateInvoiceNo( string _order_label, string _invoice_no ) {
            try {
                using( QP_MySQLHelper.connection = new MySqlConnection() ) {
                    QP_MySQLHelper.PrepareConnection();
                    if( state != System.Data.ConnectionState.Open )
                        connection.Open();
                    string query = "UPDATE qptest.sales_order SET invoice_no = '" + _invoice_no + "' where increment_id = '" + _order_label + "';";
                    MySqlCommand cmd = new MySqlCommand( query, connection );
                    int value = cmd.ExecuteNonQuery();
                    if( value > 0 )
                        return true;
                    else
                        return false;
                }
            }
            catch {
                return false;
            } finally {
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
            }
        }

        public static int? GetM2ProductId( string _sku ) {
            try {
                using( QP_MySQLHelper.connection = new MySqlConnection() ) {
                    QP_MySQLHelper.PrepareConnection();
                    if( state != System.Data.ConnectionState.Open )
                        connection.Open();
                    string _query = "SELECT a.entity_id FROM qptest.catalog_product_entity as a where a.sku = '" + _sku + "'; ";
                    int id = 0;
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while( dataReader.Read() ) {
                        id = int.Parse( dataReader[ "entity_id" ].ToString() );
                    }
                    dataReader.Close();
                    return id;
                }
            }
            catch {
                return null;
            } finally {
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
            }
        }


    }
}
