using Merchanter.Classes;

namespace Merchanter {
    public static partial class Helper {
        public static int UpdateIdeaProduct(int _id, decimal _price1, float _qty) {
            var pro_json = new {
                price1 = _price1,
                marketPriceDetail = _price1.ToString().Replace(".", string.Empty).Replace(",", "."),
                stockAmount = _qty,
            };

            using Executioner executioner = new();
            var json = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/products/" + _id.ToString(), RestSharp.Method.Put, pro_json, Helper.global.ideasoft.access_token);
            if (json != null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                return idea_product != null ? idea_product.id : 0;
            }
            return 0;
        }

        public static int InsertIdeaProduct(Product _product, int? _brand_id, List<int> _category_ids) {
            List<object> category_ids = [];
            if (_category_ids.Count > 0) {
                foreach (var item in _category_ids) {
                    if (item != 0)
                        category_ids.Add(new { id = item });
                }
            }
            var pro_json = new {
                status = 0,
                sku = _product.sku,
                name = _product.name,
                barcode = _product.barcode,
                price1 = _product.price,
                taxIncluded = _product.tax_included ? 1 : 0,
                tax = _product.tax,
                discountType = 1,
                discount = 0,
                currency = new { id = 3 }, //ID=3 is for Turkish Lira
                marketPriceDetail = _product.price.ToString().Replace(".", string.Empty).Replace(",", "."),
                stockAmount = _product.total_qty,
                categoryShowcaseStatus = 0,
                stockTypeLabel = "Piece",
                hasGift = 0,
                customShippingDisabled = 0,
                brand = _brand_id.HasValue ? _brand_id > 0 ? new { id = _brand_id } : null : null,
                detail = new {
                    sku = _product.sku,
                    details = _product.attributes?.FirstOrDefault(x => x.attribute.attribute_name == "description")?.value ?? ""
                },
                categories = category_ids.Count > 0 ? category_ids : null
            };

            using Executioner executioner = new();
            var json = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/products", RestSharp.Method.Post, pro_json, Helper.global.ideasoft.access_token);
            if (json != null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                return idea_product != null ? idea_product.id : 0;
            }
            return 0;
        }

        public static int InsertIdeaCategory(string _name, int? _parent_id) {
            var cat_json = new {
                name = _name,
                status = 1,
                sortOrder = 999,
                parent = _parent_id.HasValue ? _parent_id > 0 ? new { id = _parent_id } : null : null
            };
            using Executioner executioner = new();
            var json_cat = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/categories", RestSharp.Method.Post, cat_json, Helper.global.ideasoft.access_token);
            if (json_cat != null) {
                var idea_category = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Category>(json_cat);
                return idea_category != null ? idea_category.id : 0;
            }
            return 0;
        }

        public static int InsertIdeaBrand(string _name) {
            var brand_json = new {
                name = _name,
                status = 1,
                sortOrder = 999
            };
            using Executioner executioner = new();
            var json_brand = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/brands", RestSharp.Method.Post, brand_json, Helper.global.ideasoft.access_token);
            if (json_brand != null) {
                var idea_brand = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Brand>(json_brand);
                return idea_brand != null ? idea_brand.id : 0;
            }
            return 0;
        }

        public static IDEA_Product? GetIdeaProduct(string _sku, int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/products?s=" + _sku + "&limit=" + _limit.ToString() + "&page=" + page.ToString(), RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Product>>(json);
                if (temp_products != null) {
                    if (temp_products.Count == _limit) {
                        products.AddRange(temp_products);
                        page++;
                        goto QUERY;
                    }
                    else {
                        products.AddRange(temp_products);
                    }
                }
            }

            return products.FirstOrDefault(x => x.sku == _sku);
        }

        public static List<IDEA_Product>? GetIdeaProducts(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/products?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Product>>(json);
                if (temp_products != null) {
                    if (temp_products.Count == _limit) {
                        products.AddRange(temp_products);
                        page++;
                        goto QUERY;
                    }
                    else {
                        products.AddRange(temp_products);
                        return products;
                    }
                }
            }
            return null;
        }

        public static List<IDEA_Category>? GetIdeaCategories(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Category> categories = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/categories?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Category>>(json);
                if (temp_categories != null) {
                    if (temp_categories.Count == _limit) {
                        categories.AddRange(temp_categories);
                        page++;
                        goto QUERY;
                    }
                    else {
                        categories.AddRange(temp_categories);
                        return categories;
                    }
                }
            }
            return null;
        }

        public static List<IDEA_Brand>? GetIdeaBrands(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Brand> brands = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/brands?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_brands = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Brand>>(json);
                if (temp_brands != null) {
                    if (temp_brands.Count == _limit) {
                        brands.AddRange(temp_brands);
                        page++;
                        goto QUERY;
                    }
                    else {
                        brands.AddRange(temp_brands);
                        return brands;
                    }
                }
            }
            return null;
        }

        public static List<IDEA_Order>? GetIdeaOrders(int _daysto_ordersync, int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Order> orders = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/orders?limit=" + _limit.ToString() + "&page=" + page.ToString() + "&startCreatedAt=" + DateTime.Now.AddDays(_daysto_ordersync * -1).ToString("yyyy-MM-dd") + "&endCreatedAt=" + DateTime.Now.ToString("yyyy-MM-dd"),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Order>>(json);
                if (temp_orders != null) {
                    if (temp_orders.Count == _limit) {
                        orders.AddRange(temp_orders);
                        page++;
                        goto QUERY;
                    }
                    else {
                        orders.AddRange(temp_orders);
                        return orders;
                    }
                }
            }
            return null;
        }
    }
}
