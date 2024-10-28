using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MySql.Data.MySqlClient;
using NetOpenX.Rest.Core.Models;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Merchanter {
    public class DbHelper {
        #region CORE
        private MySqlConnection connection = new();
        public MySqlConnection Connection {
            get { return connection; }
            set { connection = value; }
        }

        private string server = string.Empty;
        public string Server {
            get { return server; }
            set { server = value; }
        }

        private string user = string.Empty;
        public string User {
            get { return user; }
            set { user = value; }
        }

        private string password = string.Empty;
        public string Password {
            get { return password; }
            set { password = value; }
        }

        private int port;
        public int Port {
            get { return port; }
            set { port = value; }
        }

        private string database = string.Empty;
        public string Database {
            get { return database; }
            set { database = value; }
        }

        private string? connectionString { get; set; } = null;
        public System.Data.ConnectionState state { get; set; }
        public List<DBSetting> db_settings { get; set; } = [];
        public DbHelper invoice { get; set; }
        public DbHelper xml { get; set; }
        public DbHelper notification { get; set; }

        private void Connection_StateChange( object sender, System.Data.StateChangeEventArgs e ) => state = e.CurrentState;
        public event EventHandler<string> ErrorOccured;

        public DbHelper( string _server, string _user, string _password, string _database, int _port = 3306 ) {
            try {
                Server = _server;
                User = _user;
                Password = _password;
                Database = _database;
                Port = _port;
                connectionString = "Server=" + Server + ";Port=" + Port.ToString() + ";" + "Database=" +
            Database + ";" + "Uid=" + User + ";" + "Pwd=" + Password + ";CharSet=utf8;";
                //Connection = new MySqlConnection( connectionString );
                Connection.ConnectionString = connectionString;
                Connection.StateChange += Connection_StateChange;
            }
            catch {
            }
        }

        protected virtual void OnError( string e ) {
            ErrorOccured?.Invoke( this, e );
        }

        public bool OpenConnection() {
            try {
                connection.Open();
                return true;
            }
            catch( MySqlException ex ) {
                switch( ex.Number ) {
                    case 0:
                        OnError( ex.Message );
                        break;

                    case 1045:
                        OnError( ex.Message );
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection() {
            try {
                connection.Close();
                return true;
            }
            catch( MySqlException ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion


        #region Customers
        public Customer GetCustomer( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM customer WHERE customer_id=@customer_id";
                Customer c = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    c = new Customer {
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        user_name = dataReader[ "user_name" ].ToString(),
                        password = dataReader[ "password" ].ToString(),
                        status = Convert.ToBoolean( int.Parse( dataReader[ "status" ].ToString() ) ),
                        product_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "product_sync_status" ].ToString() ) ),
                        order_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "order_sync_status" ].ToString() ) ),
                        xml_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "xml_sync_status" ].ToString() ) ),
                        invoice_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "invoice_sync_status" ].ToString() ) ),
                        notification_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "notification_sync_status" ].ToString() ) ),
                        product_sync_timer = int.Parse( dataReader[ "product_sync_timer" ].ToString() ),
                        order_sync_timer = int.Parse( dataReader[ "order_sync_timer" ].ToString() ),
                        xml_sync_timer = int.Parse( dataReader[ "xml_sync_timer" ].ToString() ),
                        invoice_sync_timer = int.Parse( dataReader[ "invoice_sync_timer" ].ToString() ),
                        notification_sync_timer = int.Parse( dataReader[ "notification_sync_timer" ].ToString() ),
                        last_product_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_product_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_product_sync_date" ].ToString() ) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_order_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_order_sync_date" ].ToString() ) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_xml_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_xml_sync_date" ].ToString() ) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_invoice_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_invoice_sync_date" ].ToString() ) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_notification_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_notification_sync_date" ].ToString() ) : null,
                        is_productsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_productsync_working" ].ToString() ) ),
                        is_ordersync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_ordersync_working" ].ToString() ) ),
                        is_xmlsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_xmlsync_working" ].ToString() ) ),
                        is_invoicesync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_invoicesync_working" ].ToString() ) ),
                        is_notificationsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_notificationsync_working" ].ToString() ) ),
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return c;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Customer GetCustomer( int _customer_id, string _username, string _password ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM customer WHERE user_name=@user_name AND password=@password AND customer_id=@customer_id";
                Customer c = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "user_name", _username ) );
                cmd.Parameters.Add( new MySqlParameter( "password", _password ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    c = new Customer {
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        user_name = dataReader[ "user_name" ].ToString(),
                        password = dataReader[ "password" ].ToString(),
                        status = Convert.ToBoolean( int.Parse( dataReader[ "status" ].ToString() ) ),
                        product_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "product_sync_status" ].ToString() ) ),
                        order_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "order_sync_status" ].ToString() ) ),
                        xml_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "xml_sync_status" ].ToString() ) ),
                        invoice_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "invoice_sync_status" ].ToString() ) ),
                        notification_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "notification_sync_status" ].ToString() ) ),
                        product_sync_timer = int.Parse( dataReader[ "product_sync_timer" ].ToString() ),
                        order_sync_timer = int.Parse( dataReader[ "order_sync_timer" ].ToString() ),
                        xml_sync_timer = int.Parse( dataReader[ "xml_sync_timer" ].ToString() ),
                        invoice_sync_timer = int.Parse( dataReader[ "invoice_sync_timer" ].ToString() ),
                        notification_sync_timer = int.Parse( dataReader[ "notification_sync_timer" ].ToString() ),
                        last_product_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_product_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_product_sync_date" ].ToString() ) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_order_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_order_sync_date" ].ToString() ) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_xml_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_xml_sync_date" ].ToString() ) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_invoice_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_invoice_sync_date" ].ToString() ) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_notification_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_notification_sync_date" ].ToString() ) : null,
                        is_productsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_productsync_working" ].ToString() ) ),
                        is_ordersync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_ordersync_working" ].ToString() ) ),
                        is_xmlsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_xmlsync_working" ].ToString() ) ),
                        is_invoicesync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_invoicesync_working" ].ToString() ) ),
                        is_notificationsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_notificationsync_working" ].ToString() ) ),
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return c;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<Customer> GetCustomers() {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM customer";
                Customer c = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                List<Customer> customers = new List<Customer>();
                while( dataReader.Read() ) {
                    customers.Add( new Customer {
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        user_name = dataReader[ "user_name" ].ToString(),
                        password = dataReader[ "password" ].ToString(),
                        status = Convert.ToBoolean( int.Parse( dataReader[ "status" ].ToString() ) ),
                        product_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "product_sync_status" ].ToString() ) ),
                        order_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "order_sync_status" ].ToString() ) ),
                        xml_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "xml_sync_status" ].ToString() ) ),
                        invoice_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "invoice_sync_status" ].ToString() ) ),
                        notification_sync_status = Convert.ToBoolean( int.Parse( dataReader[ "notification_sync_status" ].ToString() ) ),
                        product_sync_timer = int.Parse( dataReader[ "product_sync_timer" ].ToString() ),
                        order_sync_timer = int.Parse( dataReader[ "order_sync_timer" ].ToString() ),
                        xml_sync_timer = int.Parse( dataReader[ "xml_sync_timer" ].ToString() ),
                        invoice_sync_timer = int.Parse( dataReader[ "invoice_sync_timer" ].ToString() ),
                        notification_sync_timer = int.Parse( dataReader[ "notification_sync_timer" ].ToString() ),
                        last_product_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_product_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_product_sync_date" ].ToString() ) : null,
                        last_order_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_order_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_order_sync_date" ].ToString() ) : null,
                        last_xml_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_xml_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_xml_sync_date" ].ToString() ) : null,
                        last_invoice_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_invoice_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_invoice_sync_date" ].ToString() ) : null,
                        last_notification_sync_date = !string.IsNullOrWhiteSpace( dataReader[ "last_notification_sync_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "last_notification_sync_date" ].ToString() ) : null,
                        is_productsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_productsync_working" ].ToString() ) ),
                        is_ordersync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_ordersync_working" ].ToString() ) ),
                        is_xmlsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_xmlsync_working" ].ToString() ) ),
                        is_invoicesync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_invoicesync_working" ].ToString() ) ),
                        is_notificationsync_working = Convert.ToBoolean( int.Parse( dataReader[ "is_notificationsync_working" ].ToString() ) ),
                    } );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return customers;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Customer SaveCustomer( int _customer_id, Customer _customer ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                object val; int inserted_id;
                string _query = "START TRANSACTION;" +
                    "UPDATE customer SET customer_id=LAST_INSERT_ID(@customer_id),user_name=@user_name,password=@password,status=@status,product_sync_status=@product_sync_status,order_sync_status=@order_sync_status,xml_sync_status=@xml_sync_status,invoice_sync_status=@invoice_sync_status,notification_sync_status=@notification_sync_status,is_productsync_working=@is_productsync_working,is_ordersync_working=@is_ordersync_working,is_xmlsync_working=@is_xmlsync_working,is_notificationsync_working=@is_notificationsync_working,is_invoicesync_working=@is_invoicesync_working WHERE customer_id=@customer_id;" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "user_name", _customer.user_name ) );
                cmd.Parameters.Add( new MySqlParameter( "password", _customer.password ) );
                cmd.Parameters.Add( new MySqlParameter( "status", _customer.status ) );
                cmd.Parameters.Add( new MySqlParameter( "product_sync_status", _customer.product_sync_status ) );
                cmd.Parameters.Add( new MySqlParameter( "order_sync_status", _customer.order_sync_status ) );
                cmd.Parameters.Add( new MySqlParameter( "xml_sync_status", _customer.xml_sync_status ) );
                cmd.Parameters.Add( new MySqlParameter( "invoice_sync_status", _customer.invoice_sync_status ) );
                cmd.Parameters.Add( new MySqlParameter( "notification_sync_status", _customer.notification_sync_status ) );
                cmd.Parameters.Add( new MySqlParameter( "is_productsync_working", _customer.is_productsync_working ) );
                cmd.Parameters.Add( new MySqlParameter( "is_ordersync_working", _customer.is_ordersync_working ) );
                cmd.Parameters.Add( new MySqlParameter( "is_xmlsync_working", _customer.is_xmlsync_working ) );
                cmd.Parameters.Add( new MySqlParameter( "is_invoicesync_working", _customer.is_invoicesync_working ) );
                cmd.Parameters.Add( new MySqlParameter( "is_notificationsync_working", _customer.is_notificationsync_working ) );
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                val = cmd.ExecuteScalar();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( val != null ) {
                    if( int.TryParse( val.ToString(), out inserted_id ) ) {
                        return GetCustomer( inserted_id );
                    }
                }
                return null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion

        #region Admin
        public Admin? GetAdmin( int _admin_id, string _name, string _password ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM admin WHERE name=@name AND password=@password AND id=@id";
                Admin? a = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "id", _admin_id ) );
                cmd.Parameters.Add( new MySqlParameter( "name", _name ) );
                cmd.Parameters.Add( new MySqlParameter( "password", _password ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    a = new Admin {
                        id = Convert.ToInt32( dataReader[ "id" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        password = dataReader[ "password" ].ToString(),
                        type = Convert.ToInt32( dataReader[ "type" ].ToString() )
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return a;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion

        #region Log
        public List<Log> GetLastLogs( int _customer_id, int _items_per_page = 20, int _current_page_index = 0 ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id ORDER BY id DESC LIMIT @start,@end";
                List<Log> list = new List<Log>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "start", _items_per_page * (_current_page_index) ) );
                cmd.Parameters.Add( new MySqlParameter( "end", _items_per_page ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Log l = new Log {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        worker = dataReader[ "worker" ].ToString(),
                        title = dataReader[ "title" ].ToString(),
                        message = dataReader[ "message" ].ToString(),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                    };
                    list.Add( l );
                }
                dataReader.Close();


                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        public List<Log> GetLastLogs( int _customer_id, Dictionary<string, string> _filters, int _items_per_page = 20, int _current_page_index = 0 ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM log WHERE customer_id=@customer_id";
                string? filtered_worker = _filters[ "filtered_worker" ];
                string? filtered_title = _filters[ "filtered_title" ];
                string? filtered_message = _filters[ "filtered_message" ];
                string? filtered_date = _filters[ "filtered_date" ];
                List<Log> list = new List<Log>();
                if( filtered_worker != null && filtered_title != null && filtered_message != null && filtered_date != null ) {
                    _query += !string.IsNullOrWhiteSpace( filtered_worker ) && filtered_worker != "0" ? " AND worker='" + filtered_worker + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace( filtered_title ) && filtered_title != "0" ? " AND title='" + filtered_title + "'" : string.Empty;
                    _query += !string.IsNullOrWhiteSpace( filtered_message ) && filtered_message != "0" ? " AND message LIKE '%" + filtered_message + "%'" : string.Empty;
                    //_query += !string.IsNullOrWhiteSpace( filtered_date ) && filtered_worker != "0" ? " AND update_date='" + filtered_date + "'" : string.Empty;
                    _query += " ORDER BY id DESC LIMIT @start,@end";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "start", _items_per_page * (_current_page_index) ) );
                    cmd.Parameters.Add( new MySqlParameter( "end", _items_per_page ) );
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while( dataReader.Read() ) {
                        Log l = new Log {
                            id = int.Parse( dataReader[ "id" ].ToString() ),
                            customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                            worker = dataReader[ "worker" ].ToString(),
                            title = dataReader[ "title" ].ToString(),
                            message = dataReader[ "message" ].ToString(),
                            update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        };
                        list.Add( l );
                    }
                    dataReader.Close();


                }
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion

        #region Settings
        public bool SaveSetting( int _customer_id, string _name, string _value ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "UPDATE db_settings SET value=@value, update_date=@update_date WHERE customer_id=@customer_id AND name=@name";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                        cmd.Parameters.Add( new MySqlParameter( "name", _name ) );
                        cmd.Parameters.Add( new MySqlParameter( "value", _value ) );
                        cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );

                        int value = cmd.ExecuteNonQuery();

                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        if( value > 0 )
                            return true;
                        else
                            return false;
                    }
                return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public SettingsMerchanter LoadSettings( int _customer_id ) {
            try {
                db_settings = this.GetSettings( _customer_id );
                Helper.global = new SettingsMerchanter( _customer_id ) {
                    customer_id = _customer_id,
                    customer_root_category = GetRootCategory( _customer_id ).id,
                    settings = GetCustomerSettings( _customer_id ),
                    magento = GetMagentoSettings( _customer_id ),
                    netsis = GetNetsisSettings( _customer_id ),
                    entegra = GetEntegraSettings( _customer_id ),
                    order_statuses = LoadOrderStatuses( _customer_id ),
                    payment_methods = LoadPaymentMethods( _customer_id ),
                    shipment_methods = LoadShipmentMethods( _customer_id ),
                    work_sources = LoadWorkSources( _customer_id ),
                    sync_mappings = GetCustomerSyncMappings( _customer_id ),
                    DefaultBrand = db_settings.Where( x => x.name == "DefaultBrand" ).First().value,
                    yurtici_kargo_user_name = db_settings.Where( x => x.name == "yurtici_kargo_user_name" ).First().value,
                    yurtici_kargo_password = db_settings.Where( x => x.name == "yurtici_kargo_password" ).First().value,
                    yurtici_kargo_user_language = db_settings.Where( x => x.name == "yurtici_kargo_user_language" ).First().value,
                    erp_invoice_ftp_username = db_settings.Where( x => x.name == "erp_invoice_ftp_username" ).First().value,
                    erp_invoice_ftp_password = db_settings.Where( x => x.name == "erp_invoice_ftp_password" ).First().value,
                    erp_invoice_ftp_url = db_settings.Where( x => x.name == "erp_invoice_ftp_url" ).First().value,
                    xml_bogazici_bayikodu = db_settings.Where( x => x.name == "xml_bogazici_bayikodu" ).First().value,
                    xml_bogazici_email = db_settings.Where( x => x.name == "xml_bogazici_email" ).First().value,
                    xml_bogazici_sifre = db_settings.Where( x => x.name == "xml_bogazici_sifre" ).First().value,
                    xml_fsp_url = db_settings.Where( x => x.name == "xml_fsp_url" ).First().value,
                    xml_koyuncu_url = db_settings.Where( x => x.name == "xml_koyuncu_url" ).First().value,
                    xml_oksid_url = db_settings.Where( x => x.name == "xml_oksid_url" ).First().value,
                    xml_penta_base_url = db_settings.Where( x => x.name == "xml_penta_base_url" ).First().value,
                    xml_penta_customerid = db_settings.Where( x => x.name == "xml_penta_customerid" ).First().value,
                };

                #region Decyrption
                Helper.global.magento.token = DBSetting.Decrypt( Helper.global.magento.token );
                Helper.global.entegra.api_password = DBSetting.Decrypt( Helper.global.entegra.api_password );
                Helper.global.netsis.netopenx_password = DBSetting.Decrypt( Helper.global.netsis.netopenx_password );
                Helper.global.netsis.dbpassword = DBSetting.Decrypt( Helper.global.netsis.dbpassword );
                Helper.global.yurtici_kargo_password = DBSetting.Decrypt( Helper.global.yurtici_kargo_password );
                Helper.global.erp_invoice_ftp_password = DBSetting.Decrypt( Helper.global.erp_invoice_ftp_password );

                #endregion

                return Helper.global;
            }
            catch {
                Helper.global = null;
                return null;
            }

        }

        public SettingsMerchanter LoadSettings( int _customer_id, SettingsMerchanter? _setting ) {
            try {
                db_settings = this.GetSettings( _customer_id );
                _setting = new SettingsMerchanter( _customer_id ) {
                    customer_id = _customer_id,
                    customer_root_category = GetRootCategory( _customer_id ).id,
                    settings = GetCustomerSettings( _customer_id ),
                    magento = GetMagentoSettings( _customer_id ),
                    netsis = GetNetsisSettings( _customer_id ),
                    entegra = GetEntegraSettings( _customer_id ),
                    order_statuses = LoadOrderStatuses( _customer_id ),
                    payment_methods = LoadPaymentMethods( _customer_id ),
                    shipment_methods = LoadShipmentMethods( _customer_id ),
                    work_sources = LoadWorkSources( _customer_id ),
                    sync_mappings = GetCustomerSyncMappings( _customer_id ),
                    DefaultBrand = db_settings.Where( x => x.name == "DefaultBrand" ).First().value,
                    yurtici_kargo_user_name = db_settings.Where( x => x.name == "yurtici_kargo_user_name" ).First().value,
                    yurtici_kargo_password = db_settings.Where( x => x.name == "yurtici_kargo_password" ).First().value,
                    yurtici_kargo_user_language = db_settings.Where( x => x.name == "yurtici_kargo_user_language" ).First().value,
                    erp_invoice_ftp_username = db_settings.Where( x => x.name == "erp_invoice_ftp_username" ).First().value,
                    erp_invoice_ftp_password = db_settings.Where( x => x.name == "erp_invoice_ftp_password" ).First().value,
                    erp_invoice_ftp_url = db_settings.Where( x => x.name == "erp_invoice_ftp_url" ).First().value,
                    xml_bogazici_bayikodu = db_settings.Where( x => x.name == "xml_bogazici_bayikodu" ).First().value,
                    xml_bogazici_email = db_settings.Where( x => x.name == "xml_bogazici_email" ).First().value,
                    xml_bogazici_sifre = db_settings.Where( x => x.name == "xml_bogazici_sifre" ).First().value,
                    xml_fsp_url = db_settings.Where( x => x.name == "xml_fsp_url" ).First().value,
                    xml_koyuncu_url = db_settings.Where( x => x.name == "xml_koyuncu_url" ).First().value,
                    xml_oksid_url = db_settings.Where( x => x.name == "xml_oksid_url" ).First().value,
                    xml_penta_base_url = db_settings.Where( x => x.name == "xml_penta_base_url" ).First().value,
                    xml_penta_customerid = db_settings.Where( x => x.name == "xml_penta_customerid" ).First().value,
                };

                #region Decyrption
                _setting.magento.token = DBSetting.Decrypt( Helper.global.magento.token );
                _setting.entegra.api_password = DBSetting.Decrypt( Helper.global.entegra.api_password );
                _setting.netsis.netopenx_password = DBSetting.Decrypt( Helper.global.netsis.netopenx_password );
                _setting.netsis.dbpassword = DBSetting.Decrypt( Helper.global.netsis.dbpassword );
                _setting.yurtici_kargo_password = DBSetting.Decrypt( Helper.global.yurtici_kargo_password );
                _setting.erp_invoice_ftp_password = DBSetting.Decrypt( Helper.global.erp_invoice_ftp_password );

                #endregion

                return _setting;
            }
            catch {
                _setting = null;
                return null;
            }

        }

        public List<DBSetting> GetSettings( int _customer_id ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "SELECT * FROM db_settings WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id.ToString() ) );
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<DBSetting> list = new List<DBSetting>();
                        while( dataReader.Read() ) {
                            list.Add( new DBSetting() {
                                id = int.Parse( dataReader[ "id" ].ToString() ),
                                customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                                name = dataReader[ "name" ].ToString(),
                                value = dataReader[ "value" ].ToString(),
                                group_name = dataReader[ "group_name" ].ToString(),
                                description = dataReader[ "description" ].ToString(),
                                update_date = Convert.ToDateTime( dataReader[ "update_date" ].ToString() )
                            } );
                        }
                        dataReader.Close();
                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        return list;
                    }

                return new List<DBSetting>();
            }
            catch {
                return new List<DBSetting>();
            }
        }

        public SettingsGeneral GetCustomerSettings( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM settings WHERE customer_id=@customer_id";
                SettingsGeneral cs = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    cs = new SettingsGeneral {
                        id = Convert.ToInt32( dataReader[ "id" ].ToString() ),
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        company_name = dataReader[ "company_name" ].ToString(),
                        rate_TL = Convert.ToDecimal( dataReader[ "rate_TL" ].ToString() ),
                        rate_USD = Convert.ToDecimal( dataReader[ "rate_USD" ].ToString() ),
                        rate_EUR = Convert.ToDecimal( dataReader[ "rate_EUR" ].ToString() ),
                        daysto_ordersync = Convert.ToInt32( dataReader[ "daysto_ordersync" ].ToString() ),
                        daysto_invoicesync = Convert.ToInt32( dataReader[ "daysto_invoicesync" ].ToString() ),
                        yurtici_kargo = Convert.ToBoolean( int.Parse( dataReader[ "yurtici_kargo" ].ToString() ) ),
                        mng_kargo = Convert.ToBoolean( int.Parse( dataReader[ "mng_kargo" ].ToString() ) ),
                        aras_kargo = Convert.ToBoolean( int.Parse( dataReader[ "aras_kargo" ].ToString() ) ),
                        xml_qty_addictive_enable = Convert.ToBoolean( int.Parse( dataReader[ "xml_qty_addictive_enable" ].ToString() ) )
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return cs;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool SaveCustomerSettings( int _customer_id, SettingsGeneral _settings ) {
            try {
                int val = 0;
                string _query = "UPDATE settings SET company_name=@company_name,rate_TL=@rate_TL,rate_USD=@rate_USD,rate_EUR=@rate_EUR,daysto_ordersync=@daysto_ordersync,daysto_invoicesync=@daysto_invoicesync,yurtici_kargo=@yurtici_kargo,mng_kargo=@mng_kargo,aras_kargo=@aras_kargo,xml_qty_addictive_enable=@xml_qty_addictive_enable WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "company_name", _settings.company_name ) );
                cmd.Parameters.Add( new MySqlParameter( "rate_TL", _settings.rate_TL ) );
                cmd.Parameters.Add( new MySqlParameter( "rate_USD", _settings.rate_USD ) );
                cmd.Parameters.Add( new MySqlParameter( "rate_EUR", _settings.rate_EUR ) );
                cmd.Parameters.Add( new MySqlParameter( "daysto_ordersync", _settings.daysto_ordersync ) );
                cmd.Parameters.Add( new MySqlParameter( "daysto_invoicesync", _settings.daysto_invoicesync ) );
                cmd.Parameters.Add( new MySqlParameter( "yurtici_kargo", _settings.yurtici_kargo ) );
                cmd.Parameters.Add( new MySqlParameter( "mng_kargo", _settings.mng_kargo ) );
                cmd.Parameters.Add( new MySqlParameter( "aras_kargo", _settings.aras_kargo ) );
                cmd.Parameters.Add( new MySqlParameter( "xml_qty_addictive_enable", _settings.xml_qty_addictive_enable ) );

                if( state != System.Data.ConnectionState.Open ) connection.Open();
                val = cmd.ExecuteNonQuery();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SaveCustomerWorkSources( int _customer_id, List<WorkSource> _settings ) {
            try {
                int val = 0;
                foreach( WorkSource item in _settings ) {
                    string _query = "UPDATE m_work_sources SET type=@type,name=@name,direction=@direction,is_active=@is_active WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "type", item.type ) );
                    cmd.Parameters.Add( new MySqlParameter( "name", item.name ) );
                    cmd.Parameters.Add( new MySqlParameter( "direction", item.direction ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_active", item.is_active ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public SettingsEntegra GetEntegraSettings( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM settings_entegra WHERE customer_id=@customer_id";
                SettingsEntegra es = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    es = new SettingsEntegra {
                        id = Convert.ToInt32( dataReader[ "id" ].ToString() ),
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        api_url = dataReader[ "api_url" ].ToString(),
                        api_username = dataReader[ "api_username" ].ToString(),
                        api_password = dataReader[ "api_password" ].ToString()
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return es;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public SettingsMagento GetMagentoSettings( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM settings_magento WHERE customer_id=@customer_id";
                SettingsMagento ms = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    ms = new SettingsMagento {
                        id = Convert.ToInt32( dataReader[ "id" ].ToString() ),
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        base_url = dataReader[ "base_url" ].ToString(),
                        token = dataReader[ "token" ].ToString(),
                        root_category_id = Convert.ToInt32( dataReader[ "root_category_id" ].ToString() ),
                        brand_attribute_code = dataReader[ "brand_attribute_code" ].ToString(),
                        barcode_attribute_code = dataReader[ "barcode_attribute_code" ].ToString(),
                        is_xml_enabled_attribute_code = dataReader[ "is_xml_enabled_attribute_code" ].ToString(),
                        xml_sources_attribute_code = dataReader[ "xml_sources_attribute_code" ].ToString(),
                        customer_tc_no_attribute_code = dataReader[ "customer_tc_no_attribute_code" ].ToString(),
                        customer_firma_ismi_attribute_code = dataReader[ "customer_firma_ismi_attribute_code" ].ToString(),
                        customer_firma_vergidairesi_attribute_code = dataReader[ "customer_firma_vergidairesi_attribute_code" ].ToString(),
                        customer_firma_vergino_attribute_code = dataReader[ "customer_firma_vergino_attribute_code" ].ToString(),
                        order_processing_comment = dataReader[ "order_processing_comment" ].ToString()
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return ms;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public SettingsNetsis GetNetsisSettings( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM settings_netsis WHERE customer_id=@customer_id";
                SettingsNetsis ns = null;
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader( System.Data.CommandBehavior.CloseConnection );
                while( dataReader.Read() ) {
                    ns = new SettingsNetsis {
                        id = Convert.ToInt32( dataReader[ "id" ].ToString() ),
                        customer_id = Convert.ToInt32( dataReader[ "customer_id" ].ToString() ),
                        netopenx_user = dataReader[ "netopenx_user" ].ToString(),
                        netopenx_password = dataReader[ "netopenx_password" ].ToString(),
                        dbname = dataReader[ "dbname" ].ToString(),
                        dbuser = dataReader[ "dbuser" ].ToString(),
                        dbpassword = dataReader[ "dbpassword" ].ToString(),
                        rest_url = dataReader[ "rest_url" ].ToString(),
                        belgeonek_musterisiparisi = dataReader[ "belgeonek_musterisiparisi" ].ToString(),
                        siparis_carionek = dataReader[ "siparis_carionek" ].ToString(),
                        cari_siparis_grupkodu = dataReader[ "cari_siparis_grupkodu" ].ToString(),
                        sipari_caritip = dataReader[ "sipari_caritip" ].ToString(),
                        siparis_muhasebekodu = dataReader[ "siparis_muhasebekodu" ].ToString(),
                        siparis_kdvdahilmi = Convert.ToBoolean( int.Parse( dataReader[ "siparis_kdvdahilmi" ].ToString() ) ),
                        siparis_subekodu = Convert.ToInt32( dataReader[ "siparis_subekodu" ].ToString() ),
                        siparis_depokodu = Convert.ToInt32( dataReader[ "siparis_depokodu" ].ToString() ),
                        siparis_kargo_sku = dataReader[ "siparis_kargo_sku" ].ToString(),
                        siparis_taksitkomisyon_sku = dataReader[ "siparis_taksitkomisyon_sku" ].ToString(),
                        is_rewrite_siparis = Convert.ToBoolean( int.Parse( dataReader[ "is_rewrite_siparis" ].ToString() ) ),
                        ebelge_dizayn_earsiv = dataReader[ "ebelge_dizayn_earsiv" ].ToString(),
                        ebelge_dizayn_efatura = dataReader[ "ebelge_dizayn_efatura" ].ToString(),
                        ebelge_klasorismi = dataReader[ "ebelge_klasorismi" ].ToString(),
                        efatura_belge_onek = dataReader[ "efatura_belge_onek" ].ToString(),
                        earsiv_belge_onek = dataReader[ "earsiv_belge_onek" ].ToString(),
                        fatura_cari_gruplari = dataReader[ "fatura_cari_gruplari" ].ToString(),
                        siparis_kod2 = dataReader[ "siparis_kod2" ].ToString(),
                        siparis_cyedek1 = dataReader[ "siparis_cyedek1" ].ToString(),
                        siparis_ekack15 = dataReader[ "siparis_ekack15" ].ToString(),
                        siparis_ekack10 = dataReader[ "siparis_ekack10" ].ToString(),
                        siparis_ekack11 = dataReader[ "siparis_ekack11" ].ToString(),
                        siparis_ekack4 = dataReader[ "siparis_ekack4" ].ToString()
                    };
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return ns;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public static string? GetSettingValue( string _setting, string _filter ) {
            var temp = _setting.Split( '|' );
            foreach( var item in temp ) {
                if( item.Split( '=' )[ 0 ] == _filter )
                    return item.Split( '=' )[ 1 ];
            }
            return temp[ 0 ].Split( '=' )[ 1 ];
        }

        public List<OrderStatus> LoadOrderStatuses( int _customer_id ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "SELECT * FROM m_order_statuses WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id.ToString() ) );
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<OrderStatus> list = new List<OrderStatus>();
                        while( dataReader.Read() ) {
                            list.Add( new OrderStatus(
                                int.Parse( dataReader[ "id" ].ToString() ),
                                 int.Parse( dataReader[ "customer_id" ].ToString() ),
                                 dataReader[ "status_name" ].ToString(),
                                 dataReader[ "status_code" ].ToString(),
                                 dataReader[ "magento2_status_code" ].ToString(),
                                 Convert.ToBoolean( int.Parse( dataReader[ "sync_status" ].ToString() ) ),
                                 Convert.ToBoolean( int.Parse( dataReader[ "process_status" ].ToString() ) )
                            ) );
                        }
                        dataReader.Close();
                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        return list;
                    }

                return new List<OrderStatus>();
            }
            catch {
                return new List<OrderStatus>();
            }
        }

        public List<PaymentMethod> LoadPaymentMethods( int _customer_id ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "SELECT * FROM m_payment_methods WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id.ToString() ) );
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<PaymentMethod> list = new List<PaymentMethod>();
                        while( dataReader.Read() ) {
                            list.Add( new PaymentMethod(
                                int.Parse( dataReader[ "id" ].ToString() ),
                                 int.Parse( dataReader[ "customer_id" ].ToString() ),
                                 dataReader[ "payment_name" ].ToString(),
                                 dataReader[ "payment_code" ].ToString(),
                                 dataReader[ "magento2_payment_code" ].ToString()
                            ) );
                        }
                        dataReader.Close();
                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        return list;
                    }

                return new List<PaymentMethod>();
            }
            catch {
                return new List<PaymentMethod>();
            }
        }

        public List<ShipmentMethod> LoadShipmentMethods( int _customer_id ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "SELECT * FROM m_shipment_methods WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id.ToString() ) );
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<ShipmentMethod> list = new List<ShipmentMethod>();
                        while( dataReader.Read() ) {
                            list.Add( new ShipmentMethod(
                                int.Parse( dataReader[ "id" ].ToString() ),
                                 int.Parse( dataReader[ "customer_id" ].ToString() ),
                                 dataReader[ "shipment_name" ].ToString(),
                                 dataReader[ "shipment_code" ].ToString(),
                                 dataReader[ "magento2_shipment_code" ].ToString()
                            ) );
                        }
                        dataReader.Close();
                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        return list;
                    }

                return new List<ShipmentMethod>();
            }
            catch {
                return new List<ShipmentMethod>();
            }
        }

        public List<WorkSource> LoadWorkSources( int _customer_id ) {
            try {
                if( this.state != System.Data.ConnectionState.Open )
                    if( this.OpenConnection() ) {
                        string _query = "SELECT * FROM m_work_sources WHERE customer_id=@customer_id";
                        MySqlCommand cmd = new MySqlCommand( _query, Connection );
                        cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id.ToString() ) );
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        List<WorkSource> list = new List<WorkSource>();
                        while( dataReader.Read() ) {
                            list.Add( new WorkSource(
                                int.Parse( dataReader[ "id" ].ToString() ),
                                 int.Parse( dataReader[ "customer_id" ].ToString() ),
                                 dataReader[ "name" ].ToString(),
                                 dataReader[ "type" ].ToString(),
                                 dataReader[ "direction" ].ToString(),
                                 Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) )
                            ) );
                        }
                        dataReader.Close();
                        if( state == System.Data.ConnectionState.Open )
                            this.CloseConnection();

                        return list;
                    }

                return new List<WorkSource>();
            }
            catch {
                return new List<WorkSource>();
            }
        }

        public bool LogToServer( string _title, string _message, int _customer_id, string _worker = "general" ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "INSERT INTO log (title,message,worker,customer_id) VALUES (@title,@message,@worker,@customer_id);";
                MySqlCommand cmd = new MySqlCommand( query, connection );
                cmd.Parameters.Add( new MySqlParameter() { ParameterName = "title", Value = _title } );
                cmd.Parameters.Add( new MySqlParameter() { ParameterName = "message", Value = _message } );
                cmd.Parameters.Add( new MySqlParameter() { ParameterName = "worker", Value = _worker } );
                cmd.Parameters.Add( new MySqlParameter() { ParameterName = "customer_id", Value = _customer_id } );
                int value = cmd.ExecuteNonQuery();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                WriteLogLine( "log|" + _title + ":" + _message, ConsoleColor.Green );
                return true;
            }
            catch( Exception ex ) {
                OnError( "LOG ERROR GG - " + ex.Message );
                return false;
            }
        }
        #endregion

        #region Notifications
        public bool InsertNotifications( int _customer_id, List<Notification> _notifications ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                foreach( Notification item in _notifications ) {
                    string _query = "INSERT INTO notifications (customer_id,type,order_label,product_sku,xproduct_barcode,invoice_no,notification_content) VALUES (@customer_id,@type,@order_label,@product_sku,@xproduct_barcode,@invoice_no,@notification_content)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "type", item.type ) );
                    cmd.Parameters.Add( new MySqlParameter( "notification_content", item.notification_content ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "product_sku", item.product_sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "xproduct_barcode", item.xproduct_barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", item.invoice_no ) );
                    val += cmd.ExecuteNonQuery();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateNotifications( int _customer_id, List<Notification> _notifications ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                foreach( Notification item in _notifications ) {
                    string _query = "UPDATE notifications SET notification_content=@notification_content,is_notification_sent=@is_notification_sent,notification_date=@notification_date WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "notification_content", item.notification_content ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_notification_sent", item.is_notification_sent ) );
                    cmd.Parameters.Add( new MySqlParameter( "notification_date", DateTime.Now ) );
                    val += cmd.ExecuteNonQuery();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<Notification>? GetNotifications( int _customer_id, bool? _is_notification_sent ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = (!_is_notification_sent.HasValue ? "SELECT * FROM notifications WHERE customer_id=@customer_id" :
                    "SELECT * FROM notifications WHERE is_notification_sent = " + (_is_notification_sent.Value ? "1" : "0") + " AND customer_id=@customer_id");
                List<Notification> list = new List<Notification>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Notification n = new Notification {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        type = (Notification.NotificationTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        order_label = dataReader[ "order_label" ].ToString(),
                        product_sku = dataReader[ "product_sku" ].ToString(),
                        xproduct_barcode = dataReader[ "xproduct_barcode" ].ToString(),
                        invoice_no = dataReader[ "invoice_no" ].ToString(),
                        notification_content = dataReader[ "notification_content" ].ToString(),
                        is_notification_sent = Convert.ToBoolean( int.Parse( dataReader[ "is_notification_sent" ].ToString() ) ),
                        create_date = !string.IsNullOrWhiteSpace( dataReader[ "create_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "create_date" ].ToString() ) : null,
                        notification_date = !string.IsNullOrWhiteSpace( dataReader[ "notification_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "notification_date" ].ToString() ) : null,
                    };
                    list.Add( n );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion

        #region Sync Mappings
        public List<SyncMapping>? GetCustomerSyncMappings( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM sync_mapping WHERE customer_id=@customer_id";
                List<SyncMapping> list = new List<SyncMapping>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    SyncMapping sm = new SyncMapping {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        attribute_source = dataReader[ "attribute_source" ].ToString(),
                        attribute_type = dataReader[ "attribute_type" ].ToString(),
                        variable_type = dataReader[ "variable_type" ].ToString(),
                        product_attribute = dataReader[ "product_attribute" ].ToString(),
                        work_source = dataReader[ "work_source" ].ToString(),
                        source_attribute = dataReader[ "source_attribute" ].ToString(),
                        regex = dataReader[ "regex" ].ToString(),
                        is_active = dataReader[ "is_active" ] != null ? dataReader[ "is_active" ].ToString() == "1" ? true : false : false,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null
                    };
                    list.Add( sm );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        #endregion

        #region Products

        public List<Product>? GetProducts( int _customer_id, bool _with_ext = true ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM products WHERE customer_id=@customer_id";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Product p = new Product {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        source_product_id = int.Parse( dataReader[ "source_product_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        sku = dataReader[ "sku" ].ToString(),
                        type = (Product.ProductTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        total_qty = int.Parse( dataReader[ "total_qty" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        price = decimal.Parse( dataReader[ "price" ].ToString() ),
                        special_price = decimal.Parse( dataReader[ "special_price" ].ToString() ),
                        custom_price = decimal.Parse( dataReader[ "custom_price" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = Convert.ToBoolean( int.Parse( dataReader[ "tax_included" ].ToString() ) ),
                    };
                    list.Add( p );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _with_ext ) {
                    var exts = GetProductExts( _customer_id );
                    if( exts != null ) {
                        var brands = GetBrands( _customer_id );
                        foreach( var item in list ) {
                            item.extension = exts.Where( x => x.sku == item.sku ).FirstOrDefault();
                            item.brand = brands.Where( x => x.id == item.extension.brand_id ).FirstOrDefault();
                        }
                    }
                }

                var product_sources = GetProductSources( _customer_id );
                if( product_sources != null )
                    foreach( var item in list ) {
                        item.sources = product_sources.Where( x => x.sku == item.sku ).ToList();
                    }


                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<Product>? GetXMLEnabledProducts( int _customer_id, bool _val = true, bool _with_ext = true ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT p.id,p.customer_id,p.source_product_id,p.update_date,p.sku,p.type,p.total_qty,p.name,p.barcode,p.price,p.special_price,p.custom_price,p.currency,p.tax,p.tax_included " +
                    "FROM products_ext AS ext INNER JOIN products AS p ON ext.sku=p.sku WHERE ext.is_xml_enabled=@is_xml_enabled AND ext.customer_id=@customer_id";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "is_xml_enabled", _val ? 1 : 0 ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Product p = new Product {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        source_product_id = int.Parse( dataReader[ "source_product_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        sku = dataReader[ "sku" ].ToString(),
                        type = (Product.ProductTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        total_qty = int.Parse( dataReader[ "total_qty" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        price = decimal.Parse( dataReader[ "price" ].ToString() ),
                        special_price = decimal.Parse( dataReader[ "special_price" ].ToString() ),
                        custom_price = decimal.Parse( dataReader[ "custom_price" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = Convert.ToBoolean( int.Parse( dataReader[ "tax_included" ].ToString() ) ),
                    };
                    list.Add( p );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _with_ext ) {
                    var exts = GetProductExts( _customer_id );
                    if( exts != null ) {
                        var brands = GetBrands( _customer_id );
                        foreach( var item in list ) {
                            item.extension = exts.Where( x => x.sku == item.sku ).FirstOrDefault();
                            item.brand = brands.Where( x => x.id == item.extension.brand_id ).FirstOrDefault();
                        }
                    }
                }

                var product_sources = GetProductSources( _customer_id );
                if( product_sources != null )
                    foreach( var item in list ) {
                        item.sources = product_sources.Where( x => x.sku == item.sku ).ToList();
                    }


                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<Product>? GetProducts( int _customer_id, int _items_per_page, int _current_page_index, bool _with_ext = true ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM products WHERE customer_id=@customer_id ORDER BY id DESC LIMIT @start,@end";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "start", _items_per_page * (_current_page_index) ) );
                cmd.Parameters.Add( new MySqlParameter( "end", _items_per_page ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Product p = new Product {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        source_product_id = int.Parse( dataReader[ "source_product_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        sku = dataReader[ "sku" ].ToString(),
                        type = (Product.ProductTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        total_qty = int.Parse( dataReader[ "total_qty" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        price = decimal.Parse( dataReader[ "price" ].ToString() ),
                        special_price = decimal.Parse( dataReader[ "special_price" ].ToString() ),
                        custom_price = decimal.Parse( dataReader[ "custom_price" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = Convert.ToBoolean( int.Parse( dataReader[ "tax_included" ].ToString() ) ),
                    };
                    list.Add( p );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _with_ext ) {
                    var exts = GetProductExts( _customer_id );
                    if( exts != null ) {
                        var brands = GetBrands( _customer_id );
                        foreach( var item in list ) {
                            item.extension = exts.Where( x => x.sku == item.sku ).FirstOrDefault();
                            item.brand = brands.Where( x => x.id == item.extension.brand_id ).FirstOrDefault();
                        }
                    }
                }

                var product_sources = GetProductSources( _customer_id );
                if( product_sources != null )
                    foreach( var item in list ) {
                        item.sources = product_sources.Where( x => x.sku == item.sku ).ToList();
                    }


                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public int GetProductsCount( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT COUNT(*) FROM products WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                int.TryParse( cmd.ExecuteScalar().ToString(), out int total_count );
                return total_count;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return 0;
            }
        }

        public List<Product>? SearchProducts( int _customer_id, string _keyword, bool _with_ext = true ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM products WHERE (sku LIKE @keyword OR name LIKE @keyword OR barcode LIKE @keyword) AND customer_id=@customer_id ORDER BY id DESC";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "keyword", string.Format( "%{0}%", _keyword ) ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Product p = new Product {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        source_product_id = int.Parse( dataReader[ "source_product_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        sku = dataReader[ "sku" ].ToString(),
                        type = (Product.ProductTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        total_qty = int.Parse( dataReader[ "total_qty" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        price = decimal.Parse( dataReader[ "price" ].ToString() ),
                        special_price = decimal.Parse( dataReader[ "special_price" ].ToString() ),
                        custom_price = decimal.Parse( dataReader[ "custom_price" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = Convert.ToBoolean( int.Parse( dataReader[ "tax_included" ].ToString() ) ),
                    };
                    list.Add( p );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _with_ext ) {
                    var exts = GetProductExts( _customer_id );
                    if( exts != null ) {
                        var brands = GetBrands( _customer_id );
                        foreach( var item in list ) {
                            item.extension = exts.Where( x => x.sku == item.sku ).FirstOrDefault();
                            item.brand = brands.Where( x => x.id == item.extension.brand_id ).FirstOrDefault();
                        }
                    }
                }

                var product_sources = GetProductSources( _customer_id );
                if( product_sources != null )
                    foreach( var item in list ) {
                        item.sources = product_sources.Where( x => x.sku == item.sku ).ToList();
                    }


                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Product? GetProductBySku( int _customer_id, string _sku ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM products WHERE sku=@sku AND customer_id=@customer_id";
                List<Product> list = new List<Product>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _sku ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if( dataReader.Read() ) {
                    Product p = new Product {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        source_product_id = int.Parse( dataReader[ "source_product_id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        sku = dataReader[ "sku" ].ToString(),
                        type = (Product.ProductTypes)int.Parse( dataReader[ "type" ].ToString() ),
                        total_qty = int.Parse( dataReader[ "total_qty" ].ToString() ),
                        name = dataReader[ "name" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        price = decimal.Parse( dataReader[ "price" ].ToString() ),
                        special_price = decimal.Parse( dataReader[ "special_price" ].ToString() ),
                        custom_price = decimal.Parse( dataReader[ "custom_price" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = Convert.ToBoolean( int.Parse( dataReader[ "tax_included" ].ToString() ) )
                    };
                    list.Add( p );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                var ss = GetProductSources( _customer_id, _sku );
                if( ss != null ) {
                    list[ 0 ].sources = new List<ProductSource>();
                    list[ 0 ].sources.AddRange( ss );
                }

                list[ 0 ].extension = GetProductExt( _customer_id, list[ 0 ].sku );
                list[ 0 ].brand = GetBrand( _customer_id, list[ 0 ].extension.brand_id );

                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list[ 0 ];
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertProducts( int _customer_id, List<Product> _products, bool _with_ext = true ) {
            try {
                int val = 0;
                foreach( Product item in _products ) {
                    string _query = "INSERT INTO products (customer_id,source_product_id,sku,type,barcode,total_qty,price,special_price,custom_price,currency,tax,tax_included,name,sources) VALUES (@customer_id,@source_product_id,@sku,@type,@barcode,@total_qty,@price,@special_price,@custom_price,@currency,@tax,@tax_included,@name,@sources)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_product_id", item.source_product_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "sku", item.sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "type", item.type ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "total_qty", item.sources.Where( x => x.is_active ).Sum( x => x.qty ) ) );
                    cmd.Parameters.Add( new MySqlParameter( "price", item.price ) );
                    cmd.Parameters.Add( new MySqlParameter( "special_price", item.special_price ) );
                    cmd.Parameters.Add( new MySqlParameter( "custom_price", item.custom_price ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    cmd.Parameters.Add( new MySqlParameter( "tax", item.tax ) );
                    cmd.Parameters.Add( new MySqlParameter( "tax_included", item.tax_included ) );
                    cmd.Parameters.Add( new MySqlParameter( "name", item.name ) );
                    cmd.Parameters.Add( new MySqlParameter( "sources", string.Join( ",", item.sources.Where( x => x.is_active ).Select( x => x.name ) ) ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                    foreach( var source_item in item.sources )
                        InsertProductSource( _customer_id, source_item );
                    if( _with_ext ) {
                        if( item.extension != null ) {
                            InsertProductExt( _customer_id, item.extension );
                        }
                    }
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateProducts( int _customer_id, List<Product> _products, bool _with_ext = true ) {
            try {
                int val = 0;
                foreach( Product item in _products ) {
                    string _query = "UPDATE products SET type=@type,barcode=@barcode,total_qty=@total_qty,price=@price,special_price=@special_price,custom_price=@custom_price,currency=@currency,tax=@tax,tax_included=@tax_included,update_date=@update_date,name=@name,sources=@sources,source_product_id=@source_product_id WHERE id=@id AND sku=@sku AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_product_id", item.source_product_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    cmd.Parameters.Add( new MySqlParameter( "sku", item.sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "type", item.type ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "price", item.price ) );
                    cmd.Parameters.Add( new MySqlParameter( "special_price", item.special_price ) );
                    cmd.Parameters.Add( new MySqlParameter( "custom_price", item.custom_price ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    cmd.Parameters.Add( new MySqlParameter( "tax", item.tax ) );
                    cmd.Parameters.Add( new MySqlParameter( "tax_included", item.tax_included ) );
                    cmd.Parameters.Add( new MySqlParameter( "name", item.name ) );
                    cmd.Parameters.Add( new MySqlParameter( "total_qty", item.sources.Where( x => x.is_active ).Sum( x => x.qty ) ) );
                    cmd.Parameters.Add( new MySqlParameter( "sources", string.Join( ",", item.sources.Where( x => x.is_active ).Select( x => x.name ).ToArray() ) ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                    DeleteProductSources( _customer_id, item.sku.ToString() );
                    foreach( var source_item in item.sources )
                        InsertProductSource( _customer_id, source_item );
                    if( _with_ext && item.extension != null ) {
                        UpdateProductExt( _customer_id, item.extension );

                    }
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateXMLStatusByProductBarcode( int _customer_id, string _barcode, bool _is_xml_enabled, string[]? _xml_sources, bool _with_ext = true ) {
            try {
                int val = 0;
                if( !string.IsNullOrWhiteSpace( _barcode ) ) {
                    string _query = "UPDATE products_ext SET is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources WHERE barcode=@barcode AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", _barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_xml_enabled", _is_xml_enabled ? 1 : 0 ) );
                    cmd.Parameters.Add( new MySqlParameter( "xml_sources", _xml_sources != null ? string.Join( ",", _xml_sources ) : null ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();

                    if( val > 0 )
                        return true;
                    else return false;
                }
                return true;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public ProductExtension? GetProductExt( int _customer_id, string _sku ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM products_ext WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _sku ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                ProductExtension px = null;
                if( dataReader.Read() ) {
                    px = new ProductExtension();
                    px.id = int.Parse( dataReader[ "id" ].ToString() );
                    px.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    px.brand_id = int.Parse( dataReader[ "brand_id" ].ToString() );
                    px.category_ids = dataReader[ "category_ids" ].ToString();
                    px.sku = dataReader[ "sku" ].ToString();
                    px.barcode = dataReader[ "barcode" ].ToString();
                    px.is_xml_enabled = Convert.ToBoolean( int.Parse( dataReader[ "is_xml_enabled" ].ToString() ) );
                    px.xml_sources = dataReader[ "xml_sources" ].ToString().Split( ',' );
                    px.update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return px;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<ProductExtension>? GetProductExts( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM products_ext WHERE customer_id=@customer_id";
                List<ProductExtension> list = new List<ProductExtension>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    ProductExtension s = new ProductExtension {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        brand_id = int.Parse( dataReader[ "brand_id" ].ToString() ),
                        category_ids = dataReader[ "category_ids" ].ToString(),
                        sku = dataReader[ "sku" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        is_xml_enabled = dataReader[ "is_xml_enabled" ] != null ? dataReader[ "is_xml_enabled" ].ToString() == "1" ? true : false : false,
                        xml_sources = dataReader[ "xml_sources" ].ToString().Split( ',' ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null
                    };
                    list.Add( s );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertProductExt( int _customer_id, ProductExtension _source ) {
            try {
                var temp_ext = GetProductExt( _customer_id, _source.sku );
                if( temp_ext != null ) throw new Exception( "Sku is already in table." );

                int val = 0;
                string _query = "INSERT INTO products_ext (customer_id,brand_id,category_ids,sku,barcode) VALUES " +
                    "(@customer_id,@brand_id,@category_ids,@sku,@barcode)"; //,@is_xml_enabled,@xml_sources
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "brand_id", _source.brand_id ) );
                cmd.Parameters.Add( new MySqlParameter( "category_ids", _source.category_ids ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _source.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "barcode", _source.barcode ) );
                //cmd.Parameters.Add( new MySqlParameter( "is_xml_enabled", _source.is_xml_enabled ) );
                //cmd.Parameters.Add( new MySqlParameter( "xml_sources", string.Join( ",", _source.xml_sources ) ) );
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                val = cmd.ExecuteNonQuery();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        /// <summary>
        /// SKU based update
        /// </summary>
        /// <param name="_customer_id"></param>
        /// <param name="_source"></param>
        /// <returns></returns>
        public bool UpdateProductExt( int _customer_id, ProductExtension _source ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "UPDATE products_ext SET brand_id=@brand_id,category_ids=@category_ids,barcode=@barcode,is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources,update_date=@update_date " +
                    "WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _source.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "brand_id", _source.brand_id ) );
                cmd.Parameters.Add( new MySqlParameter( "category_ids", _source.category_ids ) );
                cmd.Parameters.Add( new MySqlParameter( "barcode", _source.barcode ) );
                cmd.Parameters.Add( new MySqlParameter( "xml_sources", string.Join( ",", _source.xml_sources ) ) );
                cmd.Parameters.Add( new MySqlParameter( "is_xml_enabled", _source.is_xml_enabled ) );
                cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool DeleteProductExt( int _customer_id, string _sku ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM products_ext WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "product_id", _sku ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        [Obsolete]
        public bool UpdateProductSource( int _customer_id, ProductSource _source ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "UPDATE product_sources SET name=@name,qty=@qty,is_active=@is_active WHERE sku=@sku AND barcode=@barcode AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "name", _source.name ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _source.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "barcode", _source.barcode ) );
                cmd.Parameters.Add( new MySqlParameter( "qty", _source.qty ) );
                cmd.Parameters.Add( new MySqlParameter( "is_active", _source.is_active ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<ProductSource>? GetProductSources( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM product_sources WHERE customer_id=@customer_id";
                List<ProductSource> list = new List<ProductSource>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    ProductSource s = new ProductSource {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        sku = dataReader[ "sku" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        name = dataReader[ "name" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        is_active = Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) ),
                    };
                    list.Add( s );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<ProductSource>? GetProductSources( int _customer_id, string? _sku ) {
            try {
                if( _sku == null ) return null;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                List<ProductSource> list = new List<ProductSource>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _sku ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    ProductSource s = new ProductSource {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        sku = _sku,
                        barcode = dataReader[ "barcode" ].ToString(),
                        name = dataReader[ "name" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        is_active = Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) ),
                    };
                    list.Add( s );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertProductSource( int _customer_id, ProductSource _source ) {
            try {
                int val = 0;
                string _query = "INSERT INTO product_sources (customer_id,name,sku,barcode,qty,is_active) VALUES (@customer_id,@name,@sku,@barcode,@qty,@is_active)";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "name", _source.name ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _source.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "barcode", _source.barcode ) );
                cmd.Parameters.Add( new MySqlParameter( "qty", _source.qty ) );
                cmd.Parameters.Add( new MySqlParameter( "is_active", _source.is_active ) );
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                val = cmd.ExecuteNonQuery();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool DeleteProductSources( int _customer_id, string _sku ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _sku ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion

        #region Brand
        public Brand? GetBrand( int _customer_id, int _id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM brands " +
                    "WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "id", _id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if( dataReader.Read() ) {
                    b = new Brand();
                    b.id = int.Parse( dataReader[ "id" ].ToString() );
                    b.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    b.brand_name = dataReader[ "brand_name" ].ToString();
                    b.status = dataReader[ "status" ] != null ? dataReader[ "status" ].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                return b ??= null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<Brand> GetBrands( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM brands WHERE customer_id=@customer_id";
                List<Brand> list = new List<Brand>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Brand b = new Brand {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        brand_name = dataReader[ "brand_name" ].ToString(),
                        status = Convert.ToBoolean( int.Parse( dataReader[ "status" ].ToString() ) ),
                    };
                    list.Add( b );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Brand? GetBrand( int _customer_id, string _sku ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT b.id AS 'BID', b.brand_name,b.status AS 'brand_status',b.customer_id AS 'customer' FROM products_ext AS pex INNER JOIN brands AS b ON pex.brand_id=b.id " +
                    "WHERE pex.sku=@sku AND b.customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _sku ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if( dataReader.Read() ) {
                    b = new Brand();
                    b.id = int.Parse( dataReader[ "BID" ].ToString() );
                    b.customer_id = int.Parse( dataReader[ "customer" ].ToString() );
                    b.brand_name = dataReader[ "brand_name" ].ToString();
                    b.status = dataReader[ "brand_status" ] != null ? dataReader[ "brand_status" ].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                return b ??= null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Brand? GetBrandByName( int _customer_id, string _name ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM brands " +
                    "WHERE UPPER(brand_name)=UPPER(@brand_name) AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "brand_name", _name ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Brand? b = null;
                if( dataReader.Read() ) {
                    b = new Brand();
                    b.id = int.Parse( dataReader[ "id" ].ToString() );
                    b.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    b.brand_name = dataReader[ "brand_name" ].ToString();
                    b.status = dataReader[ "status" ] != null ? dataReader[ "status" ].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                return b ??= null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Brand? InsertBrand( int _customer_id, Brand _brand ) {
            try {
                object val; int inserted_id;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO brands (customer_id,brand_name,status) VALUES (@customer_id,@brand_name,@status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "brand_name", _brand.brand_name ) );
                cmd.Parameters.Add( new MySqlParameter( "status", _brand.status ) );
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                val = cmd.ExecuteScalar();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( val != null ) {
                    if( int.TryParse( val.ToString(), out inserted_id ) ) {
                        return GetBrand( _customer_id, inserted_id );
                    }
                }
                return null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion

        #region Category
        /// <summary>
        /// GetRootCategory
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_parent_id">1 selecting customer root cateogry</param>
        /// <returns></returns>
        public Category GetRootCategory( int _customer_id, int _parent_id = 1 ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM categories " +
                    "WHERE parent_id=@parent_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "parent_id", _parent_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Category c = null;
                if( dataReader.Read() ) {
                    c = new Category();
                    c.id = int.Parse( dataReader[ "id" ].ToString() );
                    c.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    c.parent_id = int.Parse( dataReader[ "parent_id" ].ToString() );
                    c.category_name = dataReader[ "category_name" ].ToString();
                    c.is_active = dataReader[ "is_active" ] != null ? dataReader[ "is_active" ].ToString() == "1" ? true : false : false;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                return c ??= null;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }
        #endregion


        #region XML Source
        public List<XProduct> GetXProducts( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE customer_id=@customer_id";
                List<XProduct> list = new List<XProduct>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    XProduct xp = new XProduct {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        barcode = dataReader[ "barcode" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        source_sku = dataReader[ "source_sku" ].ToString(),
                        source_brand = dataReader[ "source_brand" ].ToString(),
                        source_product_group = dataReader[ "source_product_group" ].ToString(),
                        xml_source = dataReader[ "xml_source" ].ToString(),
                        price1 = decimal.Parse( dataReader[ "price1" ].ToString() ),
                        price2 = decimal.Parse( dataReader[ "price2" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        is_infosent = Convert.ToBoolean( int.Parse( dataReader[ "is_infosent" ].ToString() ) ),
                        is_active = Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) )
                    };

                    list.Add( xp );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return new();
            }
        }

        public List<XProduct> GetXProducts( int _customer_id, string _xml_source ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE xml_source=@xml_source AND customer_id=@customer_id";
                List<XProduct> list = new List<XProduct>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "xml_source", _xml_source ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    XProduct xp = new XProduct {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        barcode = dataReader[ "barcode" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        source_sku = dataReader[ "source_sku" ].ToString(),
                        source_brand = dataReader[ "source_brand" ].ToString(),
                        source_product_group = dataReader[ "source_product_group" ].ToString(),
                        xml_source = dataReader[ "xml_source" ].ToString(),
                        price1 = decimal.Parse( dataReader[ "price1" ].ToString() ),
                        price2 = decimal.Parse( dataReader[ "price2" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        is_infosent = Convert.ToBoolean( int.Parse( dataReader[ "is_infosent" ].ToString() ) ),
                        is_active = Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) )
                    };

                    list.Add( xp );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return new();
            }
        }

        public List<XProduct> GetXProduct( int _customer_id, string _barcode ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM xml_products WHERE barcode=@barcode AND customer_id=@customer_id";
                List<XProduct> list = new List<XProduct>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "barcode", _barcode ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    XProduct xp = new XProduct {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        barcode = dataReader[ "barcode" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        source_sku = dataReader[ "source_sku" ].ToString(),
                        source_brand = dataReader[ "source_brand" ].ToString(),
                        source_product_group = dataReader[ "source_product_group" ].ToString(),
                        xml_source = dataReader[ "xml_source" ].ToString(),
                        price1 = decimal.Parse( dataReader[ "price1" ].ToString() ),
                        price2 = decimal.Parse( dataReader[ "price2" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        is_infosent = Convert.ToBoolean( int.Parse( dataReader[ "is_infosent" ].ToString() ) ),
                        is_active = Convert.ToBoolean( int.Parse( dataReader[ "is_active" ].ToString() ) )
                    };

                    list.Add( xp );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return new();
            }
        }

        public bool InsertXProducts( int _customer_id, List<XProduct> _products ) {
            try {
                int val = 0;
                foreach( XProduct item in _products ) {
                    string _query = "INSERT INTO xml_products (customer_id,barcode,source_sku,source_brand,source_product_group,xml_source,qty,price1,price2,currency) VALUES " +
                        "(@customer_id,@barcode,@source_sku,@source_brand,@source_product_group,@xml_source,@qty,@price1,@price2,@currency)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_sku", item.source_sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_brand", item.source_brand ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_product_group", item.source_product_group ) );
                    cmd.Parameters.Add( new MySqlParameter( "xml_source", item.xml_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "qty", item.qty ) );
                    cmd.Parameters.Add( new MySqlParameter( "price1", item.price1 ) );
                    cmd.Parameters.Add( new MySqlParameter( "price2", item.price2 ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    //cmd.Parameters.Add( new MySqlParameter( "is_active", item.is_active ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateXProducts( int _customer_id, List<XProduct> _products ) {
            try {
                int val = 0;
                foreach( XProduct item in _products ) {
                    string _query = "UPDATE xml_products SET source_sku=@source_sku,source_brand=@source_brand,source_product_group=@source_product_group,xml_source=@xml_source,qty=@qty,price1=@price1,price2=@price2,currency=@currency,is_infosent=@is_infosent,is_active=@is_active,update_date=@update_date WHERE id=@id AND barcode=@barcode AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_sku", item.source_sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_brand", item.source_brand ) );
                    cmd.Parameters.Add( new MySqlParameter( "source_product_group", item.source_product_group ) );
                    cmd.Parameters.Add( new MySqlParameter( "xml_source", item.xml_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "qty", item.qty ) );
                    cmd.Parameters.Add( new MySqlParameter( "price1", item.price1 ) );
                    cmd.Parameters.Add( new MySqlParameter( "price2", item.price2 ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_infosent", item.is_infosent ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_active", item.is_active ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool DeleteXProduct( int _customer_id, int _id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM xml_products WHERE id=@id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "id", _id ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion


        #region Shipments
        public List<Shipment>? GetShipments( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM shipments WHERE customer_id=@customer_id";
                List<Shipment> list = new List<Shipment>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Shipment s = new Shipment {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        order_label = dataReader[ "order_label" ].ToString(),
                        shipment_method = dataReader[ "shipment_method" ].ToString(),
                        order_source = dataReader[ "order_source" ].ToString(),
                        barcode = dataReader[ "barcode" ].ToString(),
                        is_shipped = dataReader[ "is_shipped" ] != null ? dataReader[ "is_shipped" ].ToString() == "1" ? true : false : false,
                        tracking_number = dataReader[ "tracking_number" ].ToString(),
                        order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : null,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                        shipment_date = !string.IsNullOrWhiteSpace( dataReader[ "shipment_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "shipment_date" ].ToString() ) : null,
                    };
                    list.Add( s );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Shipment? GetShipment( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM shipments WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Shipment s = null;
                if( dataReader.Read() ) {
                    s = new Shipment();
                    s.id = int.Parse( dataReader[ "id" ].ToString() );
                    s.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    s.order_id = int.Parse( dataReader[ "order_id" ].ToString() );
                    s.order_label = dataReader[ "order_label" ].ToString();
                    s.shipment_method = dataReader[ "shipment_method" ].ToString();
                    s.order_source = dataReader[ "order_source" ].ToString();
                    s.barcode = dataReader[ "barcode" ].ToString();
                    s.is_shipped = dataReader[ "is_shipped" ] != null ? dataReader[ "is_shipped" ].ToString() == "1" ? true : false : false;
                    s.tracking_number = dataReader[ "tracking_number" ].ToString();
                    s.order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : null;
                    s.update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null;
                    s.shipment_date = !string.IsNullOrWhiteSpace( dataReader[ "shipment_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "shipment_date" ].ToString() ) : null;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                return s;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertShipments( int _customer_id, List<Shipment> _shipments ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                foreach( Shipment item in _shipments ) {
                    string _query = "INSERT INTO shipments (customer_id,order_id,order_label,order_source,barcode,is_shipped,tracking_number,shipment_date,order_date,shipment_method) VALUES (@customer_id,@order_id,@order_label,@order_source,@barcode,@is_shipped,@tracking_number,@shipment_date,@order_date,@shipment_method)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_source", item.order_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_method", item.shipment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_shipped", item.is_shipped ) );
                    cmd.Parameters.Add( new MySqlParameter( "tracking_number", item.tracking_number ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_date", item.shipment_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    val += cmd.ExecuteNonQuery();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateShipments( int _customer_id, List<Shipment> _shipments ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                foreach( Shipment item in _shipments ) {
                    string _query = "UPDATE shipments SET order_label=@order_label,order_source=@order_source,barcode=@barcode,is_shipped=@is_shipped,tracking_number=@tracking_number,shipment_date=@shipment_date,order_date=@order_date,update_date=@update_date,shipment_method=@shipment_method WHERE order_id=@order_id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_source", item.order_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_method", item.shipment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "barcode", item.barcode ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_shipped", item.is_shipped ) );
                    cmd.Parameters.Add( new MySqlParameter( "tracking_number", item.tracking_number ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_date", item.shipment_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    val += cmd.ExecuteNonQuery();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetShipped( int _customer_id, string _order_label, string _tracking_numbers, bool _value = true ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE shipments SET is_shipped=" + (_value ? "1" : "0") + ",shipment_date=@shipment_date,tracking_number=@tracking_number WHERE order_label=@order_label AND customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_date", DateTime.Now ) );
                    cmd.Parameters.Add( new MySqlParameter( "tracking_number", _tracking_numbers ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", _order_label ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion


        #region Invoices
        public List<Invoice>? GetInvoices( int _customer_id, bool _with_ext = true ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM invoices WHERE customer_id=@customer_id";
                List<Invoice> list = new List<Invoice>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Invoice i = new Invoice {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = dataReader[ "order_id" ].ToString(),
                        order_label = dataReader[ "order_label" ].ToString(),
                        erp_customer_code = dataReader[ "erp_customer_code" ].ToString(),
                        erp_customer_group = dataReader[ "erp_customer_group" ].ToString(),
                        is_belge_created = dataReader[ "is_belge_created" ] != null ? dataReader[ "is_belge_created" ].ToString() == "1" ? true : false : false,
                        erp_no = dataReader[ "erp_no" ].ToString(),
                        invoice_no = dataReader[ "invoice_no" ].ToString(),
                        belge_url = dataReader[ "belge_url" ].ToString(),
                        gib_fatura_no = dataReader[ "gib_fatura_no" ].ToString(),
                        order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : null,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                    };
                    list.Add( i );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _with_ext ) {
                    var items = GetInvoiceItems( _customer_id );
                    if( items != null ) {
                        foreach( var item in list ) {
                            item.items = items.Where( x => x.erp_no == item.erp_no ).ToList();
                        }
                    }
                }
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Invoice? GetInvoice( int _customer_id, string _erp_no ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM invoices WHERE erp_no=@erp_no AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _erp_no ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Invoice i = null;
                if( dataReader.Read() ) {
                    i = new Invoice();
                    i.id = int.Parse( dataReader[ "id" ].ToString() );
                    i.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    i.order_id = dataReader[ "order_id" ].ToString();
                    i.order_label = dataReader[ "order_label" ].ToString();
                    i.is_belge_created = dataReader[ "is_belge_created" ] != null ? dataReader[ "is_belge_created" ].ToString() == "1" ? true : false : false;
                    i.erp_customer_code = dataReader[ "erp_customer_code" ].ToString();
                    i.erp_customer_group = dataReader[ "erp_customer_group" ].ToString();
                    i.erp_no = dataReader[ "erp_no" ].ToString();
                    i.invoice_no = dataReader[ "invoice_no" ].ToString();
                    i.belge_url = dataReader[ "belge_url" ].ToString();
                    i.gib_fatura_no = dataReader[ "gib_fatura_no" ].ToString();
                    i.order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : null;
                    i.update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( i != null )
                    i.items = GetInvoiceItems( _customer_id, _erp_no );
                return i;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertInvoices( int _customer_id, List<Invoice> _invoices, bool _with_items = true ) {
            try {
                int val = 0;
                foreach( Invoice item in _invoices ) {
                    string _query = "INSERT INTO invoices (customer_id,order_id,order_label,erp_customer_code,erp_customer_group,erp_no,invoice_no,gib_fatura_no,order_date) VALUES " +
                        "(@customer_id,@order_id,@order_label,@erp_customer_code,@erp_customer_group,@erp_no,@invoice_no,@gib_fatura_no,@order_date)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_customer_code", item.erp_customer_code ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_customer_group", item.erp_customer_group ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", item.invoice_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "gib_fatura_no", item.gib_fatura_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();

                    if( _with_items ) {
                        if( item.items != null && item.items.Count > 0 ) {
                            if( item.erp_no != "" ) {
                                InsertInvoiceItems( _customer_id, item.items );
                            }
                        }
                    }
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateInvoices( int _customer_id, List<Invoice> _invoices, bool _with_ext = true ) {
            try {
                int val = 0;
                foreach( Invoice item in _invoices ) {
                    string _query = "UPDATE invoices SET order_label=@order_label,erp_customer_code=@erp_customer_code,erp_customer_group=@erp_customer_group,invoice_no=@invoice_no,gib_fatura_no=@gib_fatura_no,order_date=@order_date,update_date=@update_date WHERE id=@id AND erp_no=@erp_no AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_customer_code", item.erp_customer_code ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_customer_group", item.erp_customer_group ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", item.invoice_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "gib_fatura_no", item.gib_fatura_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();

                    if( _with_ext ) {
                        if( item.items != null && item.items.Count > 0 ) {
                            UpdateInvoiceItems( _customer_id, item.items );
                        }
                    }
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<InvoiceItem>? GetInvoiceItems( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM invoice_items WHERE customer_id=@customer_id";
                List<InvoiceItem> list = new List<InvoiceItem>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    InvoiceItem ii = new InvoiceItem {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        invoice_no = dataReader[ "invoice_no" ].ToString(),
                        erp_no = dataReader[ "erp_no" ].ToString(),
                        sku = dataReader[ "sku" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        serials = dataReader[ "serials" ].ToString().Split( ',' ),
                        create_date = !string.IsNullOrWhiteSpace( dataReader[ "create_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "create_date" ].ToString() ) : null,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null
                    };
                    list.Add( ii );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<InvoiceItem> GetInvoiceItems( int _customer_id, string _erp_no ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM invoice_items WHERE erp_no=@erp_no AND customer_id=@customer_id";
                List<InvoiceItem> list = new List<InvoiceItem>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "erp_no", _erp_no ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    InvoiceItem ii = new InvoiceItem {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        invoice_no = dataReader[ "invoice_no" ].ToString(),
                        erp_no = dataReader[ "erp_no" ].ToString(),
                        sku = dataReader[ "sku" ].ToString(),
                        qty = int.Parse( dataReader[ "qty" ].ToString() ),
                        serials = dataReader[ "xml_sources" ].ToString().Split( ',' ),
                        create_date = !string.IsNullOrWhiteSpace( dataReader[ "create_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "create_date" ].ToString() ) : null,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null
                    };
                    list.Add( ii );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertInvoiceItems( int _customer_id, List<InvoiceItem> _items ) {
            try {
                int val = 0;
                foreach( var item in _items ) {
                    string _query = "INSERT INTO invoice_items (customer_id,invoice_no,erp_no,sku,qty,serials) VALUES " +
                        "(@customer_id,@invoice_no,@erp_no,@sku,@qty,@serials)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", item.invoice_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "sku", item.sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "qty", item.qty ) );
                    cmd.Parameters.Add( new MySqlParameter( "serials", string.Join( ",", item.serials ) ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val = cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateInvoiceItems( int _customer_id, List<InvoiceItem> _items ) {
            try {
                int val = 0;
                foreach( InvoiceItem item in _items ) {
                    string _query = "UPDATE invoice_items SET invoice_no=@invoice_no,sku=@sku,qty=@qty,serials=@serials,update_date=@update_date " +
                        "WHERE id=@id AND erp_no=@erp_no AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "id", item.id ) );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", item.invoice_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "sku", item.sku ) );
                    cmd.Parameters.Add( new MySqlParameter( "qty", item.qty ) );
                    cmd.Parameters.Add( new MySqlParameter( "serials", string.Join( ",", item.serials ) ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", item.update_date ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                }

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetInvoiceCreated( int _customer_id, string _invoice_no, string _fullpath = "" ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE invoices SET is_belge_created=1,belge_url=@belge_url WHERE invoice_no=@invoice_no AND customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "invoice_no", _invoice_no ) );
                    cmd.Parameters.Add( new MySqlParameter( "belge_url", _fullpath ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion


        #region Orders
        public List<Order>? GetOrders( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM orders WHERE customer_id=@customer_id";
                List<Order> list = new List<Order>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Order o = new Order {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        email = dataReader[ "email" ].ToString(),
                        firstname = dataReader[ "firstname" ].ToString(),
                        lastname = dataReader[ "lastname" ].ToString(),
                        order_label = dataReader[ "order_label" ].ToString(),
                        order_source = dataReader[ "order_source" ].ToString(),
                        payment_method = dataReader[ "payment_method" ].ToString(),
                        shipment_method = dataReader[ "shipment_method" ].ToString(),
                        comment = dataReader[ "comment" ].ToString(),
                        order_shipping_barcode = dataReader[ "order_shipping_barcode" ].ToString(),
                        erp_no = dataReader[ "erp_no" ] != null ? dataReader[ "erp_no" ].ToString() : null,
                        is_erp_sent = dataReader[ "is_erp_sent" ] != null ? dataReader[ "is_erp_sent" ].ToString().Equals( "1" ) : false,
                        grand_total = float.Parse( dataReader[ "grand_total" ].ToString() ),
                        subtotal = float.Parse( dataReader[ "subtotal" ].ToString() ),
                        discount_amount = float.Parse( dataReader[ "discount_amount" ].ToString() ),
                        installment_amount = float.Parse( dataReader[ "installment_amount" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        order_status = dataReader[ "order_status" ].ToString(),
                        order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : DateTime.Now,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                    };
                    list.Add( o );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                var order_items = GetOrderItems( _customer_id );
                if( order_items != null )
                    foreach( var item in list ) {
                        item.order_items = order_items.Where( x => x.order_id == item.order_id ).ToList();
                    }
                var billing_items = GetBillingAddresses( _customer_id );
                if( billing_items != null )
                    foreach( var item in list ) {
                        item.billing_address = billing_items.Where( x => x.order_id == item.order_id )?.FirstOrDefault();
                    }
                var shipping_items = GetShippingAddresses( _customer_id );
                if( shipping_items != null )
                    foreach( var item in list ) {
                        item.shipping_address = shipping_items.Where( x => x.order_id == item.order_id )?.FirstOrDefault();
                    }

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<Order>? GetOrders( int _customer_id, string[] _order_statuses ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM orders WHERE customer_id=@customer_id";
                string _query_ext = "order_status IN (";
                if( _order_statuses.Length > 0 ) {
                    foreach( var item in _order_statuses ) {
                        _query_ext += "'" + item + "',";
                    }
                    _query_ext = _query_ext.Remove( _query_ext.Length - 1, 1 ) + ")";
                    _query = "SELECT * FROM orders WHERE " + _query_ext + " AND customer_id=@customer_id";
                }
                List<Order> list = new List<Order>();
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                //cmd.Parameters.Add( new MySqlParameter( "order_status", _order_statuses ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while( dataReader.Read() ) {
                    Order o = new Order {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        email = dataReader[ "email" ].ToString(),
                        firstname = dataReader[ "firstname" ].ToString(),
                        lastname = dataReader[ "lastname" ].ToString(),
                        order_label = dataReader[ "order_label" ].ToString(),
                        order_source = dataReader[ "order_source" ].ToString(),
                        payment_method = dataReader[ "payment_method" ].ToString(),
                        shipment_method = dataReader[ "shipment_method" ].ToString(),
                        comment = dataReader[ "comment" ].ToString(),
                        order_shipping_barcode = dataReader[ "order_shipping_barcode" ].ToString(),
                        erp_no = dataReader[ "erp_no" ] != null ? dataReader[ "erp_no" ].ToString() : null,
                        is_erp_sent = dataReader[ "is_erp_sent" ] != null ? dataReader[ "is_erp_sent" ].ToString().Equals( "1" ) : false,
                        grand_total = float.Parse( dataReader[ "grand_total" ].ToString() ),
                        subtotal = float.Parse( dataReader[ "subtotal" ].ToString() ),
                        discount_amount = float.Parse( dataReader[ "discount_amount" ].ToString() ),
                        installment_amount = float.Parse( dataReader[ "installment_amount" ].ToString() ),
                        currency = dataReader[ "currency" ].ToString(),
                        order_status = dataReader[ "order_status" ].ToString(),
                        order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : DateTime.Now,
                        update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null,
                    };
                    list.Add( o );
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                var order_items = GetOrderItems( _customer_id );
                if( order_items != null )
                    foreach( var item in list ) {
                        item.order_items = order_items.Where( x => x.order_id == item.order_id ).ToList();
                    }
                var billing_items = GetBillingAddresses( _customer_id );
                if( billing_items != null )
                    foreach( var item in list ) {
                        item.billing_address = billing_items.Where( x => x.order_id == item.order_id )?.FirstOrDefault();
                    }
                var shipping_items = GetShippingAddresses( _customer_id );
                if( shipping_items != null )
                    foreach( var item in list ) {
                        item.shipping_address = shipping_items.Where( x => x.order_id == item.order_id )?.FirstOrDefault();
                    }

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public Order? GetOrder( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string _query = "SELECT * FROM orders WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                Order o = null;
                if( dataReader.Read() ) {
                    o = new Order();
                    o.id = int.Parse( dataReader[ "id" ].ToString() );
                    o.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    o.order_id = int.Parse( dataReader[ "order_id" ].ToString() );
                    o.email = dataReader[ "email" ].ToString();
                    o.firstname = dataReader[ "firstname" ].ToString();
                    o.lastname = dataReader[ "lastname" ].ToString();
                    o.order_label = dataReader[ "order_label" ].ToString();
                    o.order_source = dataReader[ "order_source" ].ToString();
                    o.payment_method = dataReader[ "payment_method" ].ToString();
                    o.shipment_method = dataReader[ "shipment_method" ].ToString();
                    o.comment = dataReader[ "comment" ].ToString();
                    o.order_shipping_barcode = dataReader[ "order_shipping_barcode" ].ToString();
                    o.erp_no = dataReader[ "erp_no" ] != null ? dataReader[ "erp_no" ].ToString() : null;
                    o.is_erp_sent = dataReader[ "is_erp_sent" ] != null ? dataReader[ "is_erp_sent" ].ToString().Equals( "1" ) : false;
                    o.grand_total = float.Parse( dataReader[ "grand_total" ].ToString() );
                    o.subtotal = float.Parse( dataReader[ "subtotal" ].ToString() );
                    o.discount_amount = float.Parse( dataReader[ "discount_amount" ].ToString() );
                    o.installment_amount = float.Parse( dataReader[ "installment_amount" ].ToString() );
                    o.currency = dataReader[ "currency" ].ToString();
                    o.order_status = dataReader[ "order_status" ].ToString();
                    o.order_date = !string.IsNullOrWhiteSpace( dataReader[ "order_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "order_date" ].ToString() ) : DateTime.Now;
                    o.update_date = !string.IsNullOrWhiteSpace( dataReader[ "update_date" ].ToString() ) ? Convert.ToDateTime( dataReader[ "update_date" ].ToString() ) : null;
                }
                dataReader.Close();
                if( state == System.Data.ConnectionState.Open ) connection.Close();

                if( o != null ) {
                    o.order_items = GetOrderItems( _customer_id, _order_id );
                    o.billing_address = GetBillingAddresses( _customer_id, _order_id );
                    o.shipping_address = GetShippingAddresses( _customer_id, _order_id );
                }

                return o;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertOrders( int _customer_id, List<Order> _orders ) {
            try {
                int val = 0;
                foreach( Order item in _orders ) {
                    string _query = "INSERT INTO orders (customer_id,order_id,email,firstname,lastname,order_label,order_source,payment_method,shipment_method,comment,grand_total,subtotal,discount_amount,installment_amount,order_status,order_date,currency) VALUES (@customer_id,@order_id,@email,@firstname,@lastname,@order_label,@order_source,@payment_method,@shipment_method,@comment,@grand_total,@subtotal,@discount_amount,@installment_amount,@order_status,@order_date,@currency)";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "email", item.email ) );
                    cmd.Parameters.Add( new MySqlParameter( "firstname", item.firstname ) );
                    cmd.Parameters.Add( new MySqlParameter( "lastname", item.lastname ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_source", item.order_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "payment_method", item.payment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_method", item.shipment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "comment", item.comment ) );
                    //cmd.Parameters.Add( new MySqlParameter( "order_shipping_barcode", item.order_shipping_barcode ) );
                    //cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    //cmd.Parameters.Add( new MySqlParameter( "is_erp_sent", item.is_erp_sent ) );
                    cmd.Parameters.Add( new MySqlParameter( "grand_total", item.grand_total ) );
                    cmd.Parameters.Add( new MySqlParameter( "subtotal", item.subtotal ) );
                    cmd.Parameters.Add( new MySqlParameter( "discount_amount", item.discount_amount ) );
                    cmd.Parameters.Add( new MySqlParameter( "installment_amount", item.installment_amount ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_status", item.order_status ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();

                    foreach( var order_item in item.order_items )
                        InsertOrderItem( _customer_id, order_item );

                    if( item.billing_address != null )
                        InsertOrderBillingAddress( _customer_id, item.billing_address );
                    if( item.shipping_address != null )
                        InsertOrderShippingAddress( _customer_id, item.shipping_address );
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateOrders( int _customer_id, List<Order> _orders ) {
            try {
                int val = 0;
                foreach( Order item in _orders ) {
                    string _query = "UPDATE orders SET order_label=@order_label,email=@email,firstname=@firstname,lastname=@lastname,order_source=@order_source,payment_method=@payment_method,shipment_method=@shipment_method,comment=@comment,grand_total=@grand_total,subtotal=@subtotal,discount_amount=@discount_amount,installment_amount=@installment_amount,order_status=@order_status,order_date=@order_date,update_date=@update_date,currency=@currency WHERE order_id=@order_id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand( _query, connection );
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "update_date", DateTime.Now ) );
                    cmd.Parameters.Add( new MySqlParameter( "email", item.email ) );
                    cmd.Parameters.Add( new MySqlParameter( "firstname", item.firstname ) );
                    cmd.Parameters.Add( new MySqlParameter( "lastname", item.lastname ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_label", item.order_label ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_source", item.order_source ) );
                    cmd.Parameters.Add( new MySqlParameter( "payment_method", item.payment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "shipment_method", item.shipment_method ) );
                    cmd.Parameters.Add( new MySqlParameter( "comment", item.comment ) );
                    //cmd.Parameters.Add( new MySqlParameter( "order_shipping_barcode", item.order_shipping_barcode ) );
                    //cmd.Parameters.Add( new MySqlParameter( "erp_no", item.erp_no ) );
                    //cmd.Parameters.Add( new MySqlParameter( "is_erp_sent", item.is_erp_sent ) );
                    cmd.Parameters.Add( new MySqlParameter( "grand_total", item.grand_total ) );
                    cmd.Parameters.Add( new MySqlParameter( "subtotal", item.subtotal ) );
                    cmd.Parameters.Add( new MySqlParameter( "discount_amount", item.discount_amount ) );
                    cmd.Parameters.Add( new MySqlParameter( "installment_amount", item.installment_amount ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_status", item.order_status ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_date", item.order_date ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", item.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "currency", item.currency ) );
                    if( state != System.Data.ConnectionState.Open ) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if( state == System.Data.ConnectionState.Open ) connection.Close();
                    //DeleteOrderItems( _customer_id, item.order_id );
                    foreach( var order_item in item.order_items )
                        UpdateOrderItem( _customer_id, order_item );

                    if( item.billing_address != null )
                        UpdateOrderBillingAddress( _customer_id, item.billing_address );
                    if( item.shipping_address != null )
                        UpdateOrderShippingAddress( _customer_id, item.shipping_address );
                }
                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<OrderItem>? GetOrderItems( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_items WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<OrderItem> list = new List<OrderItem>();
                while( dataReader.Read() ) {
                    OrderItem oi = new OrderItem() {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        order_item_id = int.Parse( dataReader[ "order_item_id" ].ToString() ),
                        sku = dataReader[ "sku" ].ToString(),
                        parent_sku = dataReader[ "parent_sku" ].ToString(),
                        price = float.Parse( dataReader[ "price" ].ToString() ),
                        tax_amount = float.Parse( dataReader[ "tax_amount" ].ToString() ),
                        qty_ordered = int.Parse( dataReader[ "qty_ordered" ].ToString() ),
                        qty_invoiced = int.Parse( dataReader[ "qty_invoiced" ].ToString() ),
                        qty_cancelled = int.Parse( dataReader[ "qty_cancelled" ].ToString() ),
                        qty_refunded = int.Parse( dataReader[ "qty_refunded" ].ToString() ),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = dataReader[ "tax_included" ] != null ? dataReader[ "tax_included" ].ToString().Equals( "1" ) : false,
                    };
                    list.Add( oi );
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public List<OrderItem>? GetOrderItems( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_items WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<OrderItem> list = new List<OrderItem>();
                while( dataReader.Read() ) {
                    OrderItem oi = new OrderItem() {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        order_item_id = int.Parse( dataReader[ "order_item_id" ].ToString() ),
                        sku = dataReader[ "sku" ].ToString(),
                        parent_sku = dataReader[ "parent_sku" ].ToString(),
                        price = float.Parse( dataReader[ "price" ].ToString() ),
                        tax_amount = float.Parse( dataReader[ "tax_amount" ].ToString() ),
                        qty_ordered = int.Parse( dataReader[ "qty_ordered" ].ToString() ),
                        qty_invoiced = int.Parse( dataReader[ "qty_invoiced" ].ToString() ),
                        qty_cancelled = int.Parse( dataReader[ "qty_cancelled" ].ToString() ),
                        qty_refunded = int.Parse( dataReader[ "qty_refunded" ].ToString() ),
                        tax = int.Parse( dataReader[ "tax" ].ToString() ),
                        tax_included = dataReader[ "tax_included" ] != null ? dataReader[ "tax_included" ].ToString().Equals( "1" ) : false,
                    };
                    list.Add( oi );
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertOrderItem( int _customer_id, OrderItem _order_item ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_items (customer_id,order_id,order_item_id,sku,parent_sku,price,tax_amount,qty_ordered,qty_invoiced,qty_cancelled,qty_refunded,tax,tax_included) VALUES (@customer_id,@order_id,@order_item_id,@sku,@parent_sku,@price,@tax_amount,@qty_ordered,@qty_invoiced,@qty_cancelled,@qty_refunded,@tax,@tax_included)";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_item.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_item_id", _order_item.order_item_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _order_item.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "parent_sku", _order_item.parent_sku ) );
                cmd.Parameters.Add( new MySqlParameter( "price", _order_item.price ) );
                cmd.Parameters.Add( new MySqlParameter( "tax_amount", _order_item.tax_amount ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_ordered", _order_item.qty_ordered ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_invoiced", _order_item.qty_invoiced ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_cancelled", _order_item.qty_cancelled ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_refunded", _order_item.qty_refunded ) );
                cmd.Parameters.Add( new MySqlParameter( "tax", _order_item.tax ) );
                cmd.Parameters.Add( new MySqlParameter( "tax_included", _order_item.tax_included ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateOrderItem( int _customer_id, OrderItem _order_item ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "UPDATE order_items SET sku=@sku,parent_sku=@parent_sku,price=@price,tax_amount=@tax_amount,qty_ordered=@qty_ordered,qty_invoiced=@qty_invoiced,qty_cancelled=@qty_cancelled,qty_refunded=@qty_refunded,tax=@tax,tax_included=@tax_included WHERE order_item_id=@order_item_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_item.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_item_id", _order_item.order_item_id ) );
                cmd.Parameters.Add( new MySqlParameter( "sku", _order_item.sku ) );
                cmd.Parameters.Add( new MySqlParameter( "parent_sku", _order_item.parent_sku ) );
                cmd.Parameters.Add( new MySqlParameter( "price", _order_item.price ) );
                cmd.Parameters.Add( new MySqlParameter( "tax_amount", _order_item.tax_amount ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_ordered", _order_item.qty_ordered ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_invoiced", _order_item.qty_invoiced ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_cancelled", _order_item.qty_cancelled ) );
                cmd.Parameters.Add( new MySqlParameter( "qty_refunded", _order_item.qty_refunded ) );
                cmd.Parameters.Add( new MySqlParameter( "tax", _order_item.tax ) );
                cmd.Parameters.Add( new MySqlParameter( "tax_included", _order_item.tax_included ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool DeleteOrderItems( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM order_items WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<BillingAddress>? GetBillingAddresses( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_billing_addresses WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<BillingAddress> list = new List<BillingAddress>();
                while( dataReader.Read() ) {
                    BillingAddress ba = new BillingAddress() {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        billing_id = int.Parse( dataReader[ "billing_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        firstname = dataReader[ "firstname" ].ToString(),
                        lastname = dataReader[ "lastname" ].ToString(),
                        telephone = dataReader[ "telephone" ].ToString(),
                        street = dataReader[ "street" ].ToString(),
                        region = dataReader[ "region" ].ToString(),
                        city = dataReader[ "city" ].ToString(),
                        is_corporate = dataReader[ "is_corporate" ] != null ? dataReader[ "is_corporate" ].ToString().Equals( "1" ) : false,
                        firma_ismi = dataReader[ "firma_ismi" ].ToString(),
                        firma_vergidairesi = dataReader[ "firma_vergidairesi" ].ToString(),
                        firma_vergino = dataReader[ "firma_vergino" ].ToString(),
                        tc_no = dataReader[ "tc_no" ].ToString()
                    };
                    list.Add( ba );
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public BillingAddress? GetBillingAddresses( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_billing_addresses WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                BillingAddress ba = new BillingAddress();
                if( dataReader.Read() ) {
                    ba.id = int.Parse( dataReader[ "id" ].ToString() );
                    ba.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    ba.billing_id = int.Parse( dataReader[ "billing_id" ].ToString() );
                    ba.order_id = int.Parse( dataReader[ "order_id" ].ToString() );
                    ba.firstname = dataReader[ "firstname" ].ToString();
                    ba.lastname = dataReader[ "lastname" ].ToString();
                    ba.telephone = dataReader[ "telephone" ].ToString();
                    ba.street = dataReader[ "street" ].ToString();
                    ba.region = dataReader[ "region" ].ToString();
                    ba.city = dataReader[ "city" ].ToString();
                    ba.is_corporate = dataReader[ "is_corporate" ] != null ? dataReader[ "is_corporate" ].ToString().Equals( "1" ) : false;
                    ba.firma_ismi = dataReader[ "firma_ismi" ].ToString();
                    ba.firma_vergidairesi = dataReader[ "firma_vergidairesi" ].ToString();
                    ba.firma_vergino = dataReader[ "firma_vergino" ].ToString();
                    ba.tc_no = dataReader[ "tc_no" ].ToString();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return ba;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertOrderBillingAddress( int _customer_id, BillingAddress _billing_address ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_billing_addresses (customer_id,billing_id,order_id,firstname,lastname,telephone,street,region,city,is_corporate,firma_ismi,firma_vergidairesi,firma_vergino,tc_no) VALUES (@customer_id,@billing_id,@order_id,@firstname,@lastname,@telephone,@street,@region,@city,@is_corporate,@firma_ismi,@firma_vergidairesi,@firma_vergino,@tc_no)";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "billing_id", _billing_address.billing_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _billing_address.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "firstname", _billing_address.firstname ) );
                cmd.Parameters.Add( new MySqlParameter( "lastname", _billing_address.lastname ) );
                cmd.Parameters.Add( new MySqlParameter( "telephone", _billing_address.telephone ) );
                cmd.Parameters.Add( new MySqlParameter( "street", _billing_address.street ) );
                cmd.Parameters.Add( new MySqlParameter( "region", _billing_address.region ) );
                cmd.Parameters.Add( new MySqlParameter( "city", _billing_address.city ) );
                cmd.Parameters.Add( new MySqlParameter( "is_corporate", _billing_address.is_corporate ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_ismi", _billing_address.firma_ismi ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_vergidairesi", _billing_address.firma_vergidairesi ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_vergino", _billing_address.firma_vergino ) );
                cmd.Parameters.Add( new MySqlParameter( "tc_no", _billing_address.tc_no ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateOrderBillingAddress( int _customer_id, BillingAddress _billing_address ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "UPDATE order_billing_addresses SET firstname=@firstname,lastname=@lastname,telephone=@telephone,street=@street,region=@region,city=@city,is_corporate=@is_corporate,firma_ismi=@firma_ismi,firma_vergidairesi=@firma_vergidairesi,firma_vergino=@firma_vergino,tc_no=@tc_no WHERE billing_id=@billing_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "billing_id", _billing_address.billing_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _billing_address.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "firstname", _billing_address.firstname ) );
                cmd.Parameters.Add( new MySqlParameter( "lastname", _billing_address.lastname ) );
                cmd.Parameters.Add( new MySqlParameter( "telephone", _billing_address.telephone ) );
                cmd.Parameters.Add( new MySqlParameter( "street", _billing_address.street ) );
                cmd.Parameters.Add( new MySqlParameter( "region", _billing_address.region ) );
                cmd.Parameters.Add( new MySqlParameter( "city", _billing_address.city ) );
                cmd.Parameters.Add( new MySqlParameter( "is_corporate", _billing_address.is_corporate ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_ismi", _billing_address.firma_ismi ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_vergidairesi", _billing_address.firma_vergidairesi ) );
                cmd.Parameters.Add( new MySqlParameter( "firma_vergino", _billing_address.firma_vergino ) );
                cmd.Parameters.Add( new MySqlParameter( "tc_no", _billing_address.tc_no ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public List<ShippingAddress>? GetShippingAddresses( int _customer_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_shipping_addresses WHERE customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<ShippingAddress> list = new List<ShippingAddress>();
                while( dataReader.Read() ) {
                    ShippingAddress sa = new ShippingAddress() {
                        id = int.Parse( dataReader[ "id" ].ToString() ),
                        customer_id = int.Parse( dataReader[ "customer_id" ].ToString() ),
                        shipping_id = int.Parse( dataReader[ "shipping_id" ].ToString() ),
                        order_id = int.Parse( dataReader[ "order_id" ].ToString() ),
                        firstname = dataReader[ "firstname" ].ToString(),
                        lastname = dataReader[ "lastname" ].ToString(),
                        telephone = dataReader[ "telephone" ].ToString(),
                        street = dataReader[ "street" ].ToString(),
                        region = dataReader[ "region" ].ToString(),
                        city = dataReader[ "city" ].ToString()
                    };
                    list.Add( sa );
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return list;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public ShippingAddress? GetShippingAddresses( int _customer_id, int _order_id ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string _query = "SELECT * FROM order_shipping_addresses WHERE order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                MySqlDataReader dataReader = cmd.ExecuteReader();
                ShippingAddress sa = new ShippingAddress();
                if( dataReader.Read() ) {
                    sa.id = int.Parse( dataReader[ "id" ].ToString() );
                    sa.customer_id = int.Parse( dataReader[ "customer_id" ].ToString() );
                    sa.shipping_id = int.Parse( dataReader[ "shipping_id" ].ToString() );
                    sa.order_id = int.Parse( dataReader[ "order_id" ].ToString() );
                    sa.firstname = dataReader[ "firstname" ].ToString();
                    sa.lastname = dataReader[ "lastname" ].ToString();
                    sa.telephone = dataReader[ "telephone" ].ToString();
                    sa.street = dataReader[ "street" ].ToString();
                    sa.region = dataReader[ "region" ].ToString();
                    sa.city = dataReader[ "city" ].ToString();
                }

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                return sa;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return null;
            }
        }

        public bool InsertOrderShippingAddress( int _customer_id, ShippingAddress _shipping_address ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "INSERT INTO order_shipping_addresses (shipping_id,customer_id,order_id,firstname,lastname,telephone,street,region,city) VALUES (@shipping_id,@customer_id,@order_id,@firstname,@lastname,@telephone,@street,@region,@city)";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "shipping_id", _shipping_address.shipping_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _shipping_address.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "firstname", _shipping_address.firstname ) );
                cmd.Parameters.Add( new MySqlParameter( "lastname", _shipping_address.lastname ) );
                cmd.Parameters.Add( new MySqlParameter( "telephone", _shipping_address.telephone ) );
                cmd.Parameters.Add( new MySqlParameter( "street", _shipping_address.street ) );
                cmd.Parameters.Add( new MySqlParameter( "region", _shipping_address.region ) );
                cmd.Parameters.Add( new MySqlParameter( "city", _shipping_address.city ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool UpdateOrderShippingAddress( int _customer_id, ShippingAddress _shipping_address ) {
            try {
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();

                int val = 0;
                string _query = "UPDATE order_shipping_addresses SET firstname=@firstname,lastname=@lastname,telephone=@telephone,street=@street,region=@region,city=@city WHERE shipping_id=@shipping_id AND order_id=@order_id AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand( _query, connection );
                cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                cmd.Parameters.Add( new MySqlParameter( "shipping_id", _shipping_address.shipping_id ) );
                cmd.Parameters.Add( new MySqlParameter( "order_id", _shipping_address.order_id ) );
                cmd.Parameters.Add( new MySqlParameter( "firstname", _shipping_address.firstname ) );
                cmd.Parameters.Add( new MySqlParameter( "lastname", _shipping_address.lastname ) );
                cmd.Parameters.Add( new MySqlParameter( "telephone", _shipping_address.telephone ) );
                cmd.Parameters.Add( new MySqlParameter( "street", _shipping_address.street ) );
                cmd.Parameters.Add( new MySqlParameter( "region", _shipping_address.region ) );
                cmd.Parameters.Add( new MySqlParameter( "city", _shipping_address.city ) );
                val = cmd.ExecuteNonQuery();

                if( state == System.Data.ConnectionState.Open )
                    connection.Close();

                if( val > 0 )
                    return true;
                else return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetOrderProcess( int _customer_id, int _order_id, string _erp_no ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE orders SET is_erp_sent=@is_erp_sent,erp_no=@erp_no WHERE order_id=@order_id AND customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_erp_sent", !string.IsNullOrWhiteSpace( _erp_no ) ) );
                    cmd.Parameters.Add( new MySqlParameter( "erp_no", _erp_no ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetOrderStatus( int _customer_id, Order _order, string _status ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open ) connection.Open();
                string query = "UPDATE orders SET order_status=@order_status WHERE order_id=@order_id AND customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", _order.order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_status", _status ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open ) connection.Close();
                if( _status == "HAZIRLANIYOR" ) {
                    foreach( var item in _order.order_items ) {
                        item.qty_invoiced = item.qty_ordered - item.qty_refunded - item.qty_cancelled;
                        UpdateOrderItem( _customer_id, item );
                    }
                }
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetOrderShipmentBarcode( int _customer_id, int _order_id, string _order_shipping_barcode ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE orders SET order_shipping_barcode=@order_shipping_barcode WHERE order_id=@order_id AND customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_id", _order_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "order_shipping_barcode", _order_shipping_barcode ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion


        #region Is_Working Functions
        public bool SetProductSyncWorking( int _customer_id, bool _val ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET is_productsync_working=@is_productsync_working WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_productsync_working", _val ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetOrderSyncWorking( int _customer_id, bool _val ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET is_ordersync_working=@is_ordersync_working WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_ordersync_working", _val ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetXmlSyncWorking( int _customer_id, bool _val ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET is_xmlsync_working=@is_xmlsync_working WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_xmlsync_working", _val ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetInvoiceSyncWorking( int _customer_id, bool _val ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET is_invoicesync_working=@is_invoicesync_working WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_invoicesync_working", _val ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetNotificationSyncWorking( int _customer_id, bool _val ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET is_notificationsync_working=@is_notificationsync_working WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "is_notificationsync_working", _val ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetProductSyncDate( int _customer_id ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET last_product_sync_date=@last_product_sync_date WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "last_product_sync_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetOrderSyncDate( int _customer_id ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET last_order_sync_date=@last_order_sync_date WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "last_order_sync_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetXmlSyncDate( int _customer_id ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET last_xml_sync_date=@last_xml_sync_date WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "last_xml_sync_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetInvoiceSyncDate( int _customer_id ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET last_invoice_sync_date=@last_invoice_sync_date WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "last_invoice_sync_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }

        public bool SetNotificationSyncDate( int _customer_id ) {
            try {
                int value = 0;
                if( state != System.Data.ConnectionState.Open )
                    connection.Open();
                string query = "UPDATE customer SET last_notification_sync_date=@last_notification_sync_date WHERE customer_id=@customer_id";
                using( MySqlCommand cmd = new MySqlCommand( query, connection ) ) {
                    cmd.Parameters.Add( new MySqlParameter( "customer_id", _customer_id ) );
                    cmd.Parameters.Add( new MySqlParameter( "last_notification_sync_date", DateTime.Now ) );
                    value = cmd.ExecuteNonQuery();
                }
                if( state == System.Data.ConnectionState.Open )
                    connection.Close();
                if( value > 0 )
                    return true;
                else
                    return false;
            }
            catch( Exception ex ) {
                OnError( ex.Message );
                return false;
            }
        }
        #endregion

        private void WriteLogLine( string value, ConsoleColor _color ) {
            Console.ForegroundColor = _color;
            Console.WriteLine( value.PadRight( Console.WindowWidth - 1 ) );
            Console.ResetColor();
        }
    }
}
