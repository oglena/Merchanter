using Merchanter.Classes;
using Newtonsoft.Json;
using Order = Merchanter.Classes.Order;

namespace Merchanter {
    public static partial class Helper {
        #region MAGENTO2
        public static M2_ProductStocks? GetProductStocks(int _scope = 0, int _max_qty = 99999) {
            using (Executioner executioner = new Executioner()) {
                string url_product_stocks = global.magento.base_url + "rest/all/V1/stockItems/lowStock/?" +
                     "&scopeId=" + _scope.ToString() +
                     "&qty=" + _max_qty.ToString() +
                     "&currentPage=1&pageSize=99999";
                var m2_json = executioner.Execute(url_product_stocks, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_product_stocks = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_ProductStocks>(m2_json);
                    if (m2_product_stocks == null) { PrintConsole("Magento Product Stocks Load Failed. Exiting."); return null; }
                    PrintConsole(m2_product_stocks.items.Count().ToString() + " Magento Product Stocks Loaded.");
                    return m2_product_stocks;
                }
                else {
                    PrintConsole("Magento Product Stocks Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_AttributeSets? GetAttributeSets(int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_attribute_set = global.magento.base_url + "rest/all/V1/products/attribute-sets/sets/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_attribute_set, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute_sets = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSets>(m2_json);
                    if (m2_attribute_sets == null) { PrintConsole("Magento Attribute Sets Load Failed. Exiting."); return null; }
                    PrintConsole(m2_attribute_sets.items.Count().ToString() + " Magento Attribute Sets Loaded.");
                    return m2_attribute_sets;
                }
                else {
                    PrintConsole("Magento Attribute Sets Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static List<M2_Attribute>? GetAttributesBySetID(int _attribute_set_id) {
            using (Executioner executioner = new Executioner()) {
                string url_attribute = global.magento.base_url + "rest/all/V1/products/attribute-sets/" + _attribute_set_id.ToString() + "/attributes";
                var m2_json = executioner.Execute(url_attribute, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute_by_id = Newtonsoft.Json.JsonConvert.DeserializeObject<List<M2_Attribute>>(m2_json);
                    if (m2_attribute_by_id == null) { PrintConsole("Magento Attribute Load Failed. Exiting."); return null; }
                    PrintConsole(m2_attribute_by_id.Count().ToString() + " Magento Attribute Loaded for " + _attribute_set_id.ToString());
                    return m2_attribute_by_id;
                }
                else {
                    PrintConsole("Attribute Set(" + _attribute_set_id.ToString() + ") Magento Attributes Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_EAVAttributeSets? GetEAVAttributeSets(int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_eav_attribute_set = global.magento.base_url + "rest/all/V1/eav/attribute-sets/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_eav_attribute_set, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_eav_attribute_sets = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_EAVAttributeSets>(m2_json);
                    if (m2_eav_attribute_sets == null) { PrintConsole("Magento EAV Attribute Sets Load Failed. Exiting."); return null; }
                    PrintConsole(m2_eav_attribute_sets.items.Count().ToString() + " Magento EAV Attribute Sets Loaded.");
                    return m2_eav_attribute_sets;
                }
                else {
                    PrintConsole("Magento EAV Attribute Sets Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_AttributeSetGroups? GetAttributeSetGroups(int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_attribute_set_group = global.magento.base_url + "rest/all/V1/products/attribute-sets/groups/list?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_attribute_set_group, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute_set_groups = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroups>(m2_json);
                    if (m2_attribute_set_groups == null) { PrintConsole("Magento Attribute Set Groups Load Failed. Exiting."); return null; }
                    PrintConsole(m2_attribute_set_groups.items.Count().ToString() + " Magento Attribute Set Groups Loaded.");
                    return m2_attribute_set_groups;
                }
                else {
                    PrintConsole("Magento Attribute Set Groups Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_AttributeSetGroups? GetAttributeSetGroupsBySetID(int _attribute_set_id, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_attribute_set_group = global.magento.base_url + "rest/all/V1/products/attribute-sets/groups/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "attribute_set_id" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_set_id.ToString() + "" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_attribute_set_group, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute_set_groups = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroups>(m2_json);
                    if (m2_attribute_set_groups == null) { PrintConsole("Magento Attribute Set Groups Load Failed. Exiting."); return null; }
                    PrintConsole(m2_attribute_set_groups.items.Count().ToString() + " Magento Attribute Set Groups Loaded.");
                    return m2_attribute_set_groups;
                }
                else {
                    PrintConsole("Magento Attribute Set Groups Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Categories? GetCategories(bool? _is_active, bool? _include_in_menu, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_category = global.magento.base_url + "rest/all/V1/categories/list?";
                if (_is_active.HasValue) {
                    url_category += "&searchCriteria[filterGroups][0][filters][0][field]=" + "is_active" +
                      "&searchCriteria[filterGroups][0][filters][0][value]=" + (_is_active.Value ? "1" : "0") +
                      "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq";
                }
                if (_include_in_menu.HasValue) {
                    if (_is_active.HasValue)
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
                var m2_json = executioner.Execute(url_category, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>(m2_json);
                    if (m2_categories == null) { PrintConsole("Magento Categories Load Failed. Exiting."); return null; }
                    PrintConsole(m2_categories.items.Count().ToString() + " Magento Categories Loaded.", false);
                    return m2_categories;
                }
                else {
                    PrintConsole("Magento Categories Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_CategoryList? GetCategoryList(int _root_category_id) {
            using (Executioner executioner = new Executioner()) {
                string url_category = global.magento.base_url + "rest/all/V1/categories?rootCategoryId=" + _root_category_id.ToString();
                var m2_json = executioner.Execute(url_category, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_CategoryList>(m2_json);
                    if (m2_categories == null) { PrintConsole("Magento Categories Load Failed. Exiting."); return null; }
                    PrintConsole(m2_categories.children_data.Count().ToString() + " Magento Categories Loaded.");
                    return m2_categories;
                }
                else {
                    PrintConsole("Magento Categories Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Categories? SearchCategories(string _search_term, bool? _is_active, bool? _include_in_menu, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_category = global.magento.base_url + "rest/all/V1/categories/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=%25" + _search_term + "%25" +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like";
                if (_is_active != null) {
                    url_category += "&searchCriteria[filterGroups][1][filters][0][field]=" + "is_active" +
                      "&searchCriteria[filterGroups][1][filters][0][value]=" + (_is_active.Value ? "1" : "0") +
                      "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                }
                if (_include_in_menu.HasValue) {
                    if (_is_active.HasValue)
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
                var m2_json = executioner.Execute(url_category, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>(m2_json);
                    if (m2_categories == null) { PrintConsole("Magento Categories Load Failed. Exiting."); return null; }
                    PrintConsole(m2_categories.items.Count().ToString() + " Magento Categories Loaded.");
                    return m2_categories;
                }
                else {
                    PrintConsole("Magento Categories Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Category? GetCategoryByName(string _name, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_category = global.magento.base_url + "rest/all/V1/categories/list?" +
                     "&searchCriteria[filterGroups][0][filters][0][field]=" + "name" +
                     "&searchCriteria[filterGroups][0][filters][0][value]=" + _name +
                     "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "like" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_category, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Categories>(m2_json);
                    if (m2_categories == null) { PrintConsole("Magento Category Load Failed. Exiting."); return null; }
                    PrintConsole(m2_categories.items[0].id + " Magento Category Loaded.");
                    return m2_categories.items[0];
                }
                else {
                    PrintConsole("Magento Category Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Category? GetCategoryById(int _category_id) {
            using (Executioner executioner = new Executioner()) {
                string url_category = global.magento.base_url + "rest/all/V1/categories/" + _category_id.ToString();
                var m2_json = executioner.Execute(url_category, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_category = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Category>(m2_json);
                    if (m2_category == null) { PrintConsole("Magento Category Load Failed. Exiting."); return null; }
                    PrintConsole(m2_category.id.ToString() + " Magento Category Loaded.");
                    return m2_category;
                }
                else {
                    PrintConsole("Magento Category Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_TaxRules? GetTaxRulesById(int _tax_rule_id) {
            using (Executioner executioner = new Executioner()) {
                string url_tax_class = global.magento.base_url + "rest/all/V1/taxRules/" + _tax_rule_id.ToString();
                var m2_json = executioner.Execute(url_tax_class, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_tax_class = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_TaxRules>(m2_json);
                    if (m2_tax_class == null) { PrintConsole("Magento Tax Class Load Failed. Exiting2."); return null; }
                    PrintConsole(m2_tax_class.code.ToString() + " Magento Tax Class Loaded.");
                    return m2_tax_class;
                }
                else {
                    PrintConsole("Magento Tax Class Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_TaxRate? GetTaxRateById(int _tax_rate_id) {
            using (Executioner executioner = new Executioner()) {
                string url_tax_rate = global.magento.base_url + "rest/all/V1/taxRates/" + _tax_rate_id.ToString();
                var m2_json = executioner.Execute(url_tax_rate, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_tax_rate = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_TaxRate>(m2_json);
                    if (m2_tax_rate == null) { PrintConsole("Magento Tax Class Load Failed. Exiting2."); return null; }
                    PrintConsole(m2_tax_rate.code.ToString() + " Magento Tax Class Loaded.");
                    return m2_tax_rate;
                }
                else {
                    PrintConsole("Magento Tax Class Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? GetProducts(bool? _status, string? _type, Dictionary<int, string>? _category_ids = null, int? _brand_id = null, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?";
                if (_status != null) {
                    url_product += "&searchCriteria[filterGroups][0][filters][0][field]=" + "status" +
                                   "&searchCriteria[filterGroups][0][filters][0][value]=" + (_status.Value ? "1" : "2") +
                                   "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq";
                }
                if (_category_ids != null) {
                    url_product += "&searchCriteria[filterGroups][0][filters][1][field]=" + "category_id" +
                                   "&searchCriteria[filterGroups][0][filters][1][value]=" + string.Join(",", _category_ids.Keys) +
                                   "&searchCriteria[filterGroups][0][filters][1][conditionType]=" + "in";
                }
                if (_brand_id != null) {
                    url_product += "&searchCriteria[filterGroups][2][filters][0][field]=" + "brand" +
                                   "&searchCriteria[filterGroups][2][filters][0][value]=" + _brand_id.ToString() +
                                   "&searchCriteria[filterGroups][2][filters][0][conditionType]=" + "eq";
                }
                if (!string.IsNullOrWhiteSpace(_type)) {
                    url_product += "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][field]=" + "type_id" +
                                   "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][value]=" + _type +
                                   "&searchCriteria[filterGroups][" + ((_status != null) ? "1" : "0") + "][filters][0][conditionType]=" + "eq";
                }
                url_product += "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting2."); return null; }
                    PrintConsole(m2_products.items.Count().ToString() + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole("Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? GetProductsByStatus(bool _status, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "status" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + (_status ? "1" : "2") +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                    "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(m2_products.items.Count().ToString() + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole("Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? GetProductsBySkus(List<string> _skus, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "sku" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + String.Join(",", _skus.ToArray()) +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "in" +
                    "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                    "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(m2_products.items.Count().ToString() + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole("Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? SearchProducts(string _search_term, bool? _status, string? _type, Dictionary<int, string>? _category_ids = null, int? _brand_id = null, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?" +
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
                if (_category_ids != null) {
                    url_product += "&searchCriteria[filterGroups][0][filters][5][field]=" + "category_id" +
                                   "&searchCriteria[filterGroups][0][filters][5][value]=" + string.Join(",", _category_ids.Keys) +
                                   "&searchCriteria[filterGroups][0][filters][5][conditionType]=" + "in";
                }
                if (_brand_id != null) {
                    url_product += "&searchCriteria[filterGroups][3][filters][6][field]=" + "brand" +
                                   "&searchCriteria[filterGroups][3][filters][6][value]=" + _brand_id.ToString() +
                                   "&searchCriteria[filterGroups][3][filters][6][conditionType]=" + "eq";
                }
                if (_status != null) {
                    url_product += "&searchCriteria[filterGroups][1][filters][0][field]=" + "status" +
                      "&searchCriteria[filterGroups][1][filters][0][value]=" + (_status.Value ? "1" : "2") +
                      "&searchCriteria[filterGroups][1][filters][0][conditionType]=" + "eq";
                }
                if (!string.IsNullOrWhiteSpace(_type)) {
                    url_product += "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][field]=" + "type_id" +
                     "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][value]=" + _type +
                     "&searchCriteria[filterGroups][" + ((_status != null) ? "2" : "1") + "][filters][0][conditionType]=" + "eq";
                }
                url_product += "&searchCriteria[sortOrders][0][field]=" + "created_at" +
                     "&searchCriteria[sortOrders][0][direction]=" + "desc" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(m2_products.items.Count().ToString() + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole("Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? SearchProductsByStatus(string _search_term, bool _status, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?" +
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
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(m2_products.items.Count().ToString() + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole("Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Attributes? GetProductAttributes(int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_p_attributes = global.magento.base_url + "rest/all/V1/products/attributes?" +
                     "&searchCriteria[currentPage]=" + _current_page.ToString() +
                     "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_p_attributes, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attributes = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attributes>(m2_json);
                    if (m2_attributes == null) { PrintConsole("Magento Product Attributes Load Failed. Exiting."); return null; }
                    PrintConsole(m2_attributes.items.Count().ToString() + " Magento Product Attributes Loaded.");
                    return m2_attributes;
                }
                else {
                    PrintConsole("Magento Product Attributes Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Magento Product Attribute by Attribute Code
        /// </summary>
        /// <param name="_attribute_code">Magento Attribute Code</param>
        /// <returns>Magento Product Attribute</returns>
        public static M2_Attribute? GetProductAttribute(string _attribute_code) {
            using (Executioner executioner = new Executioner()) {
                string url_p_attribute = global.magento.base_url + "rest/all/V1/products/attributes/" + _attribute_code;
                var m2_json = executioner.Execute(url_p_attribute, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attribute>(m2_json);
                    if (m2_attribute == null) { PrintConsole("Magento Product Attributes Load Failed. Exiting."); return null; }
                    PrintConsole("Magento " + m2_attribute.attribute_code + " Loaded.", false);
                    return m2_attribute;
                }
                else {
                    PrintConsole("Magento " + _attribute_code + "Product Attribute Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Attribute? GetProductAttribute(int _attribute_id) {
            using (Executioner executioner = new Executioner()) {
                string url_p_attribute = global.magento.base_url + "rest/all/V1/products/attributes?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "attribute_id" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_id.ToString() +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                    "&searchCriteria[currentPage]=" + "1".ToString() +
                    "&searchCriteria[pageSize]=" + "1".ToString();
                var m2_json = executioner.Execute(url_p_attribute, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_attribute = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Attribute>(m2_json);
                    if (m2_attribute == null) { PrintConsole("Product Attributes Load Failed. Exiting."); return null; }
                    PrintConsole("Magento " + m2_attribute.attribute_code + " Loaded.");
                    return m2_attribute;
                }
                else {
                    PrintConsole("Magento " + _attribute_id.ToString() + "Product Attribute Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Magento Product Attribute by Attribute Code
        /// </summary>
        /// <param name="_attribute_code">Magento Attribute Code</param>
        /// <param name="_attribute_value">Magento Attribute Value</param>
        /// <param name="_page_size">Page Size</param>
        /// <param name="_current_page">Current Page</param>
        /// <returns>Magento Products</returns>
        public static List<M2_Product>? SearchProductByAttribute(string _attribute_code, string _attribute_value, int _page_size = 99999, int _current_page = 1) {
            try {
                using (Executioner executioner = new Executioner()) {
                    var url_product = string.Empty;
                    if (_page_size == 99999) {
                        List<M2_Product> m2_products = new List<M2_Product>();
                        _current_page = 1; _page_size = 250;
                    START:
                        url_product = global.magento.base_url + "index.php/rest/all/V1/products?" +
                            "&searchCriteria[filterGroups][0][filters][0][field]=" + _attribute_code +
                            "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_value +
                            "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                            "&searchCriteria[currentPage]=" + _current_page.ToString() +
                            "&searchCriteria[pageSize]=" + _page_size.ToString();
                        var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                        if (m2_json != null) {
                            var query_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json)?.items?.ToList();
                            if (query_products != null) {
                                if (query_products.Count == _page_size || query_products.Count > 0) {
                                    m2_products.AddRange(query_products);
                                    _current_page++;
                                    Thread.Sleep(10); goto START;
                                }
                            }

                            if (m2_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return new List<M2_Product>(); }
                            PrintConsole(m2_products.Count.ToString() + " Magento Products Loaded.");
                            return m2_products;
                        }
                        else {
                            PrintConsole("Magento Products Load Failed. Exiting2.");
                            return null;
                        }
                    }
                    else {
                        url_product = global.magento.base_url + "index.php/rest/all/V1/products?" +
                            "&searchCriteria[filterGroups][0][filters][0][field]=" + _attribute_code +
                            "&searchCriteria[filterGroups][0][filters][0][value]=" + _attribute_value +
                            "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "eq" +
                            "&searchCriteria[currentPage]=" + _current_page.ToString() +
                            "&searchCriteria[pageSize]=" + _page_size.ToString();
                        var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                        if (m2_json != null) {
                            var query_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json)?.items?.ToList();
                            if (query_products == null) { PrintConsole("Magento Products Load Failed. Exiting."); return new List<M2_Product>(); }
                            PrintConsole(query_products.Count().ToString() + " Magento Products Loaded.");
                            return query_products;
                        }
                        else {
                            PrintConsole("Magento Products Load Failed. Exiting2.");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        public static M2_AttributeSetGroup? InsertProductAttributeGroup(M2_AttributeSetGroup _attribute_set_group) {
            using (Executioner executioner = new Executioner()) {
                var json_body = new {
                    group = new {
                        attribute_group_name = _attribute_set_group.attribute_group_name,
                        attribute_set_id = _attribute_set_group.attribute_set_id
                    }
                };
                var m2_json = executioner.Execute(global.magento.base_url + "rest/all/V1/products/attribute-sets/groups", RestSharp.Method.Post, Newtonsoft.Json.JsonConvert.SerializeObject(json_body, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), global.magento.token);
                if (m2_json != null) {
                    var m2_attribute_set_group = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_AttributeSetGroup>(m2_json);
                    PrintConsole(m2_attribute_set_group.attribute_set_id + "-" + m2_attribute_set_group.attribute_group_id.ToString() + "-" + m2_attribute_set_group.attribute_group_name + " ADDED!..");

                    return m2_attribute_set_group;
                }
                else {
                    return null;
                }
            }
        }

        public static M2_StockItem? GetProductStock(string _sku, int _scope = 0) {
            using (Executioner executioner = new Executioner()) {
                string url_stock_item = global.magento.base_url + "rest/all/V1/stockItems/" + _sku + "?scopeId=" + _scope.ToString();
                var m2_json = executioner.Execute(url_stock_item, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_stock_item = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_StockItem>(m2_json);
                    if (m2_stock_item == null) { PrintConsole(_sku + " Magento Product Load Failed. Exiting."); return null; }
                    PrintConsole(_sku + " Loaded.");
                    return m2_stock_item;
                }
                else {
                    PrintConsole(_sku + " Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Magento Product by SKU
        /// </summary>
        /// <param name="_sku">Magento Product SKU</param>
        /// <returns>Magento Product</returns>
        public static M2_Product? GetProductBySKU(string _sku) {
            try {
                using (Executioner executioner = new Executioner()) {
                    string url_product = global.magento.base_url + "rest/all/V1/products/" + _sku;
                    var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                    if (m2_json != null) {
                        var m2_product = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Product>(m2_json);
                        if (m2_product == null) { PrintConsole(_sku + " Magento Product Load Failed. Exiting."); return null; }
                        PrintConsole(_sku + " Magento Product Loaded.", false);
                        return m2_product;
                    }
                    else {
                        PrintConsole(_sku + " Magento Product Load Failed. Exiting2.");
                        return null;
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(_sku + " Magento Product Parse Error." + Environment.NewLine + ex.ToString());
                return null;
            }
        }

        public static List<M2_ConfigurableChild>? GetConfigurableChildrenBySKU(string _sku) {
            using (Executioner executioner = new Executioner()) {
                string url_products = global.magento.base_url + "rest/all/V1/configurable-products/" + _sku + "/children";
                var m2_json = executioner.Execute(url_products, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<M2_ConfigurableChild>>(m2_json);
                    if (m2_products == null) { PrintConsole(_sku + " Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(_sku + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole(_sku + " Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Products? GetProductByIDs(List<int> _product_ids) {
            using (Executioner executioner = new Executioner()) {
                string url_product = global.magento.base_url + "rest/all/V1/products?" +
                    "&searchCriteria[filterGroups][0][filters][0][field]=" + "entity_id" +
                    "&searchCriteria[filterGroups][0][filters][0][value]=" + string.Join(",", _product_ids) +
                    "&searchCriteria[filterGroups][0][filters][0][conditionType]=" + "in" +
                    "&searchCriteria[currentPage]=" + "1".ToString() +
                    "&searchCriteria[pageSize]=" + "1".ToString();
                var m2_json = executioner.Execute(url_product, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_products = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Products>(m2_json);
                    if (m2_products == null) { PrintConsole(string.Join(",", _product_ids) + " Magento Products Load Failed. Exiting."); return null; }
                    PrintConsole(string.Join(",", _product_ids) + " Magento Products Loaded.");
                    return m2_products;
                }
                else {
                    PrintConsole(string.Join(",", _product_ids) + " Magento Products Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Currency? GetCurrency() {
            using (Executioner executioner = new Executioner()) {
                string url_currency = global.magento.base_url + "rest/all/V1/directory/currency";
                var m2_json = executioner.Execute(url_currency, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_currency = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Currency>(m2_json);
                    if (m2_currency == null) { PrintConsole("Magento Currency Load Failed. Exiting."); return null; }
                    PrintConsole("[" + DateTime.Now.ToString() + "] " + global.settings.company_name + "Magento Currency Loaded.");
                    return m2_currency;
                }
                else {
                    PrintConsole("[" + DateTime.Now.ToString() + "] " + global.settings.company_name + " Magento Currency Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static string GetVisiblity(int _value) {
            if (_value == 1)
                return "Not Visible Individually";
            if (_value == 2)
                return "Catalog";
            if (_value == 3)
                return "Search";
            if (_value == 4)
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

        /// <summary>
        /// Get Magento Orders
        /// </summary>
        /// <param name="_daysto_ordersync">Days to Sync</param>
        /// <param name="_page_size">Page Size</param>
        /// <param name="_current_page">Current Page</param>
        /// <returns>Magento Orders</returns>
        public static M2_Orders? GetOrders(int _daysto_ordersync, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_order = global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + ConvertDateToString(DateTime.Now.AddDays(_daysto_ordersync * -1).AddHours(3), false) +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=from" +
                    "&searchCriteria[filter_groups][1][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][1][filters][0][value]=" + ConvertDateToString(DateTime.Now.AddHours(3), false) +
                    "&searchCriteria[filter_groups][1][filters][0][condition_type]=to" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_order, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>(m2_json);
                    if (m2_orders == null) { PrintConsole("Magento Orders Load Failed. Exiting."); return null; }
                    return m2_orders;
                }
                else {
                    PrintConsole("Magento Orders Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Magento Orders by Statuses
        /// </summary>
        /// <param name="_daysto_ordersync">Days to Sync</param>
        /// <param name="_statuses">Order Statuses</param>
        /// <param name="_page_size">Page Size</param>
        /// <param name="_current_page">Current Page</param>
        /// <returns>Magento Orders</returns>
        public static M2_Orders? GetOrders(int _daysto_ordersync, string[] _statuses, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_order = global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + ConvertDateToString(DateTime.Now.AddDays(_daysto_ordersync * -1).AddHours(3), false) +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=from" +
                    "&searchCriteria[filter_groups][1][filters][0][field]=created_at" +
                    "&searchCriteria[filter_groups][1][filters][0][value]=" + ConvertDateToString(DateTime.Now.AddHours(3), false) +
                    "&searchCriteria[filter_groups][1][filters][0][condition_type]=to" +
                    "&searchCriteria[filter_groups][2][filters][0][field]=status" +
                    "&searchCriteria[filter_groups][2][filters][0][value]=" + string.Join(",", _statuses) +
                    "&searchCriteria[filter_groups][2][filters][0][condition_type]=in" +
                    "&searchCriteria[currentPage]=" + _current_page.ToString() +
                    "&searchCriteria[pageSize]=" + _page_size.ToString();
                var m2_json = executioner.Execute(url_order, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>(m2_json);
                    if (m2_orders == null) { PrintConsole("Magento Orders Load Failed. Exiting."); return null; }
                    PrintConsole(m2_orders.items.Count().ToString() + " Magento Orders Loaded.", false);
                    return m2_orders;
                }
                else {
                    PrintConsole("Magento Orders Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        public static M2_Order? GetOrderByIncrementID(string _increment_id) {
            using (Executioner executioner = new Executioner()) {
                string url_order = global.magento.base_url + "rest/all/V1/orders?" +
                    "searchCriteria[filter_groups][0][filters][0][field]=increment_id" +
                    "&searchCriteria[filter_groups][0][filters][0][value]=" + _increment_id +
                    "&searchCriteria[filter_groups][0][filters][0][condition_type]=eq";
                var m2_json = executioner.Execute(url_order, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Orders>(m2_json);
                    if (m2_orders != null) {
                        var m2_order = m2_orders.items.Length > 0 ? m2_orders.items[0] : null;
                        if (m2_order == null) { PrintConsole(_increment_id + " Magento Order Load Failed. Exiting."); return null; }
                        return m2_order;
                    }
                    else
                        return null;
                }
                else {
                    PrintConsole(_increment_id + "Magento Order Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Magento Customer by Email
        /// </summary>
        /// <param name="_email">Magento Customer Email</param>
        /// <param name="_page_size">Page Size</param>
        /// <param name="_current_page">Current Page</param>
        /// <returns>Magento Customer</returns>
        public static M2_Customer? GetCustomer(string _email, int _page_size = 99999, int _current_page = 1) {
            using (Executioner executioner = new Executioner()) {
                string url_customer = global.magento.base_url + "rest/all/V1/customers/search?" +
                "searchCriteria[filter_groups][0][filters][0][field]=email" +
                "&searchCriteria[filter_groups][0][filters][0][value]=" + _email +
                "&searchCriteria[filter_groups][0][filters][0][condition_type]=eq" +
                "&searchCriteria[currentPage]=1" +
                "&searchCriteria[pageSize]=1000";
                var m2_json = executioner.Execute(url_customer, RestSharp.Method.Get, _json: null, global.magento.token);
                if (m2_json != null) {
                    var m2_customers = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Customers>(m2_json);
                    if (m2_customers == null) { PrintConsole("Magento Orders Load Failed. Exiting."); return null; }
                    if (m2_customers.items.Length > 0) {
                        var c = m2_customers.items.Where(x => x.website_id == 2).FirstOrDefault();
                        if (c != null) {
                            PrintConsole(c.email + " Magento Customer Loaded.", false);
                            return c;
                        }
                    }
                    return null;
                }
                else {
                    PrintConsole(_email + " Magento Customer Load Failed. Exiting2.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets Magento Customer Corporate Info
        /// </summary>
        /// <param name="_email">Magento Customer Email</param>
        /// <param name="_is_corporate">Is Corporate</param>
        /// <returns>Dictionary as {attr_code,attr_value}</returns>
        public static Dictionary<string, string>? GetCustomerCorporateInfo(string _email, out bool _is_corporate) {
            var selected_customer = GetCustomer(_email);
            Dictionary<string, string>? _company_infos = null;
            string firma_ismi = string.Empty;
            string firma_vergidairesi = string.Empty;
            string firma_vergino = string.Empty;
            string tc_no = string.Empty;
            _is_corporate = false;
            if (selected_customer != null) {
                if (selected_customer.website_id == 2) {  //firma bilgileri doldur
                    firma_ismi = selected_customer.custom_attributes.Where(x => x.attribute_code == global.magento.customer_firma_ismi_attribute_code).FirstOrDefault()?.value;
                    firma_vergidairesi = selected_customer.custom_attributes.Where(x => x.attribute_code == global.magento.customer_firma_vergidairesi_attribute_code).FirstOrDefault()?.value;
                    firma_vergino = selected_customer.custom_attributes.Where(x => x.attribute_code == global.magento.customer_firma_vergino_attribute_code).FirstOrDefault()?.value;
                    tc_no = selected_customer.custom_attributes.Where(x => x.attribute_code == global.magento.customer_tc_no_attribute_code).FirstOrDefault()?.value;
                    if (!string.IsNullOrWhiteSpace(firma_vergidairesi) && !string.IsNullOrWhiteSpace(firma_vergino) && !string.IsNullOrWhiteSpace(firma_ismi)) {
                        _is_corporate = true;
                        _company_infos = new Dictionary<string, string> {
                            { global.magento.customer_firma_ismi_attribute_code, firma_ismi },
                            { global.magento.customer_firma_vergidairesi_attribute_code, firma_vergidairesi },
                            { global.magento.customer_firma_vergino_attribute_code, firma_vergino },
                            { global.magento.customer_tc_no_attribute_code, tc_no?? "11111111111" }
                        };
                        PrintConsole("Customer loaded from magento " + _email + " Firma Ismi: " + firma_ismi + " Vergi Dairesi: " + firma_vergidairesi + " Vergi No: " + firma_vergino, false);
                        return _company_infos;
                    }
                    else {
                        _company_infos = new Dictionary<string, string> {
                            { global.magento.customer_tc_no_attribute_code, tc_no?? "11111111111" }
                        };
                        PrintConsole("Customer loaded from magento " + _email + " TC No: " + tc_no, false);
                        return _company_infos;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Creates Magento Order Invoice
        /// </summary>
        /// <param name="_order">Magento Order</param>
        /// <returns>Created Order Invoice No</returns>
        public static string? CreateOrderInvoice(Order _order) {
            try {
                var invoice = new M2_InvoiceRequest() {
                    capture = true, appendComment = true, notify = true, comment = new M2_InvoiceRequestInvoiceComment() {
                        comment = global.magento.order_processing_comment, is_visible_on_front = 0, extension_attributes = new()
                    }, items = [], arguments = new() { extension_attributes = new() }
                };
                foreach (var item in _order.order_items) {
                    invoice.items.Add(new M2_InvoiceRequestInvoiceItems() { order_item_id = item.order_item_id, qty = item.qty_ordered, extension_attributes = new() });
                }

                using (Executioner executioner = new Executioner()) {
                    var json_invoice = executioner.Execute(global.magento.base_url + "rest/all/V1/order/" + _order.order_id.ToString() + "/invoice", RestSharp.Method.Post, invoice, global.magento.token);
                    if (json_invoice != null) {
                        PrintConsole(json_invoice + " magento invoice created.");
                        return json_invoice;
                    }
                    else {
                        return null;
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Creates Magento Order Shipment
        /// </summary>
        /// <param name="_order_id">Magento Order ID</param>
        /// <param name="_order_label">Magento Order Label</param>
        /// <param name="_tracking_numbers">Tracking Numbers</param>
        /// <param name="_comment">Comment</param>
        /// <param name="_carrier_code">Carrier Code</param>
        /// <param name="_carrier_title">Carrier Title</param>
        /// <returns>Created Order Shipment No</returns>
        public static string? CreateOrderShipment(int _order_id, string _order_label, string _tracking_numbers, string _comment, string _carrier_code, string _carrier_title) {
            try {
                var ship = new M2_ShippingRequest() {
                    notify = true, appendComment = true, comment = new M2_ShippingRequestShipment_Comment() {
                        comment = _comment, extension_attributes = new M2_ShippingRequestShipment_Extension_Attributes(), is_visible_on_front = 1
                    }, tracks = [ new M2_ShippingRequestShipment_Track(){
                         carrier_code = _carrier_code, title = _carrier_title, extension_attributes = new M2_ShippingRequestShipment_Extension_Attributes(),
                         track_number = _tracking_numbers
                    }]
                };

                using (Executioner executioner = new Executioner()) {
                    var json_order = executioner.Execute(global.magento.base_url + "/index.php/rest/all/V1/order/" + _order_id.ToString() + "/ship", RestSharp.Method.Post, ship, global.magento.token);
                    if (json_order != null) {
                        PrintConsole(_order_label + ":" + _carrier_title + " => " + _tracking_numbers + " magento order shipped.");
                        return json_order;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Changes Magento Order Status
        /// </summary>
        /// <param name="_order">Magento Order</param>
        /// <param name="_status">Magento Order Status</param>
        public static void ChangeOrderStatus(Order _order, string? _status) {
            try {
                if (!string.IsNullOrWhiteSpace(_status)) {
                    var order = new { entity = new { entity_id = _order.order_id, increment_id = _order.order_label, status = _status } };

                    using (Executioner executioner = new Executioner()) {
                        var json_order = executioner.Execute(global.magento.base_url + "rest/all/V1/orders", RestSharp.Method.Post, order, global.magento.token);
                        if (json_order != null) {
                            PrintConsole(_order.order_status + " => " + _status + " magento order status changed!");
                        }
                    }
                }
                else {
                    PrintConsole("Status missing for " + _order.order_label);
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
            }
        }

        public static M2_Product? InsertMagentoProduct(Product _product, int _brand_id, List<int> _category_ids, CurrencyRate _currency_rate) {
            try {
                List<object> category_ids = [];
                if (_category_ids.Count > 0) {
                    foreach (var item in _category_ids) {
                        if (item != 0)
                            category_ids.Add(new { category_id = item, position = 0 });
                    }
                }
                var specialPriceValue = _product.special_price > 0
                    ? Math.Round(
                        _product.special_price *
                        (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) * _currency_rate.rate,
                        2, MidpointRounding.AwayFromZero)
                    : (decimal?)null;

                var customAttributes = new List<object> {
                    new {
                        attribute_code = "barcode",
                        value = _product.barcode
                    },
                    new {
                        attribute_code = "brand",
                        value = _brand_id
                    }
                };

                if (specialPriceValue.HasValue) {
                    customAttributes.Add(new {
                        attribute_code = "special_price",
                        value = specialPriceValue.Value
                    });
                }

                var json = new {
                    product = new {
                        sku = _product.sku,
                        universal_sku = _product.sku,
                        name = _product.name,
                        attribute_set_id = 4,
                        visibility = 4,
                        type_id = "simple",
                        price = Math.Round(
                            _product.price *
                            (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) * _currency_rate.rate,
                            2, MidpointRounding.AwayFromZero),
                        status = _product.extension.is_enabled ? 1 : 2,
                        weight = (float)_product.extension.weight,
                        extension_attributes = new {
                            stock_item = new {
                                is_in_stock = _product.total_qty > 0,
                                qty = _product.total_qty
                            },
                            category_links = category_ids.Count > 0 ? category_ids : null
                        },
                        custom_attributes = customAttributes
                    },
                    saveOptions = true
                };

                using (Executioner executioner = new Executioner()) {
                    var json_product = executioner.Execute(global.magento.base_url + "rest/all/V1/products", RestSharp.Method.Post, json, global.magento.token);
                    if (json_product != null) {
                        var updated = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Product>(json_product);
                        if (updated != null) {
                            PrintConsole("Sku:" + _product.sku + " new product created => [qty=" + _product.total_qty.ToString() + "]");
                            return updated;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        public static int? InsertM2Category(string category_name, int? parent_id = null, int is_active = 1, int position = 999) {
            try {
                // Magento requires a "category" object in the payload
                var cat_json = new {
                    category = new {
                        name = category_name,
                        is_active = is_active,
                        position = position,
                        parent_id = parent_id ?? global.magento.root_category_id, // fallback to root if not provided
                        include_in_menu = true
                    }
                };

                using (Executioner executioner = new Executioner()) {
                    var json_cat = executioner.Execute(
                        global.magento.base_url + "rest/all/V1/categories",
                        RestSharp.Method.Post,
                        cat_json,
                        global.magento.token);

                    if (json_cat != null) {
                        // Magento returns the created category object
                        var m2_category = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Category>(json_cat);
                        if (m2_category != null && m2_category.id > 0) {
                            PrintConsole("Magento Category Inserted: " + m2_category.id + " - " + m2_category.name);
                            return m2_category.id;
                        }
                    }
                }
                PrintConsole("Magento Category Insert failed: " + category_name);
                return null;
            }
            catch (Exception ex) {
                PrintConsole("Magento Category Insert Exception: " + ex.ToString());
                return null;
            }
        }

        public static List<Product>? BULK_UpdateProducts(List<Product> _products, CurrencyRate _currency_rate) {
            try {
                var prices = new { prices = new List<M2_PriceRequest>() { } };
                var special_prices = new { prices = new List<M2_SpecialPriceRequest>() { } };
                var special_prices_delete = new { prices = new List<M2_SpecialPriceDeleteRequest>() { } };
                foreach (var item in _products) {
                    if (item.price > 0) {
                        prices.prices.Add(new M2_PriceRequest() {
                            price = Math.Round(
                                item.price *
                                (item.tax_included ? 1 : (1 + ((decimal)item.tax / 100m))) * _currency_rate.rate
                            , 2, MidpointRounding.AwayFromZero),
                            store_id = 0,
                            sku = item.sku,
                            extension_attributes = []
                        });
                    }
                    if (item.special_price > 0) {
                        special_prices.prices.Add(new M2_SpecialPriceRequest() {
                            price = Math.Round(
                                item.special_price *
                                (item.tax_included ? 1 : (1 + ((decimal)item.tax / 100m))) * _currency_rate.rate
                            , 2, MidpointRounding.AwayFromZero),
                            store_id = 0,
                            sku = item.sku,
                            extension_attributes = [],
                            price_from = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"),
                            price_to = ""
                        });
                    }
                    else {
                        special_prices_delete.prices.Add(new M2_SpecialPriceDeleteRequest() {
                            price = 0,
                            store_id = 0,
                            sku = item.sku,
                            price_from = "",
                            price_to = "",
                            extension_attributes = []
                        });
                    }
                }

                using (Executioner executioner = new Executioner()) {
                    var json_price_bulk = executioner.Execute(global.magento.base_url + "/index.php/rest/all/V1/products/base-prices", RestSharp.Method.Post, prices, global.magento.token);
                    var json_special_prices_bulk = executioner.Execute(global.magento.base_url + "/index.php/rest/all/V1/products/special-price", RestSharp.Method.Post, special_prices, global.magento.token);
                    var json_special_prices_delete_bulk = executioner.Execute(global.magento.base_url + "/index.php/rest/all/V1/products/special-price-delete", RestSharp.Method.Post, special_prices_delete, global.magento.token);
                }
                PrintConsole("BULK update prices completed.");

                foreach (var item in _products) {
                    if (item.custom_price > 0) {
                        UpdateProductCustomPrice(item, _currency_rate);
                    }
                }
                PrintConsole("Bulk update custom prices completed.");

                PrintConsole("Count:" + _products.Count.ToString());

                return _products;
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates Magento Product Price
        /// </summary>
        /// <param name="_product">Product</param>
        /// <param name="_currency_rate">Currency Rates</param>
        /// <returns>[Error] returns 'false'</returns>
        public static bool UpdateProductPrice(Product _product, CurrencyRate _currency_rate) {
            try {
                var prices = new {
                    prices = new M2_PriceRequest[] {
                        new M2_PriceRequest(){
                            price = Math.Round(
                                _product.price *
                                (_product.tax_included ? 1 : (1 + ( (decimal)_product.tax / 100m))) * _currency_rate.rate
                            , 2, MidpointRounding.AwayFromZero ),
                            store_id = 0,
                            sku = _product.sku,
                            extension_attributes = []
                    } }
                };
                using (Executioner executioner = new Executioner()) {
                    var json_price = executioner.Execute(global.magento.base_url + "index.php/rest/all/V1/products/base-prices", RestSharp.Method.Post, prices, global.magento.token);
                    if (json_price != null) {
                        PrintConsole("Sku:" + _product.sku + " updated => [price=" + _product.price.ToString() + "]");
                        return true;
                    }
                    else {
                        PrintConsole("Sku:" + _product.sku + " updated => [price=" + _product.price.ToString() + "] failed.");
                        return false;
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates Magento Product Special Price
        /// </summary>
        /// <param name="_product">Product</param>
        /// <param name="_currency_rate">Currency Rates</param>
        /// <returns>[Error] returns 'false'</returns>
        public static bool UpdateProductSpecialPrice(Product _product, CurrencyRate _currency_rate) {
            try {
                if (_product.special_price > 0) {
                    var special_prices = new {
                        prices = new M2_SpecialPriceRequest[]{
                            new M2_SpecialPriceRequest(){
                                price = Math.Round(
                                    _product.special_price *
                                    (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) * _currency_rate.rate
                                , 2, MidpointRounding.AwayFromZero ),
                                store_id = 0,
                                sku = _product.sku,
                                price_from = DateTime.Now.AddDays( -2 ).ToString( "yyyy-MM-dd 00:00:00" ),
                                price_to = "",
                                extension_attributes = []
                        } }
                    };
                    using (Executioner executioner = new Executioner()) {
                        var json_special_prices = executioner.Execute(global.magento.base_url + "index.php/rest/all/V1/products/special-price", RestSharp.Method.Post, special_prices, global.magento.token);
                        if (json_special_prices != null) {
                            PrintConsole("Sku:" + _product.sku + " updated => [special_price=" + _product.special_price.ToString() + "]");
                            return true;
                        }
                        else {
                            PrintConsole("Sku:" + _product.sku + " updated => [special_price=" + _product.special_price.ToString() + "] failed.");
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
                            extension_attributes = []
                        } }
                    };
                    using (Executioner executioner = new Executioner()) {
                        var json_special_prices_delete = executioner.Execute(global.magento.base_url + "index.php/rest/all/V1/products/special-price-delete", RestSharp.Method.Post, special_prices_delete, global.magento.token);
                        if (json_special_prices_delete != null) {
                            PrintConsole("Sku:" + _product.sku + " updated => [special_price=" + _product.special_price.ToString() + "] deleted.");
                            return true;
                        }
                        else {
                            PrintConsole("Sku:" + _product.sku + " updated => [special_price=" + _product.special_price.ToString() + "] delete failed.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates Magento Product Custom Price
        /// </summary>
        /// <param name="_product">Product</param>
        /// <param name="_currency_rate">Currency Rates</param>
        /// <returns>[Error] returns 'false'</returns>
        public static bool? UpdateProductCustomPrice(Product _product, CurrencyRate _currency_rate) {
            int? product_id = QP_MySQLHelper.GetM2ProductId(_product.sku);
            if (product_id.HasValue) {
                if (_product.custom_price > 0) {
                }
                else {
                    if (_product.special_price > 0)
                        _product.custom_price = _product.special_price;
                    else
                        _product.custom_price = _product.price;
                }
                bool? temp = QP_MySQLHelper.QP_UpdateCustomBundlePC(product_id.Value.ToString(), Math.Round(
                                    _product.custom_price *
                                    (_product.tax_included ? 1 : (1 + ((decimal)_product.tax / 100m))) * _currency_rate.rate
                                , 2, MidpointRounding.AwayFromZero));
                if (temp.HasValue && temp.Value) {
                    PrintConsole("Sku:" + _product.sku + " updated => [custom_price=" + _product.custom_price.ToString() + "] (true)");
                    return true;
                }
                else if (temp.HasValue && !temp.Value) {
                    PrintConsole("Sku:" + _product.sku + " updated => [custom_price=" + _product.custom_price.ToString() + "] failed. (false)");
                    return false;
                }
                else {
                    PrintConsole("Sku:" + _product.sku + " updated => [custom_price=" + _product.custom_price.ToString() + "] failed (null).");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates Magento Product Attribute
        /// </summary>
        /// <param name="_sku">Product SKU</param>
        /// <param name="_attribute_code">Attribute Code</param>
        /// <param name="_attribute_value">Attribute Value</param>
        /// <returns>[Error] returns 'false'</returns>
        public static bool UpdateProductAttribute(string _sku, string _attribute_code, string? _attribute_value) {
            try {
                var product = new {
                    product = new {
                        custom_attributes = new M2_CustomAttributes[] { new M2_CustomAttributes(){
                            attribute_code = _attribute_code,
                            value = !string.IsNullOrWhiteSpace( _attribute_value ) ? _attribute_value.Trim() : string.Empty

                        } }
                    }
                };

                using (Executioner executioner = new Executioner()) {
                    var json_product = executioner.Execute(global.magento.base_url + "index.php/rest/all/V1/products/" + ConvertFriendly(_sku), RestSharp.Method.Put, product, global.magento.token);
                    if (json_product != null) {
                        var updated_product = Newtonsoft.Json.JsonConvert.DeserializeObject<M2_Product>(json_product);
                        if (updated_product != null) {
                            PrintConsole("Sku:" + _sku + " updated => [" + _attribute_code + "=" + _attribute_value + "]");
                            return true;
                        }
                    }
                    PrintConsole("Sku:" + _sku + " updated => [" + _attribute_code + "=" + _attribute_value + "] failed.");
                    return false;
                }
            }
            catch (Exception ex) {
                PrintConsole("Sku:" + _sku + " updated => [" + _attribute_code + "=" + _attribute_value + "] failed." + Environment.NewLine + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Inserts Magento Product Attribute Option
        /// </summary>
        /// <param name="_attribute_code">Attribute Code</param>
        /// <param name="_attribute_value">Attribute Value</param>
        /// <returns>Updated Option ID</returns>
        public static string? InsertAttributeOption(string _attribute_code, string? _attribute_value) {
            try {
                if (string.IsNullOrWhiteSpace(_attribute_value)) return null;
                var attribute_option = new {
                    option = new M2_AttributeOption() {
                        value = _attribute_value,
                        store_labels = [new M2_StoreLabel() { store_id = 0, label = _attribute_value }],
                        is_default = false,
                        sort_order = 0,
                        label = _attribute_value
                    }
                };

                using (Executioner executioner = new Executioner()) {
                    var json_product = executioner.Execute(global.magento.base_url + "rest/all/V1/products/attributes/" + ConvertFriendly(_attribute_code) + "/options", RestSharp.Method.Post, attribute_option, global.magento.token);
                    if (json_product != null) {
                        var updated_id = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(json_product);
                        if (updated_id != null) {
                            PrintConsole("Attribute Code:" + _attribute_code + " option inserted => [" + _attribute_code + "=" + _attribute_value + "]");
                            return updated_id;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates Magento Product Qty
        /// </summary>
        /// <param name="_sku">Product SKU</param>
        /// <param name="_qty">Product Qty</param>
        /// <returns>[Error] returns 'false'</returns>
        public static bool UpdateProductQty(string _sku, int _qty) {
            try {
                var stock_item = new {
                    stockItem = new M2_StockItemRequest() {
                        qty = _qty,
                        is_in_stock = (_qty > 0) ? true : false
                    }
                };

                using (Executioner executioner = new Executioner()) {
                    var json_qty = executioner.Execute(global.magento.base_url + "index.php/rest/all/V1/products/" + ConvertFriendly(_sku) + "/stockItems/1", RestSharp.Method.Put, stock_item, global.magento.token);
                    if (json_qty != null) {
                        PrintConsole("Sku:" + _sku + " updated => [qty=" + _qty.ToString() + "]");
                        return true;
                    }
                    else {
                        PrintConsole("Sku:" + _sku + " updated => [qty=" + _qty.ToString() + "] failed.");
                        return false;
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole("Sku:" + _sku + " updated => [qty=" + _qty.ToString() + "] failed." + Environment.NewLine + ex.ToString());
                return false;
            }
        }

        public static int GetM2BrandId(string thread_id, DbHelper db_helper, Customer customer, ref Dictionary<int, string>? live_m2_brands, Brand? _brand) {
            if (_brand == null || string.IsNullOrWhiteSpace(_brand.brand_name))
                return 0;

            // Try to find the brand ID in the current dictionary
            int m2_brand_id = 0;
            if (live_m2_brands != null) {
                var found = live_m2_brands.FirstOrDefault(x => x.Value.Equals(_brand.brand_name, StringComparison.OrdinalIgnoreCase));
                m2_brand_id = found.Key;
                if (m2_brand_id > 0)
                    return m2_brand_id;
            }

            // If not found, insert the brand as a new attribute option in Magento
            var insertedOptionIdStr = InsertAttributeOption("brand", _brand.brand_name);
            if (int.TryParse(insertedOptionIdStr, out m2_brand_id) && m2_brand_id > 0) {
                // Update the local dictionary so future lookups succeed
                live_m2_brands ??= [];
                live_m2_brands[m2_brand_id] = _brand.brand_name;

                PrintConsole("Brand:" + _brand.brand_name + " updated. (" + Constants.MAGENTO2 + ")");
                db_helper.LogToServer(thread_id, "brand_inserted", global.settings.company_name + " Brand:" + _brand.brand_name + " (" + Constants.MAGENTO2 + ")", customer.customer_id, "product");
            }
            else {
                PrintConsole("Brand:" + _brand.brand_name + " insert failed. (" + Constants.MAGENTO2 + ")");
                db_helper.LogToServer(thread_id, "brand_insert_error", global.settings.company_name + " Brand:" + _brand.brand_name + " (" + Constants.MAGENTO2 + ")", customer.customer_id, "product");
                m2_brand_id = 0;
            }

            return m2_brand_id;
        }

        public static List<int> GetM2CategoryIds(string thread_id, DbHelper db_helper, Customer customer, ref List<M2_Category>? live_m2_categories,
            List<CategoryTarget> category_target_relation, List<Category> _categories) {
            List<int> magento_category_ids = [];
            foreach (var citem in _categories) {
                // Skip root category if needed (adjust as per your logic)
                if (citem.id == global.product.customer_root_category_id) {
                    magento_category_ids.Add(global.magento.root_category_id);
                    continue;
                }
                if (citem.id == 0) {
                    citem.id = db_helper.InsertCategory(customer.customer_id, citem)?.id ?? 0;
                }

                var category_relation = category_target_relation.FirstOrDefault(x => x.category_id == citem.id);
                if (category_relation != null) {
                    magento_category_ids.Add(category_relation.target_id);
                }
                else {
                    int? magento_category_id = live_m2_categories?.FirstOrDefault(x => x.name == citem.category_name)?.id;
                    if (magento_category_id.HasValue) {
                        db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                            customer_id = customer.customer_id,
                            category_id = citem.id,
                            target_id = magento_category_id.Value,
                            target_name = Constants.MAGENTO2
                        });
                        magento_category_ids.Add(magento_category_id.Value);
                    }
                    else {
                        if (!string.IsNullOrWhiteSpace(citem.category_name)) {
                            magento_category_id = InsertM2Category(citem.category_name, citem.parent_id, citem.is_active ? 1 : 0, 0);
                            if (magento_category_id.HasValue && magento_category_id.Value > 0) {
                                db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                                    customer_id = customer.customer_id,
                                    category_id = citem.id,
                                    target_id = magento_category_id.Value,
                                    target_name = Constants.MAGENTO2
                                });
                                magento_category_ids.Add(magento_category_id.Value);
                                PrintConsole("Category:" + citem.category_name + " inserted and sync to Id:" + magento_category_id.Value.ToString() + " (" + Constants.MAGENTO2 + ")");
                                db_helper.LogToServer(thread_id, "category_inserted", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.MAGENTO2 + ")", customer.customer_id, "product");

                            }
                            else {
                                PrintConsole("Category:" + citem.category_name + " insert failed. (" + Constants.MAGENTO2 + ")");
                                db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.MAGENTO2 + ")", customer.customer_id, "product");
                            }
                        }
                        else {
                            PrintConsole("Category name is empty for category ID: " + citem.id + ". Skipping insert.");
                            db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category ID:" + citem.id + " has no name. (" + Constants.MAGENTO2 + ")", customer.customer_id, "product");
                        }
                    }
                }
            }
            return magento_category_ids;
        }

        #endregion
    }
}
