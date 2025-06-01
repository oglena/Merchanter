using Merchanter.Classes;
using ZstdSharp.Unsafe;
using Product = Merchanter.Classes.Product;

namespace Merchanter {
    public static partial class Helper {
        public static int UpdateIdeaProduct(int _id, Product _product, int? _brand_id, List<int> _category_ids) {
            List<dynamic> category_ids = [];
            if (_category_ids.Count > 0) {
                foreach (var item in _category_ids) {
                    if (item != 0)
                        category_ids.Add(new { id = item });
                }
            }
            var pro_json = new {
                id = _id,
                name = _product.name,
                barcode = _product.barcode,
                price1 = _product.price,
                discountType = _product.special_price > 0 ? 0 : 1,
                discount = _product.special_price > 0 ? _product.special_price : 0,
                taxIncluded = _product.tax_included ? 1 : 0,
                tax = _product.tax,
                marketPriceDetail = _product.price.ToString().Replace(".", string.Empty).Replace(",", "."),
                stockAmount = _product.total_qty,
                volumetricWeight = _product.extension.volume,
                customShippingDisabled = 1,
                brand = _brand_id.HasValue ? _brand_id > 0 ? new { id = _brand_id } : null : null,
                status = _product.extension.is_enabled == true ? 1 : 0,
                detail = new {
                    sku = _product.sku,
                    details = Base64ToString(_product.extension.description ?? "")
                },
                categories = category_ids.Count > 0 ? category_ids : null,
                images = _product.images != null && _product.images.Count > 0 ? new List<dynamic>() : null
            };

            if (_product.images != null && _product.images.Count > 0) {
                foreach (var image_item in _product.images) {
                    string? base64image = GetImageAsBase64("""C:\MerchanterServer\ankaraerp""" + @"\Images\" + global.customer.user_name + @"\" + image_item.sku, image_item.image_name);
                    if (!string.IsNullOrWhiteSpace(base64image)) {
                        pro_json?.images?.Add(new {
                            filename = image_item.image_name?.Split(".")[0], extension = image_item.image_name?.Split(".")[1].ToLower(),
                            attachment = "data:image/jpeg;base64," + base64image
                        });
                    }
                    else {
                        PrintConsole("Image not found: " + image_item.image_name);
                    }
                }
            }

            using Executioner executioner = new();
            var json = executioner.Execute(global.ideasoft.store_url + "/admin-api/products/" + _id.ToString(),
                RestSharp.Method.Put, pro_json, global.ideasoft.access_token);
            if (json != null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                //PrintConsole("Product Updated: " + idea_product?.id.ToString() + " - " + idea_product?.name);
                return idea_product != null ? idea_product.id : 0;
            }
            return 0;
        }

        public static List<int> GetIdeaCategoryIds(string thread_id, DbHelper db_helper, Customer customer, ref List<IDEA_Category>? live_idea_categories,
            List<CategoryTarget> category_target_relation, List<Category> _categories) {
            List<int> idea_category_ids = [];
            foreach (var citem in _categories) {
                if (citem.id == global.product.customer_root_category_id) continue;
                if (citem.id == 0) {
                    citem.id = db_helper.InsertCategory(customer.customer_id, citem)?.id ?? 0;
                }

                var category_relation = category_target_relation.FirstOrDefault(x => x.category_id == citem.id);
                if (category_relation != null) {
                    idea_category_ids.Add(category_relation.target_id);
                }
                else {
                    int? idea_category_id = live_idea_categories?.FirstOrDefault(x => x.name == citem.category_name)?.id;
                    if (idea_category_id.HasValue) {
                        db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                            customer_id = customer.customer_id,
                            category_id = citem.id,
                            target_id = idea_category_id.Value,
                            target_name = Constants.IDEASOFT
                        });
                        idea_category_ids.Add(idea_category_id.Value);
                    }
                    else {
                        if (!string.IsNullOrWhiteSpace(citem.category_name)) {
                            idea_category_id = InsertIdeaCategory(citem.category_name, category_target_relation.FirstOrDefault(x => x.category_id == citem.parent_id)?.target_id);
                            if (idea_category_id.HasValue && idea_category_id > 0) {
                                live_idea_categories?.Add(new IDEA_Category() { id = idea_category_id.Value, name = citem.category_name });
                                if (idea_category_id.HasValue && idea_category_id > 0) {
                                    db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                                        customer_id = customer.customer_id,
                                        category_id = citem.id,
                                        target_id = idea_category_id.Value,
                                        target_name = Constants.IDEASOFT
                                    });
                                    idea_category_ids.Add(idea_category_id.Value);
                                    PrintConsole("Category:" + citem.category_name + " inserted and sync to Id:" + idea_category_id.Value.ToString() + " (" + Constants.IDEASOFT + ")");
                                    db_helper.LogToServer(thread_id, "category_inserted", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product");
                                }
                                else {
                                    PrintConsole("Category:" + citem.category_name + " insert failed." + " (" + Constants.IDEASOFT + ")");
                                    db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product");
                                }
                            }
                        }
                        else {
                            PrintConsole("Category name is empty for category ID: " + citem.id + ". Skipping insert.");
                            db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category ID:" + citem.id + " has no name. (" + Constants.IDEASOFT + ")", customer.customer_id, "product");
                        }
                    }
                }
            }
            return idea_category_ids;
        }

        public static int GetIdeaBrandId(string thread_id, DbHelper db_helper, Customer customer, ref List<IDEA_Brand>? live_idea_brands, Brand? _brand) {
            var idea_brand_id = live_idea_brands?.FirstOrDefault(x => x.name == _brand?.brand_name)?.id;
            if (idea_brand_id == null && _brand != null && !string.IsNullOrWhiteSpace(_brand.brand_name)) {
                idea_brand_id = InsertIdeaBrand(_brand.brand_name);
                if (idea_brand_id > 0) {
                    live_idea_brands?.Add(new IDEA_Brand() { id = idea_brand_id.Value, name = _brand.brand_name });
                    PrintConsole("Brand:" + _brand.brand_name + " updated." + " (" + Constants.IDEASOFT + ")");
                    db_helper.LogToServer(thread_id, "brand_inserted", global.settings.company_name + " Brand:" + _brand.brand_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product");
                }
                else {
                    PrintConsole("Brand:" + _brand.brand_name + " insert failed." + " (" + Constants.IDEASOFT + ")");
                    db_helper.LogToServer(thread_id, "brand_insert_error", global.settings.company_name + " Brand:" + _brand.brand_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product");
                }
            }
            return idea_brand_id.HasValue ? idea_brand_id.Value : 0;
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
                status = _product.extension.is_enabled == true ? 1 : 0,
                sku = _product.sku,
                name = _product.name,
                barcode = _product.barcode,
                price1 = _product.price,
                taxIncluded = _product.tax_included ? 1 : 0,
                tax = _product.tax,
                discountType = _product.special_price > 0 ? 0 : 1,
                discount = _product.special_price > 0 ? _product.special_price : 0,
                currency = new { id = _product.currency == Currency.GetCurrency("TL") ? 3 : 3 }, //ID=3 is for Turkish Lira TODO:This should be dynamic
                marketPriceDetail = _product.price.ToString().Replace(".", string.Empty).Replace(",", "."),
                stockAmount = _product.total_qty,
                volumetricWeight = _product.extension.volume,
                customShippingDisabled = 1,
                categoryShowcaseStatus = 0,
                stockTypeLabel = "Piece",
                hasGift = 0,
                brand = _brand_id.HasValue ? _brand_id > 0 ? new { id = _brand_id } : null : null,
                detail = new {
                    sku = _product.sku,
                    details = Base64ToString(_product.extension.description ?? "")
                },
                categories = category_ids.Count > 0 ? category_ids : null,
                images = _product.images != null && _product.images.Count > 0 ? new List<dynamic>() : null
            };

            if (_product.images != null && _product.images.Count > 0) {
                foreach (var image_item in _product.images) {
                    string? base64image = GetImageAsBase64(Environment.CurrentDirectory + @"\" + global.customer.user_name + @"\Images\" + image_item.sku, image_item.image_name);
                    if (!string.IsNullOrWhiteSpace(base64image)) {
                        pro_json?.images?.Add(new {
                            filename = image_item.image_name?.Split(".")[0], extension = image_item.image_name?.Split(".")[1],
                            attachment = "data:image/jpeg;base64," + base64image
                        });
                    }
                    else {
                        PrintConsole("Image not found: " + image_item.image_name);
                    }
                }
            }

            using Executioner executioner = new();
            var json = executioner.Execute(global.ideasoft.store_url + "/admin-api/products", RestSharp.Method.Post, pro_json, global.ideasoft.access_token);
            if (json != null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                //PrintConsole("Product Inserted: " + idea_product?.id.ToString() + " - " + idea_product?.name);
                return idea_product != null ? idea_product.id : 0;
            }
            return 0;
        }

        public static int? InsertIdeaCategory(string _name, int? _parent_id) {
            var cat_json = new {
                name = _name,
                status = 1,
                sortOrder = 999,
                parent = _parent_id.HasValue ? _parent_id > 0 ? new { id = _parent_id } : null : null
            };
            using Executioner executioner = new();
            var json_cat = executioner.Execute(global.ideasoft.store_url + "/admin-api/categories", RestSharp.Method.Post, cat_json, global.ideasoft.access_token);
            if (json_cat != null) {
                var idea_category = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Category>(json_cat);
                PrintConsole("Category Inserted: " + idea_category?.id.ToString() + " - " + idea_category?.name);
                return idea_category?.id;
            }
            return null;
        }

        public static int InsertIdeaBrand(string _name) {
            var brand_json = new {
                name = _name,
                status = 1,
                sortOrder = 999
            };
            using Executioner executioner = new();
            var json_brand = executioner.Execute(global.ideasoft.store_url + "/admin-api/brands", RestSharp.Method.Post, brand_json, global.ideasoft.access_token);
            if (json_brand != null) {
                var idea_brand = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Brand>(json_brand);
                PrintConsole("Brand Inserted: " + idea_brand?.id.ToString() + " - " + idea_brand?.name);
                return idea_brand != null ? idea_brand.id : 0;
            }
            return 0;
        }

        public static IDEA_Product? GetIdeaProduct(string _sku, int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var encodedSku = Uri.EscapeDataString(_sku);
            var json = executioner.Execute(global.ideasoft.store_url + "/admin-api/products?s=" + encodedSku + "&limit=" + _limit.ToString() + "&page=" + page.ToString(), RestSharp.Method.Get, null, global.ideasoft.access_token);
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
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/products?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
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
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/categories?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
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
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/brands?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
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
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/orders?limit=" + _limit.ToString() + "&page=" + page.ToString() + "&startCreatedAt=" + DateTime.Now.AddDays(_daysto_ordersync * -1).ToString("yyyy-MM-dd") + "&endCreatedAt=" + DateTime.Now.ToString("yyyy-MM-dd"),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
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
