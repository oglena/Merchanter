using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.Responses;
using Product = Merchanter.Classes.Product;

namespace Merchanter {
    /// <summary>
    /// Provides utility methods for interacting with the Ideasoft API, including token management, product updates,
    /// category synchronization, and other operations.
    /// </summary>
    /// <remarks>This static class contains methods to facilitate communication with the Ideasoft platform,
    /// such as refreshing access tokens, managing products, categories, brands, and orders. It is designed to simplify
    /// integration with the Ideasoft API by abstracting common tasks.</remarks>
    public static partial class Helper {
        /// <summary>
        /// Attempts to refresh the access token for the provided Ideasoft settings.
        /// </summary>
        /// <remarks>This method checks whether the current token is expired (older than 11 hours) and, if
        /// so, attempts to refresh it using the provided refresh token. If the refresh is successful, the <paramref
        /// name="_settings"/> object is updated with the new access token, refresh token, and update date. If the
        /// refresh fails, the method logs the error and returns <see langword="null"/>.</remarks>
        /// <param name="_settings">A reference to the <see cref="SettingsIdeasoft"/> object containing the current token and configuration.
        /// This parameter cannot be <see langword="null"/> and must have a valid <c>refresh_token</c>.</param>
        /// <returns><see langword="true"/> if the token was successfully refreshed;  <see langword="false"/> if the token is
        /// still valid and does not need to be refreshed;  <see langword="null"/> if the operation failed due to
        /// invalid input or an error during the refresh process.</returns>
        public static bool? RefreshIdeaToken(ref SettingsIdeasoft _settings) {
            if (_settings == null) {
                PrintConsole("SettingsIdeasoft is null. Cannot refresh token.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(_settings.refresh_token)) {
                PrintConsole("Refresh token is empty. Cannot refresh token.");
                return null;
            }

            //if update_date is more than 11 hours ago, refresh token
            if (_settings.update_date.HasValue && _settings.update_date.Value.AddHours(11) < DateTime.Now) {
                PrintConsole(Constants.IDEASOFT + " Refresh token is expired. Refreshing token...");
                using Executioner executioner = new();
                var json = executioner.Execute(global.ideasoft.store_url + "/oauth/v2/token" + "?grant_type=refresh_token&client_id=" + _settings.client_id + "&client_secret=" + _settings.client_secret + "&refresh_token=" + _settings.refresh_token,
                    RestSharp.Method.Get, null, null);
                if (json is not null) {
                    var response = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Token>(json);
                    if (response is not null && !string.IsNullOrWhiteSpace(response.access_token)) {
                        if (response.error is not null) {
                            PrintConsole("Error refreshing token: " + response.error + " - " + response.error_description);
                            return null;
                        }
                        _settings.access_token = response.access_token;
                        _settings.refresh_token = response.refresh_token;
                        _settings.update_date = DateTime.Now;

                        PrintConsole("Token refreshed successfully for " + DateTime.Now.AddSeconds(response.expires_in).ToString("dd.MM.yyyy HH:mm:ss"));
                        return true;
                    }
                    else {
                        PrintConsole("Failed to refresh token. Response is null.");
                        return null;
                    }
                }
                else {
                    PrintConsole("Failed to refresh token. JSON response is null.");
                    return null;
                }
            }
            PrintConsole("Token is still valid, no need to refresh.");
            return true;
        }

        /// <summary>
        /// Updates an existing product in the IdeaSoft system with the specified details.
        /// </summary>
        /// <remarks>This method sends a PUT request to the IdeaSoft API to update the product details. 
        /// It constructs a JSON payload based on the provided parameters and handles optional fields such as brand,
        /// categories, and images.</remarks>
        /// <param name="_id">The unique identifier of the product to update.</param>
        /// <param name="_product">The product object containing updated details such as name, price, barcode, and other attributes.</param>
        /// <param name="_brand_id">The identifier of the brand associated with the product.  If <see langword="null"/> or less than or equal to
        /// 0, the brand will not be updated.</param>
        /// <param name="_category_ids">A list of category IDs to associate with the product.  Only non-zero values will be included in the update.
        /// If the list is empty, categories will not be updated.</param>
        /// <returns>The unique identifier of the updated product if the operation is successful; otherwise, 0.</returns>
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
                images = _product.images is not null && _product.images.Count > 0 ? new List<dynamic>() : null
            };

            if (_product.images is not null && _product.images.Count > 0) {
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
            if (json is not null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                //PrintConsole("Product Updated: " + idea_product?.id.ToString() + " - " + idea_product?.name);
                return idea_product is not null ? idea_product.id : 0;
            }
            return 0;
        }

        /// <summary>
        /// Retrieves a list of IDEA category IDs associated with the provided categories, creating new categories and
        /// relationships as needed.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>Skips
        /// processing for the root category and categories with an ID of 0.</item> <item>Creates new categories in the
        /// database if they do not already exist.</item> <item>Maps categories to existing IDEA category IDs using the
        /// provided relationships or creates new IDEA categories if necessary.</item> <item>Logs relevant operations
        /// and errors to the server for tracking purposes.</item> </list></remarks>
        /// <param name="thread_id">The unique identifier for the current thread, used for logging purposes.</param>
        /// <param name="db_helper">An instance of <see cref="DbHelper"/> used to interact with the database for category operations.</param>
        /// <param name="customer">The customer for whom the categories are being processed.</param>
        /// <param name="live_idea_categories">A reference to a list of live IDEA categories. This list may be updated with newly created IDEA categories
        /// during the method's execution.</param>
        /// <param name="category_target_relation">A list of existing category-to-target relationships, used to map categories to their corresponding IDEA
        /// category IDs.</param>
        /// <param name="_categories">The list of categories to process and map to IDEA category IDs.</param>
        /// <returns>A list of IDEA category IDs corresponding to the provided categories. If a category does not already have an
        /// associated IDEA category, it will be created and added to the returned list.</returns>
        public static List<int> GetIdeaCategoryIds(string thread_id, DbHelper db_helper, Customer customer, ref List<IDEA_Category>? live_idea_categories,
            List<CategoryTarget> category_target_relation, List<Category> _categories) {
            List<int> idea_category_ids = [];
            foreach (var citem in _categories) {
                if (citem.id == global.product.customer_root_category_id) continue;
                if (citem.id == 0) {
                    citem.id = db_helper.InsertCategory(customer.customer_id, citem).Result?.id ?? 0;
                }

                var category_relation = category_target_relation.FirstOrDefault(x => x.category_id == citem.id);
                if (category_relation is not null) {
                    idea_category_ids.Add(category_relation.target_id);
                }
                else {
                    int? idea_category_id = live_idea_categories?.FirstOrDefault(x => x.name == citem.category_name)?.id;
                    if (idea_category_id.HasValue) {
                        _ = db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                            customer_id = customer.customer_id,
                            category_id = citem.id,
                            target_id = idea_category_id.Value,
                            target_name = Constants.IDEASOFT
                        }).Result;
                        idea_category_ids.Add(idea_category_id.Value);
                    }
                    else {
                        if (!string.IsNullOrWhiteSpace(citem.category_name)) {
                            idea_category_id = InsertIdeaCategory(citem.category_name, category_target_relation.FirstOrDefault(x => x.category_id == citem.parent_id)?.target_id);
                            if (idea_category_id.HasValue && idea_category_id > 0) {
                                live_idea_categories?.Add(new IDEA_Category() { id = idea_category_id.Value, name = citem.category_name });
                                if (idea_category_id.HasValue && idea_category_id > 0) {
                                    _ = db_helper.InsertCategoryTarget(customer.customer_id, new CategoryTarget() {
                                        customer_id = customer.customer_id,
                                        category_id = citem.id,
                                        target_id = idea_category_id.Value,
                                        target_name = Constants.IDEASOFT
                                    }).Result;
                                    idea_category_ids.Add(idea_category_id.Value);
                                    PrintConsole("Category:" + citem.category_name + " inserted and sync to Id:" + idea_category_id.Value.ToString() + " (" + Constants.IDEASOFT + ")");
                                    _ = db_helper.LogToServer(thread_id, "category_inserted", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product").Result;
                                }
                                else {
                                    PrintConsole("Category:" + citem.category_name + " insert failed." + " (" + Constants.IDEASOFT + ")");
                                    _ = db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category:" + citem.category_name + " (" + Constants.IDEASOFT + ")", customer.customer_id, "product").Result;
                                }
                            }
                        }
                        else {
                            PrintConsole("Category name is empty for category ID: " + citem.id + ". Skipping insert.");
                            _ = db_helper.LogToServer(thread_id, "category_insert_error", global.settings.company_name + " Category ID:" + citem.id + " has no name. (" + Constants.IDEASOFT + ")", customer.customer_id, "product").Result;
                        }
                    }
                }
            }
            return idea_category_ids;
        }

        /// <summary>
        /// Retrieves the ID of an IDEA brand based on the specified brand name. If the brand does not exist, it inserts
        /// the brand into the system and updates the provided list of live IDEA brands.
        /// </summary>
        /// <remarks>If the specified brand does not exist in the provided list of live IDEA brands and
        /// has a valid name, the method attempts to insert the brand into the system. Upon successful insertion, the
        /// new brand is added to the <paramref name="live_idea_brands"/> list, and relevant logs are created.</remarks>
        /// <param name="thread_id">The unique identifier for the current thread, used for logging purposes.</param>
        /// <param name="db_helper">An instance of <see cref="DbHelper"/> used for database operations and logging.</param>
        /// <param name="customer">The customer associated with the operation, used for logging purposes.</param>
        /// <param name="live_idea_brands">A reference to the list of live IDEA brands. This list is updated if a new brand is inserted.</param>
        /// <param name="_brand">The brand to look up or insert. If null or if the brand name is empty, no insertion occurs.</param>
        /// <returns>The ID of the IDEA brand. Returns 0 if the brand could not be found or inserted.</returns>
        public static int GetIdeaBrandId(string thread_id, DbHelper db_helper, Customer customer, ref List<IDEA_Brand>? live_idea_brands, Brand? _brand) {
            var idea_brand_id = live_idea_brands?.FirstOrDefault(x => x.name == _brand?.brand_name)?.id;
            if (idea_brand_id == null && _brand is not null && !string.IsNullOrWhiteSpace(_brand.brand_name)) {
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

        /// <summary>
        /// Inserts a product into the IdeaSoft platform and returns the ID of the created product.
        /// </summary>
        /// <remarks>This method constructs a JSON payload based on the provided product details and sends
        /// it to the IdeaSoft API.  It handles optional fields such as brand, categories, and images, ensuring only
        /// valid data is included in the request.</remarks>
        /// <param name="_product">The product to be inserted, containing details such as name, SKU, price, and other attributes.</param>
        /// <param name="_brand_id">The ID of the brand associated with the product.  If null or less than or equal to 0, the product will not
        /// be associated with a brand.</param>
        /// <param name="_category_ids">A list of category IDs to associate with the product.  Only non-zero IDs will be included. If the list is
        /// empty or contains only invalid IDs, no categories will be associated.</param>
        /// <returns>The ID of the newly created product in the IdeaSoft platform.  Returns 0 if the product could not be
        /// created.</returns>
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
                images = _product.images is not null && _product.images.Count > 0 ? new List<dynamic>() : null
            };

            if (_product.images is not null && _product.images.Count > 0) {
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
            if (json is not null) {
                var idea_product = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Product>(json);
                //PrintConsole("Product Inserted: " + idea_product?.id.ToString() + " - " + idea_product?.name);
                return idea_product is not null ? idea_product.id : 0;
            }
            return 0;
        }

        /// <summary>
        /// Inserts a new idea category into the system.
        /// </summary>
        /// <remarks>This method sends a request to the IdeaSoft API to create a new category. The
        /// category is created with a default status of active and a default sort order of 999. Ensure that the global
        /// configuration for the IdeaSoft store URL and access token is properly set before calling this
        /// method.</remarks>
        /// <param name="_name">The name of the category to be created. Cannot be null or empty.</param>
        /// <param name="_parent_id">The ID of the parent category. If specified and greater than 0, the new category will be created as a
        /// subcategory. Pass <see langword="null"/> or a value less than or equal to 0 to create a top-level category.</param>
        /// <returns>The ID of the newly created category if the operation is successful; otherwise, <see langword="null"/>.</returns>
        public static int? InsertIdeaCategory(string _name, int? _parent_id) {
            var cat_json = new {
                name = _name,
                status = 1,
                sortOrder = 999,
                parent = _parent_id.HasValue ? _parent_id > 0 ? new { id = _parent_id } : null : null
            };
            using Executioner executioner = new();
            var json_cat = executioner.Execute(global.ideasoft.store_url + "/admin-api/categories", RestSharp.Method.Post, cat_json, global.ideasoft.access_token);
            if (json_cat is not null) {
                var idea_category = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Category>(json_cat);
                PrintConsole("Category Inserted: " + idea_category?.id.ToString() + " - " + idea_category?.name);
                return idea_category?.id;
            }
            return null;
        }

        /// <summary>
        /// Inserts a new brand into the IdeaSoft platform.
        /// </summary>
        /// <remarks>This method sends a POST request to the IdeaSoft API to create a new brand with the
        /// specified name. The created brand is assigned a default status of 1 and a sort order of 999.  Ensure that
        /// the global IdeaSoft store URL and access token are properly configured before calling this method.</remarks>
        /// <param name="_name">The name of the brand to be created. This value cannot be null or empty.</param>
        /// <returns>The unique identifier of the newly created brand if the operation is successful; otherwise, 0.</returns>
        public static int InsertIdeaBrand(string _name) {
            var brand_json = new {
                name = _name,
                status = 1,
                sortOrder = 999
            };
            using Executioner executioner = new();
            var json_brand = executioner.Execute(global.ideasoft.store_url + "/admin-api/brands", RestSharp.Method.Post, brand_json, global.ideasoft.access_token);
            if (json_brand is not null) {
                var idea_brand = Newtonsoft.Json.JsonConvert.DeserializeObject<IDEA_Brand>(json_brand);
                PrintConsole("Brand Inserted: " + idea_brand?.id.ToString() + " - " + idea_brand?.name);
                return idea_brand is not null ? idea_brand.id : 0;
            }
            return 0;
        }

        /// <summary>
        /// Retrieves the first product matching the specified SKU from the IDEA product catalog.
        /// </summary>
        /// <remarks>This method performs a paginated search of the IDEA product catalog using the
        /// provided SKU.  If multiple pages of results are required, the method will continue fetching until all pages 
        /// are processed or the product is found. The search is case-sensitive and matches the SKU exactly.</remarks>
        /// <param name="_sku">The SKU of the product to search for. This value cannot be null or empty.</param>
        /// <param name="_limit">The maximum number of products to retrieve per page during the search.  Defaults to 100 if not specified.
        /// Must be a positive integer.</param>
        /// <returns>An <see cref="IDEA_Product"/> object representing the product with the specified SKU,  or <see
        /// langword="null"/> if no matching product is found.</returns>
        public static IDEA_Product? GetIdeaProduct(string _sku, int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var encodedSku = Uri.EscapeDataString(_sku);
            var json = executioner.Execute(global.ideasoft.store_url + "/admin-api/products?s=" + encodedSku + "&limit=" + _limit.ToString() + "&page=" + page.ToString(), RestSharp.Method.Get, null, global.ideasoft.access_token);
            if (json is not null) {
                var temp_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Product>>(json);
                if (temp_products is not null) {
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

        /// <summary>
        /// Retrieves a list of IDEA products from the store's API.
        /// </summary>
        /// <remarks>This method fetches products from the store's API in paginated requests. If the
        /// number of products exceeds the specified limit,  the method will make additional requests to retrieve all
        /// available products. The returned list may contain fewer products than the specified limit  if the total
        /// number of products is less than the limit.</remarks>
        /// <param name="_limit">The maximum number of products to retrieve per API request. Must be a positive integer. The default value is
        /// 100.</param>
        /// <returns>A list of <see cref="IDEA_Product"/> objects representing the products retrieved from the API, or <see
        /// langword="null"/> if the request fails or no products are available.</returns>
        public static List<IDEA_Product>? GetIdeaProducts(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/products?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
            if (json is not null) {
                var temp_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Product>>(json);
                if (temp_products is not null) {
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

        /// <summary>
        /// Retrieves a list of IDEA categories from the store's API.
        /// </summary>
        /// <remarks>This method fetches categories from the store's API in paginated requests. If the
        /// number of  categories exceeds the specified limit, multiple requests are made to retrieve all available 
        /// categories. The method returns <see langword="null"/> if the API response is invalid or deserialization
        /// fails.</remarks>
        /// <param name="_limit">The maximum number of categories to retrieve per API request. Must be a positive integer.  Defaults to 100
        /// if not specified.</param>
        /// <returns>A list of <see cref="IDEA_Category"/> objects representing the categories retrieved from the API,  or <see
        /// langword="null"/> if the request fails or no categories are available.</returns>
        public static List<IDEA_Category>? GetIdeaCategories(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Category> categories = []; int page = 1;
        QUERY:
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/categories?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
            if (json is not null) {
                var temp_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Category>>(json);
                if (temp_categories is not null) {
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

        /// <summary>
        /// Retrieves a list of IDEA_Brand objects from the API, with optional pagination support.
        /// </summary>
        /// <remarks>This method fetches brands from the API in a paginated manner. If the number of
        /// brands retrieved in a single request equals the specified limit,  the method continues fetching subsequent
        /// pages until all brands are retrieved. If the API response is invalid or no brands are found, the method
        /// returns <see langword="null"/>.</remarks>
        /// <param name="_limit">The maximum number of brands to retrieve per API request. Must be a positive integer. Defaults to 100.</param>
        /// <returns>A list of <see cref="IDEA_Brand"/> objects representing the retrieved brands, or <see langword="null"/> if
        /// the API response is invalid or no brands are available.</returns>
        public static List<IDEA_Brand>? GetIdeaBrands(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Brand> brands = []; int page = 1;
        QUERY:
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/brands?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
            if (json is not null) {
                var temp_brands = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Brand>>(json);
                if (temp_brands is not null) {
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

        /// <summary>
        /// Retrieves a list of IDEA orders created within a specified number of days.
        /// </summary>
        /// <remarks>This method uses pagination to retrieve orders in batches of the specified limit. If
        /// the total number of orders exceeds the limit, multiple requests are made to fetch all available orders. The
        /// method returns <see langword="null"/> if the API response is invalid or deserialization fails.</remarks>
        /// <param name="_daysto_ordersync">The number of days in the past to include orders from. For example, a value of 7 retrieves orders created in
        /// the last 7 days.</param>
        /// <param name="_limit">The maximum number of orders to retrieve per page. Defaults to 100.</param>
        /// <returns>A list of <see cref="IDEA_Order"/> objects representing the retrieved orders, or <see langword="null"/> if
        /// no orders are found or an error occurs.</returns>
        public static List<IDEA_Order>? GetIdeaOrders(int _daysto_ordersync, int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Order> orders = []; int page = 1;
        QUERY:
            var json = executioner.Execute(global.ideasoft.store_url +
                "/admin-api/orders?limit=" + _limit.ToString() + "&page=" + page.ToString() + "&startCreatedAt=" + DateTime.Now.AddDays(_daysto_ordersync * -1).ToString("yyyy-MM-dd") + "&endCreatedAt=" + DateTime.Now.ToString("yyyy-MM-dd"),
                RestSharp.Method.Get, null, global.ideasoft.access_token);
            if (json is not null) {
                var temp_orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Order>>(json);
                if (temp_orders is not null) {
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
