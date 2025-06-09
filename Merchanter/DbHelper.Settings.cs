using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MySql.Data.MySqlClient;

namespace Merchanter {
    public partial class DbHelper {

        #region Settings
        /// <summary>
        /// Gets DB settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<DBSetting>?> GetSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM db_settings WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<DBSetting> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new DBSetting() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            name = dataReader["name"].ToString(),
                            value = dataReader["value"].ToString(),
                            group_name = dataReader["group_name"].ToString(),
                            description = dataReader["description"].ToString(),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        });
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
        }  //TODO: remove this pls

        #region Save Functions
        /// <summary>
        /// Saves the customer settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings General</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveCustomerSettings(int _customer_id, SettingsGeneral _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings SET company_name=@company_name,rate_TL=@rate_TL,rate_USD=@rate_USD,rate_EUR=@rate_EUR WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("company_name", _settings.company_name));
                cmd.Parameters.Add(new MySqlParameter("rate_TL", _settings.rate_TL));
                cmd.Parameters.Add(new MySqlParameter("rate_USD", _settings.rate_USD));
                cmd.Parameters.Add(new MySqlParameter("rate_EUR", _settings.rate_EUR));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the customer settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Invoice</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveInvoiceSettings(int _customer_id, SettingsInvoice _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_invoice SET daysto_invoicesync=@daysto_invoicesync,erp_invoice_ftp_username=@erp_invoice_ftp_username,erp_invoice_ftp_password=@erp_invoice_ftp_password,erp_invoice_ftp_url=@erp_invoice_ftp_url WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("daysto_invoicesync", _settings.daysto_invoicesync));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_username", _settings.erp_invoice_ftp_username));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_password", !string.IsNullOrWhiteSpace(_settings.erp_invoice_ftp_password) ? DBSetting.Encrypt(_settings.erp_invoice_ftp_password) : null));
                cmd.Parameters.Add(new MySqlParameter("erp_invoice_ftp_url", _settings.erp_invoice_ftp_url));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the product settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Product</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveProductSettings(int _customer_id, SettingsProduct _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_product SET default_brand=@default_brand,customer_root_category_id=@customer_root_category_id,product_list_filter_source_products=@product_list_filter_source_products,is_barcode_required=@is_barcode_required,xml_qty_addictive_enable=@xml_qty_addictive_enable WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("default_brand", _settings.default_brand));
                cmd.Parameters.Add(new MySqlParameter("customer_root_category_id", _settings.customer_root_category_id));
                cmd.Parameters.Add(new MySqlParameter("product_list_filter_source_products", _settings.product_list_filter_source_products));
                cmd.Parameters.Add(new MySqlParameter("is_barcode_required", _settings.is_barcode_required));
                cmd.Parameters.Add(new MySqlParameter("xml_qty_addictive_enable", _settings.xml_qty_addictive_enable));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the entegra settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Entegra</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveEntegraSettings(int _customer_id, SettingsEntegra _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_entegra SET api_url=@api_url,api_username=@api_username,api_password=@api_password WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("api_url", _settings.api_url));
                cmd.Parameters.Add(new MySqlParameter("api_username", _settings.api_username));
                cmd.Parameters.Add(new MySqlParameter("api_password", !string.IsNullOrWhiteSpace(_settings.api_password) ? DBSetting.Encrypt(_settings.api_password) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the shipment settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Shipment</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveShipmentSettings(int _customer_id, SettingsShipment _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_shipment SET yurtici_kargo=@yurtici_kargo,mng_kargo=@mng_kargo,aras_kargo=@aras_kargo,yurtici_kargo_user_name=@yurtici_kargo_user_name,yurtici_kargo_password=@yurtici_kargo_password,mng_kargo_customer_number=@mng_kargo_customer_number,mng_kargo_password=@mng_kargo_password,mng_kargo_client_id=@mng_kargo_client_id,mng_kargo_client_secret=@mng_kargo_client_secret WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo", _settings.yurtici_kargo));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo", _settings.mng_kargo));
                cmd.Parameters.Add(new MySqlParameter("aras_kargo", _settings.aras_kargo));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo_user_name", _settings.yurtici_kargo_user_name));
                cmd.Parameters.Add(new MySqlParameter("yurtici_kargo_password", !string.IsNullOrWhiteSpace(_settings.yurtici_kargo_password) ? DBSetting.Encrypt(_settings.yurtici_kargo_password) : null));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_customer_number", _settings.mng_kargo_customer_number));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_password", !string.IsNullOrWhiteSpace(_settings.mng_kargo_password) ? DBSetting.Encrypt(_settings.mng_kargo_password) : null));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_client_id", _settings.mng_kargo_client_id));
                cmd.Parameters.Add(new MySqlParameter("mng_kargo_client_secret", !string.IsNullOrWhiteSpace(_settings.mng_kargo_client_secret) ? DBSetting.Encrypt(_settings.mng_kargo_client_secret) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the N11 settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings N11</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveN11Settings(int _customer_id, SettingsN11 _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_n11 SET appkey=@appkey,appsecret=@appsecret WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("appkey", _settings.appkey));
                cmd.Parameters.Add(new MySqlParameter("appsecret", !string.IsNullOrWhiteSpace(_settings.appsecret) ? DBSetting.Encrypt(_settings.appsecret) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the HB settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings HB</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveHBSettings(int _customer_id, SettingsHB _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_hb SET merchant_id=@merchant_id,token=@token,user_name=@user_name,password=@password WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("merchant_id", _settings.merchant_id));
                cmd.Parameters.Add(new MySqlParameter("token", !string.IsNullOrWhiteSpace(_settings.token) ? DBSetting.Encrypt(_settings.token) : null));
                cmd.Parameters.Add(new MySqlParameter("user_name", _settings.user_name));
                cmd.Parameters.Add(new MySqlParameter("password", !string.IsNullOrWhiteSpace(_settings.password) ? DBSetting.Encrypt(_settings.password) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the TY settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings TY</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveTYSettings(int _customer_id, SettingsTY _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ty SET seller_id=@seller_id,api_key=@api_key,api_secret=@api_secret WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("seller_id", _settings.seller_id));
                cmd.Parameters.Add(new MySqlParameter("api_key", _settings.api_key));
                cmd.Parameters.Add(new MySqlParameter("api_secret", !string.IsNullOrWhiteSpace(_settings.api_secret) ? DBSetting.Encrypt(_settings.api_secret) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the Ankara ERP settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings ANK_ERP</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveAnkERPSettings(int _customer_id, SettingsAnkaraErp _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ank_erp SET company_code=@company_code,user_name=@user_name,password=@password,work_year=@work_year,url=@url WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("company_code", _settings.company_code));
                cmd.Parameters.Add(new MySqlParameter("user_name", _settings.user_name));
                cmd.Parameters.Add(new MySqlParameter("work_year", _settings.work_year));
                cmd.Parameters.Add(new MySqlParameter("url", _settings.url));
                cmd.Parameters.Add(new MySqlParameter("password", !string.IsNullOrWhiteSpace(_settings.password) ? DBSetting.Encrypt(_settings.password) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the IDEASOFT settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings IDEASOFT</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveIdeasoftSettings(int _customer_id, SettingsIdeasoft _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_ideasoft SET store_url=@store_url,client_id=@client_id,client_secret=@client_secret,refresh_token=@refresh_token,access_token=@access_token WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("store_url", _settings.store_url));
                cmd.Parameters.Add(new MySqlParameter("client_id", _settings.client_id));
                cmd.Parameters.Add(new MySqlParameter("client_secret", !string.IsNullOrWhiteSpace(_settings.client_secret) ? DBSetting.Encrypt(_settings.client_secret) : null));
                cmd.Parameters.Add(new MySqlParameter("refresh_token", !string.IsNullOrWhiteSpace(_settings.refresh_token) ? DBSetting.Encrypt(_settings.refresh_token) : null));
                cmd.Parameters.Add(new MySqlParameter("access_token", !string.IsNullOrWhiteSpace(_settings.access_token) ? DBSetting.Encrypt(_settings.access_token) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the Google settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Google</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveGoogleSettings(int _customer_id, SettingsGoogle _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_google SET email=@email,oauth2_clientid=@oauth2_clientid,oauth2_clientsecret=@oauth2_clientsecret,sender_name=@sender_name,is_enabled=@is_enabled WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("email", _settings.email));
                cmd.Parameters.Add(new MySqlParameter("oauth2_clientid", _settings.oauth2_clientid));
                cmd.Parameters.Add(new MySqlParameter("sender_name", _settings.sender_name));
                cmd.Parameters.Add(new MySqlParameter("is_enabled", _settings.is_enabled));
                cmd.Parameters.Add(new MySqlParameter("oauth2_clientsecret", !string.IsNullOrWhiteSpace(_settings.oauth2_clientsecret) ? DBSetting.Encrypt(_settings.oauth2_clientsecret) : null));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the magento settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Magento</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveMagentoSettings(int _customer_id, SettingsMagento _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_magento SET base_url=@base_url,token=@token,root_category_id=@root_category_id,order_processing_comment=@order_processing_comment,barcode_attribute_code=@barcode_attribute_code,brand_attribute_code=@brand_attribute_code,is_xml_enabled_attribute_code=@is_xml_enabled_attribute_code,xml_sources_attribute_code=@xml_sources_attribute_code,customer_tc_no_attribute_code=@customer_tc_no_attribute_code,customer_firma_ismi_attribute_code=@customer_firma_ismi_attribute_code,customer_firma_vergidairesi_attribute_code=@customer_firma_vergidairesi_attribute_code,customer_firma_vergino_attribute_code=@customer_firma_vergino_attribute_code WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("base_url", _settings.base_url));
                cmd.Parameters.Add(new MySqlParameter("token", !string.IsNullOrWhiteSpace(_settings.token) ? DBSetting.Encrypt(_settings.token) : null));
                cmd.Parameters.Add(new MySqlParameter("root_category_id", _settings.root_category_id));
                cmd.Parameters.Add(new MySqlParameter("order_processing_comment", _settings.order_processing_comment));
                cmd.Parameters.Add(new MySqlParameter("barcode_attribute_code", _settings.barcode_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("brand_attribute_code", _settings.brand_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("is_xml_enabled_attribute_code", _settings.is_xml_enabled_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("xml_sources_attribute_code", _settings.xml_sources_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_tc_no_attribute_code", _settings.customer_tc_no_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_ismi_attribute_code", _settings.customer_firma_ismi_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_vergidairesi_attribute_code", _settings.customer_firma_vergidairesi_attribute_code));
                cmd.Parameters.Add(new MySqlParameter("customer_firma_vergino_attribute_code", _settings.customer_firma_vergino_attribute_code));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the order settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Order</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveOrderSettings(int _customer_id, SettingsOrder _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_order SET daysto_ordersync=@daysto_ordersync,siparis_kargo_sku=@siparis_kargo_sku,siparis_taksitkomisyon_sku=@siparis_taksitkomisyon_sku,is_rewrite_siparis=@is_rewrite_siparis,siparis_kdvdahilmi=@siparis_kdvdahilmi WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("daysto_ordersync", _settings.daysto_ordersync));
                cmd.Parameters.Add(new MySqlParameter("siparis_kargo_sku", _settings.siparis_kargo_sku));
                cmd.Parameters.Add(new MySqlParameter("siparis_taksitkomisyon_sku", _settings.siparis_taksitkomisyon_sku));
                cmd.Parameters.Add(new MySqlParameter("is_rewrite_siparis", _settings.is_rewrite_siparis));
                cmd.Parameters.Add(new MySqlParameter("siparis_kdvdahilmi", _settings.siparis_kdvdahilmi));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the netsis settings to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Settings Netsis</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveNetsisSettings(int _customer_id, SettingsNetsis _settings) {
            try {
                int val = 0;
                string _query = "UPDATE settings_netsis SET rest_url=@rest_url,netopenx_user=@netopenx_user,netopenx_password=@netopenx_password,dbname=@dbname,dbuser=@dbuser,dbpassword=@dbpassword,belgeonek_musterisiparisi=@belgeonek_musterisiparisi,siparis_carionek=@siparis_carionek,cari_siparis_grupkodu=@cari_siparis_grupkodu,sipari_caritip=@sipari_caritip,siparis_muhasebekodu=@siparis_muhasebekodu,siparis_subekodu=@siparis_subekodu,siparis_depokodu=@siparis_depokodu,ebelge_dizayn_earsiv=@ebelge_dizayn_earsiv,ebelge_dizayn_efatura=@ebelge_dizayn_efatura,ebelge_klasorismi=@ebelge_klasorismi,efatura_belge_onek=@efatura_belge_onek,earsiv_belge_onek=@earsiv_belge_onek,fatura_cari_gruplari=@fatura_cari_gruplari,siparis_kod2=@siparis_kod2,siparis_cyedek1=@siparis_cyedek1,siparis_ekack15=@siparis_ekack15,siparis_ekack10=@siparis_ekack10,siparis_ekack11=@siparis_ekack11,siparis_ekack4=@siparis_ekack4 WHERE customer_id=@customer_id;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("rest_url", _settings.rest_url));
                cmd.Parameters.Add(new MySqlParameter("netopenx_user", _settings.netopenx_user));
                cmd.Parameters.Add(new MySqlParameter("netopenx_password", !string.IsNullOrWhiteSpace(_settings.netopenx_password) ? DBSetting.Encrypt(_settings.netopenx_password) : null));
                cmd.Parameters.Add(new MySqlParameter("dbname", _settings.dbname));
                cmd.Parameters.Add(new MySqlParameter("dbuser", _settings.dbuser)); //TODO: Add encrypt
                cmd.Parameters.Add(new MySqlParameter("dbpassword", !string.IsNullOrWhiteSpace(_settings.dbpassword) ? DBSetting.Encrypt(_settings.dbpassword) : null));
                cmd.Parameters.Add(new MySqlParameter("belgeonek_musterisiparisi", _settings.belgeonek_musterisiparisi));
                cmd.Parameters.Add(new MySqlParameter("siparis_carionek", _settings.siparis_carionek));
                cmd.Parameters.Add(new MySqlParameter("cari_siparis_grupkodu", _settings.cari_siparis_grupkodu));
                cmd.Parameters.Add(new MySqlParameter("sipari_caritip", _settings.sipari_caritip));
                cmd.Parameters.Add(new MySqlParameter("siparis_muhasebekodu", _settings.siparis_muhasebekodu));
                cmd.Parameters.Add(new MySqlParameter("siparis_subekodu", _settings.siparis_subekodu));
                cmd.Parameters.Add(new MySqlParameter("siparis_depokodu", _settings.siparis_depokodu));
                cmd.Parameters.Add(new MySqlParameter("ebelge_dizayn_earsiv", _settings.ebelge_dizayn_earsiv));
                cmd.Parameters.Add(new MySqlParameter("ebelge_dizayn_efatura", _settings.ebelge_dizayn_efatura));
                cmd.Parameters.Add(new MySqlParameter("ebelge_klasorismi", _settings.ebelge_klasorismi));
                cmd.Parameters.Add(new MySqlParameter("efatura_belge_onek", _settings.efatura_belge_onek));
                cmd.Parameters.Add(new MySqlParameter("earsiv_belge_onek", _settings.earsiv_belge_onek));
                cmd.Parameters.Add(new MySqlParameter("fatura_cari_gruplari", _settings.fatura_cari_gruplari));
                cmd.Parameters.Add(new MySqlParameter("siparis_kod2", _settings.siparis_kod2));
                cmd.Parameters.Add(new MySqlParameter("siparis_cyedek1", _settings.siparis_cyedek1));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack15", _settings.siparis_ekack15));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack10", _settings.siparis_ekack10));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack11", _settings.siparis_ekack11));
                cmd.Parameters.Add(new MySqlParameter("siparis_ekack4", _settings.siparis_ekack4));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Saves the work sources to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_settings">Work Sources</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> SaveCustomerIntegration(int _customer_id, Integration _setting) {
            try {
                int val = 0;
                string _query = "UPDATE integrations SET work_id=@work_id,is_active=@is_active WHERE id=@id AND customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("id", _setting.id));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("work_id", _setting.work.id));
                cmd.Parameters.Add(new MySqlParameter("is_active", _setting.is_active));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    val = await cmd.ExecuteNonQueryAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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

        #region Get Functions
        /// <summary>
        /// Gets the settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsGeneral?> GetCustomerSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings WHERE customer_id=@customer_id;";
                    SettingsGeneral? cs = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        cs = new SettingsGeneral {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            company_name = dataReader["company_name"].ToString(),
                            rate_TL = Convert.ToDecimal(dataReader["rate_TL"].ToString()),
                            rate_USD = Convert.ToDecimal(dataReader["rate_USD"].ToString()),
                            rate_EUR = Convert.ToDecimal(dataReader["rate_EUR"].ToString())
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return cs;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the invoice settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsInvoice?> GetInvoiceSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_invoice WHERE customer_id=@customer_id;";
                    SettingsInvoice? inv_s = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        inv_s = new SettingsInvoice {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            daysto_invoicesync = Convert.ToInt32(dataReader["daysto_invoicesync"].ToString()),
                            erp_invoice_ftp_password = dataReader["erp_invoice_ftp_password"].ToString(),
                            erp_invoice_ftp_url = dataReader["erp_invoice_ftp_url"].ToString(),
                            erp_invoice_ftp_username = dataReader["erp_invoice_ftp_username"].ToString()
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return inv_s;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsProduct?> GetProductSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_product WHERE customer_id=@customer_id;";
                    SettingsProduct? ps = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ps = new SettingsProduct {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            default_brand = dataReader["default_brand"].ToString(),
                            customer_root_category_id = Convert.ToInt32(dataReader["customer_root_category_id"].ToString()),
                            product_list_filter_source_products = Convert.ToBoolean(Convert.ToInt32(dataReader["product_list_filter_source_products"].ToString())),
                            is_barcode_required = Convert.ToBoolean(Convert.ToInt32(dataReader["is_barcode_required"].ToString())),
                            xml_qty_addictive_enable = Convert.ToBoolean(Convert.ToInt32(dataReader["xml_qty_addictive_enable"].ToString()))
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return ps;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the entegra settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsEntegra?> GetEntegraSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_entegra WHERE customer_id=@customer_id;";
                    SettingsEntegra? es = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        es = new SettingsEntegra {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            api_url = dataReader["api_url"].ToString(),
                            api_username = dataReader["api_username"].ToString(),
                            api_password = dataReader["api_password"].ToString()
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return es;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the magento settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsMagento?> GetMagentoSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_magento WHERE customer_id=@customer_id;";
                    SettingsMagento? ms = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ms = new SettingsMagento {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            base_url = dataReader["base_url"].ToString(),
                            token = dataReader["token"].ToString(),
                            root_category_id = Convert.ToInt32(dataReader["root_category_id"].ToString()),
                            brand_attribute_code = dataReader["brand_attribute_code"].ToString(),
                            barcode_attribute_code = dataReader["barcode_attribute_code"].ToString(),
                            is_xml_enabled_attribute_code = dataReader["is_xml_enabled_attribute_code"].ToString(),
                            xml_sources_attribute_code = dataReader["xml_sources_attribute_code"].ToString(),
                            customer_tc_no_attribute_code = dataReader["customer_tc_no_attribute_code"].ToString(),
                            customer_firma_ismi_attribute_code = dataReader["customer_firma_ismi_attribute_code"].ToString(),
                            customer_firma_vergidairesi_attribute_code = dataReader["customer_firma_vergidairesi_attribute_code"].ToString(),
                            customer_firma_vergino_attribute_code = dataReader["customer_firma_vergino_attribute_code"].ToString(),
                            order_processing_comment = dataReader["order_processing_comment"].ToString()
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return ms;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the order settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsOrder?> GetOrderSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_order WHERE customer_id=@customer_id;";
                    SettingsOrder? os = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        os = new SettingsOrder {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            daysto_ordersync = Convert.ToInt32(dataReader["daysto_ordersync"].ToString()),
                            siparis_kargo_sku = dataReader["siparis_kargo_sku"].ToString(),
                            siparis_taksitkomisyon_sku = dataReader["siparis_taksitkomisyon_sku"].ToString(),
                            is_rewrite_siparis = Convert.ToBoolean(Convert.ToInt32(dataReader["is_rewrite_siparis"].ToString())),
                            siparis_kdvdahilmi = Convert.ToBoolean(Convert.ToInt32(dataReader["siparis_kdvdahilmi"].ToString())),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return os;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the google settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsGoogle?> GetGoogleSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_google WHERE customer_id=@customer_id;";
                    SettingsGoogle? gs = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        gs = new SettingsGoogle {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            email = dataReader["email"].ToString(),
                            oauth2_clientid = dataReader["oauth2_clientid"].ToString(),
                            oauth2_clientsecret = dataReader["oauth2_clientsecret"].ToString(),
                            sender_name = dataReader["sender_name"].ToString(),
                            is_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_enabled"].ToString())),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return gs;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the netsis settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsNetsis?> GetNetsisSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_netsis WHERE customer_id=@customer_id;";
                    SettingsNetsis? ns = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ns = new SettingsNetsis {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            netopenx_user = dataReader["netopenx_user"].ToString(),
                            netopenx_password = dataReader["netopenx_password"].ToString(),
                            dbname = dataReader["dbname"].ToString(),
                            dbuser = dataReader["dbuser"].ToString(),
                            dbpassword = dataReader["dbpassword"].ToString(),
                            rest_url = dataReader["rest_url"].ToString(),
                            belgeonek_musterisiparisi = dataReader["belgeonek_musterisiparisi"].ToString(),
                            siparis_carionek = dataReader["siparis_carionek"].ToString(),
                            cari_siparis_grupkodu = dataReader["cari_siparis_grupkodu"].ToString(),
                            sipari_caritip = dataReader["sipari_caritip"].ToString(),
                            siparis_muhasebekodu = dataReader["siparis_muhasebekodu"].ToString(),
                            siparis_subekodu = Convert.ToInt32(dataReader["siparis_subekodu"].ToString()),
                            siparis_depokodu = Convert.ToInt32(dataReader["siparis_depokodu"].ToString()),
                            ebelge_dizayn_earsiv = dataReader["ebelge_dizayn_earsiv"].ToString(),
                            ebelge_dizayn_efatura = dataReader["ebelge_dizayn_efatura"].ToString(),
                            ebelge_klasorismi = dataReader["ebelge_klasorismi"].ToString(),
                            efatura_belge_onek = dataReader["efatura_belge_onek"].ToString(),
                            earsiv_belge_onek = dataReader["earsiv_belge_onek"].ToString(),
                            fatura_cari_gruplari = dataReader["fatura_cari_gruplari"].ToString(),
                            siparis_kod2 = dataReader["siparis_kod2"].ToString(),
                            siparis_cyedek1 = dataReader["siparis_cyedek1"].ToString(),
                            siparis_ekack15 = dataReader["siparis_ekack15"].ToString(),
                            siparis_ekack10 = dataReader["siparis_ekack10"].ToString(),
                            siparis_ekack11 = dataReader["siparis_ekack11"].ToString(),
                            siparis_ekack4 = dataReader["siparis_ekack4"].ToString()
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return ns;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the shipment settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsShipment?> GetShipmentSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_shipment WHERE customer_id=@customer_id;";
                    SettingsShipment? ss = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ss = new SettingsShipment {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            yurtici_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["yurtici_kargo"].ToString())),
                            mng_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["mng_kargo"].ToString())),
                            aras_kargo = Convert.ToBoolean(Convert.ToInt32(dataReader["aras_kargo"].ToString())),
                            yurtici_kargo_user_name = dataReader["yurtici_kargo_user_name"].ToString(),
                            yurtici_kargo_password = dataReader["yurtici_kargo_password"].ToString(),
                            mng_kargo_customer_number = dataReader["mng_kargo_customer_number"].ToString(),
                            mng_kargo_password = dataReader["mng_kargo_password"].ToString(),
                            mng_kargo_client_id = dataReader["mng_kargo_client_id"].ToString(),
                            mng_kargo_client_secret = dataReader["mng_kargo_client_secret"].ToString(),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return ss;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the N11 settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsN11?> GetN11Settings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_n11 WHERE customer_id=@customer_id;";
                    SettingsN11? sn11 = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        sn11 = new SettingsN11 {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            appkey = dataReader["appkey"].ToString(),
                            appsecret = dataReader["appsecret"].ToString(),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return sn11;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the HB settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsHB?> GetHBSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_hb WHERE customer_id=@customer_id;";
                    SettingsHB? shb = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        shb = new SettingsHB {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            merchant_id = dataReader["merchant_id"].ToString(),
                            token = dataReader["token"].ToString(),
                            user_name = dataReader["user_name"].ToString(),
                            password = dataReader["password"].ToString(),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return shb;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the TY settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsTY?> GetTYSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_ty WHERE customer_id=@customer_id;";
                    SettingsTY? sty = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        sty = new SettingsTY {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            seller_id = dataReader["seller_id"].ToString(),
                            api_key = dataReader["api_key"].ToString(),
                            api_secret = dataReader["api_secret"].ToString(),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return sty;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the ANKARA ERP settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsAnkaraErp?> GetAnkERPSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_ank_erp WHERE customer_id=@customer_id;";
                    SettingsAnkaraErp? ank = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ank = new SettingsAnkaraErp {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            company_code = dataReader["company_code"].ToString(),
                            user_name = dataReader["user_name"].ToString(),
                            password = dataReader["password"].ToString(),
                            work_year = dataReader["work_year"].ToString(),
                            url = dataReader["url"].ToString(),
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return ank;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the IDEASOFT settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsIdeasoft?> GetIdeasoftSettings(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM settings_ideasoft WHERE customer_id=@customer_id;";
                    SettingsIdeasoft? idea = null;
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        idea = new SettingsIdeasoft {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            store_url = dataReader["store_url"].ToString(),
                            client_id = dataReader["client_id"].ToString(),
                            client_secret = dataReader["client_secret"].ToString(),
                            refresh_token = dataReader["refresh_token"].ToString(),
                            access_token = dataReader["access_token"].ToString(),
                            update_date = !string.IsNullOrWhiteSpace(dataReader["update_date"].ToString()) ? Convert.ToDateTime(dataReader["update_date"].ToString()) : null
                        };
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return idea;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Load Functions
        /// <summary>
        /// Loads the settings from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<SettingsMerchanter?> LoadSettings(int _customer_id) {
            try {
                this.DbSettings = await GetSettings(_customer_id);
                Helper.global = new SettingsMerchanter(_customer_id);
                var customer = await GetCustomer(_customer_id);
                if (customer is not null) { Helper.global.customer = customer; }
                else { DbHelperBase.PrintConsole("Customer not found!", ConsoleColor.Red); return null; }

                #region Core Settings
                Helper.global.platforms = await LoadPlatforms();
                Helper.global.works = await LoadWorks();
                Helper.global.integrations = await LoadIntegrations(_customer_id);
                #endregion

                #region Customer Settings
                Helper.global.settings = await GetCustomerSettings(_customer_id);
                Helper.global.product = await GetProductSettings(_customer_id);
                Helper.global.invoice = await GetInvoiceSettings(_customer_id);
                Helper.global.order = await GetOrderSettings(_customer_id);
                Helper.global.shipment = await GetShipmentSettings(_customer_id);
                Helper.global.order_statuses = await LoadOrderStatuses(_customer_id);
                Helper.global.payment_methods = await LoadPaymentMethods(_customer_id);
                Helper.global.shipment_methods = await LoadShipmentMethods(_customer_id);
                Helper.global.sync_mappings = GetCustomerSyncMappings(_customer_id);
                #endregion

                #region Integration Settings
                //TODO: Could check if integration exists
                Helper.global.entegra = await GetEntegraSettings(_customer_id);
                Helper.global.netsis = await GetNetsisSettings(_customer_id);
                Helper.global.magento = await GetMagentoSettings(_customer_id);
                Helper.global.n11 = await GetN11Settings(_customer_id);
                Helper.global.hb = await GetHBSettings(_customer_id);
                Helper.global.ty = await GetTYSettings(_customer_id);
                Helper.global.ank_erp = await GetAnkERPSettings(_customer_id);
                Helper.global.ideasoft = await GetIdeasoftSettings(_customer_id);
                Helper.global.google = await GetGoogleSettings(_customer_id);
                #endregion

                Helper.global.erp_invoice_ftp_username = DbSettings?.Where(x => x.name == "erp_invoice_ftp_username").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.erp_invoice_ftp_password = DbSettings?.Where(x => x.name == "erp_invoice_ftp_password").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.erp_invoice_ftp_url = DbSettings?.Where(x => x.name == "erp_invoice_ftp_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_bayikodu = DbSettings?.Where(x => x.name == "xml_bogazici_bayikodu").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_email = DbSettings?.Where(x => x.name == "xml_bogazici_email").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_bogazici_sifre = DbSettings?.Where(x => x.name == "xml_bogazici_sifre").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_fsp_url = DbSettings?.Where(x => x.name == "xml_fsp_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_koyuncu_url = DbSettings?.Where(x => x.name == "xml_koyuncu_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_oksid_url = DbSettings?.Where(x => x.name == "xml_oksid_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_penta_base_url = DbSettings?.Where(x => x.name == "xml_penta_base_url").FirstOrDefault()?.value ?? string.Empty;
                Helper.global.xml_penta_customerid = DbSettings?.Where(x => x.name == "xml_penta_customerid").FirstOrDefault()?.value ?? string.Empty;


                #region Decyrption
                if (Helper.global.shipment is not null && !string.IsNullOrWhiteSpace(Helper.global.shipment.yurtici_kargo_password))
                    Helper.global.shipment.yurtici_kargo_password = DBSetting.Decrypt(Helper.global.shipment.yurtici_kargo_password);
                if (Helper.global.shipment is not null && !string.IsNullOrWhiteSpace(Helper.global.shipment.mng_kargo_password))
                    Helper.global.shipment.mng_kargo_password = DBSetting.Decrypt(Helper.global.shipment.mng_kargo_password);
                if (Helper.global.shipment is not null && !string.IsNullOrWhiteSpace(Helper.global.shipment.mng_kargo_client_secret))
                    Helper.global.shipment.mng_kargo_client_secret = DBSetting.Decrypt(Helper.global.shipment.mng_kargo_client_secret);
                if (Helper.global.magento is not null && !string.IsNullOrWhiteSpace(Helper.global.magento.token))
                    Helper.global.magento.token = DBSetting.Decrypt(Helper.global.magento.token);
                if (Helper.global.entegra is not null && !string.IsNullOrWhiteSpace(Helper.global.entegra.api_password))
                    Helper.global.entegra.api_password = DBSetting.Decrypt(Helper.global.entegra.api_password);
                if (Helper.global.netsis is not null && !string.IsNullOrWhiteSpace(Helper.global.netsis.netopenx_password))
                    Helper.global.netsis.netopenx_password = DBSetting.Decrypt(Helper.global.netsis.netopenx_password);
                if (Helper.global.netsis is not null && !string.IsNullOrWhiteSpace(Helper.global.netsis.dbpassword))
                    Helper.global.netsis.dbpassword = DBSetting.Decrypt(Helper.global.netsis.dbpassword);
                if (Helper.global.invoice is not null && !string.IsNullOrWhiteSpace(Helper.global.invoice.erp_invoice_ftp_password))
                    Helper.global.invoice.erp_invoice_ftp_password = DBSetting.Decrypt(Helper.global.invoice.erp_invoice_ftp_password);
                if (Helper.global.n11 is not null && !string.IsNullOrWhiteSpace(Helper.global.n11.appsecret))
                    Helper.global.n11.appsecret = DBSetting.Decrypt(Helper.global.n11.appsecret);
                if (Helper.global.hb is not null && !string.IsNullOrWhiteSpace(Helper.global.hb.token))
                    Helper.global.hb.token = DBSetting.Decrypt(Helper.global.hb.token);
                if (Helper.global.hb is not null && !string.IsNullOrWhiteSpace(Helper.global.hb.password))
                    Helper.global.hb.password = DBSetting.Decrypt(Helper.global.hb.password);
                if (Helper.global.ty is not null && !string.IsNullOrWhiteSpace(Helper.global.ty.api_secret))
                    Helper.global.ty.api_secret = DBSetting.Decrypt(Helper.global.ty.api_secret);
                if (Helper.global.ank_erp is not null && !string.IsNullOrWhiteSpace(Helper.global.ank_erp.password))
                    Helper.global.ank_erp.password = DBSetting.Decrypt(Helper.global.ank_erp.password);
                if (Helper.global.ideasoft is not null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.client_secret))
                    Helper.global.ideasoft.client_secret = DBSetting.Decrypt(Helper.global.ideasoft.client_secret);
                if (Helper.global.ideasoft is not null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.refresh_token))
                    Helper.global.ideasoft.refresh_token = DBSetting.Decrypt(Helper.global.ideasoft.refresh_token);
                if (Helper.global.ideasoft is not null && !string.IsNullOrWhiteSpace(Helper.global.ideasoft.access_token))
                    Helper.global.ideasoft.access_token = DBSetting.Decrypt(Helper.global.ideasoft.access_token);
                if (Helper.global.google is not null && !string.IsNullOrWhiteSpace(Helper.global.google.oauth2_clientsecret))
                    Helper.global.google.oauth2_clientsecret = DBSetting.Decrypt(Helper.global.google.oauth2_clientsecret);
                #endregion

                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local"))) {
                    DbHelperBase.PrintConsole(Helper.global.settings?.company_name + " local enabled!!!", ConsoleColor.Yellow);
                    //Console.Beep();
                    if (_customer_id == 1) {
                        if (Helper.global?.netsis is not null && Helper.global?.entegra is not null) {
                            Helper.global.netsis.rest_url = "http://85.106.8.239:7070/";
                            Helper.global.entegra.api_url = "http://85.106.8.239:5421/";
                        }
                    }
                }

                return Helper.global;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                Helper.global = null;
                return null;
            }
        }

        public async Task<List<ActiveIntegration>?> LoadActiveIntegrations(int _customer_id) {
            try {
                var integrations = await LoadIntegrations(_customer_id);
                var platforms = await LoadPlatforms();
                var works = await LoadWorks();
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM active_integrations WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<ActiveIntegration> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new ActiveIntegration() {
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            work = works.Where(x => x.id == Convert.ToInt32(dataReader["WID"].ToString())).FirstOrDefault(),
                            platform = platforms.Where(x => x.id == Convert.ToInt32(dataReader["PID"].ToString())).FirstOrDefault(),
                            integration = integrations.Where(x => x.id == Convert.ToInt32(dataReader["IID"].ToString())).FirstOrDefault(),
                            platform_name = dataReader["platform_name"].ToString(),
                            work_name = dataReader["work_name"].ToString(),
                            platform_work_type = dataReader["platform_work_type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                            available_platform_types = dataReader["available_platform_types"] is string availableTypesStr ? [.. availableTypesStr.Split(',').Select(type => Enum.TryParse(type, out Merchanter.Classes.Platform.PlatformType platformType) ? platformType : default)] : [],
                            platform_status = Convert.ToBoolean(Convert.ToInt32(dataReader["platform_status"].ToString())),
                            work_type = dataReader["work_type"] is string workType1Str && Enum.TryParse(workType1Str, out Merchanter.Classes.Work.WorkType workType1) ? workType1 : default,
                            work_direction = dataReader["work_direction"] is string workDirectionStr && Enum.TryParse(workDirectionStr, out Merchanter.Classes.Work.WorkDirection workDirection) ? workDirection : default,
                            work_status = Convert.ToBoolean(Convert.ToInt32(dataReader["work_status"].ToString())),
                            version = dataReader["version"].ToString(),
                            integration_status = Convert.ToBoolean(Convert.ToInt32(dataReader["integration_status"].ToString())),
                        });
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
        /// Gets order statuses from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<OrderStatus>?> LoadOrderStatuses(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM order_statuses WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<OrderStatus> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new OrderStatus(
                            Convert.ToInt32(dataReader["id"].ToString()),
                             Convert.ToInt32(dataReader["customer_id"].ToString()),
                             dataReader["status_name"].ToString(),
                             dataReader["status_code"].ToString(),
                             dataReader["platform"].ToString(),
                             dataReader["platform_status_code"].ToString(),
                             Convert.ToBoolean(Convert.ToInt32(dataReader["sync_status"].ToString())),
                             Convert.ToBoolean(Convert.ToInt32(dataReader["process_status"].ToString()))
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets payment methods from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<PaymentMethod>?> LoadPaymentMethods(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM payment_methods WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<PaymentMethod> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new PaymentMethod(
                            Convert.ToInt32(dataReader["id"].ToString()),
                            Convert.ToInt32(dataReader["customer_id"].ToString()),
                            dataReader["payment_name"].ToString(),
                            dataReader["payment_code"].ToString(),
                            dataReader["platform"].ToString(),
                            dataReader["platform_payment_code"].ToString()
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets shipment methods from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ShipmentMethod>?> LoadShipmentMethods(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM shipment_methods WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<ShipmentMethod> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new ShipmentMethod(
                            Convert.ToInt32(dataReader["id"].ToString()),
                            Convert.ToInt32(dataReader["customer_id"].ToString()),
                            dataReader["shipment_name"].ToString(),
                            dataReader["shipment_code"].ToString(),
                            dataReader["platform"].ToString(),
                            dataReader["platform_shipment_code"].ToString()
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets customer integrations from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Integration>?> LoadIntegrations(int _customer_id) {
            try {
                var works = await LoadWorks();
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM integrations WHERE customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, Connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id.ToString()));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<Integration> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new Integration(
                            Convert.ToInt32(dataReader["id"].ToString()),
                            Convert.ToInt32(dataReader["customer_id"].ToString()),
                            Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                            works.FirstOrDefault(x => x.id == Convert.ToInt32(dataReader["work_id"].ToString()))
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets platforms from the database
        /// </summary>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Platform>?> LoadPlatforms() {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM platforms;";
                    MySqlCommand cmd = new(_query, Connection);
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<Platform> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new Platform(
                            Convert.ToInt32(dataReader["id"].ToString()),
                            dataReader["name"].ToString(),
                            dataReader["work_type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                            dataReader["available_types"] is string availableTypesStr ? [.. availableTypesStr.Split(',').Select(type => Enum.TryParse(type, out Merchanter.Classes.Platform.PlatformType platformType) ? platformType : default)] : [],
                            Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                            Convert.ToDateTime(dataReader["update_date"].ToString()),
                            dataReader["image"].ToString()
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
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
        /// Gets works from the database
        /// </summary>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Work>?> LoadWorks() {
            try {
                var platforms = await LoadPlatforms();
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM works;";
                    MySqlCommand cmd = new(_query, Connection);
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    List<Work> list = [];
                    while (await dataReader.ReadAsync()) {
                        list.Add(new Work(
                            Convert.ToInt32(dataReader["id"].ToString()),
                            platforms.FirstOrDefault(x => x.id == Convert.ToInt32(dataReader["platform_id"].ToString())),
                            dataReader["name"].ToString(),
                            dataReader["type"] is string workTypeStr && Enum.TryParse(workTypeStr, out Merchanter.Classes.Work.WorkType workType) ? workType : default,
                            dataReader["direction"] is string workDirectionStr && Enum.TryParse(workDirectionStr, out Merchanter.Classes.Work.WorkDirection workDirection) ? workDirection : default,
                            Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                            dataReader["version"].ToString()
                        ));
                    }
                    await dataReader.CloseAsync();
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return list;
                }

                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }
        #endregion
        #endregion
    }
}
