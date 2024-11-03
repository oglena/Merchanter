using Merchanter.Classes;
using Newtonsoft.Json;
using System.Diagnostics;
using Order = Merchanter.Classes.Order;

namespace Merchanter {
    public static partial class Helper {
        #region MAGENTO2
        public static M2_ProductStocks? GetProductStocks( int _scope = 0, int _max_qty = 99999 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product_stocks = Helper.global.magento.base_url + "rest/all/V1/stockItems/lowStock/?" +
                     "&scopeId=" + _scope.ToString() +
                     "&qty=" + _max_qty.ToString() +
                     "&currentPage=1&pageSize=99999";
                var m2_json = executioner.Execute( url_product_stocks, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_product_stocks = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_ProductStocks>( m2_json );
                    if( m2_product_stocks == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Product Stocks Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_product_stocks.items.Count().ToString() + " Magento Product Stocks Loaded." );
                    return m2_product_stocks;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Product Stocks Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_AttributeSets? GetAttributeSets( int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_attribute_set = Helper.global.magento.base_url + "rest/all/V1/products/attribute-sets/sets/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_attribute_set, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute_sets = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSets>( m2_json );
                    if( m2_attribute_sets == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Sets Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_attribute_sets.items.Count().ToString() + " Magento Attribute Sets Loaded." );
                    return m2_attribute_sets;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Sets Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static List<M2_Attribute>? GetAttributesBySetID( int _attribute_set_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_attribute = Helper.global.magento.base_url + "rest/all/V1/products/attribute-sets/" + _attribute_set_id.ToString() + "/attributes";
                var m2_json = executioner.Execute( url_attribute, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute_by_id = Newtonsoft.Json.JsonConvert.DeserializeObject<List<M2_Attribute>>( m2_json );
                    if( m2_attribute_by_id == null ) { Console.WriteLine( "Magento Attribute Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_attribute_by_id.Count().ToString() + " Magento Attribute Loaded for " + _attribute_set_id.ToString() );
                    return m2_attribute_by_id;
                }
                else {
                    Console.WriteLine( "Attribute Set(" + _attribute_set_id.ToString() + ") Magento Attributes Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_EAVAttributeSets? GetEAVAttributeSets( int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_eav_attribute_set = Helper.global.magento.base_url + "rest/all/V1/eav/attribute-sets/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_eav_attribute_set, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_eav_attribute_sets = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_EAVAttributeSets>( m2_json );
                    if( m2_eav_attribute_sets == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento EAV Attribute Sets Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_eav_attribute_sets.items.Count().ToString() + " Magento EAV Attribute Sets Loaded." );
                    return m2_eav_attribute_sets;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento EAV Attribute Sets Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_AttributeSetGroups? GetAttributeSetGroups( int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_attribute_set_group = Helper.global.magento.base_url + "rest/all/V1/products/attribute-sets/groups/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_attribute_set_group, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute_set_groups = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroups>( m2_json );
                    if( m2_attribute_set_groups == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Set Groups Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_attribute_set_groups.items.Count().ToString() + " Magento Attribute Set Groups Loaded." );
                    return m2_attribute_set_groups;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Set Groups Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_AttributeSetGroups? GetAttributeSetGroupsBySetID( int _attribute_set_id, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_attribute_set_group = Helper.global.magento.base_url + "rest/all/V1/products/attribute-sets/groups/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "attribute_set_id" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_set_id.ToString() + "" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_attribute_set_group, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute_set_groups = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroups>( m2_json );
                    if( m2_attribute_set_groups == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Set Groups Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_attribute_set_groups.items.Count().ToString() + " Magento Attribute Set Groups Loaded." );
                    return m2_attribute_set_groups;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Attribute Set Groups Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Categories? GetCategories( bool? _is_active, bool? _include_in_menu, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_category = Helper.global.magento.base_url + "rest/all/V1/categories/list?";
                if( _is_active.HasValue ) {
                    url_category += "&searchCriteria[filterGroups][0][filters][0][field]=" + "is_active" +
                      "&searchCriteria[filterGroups][0][filters][0][value]=" + (_is_active.Value ? "1" : "0") +
                      "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq";
                }
                if( _include_in_menu.HasValue ) {
                    if( _is_active.HasValue )
                        url_category += "&searchCriteria[filterGroups][1][filters][0][field]=" + "include_in_menu" +
                          "&searchCriteria[filterGroups][1][filters][0][value]=" + (_include_in_menu.Value ? "1" : "0") +
                          "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                    else
                        url_category += "&searchCriteria[filterGroups][0][filters][0][field]=" + "include_in_menu" +
                          "&searchCriteria[filterGroups][0][filters][0][value]=" + (_include_in_menu.Value ? "1" : "0") +
                          "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq";
                }
                url_category += "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_category, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>( m2_json );
                    if( m2_categories == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_categories.items.Count().ToString() + " Magento Categories Loaded." );
                    return m2_categories;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_CategoryList? GetCategoryList( int _root_category_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_category = Helper.global.magento.base_url + "rest/all/V1/categories?rootCategoryId=" + _root_category_id.ToString();
                var m2_json = executioner.Execute( url_category, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_CategoryList>( m2_json );
                    if( m2_categories == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_categories.children_data.Count().ToString() + " Magento Categories Loaded." );
                    return m2_categories;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Categories? SearchCategories( string _search_term, bool? _is_active, bool? _include_in_menu, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_category = Helper.global.magento.base_url + "rest/all/V1/categories/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like";
                if( _is_active != null ) {
                    url_category += "&searchCriteria[filterGroups][1][filters][0][field]=" + "is_active" +
                      "&searchCriteria[filterGroups][1][filters][0][value]=" + (_is_active.Value ? "1" : "0") +
                      "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                }
                if( _include_in_menu.HasValue ) {
                    if( _is_active.HasValue )
                        url_category += "&searchCriteria[filterGroups][2][filters][0][field]=" + "include_in_menu" +
                          "&searchCriteria[filterGroups][2][filters][0][value]=" + (_include_in_menu.Value ? "1" : "0") +
                          "&searchCriteria[filterGroups][2][filters][0][conditionType]=" + "eq";
                    else
                        url_category += "&searchCriteria[filterGroups][1][filters][0][field]=" + "include_in_menu" +
                          "&searchCriteria[filterGroups][1][filters][0][value]=" + (_include_in_menu.Value ? "1" : "0") +
                          "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                }
                url_category += "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_category, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>( m2_json );
                    if( m2_categories == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_categories.items.Count().ToString() + " Magento Categories Loaded." );
                    return m2_categories;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Categories Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Category? GetCategoryByName( string _name, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_category = Helper.global.magento.base_url + "rest/all/V1/categories/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=" + _name +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_category, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>( m2_json );
                    if( m2_categories == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Category Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_categories.items[ 0 ].id + " Magento Category Loaded." );
                    return m2_categories.items[ 0 ];
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Category Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Category? GetCategoryById( int _category_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_category = Helper.global.magento.base_url + "rest/all/V1/categories/" + _category_id.ToString();
                var m2_json = executioner.Execute( url_category, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_category = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Category>( m2_json );
                    if( m2_category == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Category Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_category.id.ToString() + " Magento Category Loaded." );
                    return m2_category;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Category Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_TaxRules? GetTaxRulesById( int _tax_rule_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_tax_class = Helper.global.magento.base_url + "rest/all/V1/taxRules/" + _tax_rule_id.ToString();
                var m2_json = executioner.Execute( url_tax_class, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_tax_class = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_TaxRules>( m2_json );
                    if( m2_tax_class == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Tax Class Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_tax_class.code.ToString() + " Magento Tax Class Loaded." );
                    return m2_tax_class;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Tax Class Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_TaxRate? GetTaxRateById( int _tax_rate_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_tax_rate = Helper.global.magento.base_url + "rest/all/V1/taxRates/" + _tax_rate_id.ToString();
                var m2_json = executioner.Execute( url_tax_rate, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_tax_rate = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_TaxRate>( m2_json );
                    if( m2_tax_rate == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Tax Class Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_tax_rate.code.ToString() + " Magento Tax Class Loaded." );
                    return m2_tax_rate;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Tax Class Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? GetProducts( bool? _status, string? _type, Dictionary<int, string>? _category_ids = null, int? _brand_id = null, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?";
                if( _status != null ) {
                    url_product += "&searchCriteria[filterGroups][0][filters][0][field]=" + "status" +
                                   "&searchCriteria[filterGroups][0][filters][0][value]=" + (_status.Value ? "1" : "2") +
                                   "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq";
                }
                if( _category_ids != null ) {
                    url_product += "&searchCriteria[filterGroups][0][filters][1][field]=" + "category_id" +
                                   "&searchCriteria[filterGroups][0][filters][1][value]=" + string.Join( ",", _category_ids.Keys ) +
                                   "&searchCriteria[filterGroups][0][filters][1][conditionType]=" + "in";
                }
                if( _brand_id != null ) {
                    url_product += "&searchCriteria[filterGroups][2][filters][0][field]=" + "brand" +
                                   "&searchCriteria[filterGroups][2][filters][0][value]=" + _brand_id.ToString() +
                                   "&searchCriteria[filterGroups][2][filters][0][conditionType]=" + "eq";
                }
                if( !string.IsNullOrWhiteSpace( _type ) ) {
                    url_product += "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][field]=" + "type_id" +
                                   "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][value]=" + _type +
                                   "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][conditionType]=" + "eq";
                }
                url_product += "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.items.Count().ToString() + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? GetProductsByStatus( bool _status, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "status" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + (_status ? "1" : "2") +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                    "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.items.Count().ToString() + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? GetProductsBySkus( List<string> _skus, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "sku" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + String.Join( ",", _skus.ToArray() ) +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "in" +
                    "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.items.Count().ToString() + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? SearchProducts( string _search_term, bool? _status, string? _type, Dictionary<int, string>? _category_ids = null, int? _brand_id = null, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][1][field]=" + "barcode" +
                     "&searchCriteria[filterGroups][0][filters][1][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][1][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][2][field]=" + "sku" +
                     "&searchCriteria[filterGroups][0][filters][2][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][2][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][3][field]=" + "brand" +
                     "&searchCriteria[filterGroups][0][filters][3][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][3][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][4][field]=" + "color" +
                     "&searchCriteria[filterGroups][0][filters][4][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][4][conditionType]=" + "like";
                if( _category_ids != null ) {
                    url_product += "&searchCriteria[filterGroups][0][filters][5][field]=" + "category_id" +
                                   "&searchCriteria[filterGroups][0][filters][5][value]=" + string.Join( ",", _category_ids.Keys ) +
                                   "&searchCriteria[filterGroups][0][filters][5][conditionType]=" + "in";
                }
                if( _brand_id != null ) {
                    url_product += "&searchCriteria[filterGroups][3][filters][6][field]=" + "brand" +
                                   "&searchCriteria[filterGroups][3][filters][6][value]=" + _brand_id.ToString() +
                                   "&searchCriteria[filterGroups][3][filters][6][conditionType]=" + "eq";
                }
                if( _status != null ) {
                    url_product += "&searchCriteria[filterGroups][1][filters][0][field]=" + "status" +
                      "&searchCriteria[filterGroups][1][filters][0][value]=" + (_status.Value ? "1" : "2") +
                      "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                }
                if( !string.IsNullOrWhiteSpace( _type ) ) {
                    url_product += "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][field]=" + "type_id" +
                     "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][value]=" + _type +
                     "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][conditionType]=" + "eq";
                }
                url_product += "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                     "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.items.Count().ToString() + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? SearchProductsByStatus( string _search_term, bool _status, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][1][field]=" + "barcode" +
                     "&searchCriteria[filterGroups][0][filters][1][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][1][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][2][field]=" + "sku" +
                     "&searchCriteria[filterGroups][0][filters][2][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][2][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][3][field]=" + "brand" +
                     "&searchCriteria[filterGroups][0][filters][3][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][3][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][0][filters][4][field]=" + "color" +
                     "&searchCriteria[filterGroups][0][filters][4][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][4][conditionType]=" + "like" +
                     "&searchCriteria[filterGroups][1][filters][0][field]=" + "status" +
                     "&searchCriteria[filterGroups][1][filters][0][value]=" + (_status ? "1" : "2") +
                     "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq" +
                     "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                     "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.items.Count().ToString() + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Attributes? GetProductAttributes( int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_p_attributes = Helper.global.magento.base_url + "rest/all/V1/products/attributes?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_p_attributes, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attributes = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attributes>( m2_json );
                    if( m2_attributes == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Product Attributes Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_attributes.items.Count().ToString() + " Magento Product Attributes Loaded." );
                    return m2_attributes;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Product Attributes Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Attribute? GetProductAttribute( string _attribute_code ) {
            using( Executioner executioner = new Executioner() ) {
                string url_p_attribute = Helper.global.magento.base_url + "rest/all/V1/products/attributes/" + _attribute_code;
                var m2_json = executioner.Execute( url_p_attribute, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attribute>( m2_json );
                    if( m2_attribute == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Product Attributes Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento " + m2_attribute.attribute_code + " Loaded." );
                    return m2_attribute;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento " + _attribute_code + "Product Attribute Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Attribute? GetProductAttribute( int _attribute_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_p_attribute = Helper.global.magento.base_url + "rest/all/V1/products/attributes?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "attribute_id" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_id.ToString() +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                    "&searchCriteria[currentPage]=" + "1".ToString() +
                    "&searchCriteria[pageSize]=" + "1".ToString();
                var m2_json = executioner.Execute( url_p_attribute, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attribute>( m2_json );
                    if( m2_attribute == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Product Attributes Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento " + m2_attribute.attribute_code + " Loaded." );
                    return m2_attribute;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento " + _attribute_id.ToString() + "Product Attribute Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static List<M2_Product>? SearchProductByAttribute( string _attribute_code, string _attribute_value, int _page_size = 99999, int _current_page = 1 ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    var url_product = string.Empty;
                    if( _page_size == 99999 ) {
                        List<M2_Product> m2_products = new List<M2_Product>();
                        _current_page = 1; _page_size = 250;
                    START:
                        url_product = Helper.global.magento.base_url + "index.php/rest/all/V1/products?" +
                            "&searchCriteria[filterGroups][0][filters][0][field]=" + _attribute_code +
                            "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_value +
                            "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                            "&searchCriteria[currentPage]=" + _current_page.ToString() +
                            "&searchCriteria[pageSize]=" + _page_size.ToString();
                        var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                        if( m2_json != null ) {
                            var query_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json )?.items?.ToList();
                            if( query_products != null && query_products.Count > 0 ) {
                                m2_products.AddRange( query_products );
                                _current_page++; Thread.Sleep( 10 ); goto START;
                            }

                            if( m2_products == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Products Load Failed. Exiting." ); return new List<M2_Product>(); }
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_products.Count.ToString() + " Magento Products Loaded." );
                            return m2_products;
                        }
                        else {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                            return null;
                        }
                    }
                    else {
                        url_product = Helper.global.magento.base_url + "index.php/rest/all/V1/products?" +
                            "&searchCriteria[filterGroups][0][filters][0][field]=" + _attribute_code +
                            "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_value +
                            "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                            "&searchCriteria[currentPage]=" + _current_page.ToString() +
                            "&searchCriteria[pageSize]=" + _page_size.ToString();
                        var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                        if( m2_json != null ) {
                            var query_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json )?.items?.ToList();
                            if( query_products == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Products Load Failed. Exiting." ); return new List<M2_Product>(); }
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + query_products.Count().ToString() + " Magento Products Loaded." );
                            return query_products;
                        }
                        else {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Products Load Failed. Exiting2." );
                            return null;
                        }
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        public static M2_AttributeSetGroup? InsertProductAttributeGroup( M2_AttributeSetGroup _attribute_set_group ) {
            using( Executioner executioner = new Executioner() ) {
                var json_body = new {
                    group = new {
                        attribute_group_name = _attribute_set_group.attribute_group_name,
                        attribute_set_id = _attribute_set_group.attribute_set_id
                    }
                };
                var m2_json = executioner.Execute( Helper.global.magento.base_url + "rest/all/V1/products/attribute-sets/groups", RestSharp.Method.Post, Newtonsoft.Json.JsonConvert.SerializeObject( json_body, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } ), Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_attribute_set_group = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroup>( m2_json );
                    Console.WriteLine( m2_attribute_set_group.attribute_set_id + "-" + m2_attribute_set_group.attribute_group_id.ToString() + "-" + m2_attribute_set_group.attribute_group_name + " ADDED!.." );

                    return m2_attribute_set_group;
                }
                else {
                    return null;
                }
            }
        }

        public static M2_StockItem? GetProductStock( string _sku, int _scope = 0 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_stock_item = Helper.global.magento.base_url + "rest/all/V1/stockItems/" + _sku + "?scopeId=" + _scope.ToString();
                var m2_json = executioner.Execute( url_stock_item, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_stock_item = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_StockItem>( m2_json );
                    if( m2_stock_item == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Product Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Loaded." );
                    return m2_stock_item;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Product? GetProductBySKU( string _sku ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    string url_product = Helper.global.magento.base_url + "rest/all/V1/products/" + _sku;
                    var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                    if( m2_json != null ) {
                        var m2_product = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Product>( m2_json );
                        if( m2_product == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Product Load Failed. Exiting." ); return null; }
                        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Product Loaded." );
                        return m2_product;
                    }
                    else {
                        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Product Load Failed. Exiting2." );
                        return null;
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Product Parse Error." + Environment.NewLine + ex.ToString() );
                return null;
            }
        }

        public static List<M2_ConfigurableChild>? GetConfigurableChildrenBySKU( string _sku ) {
            using( Executioner executioner = new Executioner() ) {
                string url_products = Helper.global.magento.base_url + "rest/all/V1/configurable-products/" + _sku + "/children";
                var m2_json = executioner.Execute( url_products, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<M2_ConfigurableChild>>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _sku + " Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Products? GetProductByIDs( List<int> _product_ids ) {
            using( Executioner executioner = new Executioner() ) {
                string url_product = Helper.global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "entity_id" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + string.Join( ",", _product_ids ) +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "in" +
                    "&searchCriteria[currentPage]=" + "1".ToString() +
                    "&searchCriteria[pageSize]=" + "1".ToString();
                var m2_json = executioner.Execute( url_product, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>( m2_json );
                    if( m2_products == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + string.Join( ",", _product_ids ) + " Magento Products Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + string.Join( ",", _product_ids ) + " Magento Products Loaded." );
                    return m2_products;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + string.Join( ",", _product_ids ) + " Magento Products Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Currency? GetCurrency() {
            using( Executioner executioner = new Executioner() ) {
                string url_currency = Helper.global.magento.base_url + "rest/all/V1/directory/currency";
                var m2_json = executioner.Execute( url_currency, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_currency = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Currency>( m2_json );
                    if( m2_currency == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + "Magento Currency Load Failed. Exiting." ); return null; }
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + "Magento Currency Loaded." );
                    return m2_currency;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Currency Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static string GetVisiblity( int _value ) {
            if( _value == 1 )
                return "Not Visible Individually";
            if( _value == 2 )
                return "Catalog";
            if( _value == 3 )
                return "Search";
            if( _value == 4 )
                return "Catalog, Search";
            return "";
        }

        public static List<M2_ProductType> GetProductTypes() {
            return new List<M2_ProductType>() {
                new M2_ProductType(){ label = "All Product Types", name = string.Empty},
                new M2_ProductType(){ label = "Simple Product", name = "simple"},
                new M2_ProductType(){ label = "Virtual Product", name = "virtual"},
                new M2_ProductType(){ label = "Bundle Product", name = "bundle"},
                new M2_ProductType(){ label = "Downloadable Product", name = "downloadable"},
                new M2_ProductType(){ label = "Grouped Product", name = "grouped"},
                new M2_ProductType(){ label = "Configurable Product", name = "configurable"},
            };
        }

        public static M2_Orders? GetOrders( int _daysto_ordersync, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_order = Helper.global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + ConvertDateToString( DateTime.Now.AddDays( _daysto_ordersync * -1 ).AddHours( 3 ), false ) +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=from" +
                    "&searchCriteria[filter_groups][1][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][1][filters][0][value]=" + ConvertDateToString( DateTime.Now.AddHours( 3 ), false ) +
                    "&searchCriteria[filter_groups][1][filters][0][condition_type]=to" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_order, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>( m2_json );
                    if( m2_orders == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Orders Load Failed. Exiting." ); return null; }
                    return m2_orders;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Orders Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Orders? GetOrders( int _daysto_ordersync, string[] _statuses, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_order = Helper.global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + ConvertDateToString( DateTime.Now.AddDays( _daysto_ordersync * -1 ).AddHours( 3 ), false ) +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=from" +
                    "&searchCriteria[filter_groups][1][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][1][filters][0][value]=" + ConvertDateToString( DateTime.Now.AddHours( 3 ), false ) +
                    "&searchCriteria[filter_groups][1][filters][0][condition_type]=to" +
                    "&searchCriteria[filter_groups][2][filters][0][field]=status" +
                    "&searchCriteria[filter_groups][2][filters][0][value]=" + string.Join( ",", _statuses ) +
                    "&searchCriteria[filter_groups][2][filters][0][condition_type]=in" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute( url_order, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>( m2_json );
                    if( m2_orders == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Orders Load Failed. Exiting." ); return null; }
                    return m2_orders;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " Magento Orders Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Order? GetOrderByIncrementID( string _increment_id ) {
            using( Executioner executioner = new Executioner() ) {
                string url_order = Helper.global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=increment_id" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + _increment_id +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=eq";
                var m2_json = executioner.Execute( url_order, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>( m2_json );
                    if( m2_orders != null ) {
                        var m2_order = m2_orders.items.Length > 0 ? m2_orders.items[ 0 ] : null;
                        if( m2_order == null ) { Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _increment_id + " Magento Order Load Failed. Exiting." ); return null; }
                        //Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + m2_order.increment_id + " Magento Order Loaded." );
                        return m2_order;
                    }
                    else
                        return null;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _increment_id + "Magento Order Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static M2_Customer? GetCustomer( string _email, int _page_size = 99999, int _current_page = 1 ) {
            using( Executioner executioner = new Executioner() ) {
                string url_customer = Helper.global.magento.base_url + "rest/all/V1/customers/search?" +
                "searchCriteria[filter_groups][0][filters][0][field]=email" +
                "&searchCriteria[filter_groups][0][filters][0][value]=" + _email +
                "&searchCriteria[filter_groups][0][filters][0][condition_type]=eq" +
                "&searchCriteria[currentPage]=1" +
                "&searchCriteria[pageSize]=1000";
                var m2_json = executioner.Execute( url_customer, RestSharp.Method.Get, _json: null, Helper.global.magento.token );
                if( m2_json != null ) {
                    var m2_customers = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Customers>( m2_json );
                    if( m2_customers == null ) { Console.WriteLine( "Magento Orders Load Failed. Exiting." ); return null; }
                    if( m2_customers.items.Length > 0 ) {
                        var c = m2_customers.items.Where( x => x.website_id == 2 ).FirstOrDefault();
                        if( c != null ) {
                            //Console.WriteLine( Magento2Helper.settings.company + " " + c.email + " Customer Loaded." );
                            return c;
                        }
                    }
                    return null;
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + Helper.global.settings.company_name + " " + _email + " Magento Customer Load Failed. Exiting2." );
                    return null;
                }
            }
        }

        public static Dictionary<string, string>? GetCustomerCorporateInfo( string _email, out bool _is_corporate ) {
            var selected_customer = GetCustomer( _email );
            Dictionary<string, string>? _company_infos = null;
            string firma_ismi = string.Empty;
            string firma_vergidairesi = string.Empty;
            string firma_vergino = string.Empty;
            string tc_no = string.Empty;
            _is_corporate = false;
            if( selected_customer != null ) {
                if( selected_customer.website_id == 2 ) {  //firma bilgileri doldur
                    firma_ismi = selected_customer.custom_attributes.Where( x => x.attribute_code == Helper.global.magento.customer_firma_ismi_attribute_code ).FirstOrDefault()?.value;
                    firma_vergidairesi = selected_customer.custom_attributes.Where( x => x.attribute_code == Helper.global.magento.customer_firma_vergidairesi_attribute_code ).FirstOrDefault()?.value;
                    firma_vergino = selected_customer.custom_attributes.Where( x => x.attribute_code == Helper.global.magento.customer_firma_vergino_attribute_code ).FirstOrDefault()?.value;
                    tc_no = selected_customer.custom_attributes.Where( x => x.attribute_code == Helper.global.magento.customer_tc_no_attribute_code ).FirstOrDefault()?.value;
                    if( !string.IsNullOrWhiteSpace( firma_vergidairesi ) && !string.IsNullOrWhiteSpace( firma_vergino ) && !string.IsNullOrWhiteSpace( firma_ismi ) ) {
                        _is_corporate = true;
                        _company_infos = new Dictionary<string, string> {
                            { Helper.global.magento.customer_firma_ismi_attribute_code, firma_ismi },
                            { Helper.global.magento.customer_firma_vergidairesi_attribute_code, firma_vergidairesi },
                            { Helper.global.magento.customer_firma_vergino_attribute_code, firma_vergino },
                            { Helper.global.magento.customer_tc_no_attribute_code, tc_no?? "11111111111" }
                        };
                        return _company_infos;
                    }
                    else {
                        _company_infos = new Dictionary<string, string> {
                            { Helper.global.magento.customer_tc_no_attribute_code, tc_no?? "11111111111" }
                        };
                        return _company_infos;
                    }
                }
            }
            return null;
        }

        public static string? CreateOrderInvoice( Order _order ) {
            try {
                var invoice = new M2_InvoiceRequest() {
                    capture = true, appendComment = true, notify = true, comment = new M2_InvoiceComment() {
                        comment = Helper.global.magento.order_processing_comment, is_visible_on_front = 0, extension_attributes = new()
                    }, items = [], arguments = new() { extension_attributes = new() }
                };
                foreach( var item in _order.order_items ) {
                    invoice.items.Add( new M2_InvoiceItems() { order_item_id = item.order_item_id, qty = item.qty_ordered, extension_attributes = new() } );
                }

                using( Executioner executioner = new Executioner() ) {
                    var json_invoice = executioner.Execute( Helper.global.magento.base_url + "rest/all/V1/order/" + _order.order_id.ToString() + "/invoice", RestSharp.Method.Post, invoice, Helper.global.magento.token );
                    if( json_invoice != null ) {
                        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + json_invoice + " magento invoice created." );
                        return json_invoice;
                    }
                    else {
                        return null;
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        public static string? ShipOrder( int _order_id, string _order_label, string _tracking_numbers, string _comment, string _carrier_code, string _carrier_title ) {
            try {
                var ship = new M2_ShippingRequest() {
                    notify = true, appendComment = true, comment = new M2_Shipment_Comment() {
                        comment = _comment, extension_attributes = new M2_Shipment_Extension_Attributes(), is_visible_on_front = 1
                    }, tracks = [ new M2_Shipment_Track(){
                         carrier_code = _carrier_code, title = _carrier_title, extension_attributes = new M2_Shipment_Extension_Attributes(),
                         track_number = _tracking_numbers
                    }]
                };

                using( Executioner executioner = new Executioner() ) {
                    var json_order = executioner.Execute( Helper.global.magento.base_url + "/index.php/rest/all/V1/order/" + _order_id.ToString() + "/ship", RestSharp.Method.Post, ship, Helper.global.magento.token );
                    if( json_order != null ) {
                        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + _order_label + ":" + _carrier_title + " => " + _tracking_numbers + " magento order shipped." );
                        return json_order;
                    }
                    else
                        return null;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        public static void ChangeOrderStatus( Order _order, string? _status ) {
            try {
                if( !string.IsNullOrWhiteSpace( _status ) ) {
                    var order = new { entity = new { entity_id = _order.order_id, increment_id = _order.order_label, status = _status } };

                    using( Executioner executioner = new Executioner() ) {
                        var json_order = executioner.Execute( Helper.global.magento.base_url + "rest/all/V1/orders", RestSharp.Method.Post, order, Helper.global.magento.token );
                        if( json_order != null ) {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + _order.order_status + " => " + _status + " magento order status changed!" );
                        }
                    }
                }
                else {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "status missing for " + _order.order_label );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "status missing for " + _order.order_label );
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
            }
        }




        public static List<Product>? BULK_UpdateProducts( List<Product> _products, CurrencyRates _currency_rates ) {
            try {
                var prices = new { prices = new List<M2_PriceRequest>() { } };
                var special_prices = new { prices = new List<M2_SpecialPriceRequest>() { } };
                var special_prices_delete = new { prices = new List<M2_SpecialPriceDeleteRequest>() { } };
                foreach( var item in _products ) {
                    if( item.price > 0 ) {
                        prices.prices.Add( new M2_PriceRequest() {
                            price = Math.Round(
                                item.price *
                                (item.tax_included ? 1 : (1 + ((decimal)item.tax / 100m))) *
                                ((item.currency == "USD") ? _currency_rates.USD : ((item.currency == "EUR") ? _currency_rates.EUR : _currency_rates.TL))
                            , 2, MidpointRounding.AwayFromZero ),
                            store_id = 0,
                            sku = item.sku,
                            extension_attributes = { }
                        } );
                    }
                    if( item.special_price > 0 ) {
                        special_prices.prices.Add( new M2_SpecialPriceRequest() {
                            price = Math.Round(
                                item.special_price *
                                (item.tax_included ? 1 : (1 + ((decimal)item.tax / 100m))) *
                                ((item.currency == "USD") ? _currency_rates.USD : ((item.currency == "EUR") ? _currency_rates.EUR : _currency_rates.TL))
                            , 2, MidpointRounding.AwayFromZero ),
                            store_id = 0,
                            sku = item.sku,
                            extension_attributes = { },
                            price_from = DateTime.Now.AddDays( -1 ).ToString( "yyyy-MM-dd 00:00:00" ),
                            price_to = ""
                        } );
                    }
                    else {
                        special_prices_delete.prices.Add( new M2_SpecialPriceDeleteRequest() {
                            price = 0,
                            store_id = 0,
                            sku = item.sku,
                            price_from = "",
                            price_to = "",
                            extension_attributes = { }
                        } );
                    }
                }

                using( Executioner executioner = new Executioner() ) {
                    var json_price_bulk = executioner.Execute( Helper.global.magento.base_url + "/index.php/rest/all/V1/products/base-prices", RestSharp.Method.Post, prices, Helper.global.magento.token );
                    var json_special_prices_bulk = executioner.Execute( Helper.global.magento.base_url + "/index.php/rest/all/V1/products/special-price", RestSharp.Method.Post, special_prices, Helper.global.magento.token );
                    var json_special_prices_delete_bulk = executioner.Execute( Helper.global.magento.base_url + "/index.php/rest/all/V1/products/special-price-delete", RestSharp.Method.Post, special_prices_delete, Helper.global.magento.token );
                }
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "BULK update prices completed." );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "BULK update prices completed." );

                foreach( var item in _products ) {
                    if( item.custom_price > 0 ) {
                        UpdateProductCustomPrice( item, _currency_rates );
                    }
                }
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Bulk update custom prices completed." );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Bulk update custom prices completed." );

                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Count:" + _products.Count.ToString() );

                return _products;
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        public static bool UpdateProductPrice( Product _product, CurrencyRates _currency_rates ) {
            try {
                var prices = new {
                    prices = new M2_PriceRequest[] {
                        new M2_PriceRequest(){
                            price = Math.Round(
                                _product.price *
                                (_product.tax_included ? 1 : (1 + ( (decimal)_product.tax / 100m))) *
                                ((_product.currency == "USD") ? _currency_rates.USD : ((_product.currency == "EUR") ? _currency_rates.EUR : _currency_rates.TL))
                            , 2, MidpointRounding.AwayFromZero ),
                            store_id = 0,
                            sku = _product.sku,
                            extension_attributes = new M2_PriceExtensionAttributes[] { }
                    } }
                };
                using( Executioner executioner = new Executioner() ) {
                    var json_price = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/base-prices", RestSharp.Method.Post, prices, Helper.global.magento.token );
                    if( json_price != null ) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return false;
            }
        }

        public static bool UpdateProductSpecialPrice( Product _product, CurrencyRates _currency_rates ) {
            try {
                if( _product.special_price > 0 ) {
                    var special_prices = new {
                        prices = new M2_SpecialPriceRequest[]{
                            new M2_SpecialPriceRequest(){
                                price = Math.Round(
                                    _product.special_price *
                                    (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) *
                                    ((_product.currency == "USD") ? _currency_rates.USD : ((_product.currency == "EUR") ? _currency_rates.EUR : _currency_rates.TL))
                                , 2, MidpointRounding.AwayFromZero ),
                                store_id = 0,
                                sku = _product.sku,
                                price_from = DateTime.Now.AddDays( -2 ).ToString( "yyyy-MM-dd 00:00:00" ),
                                price_to = "",
                                extension_attributes = { }
                        } }
                    };
                    using( Executioner executioner = new Executioner() ) {
                        var json_special_prices = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/special-price", RestSharp.Method.Post, special_prices, Helper.global.magento.token );
                        if( json_special_prices != null ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    var special_prices_delete = new {
                        prices = new M2_SpecialPriceDeleteRequest[] {
                            new M2_SpecialPriceDeleteRequest(){
                            price = 0,
                            store_id = 0,
                            sku = _product.sku,
                            price_from = "",
                            price_to = "",
                            extension_attributes = { }
                        } }
                    };
                    using( Executioner executioner = new Executioner() ) {
                        var json_special_prices_delete = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/special-price-delete", RestSharp.Method.Post, special_prices_delete, Helper.global.magento.token );
                        if( json_special_prices_delete != null ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return false;
            }
        }

        public static bool? UpdateProductCustomPrice( Product _product, CurrencyRates _currency_rates ) {
            int? product_id = QP_MySQLHelper.GetM2ProductId( _product.sku );
            if( product_id.HasValue ) {
                if( _product.custom_price > 0 ) {
                }
                else {
                    if( _product.special_price > 0 )
                        _product.custom_price = _product.special_price;
                    else
                        _product.custom_price = _product.price;
                }
                bool? temp = QP_MySQLHelper.QP_UpdateCustomBundlePC( product_id.Value.ToString(), Math.Round(
                                    _product.custom_price *
                                    (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) *
                                    ((_product.currency == "USD") ? _currency_rates.USD : ((_product.currency == "EUR") ? _currency_rates.EUR : _currency_rates.TL))
                                , 2, MidpointRounding.AwayFromZero ) );
                if( temp.HasValue && temp.Value ) {
                    return true;
                }
                else if( temp.HasValue && !temp.Value )
                    return false;
                else
                    return null;
            }
            return null;
        }

        public static bool UpdateProductAttribute( string _sku, string _attribute_code, string? _attribute_value ) {
            try {
                var product = new {
                    product = new {
                        custom_attributes = new M2_CustomAttributes[] { new M2_CustomAttributes(){
                            attribute_code = _attribute_code,
                            value = !string.IsNullOrWhiteSpace( _attribute_value ) ? _attribute_value.Trim() : string.Empty

                        } }
                    }
                };

                using( Executioner executioner = new Executioner() ) {
                    var json_product = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/" + ConvertFriendly( _sku ), RestSharp.Method.Put, product, Helper.global.magento.token );
                    if( json_product != null ) {
                        var updated_product = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Product>( json_product );
                        if( updated_product != null ) {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] Sku:" + _sku + " updated => [" + _attribute_code + "=" + _attribute_value + "]" );
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return false;
            }
        }

        public static string? InsertAttributeOption( string _attribute_code, string? _attribute_value ) {
            try {
                if( !string.IsNullOrWhiteSpace( _attribute_value ) ) return null;
                var attribute_option = new M2_AttributeOption() {
                    option = new M2_Option() {
                        value = _attribute_value,
                        store_labels = [ new M2_StoreLabels() { store_id = 0, label = _attribute_value } ],
                        is_default = false,
                        sort_order = 0
                    }
                };

                using( Executioner executioner = new Executioner() ) {
                    var json_product = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/attributes/" + ConvertFriendly( _attribute_code ) + "/options", RestSharp.Method.Post, attribute_option, Helper.global.magento.token );
                    if( json_product != null ) {
                        var updated_id = Newtonsoft.Json.JsonConvert.DeserializeObject<string>( json_product );
                        if( updated_id != null ) {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] Attribute Code:" + _attribute_code + " option inserted => [" + _attribute_code + "=" + _attribute_value + "]" );
                            return updated_id;
                        }
                    }
                    return null;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        public static bool UpdateProductQty( string _sku, int _qty ) {
            try {
                var stock_item = new {
                    stockItem = new M2_StockItemRequest() {
                        qty = _qty,
                        is_in_stock = (_qty > 0) ? true : false
                    }
                };

                using( Executioner executioner = new Executioner() ) {
                    var json_qty = executioner.Execute( Helper.global.magento.base_url + "index.php/rest/all/V1/products/" + ConvertFriendly( _sku ) + "/stockItems/1", RestSharp.Method.Put, stock_item, Helper.global.magento.token );
                    if( json_qty != null ) {
                        return true;
                    }
                    return false;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return false;
            }
        }
        #endregion
    }
}
