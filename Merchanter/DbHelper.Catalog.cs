using Merchanter.Classes;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Attribute = Merchanter.Classes.Attribute;

namespace Merchanter {
    /// <summary>
    /// Provides methods for interacting with the database to manage products, product extensions, sources, prices,
    /// images, attributes, brands, and categories.
    /// </summary>
    /// <remarks>The <see cref="DbHelper"/> class is designed to facilitate database operations related to
    /// product management. It includes methods for retrieving, inserting, updating, and deleting various entities such
    /// as products, extensions, sources, prices, images, attributes, brands, and categories. This class assumes a
    /// properly configured database connection and handles connection state internally.</remarks>
    public partial class DbHelper {
        #region Catalog
        #region Product
        /// <summary>
        /// Gets product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Product?> GetProduct(int _customer_id, int _id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM products WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Product? p = null;
                    if (await dataReader.ReadAsync()) {
                        p = new Product {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                            total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                            name = dataReader["name"].ToString(),
                            barcode = dataReader["barcode"].ToString(),
                            price = decimal.Parse(dataReader["price"].ToString()),
                            special_price = decimal.Parse(dataReader["special_price"].ToString()),
                            custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                            currency = Currency.GetCurrency(dataReader["currency"].ToString()),
                            tax = Convert.ToInt32(dataReader["tax"].ToString()),
                            tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString()))
                        };
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();

                    if (p is not null && !string.IsNullOrWhiteSpace(p.sku)) {
                        var ext = await GetProductExt(_customer_id, p.sku);
                        if (ext is not null) {
                            p.extension = ext;
                        }
                        else {
                            OnError("GetProduct: " + p.sku + " - Product Extension Not Found");
                            return null;
                        }

                        var pas = await GetProductAttributes(_customer_id, p.id);
                        if (pas is not null) {
                            p.attributes = [.. pas];
                        }
                        else {
                            OnError("GetProduct: " + p.sku + " - Product Attributes Not Found");
                            return null;
                        }

                        var ss = await GetProductSources(_customer_id, p.sku);
                        if (ss is not null && ss.Count > 0) {
                            p.sources = [.. ss];
                        }
                        else {
                            OnError("GetProduct: " + p.sku + " - Product Source Not Found");
                            return null;
                        }

                        var images = await GetProductImages(_customer_id, p.sku);
                        if (images is not null) {
                            p.images = [.. images];
                        }
                        else {
                            OnError("GetProduct: " + p.sku + " - Product Images Not Found");
                            return null;
                        }
                    }
                    return p;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets product from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Product?> GetProductBySku(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM products WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Product? p = null;
                    if (await dataReader.ReadAsync()) {
                        p = new Product {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
                            total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
                            name = dataReader["name"].ToString(),
                            barcode = dataReader["barcode"].ToString(),
                            price = decimal.Parse(dataReader["price"].ToString()),
                            special_price = decimal.Parse(dataReader["special_price"].ToString()),
                            custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
                            currency = Currency.GetCurrency(dataReader["currency"].ToString()),
                            tax = Convert.ToInt32(dataReader["tax"].ToString()),
                            tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString()))
                        };
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();

                    if (p is not null && !string.IsNullOrWhiteSpace(p.sku)) {
                        var ext = await GetProductExt(_customer_id, p.sku);
                        if (ext is not null) {
                            p.extension = ext;
                        }
                        else {
                            OnError("GetProductBySku: " + p.sku + " - Product Extension Not Found");
                            return null;
                        }

                        var pas = await GetProductAttributes(_customer_id, p.id);
                        if (pas is not null) {
                            p.attributes = [.. pas];
                        }
                        else {
                            OnError("GetProduct: " + p.sku + " - Product Attributes Not Found");
                            return null;
                        }

                        var ss = await GetProductSources(_customer_id, p.sku);
                        if (ss is not null && ss.Count > 0) {
                            p.sources = [.. ss];
                        }
                        else {
                            OnError("GetProductBySku: " + p.sku + " - Product Source Not Found");
                            return null;
                        }

                        var imgs = await GetProductImages(_customer_id, p.id);
                        if (imgs is not null) {
                            p.images = [.. imgs];
                        }
                        else {
                            OnError("GetProductBySku: " + p.sku + " - Product Images Not Found");
                            return null;
                        }
                    }
                    return p;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brands"> Brands</param>
        /// <param name="_categories"> Categories</param>
        /// <returns>[Error] returns 'null'</returns>
        public List<Product> GetProducts(int _customer_id, out List<Brand> _brands, out List<Category> _categories,
            out List<ProductAttribute> _product_attributes, out List<ProductImage> _product_images, out List<ProductPrice> _product_prices) {
            _brands = []; _categories = []; _product_attributes = []; _product_images = []; List<ProductSource>? _product_other_sources = []; _product_prices = [];
            try {
                if (state != System.Data.ConnectionState.Open) connection.Open();
                string _query = "SELECT * FROM products_with_mainsource WHERE p_customer_id=@customer_id;";
                List<Product> list = [];
                MySqlCommand cmd = new(_query, connection);
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                MySqlDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (dataReader.Read()) {
                    Product p = new Product {
                        id = Convert.ToInt32(dataReader["p_id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["p_customer_id"].ToString()),
                        source_product_id = Convert.ToInt32(dataReader["p_source_product_id"].ToString()),
                        update_date = Convert.ToDateTime(dataReader["p_update_date"].ToString()),
                        sku = dataReader["p_sku"].ToString(),
                        type = (Product.ProductTypes)Convert.ToInt32(dataReader["p_type"].ToString()),
                        total_qty = Convert.ToInt32(dataReader["p_total_qty"].ToString()),
                        name = dataReader["p_name"].ToString(),
                        barcode = dataReader["p_barcode"].ToString(),
                        price = decimal.Parse(dataReader["p_price"].ToString()),
                        special_price = decimal.Parse(dataReader["p_special_price"].ToString()),
                        custom_price = decimal.Parse(dataReader["p_custom_price"].ToString()),
                        currency = Currency.GetCurrency(dataReader["p_currency"].ToString()),
                        tax = Convert.ToInt32(dataReader["p_tax"].ToString()),
                        tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["p_tax_included"].ToString()))
                    };
                    p.extension = new ProductExtension() {
                        id = Convert.ToInt32(dataReader["pe_id"].ToString()),
                        customer_id = p.customer_id,
                        is_enabled = dataReader["pe_is_enabled"].ToString() == "1",
                        sku = p.sku,
                        barcode = p.barcode,
                        brand_id = Convert.ToInt32(dataReader["pe_brand_id"].ToString()),
                        category_ids = dataReader["pe_category_ids"].ToString(),
                        is_xml_enabled = dataReader["pe_is_xml_enabled"].ToString() == "1",
                        xml_sources = dataReader["pe_xml_sources"]?.ToString()?.Split(','),
                        weight = Convert.ToDecimal(dataReader["pe_weight"].ToString()),
                        volume = Convert.ToDecimal(dataReader["pe_volume"].ToString()),
                        description = dataReader["pe_description"].ToString(),
                        update_date = Convert.ToDateTime(dataReader["pe_update_date"].ToString())
                    };
                    p.sources = [new ProductSource(_customer_id,
                        Convert.ToInt32(dataReader["ps_id"].ToString()),
                        dataReader["ps_name"].ToString(),
                        p.sku, p.barcode, Convert.ToInt32(dataReader["ps_qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["ps_is_active"].ToString())),
                        Convert.ToDateTime(dataReader["ps_update_date"].ToString())
                    )];
                    p.target_prices = [];
                    p.attributes = [];
                    p.images = [];
                    list.Add(p);
                }
                dataReader.Close();
                if (state == System.Data.ConnectionState.Open) connection.Close();

                _brands = GetBrands(_customer_id).Result;
                _categories = GetCategories(_customer_id).Result;
                _product_attributes = GetProductAttributes(_customer_id).Result;
                _product_images = GetProductImages(_customer_id).Result;
                _product_prices = GetProductPrices(_customer_id).Result;

                // Attach brands and categories
                for (int i = 0; i < list.Count; i++) {
                    if (list[i].extension.brand_id > 0)
                        list[i].extension.brand = _brands.First(x => x.id == list[i].extension.brand_id);
                    if (!string.IsNullOrWhiteSpace(list[i].extension.category_ids))
                        list[i].extension.categories = [.. _categories.Where(x => list[i].extension.category_ids.Split(",").Contains(x.id.ToString()))];
                }

                // Load other product sources
                var ai = LoadActiveIntegrations(_customer_id).Result;
                if (ai is null || ai.Count == 0) {
                    OnError("GetProducts: Active Integrations Not Found");
                    return [];
                }
                var main_source = ai.FirstOrDefault(x => x.work_status && x.work_type == Work.WorkType.PRODUCT && x.work_direction == Work.WorkDirection.MAIN_SOURCE);
                if (main_source is not null) {
                    _product_other_sources = GetProductOtherSources(_customer_id, main_source.work_name).Result;
                    if (_product_other_sources is not null && _product_other_sources.Count > 0) {
                        foreach (var item in list) {
                            var selected_product_source = _product_other_sources?.FindAll(x => x.sku == item.sku);
                            if (selected_product_source is not null && selected_product_source.Count > 0)
                                item.sources.AddRange(selected_product_source);
                        }
                    }
                }

                // Load product prices
                if (_product_prices is not null && _product_prices.Count > 0) {
                    foreach (var item in list) {
                        item.target_prices = [.. _product_prices.Where(x => x.product_id == item.id)];
                    }
                }

                //Attach product attributes
                if (_product_attributes is not null && _product_attributes.Count > 0) {
                    foreach (var item in list) {
                        item.attributes = [.. _product_attributes.Where(x => x.product_id == item.id)];
                    }
                }

                //Attach product images
                if (_product_images is not null && _product_images.Count > 0) {
                    foreach (var item in list) {
                        item.images = [.. _product_images.Where(x => x.product_id == item.id)];
                    }
                }

                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return [];
            }
        }

        /// <summary>
        /// Gets the products from the database with filters
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Product>?> GetProducts(int _customer_id, ApiFilter _filters) {
            try {
                List<Brand> brands = await GetBrands(_customer_id) ?? [];
                List<Category> categories = await GetCategories(_customer_id) ?? [];
                List<ProductSource>? product_other_sources = [];

                List<Product> list = [];
                string _query = "SELECT * FROM products_with_mainsource WHERE p_customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Product));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        Product p = new Product {
                            id = Convert.ToInt32(dataReader["p_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["p_customer_id"].ToString()),
                            source_product_id = Convert.ToInt32(dataReader["p_source_product_id"].ToString()),
                            update_date = Convert.ToDateTime(dataReader["p_update_date"].ToString()),
                            sku = dataReader["p_sku"].ToString(),
                            type = (Product.ProductTypes)Convert.ToInt32(dataReader["p_type"].ToString()),
                            total_qty = Convert.ToInt32(dataReader["p_total_qty"].ToString()),
                            name = dataReader["p_name"].ToString(),
                            barcode = dataReader["p_barcode"].ToString(),
                            price = decimal.Parse(dataReader["p_price"].ToString()),
                            special_price = decimal.Parse(dataReader["p_special_price"].ToString()),
                            custom_price = decimal.Parse(dataReader["p_custom_price"].ToString()),
                            currency = Currency.GetCurrency(dataReader["p_currency"].ToString()),
                            tax = Convert.ToInt32(dataReader["p_tax"].ToString()),
                            tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["p_tax_included"].ToString()))
                        };
                        p.extension = new ProductExtension() {
                            id = Convert.ToInt32(dataReader["pe_id"].ToString()),
                            customer_id = p.customer_id,
                            is_enabled = dataReader["pe_is_enabled"].ToString() == "1",
                            sku = p.sku,
                            barcode = p.barcode,
                            brand_id = Convert.ToInt32(dataReader["pe_brand_id"].ToString()),
                            category_ids = dataReader["pe_category_ids"].ToString(),
                            is_xml_enabled = dataReader["pe_is_xml_enabled"].ToString() == "1",
                            xml_sources = dataReader["pe_xml_sources"]?.ToString()?.Split(','),
                            weight = Convert.ToDecimal(dataReader["pe_weight"].ToString()),
                            volume = Convert.ToDecimal(dataReader["pe_volume"].ToString()),
                            description = dataReader["pe_description"].ToString(),
                            update_date = Convert.ToDateTime(dataReader["pe_update_date"].ToString())
                        };
                        p.sources = [new ProductSource(_customer_id,
                        Convert.ToInt32(dataReader["ps_id"].ToString()),
                        dataReader["ps_name"].ToString(),
                        p.sku, p.barcode, Convert.ToInt32(dataReader["ps_qty"].ToString()),
                        Convert.ToBoolean(Convert.ToInt32(dataReader["ps_is_active"].ToString())),
                        Convert.ToDateTime(dataReader["ps_update_date"].ToString())

                    )];
                        p.target_prices = [];
                        p.attributes = [];
                        p.images = [];
                        list.Add(p);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();

                    // Attach brands and categories
                    for (int i = 0; i < list.Count; i++) {
                        if (list[i].extension.brand_id > 0)
                            list[i].extension.brand = brands.FirstOrDefault(x => x.id == list[i].extension.brand_id);
                        if (!string.IsNullOrWhiteSpace(list[i].extension.category_ids))
                            list[i].extension.categories = categories?.Where(x => list[i].extension.category_ids.Split(",").Contains(x.id.ToString())).ToList();
                    }

                    // Load other product sources
                    var ai = await LoadActiveIntegrations(_customer_id);
                    if (ai == null || ai.Count == 0) {
                        OnError("GetProducts: Active Integrations Not Found");
                        return [];
                    }
                    var main_source = ai.FirstOrDefault(x => x.work_status && x.work_type == Work.WorkType.PRODUCT && x.work_direction == Work.WorkDirection.MAIN_SOURCE);
                    if (main_source is not null)
                        product_other_sources = await GetProductOtherSources(_customer_id, main_source.work_name);
                    if (product_other_sources is not null && product_other_sources.Count > 0) {
                        foreach (var item in list) {
                            var selected_product_source = product_other_sources?.FindAll(x => x.sku == item.sku);
                            if (selected_product_source is not null && selected_product_source.Count > 0)
                                item.sources.AddRange(selected_product_source);
                        }
                    }

                    // Load product prices
                    var product_prices = await GetProductPrices(_customer_id);
                    if (product_prices is not null && product_prices.Count > 0) {
                        foreach (var item in list) {
                            item.target_prices = [.. product_prices.Where(x => x.product_id == item.id)];
                        }
                    }

                    // Get product attributes
                    var attrs = await GetProductAttributes(_customer_id);
                    if (attrs is not null && attrs.Count > 0) {
                        foreach (var item in list) {
                            item.attributes = [.. attrs.Where(x => x.product_id == item.id)];
                        }
                    }

                    // Get product images
                    var images = await GetProductImages(_customer_id);
                    if (images is not null && images.Count > 0) {
                        foreach (var item in list) {
                            item.images = [.. images.Where(x => x.product_id == item.id)];
                        }
                    }
                    return list;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        public async Task<Product?> InsertProduct(int _customer_id, Product _product) {
            try {
                if (await IsProductExists(_customer_id, _product.sku)) {
                    OnError("InsertProducts: " + _product.sku + " - Product Already Exists");
                    return null;
                }

                ulong inserted_id = 0;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO products (customer_id,source_product_id,sku,type,barcode,total_qty,price,special_price,custom_price,currency,tax,tax_included,name,sources) VALUES (@customer_id,@source_product_id,@sku,@type,@barcode,@total_qty,@price,@special_price,@custom_price,@currency,@tax,@tax_included,@name,@sources);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("source_product_id", _product.source_product_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _product.sku));
                cmd.Parameters.Add(new MySqlParameter("type", _product.type));
                cmd.Parameters.Add(new MySqlParameter("barcode", _product.barcode));
                cmd.Parameters.Add(new MySqlParameter("total_qty", _product.sources.Where(x => x.is_active).Sum(x => x.qty)));
                cmd.Parameters.Add(new MySqlParameter("price", _product.price));
                cmd.Parameters.Add(new MySqlParameter("special_price", _product.special_price));
                cmd.Parameters.Add(new MySqlParameter("custom_price", _product.custom_price));
                cmd.Parameters.Add(new MySqlParameter("currency", _product.currency.code));
                cmd.Parameters.Add(new MySqlParameter("tax", _product.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _product.tax_included));
                cmd.Parameters.Add(new MySqlParameter("name", _product.name));
                cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", _product.sources.Where(x => x.is_active).Select(x => x.name))));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    inserted_id = (ulong)await cmd.ExecuteScalarAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();

                    if (!await InsertProductExt(_customer_id, _product.extension)) {
                        OnError("InsertProducts: " + _product.sku + " - Product Extension Insert Error");
                        return null;
                    }

                    if (!await UpdateProductSources(_customer_id, _product.sources, _product.sku)) {
                        OnError("InsertProducts: " + _product.sku + " - Product Source Insert Error");
                        return null;
                    }

                    if (_product.attributes is not null && _product.attributes.Count > 0) {
                        if (!await UpdateProductAttributes(_customer_id, _product.attributes, (int)inserted_id)) {
                            OnError("InsertProducts: " + _product.sku + " - Product Attributes Insert Error");
                            return null;
                        }
                    }

                    if (_product.target_prices is not null && _product.target_prices.Count > 0) {
                        if (!await UpdateProductPrices(_customer_id, _product.target_prices, (int)inserted_id)) {
                            OnError("InsertProducts: " + _product.sku + " - Product Prices Insert Error");
                            return null;
                        }
                    }

                    if (_product.images is not null && _product.images.Count > 0) {
                        if (!await UpdateProductImages(_customer_id, _product.images, (int)inserted_id)) {
                            OnError("InsertProducts: " + _product.sku + " - Product Images Insert Error");
                            return null;
                        }
                    }

                    if (inserted_id > 0) {
                        return await GetProduct(_customer_id, (int)inserted_id);
                    }
                    else return null;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the specified product in the database for the given customer.
        /// </summary>
        /// <remarks>This method updates the product's details, including its attributes, prices, images,
        /// and sources, in the database. If the product does not exist or any associated updates fail, the method
        /// returns <see langword="null"/>.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer associated with the product.</param>
        /// <param name="_product">The product object containing updated information to be saved.</param>
        /// <param name="_INCS">Include sources. A flag indicating whether to include associated sources in the update. Defaults to <see langword="false"/>.</param>
        /// <returns>The updated <see cref="Product"/> object if the operation is successful; otherwise, <see langword="null"/>.</returns>
        public async Task<Product?> UpdateProduct(int _customer_id, Product _product, bool _INCS = false) {
            try {
                //TODO: _INCS total quantity update
                string _query = "UPDATE products SET type=@type,barcode=@barcode,total_qty=@total_qty,price=@price,special_price=@special_price,custom_price=@custom_price,currency=@currency,tax=@tax,tax_included=@tax_included,update_date=@update_date,name=@name,sources=@sources,source_product_id=@source_product_id WHERE id=@id AND sku=@sku AND customer_id=@customer_id;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("id", _product.id));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("source_product_id", _product.source_product_id));
                cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                cmd.Parameters.Add(new MySqlParameter("sku", _product.sku));
                cmd.Parameters.Add(new MySqlParameter("type", _product.type));
                cmd.Parameters.Add(new MySqlParameter("barcode", _product.barcode));
                cmd.Parameters.Add(new MySqlParameter("price", _product.price));
                cmd.Parameters.Add(new MySqlParameter("special_price", _product.special_price));
                cmd.Parameters.Add(new MySqlParameter("custom_price", _product.custom_price));
                cmd.Parameters.Add(new MySqlParameter("currency", _product.currency.code));
                cmd.Parameters.Add(new MySqlParameter("tax", _product.tax));
                cmd.Parameters.Add(new MySqlParameter("tax_included", _product.tax_included));
                cmd.Parameters.Add(new MySqlParameter("name", _product.name));
                cmd.Parameters.Add(new MySqlParameter("total_qty", _product.sources.Where(x => x.is_active).Sum(x => x.qty)));
                cmd.Parameters.Add(new MySqlParameter("sources", string.Join(",", _product.sources.Where(x => x.is_active).Select(x => x.name).ToArray())));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();

                    if (val == 0) {
                        OnError("UpdateProduct: " + _product.sku + " - Product Not Found");
                        return null;
                    }

                    if (!await UpdateProductExt(_customer_id, _product.extension)) {
                        OnError("UpdateProduct: " + _product.sku + " - Product Extension Update Error");
                        return null;
                    }

                    if (_INCS) {
                        var deleted_sources = (await GetProductSources(_customer_id, _product.sku))
                            .ExceptBy(_product.sources.Select(s => s.name) ?? [], s => s.name).ToList();
                        foreach (var ditem in deleted_sources) {
                            if (!await DeleteProductSource(_customer_id, ditem)) {
                                OnError("UpdateProduct: " + _product.sku + " - Product Source Delete Error");
                                return null;
                            }
                        }
                        if (!await UpdateProductSources(_customer_id, _product.sources, _product.sku)) {
                            OnError("UpdateProducts: " + _product.sku + " - Product Source Insert Error");
                            return null;
                        }
                    }

                    var deleted_prices = (await GetProductPrices(_customer_id, _product.id))
                        .ExceptBy(_product.target_prices.Select(tp => tp.id), tp => tp.id).ToList();
                    foreach (var ditem in deleted_prices) {
                        if (!await DeleteProductPrice(_customer_id, ditem)) {
                            OnError("UpdateProduct: " + _product.sku + " - Product Price Delete Error");
                            return null;
                        }
                    }
                    if (_product.target_prices is not null && _product.target_prices.Count > 0) {
                        if (!await UpdateProductPrices(_customer_id, _product.target_prices, _product.id)) {
                            OnError("UpdateProduct: " + _product.sku + " - Product Prices Update Error");
                            return null;
                        }
                    }

                    var deleted_images = (await GetProductImages(_customer_id, _product.id))
                        .ExceptBy(_product.images.Select(img => img.id), img => img.id).ToList();
                    foreach (var ditem in deleted_images) {
                        if (!await DeleteProductImage(_customer_id, ditem)) {
                            OnError("UpdateProduct: " + _product.sku + " - Product Image Delete Error");
                            return null;
                        }
                    }
                    if (_product.images is not null && _product.images.Count > 0) {
                        if (!await UpdateProductImages(_customer_id, _product.images, _product.id)) {
                            OnError("UpdateProduct: " + _product.sku + " - Product Images Update Error");
                            return null;
                        }
                    }

                    if (_product.attributes is not null && _product.attributes.Count > 0) {
                        if (!await UpdateProductAttributes(_customer_id, _product.attributes, _product.id)) {
                            OnError("UpdateProduct: " + _product.sku + " - Product Attributes Update Error");
                            return null;
                        }
                    }
                    return await GetProduct(_customer_id, _product.id);
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets XML enabled products from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_val">Value</param>
        /// <param name="_with_ext">Get with extension</param>
        /// <returns>[Error] returns 'null'</returns>
        //public List<Product> GetXMLEnabledProducts(int _customer_id, bool _val = true, bool _with_ext = true) {
        //    try {
        //        if (state != System.Data.ConnectionState.Open) connection.Open();
        //        string _query = "SELECT p.id,p.customer_id,p.source_product_id,p.update_date,p.sku,p.type,p.total_qty,p.name,p.barcode,p.price,p.special_price,p.custom_price,p.currency,p.tax,p.tax_included " +
        //            "FROM products_ext AS ext INNER JOIN products AS p ON ext.sku=p.sku WHERE ext.is_xml_enabled=@is_xml_enabled AND ext.customer_id=@customer_id";
        //        List<Product> list = [];
        //        MySqlCommand cmd = new MySqlCommand(_query, connection);
        //        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
        //        cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _val ? 1 : 0));
        //        MySqlDataReader dataReader = cmd.ExecuteReader();
        //        while (dataReader.Read()) {
        //            Product p = new Product {
        //                id = Convert.ToInt32(dataReader["id"].ToString()),
        //                customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
        //                source_product_id = Convert.ToInt32(dataReader["source_product_id"].ToString()),
        //                update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
        //                sku = dataReader["sku"].ToString(),
        //                type = (Product.ProductTypes)Convert.ToInt32(dataReader["type"].ToString()),
        //                total_qty = Convert.ToInt32(dataReader["total_qty"].ToString()),
        //                name = dataReader["name"].ToString(),
        //                barcode = dataReader["barcode"].ToString(),
        //                price = decimal.Parse(dataReader["price"].ToString()),
        //                special_price = decimal.Parse(dataReader["special_price"].ToString()),
        //                custom_price = decimal.Parse(dataReader["custom_price"].ToString()),
        //                currency = Currency.GetCurrency(dataReader["currency"].ToString()),
        //                tax = Convert.ToInt32(dataReader["tax"].ToString()),
        //                tax_included = Convert.ToBoolean(Convert.ToInt32(dataReader["tax_included"].ToString())),
        //            };
        //            list.Add(p);
        //        }
        //        dataReader.Close();
        //        if (state == System.Data.ConnectionState.Open) connection.Close();
        //        if (_with_ext) {
        //            var exts = GetProductExts(_customer_id).Result;
        //            foreach (var item in list) {
        //                var selected_ext = exts?.Where(x => x.sku == item.sku).FirstOrDefault();
        //                if (selected_ext is not null)
        //                    item.extension = selected_ext;
        //                else {
        //                    OnError("GetXMLEnabledProducts: " + item.sku + " - Product Extension Not Found");
        //                    return null;
        //                }
        //            }
        //        }

        //        var product_sources = GetProductSources(_customer_id).Result;
        //        foreach (var item in list) {
        //            var selected_product_source = product_sources?.Where(x => x.sku == item.sku).ToList();
        //            if (selected_product_source is not null)
        //                item.sources = selected_product_source;
        //            else {
        //                OnError("GetXMLEnabledProducts: " + item.sku + " - Product Source Not Found");
        //                return null;
        //            }
        //        }

        //        return list;
        //    }
        //    catch (Exception ex) {
        //        OnError(ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// Determines whether a product extension exists for the specified customer and SKU.
        /// </summary>
        /// <remarks>This method queries the database to check for the existence of a product extension
        /// associated with the given customer ID and SKU. Ensure that the database connection is properly configured
        /// before calling this method.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer. Must be a valid customer ID.</param>
        /// <param name="_sku">The stock keeping unit (SKU) of the product extension. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if a product extension exists for the specified customer and SKU; otherwise, <see
        /// langword="false"/>.</returns>
        public async Task<bool> IsProductExtExists(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT COUNT(*) FROM products_ext WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    _ = int.TryParse(s: (await cmd.ExecuteScalarAsync()).ToString(), result: out int total_count);
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return total_count > 0;
                }
                return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Determines whether a product exists in the database for the specified customer and SKU.
        /// </summary>
        /// <remarks>This method queries the database to check for the existence of a product based on the
        /// provided customer ID and SKU. Ensure that the database connection is properly configured and accessible
        /// before calling this method.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer. Must be a valid customer ID.</param>
        /// <param name="_sku">The SKU (Stock Keeping Unit) of the product to check. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if a product with the specified customer ID and SKU exists in the database;
        /// otherwise, <see langword="false"/>.</returns>
        public async Task<bool> IsProductExists(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT COUNT(*) FROM products WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    _ = int.TryParse(s: (await cmd.ExecuteScalarAsync()).ToString(), out int total_count);
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return total_count > 0;
                }
                return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets product count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public async Task<int> GetProductsCount(int _customer_id, ApiFilter _filters) {
            try {
                string _query = "SELECT COUNT(*) FROM products_with_mainsource WHERE p_customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Product), true);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    object? val = await cmd.ExecuteScalarAsync();
                    if (val is null) return -1;
                    _ = int.TryParse(val.ToString(), out int total_count);
                    if (this.state == System.Data.ConnectionState.Open) await CloseConnection();
                    return total_count;
                }
                return -1;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Gets extended properties with filters of type from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <param name="_type">Type of the object</param>
        public async Task<Dictionary<string, dynamic>> GetExtendedQuery(int _customer_id, ApiFilter _filters, Type _type) {
            try {
                Dictionary<string, dynamic> _result = [];
                switch (_type) {
                    case Type t when t == typeof(Product):
                        string _query_product = "SELECT COUNT(*) AS count,MIN(p_price) AS min_price, MAX(p_price) AS max_price, MIN(p_total_qty) AS min_qty, MAX(p_total_qty) AS max_qty " +
                            "FROM products_with_mainsource WHERE p_customer_id=@customer_id;";
                        MySqlCommand cmd_product = new() { Connection = connection };
                        cmd_product.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query_product, ref cmd_product, _type, true);
                        cmd_product.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        MySqlDataReader dataReader_product = await cmd_product.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        if (await dataReader_product.ReadAsync()) {
                            _result["TotalCount"] = Convert.ToInt32(dataReader_product["count"].ToString());
                            _result["MinPrice"] = decimal.Parse(dataReader_product["min_price"].ToString());
                            _result["MaxPrice"] = decimal.Parse(dataReader_product["max_price"].ToString());
                            _result["MinQty"] = Convert.ToInt32(dataReader_product["min_qty"].ToString());
                            _result["MaxQty"] = Convert.ToInt32(dataReader_product["max_qty"].ToString());
                        }
                        if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
                        break;
                    case Type t when t == typeof(Category):
                        string _query_category = "SELECT COUNT(*) AS count FROM categories WHERE customer_id=@customer_id;";
                        MySqlCommand cmd_cat = new() { Connection = connection };
                        cmd_cat.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query_category, ref cmd_cat, _type, true);
                        cmd_cat.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        MySqlDataReader dataReader_cat = await cmd_cat.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        if (await dataReader_cat.ReadAsync()) {
                            _result["TotalCount"] = Convert.ToInt32(dataReader_cat["count"].ToString());
                        }
                        if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
                        break;
                    case Type t when t == typeof(Brand):
                        string _query_brand = "SELECT COUNT(*) AS count FROM brands WHERE customer_id=@customer_id;";
                        MySqlCommand cmd_brand = new() { Connection = connection };
                        cmd_brand.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query_brand, ref cmd_brand, _type, true);
                        cmd_brand.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        MySqlDataReader dataReader_brand = await cmd_brand.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        if (await dataReader_brand.ReadAsync()) {
                            _result["TotalCount"] = Convert.ToInt32(dataReader_brand["count"].ToString());
                        }
                        if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
                        break;
                    case Type t when t == typeof(Log):
                        string _query_log = "SELECT COUNT(*) AS count FROM log WHERE customer_id=@customer_id;";
                        MySqlCommand cmd_log = new() { Connection = connection };
                        cmd_log.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query_log, ref cmd_log, _type, true);
                        cmd_log.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        MySqlDataReader dataReader_log = await cmd_log.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        if (await dataReader_log.ReadAsync()) {
                            _result["TotalCount"] = Convert.ToInt32(dataReader_log["count"].ToString());
                        }
                        if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
                        break;
                    case Type t when t == typeof(Notification):
                        string _query_notification = "SELECT COUNT(*) AS count FROM notifications WHERE customer_id=@customer_id;";
                        MySqlCommand cmd_notification = new() { Connection = connection };
                        cmd_notification.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query_notification, ref cmd_notification, _type, true);
                        cmd_notification.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        MySqlDataReader dataReader_notification = await cmd_notification.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        if (await dataReader_notification.ReadAsync()) {
                            _result["TotalCount"] = Convert.ToInt32(dataReader_notification["count"].ToString());
                        }
                        if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();
                        break;
                }
                return _result;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return [];
            }
        }

        /// <summary>
        /// Updates XML status by product barcode in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_barcode">Barcode</param>
        /// <param name="_is_xml_enabled">Is XML Enabled</param>
        /// <param name="_xml_sources">XML Sources</param>
        /// <param name="_with_ext">Update with extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool UpdateXMLStatusByProductBarcode(int _customer_id, string _barcode, bool _is_xml_enabled, string[]? _xml_sources, bool _with_ext = true) {
            try {
                int val = 0;
                if (!string.IsNullOrWhiteSpace(_barcode)) {
                    string _query = "UPDATE products_ext SET is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources WHERE barcode=@barcode AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("barcode", _barcode));
                    cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _is_xml_enabled ? 1 : 0));
                    cmd.Parameters.Add(new MySqlParameter("xml_sources", _xml_sources is not null ? string.Join(",", _xml_sources) : null));
                    if (state != System.Data.ConnectionState.Open) connection.Open();
                    val += cmd.ExecuteNonQuery();
                    if (state == System.Data.ConnectionState.Open) connection.Close();

                    if (val > 0)
                        return true;
                    else return false;
                }
                return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Product Extension
        /// <summary>
        /// Gets the product extension from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<ProductExtension?> GetProductExt(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM products_ext WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    ProductExtension? px = null;
                    if (await dataReader.ReadAsync()) {
                        px = new ProductExtension();
                        px.id = Convert.ToInt32(dataReader["id"].ToString());
                        px.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                        px.brand_id = Convert.ToInt32(dataReader["brand_id"].ToString());
                        px.category_ids = dataReader["category_ids"].ToString();
                        px.sku = dataReader["sku"].ToString();
                        px.barcode = dataReader["barcode"].ToString();
                        px.is_xml_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_xml_enabled"].ToString()));
                        px.xml_sources = dataReader["xml_sources"]?.ToString()?.Split(',');
                        px.description = dataReader["description"].ToString();
                        px.weight = decimal.Parse(dataReader["weight"].ToString());
                        px.volume = decimal.Parse(dataReader["volume"].ToString());
                        px.is_enabled = Convert.ToBoolean(Convert.ToInt32(dataReader["is_enabled"].ToString()));
                        px.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await connection.CloseAsync();

                    if (px is not null && px.brand_id > 0) {
                        var b = await GetBrand(_customer_id, px.brand_id);
                        if (b is not null) {
                            px.brand = b;
                        }
                        else {
                            OnError("GetProductExt: " + px.sku + " - Product Brand Not Found");
                            return null;
                        }

                        var cats = await GetProductCategories(_customer_id, _sku);
                        if (cats is not null && cats.Count > 0) {
                            px.categories = [.. cats];
                        }
                        else {
                            OnError("GetProductExt: " + px.sku + " - Product Categories Not Found");
                            return null;
                        }
                    }
                    return px;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product extensions from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductExtension>?> GetProductExts(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM products_ext WHERE customer_id=@customer_id";
                    List<ProductExtension> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductExtension s = new ProductExtension {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            brand_id = Convert.ToInt32(dataReader["brand_id"].ToString()),
                            category_ids = dataReader["category_ids"].ToString(),
                            sku = dataReader["sku"].ToString(),
                            barcode = dataReader["barcode"].ToString(),
                            is_xml_enabled = dataReader["is_xml_enabled"] is not null && dataReader["is_xml_enabled"].ToString() == "1",
                            xml_sources = dataReader["xml_sources"]?.ToString()?.Split(','),
                            description = dataReader["description"].ToString(),
                            weight = decimal.Parse(dataReader["weight"].ToString()),
                            volume = decimal.Parse(dataReader["volume"].ToString()),
                            is_enabled = dataReader["is_enabled"] is not null && dataReader["is_enabled"].ToString() == "1",
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(s);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();

                    var brands = await GetBrands(_customer_id);
                    var categories = await GetCategories(_customer_id);
                    if (brands == null || categories == null) {
                        OnError("GetProductExts: Brands or Categories Not Found");
                        return null;
                    }

                    foreach (var item in list) {
                        var b = brands.FirstOrDefault(x => x.id == item.brand_id);
                        if (b is not null) {
                            item.brand = b;
                        }
                        else {
                            OnError("GetProductExts: " + item.sku + " - Product Brand Not Found");
                            return null;
                        }

                        var c = categories.Where(x => item.category_ids.Split(',').Contains(x.id.ToString())).ToList();
                        if (c is not null && c.Count > 0) {
                            item.categories = [.. c];
                        }
                        else {
                            OnError("GetProductExts: " + item.sku + " - Product Categories Not Found");
                            return null;
                        }
                    }
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
        /// Inserts the product extension to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> InsertProductExt(int _customer_id, ProductExtension _source) {
            try {
                if (await this.IsProductExtExists(_customer_id, _source.sku)) {
                    OnError("Sku is already in table.");
                    return false;
                }

                int val = 0;
                string _query = "INSERT INTO products_ext (customer_id,brand_id,category_ids,sku,barcode,weight,volume,description,is_enabled,is_xml_enabled,xml_sources) VALUES " +
                    "(@customer_id,@brand_id,@category_ids,@sku,@barcode,@weight,@volume,@description,@is_enabled,@is_xml_enabled,@xml_sources);";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_id", _source.brand_id));
                cmd.Parameters.Add(new MySqlParameter("category_ids", _source.category_ids));
                cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode ?? string.Empty));
                cmd.Parameters.Add(new MySqlParameter("weight", _source.weight));
                cmd.Parameters.Add(new MySqlParameter("volume", _source.volume));
                cmd.Parameters.Add(new MySqlParameter("description", _source.description));
                cmd.Parameters.Add(new MySqlParameter("is_enabled", _source.is_enabled ? 1 : 0));
                cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _source.is_xml_enabled));
                cmd.Parameters.Add(new MySqlParameter("xml_sources", _source.xml_sources is not null ? string.Join(",", _source.xml_sources) : null));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                val = await cmd.ExecuteNonQueryAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Updates the product extension in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Extension</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> UpdateProductExt(int _customer_id, ProductExtension _source) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    int val = 0;
                    string _query = "UPDATE products_ext SET brand_id=@brand_id,category_ids=@category_ids,barcode=@barcode,is_xml_enabled=@is_xml_enabled,xml_sources=@xml_sources,update_date=@update_date,weight=@weight,volume=@volume,description=@description,is_enabled=@is_enabled " +
                        "WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                    cmd.Parameters.Add(new MySqlParameter("brand_id", _source.brand_id));
                    cmd.Parameters.Add(new MySqlParameter("category_ids", _source.category_ids));
                    cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode));
                    cmd.Parameters.Add(new MySqlParameter("xml_sources", (_source.xml_sources is not null && _source.xml_sources.Length > 0) ? string.Join(",", _source.xml_sources) : string.Empty));
                    cmd.Parameters.Add(new MySqlParameter("is_xml_enabled", _source.is_xml_enabled));
                    cmd.Parameters.Add(new MySqlParameter("weight", _source.weight));
                    cmd.Parameters.Add(new MySqlParameter("volume", _source.volume));
                    cmd.Parameters.Add(new MySqlParameter("description", _source.description));
                    cmd.Parameters.Add(new MySqlParameter("is_enabled", _source.is_enabled));
                    cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                    val = await cmd.ExecuteNonQueryAsync();
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
        /// Deletes the product extension from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteProductExt(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    int val = 0;
                    string _query = "DELETE FROM products_ext WHERE sku=@sku AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    val = await cmd.ExecuteNonQueryAsync();
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

        #region Product Source
        /// <summary>
        /// Updates the product sources in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sources">Product Sources</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> UpdateProductSources(int _customer_id, List<ProductSource> _sources, string _sku) {
            try {
                if (_sources.Count == 0) {
                    OnError("UpdateProductSources: No sources found");
                    return false;
                }
                ulong val = 0;
                var existed_sources = await GetProductSources(_customer_id, _sku);
                foreach (var item in _sources) {
                    var existed_source = existed_sources?.FirstOrDefault(x => x.name == item.name && x.sku == item.sku);
                    if (existed_source is not null) {
                        if (existed_source.qty != item.qty || existed_source.is_active != item.is_active || existed_source.barcode != item.barcode) {
                            string _query = "UPDATE product_sources SET barcode=@barcode,qty=@qty,is_active=@is_active,update_date=@update_date WHERE sku=@sku AND name=@name AND customer_id=@customer_id;";
                            MySqlCommand cmd = new MySqlCommand(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("name", item.name));
                            cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                            cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                            cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                            cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) await OpenConnection();
                            val = (ulong)await cmd.ExecuteNonQueryAsync();
                            if (state == System.Data.ConnectionState.Open) await CloseConnection();
                        }
                        else val++;
                    }
                    else {
                        string _query = "INSERT INTO product_sources (customer_id,name,sku,barcode,qty,is_active) VALUES (@customer_id,@name,@sku,@barcode,@qty,@is_active);";
                        MySqlCommand cmd = new MySqlCommand(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("name", item.name));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("barcode", item.barcode));
                        cmd.Parameters.Add(new MySqlParameter("qty", item.qty));
                        cmd.Parameters.Add(new MySqlParameter("is_active", item.is_active));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        val = (ulong)await cmd.ExecuteNonQueryAsync();
                        if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    }
                }

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the product sources from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductSource>?> GetProductSources(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_sources WHERE customer_id=@customer_id;";
                    List<ProductSource> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductSource s = new ProductSource(
                            _customer_id,
                            Convert.ToInt32(dataReader["id"].ToString()),
                            dataReader["name"].ToString(),
                            dataReader["sku"].ToString(),
                            dataReader["barcode"].ToString(),
                            Convert.ToInt32(dataReader["qty"].ToString()),
                            Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                            Convert.ToDateTime(dataReader["update_date"].ToString())
                        );

                        list.Add(s);
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
        /// Gets the product sources from the database except the main source
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductSource>?> GetProductOtherSources(int _customer_id, string _main_source) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_sources WHERE name <> @name AND customer_id=@customer_id;";
                    List<ProductSource> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("name", _main_source));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductSource s = new ProductSource(
                            _customer_id,
                            Convert.ToInt32(dataReader["id"].ToString()),
                            dataReader["name"].ToString(),
                            dataReader["sku"].ToString(),
                            dataReader["barcode"].ToString(),
                            Convert.ToInt32(dataReader["qty"].ToString()),
                            Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                            Convert.ToDateTime(dataReader["update_date"].ToString())
                        );

                        list.Add(s);
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
        /// Gets the product sources from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductSource>?> GetProductSources(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                    List<ProductSource> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductSource s = new ProductSource(
                            _customer_id,
                            Convert.ToInt32(dataReader["id"].ToString()),
                            dataReader["name"].ToString(),
                            dataReader["sku"].ToString(),
                            dataReader["barcode"].ToString(),
                            Convert.ToInt32(dataReader["qty"].ToString()),
                            Convert.ToBoolean(Convert.ToInt32(dataReader["is_active"].ToString())),
                            Convert.ToDateTime(dataReader["update_date"].ToString())
                        );
                        list.Add(s);
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
        /// Inserts the product source to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_source">Product Source</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public bool InsertProductSource(int _customer_id, ProductSource _source) {
            try {
                string _query = "INSERT INTO product_sources (customer_id,name,sku,barcode,qty,is_active) VALUES (@customer_id,@name,@sku,@barcode,@qty,@is_active)";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("name", _source.name));
                cmd.Parameters.Add(new MySqlParameter("sku", _source.sku));
                cmd.Parameters.Add(new MySqlParameter("barcode", _source.barcode));
                cmd.Parameters.Add(new MySqlParameter("qty", _source.qty));
                cmd.Parameters.Add(new MySqlParameter("is_active", _source.is_active));
                if (state != System.Data.ConnectionState.Open) connection.Open();
                int val = cmd.ExecuteNonQuery();
                if (state == System.Data.ConnectionState.Open) connection.Close();
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the product source from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_source">ProductSource</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteProductSource(int _customer_id, ProductSource _product_source) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "DELETE FROM product_sources WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _product_source.id));
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
        /// Deletes the product source from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        [Obsolete]
        public bool DeleteProductSources(int _customer_id, string _sku) {
            try {
                if (state != System.Data.ConnectionState.Open)
                    connection.Open();

                int val = 0;
                string _query = "DELETE FROM product_sources WHERE sku=@sku AND customer_id=@customer_id";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                val = cmd.ExecuteNonQuery();

                if (state == System.Data.ConnectionState.Open)
                    connection.Close();

                if (val > 0)
                    return true;
                else return false;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Product Price
        /// <summary>
        /// Get product prices from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductPrice>?> GetProductPrices(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_prices WHERE customer_id=@customer_id;";
                    List<ProductPrice> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductPrice s = new ProductPrice() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            platform_name = dataReader["platform_name"].ToString(),
                            price1 = decimal.Parse(dataReader["price1"].ToString()),
                            price2 = decimal.Parse(dataReader["price2"].ToString()),
                            update_currency_as = Currency.GetCurrency(dataReader["update_currency_as"].ToString()),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(s);
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
        /// Inserts the product price to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>Error returns 'int:0'</returns>
        public async Task<int> InsertProductPrice(int _customer_id, ProductPrice _price) {
            try {
                object? val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO product_prices (customer_id,product_id,sku,platform_name,price1,price2,update_currency_as) VALUES (@customer_id,@product_id,@sku,@platform_name,@price1,@price2,@update_currency_as);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _price.product_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _price.sku));
                cmd.Parameters.Add(new MySqlParameter("platform_name", _price.platform_name));
                cmd.Parameters.Add(new MySqlParameter("price1", _price.price1));
                cmd.Parameters.Add(new MySqlParameter("price2", _price.price2));
                cmd.Parameters.Add(new MySqlParameter("update_currency_as", _price.update_currency_as.ToString()));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int PPID))
                        return PPID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Updates the product price in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_prices">ProductPrice</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<bool> UpdateProductPrices(int _customer_id, List<ProductPrice> _prices, int _product_id) {
            try {
                ulong val = 0;
                var existed_prices = await GetProductPrices(_customer_id, _product_id);
                foreach (var item in _prices) {
                    var existed_price = existed_prices?.FirstOrDefault(x => x.platform_name == item.platform_name && x.product_id == item.product_id);
                    if (existed_price is not null) {
                        if (existed_price.price1 != item.price1 || existed_price.price2 != item.price2 || existed_price.update_currency_as != item.update_currency_as) {
                            string _query = "UPDATE product_prices SET price1=@price1,price2=@price2,update_currency_as=@update_currency_as WHERE product_id=@product_id AND platform_name=@platform_name AND customer_id=@customer_id;";
                            MySqlCommand cmd = new(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                            cmd.Parameters.Add(new MySqlParameter("platform_name", item.platform_name));
                            cmd.Parameters.Add(new MySqlParameter("price1", item.price1));
                            cmd.Parameters.Add(new MySqlParameter("price2", item.price2));
                            cmd.Parameters.Add(new MySqlParameter("update_currency_as", item.update_currency_as.code));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) await OpenConnection();
                            val += (ulong)await cmd.ExecuteNonQueryAsync();
                            if (state == System.Data.ConnectionState.Open) await CloseConnection();
                        }
                        else val++;
                    }
                    else {
                        string _query = "START TRANSACTION;" +
                            "INSERT INTO product_prices (customer_id,product_id,sku,platform_name,price1,price2,update_currency_as) VALUES (@customer_id,@product_id,@sku,@platform_name,@price1,@price2,@update_currency_as);" +
                            "SELECT LAST_INSERT_ID();" +
                            "COMMIT;";
                        MySqlCommand cmd = new(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("platform_name", item.platform_name));
                        cmd.Parameters.Add(new MySqlParameter("price1", item.price1));
                        cmd.Parameters.Add(new MySqlParameter("price2", item.price2));
                        cmd.Parameters.Add(new MySqlParameter("update_currency_as", item.update_currency_as.code));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        val += (ulong)await cmd.ExecuteScalarAsync();
                        if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    }
                }
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the product prices by product_id from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductPrice>?> GetProductPrices(int _customer_id, int _product_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_prices WHERE customer_id=@customer_id AND product_id=@product_id;";
                    List<ProductPrice> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductPrice pp = new ProductPrice {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            platform_name = dataReader["platform_name"].ToString(),
                            price1 = decimal.Parse(dataReader["price1"].ToString()),
                            price2 = decimal.Parse(dataReader["price2"].ToString()),
                            update_currency_as = Currency.GetCurrency(dataReader["update_currency_as"].ToString()),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pp);
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
        /// Deletes the product price target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_price">ProductPrice</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteProductPrice(int _customer_id, ProductPrice _product_price) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "DELETE FROM product_prices WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _product_price.id));
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

        #region Product Image
        /// <summary>
        /// Inserts the product image to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_image">ProductImage</param>
        /// <returns>Error returns 'int:0'</returns>
        public async Task<int> InsertProductImage(int _customer_id, ProductImage _image) {
            try {
                object? val;
                string _query = "START TRANSACTION;" +
                    "INSERT INTO product_images (customer_id,product_id,sku,type,image_name,image_url,image_base64,is_default) VALUES (@customer_id,@product_id,@sku,@type,@image_name,@image_url,@image_base64,@is_default);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("product_id", _image.product_id));
                cmd.Parameters.Add(new MySqlParameter("sku", _image.sku));
                cmd.Parameters.Add(new MySqlParameter("type", (int)_image.type));
                cmd.Parameters.Add(new MySqlParameter("image_name", _image.image_name));
                cmd.Parameters.Add(new MySqlParameter("image_url", _image.image_url));
                cmd.Parameters.Add(new MySqlParameter("image_base64", _image.image_base64));
                cmd.Parameters.Add(new MySqlParameter("is_default", _image.is_default ? 1 : 0));

                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int PIID))
                        return PIID;
                }
                return 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Updates the product image in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_images">ProductImages</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<bool> UpdateProductImages(int _customer_id, List<ProductImage> _images, int _product_id) {
            try {
                ulong val = 0;
                var existed_images = await GetProductImages(_customer_id, _product_id);
                foreach (var item in _images) {
                    var existed_image = existed_images?.FirstOrDefault(x => x.image_name == item.image_name && x.sku == item.sku);
                    if (existed_image is not null) {
                        if (existed_image.image_url != item.image_url || existed_image.is_default != item.is_default || existed_image.type != item.type) {
                            string _query = "UPDATE product_images SET type=@type,image_url=@image_url,image_base64=@image_base64,is_default=@is_default,update_date=@update_date WHERE sku=@sku AND product_id=@product_id AND image_name=@image_name AND customer_id=@customer_id;";
                            MySqlCommand cmd = new(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                            cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                            cmd.Parameters.Add(new MySqlParameter("type", item.type));
                            cmd.Parameters.Add(new MySqlParameter("image_name", item.image_name));
                            cmd.Parameters.Add(new MySqlParameter("image_url", item.image_url));
                            cmd.Parameters.Add(new MySqlParameter("image_base64", item.image_base64));
                            cmd.Parameters.Add(new MySqlParameter("is_default", item.is_default));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) await OpenConnection();
                            val += (ulong)await cmd.ExecuteNonQueryAsync();
                            if (state == System.Data.ConnectionState.Open) await CloseConnection();
                        }
                        else val++;
                    }
                    else {
                        string _query = "START TRANSACTION;" +
                            "INSERT INTO product_images (customer_id,product_id,sku,type,image_name,image_url,image_base64,is_default) VALUES (@customer_id,@product_id,@sku,@type,@image_name,@image_url,@image_base64,@is_default);" +
                            "SELECT LAST_INSERT_ID();" +
                            "COMMIT;";
                        MySqlCommand cmd = new(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("type", item.type));
                        cmd.Parameters.Add(new MySqlParameter("image_name", item.image_name));
                        cmd.Parameters.Add(new MySqlParameter("image_url", item.image_url));
                        cmd.Parameters.Add(new MySqlParameter("image_base64", item.image_base64));
                        cmd.Parameters.Add(new MySqlParameter("is_default", item.is_default));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        val += (ulong)await cmd.ExecuteScalarAsync();
                        if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    }
                }
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the product images from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductImage>?> GetProductImages(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id;";
                    List<ProductImage> list = [];
                    MySqlCommand cmd = new(_query, connection) {
                        CommandTimeout = 600
                    };
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductImage pi = new ProductImage {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                            image_name = dataReader["image_name"].ToString(),
                            image_url = dataReader["image_url"].ToString(),
                            image_base64 = dataReader["image_base64"].ToString(),
                            is_default = dataReader["is_default"] is not null && dataReader["is_default"].ToString() == "1",
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pi);
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
        /// Gets the product images by sku from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductImage>?> GetProductImages(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id AND sku=@sku;";
                    List<ProductImage> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductImage pi = new ProductImage {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                            image_name = dataReader["image_name"].ToString(),
                            image_url = dataReader["image_url"].ToString(),
                            image_base64 = dataReader["image_base64"].ToString(),
                            is_default = dataReader["is_default"] is not null && dataReader["is_default"].ToString() == "1",
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pi);
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
        /// Gets the product images by product_id from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductImage>?> GetProductImages(int _customer_id, int _product_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_images WHERE customer_id=@customer_id AND product_id=@product_id;";
                    List<ProductImage> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductImage pi = new ProductImage {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            type = (ImageTypes)Convert.ToInt32(dataReader["type"].ToString()),
                            image_name = dataReader["image_name"].ToString(),
                            image_url = dataReader["image_url"].ToString(),
                            image_base64 = dataReader["image_base64"].ToString(),
                            is_default = dataReader["is_default"] is not null && dataReader["is_default"].ToString() == "1",
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString())
                        };
                        list.Add(pi);
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
        /// Deletes the product image target from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_image">Product Image</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    int val = 0;
                    string _query = "DELETE FROM product_images WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _product_image.id));
                    val = await cmd.ExecuteNonQueryAsync();
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

        #region Attribute
        /// <summary>
        /// Gets the attribute from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_name">Attribute Name</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Attribute?> GetAttribute(int _customer_id, string _name) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM attributes WHERE attribute_name=@attribute_name AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("attribute_name", _name));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Attribute? a = null;
                    if (await dataReader.ReadAsync()) {
                        a = new Attribute();
                        a.id = Convert.ToInt32(dataReader["id"].ToString());
                        a.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                        a.attribute_name = dataReader["attribute_name"].ToString();
                        a.attribute_title = dataReader["attribute_title"].ToString();
                        a.type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString());
                        a.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return a;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the attribute from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attribute_id">Attribute ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Attribute?> GetAttribute(int _customer_id, int _attribute_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM attributes WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _attribute_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Attribute? a = null;
                    if (await dataReader.ReadAsync()) {
                        a = new Attribute();
                        a.id = Convert.ToInt32(dataReader["id"].ToString());
                        a.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                        a.attribute_name = dataReader["attribute_name"].ToString();
                        a.attribute_title = dataReader["attribute_title"].ToString();
                        a.type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString());
                        a.update_date = Convert.ToDateTime(dataReader["update_date"].ToString());
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return a;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the attribute options from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attribute_id">Attribute ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<AttributeOption>?> GetAttributeOptions(int _customer_id, int _attribute_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM attribute_options WHERE attribute_id=@attribute_id AND customer_id=@customer_id;";
                    List<AttributeOption> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("attribute_id", _attribute_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        AttributeOption ao = new AttributeOption {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                            option_name = dataReader["option_name"].ToString(),
                            option_value = dataReader["option_value"].ToString(),
                        };
                        list.Add(ao);
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
        /// Gets the attribute options from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_option_ids">Option ID's</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<AttributeOption>?> GetAttributeOptions(int _customer_id, string _option_ids) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = $"SELECT * FROM attribute_options WHERE id IN ({_option_ids}) AND customer_id=@customer_id;";
                    List<AttributeOption> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    //cmd.Parameters.Add( new MySqlParameter( "option_ids", _option_ids ) );
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        AttributeOption ao = new AttributeOption {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                            option_name = dataReader["option_name"].ToString(),
                            option_value = dataReader["option_value"].ToString(),
                        };
                        list.Add(ao);
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
        /// Gets the product attributes from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductAttribute>?> GetProductAttributes(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_attributes WHERE customer_id=@customer_id;";
                    List<ProductAttribute> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductAttribute pa = new ProductAttribute {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                            type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString()),
                            value = dataReader["value"].ToString(),
                            option_ids = dataReader["option_ids"].ToString(),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        };
                        list.Add(pa);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();

                    foreach (var item in list) {
                        var attr = await GetAttribute(_customer_id, item.attribute_id);
                        if (attr is not null) {
                            item.attribute = attr;
                        }
                        else {
                            OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Not Found");
                            return null;
                        }
                        if (item.option_ids is not null && item.option_ids.Length > 0) {
                            var options = await GetAttributeOptions(_customer_id, item.option_ids);
                            if (options is not null && options.Count > 0) {
                                item.options = [.. options];
                            }
                            else {
                                OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Options Not Found");
                                return null;
                            }
                        }
                    }
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
        /// Gets the product attributes from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_product_id">Product ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<ProductAttribute>?> GetProductAttributes(int _customer_id, int _product_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM product_attributes WHERE product_id=@product_id AND customer_id=@customer_id;";
                    List<ProductAttribute> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        ProductAttribute pa = new ProductAttribute {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            product_id = Convert.ToInt32(dataReader["product_id"].ToString()),
                            sku = dataReader["sku"].ToString(),
                            attribute_id = Convert.ToInt32(dataReader["attribute_id"].ToString()),
                            type = Enum.Parse<AttributeTypes>(dataReader["type"].ToString()),
                            value = dataReader["value"].ToString(),
                            option_ids = dataReader["option_ids"].ToString(),
                            update_date = Convert.ToDateTime(dataReader["update_date"].ToString()),
                        };
                        list.Add(pa);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();

                    foreach (var item in list) {
                        var attr = await GetAttribute(_customer_id, item.attribute_id);
                        if (attr is not null) {
                            item.attribute = attr;
                        }
                        else {
                            OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Not Found");
                            return null;
                        }
                        if (item.option_ids is not null && item.option_ids.Length > 0) {
                            var options = await GetAttributeOptions(_customer_id, item.option_ids);
                            if (options is not null && options.Count > 0) {
                                item.options = [.. options];
                            }
                            else {
                                OnError("GetProductAttributes: " + item.product_id + " - Product Attribute Options Not Found");
                                return null;
                            }
                        }
                    }
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
        /// Updates the product attributes in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_attrs">Product Attributes</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> UpdateProductAttributes(int _customer_id, List<ProductAttribute> _attrs, int _product_id) {
            try {
                ulong val = 0;
                var existed_attrs = await GetProductAttributes(_customer_id, _product_id);
                foreach (var item in _attrs) {
                    var existed_attr = existed_attrs?.FirstOrDefault(x => x.attribute_id == item.attribute_id && x.sku == item.sku);
                    if (existed_attr is not null) {
                        if (existed_attr.value != item.value || existed_attr.type != item.type || existed_attr.option_ids != item.option_ids) {
                            string _query = "UPDATE product_attributes SET value=@value,type=@type,option_ids=@option_ids,update_date=@update_date WHERE sku=@sku AND product_id=@product_id attribute_id=@attribute_id AND customer_id=@customer_id;";
                            MySqlCommand cmd = new(_query, connection);
                            cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                            cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                            cmd.Parameters.Add(new MySqlParameter("attribute_id", item.attribute_id));
                            cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                            cmd.Parameters.Add(new MySqlParameter("type", item.type));
                            cmd.Parameters.Add(new MySqlParameter("value", item.value));
                            cmd.Parameters.Add(new MySqlParameter("option_ids", item.option_ids));
                            cmd.Parameters.Add(new MySqlParameter("update_date", DateTime.Now));
                            if (state != System.Data.ConnectionState.Open) await OpenConnection();
                            val += (ulong)await cmd.ExecuteNonQueryAsync();
                            if (state == System.Data.ConnectionState.Open) await CloseConnection();
                        }
                        else val++;
                    }
                    else {
                        string _query = "START TRANSACTION;" +
                            "INSERT INTO product_attributes (customer_id,product_id,sku,attribute_id,type,value,option_ids) VALUES (@customer_id,@product_id,@sku,@attribute_id,@type,@value,@option_ids);" +
                            "SELECT LAST_INSERT_ID();" +
                            "COMMIT;";
                        MySqlCommand cmd = new(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("product_id", _product_id));
                        cmd.Parameters.Add(new MySqlParameter("attribute_id", item.attribute_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", item.sku));
                        cmd.Parameters.Add(new MySqlParameter("type", (int)item.type));
                        cmd.Parameters.Add(new MySqlParameter("value", item.value));
                        cmd.Parameters.Add(new MySqlParameter("option_ids", item.option_ids));
                        if (state != System.Data.ConnectionState.Open) await OpenConnection();
                        val += (ulong)await cmd.ExecuteScalarAsync();
                        if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    }
                }
                return val > 0;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Brand
        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Brand ID</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Brand?> GetBrand(int _customer_id, int _id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM brands " +
                    "WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Brand? b = null;
                    if (await dataReader.ReadAsync()) {
                        b = new Brand();
                        b.id = Convert.ToInt32(dataReader["id"].ToString());
                        b.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                        b.brand_name = dataReader["brand_name"].ToString();
                        b.status = dataReader["status"] is not null ? dataReader["status"].ToString() == "1" ? true : false : false;
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return b;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the default brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<Brand?> GetDefaultBrand(int _customer_id) {
            try {
                var test = await GetProductSettings(_customer_id);
                if (test is null) {
                    OnError("GetDefaultBrand: Product Settings Not Found for Customer ID: " + _customer_id);
                    return null;
                }
                var default_brand = await GetBrandByName(_customer_id, test.default_brand);
                if (default_brand is null) {
                    var inserted_default_brand = await InsertBrand(_customer_id, new Brand() { customer_id = _customer_id, brand_name = Helper.global.product.default_brand, status = true });
                    if (inserted_default_brand is not null) {
                        return inserted_default_brand;
                    }
                    else {
                        OnError("GetDefaultBrand: Default Brand Not Found for Customer ID: " + _customer_id);
                        return null;
                    }
                }
                else {
                    return default_brand;
                }
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brands from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Brand>?> GetBrands(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM brands WHERE customer_id=@customer_id;";
                    List<Brand> list = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        Brand b = new Brand {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            brand_name = dataReader["brand_name"].ToString(),
                            status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                        };
                        list.Add(b);
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
        /// Gets the brands from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Brand>?> GetBrands(int _customer_id, ApiFilter _filters) {
            try {
                List<Brand> list = [];
                string _query = "SELECT * FROM brands WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Brand));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                while (await dataReader.ReadAsync()) {
                    Brand b = new Brand {
                        id = Convert.ToInt32(dataReader["id"].ToString()),
                        customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                        brand_name = dataReader["brand_name"].ToString(),
                        status = Convert.ToBoolean(Convert.ToInt32(dataReader["status"].ToString())),
                    };
                    list.Add(b);
                }
                await dataReader.CloseAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                return list;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Brand?> GetBrand(int _customer_id, string _sku) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT b.id AS 'BID', b.brand_name,b.status AS 'brand_status',b.customer_id AS 'customer' FROM products_ext AS pex INNER JOIN brands AS b ON pex.brand_id=b.id " +
                    "WHERE pex.sku=@sku AND b.customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Brand? b = null;
                    if (await dataReader.ReadAsync()) {
                        b = new Brand();
                        b.id = Convert.ToInt32(dataReader["BID"].ToString());
                        b.customer_id = Convert.ToInt32(dataReader["customer"].ToString());
                        b.brand_name = dataReader["brand_name"].ToString();
                        b.status = dataReader["brand_status"] is not null ? dataReader["brand_status"].ToString() == "1" ? true : false : false;
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return b;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_name">Brand Name</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Brand?> GetBrandByName(int _customer_id, string _name) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM brands " +
                    "WHERE LOWER(brand_name)=LOWER(@brand_name) AND customer_id=@customer_id";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("brand_name", _name));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Brand? b = null;
                    if (await dataReader.ReadAsync()) {
                        b = new Brand();
                        b.id = Convert.ToInt32(dataReader["id"].ToString());
                        b.customer_id = Convert.ToInt32(dataReader["customer_id"].ToString());
                        b.brand_name = dataReader["brand_name"].ToString();
                        b.status = dataReader["status"] is not null ? dataReader["status"].ToString() == "1" ? true : false : false;
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return b;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the brand to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand">Brand</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Brand?> InsertBrand(int _customer_id, Brand _brand) {
            try {
                object val;
                var existed_brand = await GetBrandByName(_customer_id, _brand.brand_name);
                if (existed_brand is not null) {
                    return existed_brand;
                }
                string _query = "START TRANSACTION;" +
                    "INSERT INTO brands (customer_id,brand_name,status) VALUES (@customer_id,@brand_name,@status);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("brand_name", _brand.brand_name));
                cmd.Parameters.Add(new MySqlParameter("status", _brand.status));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int inserted_id)) {
                        return await GetBrand(_customer_id, inserted_id);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the brand in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand">Brand</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<Brand?> UpdateBrand(int _customer_id, Brand _brand) {
            try {
                var existed_brand = await GetBrandByName(_customer_id, _brand.brand_name);
                if (existed_brand is not null && existed_brand.id != _brand.id) {
                    return null;
                }
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "UPDATE brands SET brand_name=@brand_name,status=@status " +
                        "WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _brand.id));
                    cmd.Parameters.Add(new MySqlParameter("brand_name", _brand.brand_name));
                    cmd.Parameters.Add(new MySqlParameter("status", _brand.status));
                    int val = await cmd.ExecuteNonQueryAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    if (val > 0)
                        return _brand;
                    else return null;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the brand from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_brand_id">Brand ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteBrand(int _customer_id, int _brand_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "DELETE FROM brands WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _brand_id));
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
        /// Gets brand count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public async Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters) {
            try {
                string _query = "SELECT COUNT(*) FROM brands WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Brand), true);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                _ = int.TryParse((await cmd.ExecuteScalarAsync())?.ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #region Category
        /// <summary>
        /// Gets the root category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_parent_id">Send '1' for system_category_root_id</param>
        /// <returns>No data and Error returns 'null'</returns>
        public async Task<Category?> GetRootCategory(int _customer_id, int _parent_id = 1) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM categories WHERE parent_id=@parent_id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("parent_id", _parent_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Category? c = null;
                    if (await dataReader.ReadAsync()) {
                        c = new Category {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] is not null ? (dataReader["is_active"].ToString() == "1") : false,
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return c;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the product categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_sku">SKU</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Category>?> GetProductCategories(int _customer_id, string _sku) {
            try {
                List<Category> categories = [];
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string? category_ids = string.Empty;
                    {
                        string _query = "SELECT category_ids FROM products_ext " +
                          "WHERE customer_id=@customer_id AND sku=@sku;";
                        MySqlCommand cmd = new(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        cmd.Parameters.Add(new MySqlParameter("sku", _sku));
                        category_ids = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (category_ids is not null) {
                        string _query = string.Format("SELECT * FROM categories " +
                            "WHERE customer_id=@customer_id AND id IN ({0});", category_ids);
                        MySqlCommand cmd = new(_query, connection);
                        cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                        MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                        while (await dataReader.ReadAsync()) {
                            Category c = new() {
                                id = Convert.ToInt32(dataReader["id"].ToString()),
                                customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                                parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                                category_name = dataReader["category_name"].ToString(),
                                is_active = dataReader["is_active"] is not null ? (dataReader["is_active"].ToString() == "1") : false,
                                source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                            };
                            categories.Add(c);
                        }
                        await dataReader.CloseAsync();
                        if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    }

                    return categories;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Category>?> GetCategories(int _customer_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM categories " +
                    "WHERE customer_id=@customer_id;";
                    List<Category> categories = [];
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        Category c = new() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] is not null ? dataReader["is_active"].ToString() == "1" ? true : false : false,
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                        categories.Add(c);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return categories;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the categories from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>[Error] returns 'null'</returns>
        public async Task<List<Category>?> GetCategories(int _customer_id, ApiFilter _filters) {
            try {
                List<Category> categories = [];
                string _query = "SELECT * FROM categories WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Category));
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    while (await dataReader.ReadAsync()) {
                        Category c = new() {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] is not null ? (dataReader["is_active"].ToString() == "1") : false,
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                        categories.Add(c);
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return categories;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Category ID</param>
        /// <returns>No data and Error returns 'null'</returns>
        public async Task<Category?> GetCategory(int _customer_id, int _id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM categories " +
                    "WHERE id=@id AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _id));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Category? c = null;
                    if (await dataReader.ReadAsync()) {
                        c = new Category {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] is not null ? (dataReader["is_active"].ToString() == "1") : false,
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return c;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Category Name</param>
        /// <returns>No data and Error returns 'null'</returns>
        public async Task<Category?> GetCategoryByName(int _customer_id, string _category_name) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "SELECT * FROM categories " +
                    "WHERE category_name=@category_name AND customer_id=@customer_id";
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("category_name", _category_name));
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                    Category? c = null;
                    if (await dataReader.ReadAsync()) {
                        c = new Category {
                            id = Convert.ToInt32(dataReader["id"].ToString()),
                            customer_id = Convert.ToInt32(dataReader["customer_id"].ToString()),
                            parent_id = Convert.ToInt32(dataReader["parent_id"].ToString()),
                            category_name = dataReader["category_name"].ToString(),
                            is_active = dataReader["is_active"] is not null ? (dataReader["is_active"].ToString() == "1") : false,
                            source_category_id = Convert.ToInt32(dataReader["source_category_id"].ToString())
                        };
                    }
                    await dataReader.CloseAsync();
                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    return c;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Inserts the category to the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category</param>
        /// <returns>[No data] or [Error] returns 'null'</returns>
        public async Task<Category?> InsertCategory(int _customer_id, Category _category) {
            try {
                var existed_category = await GetCategoryByName(_customer_id, _category.category_name);
                if (existed_category is not null && existed_category.parent_id == _category.parent_id) {
                    return null;
                }
                string _query = "START TRANSACTION;" +
                    "INSERT INTO categories (customer_id,parent_id,category_name,is_active,source_category_id) VALUES (@customer_id,@parent_id,@category_name,@is_active,@source_category_id);" +
                    "SELECT LAST_INSERT_ID();" +
                    "COMMIT;";
                MySqlCommand cmd = new MySqlCommand(_query, connection);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                cmd.Parameters.Add(new MySqlParameter("parent_id", _category.parent_id));
                cmd.Parameters.Add(new MySqlParameter("category_name", _category.category_name));
                cmd.Parameters.Add(new MySqlParameter("is_active", _category.is_active));
                cmd.Parameters.Add(new MySqlParameter("source_category_id", _category.source_category_id));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                object? val = await cmd.ExecuteScalarAsync();
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                if (val is not null) {
                    if (int.TryParse(val.ToString(), out int inserted_id)) {
                        return await GetCategory(_customer_id, inserted_id);
                    }
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the category in the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category</param>
        /// <returns>[No change] or [Error] returns 'null'</returns>
        public async Task<Category?> UpdateCategory(int _customer_id, Category _category) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    string _query = "UPDATE categories SET category_name=@category_name,is_active=@is_active,parent_id=@parent_id,source_category_id=@source_category_id " +
                            "WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _category.id));
                    cmd.Parameters.Add(new MySqlParameter("parent_id", _category.parent_id));
                    cmd.Parameters.Add(new MySqlParameter("category_name", _category.category_name));
                    cmd.Parameters.Add(new MySqlParameter("is_active", _category.is_active));
                    cmd.Parameters.Add(new MySqlParameter("source_category_id", _category.source_category_id));
                    int val = await cmd.ExecuteNonQueryAsync();

                    if (state == System.Data.ConnectionState.Open) await CloseConnection();
                    if (val > 0)
                        return _category;
                    else return null;
                }
                return null;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the category from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>[No change] or [Error] returns 'false'</returns>
        public async Task<bool> DeleteCategory(int _customer_id, int _category_id) {
            try {
                if (this.state != System.Data.ConnectionState.Open && await OpenConnection()) {
                    int val = 0;
                    string _query = "DELETE FROM categories WHERE id=@id AND customer_id=@customer_id;";
                    MySqlCommand cmd = new(_query, connection);
                    cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                    cmd.Parameters.Add(new MySqlParameter("id", _category_id));
                    val = await cmd.ExecuteNonQueryAsync();
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
        /// Gets category count from the database
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filters</param>
        /// <returns>Error returns 'int:-1'</returns>
        public async Task<int> GetCategoryCount(int _customer_id, ApiFilter _filters) {
            try {
                string _query = "SELECT COUNT(*) FROM categories WHERE customer_id=@customer_id";
                MySqlCommand cmd = new() { Connection = connection };
                cmd.CommandText = DbHelperBase.BuildDBQuery(_filters, ref _query, ref cmd, typeof(Category), true);
                cmd.Parameters.Add(new MySqlParameter("customer_id", _customer_id));
                if (state != System.Data.ConnectionState.Open) await OpenConnection();
                _ = int.TryParse((await cmd.ExecuteScalarAsync())?.ToString(), out int total_count);
                if (state == System.Data.ConnectionState.Open) await CloseConnection();
                return total_count;
            }
            catch (Exception ex) {
                OnError(ex.ToString());
                return -1;
            }
        }
        #endregion

        #endregion
    }
}
