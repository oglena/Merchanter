using Merchanter;
using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MerchanterHelpers;
using MerchanterHelpers.Classes;
using ShipmentHelpers;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MerchanterServer;
internal class MainLoop {
    #region Constant Variables
    public const string newline = "\r\n";
    public const string not_available = "N/A";
    #endregion

    #region Properties
    private string thread_id { get; set; }
    private Customer customer { get; set; }
    private DbHelper db_helper { get; set; }
    private string? product_main_source { get; set; } = null;
    private string? order_main_target { get; set; } = null;
    private string[] other_product_sources { get; set; } = [];
    private string[] product_targets { get; set; } = [];
    private string[] order_sources { get; set; } = [];
    private string[] available_shipments { get; set; } = [];
    #endregion

    #region Variables
    private bool xml_has_error;
    private CurrencyRates rates = new();
    private List<Product> live_products = [];
    private List<ProductTarget> product_target_relation = [];
    private List<CategoryTarget> category_target_relation = [];
    private List<ProductImage> product_images = [];
    private List<ProductAttribute> product_attributes = [];
    private List<Order> live_orders = [];
    private List<XProduct> xproducts = [];
    private List<Brand> brands = [];
    private Brand default_brand = new();
    private List<Category> categories = [];
    private Category? root_category = new();
    private List<Product> products = [];
    private List<Order> orders = [];
    private List<Invoice> invoices = [];
    private List<Notification> notifications = [];
    #endregion

    public MainLoop(string _thread_id, Customer _customer, DbHelper _db_helper) {
        thread_id = _thread_id;
        customer = _customer;
        db_helper = _db_helper;

        #region Integrations
        product_main_source = Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.PRODUCT &&
            x.work?.direction == Work.WorkDirection.MAIN_SOURCE && x.work.status && x.is_active
        ).FirstOrDefault()?.work.name;
        order_main_target = Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.ORDER &&
            x.work?.direction == Work.WorkDirection.MAIN_TARGET && x.work.status && x.is_active
        ).FirstOrDefault()?.work.name;
        other_product_sources = [.. Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.PRODUCT &&
            x.work?.direction == Work.WorkDirection.SOURCE && x.work.status && x.is_active
        ).Select(x => x.work.name)];
        product_targets = [.. Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.PRODUCT &&
            x.work?.direction == Work.WorkDirection.TARGET && x.work.status && x.is_active
        ).Select(x => x.work.name)];
        order_sources = [.. Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.ORDER &&
            x.work?.direction == Work.WorkDirection.SOURCE && x.work.status && x.is_active
        ).Select(x => x.work.name)];
        available_shipments = [.. Helper.global.integrations.Where(x => x.work?.type == Work.WorkType.SHIPMENT &&
            x.work?.direction == Work.WorkDirection.BOTH && x.work.status && x.is_active
        ).Select(x => x.work.name)];
        Console.Write("ProductMainSource:" + product_main_source + "; OrderMainTarget:" + order_main_target);
        Console.WriteLine("; OtherProductSources:" + string.Join(",", other_product_sources));
        Console.Write("ProductTargets:" + string.Join(",", product_targets));
        Console.Write("; OrderSources:" + string.Join(",", order_sources));
        Console.Write("; AvailableShipments:" + string.Join(",", available_shipments) + Environment.NewLine);
        Console.WriteLine("All settings and integrations initiated...");
        #endregion
    }

    /// <summary>
    /// Main Work Loop for Work Sources
    /// </summary>
    /// <returns>Main thread health status</returns>
    public bool DoWork() {
        bool health = true;

        #region Xml LOOP
        if (other_product_sources?.Length > 0) {
            #region Currency XProducts from MerchanterDB
            xproducts = db_helper.GetXProducts(customer.customer_id);
            #endregion
            if (customer.xml_sync_status && !customer.is_xmlsync_working) {
                db_helper.xml_clone.SetXmlSyncWorking(customer.customer_id, true);
                if (health) { this.XmlLoop(Helper.global); }
                //Task task = Task.Run(() => this.XmlLoop(Helper.global));
            }
        }
        #endregion

        #region Product LOOP
        if (customer.product_sync_status && !customer.is_productsync_working) {
            db_helper.SetProductSyncWorking(customer.customer_id, true);

            #region Current Catalog from MerchanterDB
            default_brand = db_helper.GetDefaultBrand(customer.customer_id);
            root_category = db_helper.GetRootCategory(customer.customer_id);
            Console.WriteLine("DefaultBrand:" + default_brand.brand_name + ", RootCategory:" + root_category?.category_name);
            if (default_brand == null || root_category == null) { health = false; }

            products = db_helper.GetProducts(customer.customer_id, out brands, out categories, out product_attributes, out product_images);
            Console.WriteLine("Products:" + products.Count + "; Brands:" + brands.Count + "; Categories:" + categories.Count);
            Console.WriteLine("Attributes:" + product_attributes.Count + "; Images:" + product_images.Count);
            product_target_relation = db_helper.GetProductTargets(customer.customer_id);
            Console.Write("ProductTargets:" + product_target_relation.Count);
            category_target_relation = db_helper.GetCategoryTargets(customer.customer_id);
            Console.Write("; CategoryTargets:" + category_target_relation.Count + Environment.NewLine);
            if (products == null) { health = false; }
            #endregion

            if (health) { this.ProductLoop(out health); }

            if (health) { this.ProductSourceLoop(out health); }

            if (health) { this.ProductSync(out health); }

            if (customer.product_sync_status) {
                db_helper.SetProductSyncDate(customer.customer_id);
                db_helper.SetProductSyncWorking(customer.customer_id, false);
            }
        }
        #endregion

        #region Order LOOP
        if (customer.order_sync_status && !customer.is_ordersync_working) {
            db_helper.SetOrderSyncWorking(customer.customer_id, true);

            #region Current Orders from MerchanterDB
            orders = db_helper.GetOrders(customer.customer_id, OrderStatus.GetProcessEnabledCodes());
            #endregion

            if (health) { this.OrderLoop(out health); }

            if (health) { this.OrderSync(out health); }

            if (customer.order_sync_status) {
                db_helper.SetOrderSyncDate(customer.customer_id);
                db_helper.SetOrderSyncWorking(customer.customer_id, false);
            }
        }
        #endregion

        #region Invoices LOOP
        if (customer.invoice_sync_status && !customer.is_invoicesync_working) {
            db_helper.invoice_clone.SetInvoiceSyncWorking(customer.customer_id, true);
            Task task = Task.Run(this.InvoiceLoop);
        }
        #endregion

        //INFO: Need to be last executed 
        #region Notification LOOP
        notifications = db_helper.notification_clone.GetNotifications(customer.customer_id, false);
        if (customer.notification_sync_status && !customer.is_notificationsync_working) {
            db_helper.notification_clone.SetNotificationSyncWorking(customer.customer_id, true);
            //Task task = Task.Run( this.NotificationLoop );
            if (health) { this.NotificationLoop(); }
        }
        #endregion

        return health;
    }

    #region Individual Threads - XML, Invoice, Notification
    private void XmlLoop(SettingsMerchanter _global) {
        List<Notification> notifications = [];
        List<XProduct> live_xproducts = [];
        List<Product>? xml_enabled_products = db_helper.xml_clone.GetXMLEnabledProducts(customer.customer_id);

        try {
            PrintConsole("XML sync started.");
            string info;

            #region Inject XML Sources for QP Bilisim
            if (customer.customer_id == 1 && xml_enabled_products != null) {
                try {
                    PrintConsole("Injecting XML Sources for " + customer.user_name);
                    var xml_sources_1 = Helper.GetProductAttribute(_global.magento.xml_sources_attribute_code);
                    var xml_enabled_products_1 = Helper.SearchProductByAttribute(_global.magento.is_xml_enabled_attribute_code, "1");
                    if (xml_enabled_products_1 != null) {
                        foreach (var item in xml_enabled_products_1) {
                            string? raw_sources_1 = item.custom_attributes.Where(x => x.attribute_code == _global.magento.xml_sources_attribute_code)?.First().value?.ToString();
                            if (raw_sources_1 != null && raw_sources_1.Length > 0) {
                                var live_sources = xml_sources_1?.options.Where(x => raw_sources_1.Contains(x.value)).Where(x => !string.IsNullOrWhiteSpace(x.value)).ToList();
                                if (live_sources?.Count > 0) {
                                    var selected_xml_source_barcode = item.custom_attributes.Where(x => x.attribute_code == _global.magento.barcode_attribute_code).FirstOrDefault();
                                    if (selected_xml_source_barcode != null) {
                                        if (selected_xml_source_barcode.value != null) {
                                            var x_enabled_product = xml_enabled_products.Where(x => x.barcode == selected_xml_source_barcode.value.ToString()).FirstOrDefault();
                                            if (x_enabled_product == null) {
                                                db_helper.xml_clone.UpdateXMLStatusByProductBarcode(customer.customer_id, selected_xml_source_barcode.value.ToString(), true, live_sources.Select(x => x.label).ToArray());
                                            }
                                            else {
                                                if (x_enabled_product.extension != null && string.Join(",", live_sources.Select(x => x.label).ToArray()).Trim() != string.Join(",", x_enabled_product.extension.xml_sources).Trim()) {
                                                    db_helper.xml_clone.UpdateXMLStatusByProductBarcode(customer.customer_id, selected_xml_source_barcode.value.ToString(), true, live_sources.Select(x => x.label).ToArray());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var item in xml_enabled_products) {
                            var xproduct = xml_enabled_products.Where(x => x.barcode == item.barcode).FirstOrDefault();
                            if (xproduct == null) {
                                db_helper.xml_clone.UpdateXMLStatusByProductBarcode(customer.customer_id, item.barcode, false, null);

                                #region Notification of XML_PRODUCT_REMOVED_BY_USER
                                db_helper.xml_clone.LogToServer(thread_id, "xml_product_removed_by_user", item.barcode, customer.customer_id, "xml");
                                notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_PRODUCT_REMOVED_BY_USER, xproduct_barcode = item.barcode });
                                #endregion
                            }
                        }

                        xml_enabled_products = db_helper.xml_clone.GetXMLEnabledProducts(customer.customer_id);
                    }
                    PrintConsole("Injecting XML Sources for " + customer.user_name + " Done.");
                }
                catch (Exception _ex) {
                    db_helper.xml_clone.LogToServer(thread_id, "error", _ex.ToString(), customer.customer_id, "xml");
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SYNC_ERROR, notification_content = _ex.ToString() }]);
                }
            }
            #endregion

            if (other_product_sources.Contains(Constants.FSP)) {
                FSP fsp = new(_global.xml_fsp_url);
                var fsp_products = fsp.GetProducts(out info);
                PrintConsole(info + " " + Constants.FSP);
                if (fsp_products != null && fsp_products.Product.Length > 0) {
                    foreach (var item in fsp_products.Product) {
                        XProduct xp = new() {
                            customer_id = customer.customer_id,
                            xml_source = Constants.FSP,
                            barcode = item.Barcode.Trim(),
                            source_brand = item.Brand.Trim(),
                            source_sku = item.Product_code.Trim(),
                            source_product_group = item.category,
                            qty = item.Stock != null ? item.Stock.Value : 0,
                            price1 = item.TESKfiyat,
                            price2 = item.BAYİfiyat,
                            currency = item.CurrencyType.Trim(),
                            is_active = false
                        };
                        if (!string.IsNullOrWhiteSpace(xp.barcode)) {
                            if (xml_enabled_products?.Where(x => x.barcode == xp.barcode && x.extension.is_xml_enabled && x.extension.xml_sources.Contains(Constants.FSP)).FirstOrDefault() != null) {
                                xp.is_active = true;
                            }
                            var tempx = live_xproducts.Where(x => x.xml_source == Constants.FSP).Where(x => x.barcode == xp.barcode).FirstOrDefault();
                            if (tempx == null)
                                live_xproducts.Add(xp);
                        }
                    }
                }
                else {
                    #region Notify - XML_SOURCE_FAILED
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SOURCE_FAILED, notification_content = Constants.FSP }]);
                    #endregion

                    db_helper.xml_clone.LogToServer(thread_id, "source_error", Constants.FSP, customer.customer_id, "xml");
                    PrintConsole(Constants.FSP + " xproducts load failed");
                    xml_has_error = true;
                    return;
                }
            }

            if (other_product_sources.Contains(Constants.PENTA)) {
                PENTA penta = new(_global.xml_penta_base_url, _global.xml_penta_customerid);
                var penta_products = penta.GetProducts(out info);
                PrintConsole(info + " " + Constants.PENTA);
                if (penta_products != null && penta_products.Stok != null && penta_products.Stok.Length > 0) {
                    foreach (var item in penta_products.Stok) {
                        XProduct xp = new() {
                            customer_id = customer.customer_id,
                            xml_source = Constants.PENTA,
                            barcode = !string.IsNullOrWhiteSpace(item.UreticiBarkodNo) ? ((item.UreticiBarkodNo[0] == '0') ? item.UreticiBarkodNo.Remove(0, 1).Trim() : item.UreticiBarkodNo.Trim()) : string.Empty,
                            source_brand = !string.IsNullOrWhiteSpace(item.MarkaIsim) ? item.MarkaIsim.Trim() : "MARKASIZ",
                            source_sku = item.Kod.ToString().Trim(),
                            source_product_group = item.AnaGrup_Ad,
                            qty = item.Miktar != null ? ((item.Miktar != "Stoksuz") ? int.Parse(item.Miktar.Replace("+", string.Empty)) : 999) : 0,
                            price1 = item.Fiyat_SKullanici,
                            price2 = item.Fiyat_Ozel,
                            currency = item.Doviz?.Trim() ?? string.Empty,
                            is_active = false
                        };

                        if (!string.IsNullOrWhiteSpace(xp.barcode)) {
                            if (xml_enabled_products?.Where(x => x.barcode == xp.barcode && x.extension.is_xml_enabled && x.extension.xml_sources.Contains(Constants.PENTA)).FirstOrDefault() != null) {
                                xp.is_active = true;
                            }
                            var tempx = live_xproducts.Where(x => x.xml_source == Constants.PENTA).Where(x => x.barcode == xp.barcode).FirstOrDefault();
                            if (tempx == null)
                                live_xproducts.Add(xp);
                        }
                    }
                }
                else {
                    #region Notify - XML_SOURCE_FAILED
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SOURCE_FAILED, notification_content = Constants.PENTA }]);
                    #endregion

                    db_helper.xml_clone.LogToServer(thread_id, "source_error", Constants.PENTA, customer.customer_id, "xml");
                    PrintConsole(Constants.PENTA + " xproducts load failed");
                    xml_has_error = true;
                    return;
                }
            }

            if (other_product_sources.Contains(Constants.KOYUNCU)) {
                KOYUNCU koyuncu = new(_global.xml_koyuncu_url);
                var koyuncu_products = koyuncu.GetProducts(out info);
                PrintConsole(info + " " + Constants.KOYUNCU);
                if (koyuncu_products != null && koyuncu_products.ProductDto.Length > 0) {
                    foreach (var item in koyuncu_products.ProductDto) {
                        XProduct xp = new() {
                            customer_id = customer.customer_id,
                            xml_source = Constants.KOYUNCU,
                            barcode = (!string.IsNullOrWhiteSpace(item.EANCode)) ? ((item.EANCode[0] == '0') ? item.EANCode[1..].Trim() : item.EANCode.Trim()) : string.Empty,
                            source_brand = item.BrandName.Trim(),
                            source_sku = item.ProductCode.Trim(),
                            source_product_group = item.CategoryCode.ToString(),
                            qty = item.Stock != null ? int.Parse(item.Stock.Replace("+", string.Empty)) : 0,
                            price1 = item.DefaultPrice,
                            price2 = item.CustomerPrice,
                            currency = item.Currency.Trim(),
                            is_active = false
                        };
                        if (!string.IsNullOrWhiteSpace(xp.barcode)) {
                            if (xml_enabled_products?.Where(x => x.barcode == xp.barcode && x.extension.is_xml_enabled && x.extension.xml_sources.Contains(Constants.KOYUNCU)).FirstOrDefault() != null) {
                                xp.is_active = true;
                            }
                            var tempx = live_xproducts.Where(x => x.xml_source == Constants.KOYUNCU).Where(x => x.barcode == xp.barcode).FirstOrDefault();
                            if (tempx == null)
                                live_xproducts.Add(xp);
                        }
                    }
                }
                else {
                    #region Notify - XML_SOURCE_FAILED
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SOURCE_FAILED, notification_content = Constants.KOYUNCU }]);
                    #endregion

                    db_helper.xml_clone.LogToServer(thread_id, "source_error", Constants.KOYUNCU, customer.customer_id, "xml");
                    PrintConsole(Constants.KOYUNCU + " xproducts load failed");
                    xml_has_error = true;
                    return;
                }
            }

            if (other_product_sources.Contains(Constants.OKSID)) {
                OKSID oksid = new(_global.xml_oksid_url);
                var oksid_products = oksid.GetProducts(out info);
                PrintConsole(info + " " + Constants.OKSID);
                if (oksid_products != null && oksid_products.Stok.Length > 0) {
                    foreach (var item in oksid_products.Stok) {
                        XProduct xp = new() {
                            customer_id = customer.customer_id,
                            xml_source = Constants.OKSID,
                            barcode = item.barkod.Trim(),
                            source_brand = item.Marka_Ismi.Trim(),
                            source_sku = item.Kod.Trim(),
                            source_product_group = item.Kategori_Id.ToString(),
                            qty = item.Miktar,
                            price1 = item.Fiat_SKullanici,
                            price2 = item.Fiat_Bayi,
                            currency = item.Doviz_Cinsi.Trim(),
                            is_active = false
                        };
                        if (!string.IsNullOrWhiteSpace(xp.barcode)) {
                            if (xml_enabled_products?.Where(x => x.barcode == xp.barcode && x.extension.is_xml_enabled && x.extension.xml_sources.Contains(Constants.OKSID)).FirstOrDefault() != null) {
                                xp.is_active = true;
                            }
                            var tempx = live_xproducts.Where(x => x.xml_source == Constants.OKSID).Where(x => x.barcode == xp.barcode).FirstOrDefault();
                            if (tempx == null)
                                live_xproducts.Add(xp);
                        }
                    }
                }
                else {
                    #region Notify - XML_SOURCE_FAILED
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SOURCE_FAILED, notification_content = Constants.OKSID }]);
                    #endregion

                    db_helper.xml_clone.LogToServer(thread_id, "source_error", Constants.OKSID, customer.customer_id, "xml");
                    PrintConsole(Constants.OKSID + " xproducts load failed");
                    xml_has_error = true;
                    return;
                }
            }

            if (other_product_sources.Contains(Constants.BOGAZICI)) {
                BOGAZICI bogazici = new(_global.xml_bogazici_bayikodu, _global.xml_bogazici_email, _global.xml_bogazici_sifre);
                var bogazici_products = bogazici.getBogaziciProducts();
                if (bogazici_products != null && bogazici_products.Count > 0) {
                    foreach (var item in bogazici_products) {
                        XProduct xp = new() {
                            customer_id = customer.customer_id,
                            xml_source = Constants.BOGAZICI,
                            barcode = (!string.IsNullOrWhiteSpace(item.EAN1)) ? ((item.EAN1[0] == '0') ? item.EAN1[1..].Trim() : item.EAN1.Trim()) : string.Empty,
                            source_brand = item.MARKA.Trim(),
                            source_sku = item.PRODUCERCODE.Trim(),
                            source_product_group = item.KATEGORI,
                            qty = (item.STOK != null) ? int.Parse(item.STOK.Replace("+", string.Empty)) : 0,
                            price1 = Convert.ToDecimal(item.SKFIYAT),
                            price2 = Convert.ToDecimal(item.BIRIMFIYAT),
                            currency = item.BIRIMDOVIZ.Trim(),
                            is_active = false
                        };
                        if (!string.IsNullOrWhiteSpace(xp.barcode)) {
                            if (xml_enabled_products?.Where(x => x.barcode == xp.barcode && x.extension.is_xml_enabled && x.extension.xml_sources.Contains(Constants.BOGAZICI)).FirstOrDefault() != null) {
                                xp.is_active = true;
                            }
                            var tempx = live_xproducts.Where(x => x.xml_source == Constants.BOGAZICI).Where(x => x.barcode == xp.barcode).FirstOrDefault();
                            if (tempx == null) {
                                live_xproducts.Add(xp);
                            }
                        }
                    }
                    PrintConsole("Success " + Constants.BOGAZICI);
                }
                else {
                    #region Notify - XML_SOURCE_FAILED
                    db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SOURCE_FAILED, notification_content = Constants.BOGAZICI }]);
                    #endregion

                    db_helper.xml_clone.LogToServer(thread_id, "source_error", Constants.BOGAZICI, customer.customer_id, "xml");
                    PrintConsole(Constants.BOGAZICI + " xproducts load failed");
                    xml_has_error = true;
                    return;
                }
            }

            PrintConsole(live_xproducts.Count + " live xproducts loaded.");

            if (xproducts != null) {
                foreach (var item in xproducts) {
                    var need_to_delete_xp = live_xproducts.Where(x => x.xml_source == item.xml_source).Where(x => x.barcode == item.barcode).FirstOrDefault();
                    if (need_to_delete_xp == null) {
                        db_helper.xml_clone.DeleteXProduct(customer.customer_id, item.id);
                        PrintConsole(item.barcode + " source=" + item.xml_source + " " + " xproduct removed.");

                        #region Notify - XML_PRODUCT_REMOVED
                        db_helper.xml_clone.LogToServer(thread_id, "xml_product_removed", item.xml_source + ":" + item.barcode + ": " + item.qty, customer.customer_id, "xml");
                        if (xml_enabled_products?.Where(x => x.barcode == item.barcode).ToList().Count == 0) {
                            notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_PRODUCT_REMOVED, xproduct_barcode = item.barcode, notification_content = item.xml_source });
                        }
                        #endregion
                    }
                }

                foreach (var item in live_xproducts) {
                    var tempx = xproducts.Where(x => x.xml_source == item.xml_source).Where(x => x.barcode == item.barcode).FirstOrDefault();
                    if (tempx != null) {
                        item.id = tempx.id;
                        item.is_active = tempx.is_active;
                        item.is_infosent = tempx.is_infosent;

                        #region Notify - XML_PRICE_CHANGED
                        if (xml_enabled_products?.Where(x => x.barcode == item.barcode).ToList().Count > 0) {
                            if (item.price2 != tempx.price2) {
                                db_helper.xml_clone.LogToServer(thread_id, "xml_price_changed", item.xml_source + ":" + item.barcode + "=>" + item.price2 + "|" + tempx.price2, customer.customer_id, "xml");
                                notifications.Add(new Notification() {
                                    customer_id = customer.customer_id,
                                    type = Notification.NotificationTypes.XML_PRICE_CHANGED,
                                    xproduct_barcode = item.barcode,
                                    notification_content = item.xml_source + "=" + item.price2 + "|" + tempx.price2
                                });
                            }
                        }
                        #endregion

                        #region Notify - XML_QTY_CHANGED
                        if (xml_enabled_products?.Where(x => x.barcode == item.barcode).ToList().Count > 0) {
                            if (item.qty != tempx.qty) {
                                db_helper.xml_clone.LogToServer(thread_id, "xml_qty_changed", item.xml_source + ":" + item.barcode + "=>" + item.qty + "|" + tempx.qty, customer.customer_id, "xml");
                                notifications.Add(new Notification() {
                                    customer_id = customer.customer_id,
                                    type = Notification.NotificationTypes.XML_QTY_CHANGED,
                                    xproduct_barcode = item.barcode,
                                    notification_content = item.xml_source + "=" + item.qty + "|" + tempx.qty
                                });
                            }
                        }
                        #endregion
                    }
                    else {
                        #region Notify - XML_PRODUCT_ADDED
                        if (xml_enabled_products?.Where(x => x.barcode == item.barcode).ToList().Count > 0) {
                            db_helper.xml_clone.LogToServer(thread_id, "xml_product_added", item.xml_source + ":" + item.barcode + ": " + item.qty, customer.customer_id, "xml");
                            notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_PRODUCT_ADDED, xproduct_barcode = item.barcode });
                        }
                        #endregion
                    }
                }
            }

            foreach (var item in live_xproducts) {
                if (item.id == 0) {
                    db_helper.xml_clone.InsertXProducts(customer.customer_id, [item]);
                }
                else {
                    db_helper.xml_clone.UpdateXProducts(customer.customer_id, [item]);
                }
            }

            if (notifications.Count > 0) {
                db_helper.xml_clone.InsertNotifications(customer.customer_id, notifications);
            }
        }
        catch (Exception _ex) {
            db_helper.xml_clone.LogToServer(thread_id, "error", _ex.ToString(), customer.customer_id, "xml");
            db_helper.xml_clone.InsertNotifications(customer.customer_id, [new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.XML_SYNC_ERROR, is_notification_sent = true, notification_content = _ex.ToString() }]);
            PrintConsole(xproducts.Count + " xproducts loaded from old cache");
        }
        finally {
            xproducts = db_helper.xml_clone.GetXProducts(customer.customer_id);
            if (customer.xml_sync_status) {
                db_helper.xml_clone.SetXmlSyncDate(customer.customer_id);
                db_helper.xml_clone.SetXmlSyncWorking(customer.customer_id, false);
            }
            if (!xml_has_error)
                PrintConsole(xproducts.Count + " xproducts loaded from new cache");
            else
                PrintConsole(xproducts.Count + " xproducts loaded from old cache");
        }
    }

    private void InvoiceLoop() {
        try {
            List<Notification> notifications = [];
            if (order_main_target != null && order_main_target == Constants.NETSIS) {
                //var fats = NetOpenXHelper.GetNetsisFaturaCountByDate( 3 );
                //var fat = NetOpenXHelper.GetNetsisFatura( "QPB024000004847" );
                //var sip = NetOpenXHelper.GetNetsisSiparis( "000000099108639", false );
                invoices = db_helper.invoice_clone.GetInvoices(customer.customer_id);
                var live_faturas = NetOpenXHelper.GetNetsisFaturas(Helper.global.invoice.daysto_invoicesync, invoices);
                if (invoices != null && live_faturas != null) {
                    foreach (var item in live_faturas) {
                        var selected_invoice = invoices.Where(x => x.invoice_no == item.FATURANO).FirstOrDefault();
                        Invoice invoice = new Invoice() {
                            customer_id = customer.customer_id,
                            order_id = item.EKACK1,
                            order_label = item.EKACK2 ?? "",
                            invoice_no = item.FATURANO,
                            erp_no = item.SIPARISNO,
                            erp_customer_code = item.CARIKODU,
                            erp_customer_group = item.CARIGRUP,
                            gib_fatura_no = item.GIB_FATIRS_NO,
                            order_date = item.TARIH != null ? item.TARIH.Value : DateTime.MinValue,
                            items = new List<InvoiceItem>()
                        };
                        foreach (var kalem in item.KALEMS) {
                            invoice.items.Add(new InvoiceItem() {
                                erp_no = kalem.SIPARISNO,
                                invoice_no = kalem.FATURANO,
                                customer_id = customer.customer_id,
                                sku = kalem.STOKKODU,
                                qty = kalem.MIKTAR,
                                create_date = kalem.TARIH != null ? kalem.TARIH.Value : DateTime.MinValue,
                                serials = [.. kalem.SERILER]
                            });
                        }
                        if (selected_invoice != null) {
                            invoice.id = selected_invoice.id;
                            if (!selected_invoice.is_belge_created) {
                                if (string.IsNullOrWhiteSpace(selected_invoice.gib_fatura_no)) continue;
                                string? fullpath = NetOpenXHelper.GetEbelge(selected_invoice.gib_fatura_no);
                                if (!string.IsNullOrWhiteSpace(fullpath)) {
                                    if (order_sources.Contains(Constants.MAGENTO2)) {
                                        if (selected_invoice.order_label != null && selected_invoice.erp_customer_group != null &&
                                            Helper.global.netsis.fatura_cari_gruplari.Contains(selected_invoice.erp_customer_group.Trim())) {
                                            if (selected_invoice.erp_customer_group == "WEB" || selected_invoice.erp_customer_group == "WEBM2") {
                                                Helper.UploadFileToFtp(Helper.global.erp_invoice_ftp_url, fullpath,
                                                    Helper.global.erp_invoice_ftp_username, Helper.global.erp_invoice_ftp_password);
                                                db_helper.invoice_clone.LogToServer(thread_id, "ftpupload", fullpath + " => " + Helper.global.erp_invoice_ftp_url, customer.customer_id, "invoice");
                                                if (QP_MySQLHelper.QP_UpdateInvoiceNo(selected_invoice.order_label, invoice.gib_fatura_no)) {
                                                    db_helper.invoice_clone.LogToServer(thread_id, "qp_mysqlupdate", selected_invoice.order_label + " => " + invoice.gib_fatura_no, customer.customer_id, "invoice");
                                                }
                                            }
                                        }
                                    }
                                    if (order_sources.Contains(Constants.N11)) {
                                        if (selected_invoice.erp_customer_group == Constants.N11) { }
                                    }
                                    if (order_sources.Contains(Constants.HEPSIBURADA)) {
                                        if (selected_invoice.erp_customer_group == Constants.HEPSIBURADA) { }
                                    }
                                    if (order_sources.Contains(Constants.TRENDYOL)) {
                                        if (selected_invoice.erp_customer_group == Constants.TRENDYOL) { }
                                    }

                                    if (db_helper.invoice_clone.SetInvoiceCreated(customer.customer_id, invoice.invoice_no, fullpath)) { //TODO: belge_url <> local_path different condition
                                        #region Notify - NEW_INVOICE
                                        notifications.Add(new Notification() {
                                            customer_id = customer.customer_id,
                                            type = Notification.NotificationTypes.NEW_INVOICE,
                                            invoice_no = invoice.gib_fatura_no,
                                            order_label = selected_invoice.order_label,
                                            notification_content = Helper.global.erp_invoice_ftp_url + invoice.gib_fatura_no + ".pdf"
                                            ,
                                            is_notification_sent = true
                                        }); //TODO: this gonna change
                                        #endregion
                                        PrintConsole(Constants.NETSIS + " " + invoice.invoice_no + " created at [" + fullpath + "]");
                                        db_helper.invoice_clone.LogToServer(thread_id, "info", selected_invoice.order_label + " => " + invoice.gib_fatura_no, customer.customer_id, "invoice");
                                    }
                                }
                                if (db_helper.invoice_clone.UpdateInvoices(customer.customer_id, [invoice])) {
                                    PrintConsole(Constants.NETSIS + " " + invoice.invoice_no + " updated.");
                                }
                            }
                        }
                        else {
                            if (!string.IsNullOrWhiteSpace(invoice.order_id) && !string.IsNullOrWhiteSpace(invoice.order_label)) {
                                if (db_helper.invoice_clone.InsertInvoices(customer.customer_id, [invoice])) {
                                    PrintConsole(Constants.NETSIS + " " + invoice.invoice_no + " inserted.");
                                }
                            }
                        }
                    }
                }
            }

            if (notifications.Count > 0) {
                db_helper.invoice_clone.InsertNotifications(customer.customer_id, notifications);
            }
        }
        catch (Exception _ex) {
            db_helper.invoice_clone.LogToServer(thread_id, "error", _ex.ToString(), customer.customer_id, "invoice");
        }
        finally {
            if (customer.invoice_sync_status) {
                db_helper.invoice_clone.SetInvoiceSyncDate(customer.customer_id);
                db_helper.invoice_clone.SetInvoiceSyncWorking(customer.customer_id, false);
            }
            PrintConsole("Invoice sync ended.");
        }
    }

    private void NotificationLoop() {
        try {
            if (notifications == null) return;
            if (notifications.Count == 0) return;
            foreach (var item in notifications) {
                Product? selected_product = null;
                List<XProduct>? selected_xproducts = null;
                Order? selected_order = null;
                switch (item.type) {
                    case Notification.NotificationTypes.GENERAL:
                        break;

                    case Notification.NotificationTypes.NEW_ORDER:
                        selected_order = orders?.Where(x => x.order_label == item.order_label).FirstOrDefault();
                        if (selected_order != null) {
                            string mail_title = string.Format("NEW ORDER {0}", item.order_label);
                            string mail_body = string.Empty;

                            item.is_notification_sent = true;
                            item.notification_content = mail_title;
                            if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                db_helper.notification_clone.LogToServer(thread_id, "new_order", mail_title + " => " + mail_body, customer.customer_id, "notification");
                            }
                        }
                        break;

                    case Notification.NotificationTypes.OUT_OF_STOCK_PRODUCT_SOLD:
                        selected_product = products?.Where(x => x.sku == item.product_sku).FirstOrDefault();
                        selected_xproducts = xproducts?.Where(x => x.barcode == item.xproduct_barcode).ToList();
                        selected_order = orders?.Where(x => x.order_label == item.order_label).FirstOrDefault();

                        if (selected_product != null && selected_order != null) {
                            string mail_title = string.Format("{0} - OUT OF STOCK PRODUCT SOLD", item.product_sku);
                            string mail_body = string.Format("<strong>Product:</strong> {0}<br>" +
                                   "<strong>Sku:</strong> {1}<br>" +
                                   "<strong>Barcode:</strong> {2}<br>" +
                                   "<strong>Brand:</strong> {3}<br>" +
                                   "<strong>Order Label:</strong> {4}<br>" +
                                   "<strong>Order Qty:</strong> {5}<br>" +
                                   "<strong>Order Price:</strong> {6}<br><br>" +
                                   "<table cellpadding='2' border='1'><thead style='font-weight:bold;'><td>Source</td><td>Qty</td><td>Price</td><td>Status</td></thead> {7} </table> <br>",
                                   selected_product.name,
                                   selected_product.sku,
                                   selected_product.barcode,
                                   selected_product.extension.brand.brand_name,
                                   selected_order.order_label,
                                   selected_order.order_items.Where(x => x.sku == item.product_sku).FirstOrDefault()?.qty_ordered,
                                   selected_order.order_items.Where(x => x.sku == item.product_sku).FirstOrDefault()?.price + " " + selected_order.currency,
                                   string.Join("", selected_product.sources.SelectMany(x => new List<string>() {
                                           "<tr>" +
                                           "<td>" + x.name + "</td>" +
                                           "<td>" + x.qty + "</td>" +
                                           "<td>" +
                                                (x.name == product_main_source ? "Price: " +
                                                (selected_product.special_price != 0 ? Math.Round(selected_product.special_price * (1 + (selected_product.tax / 100)),2,MidpointRounding.AwayFromZero) + selected_product.currency :
                                                Math.Round(selected_product.price * (1m + (selected_product.tax / 100m)),2,MidpointRounding.AwayFromZero) + selected_product.currency) :
                                                ((selected_xproducts != null && selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault() != null) ?
                                                "Price: " + Math.Round(selected_xproducts.FirstOrDefault( xp => xp.xml_source == x.name ).price2,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency + "<br>" +
                                                "MSRP: " + Math.Round(selected_xproducts.FirstOrDefault( xp => xp.xml_source == x.name ).price1,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency : string.Empty)) +
                                           "</td>" +
                                           "<td>" + (x.is_active ? "active" : "passive") + "</td>" +
                                           "</tr>"
                                   })));
                            if (Helper.global.google == null) {
                                Helper.global.google = new SettingsGoogle {
                                    sender_name = Helper.global.settings.company_name,
                                    is_enabled = true
                                };
                            }
                            if (GMailOAuth2.Send(customer.user_name, Helper.global.google.sender_name, Helper.global.google.oauth2_clientid, Helper.global.google.oauth2_clientsecret, Constants.QP_MailSender, Constants.QP_MailTo,
                                mail_title,
                                mail_body)) {
                                item.is_notification_sent = true;
                                item.notification_content = mail_body;
                                if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                    PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                    db_helper.notification_clone.LogToServer(thread_id, "out_of_stock_product_sold", mail_title + " => " + mail_body, customer.customer_id, "notification");
                                }
                            }
                            selected_product = null;
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_IN_STOCK:
                        selected_product = products?.Where(x => x.sku == item.product_sku).FirstOrDefault();
                        selected_xproducts = xproducts?.Where(x => x.barcode == item.xproduct_barcode).ToList();

                        if (selected_product != null) {
                            string mail_title = string.Format("{0} - PRODUCT IN STOCK", item.product_sku);
                            string mail_body = string.Format("<strong>Product:</strong> {0}<br>" +
                                   (product_targets.Contains(Constants.MAGENTO2) && Helper.GetProductBySKU(selected_product.sku)?.status == 1 ? "<strong>Status:</strong> Enabled<br>" : "<strong>Status:</strong> Disabled<br>") +
                                   "<strong>Sku:</strong> {1}<br>" +
                                   "<strong>Barcode:</strong> {2}<br>" +
                                   "<strong>Brand:</strong> {3}<br>" +
                                   "<strong>Selling Price:</strong> {4}<br><br>" +
                                   "<table cellpadding='2' border='1'><thead style='font-weight:bold;'><td>Source</td><td>Qty</td><td>Prices</td><td>Status</td></thead>{5}</table><br>",
                                   selected_product.name,
                                   selected_product.sku,
                                   selected_product.barcode,
                                   selected_product.extension.brand.brand_name,
                                   (selected_product.special_price != 0 ? (selected_product.special_price * (1 + (selected_product.tax / 100)) * (selected_product.currency == "USD" ? Helper.global.settings.rate_USD : (selected_product.currency == "EUR" ? Helper.global.settings.rate_EUR : 1))) + "TL" :
                                            Math.Round(selected_product.price * (1m + (selected_product.tax / 100m)) * (selected_product.currency == "USD" ? Helper.global.settings.rate_USD : (selected_product.currency == "EUR" ? Helper.global.settings.rate_EUR : 1)), 2, MidpointRounding.AwayFromZero) + "TL"),
                                   string.Join("", selected_product.sources.SelectMany(x => new List<string>() {
                                           "<tr>" +
                                           "<td>" + x.name + "</td>" +
                                           "<td>" + x.qty + "</td>" +
                                           "<td>" +
                                                (x.name == product_main_source ? "Price: " +
                                                (selected_product.special_price != 0 ? Math.Round(selected_product.special_price * (1 + (selected_product.tax / 100)),2,MidpointRounding.AwayFromZero) + selected_product.currency :
                                                Math.Round(selected_product.price * (1m + (selected_product.tax / 100m)),2,MidpointRounding.AwayFromZero) + selected_product.currency) :
                                                ((selected_xproducts != null && selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault() != null) ?
                                                "Price: " + Math.Round(selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault().price2,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency + "<br>" +
                                                "MSRP: " + Math.Round(selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault().price1,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency : string.Empty)) +
                                           "</td>" +
                                           "<td>" + (x.is_active ? "active" : "passive") + "</td>" +
                                           "</tr>"
                                   })));
                            if (Helper.global.google == null) {
                                Helper.global.google = new SettingsGoogle {
                                    sender_name = Helper.global.settings.company_name
                                };
                            }
                            if (GMailOAuth2.Send(customer.user_name, Helper.global.google.sender_name, Helper.global.google.oauth2_clientid, Helper.global.google.oauth2_clientsecret, Constants.QP_MailSender, Constants.QP_MailTo,
                                mail_title,
                                mail_body)) {
                                item.is_notification_sent = true;
                                item.notification_content = mail_body;
                                if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                    PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                    db_helper.notification_clone.LogToServer(thread_id, "product_in_stock", mail_title + " => " + mail_body, customer.customer_id, "notification");
                                }
                            }
                            selected_product = null;
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_OUT_OF_STOCK:
                        selected_product = products?.Where(x => x.sku == item.product_sku).FirstOrDefault();
                        selected_xproducts = xproducts?.Where(x => x.barcode == item.xproduct_barcode).ToList();

                        if (selected_product != null) {
                            string mail_title = string.Format("{0} - PRODUCT OUT OF STOCK", item.product_sku);
                            string mail_body = string.Format("<strong>Product:</strong> {0}<br>" +
                                   (product_targets.Contains(Constants.MAGENTO2) && Helper.GetProductBySKU(selected_product.sku)?.status == 1 ? "<strong>Status:</strong> Enabled<br>" : "<strong>Status:</strong> Disabled<br>") +
                                   "<strong>Sku:</strong> {1}<br>" +
                                   "<strong>Barcode:</strong> {2}<br>" +
                                   "<strong>Brand:</strong> {3}<br>" +
                                   "<strong>Selling Price:</strong> {4}<br><br>" +
                                   "<table cellpadding='2' border='1'><thead style='font-weight:bold;'><td>Source</td><td>Qty</td><td>Prices</td><td>Status</td></thead>{5}</table><br>",
                                   selected_product.name,
                                   selected_product.sku,
                                   selected_product.barcode,
                                   selected_product.extension.brand.brand_name,
                                   (selected_product.special_price != 0 ? (selected_product.special_price * (1 + (selected_product.tax / 100)) * (selected_product.currency == "USD" ? Helper.global.settings.rate_USD : (selected_product.currency == "EUR" ? Helper.global.settings.rate_EUR : 1))) + "TL" :
                                            Math.Round(selected_product.price * (1m + (selected_product.tax / 100m)) * (selected_product.currency == "USD" ? Helper.global.settings.rate_USD : (selected_product.currency == "EUR" ? Helper.global.settings.rate_EUR : 1)), 2, MidpointRounding.AwayFromZero) + "TL"),
                                   string.Join("", selected_product.sources.SelectMany(x => new List<string>() {
                                           "<tr>" +
                                           "<td>" + x.name + "</td>" +
                                           "<td>" + x.qty + "</td>" +
                                           "<td>" +
                                                (x.name == product_main_source ? "Price: " +
                                                (selected_product.special_price != 0 ? Math.Round(selected_product.special_price * (1 + (selected_product.tax / 100)),2,MidpointRounding.AwayFromZero) + selected_product.currency :
                                                Math.Round(selected_product.price * (1m + (selected_product.tax / 100m)),2,MidpointRounding.AwayFromZero) + selected_product.currency) :
                                                ((selected_xproducts != null && selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault() != null) ?
                                                "Price: " + Math.Round(selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault().price2,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency + "<br>" +
                                                "MSRP: " + Math.Round(selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault().price1,2,MidpointRounding.AwayFromZero) + selected_xproducts.Where(xp => xp.xml_source == x.name).FirstOrDefault()?.currency : string.Empty)) +
                                           "</td>" +
                                           "<td>" + (x.is_active ? "active" : "passive") + "</td>" +
                                           "</tr>"
                                   })));
                            if (Helper.global.google == null) {
                                Helper.global.google = new SettingsGoogle {
                                    sender_name = Helper.global.settings.company_name
                                };
                            }
                            if (GMailOAuth2.Send(customer.user_name, Helper.global.google.sender_name, Helper.global.google.oauth2_clientid, Helper.global.google.oauth2_clientsecret, Constants.QP_MailSender, Constants.QP_MailTo,
                                mail_title,
                                mail_body)) {
                                item.is_notification_sent = true;
                                item.notification_content = mail_body;
                                if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                    PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                    db_helper.notification_clone.LogToServer(thread_id, "product_out_of_stock", mail_title + " => " + mail_body, customer.customer_id, "notification");
                                }
                            }
                            selected_product = null;
                        }
                        break;

                    case Notification.NotificationTypes.XML_PRODUCT_ADDED:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("XML_PRODUCT_ADDED {0}", item.xproduct_barcode);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_product_added", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_PRODUCT_REMOVED:
                        string mail_title1 = string.Format("XML PRODUCT REMOVED {0}", item.xproduct_barcode);
                        string mail_body1 = string.Empty;

                        item.is_notification_sent = true;
                        item.notification_content = mail_title1;
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title1 + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_product_removed", mail_title1 + " => " + mail_title1, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_PRICE_CHANGED:
                        selected_product = products?.Where(x => x.barcode == item.xproduct_barcode).FirstOrDefault();
                        selected_xproducts = xproducts?.Where(x => x.barcode == item.xproduct_barcode).ToList();
                        string? xsource = item.notification_content?.Split("=")[0];
                        string? xprices = item.notification_content?.Split("=")[1];

                        if (selected_product != null && xsource != null && xprices != null) {
                            string mail_title = string.Format("{0} - XML PRICE CHANGED", selected_product.sku);
                            decimal.TryParse(xprices.Split("|")[0], out decimal new_price);
                            decimal.TryParse(xprices.Split("|")[1], out decimal old_price);
                            if (old_price != 0 && new_price != 0) {
                                string mail_body = string.Format("<strong>Product:</strong> {0}<br>" +
                                       (product_targets.Contains(Constants.MAGENTO2) && Helper.GetProductBySKU(selected_product.sku)?.status == 1 ? "<strong>Status:</strong> Enabled<br>" : "<strong>Status:</strong> Disabled<br>") +
                                       "<strong>Sku:</strong> {1}<br>" +
                                       "<strong>Barcode:</strong> {2}<br>" +
                                       "<strong>Brand:</strong> {3}<br>" +
                                       "<strong>Source:</strong> {4}<br>" +
                                       "<strong>Status:</strong> {5}<br>" +
                                       "<strong>Qty:</strong> {6}<br>" +
                                       "<strong>Old Price:</strong> {7}<br>" +
                                       "<strong>New Price:</strong> {8}<br>",
                                       selected_product.name,
                                       selected_product.sku,
                                       selected_product.barcode,
                                       selected_product.extension.brand.brand_name,
                                       xsource, ((selected_xproducts?.Where(x => x.xml_source == xsource).FirstOrDefault()?.is_active == true) ? "active" : "passive"),
                                       selected_xproducts?.Where(x => x.xml_source == xsource).FirstOrDefault()?.qty,
                                       Math.Round(old_price, 2, MidpointRounding.AwayFromZero) + selected_xproducts?.Where(x => x.xml_source == xsource).FirstOrDefault()?.currency,
                                       Math.Round(new_price, 2, MidpointRounding.AwayFromZero) + selected_xproducts?.Where(x => x.xml_source == xsource).FirstOrDefault()?.currency);
                                if (Helper.global.google == null) {
                                    Helper.global.google = new SettingsGoogle {
                                        sender_name = Helper.global.settings.company_name
                                    };
                                }
                                if (GMailOAuth2.Send(customer.user_name, Helper.global.google.sender_name, Helper.global.google.oauth2_clientid, Helper.global.google.oauth2_clientsecret, Constants.QP_MailSender, Constants.QP_MailTo,
                                       mail_title,
                                   mail_body)) {
                                    item.is_notification_sent = true;
                                    item.notification_content = mail_body;
                                    if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                        PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                        db_helper.notification_clone.LogToServer(thread_id, "xml_price_changed", mail_title + " => " + mail_body, customer.customer_id, "notification");
                                    }
                                }
                                selected_product = null;
                            }
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_PRICE_UPDATE_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("PRODUCT_PRICE_UPDATE_ERROR {0}", item.product_sku);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.product_sku + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "product_price_update_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_SPECIAL_PRICE_UPDATE_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("PRODUCT_SPECIAL_PRICE_UPDATE_ERROR {0}", item.product_sku);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.product_sku + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "product_special_price_update_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_CUSTOM_PRICE_UPDATE_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("PRODUCT_CUSTOM_PRICE_UPDATE_ERROR {0}", item.product_sku);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.product_sku + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "product_custom_price_update_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_QTY_UPDATE_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("PRODUCT_QTY_UPDATE_ERROR {0}", item.product_sku);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.product_sku + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "product_qty_update_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.PRODUCT_UPDATE_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("PRODUCT_UPDATE_ERROR {0}", item.product_sku);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.product_sku + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "product_update_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_SYNC_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("XML SYNC ERROR {0}", item.xproduct_barcode);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_sync_error", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_QTY_CHANGED:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("XML QTY CHANGED {0}", item.xproduct_barcode);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_qty_changed", item.notification_content, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_PRODUCT_REMOVED_BY_USER:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("XML PRODUCT REMOVED BY USER {0}", item.xproduct_barcode);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_product_removed_by_user", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.XML_SOURCE_FAILED:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("XML PRODUCT REMOVED {0}", item.xproduct_barcode);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "xml_product_removed", item.notification_content + " => " + item.xproduct_barcode, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.NEW_INVOICE:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("NEW INVOICE {0}", item.invoice_no);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID: " + item.id.ToString() + " notification sent[" + item.xproduct_barcode + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "new_invoice", item.notification_content + " => " + item.invoice_no, customer.customer_id, "notification");
                        }
                        break;

                    case Notification.NotificationTypes.ORDER_COMPLETE:
                        selected_order = orders?.Where(x => x.order_label == item.order_label).FirstOrDefault();

                        if (selected_order != null) {
                            string mail_title = string.Format("ORDER COMPLETE {0}", item.order_label);
                            string mail_body = string.Empty;

                            item.is_notification_sent = true;
                            item.notification_content = mail_title;
                            if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                db_helper.notification_clone.LogToServer(thread_id, "order_complete", mail_title + " => " + mail_body, customer.customer_id, "notification");
                            }
                        }
                        break;

                    case Notification.NotificationTypes.ORDER_PROCESS:
                        selected_order = orders?.Where(x => x.order_label == item.order_label).FirstOrDefault();

                        if (selected_order != null) {
                            string mail_title = string.Format("ORDER PROCESS {0}", item.order_label);
                            string mail_body = string.Empty;

                            item.is_notification_sent = true;
                            item.notification_content = mail_title;
                            if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                db_helper.notification_clone.LogToServer(thread_id, "order_process", mail_title + " => " + mail_body, customer.customer_id, "notification");
                            }
                        }
                        break;

                    case Notification.NotificationTypes.ORDER_SHIPPED:
                        selected_order = orders?.Where(x => x.order_label == item.order_label).FirstOrDefault();

                        if (selected_order != null) {
                            string mail_title = string.Format("ORDER SHIPPED {0}", item.order_label);
                            string mail_body = string.Empty;

                            item.is_notification_sent = true;
                            item.notification_content = mail_title;
                            if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                                PrintConsole("ID: " + item.id.ToString() + " notification sent[" + mail_title + "]");
                                db_helper.notification_clone.LogToServer(thread_id, "order_shipped", mail_title + " => " + mail_body, customer.customer_id, "notification");
                            }
                        }
                        break;
                    case Notification.NotificationTypes.NEW_ORDER_ERROR:
                        item.is_notification_sent = true;
                        item.notification_content = string.Format("NEW_ORDER_ERROR {0}", item.order_label);
                        if (db_helper.notification_clone.UpdateNotifications(customer.customer_id, [item])) {
                            PrintConsole("ID:" + item.id.ToString() + " notification sent [" + item.order_label + "]");
                            db_helper.notification_clone.LogToServer(thread_id, "new_order_error", item.notification_content, customer.customer_id, "notification");
                        }
                        break;
                }
            }
        }
        catch (Exception _ex) {
            db_helper.notification_clone.LogToServer(thread_id, "error", _ex.ToString(), customer.customer_id, "notification");
        }
        finally {
            if (customer.notification_sync_status) {
                db_helper.notification_clone.SetNotificationSyncDate(customer.customer_id);
                db_helper.notification_clone.SetNotificationSyncWorking(customer.customer_id, false);
            }
            //PrintConsole("Notification sync ended");
        }
    }
    #endregion

    #region Product Threads - ProductLoop, ProductSourceLoop, ProductSync
    private void ProductLoop(out bool _health) {
        _health = true;
        try {
            #region Main Product Source
            if (product_main_source != null && product_main_source == Constants.ENTEGRA) {
                PrintConsole("Started loading " + Constants.ENTEGRA + " sources.");

                //TODO: main currency source check will be here
                var source_currency_rates = Helper.GetENTCurrencyRates();
                if (source_currency_rates != null) {
                    rates.TL = source_currency_rates.TL;
                    rates.USD = source_currency_rates.USD;
                    rates.EUR = source_currency_rates.EUR;
                    PrintConsole(Constants.ENTEGRA + " TRY:" + rates.TL.ToString() + "|USD:" + rates.USD.ToString() + "|EUR:" + rates.EUR.ToString());
                }
                else {
                    PrintConsole("MAIN_SOURCE:" + Constants.ENTEGRA + " connection:currencies failed");
                }
                var ent_products = Helper.GetENTProducts();
                if (ent_products != null && ent_products.Count > 0) {
                    foreach (var item in ent_products) {
                        if (Helper.global.product.is_barcode_required) {
                            if (string.IsNullOrWhiteSpace(item.Barcode)) {
                                PrintConsole(Constants.ENTEGRA + " " + item.Sku + " barcode missing, not sync.", false);
                                continue;
                            }
                        }

                        if (item.Sku != string.Empty) {
                            #region Checking Product Extension If exist
                            //Brand? existed_brand = brands.Where(x => x.brand_name.Trim().Equals(item.BrandName?.Trim().ToLower(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                            Brand existed_brand = !string.IsNullOrWhiteSpace(item.BrandName?.Trim()) ? brands.FirstOrDefault(x => x.brand_name == item.BrandName.Trim()) ?? default_brand : default_brand;
                            ProductExtension? existed_p_ext = products.Where(x => x.sku == item.Sku).FirstOrDefault()?.extension;
                            if (existed_p_ext != null) existed_p_ext.brand = existed_brand;
                            List<Category>? existed_p_cats = existed_p_ext != null ? categories?.Where(x => existed_p_ext.category_ids.Split(",").Contains(x.id.ToString())).ToList() : null;
                            if (existed_p_ext != null) existed_p_ext.categories = existed_p_cats;
                            #endregion

                            //TODO: Product attribute source mapping condition will be here
                            var p = new Product() {
                                customer_id = customer.customer_id,
                                source_product_id = item.ProductId,
                                barcode = item.Barcode,
                                currency = item.Currency,
                                name = item.Name,
                                sku = item.Sku,
                                price = item.Price,
                                special_price = item.Special_Price,
                                custom_price = item.Custom_Price,
                                tax = item.Tax,

                                tax_included = item.TaxIncluded,
                                sources = [ new ProductSource( customer.customer_id, 0,
                                        Constants.ENTEGRA,
                                        item.Sku,
                                        item.Barcode,
                                        item.Qty,
                                        true, DateTime.Now) ],
                                extension = new ProductExtension() {
                                    sku = item.Sku,
                                    barcode = item.Barcode,
                                    customer_id = customer.customer_id,
                                    brand_id = existed_brand != null ? existed_brand.id : (string.IsNullOrEmpty(item.BrandName) ? default_brand.id : 0),
                                    is_xml_enabled = existed_p_ext != null && existed_p_ext.is_xml_enabled,
                                    xml_sources = existed_p_ext != null ? existed_p_ext.xml_sources : [],
                                    category_ids = existed_p_ext != null ? existed_p_ext.category_ids : Helper.global.product.customer_root_category_id.ToString(),
                                    categories = existed_p_cats ?? ([root_category]),
                                    brand = existed_brand != null ? existed_brand : (string.IsNullOrEmpty(item.BrandName) ?
                                    default_brand
                                    : new Brand() {
                                        customer_id = customer.customer_id,
                                        brand_name = item.BrandName,
                                        status = true,
                                        id = 0
                                    }),
                                    is_enabled = true,
                                    volume = 0,
                                    weight = 0,
                                    description = null
                                },
                                type = Product.ProductTypes.SIMPLE,
                                attributes = [],
                                images = []
                            };

                            live_products.Add(p);
                        }
                        else {
                            PrintConsole(Constants.ENTEGRA + " " + item.Barcode + " sku missing, not sync.", false);
                        }
                    }
                }
                else {
                    PrintConsole("MAIN_SOURCE:" + Constants.ENTEGRA + " connection:products failed");
                }
            }

            if (product_main_source != null && product_main_source == Constants.MERCHANTER) {
                live_products = products ?? [];
            }

            if (product_main_source != null && product_main_source == Constants.NETSIS) {
                //TODO: NETSIS product main source will be here
            }

            if (product_main_source != null && product_main_source == Constants.ANK_ERP) {
                if (Helper.global.ank_erp != null && Helper.global.ank_erp.company_code != null && Helper.global.ank_erp.user_name != null && Helper.global.ank_erp.password != null && Helper.global.ank_erp.work_year != null && Helper.global.ank_erp.url != null) {
                    ANKERP ank_erp = new(Helper.global.ank_erp.company_code, Helper.global.ank_erp.user_name, Helper.global.ank_erp.password, Helper.global.ank_erp.work_year, Helper.global.ank_erp.url,
                        """C:\MerchanterServer\ankaraerp""");
                    PrintConsole("Started loading " + Constants.ANK_ERP + " sources.");

                    #region Category Pre-Sync
                    //var ank_categories = ank_erp.GetCategoriesFromFolder("""C:\Users\caqn_\OneDrive\Masaüstü\otoahmet_categories""")?.DokumanPaketKategori.Eleman.ElemanListe.KategoriItem;
                    var ank_categories = ank_erp.GetCategories()?.Result?.DokumanPaketKategori.Eleman.ElemanListe.KategoriItem;
                    PrintConsole(Constants.ANK_ERP + " total " + ank_categories?.Count + " categories found.");
                    if (ank_categories != null && ank_categories.Count > 0) {
                        foreach (var item in ank_categories) {
                            if (item.Kodu == "0" || !int.TryParse(item.Kodu, out int category_code) || !int.TryParse(item.UstBaslik, out int parent_code))
                                continue;
                            var existed_category = categories?.FirstOrDefault(x => x.source_category_id == category_code);
                            if (existed_category != null) {
                                bool need_update = false;
                                if (existed_category.category_name != item.Adi) {
                                    existed_category.category_name = item.Adi;
                                    need_update = true;
                                }
                                if (existed_category.is_active != (item.Pasif == "H")) {
                                    existed_category.is_active = item.Pasif == "H";
                                    need_update = true;
                                }

                                if (item.UstBaslik == "0") {
                                    existed_category.parent_id = Helper.global.product.customer_root_category_id;
                                }
                                else {
                                    var existed_parent_category = categories?.FirstOrDefault(x => x.source_category_id == parent_code);
                                    if (existed_parent_category != null) {
                                        if (existed_category.parent_id != existed_parent_category.id) {
                                            existed_category.parent_id = existed_parent_category.id;
                                            need_update = true;
                                        }
                                    }
                                    else {
                                        existed_category.parent_id = Helper.global.product.customer_root_category_id;
                                    }
                                }
                                if (need_update) {
                                    if (db_helper.UpdateCategory(customer.customer_id, existed_category)) {
                                        db_helper.LogToServer(thread_id, "product_category_updated", existed_category.category_name, customer.customer_id, "product");
                                        PrintConsole(Constants.ANK_ERP + " " + existed_category.category_name + " category updated.", false);
                                    }
                                    else {
                                        PrintConsole(Constants.ANK_ERP + " " + item.Kodu + "-" + item.Adi + " category update error.", false);
                                    }
                                }
                            }
                            else {
                                Category c = new() {
                                    customer_id = customer.customer_id,
                                    category_name = item.Adi,
                                    is_active = item.Pasif == "H",
                                    parent_id = Helper.global.product.customer_root_category_id,
                                    source_category_id = Convert.ToInt32(item.Kodu),
                                };
                                var inserted_category = db_helper.InsertCategory(customer.customer_id, c);
                                if (inserted_category != null) {
                                    categories?.Add(inserted_category);
                                    db_helper.LogToServer(thread_id, "product_category_inserted", inserted_category.category_name, customer.customer_id, "product");
                                    PrintConsole(Constants.ANK_ERP + " " + inserted_category.category_name + " category inserted.", false);
                                }
                                else {
                                    PrintConsole(Constants.ANK_ERP + " " + item.Kodu + "-" + item.Adi + " category insert error.", false);
                                }
                            }
                        }
                    }
                    else {
                        PrintConsole(Constants.ANK_ERP + " connection:categories failed");
                    }
                    PrintConsole(Constants.ANK_ERP + " category sync ended.");
                    categories?.Clear();
                    categories = db_helper.GetCategories(customer.customer_id);  //Re-take updated categories
                    #endregion

                    #region Product Memory Take
                    PrintConsole(Constants.ANK_ERP + " product api take started.");
                    var ank_products_zip = ank_erp.GetProducts().Result;
                    //var ank_products_zip = ank_erp.GetProductsFromFolder("""C:\Users\caqn_\OneDrive\Masaüstü\otoahmet_products_2""");
                    List<UrunSicil> ank_products = [];
                    if (ank_products_zip != null && ank_products_zip.Count > 0) {
                        foreach (var zip_item in ank_products_zip) {
                            ank_products.AddRange([.. zip_item.DokumanPaketUrun.Eleman.ElemanListe.UrunSicil]);
                        }
                    }
                    else {
                        PrintConsole(Constants.ANK_ERP + " connection:products failed.");
                    }
                    PrintConsole(Constants.ANK_ERP + " " + ank_products.Count + " products found in api.", false);
                    ank_products = [.. ank_products.DistinctBy(x => x.UrunTanim.SicilKodu)];  //Remove duplicates
                    PrintConsole(Constants.ANK_ERP + " " + ank_products.Count + " products found in api. --Duplicates removed by SicilKodu !!!");
                    #endregion

                    foreach (var item in ank_products) {
                        if (customer.customer_id == 3 && item.UrunTanim.SicilKodu == "144608527RÜ") continue; //this products are corrupted
                        if (Helper.global.product.is_barcode_required) {
                            if (string.IsNullOrWhiteSpace(item.UrunTanim.BarkodKodu?.ToString())) {
                                PrintConsole(Constants.ANK_ERP + " " + item.UrunTanim.SicilKodu + " barcode missing, not sync.", false);
                                continue;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(item.UrunTanim.SicilKodu)) {

                            #region Checking existing Product Brand and Categories
                            Brand? existed_brand = !string.IsNullOrWhiteSpace(item.UrunTanim.UrAdi) ? brands.FirstOrDefault(x => x.brand_name == item.UrunTanim.UrAdi.Trim()) : default_brand;
                            List<Category> existed_p_cats = [root_category];
                            foreach (var citem in item.UrunTanim.KategoriKodu.Split(",")) {
                                if (int.TryParse(citem, out int cat_code) && ank_categories != null) {
                                    var existed_cat = categories.FirstOrDefault(x => x.source_category_id == cat_code);
                                    if (existed_cat != null) existed_p_cats.Add(existed_cat);
                                    else {
                                        string? new_category = ank_categories.FirstOrDefault(x => x.Kodu == citem)?.Adi;
                                        if (!string.IsNullOrWhiteSpace(new_category))
                                            existed_p_cats.Add(new Category() {
                                                id = 0, customer_id = customer.customer_id, is_active = true,
                                                source_category_id = Convert.ToInt32(citem),
                                                category_name = ank_categories.FirstOrDefault(x => x.Kodu == citem)?.Adi,
                                                parent_id = Helper.global.product.customer_root_category_id
                                            });
                                    }
                                }
                            }
                            #endregion

                            #region Creating Product Core
                            var p = new Product() {
                                customer_id = customer.customer_id,
                                sku = item.UrunTanim.SicilKodu.Trim(),
                                barcode = item.UrunTanim.BarkodKodu != null ? item.UrunTanim.BarkodKodu.Trim() : string.Empty,
                                name = !string.IsNullOrWhiteSpace(item.UrunTanim.SicilAdiy) ? item.UrunTanim.SicilAdiy.Trim() : item.UrunTanim.SicilAdi?.Trim(),
                                type = Product.ProductTypes.SIMPLE,
                                tax = item.UrunTanim.KdvOrani,
                                currency = item.UrunTanim.ParaCinsi.Trim(),
                                price = item.UrunTanim.PerSatFiyat,
                                special_price = decimal.TryParse(item.UrunTanim.KamSatFiyat?.Replace(",", "").Replace(".", ","), out decimal _special_price) ? _special_price : 0,
                                tax_included = true,
                                extension = new ProductExtension() {
                                    customer_id = customer.customer_id,
                                    barcode = item.UrunTanim.BarkodKodu != null ? item.UrunTanim.BarkodKodu.Trim() : string.Empty,
                                    is_xml_enabled = false,
                                    xml_sources = [],
                                    sku = item.UrunTanim.SicilKodu.Trim(),
                                    brand_id = existed_brand != null ? existed_brand.id : (string.IsNullOrWhiteSpace(item.UrunTanim.UrAdi?.Trim()) ? default_brand.id : 0),
                                    brand = existed_brand != null ? existed_brand : (string.IsNullOrWhiteSpace(item.UrunTanim.UrAdi?.Trim()) ? default_brand :
                                        new Brand() {
                                            customer_id = customer.customer_id,
                                            brand_name = item.UrunTanim.UrAdi.Trim(),
                                            status = true,
                                            id = 0
                                        }),
                                    category_ids = string.Join(",", existed_p_cats.Select(x => x.id)),
                                    categories = existed_p_cats,
                                    is_enabled = true,
                                    weight = decimal.TryParse(item.UrunTanim.AgirligiKg.ToString(), out decimal _weight) ? _weight : 0,
                                    volume = decimal.TryParse(item.UrunTanim.HacmiM3.ToString(), out decimal _volume) ? _volume : 0,
                                    description = item.Gsozelliks.Gsozellik.Count > 0 ? !string.IsNullOrWhiteSpace(item.Gsozelliks.Gsozellik[0].IBase64Value) ? item.Gsozelliks.Gsozellik[0].IBase64Value : null : null,
                                },
                                sources = [ new ProductSource( customer.customer_id, 0,
                                    Constants.ANK_ERP,
                                    item.UrunTanim.SicilKodu.Trim(),
                                    item.UrunTanim.BarkodKodu != null ? item.UrunTanim.BarkodKodu.Trim() : string.Empty,
                                    item.UrunTanim.StokMevcudu > 0 ? item.UrunTanim.StokMevcudu : 0,
                                    true, DateTime.Now) ],
                                attributes = [],
                                images = [],
                            };
                            #endregion

                            #region Checking existing Product Images
                            for (int i = 0; i < item.Images.Image.Count; i++) {
                                if (!string.IsNullOrWhiteSpace(item.Images.Image[i].IFilename) && !string.IsNullOrWhiteSpace(item.Images.Image[i].IBase64Value)) {
                                    if (item.Images.Image[i].IFilename?.Split(".")[1].ToLower() == "jpg" || item.Images.Image[i].IFilename?.Split(".")[1].ToLower() == "png" ||
                                        item.Images.Image[i].IFilename?.Split(".")[1].ToLower() == "jpeg" || item.Images.Image[i].IFilename?.Split(".")[1].ToLower() == "gif") {
                                        var existed_image = product_images.FirstOrDefault(x => x.sku == item.UrunTanim.SicilKodu && x.image_name == item.Images.Image[i].IFilename);
                                        //Save image to folder
                                        string image_path = Path.Combine("""C:\MerchanterServer\ankaraerp\Images""", customer.user_name);
                                        if (!Directory.Exists(image_path + """\""" + item.UrunTanim.SicilKodu.Trim())) {
                                            Directory.CreateDirectory(image_path + """\""" + item.UrunTanim.SicilKodu.Trim());
                                        }
                                        string? image_name = item.Images.Image[i].IFilename?.Trim();
                                        if (!string.IsNullOrWhiteSpace(image_name)) {
                                            string image_full_path = Path.Combine(image_path, item.UrunTanim.SicilKodu.Trim(), image_name);
                                            if (!File.Exists(image_full_path)) {
                                                File.WriteAllBytes(image_full_path, Convert.FromBase64String(item.Images.Image[i].IBase64Value));
                                            }
                                            else {
                                                PrintConsole(Constants.ANK_ERP + " " + item.UrunTanim.SicilKodu + " image already exist.", false);
                                            }
                                            p.images.Add(new ProductImage() {
                                                customer_id = customer.customer_id,
                                                product_id = existed_image != null ? existed_image.product_id : 0,
                                                sku = item.UrunTanim.SicilKodu.Trim(),
                                                image_name = image_name,
                                                image_base64 = null,
                                                is_default = (i == 0),
                                                type = ImageTypes.url,
                                                image_url = Path.Combine(customer.user_name, item.UrunTanim.SicilKodu.Trim(), image_name)
                                            });
                                        }
                                        else {
                                            PrintConsole(Constants.ANK_ERP + " " + item.UrunTanim.SicilKodu + " image name missing, not sync.", false);
                                        }
                                    }
                                    else {
                                        PrintConsole(Constants.ANK_ERP + " " + item.UrunTanim.SicilKodu + " image type not supported, not sync.", false);
                                    }
                                }
                            }
                            #endregion

                            live_products.Add(p);
                        }
                    }
                }
            }
            #endregion
        }
        catch (Exception _ex) {
            db_helper.LogToServer(thread_id, "product_source_error", _ex.Message + newline + _ex.ToString(), customer.customer_id, "product");
            _health = false;
        }
        finally {
            PrintConsole(product_main_source + " " + live_products.Count + " products initialized.");
        }
    }

    private void ProductSourceLoop(out bool _health) {
        _health = true;
        try {
            #region XML [qty] Sources
            if (!string.IsNullOrWhiteSpace(product_main_source) && other_product_sources.Length > 0) {
                PrintConsole(Helper.global.settings.company_name + " started loading other sources.", false);
                foreach (var item in live_products) {
                    if (item.sources != null && item.extension.xml_sources != null) {
                        var selected_xproducts = xproducts.Where(x => x.barcode == item.barcode).ToList();
                        if (selected_xproducts != null && selected_xproducts.Count > 0) {
                            foreach (var xitem in selected_xproducts) {
                                item.sources.Add(new ProductSource(
                                    customer.customer_id,
                                    0,
                                    xitem.xml_source,
                                    item.sku,
                                    item.barcode,
                                    xitem.qty,
                                    (Helper.global.product.xml_qty_addictive_enable || item.sources[0].qty <= 0) && (xitem.is_active ? (item.extension.is_xml_enabled && item.extension.xml_sources.Contains(xitem.xml_source)) : false),
                                    DateTime.Now
                                ));
                            }
                        }
                    }
                }
            }
            #endregion

            #region Offline Storage

            #endregion
        }
        catch (Exception _ex) {
            db_helper.LogToServer(thread_id, "product_source_error", _ex.ToString(), customer.customer_id, "product");
            _health = false;
        }
        finally {
            #region Calculate [total_qty] for Sources
            PrintConsole(Helper.global.settings.company_name + " calculating [total_qty] for all products");
            int global_total_qty = 0;
            Dictionary<string, int> source_total_qtys = [];
            foreach (var item in live_products) {
                item.total_qty = item.sources.Where(x => x.is_active).Sum(x => x.qty);
                global_total_qty += item.sources.Where(x => x.is_active).Sum(x => x.qty);
                foreach (var sitem in item.sources) {
                    var temp = source_total_qtys.Where(x => x.Key == sitem.name).Count();
                    if (temp > 0) {
                        source_total_qtys[sitem.name] += sitem.is_active ? sitem.qty : 0;
                    }
                    else {
                        source_total_qtys.Add(sitem.name, sitem.is_active ? sitem.qty : 0);
                    }
                }
            }
            foreach (var item in source_total_qtys) {
                PrintConsole(Helper.global.settings.company_name + " calculated " + item.Key + ":[total_qty] is " + item.Value.ToString(), false);
            }
            PrintConsole(Helper.global.settings.company_name + " calculated [total_qty] is " + global_total_qty.ToString());
            #endregion
        }
    }

    private void ProductSync(out bool _health) {
        _health = true;
        try {
            if (products != null) {
                List<Notification> notifications = [];
                if (product_targets.Contains(Constants.MAGENTO2)) {
                    bool is_need_indexer = false;
                    foreach (var item in live_products) {
                        bool is_update = false; bool is_insert = false;
                        Product prepared_product = null;
                        var selected_product = products.FirstOrDefault(x => x.sku == item.sku);

                        if (selected_product != null) { //existing product
                            #region Name
                            if (selected_product.name?.ToString().Trim() != item.name?.ToString().Trim()) {

                            }
                            #endregion

                            #region Brand
                            if (!selected_product.extension.brand.brand_name.Trim().Equals(item.extension.brand.brand_name?.Trim(), StringComparison.CurrentCultureIgnoreCase)) {
                                var source_brands = Helper.GetProductAttribute(Helper.global.magento.brand_attribute_code);
                                is_update = true; is_need_indexer = true;
                                if (source_brands != null) {
                                    var selected_source_brand = source_brands.options.Where(x => x.label.Equals(item.extension.brand.brand_name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                                    if (selected_source_brand != null) { //update
                                        if (true || Helper.UpdateProductAttribute(item.sku, Helper.global.magento.brand_attribute_code, selected_source_brand.value)) {
                                            PrintConsole("Sku:" + item.sku + "Brand updated " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString());
                                            db_helper.LogToServer(thread_id, "product_brand_updated", Helper.global.settings.company_name + " Sku:" + item.sku + selected_source_brand.label + " => " + item.extension.brand.brand_name, customer.customer_id, "product");
                                        }
                                        else {
                                            PrintConsole("Sku:" + item.sku + "Brand update error " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString());
                                            db_helper.LogToServer(thread_id, "product_brand_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString(), customer.customer_id, "product");
                                        }
                                    }
                                    else { //insert
                                           //var inserted_id = Helper.InsertAttributeOption( Helper.global.magento.brand_attribute_code, item.brand.brand_name );
                                        if (true /*inserted_id != null*/ ) {
                                            if (true /*|| Helper.UpdateProductAttribute( item.sku, Helper.global.magento.brand_attribute_code, inserted_id )*/ ) {
                                                PrintConsole("Sku:" + item.sku + "Brand inserted " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString());
                                                db_helper.LogToServer(thread_id, "product_brand_inserted", Helper.global.settings.company_name + " Sku:" + item.sku + "= " + selected_product.extension.brand.brand_name + " => " + item.extension.brand.brand_name, customer.customer_id, "product");
                                            }
                                            else {
                                                PrintConsole("Sku:" + item.sku + "Brand insert error " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString());
                                                db_helper.LogToServer(thread_id, "product_brand_insert_error", Helper.global.settings.company_name + " Sku:" + item.sku + "= " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString(), customer.customer_id, "product");
                                            }
                                        }
                                    }

                                    if (item.extension.brand.id == 0) {
                                        item.extension.brand.id = db_helper.InsertBrand(customer.customer_id, item.extension.brand, true);
                                        item.extension.brand_id = item.extension.brand.id;
                                    }

                                }
                                else { //no brand attribute exists
                                    PrintConsole("Sku:" + item.sku + "Brand insert error " + selected_product.extension.brand.brand_name.ToString() + " => " + item.extension.brand.brand_name?.ToString());
                                    db_helper.LogToServer(thread_id, "product_brand_insert_error", Helper.global.settings.company_name + " - " + Helper.global.magento.brand_attribute_code + " brand attribute missing?", customer.customer_id, "product");
                                }
                            }
                            #endregion

                            #region Barcode
                            //if(false || !selected_product.extension.barcode.Trim().Equals( item.extension.barcode.Trim(), StringComparison.CurrentCultureIgnoreCase ) ) {
                            //    is_update = true; is_need_indexer = true;
                            //    if( true || Helper.UpdateProductAttribute( item.sku, Helper.global.magento.barcode_attribute_code, item.barcode?.Trim() ) ) {
                            //        db_helper.LogToServer( thread_id, "product_barcode_updated", Helper.global.settings.company_name + " Sku:" + item.sku + "= " + selected_product.barcode + " => " + item.barcode, customer.customer_id, "product" );
                            //    }
                            //    else {
                            //        db_helper.LogToServer( thread_id, "product_barcode_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + "= " + selected_product.price.ToString() + " => " + item.barcode?.ToString(), customer.customer_id, "product" );
                            //    }
                            //}
                            #endregion

                            #region Qty
                            if (selected_product.total_qty != item.total_qty) {
                                if (Helper.UpdateProductQty(item.sku, item.total_qty)) {
                                    is_update = true;
                                    PrintConsole("Sku:" + item.sku + " updated " + selected_product.total_qty + " => " + item.total_qty);
                                    db_helper.LogToServer(thread_id, "product_qty_updated", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.total_qty.ToString() + " => " + item.total_qty.ToString(), customer.customer_id, "product");
                                    #region Notify Product - PRODUCT_IN_STOCK, PRODUCT_OUT_OF_STOCK
                                    if (selected_product.total_qty <= 0 && item.total_qty > 0) {
                                        notifications.Add(new Notification() {
                                            customer_id = customer.customer_id,
                                            type = Notification.NotificationTypes.PRODUCT_IN_STOCK,
                                            product_sku = item.sku,
                                            xproduct_barcode = item.sources.Count > 1 ? item.barcode : null,
                                            notification_content = Constants.MAGENTO2
                                        });
                                    }
                                    if (selected_product.total_qty > 0 && item.total_qty <= 0) {
                                        notifications.Add(new Notification() {
                                            customer_id = customer.customer_id,
                                            type = Notification.NotificationTypes.PRODUCT_OUT_OF_STOCK,
                                            product_sku = item.sku,
                                            xproduct_barcode = item.sources.Count > 1 ? item.barcode : null,
                                            notification_content = Constants.MAGENTO2
                                        });
                                    }
                                    #endregion
                                }
                                else {
                                    is_update = false;
                                    notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.PRODUCT_QTY_UPDATE_ERROR, product_sku = item.sku, notification_content = Constants.MAGENTO2 });
                                    db_helper.LogToServer(thread_id, "product_qty_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.total_qty.ToString() + " => " + item.total_qty.ToString(), customer.customer_id, "product");
                                }
                            }
                            #endregion

                            #region Prices
                            if (selected_product.price != item.price) {
                                is_update = true; is_need_indexer = true;
                                if (Helper.UpdateProductPrice(item, rates)) {
                                    PrintConsole("Sku:" + item.sku + " updated " + selected_product.price + " => " + item.price);
                                    db_helper.LogToServer(thread_id, "product_price_updated", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.price.ToString() + " => " + item.price.ToString(), customer.customer_id, "product");
                                }
                                else {
                                    is_update = false;
                                    notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.PRODUCT_PRICE_UPDATE_ERROR, product_sku = item.sku, notification_content = Constants.MAGENTO2 });
                                    db_helper.LogToServer(thread_id, "product_price_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.price.ToString() + " => " + item.price.ToString(), customer.customer_id, "product");
                                }
                            }
                            if (selected_product.special_price != item.special_price) {
                                is_update = true; is_need_indexer = true;
                                if (Helper.UpdateProductSpecialPrice(item, rates)) {
                                    PrintConsole("Sku:" + item.sku + " updated " + selected_product.special_price + " => " + item.special_price);
                                    db_helper.LogToServer(thread_id, "product_special_price_updated", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.special_price.ToString() + " => " + item.special_price.ToString(), customer.customer_id, "product");
                                }
                                else {
                                    is_update = false;
                                    notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.PRODUCT_SPECIAL_PRICE_UPDATE_ERROR, product_sku = item.sku, notification_content = Constants.MAGENTO2 });
                                    db_helper.LogToServer(thread_id, "product_special_price_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.special_price.ToString() + " => " + item.special_price.ToString(), customer.customer_id, "product");
                                }
                            }
                            if (selected_product.custom_price != item.custom_price) {
                                is_update = true; is_need_indexer = true;
                                bool? temp_is_inserted = Helper.UpdateProductCustomPrice(item, rates);
                                if (temp_is_inserted.HasValue && temp_is_inserted.Value) {
                                    PrintConsole("Sku:" + item.sku + " updated " + selected_product.custom_price + " => " + item.custom_price);
                                    db_helper.LogToServer(thread_id, "product_custom_price_updated", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.custom_price.ToString() + " => " + item.custom_price.ToString(), customer.customer_id, "product");
                                }
                                else if (temp_is_inserted.HasValue && !temp_is_inserted.Value) {
                                    PrintConsole("Sku:" + item.sku + " cannot update " + selected_product.custom_price + " => " + item.custom_price);
                                    db_helper.LogToServer(thread_id, "product_custom_price_cannot_update", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.custom_price.ToString() + " => " + item.custom_price.ToString(), customer.customer_id, "product");
                                }
                                else {
                                    is_update = false;
                                    notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.PRODUCT_CUSTOM_PRICE_UPDATE_ERROR, product_sku = item.sku, notification_content = Constants.MAGENTO2 });
                                    db_helper.LogToServer(thread_id, "product_custom_price_update_error", Helper.global.settings.company_name + " Sku:" + item.sku + " " + selected_product.custom_price.ToString() + " => " + item.custom_price.ToString(), customer.customer_id, "product");
                                }
                            }
                            #endregion
                        }
                        else { //new product
                            prepared_product = new Product() {
                                id = 0,
                                customer_id = customer.customer_id,
                                type = item.type,
                                sku = item.sku,
                                barcode = item.barcode,
                                name = item.name,
                                source_product_id = item.source_product_id,
                                price = item.price,
                                special_price = item.special_price,
                                custom_price = item.custom_price,
                                tax = item.tax,
                                tax_included = item.tax_included,
                                currency = item.currency,
                                extension = new ProductExtension() {
                                    customer_id = customer.customer_id,
                                    sku = item.sku,
                                    barcode = item.barcode,
                                    brand_id = item.extension.brand.id,
                                    is_xml_enabled = item.extension.is_xml_enabled,
                                    xml_sources = item.extension.xml_sources,
                                    category_ids = item.extension.category_ids
                                },
                                total_qty = item.total_qty,
                                sources = item.sources
                            };
                            is_insert = true;
                        }

                        if (is_update) {
                            if (selected_product != null && selected_product.id > 0) {
                                item.id = selected_product.id;
                                if (db_helper.UpdateProducts(customer.customer_id, [item])) {
                                    PrintConsole("Sku:" + item.sku + " " + "updated.");
                                }
                            }
                        }
                        if (is_insert) {
                            if (prepared_product?.id == 0) {
                                if (db_helper.InsertProducts(customer.customer_id, [prepared_product]) > 0) {
                                    PrintConsole("Sku:" + item.sku + " " + "inserted.");
                                    db_helper.LogToServer(thread_id, "new_product", Helper.global.settings.company_name + " Sku:" + item.sku, customer.customer_id, "product");
                                }
                                else {
                                    PrintConsole("Sku:" + item.sku + " " + "insert error.");
                                }
                            }
                        }
                    }

                    #region ReIndex Magento for QP
                    if (is_need_indexer) { //for magento2 :\
                        if (customer.customer_id == 1) {
                            Thread th = new Thread(new ParameterizedThreadStart(Helper.PostPageAll));
                            th.Start("https://www.qp.com.tr/pub/qp/_automation.php");
                        }
                    }
                    #endregion


                    if (Helper.global.product.product_list_filter_source_products) {
                        //foreach (var item in products) {
                        //    var deleted_product = live_products.FirstOrDefault(x => x.sku == item.sku);
                        //    if (deleted_product == null && item.extension.is_enabled) {
                        //        item.extension.is_enabled = false;
                        //        if (db_helper.UpdateProducts(customer.customer_id, [item])) {
                        //            PrintConsole("Sku:" + item.sku + " updated on MerchanterDB. -disabled");
                        //        }
                        //        else {
                        //            PrintConsole("Sku:" + item.sku + " " + "update error on MerchanterDB. -disabled");
                        //        }
                        //    }
                        //    if (deleted_product != null && !item.extension.is_enabled) {
                        //        //TODO: Disable product on TARGET
                        //    }
                        //}
                    }

                    if (notifications.Count > 0) {
                        db_helper.InsertNotifications(customer.customer_id, notifications);
                    }
                }

                if (product_targets.Contains(Constants.IDEASOFT)) {
                    //var live_idea_products = Helper.GetIdeaProducts();
                    PrintConsole(Constants.IDEASOFT + " brands and categories started loading...");
                    var live_idea_categories = Helper.GetIdeaCategories(); PrintConsole(Constants.IDEASOFT + " total " + live_idea_categories?.Count + " categories found.");
                    var live_idea_brands = Helper.GetIdeaBrands(); PrintConsole(Constants.IDEASOFT + " total " + live_idea_brands?.Count + " brands found.");

                    foreach (var item in live_products) {
                        bool is_update = false; bool is_insert = false; string updated_attrs = "";
                        bool is_processed = false; int idea_product_id = 0;
                        Product prepared_product = null;
                        var selected_product = products.FirstOrDefault(x => x.sku == item.sku);

                        if (selected_product != null) { //existing product
                            #region Image
                            if (item.images != null && item.images.Count > 0) {
                                foreach (var image_item in item.images) {
                                    if (selected_product.images?.FirstOrDefault(x => x.image_name == image_item.image_name) == null) {
                                        is_update = true; updated_attrs += "image,";
                                    }
                                }
                            }
                            #endregion

                            #region Qty
                            if (selected_product.total_qty != item.total_qty) {
                                is_update = true;
                                updated_attrs += "qty,";
                            }
                            #endregion

                            #region Price
                            if (selected_product.price != item.price) {
                                is_update = true;
                                updated_attrs += "price,";
                            }
                            if (selected_product.special_price != item.special_price) {
                                is_update = true;
                                updated_attrs += "special_price,";
                            }
                            #endregion

                            #region Name
                            if (string.IsNullOrWhiteSpace(selected_product.name)) {
                                is_update = true;
                                updated_attrs += "name,";
                            }
                            else if (!selected_product.name.Equals(item.name, StringComparison.CurrentCulture)) {
                                is_update = true;
                                updated_attrs += "name,";
                            }
                            #endregion

                            #region Is Enabled
                            if (selected_product.extension.is_enabled != item.extension.is_enabled) {
                                is_update = true;
                                updated_attrs += "is_enabled,";
                            }
                            #endregion

                            #region Brand
                            if (selected_product.extension.brand != null) {
                                if (!selected_product.extension.brand.brand_name.Equals(item.extension.brand?.brand_name)) {
                                    is_update = true;
                                    updated_attrs += "brand,";
                                }
                                if (selected_product.extension.brand_id != item.extension.brand_id) {
                                    is_update = true;
                                    updated_attrs += "brand_id,";
                                }
                            }
                            #endregion

                            #region Category
                            if (selected_product.extension.category_ids != item.extension.category_ids) {
                                is_update = true;
                                updated_attrs += "category_ids,";
                            }
                            #endregion
                        }
                        else { //new product
                            prepared_product = new Product() {
                                id = 0,
                                customer_id = customer.customer_id,
                                type = item.type,
                                sku = item.sku,
                                barcode = item.barcode,
                                name = item.name,
                                source_product_id = item.source_product_id,
                                price = item.price,
                                special_price = item.special_price,
                                custom_price = item.custom_price,
                                tax = item.tax,
                                tax_included = item.tax_included,
                                currency = item.currency,
                                extension = new ProductExtension() {
                                    customer_id = customer.customer_id,
                                    sku = item.sku,
                                    barcode = item.barcode,
                                    brand_id = item.extension.brand_id,
                                    is_xml_enabled = item.extension.is_xml_enabled,
                                    xml_sources = item.extension.xml_sources,
                                    category_ids = item.extension.category_ids,
                                    categories = item.extension.categories,
                                    brand = item.extension.brand ?? default_brand,
                                    is_enabled = true,
                                    weight = item.extension.weight,
                                    volume = item.extension.volume,
                                    description = item.extension.description
                                },
                                total_qty = item.total_qty,
                                sources = item.sources,
                                attributes = item.attributes,
                                images = item.images
                            };
                            is_insert = true;
                        }

                        if (is_update) item.id = selected_product != null ? selected_product.id : 0;  //important early arrangement
                        if (is_update || is_insert) {
                            var selected_live_idea_product = Helper.GetIdeaProduct(item.sku);
                            if (selected_live_idea_product != null) { //update ideasoft product
                                idea_product_id = selected_live_idea_product.id;
                                //idea_product_id = relation.target_id;
                                #region Ideasoft Category and Brand Sync
                                List<int> idea_category_ids = Helper.GetIdeaCategoryIds(thread_id, db_helper, customer, ref live_idea_categories,
                                    category_target_relation, item.extension.categories);
                                var idea_brand_id = Helper.GetIdeaBrandId(thread_id, db_helper, customer, ref live_idea_brands, item.extension.brand);
                                #endregion
                                idea_product_id = Helper.UpdateIdeaProduct(idea_product_id, item, idea_brand_id, idea_category_ids);
                                if (idea_product_id > 0) {
                                    is_processed = true;
                                    PrintConsole("Sku:" + item.sku + " updated and sync to Id:" + idea_product_id.ToString() + " ," + Constants.IDEASOFT, ConsoleColor.Green);
                                    db_helper.LogToServer(thread_id, "product_updated", Helper.global.settings.company_name + " Sku:" + item.sku + ", " + Constants.IDEASOFT, customer.customer_id, "product");
                                }
                                else {
                                    notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.PRODUCT_UPDATE_ERROR, product_sku = item.sku, notification_content = Constants.IDEASOFT });
                                    PrintConsole("Sku:" + item.sku + " update error" + " ," + Constants.IDEASOFT, ConsoleColor.Red);
                                    db_helper.LogToServer(thread_id, "product_update_error", Helper.global.settings.company_name + " Sku:" + item.sku, customer.customer_id, "product");
                                }
                                //}
                            }
                            else { //insert ideasoft product
                                if (is_update) {  //insert ideasoft product and update product
                                    var product_relation = product_target_relation.FirstOrDefault(x => x.product_id == item.id && x.target_name == Constants.IDEASOFT);
                                    if (product_relation != null) {
                                        is_processed = true;
                                        if (db_helper.DeleteProductTarget(customer.customer_id, item.id)) {
                                            PrintConsole("Product relation removed:" + item.sku + " deleted" + ", " + Constants.IDEASOFT);
                                        }
                                        goto LEAP;
                                    }
                                }
                                if (prepared_product != null) {
                                    #region Ideasoft Category and Brand Sync
                                    List<int> idea_category_ids = Helper.GetIdeaCategoryIds(thread_id, db_helper, customer, ref live_idea_categories,
                                        category_target_relation, prepared_product.extension.categories);
                                    var idea_brand_id = Helper.GetIdeaBrandId(thread_id, db_helper, customer, ref live_idea_brands, prepared_product.extension.brand);
                                    #endregion

                                    #region Ideasoft Product Insert
                                    idea_product_id = Helper.InsertIdeaProduct(prepared_product, idea_brand_id, idea_category_ids);
                                    if (idea_product_id > 0) {
                                        is_processed = true;
                                        PrintConsole("Sku:" + (prepared_product.sku) + " inserted and sync to Id:" + idea_product_id.ToString() + ", " + Constants.IDEASOFT);
                                        db_helper.LogToServer(thread_id, "product_inserted", Helper.global.settings.company_name + " Sku:" + prepared_product.sku + ", " + Constants.IDEASOFT, customer.customer_id, "product");
                                    }
                                    else {
                                        PrintConsole("Sku:" + prepared_product.sku + " insert failed." + " ," + Constants.IDEASOFT);
                                        db_helper.LogToServer(thread_id, "product_insert_error", Helper.global.settings.company_name + " Sku:" + prepared_product.sku + ", " + Constants.IDEASOFT, customer.customer_id, "product");
                                    }
                                    #endregion
                                }
                            }
                        }
                    LEAP:

                        if (is_processed && is_update) {
                            if (db_helper.UpdateProducts(customer.customer_id, [item])) {
                                if (idea_product_id > 0) {
                                    var product_relation = product_target_relation.FirstOrDefault(x => x.target_id == idea_product_id && x.target_name == Constants.IDEASOFT);
                                    if (product_relation != null) {
                                        if (product_relation.product_id != item.id) {
                                            product_relation.product_id = item.id;
                                            db_helper.UpdateProductTarget(customer.customer_id, product_relation);
                                        }
                                    }
                                    else {
                                        db_helper.InsertProductTarget(customer.customer_id, new ProductTarget() {
                                            customer_id = customer.customer_id, product_id = item.id, target_id = idea_product_id, target_name = Constants.IDEASOFT
                                        });
                                    }
                                }
                                PrintConsole("Sku:" + item.sku + " updated on MerchanterDB. " + (updated_attrs.Length > 0 ? ("-" + updated_attrs[..^1]) : ""));
                            }
                            else {
                                PrintConsole("Sku:" + item.sku + " " + "update error on MerchanterDB. " + (updated_attrs.Length > 0 ? ("-" + updated_attrs[..^1]) : ""));
                            }
                        }

                        if (is_processed && is_insert && prepared_product != null) {
                            int inserted_id = db_helper.InsertProducts(customer.customer_id, [prepared_product]);
                            if (inserted_id > 0) {
                                if (idea_product_id > 0)
                                    db_helper.InsertProductTarget(customer.customer_id, new ProductTarget() {
                                        customer_id = customer.customer_id,
                                        product_id = inserted_id,
                                        target_id = idea_product_id,
                                        target_name = Constants.IDEASOFT
                                    });
                                PrintConsole("Sku:" + item.sku + " inserted on MerchanterDB.");
                                db_helper.LogToServer(thread_id, "new_product", Helper.global.settings.company_name + " Sku:" + item.sku, customer.customer_id, "product");
                            }
                            else {
                                PrintConsole("Sku:" + item.sku + " " + "insert error on MerchanterDB.");
                            }
                        }
                    }

                    if (notifications.Count > 0) {
                        db_helper.InsertNotifications(customer.customer_id, notifications);
                    }
                }
            }
        }
        catch (Exception _ex) {
            db_helper.LogToServer(thread_id, "product_update_error", _ex.Message + newline + _ex.ToString(), customer.customer_id, "product");
            _health = false;
        }
    }
    #endregion

    #region Order Threads - OrderLoop, OrderSync
    private void OrderLoop(out bool _health) {
        _health = true;
        try {
            if (order_sources.Contains(Constants.MAGENTO2)) {
                var m2_orders = Helper.GetOrders(Helper.global.order.daysto_ordersync, OrderStatus.GetSyncEnabledCodes(Constants.MAGENTO2));
                if (m2_orders != null && m2_orders.items.Length > 0) {
                    foreach (var item in m2_orders.items) {
                        var o = new Order() {
                            order_id = item.entity_id,
                            order_label = item.increment_id,
                            grand_total = item.grand_total,
                            order_status = OrderStatus.GetStatusOf(item.status, Constants.MAGENTO2),
                            payment_method = PaymentMethod.GetPaymentMethodOf(item.payment.method, Constants.MAGENTO2),
                            shipment_method = ShipmentMethod.GetShipmentMethodOf(item.extension_attributes.shipping_assignments[0].shipping.method, Constants.MAGENTO2),
                            order_date = Convert.ToDateTime(item.updated_at),
                            order_source = Constants.MAGENTO2,
                            currency = item.base_currency_code == "TRY" ? "TL" : item.base_currency_code,
                            email = item.customer_email,
                            firstname = item.customer_firstname,
                            lastname = item.customer_lastname,
                            subtotal = item.grand_total - item.tax_amount,
                            discount_amount = item.discount_amount,
                            comment = item.status_histories.Length > 0 ? string.Join(". ", item.status_histories[0].comment) : string.Empty,
                            billing_address = new BillingAddress() {
                                billing_id = item.billing_address_id,
                                order_id = item.entity_id,
                                firstname = item.billing_address.firstname,
                                lastname = item.billing_address.lastname,
                                telephone = item.billing_address.telephone,
                                street = string.Concat([.. item.billing_address.street]),
                                region = item.billing_address.region,
                                city = item.billing_address.city
                            },
                            shipping_address = new ShippingAddress() {
                                shipping_id = item.billing_address_id,
                                order_id = item.entity_id,
                                firstname = item.extension_attributes.shipping_assignments[0].shipping.address.firstname,
                                lastname = item.extension_attributes.shipping_assignments[0].shipping.address.lastname,
                                telephone = item.extension_attributes.shipping_assignments[0].shipping.address.telephone,
                                street = string.Concat([.. item.extension_attributes.shipping_assignments[0].shipping.address.street]),
                                region = item.extension_attributes.shipping_assignments[0].shipping.address.region,
                                city = item.extension_attributes.shipping_assignments[0].shipping.address.city
                            },
                            order_items = new List<OrderItem>()
                        };

                        #region OLD
                        //#region Corporate Info
                        //var corporate_info = Helper.GetCustomerCorporateInfo( item.customer_email, out bool is_corporate );
                        //if( corporate_info != null && is_corporate ) {
                        //    o.billing_address.is_corporate = is_corporate;
                        //    o.billing_address.tc_no = corporate_info.GetValueOrDefault( Helper.global.magento.customer_tc_no_attribute_code ) ?? Constants.DUMMY_TCNO;
                        //    o.billing_address.firma_ismi = corporate_info.GetValueOrDefault( Helper.global.magento.customer_firma_ismi_attribute_code );
                        //    o.billing_address.firma_vergidairesi = corporate_info.GetValueOrDefault( Helper.global.magento.customer_firma_vergidairesi_attribute_code );
                        //    o.billing_address.firma_vergino = corporate_info.GetValueOrDefault( Helper.global.magento.customer_firma_vergino_attribute_code );
                        //}
                        //else if( corporate_info != null && !is_corporate ) {
                        //    o.billing_address.tc_no = corporate_info.GetValueOrDefault( Helper.global.magento.customer_tc_no_attribute_code ) ?? Constants.DUMMY_TCNO;
                        //}
                        //#endregion 
                        #endregion

                        float calculated_grand_total = 0;
                        foreach (var order_item in item.items) {
                            if (order_item.sku.Contains("secili") || order_item.sku.Contains("seçili")) continue;
                            if (order_item.product_type == "simple") {
                                if (order_item.parent_item != null && order_item.parent_item.product_type == "configurable") {
                                    if (order_item.parent_item.base_price_incl_tax <= 0) continue;
                                    o.order_items.Add(new OrderItem() {
                                        order_id = item.entity_id,
                                        order_item_id = order_item.parent_item.item_id,
                                        sku = order_item.parent_item.sku,
                                        parent_sku = order_item.parent_item.sku,
                                        tax_included = Helper.global.order.siparis_kdvdahilmi,
                                        tax = order_item.parent_item.tax_percent,
                                        price = order_item.parent_item.base_price_incl_tax,
                                        tax_amount = order_item.parent_item.base_price_incl_tax -
                                            (order_item.parent_item.base_price_incl_tax / (1f + (order_item.parent_item.tax_percent / 100f))),
                                        qty_ordered = order_item.parent_item.qty_ordered,
                                        qty_invoiced = order_item.parent_item.qty_invoiced,
                                        qty_cancelled = order_item.parent_item.qty_canceled,
                                        qty_refunded = order_item.parent_item.qty_refunded
                                    });
                                    calculated_grand_total += order_item.parent_item.base_price_incl_tax * order_item.parent_item.qty_ordered;
                                }
                                else {
                                    if (order_item.base_price_incl_tax <= 0) continue;
                                    o.order_items.Add(new OrderItem() {
                                        order_id = item.entity_id,
                                        order_item_id = order_item.item_id,
                                        sku = order_item.sku,
                                        parent_sku = order_item.parent_item?.sku,
                                        tax_included = Helper.global.order.siparis_kdvdahilmi,
                                        tax = order_item.tax_percent,
                                        price = order_item.base_price_incl_tax,
                                        tax_amount = order_item.base_price_incl_tax -
                                            (order_item.base_price_incl_tax / (1f + (order_item.tax_percent / 100f))),
                                        qty_ordered = order_item.qty_ordered,
                                        qty_invoiced = order_item.qty_invoiced,
                                        qty_cancelled = order_item.qty_canceled,
                                        qty_refunded = order_item.qty_refunded
                                    });
                                    calculated_grand_total += order_item.base_price_incl_tax * order_item.qty_ordered;
                                }
                            }
                        }
                        o.installment_amount = item.grand_total + (item.discount_amount * -1) - calculated_grand_total;
                        if (o.installment_amount > 0)
                            o.subtotal = item.grand_total - o.installment_amount - item.tax_amount;
                        live_orders.Add(o);
                    }
                }
                PrintConsole(m2_orders?.items.Length + " " + Constants.MAGENTO2 + " orders loaded.");
            }

            if (order_sources.Contains(Constants.IDEASOFT)) {
                var idea_orders = Helper.GetIdeaOrders(Helper.global.order.daysto_ordersync);
                if (idea_orders != null && idea_orders.Count > 0) {
                    foreach (var item in idea_orders) {
                        var o = new Order {
                            order_id = item.id,
                            order_label = item.transactionId,
                            grand_total = item.finalAmount,
                            order_status = OrderStatus.GetStatusOf(item.status, Constants.IDEASOFT),
                            payment_method = PaymentMethod.GetPaymentMethodOf(item.paymentTypeName, Constants.IDEASOFT),
                            shipment_method = ShipmentMethod.GetShipmentMethodOf(item.shippingProviderCode, Constants.IDEASOFT),
                            order_date = item.createdAt,
                            order_source = Constants.IDEASOFT,
                            currency = item.currency,
                            email = item.customerEmail,
                            firstname = item.customerFirstname,
                            lastname = item.customerSurname,
                            subtotal = item.amount,
                            installment_amount = (item.generalAmount * item.installmentRate) - item.generalAmount,
                            discount_amount = item.couponDiscount + item.promotionDiscount,
                            shipment_amount = item.shippingAmount,
                            comment = item.paymentProviderName + "," + item.paymentGatewayName,
                            billing_address = new BillingAddress() {
                                billing_id = item.billingAddress.id,
                                order_id = item.id,
                                firstname = item.billingAddress.firstname,
                                lastname = item.billingAddress.surname,
                                telephone = item.billingAddress.phoneNumber,
                                street = item.billingAddress.address,
                                region = item.billingAddress.subLocation,
                                city = item.billingAddress.location,
                                is_corporate = item.billingAddress.invoiceType == "corporate",
                                firma_ismi = item.billingAddress.firstname + " " + item.billingAddress.surname,
                                firma_vergidairesi = item.billingAddress.invoiceType == "corporate" ? item.billingAddress.taxOffice.ToString() : "YOK",
                                firma_vergino = item.billingAddress.invoiceType == "corporate" ? item.billingAddress.taxNo.ToString() : ""
                            },
                            shipping_address = new ShippingAddress() {
                                shipping_id = item.shippingAddress.id,
                                order_id = item.id,
                                firstname = item.shippingAddress.firstname,
                                lastname = item.shippingAddress.surname,
                                telephone = item.shippingAddress.phoneNumber,
                                street = item.shippingAddress.address,
                                region = item.shippingAddress.subLocation,
                                city = item.shippingAddress.location
                            },
                            order_items = []
                        };

                        foreach (var order_item in item.orderItems) {
                            o.order_items.Add(new OrderItem() {
                                order_id = item.id,
                                order_item_id = order_item.id,
                                sku = order_item.productSku,
                                price = order_item.productPrice,
                                tax = (int)order_item.productTax,
                                tax_included = Helper.global.order.siparis_kdvdahilmi,
                                qty_ordered = (int)order_item.productQuantity,
                                tax_amount = order_item.productPrice * (order_item.productTax / 100f)
                            });
                        }
                        o.order_shipping_barcode = available_shipments.Length > 0 ? null : not_available;

                        live_orders.Add(o);
                    }
                }
                PrintConsole(idea_orders?.Count + " " + Constants.IDEASOFT + " orders loaded.");
            }
        }
        catch (Exception _ex) {
            db_helper.LogToServer(thread_id, "order_source_error", _ex.Message + newline + _ex.ToString(), customer.customer_id, "order");
            _health = false;
        }
        finally {
            PrintConsole(live_orders.Count + " total live orders loaded.");
        }
    }

    private void OrderSync(out bool _health) {
        _health = true;
        try {
            if (orders != null) {
                List<Notification> notifications = [];
                foreach (var order_item in live_orders) {
                    var selected_order = orders.Where(x => x.order_id == order_item.order_id).FirstOrDefault();
                    if (selected_order != null) {
                        if (!string.IsNullOrWhiteSpace(selected_order.order_shipping_barcode)) {
                            if (!selected_order.is_erp_sent) {  //process
                                string? inserted_musteri_siparis_no = null;

                                if (order_sources.Contains(Constants.MAGENTO2)) {
                                    #region Fill Corporate Info for Order
                                    var corporate_info = Helper.GetCustomerCorporateInfo(order_item.email, out bool is_corporate);
                                    if (corporate_info != null && is_corporate) {
                                        order_item.billing_address.is_corporate = is_corporate;
                                        order_item.billing_address.tc_no = corporate_info.GetValueOrDefault(Helper.global.magento.customer_tc_no_attribute_code) ?? Constants.DUMMY_TCNO;
                                        order_item.billing_address.firma_ismi = corporate_info.GetValueOrDefault(Helper.global.magento.customer_firma_ismi_attribute_code);
                                        order_item.billing_address.firma_vergidairesi = corporate_info.GetValueOrDefault(Helper.global.magento.customer_firma_vergidairesi_attribute_code);
                                        order_item.billing_address.firma_vergino = corporate_info.GetValueOrDefault(Helper.global.magento.customer_firma_vergino_attribute_code);
                                    }
                                    else if (corporate_info != null && !is_corporate) {
                                        order_item.billing_address.tc_no = corporate_info.GetValueOrDefault(Helper.global.magento.customer_tc_no_attribute_code) ?? Constants.DUMMY_TCNO;
                                    }
                                    #endregion
                                }

                                if (order_main_target != null && order_main_target == Constants.NETSIS) {
                                    if (Helper.global.netsis != null && Helper.global.netsis.netopenx_user != null && Helper.global.netsis.netopenx_password != null) {
                                        string? inserted_cari_kodu = NetOpenXHelper.InsertNetsisCari(order_item.billing_address.billing_id.ToString(), order_item.email, order_item.billing_address, order_item.payment_method);
                                        if (inserted_cari_kodu != null) {
                                            string? musteri_selected_siparis_no = NetOpenXHelper.GetNetsisSiparis(order_item.order_id.ToString());
                                            if (Helper.global.order.is_rewrite_siparis && !string.IsNullOrWhiteSpace(musteri_selected_siparis_no)) {
                                                //TODO: rewrite ?
                                            }
                                            inserted_musteri_siparis_no = NetOpenXHelper.InsertNetsisMusSiparis(order_item, inserted_cari_kodu, selected_order.order_shipping_barcode);
                                            if (!string.IsNullOrWhiteSpace(inserted_musteri_siparis_no)) {
                                                db_helper.LogToServer(thread_id, "new_order", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + order_item.grand_total.ToString() + order_item.currency, customer.customer_id, "order");
                                                #region Notify Order - NEW_ORDER
                                                notifications.Add(new Notification() {
                                                    customer_id = customer.customer_id,
                                                    type = Notification.NotificationTypes.NEW_ORDER,
                                                    order_label = order_item.order_label,
                                                    notification_content = Constants.NETSIS,
                                                    is_notification_sent = true
                                                });
                                                #endregion
                                            }
                                            else {
                                                db_helper.LogToServer(thread_id, "new_order_error", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + order_item.grand_total.ToString() + order_item.currency, customer.customer_id, "order");
                                                #region Notify Order - NEW_ORDER_ERROR
                                                notifications.Add(new Notification() {
                                                    customer_id = customer.customer_id,
                                                    type = Notification.NotificationTypes.NEW_ORDER_ERROR,
                                                    order_label = order_item.order_label,
                                                    notification_content = Constants.NETSIS,
                                                    is_notification_sent = true
                                                });
                                                #endregion
                                            }
                                        }
                                    }
                                }

                                if (order_main_target != null && order_main_target == Constants.ANK_ERP) {
                                    if (selected_order.order_status == "YENİ_SİPARİŞ") {
                                        if (Helper.global.ank_erp != null && Helper.global.ank_erp.company_code != null && Helper.global.ank_erp.user_name != null && Helper.global.ank_erp.password != null && Helper.global.ank_erp.work_year != null && Helper.global.ank_erp.url != null) {
                                            ANKERP ank_erp = new(Helper.global.ank_erp.company_code, Helper.global.ank_erp.user_name, Helper.global.ank_erp.password, Helper.global.ank_erp.work_year, Helper.global.ank_erp.url,
                                            """C:\MerchanterServer\ankaraerp""");
                                            Guid guid = Guid.NewGuid(); bool health = true;

                                            #region ANKARA ERP - TICARI DOKUMAN
                                            #region CORE
                                            TicariDokumanSip dokuman = new() {
                                                DokumanBaslik = new DokumanBaslikSip() {
                                                    Versiyon = "1.0",
                                                    Gonderen = new GonderenSip() {
                                                        VergiNo = "0123456789",
                                                        Unvani = "OTO AHMET AHMET AVCI",
                                                        Tanimlayici = "WSU003"
                                                    },
                                                    Alici = new AliciSip() {
                                                        VergiNo = "0123456789",
                                                        Unvani = "parcamatik",
                                                        Tanimlayici = "WSU003"
                                                    },
                                                    DokumanTanimi = new DokumanTanimiSip() {
                                                        Turu = "GIDEN",
                                                        Versiyon = "1.0",
                                                        DosyaAdi = guid.ToString(),
                                                        OlusturulmaZamani = DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss"),
                                                    }
                                                },
                                                DokumanPaket = new DokumanPaketSip() {
                                                    Eleman = new ElemanSip() {
                                                        ElemanTipi = "TICARIBELGE",
                                                        ElemanSayisi = 1,
                                                        ElemanListe = new ElemanListeSip() {
                                                            BelgeSicil = new BelgeSicilSip() {
                                                                Baslik = new BaslikSip() {
                                                                    Aciliyet = "NORMAL",
                                                                    HareketKodu = string.Empty,
                                                                    ProjeNo = string.Empty,
                                                                    BelgeNo = order_item.order_id + 500000,
                                                                    MBelgeNo = order_item.order_id + 500000,
                                                                    OzelKod = (order_item.order_id + 500000).ToString(),
                                                                    DovizKodu = "TRL",
                                                                    DovizKuru = 1,
                                                                    HariciNumara = string.Empty,
                                                                    TanzimTarihi = order_item.order_date.ToString("dd-MM-yyyy"),
                                                                    TanzimSaati = order_item.order_date.ToString("HH:mm"),
                                                                    TeslimTarihi = order_item.order_date.ToString("dd-MM-yyyy"),
                                                                    PlasiyerKodu = "B2C",
                                                                    OdemeSekli = order_item.payment_method == "BANKA_TRANSFERİ" ? "EFT/HAVALE" : "KAPIDAODEME",
                                                                    OdemeTarihi = order_item.order_date.ToString("dd-MM-yyyy"),
                                                                    OdemeRefNo = order_item.order_label,
                                                                    OdemeTutar = Math.Round((decimal)order_item.grand_total, 2, MidpointRounding.AwayFromZero),
                                                                    OdemeNotu = order_item.payment_method == "BANKA_TRANSFERİ" ? "ZIBN2" : "PYTR",
                                                                    MalTutar = Math.Round((decimal)order_item.grand_total - (decimal)order_item.shipment_amount, 2, MidpointRounding.AwayFromZero),
                                                                    MalIndTutar = Math.Round((decimal)order_item.discount_amount, 2, MidpointRounding.AwayFromZero),
                                                                    HizmetTutar = Math.Round((decimal)order_item.shipment_amount, 2, MidpointRounding.AwayFromZero),
                                                                    HizmetIndTutar = 0,
                                                                    OtvMatrah = 0,
                                                                    OtvOran = 0,
                                                                    OtvTutar = 0,
                                                                    KdvMatrah = 0,
                                                                    KdvTutar = 0,
                                                                    BelgeTutar = 0
                                                                },
                                                                BaslikNot = string.Empty,
                                                                Dipnot = string.Empty,
                                                                SatirDetay = [],
                                                                CariSicil = new CariSicilSip() {
                                                                    HesapNo = string.Empty,
                                                                    Adi = order_item.billing_address.firstname.Trim().ToUpper(),
                                                                    Soyadi = order_item.billing_address.lastname.Trim().ToUpper(),
                                                                    Unvani = order_item.billing_address.is_corporate ? order_item.billing_address.firma_ismi.Trim().ToUpper() : (order_item.firstname.Trim().ToUpper() + " " + order_item.lastname.Trim().ToUpper()),
                                                                    Adres1 = order_item.billing_address.street.Length > 50 ? string.Join(" ", order_item.billing_address.street.Split(' ').TakeWhile((s, i) => string.Join(" ", order_item.billing_address.street.Split(' ').Take(i + 1)).Length <= 50)).Trim().ToUpper() : order_item.billing_address.street.Trim().ToUpper(),
                                                                    Adres2 = order_item.billing_address.street.Length > 50 ? string.Join(" ", order_item.billing_address.street.Split(' ').SkipWhile((s, i) => string.Join(" ", order_item.billing_address.street.Split(' ').Take(i + 1)).Length <= 50).TakeWhile((s, i) => string.Join(" ", order_item.billing_address.street.Split(' ').SkipWhile((s, j) => string.Join(" ", order_item.billing_address.street.Split(' ').Take(j + 1)).Length <= 50).Take(i + 1)).Length <= 50)).Trim().ToUpper() : string.Empty,
                                                                    Adres3 = order_item.billing_address.street.Length > 100 ? string.Join(" ", order_item.billing_address.street.Split(' ').SkipWhile((s, i) => string.Join(" ", order_item.billing_address.street.Split(' ').Take(i + 1)).Length <= 100)).Trim().ToUpper() : string.Empty,
                                                                    Ilce = order_item.billing_address.region.Trim().ToUpper(),
                                                                    Sehir = order_item.billing_address.city.Trim().ToUpper(),
                                                                    Ulke = "TÜRKİYE",
                                                                    Telefon = order_item.billing_address.telephone?.Replace("+9", "").Replace(" ", "").Replace("(", "").Replace(")", ""),
                                                                    GsmNo = order_item.billing_address.telephone?.Replace("+9", "").Replace(" ", "").Replace("(", "").Replace(")", ""),
                                                                    FaksNo = string.Empty,
                                                                    EPosta = order_item.email,
                                                                    WebSiteURL = string.Empty,
                                                                    VergiNo = order_item.billing_address.is_corporate ? order_item.billing_address.firma_vergino : order_item.billing_address.tc_no,
                                                                    VergiDaireAdi = order_item.billing_address.is_corporate ? order_item.billing_address.firma_vergidairesi.Trim().ToUpper() : "YOK",
                                                                    OzelKod = string.Empty, OzelKod1 = string.Empty, OzelKod2 = string.Empty, OzelKod3 = string.Empty,
                                                                    PostaKodu = string.Empty, Notlar = string.Empty
                                                                },
                                                                SevkYeri = new SevkYeriSip() {
                                                                    HesapNo = string.Empty,
                                                                    Adi = order_item.shipping_address.firstname.Trim().ToUpper(),
                                                                    Soyadi = order_item.shipping_address.lastname.Trim().ToUpper(),
                                                                    Unvani = order_item.shipping_address.firstname.Trim().ToUpper() + " " + order_item.shipping_address.lastname.Trim().ToUpper(),

                                                                    Adres1 = order_item.shipping_address.street.Length > 50 ? string.Join(" ", order_item.shipping_address.street.Split(' ').TakeWhile((s, i) => string.Join(" ", order_item.shipping_address.street.Split(' ').Take(i + 1)).Length <= 50)).Trim().ToUpper() : order_item.shipping_address.street.Trim().ToUpper(),
                                                                    Adres2 = order_item.shipping_address.street.Length > 50 ? string.Join(" ", order_item.shipping_address.street.Split(' ').SkipWhile((s, i) => string.Join(" ", order_item.shipping_address.street.Split(' ').Take(i + 1)).Length <= 50).TakeWhile((s, i) => string.Join(" ", order_item.shipping_address.street.Split(' ').SkipWhile((s, j) => string.Join(" ", order_item.shipping_address.street.Split(' ').Take(j + 1)).Length <= 50).Take(i + 1)).Length <= 50)).Trim().ToUpper() : string.Empty,
                                                                    Adres3 = order_item.shipping_address.street.Length > 100 ? string.Join(" ", order_item.shipping_address.street.Split(' ').SkipWhile((s, i) => string.Join(" ", order_item.shipping_address.street.Split(' ').Take(i + 1)).Length <= 100)).Trim().ToUpper() : string.Empty,
                                                                    Ilce = order_item.shipping_address.region.Trim().ToUpper(),
                                                                    Sehir = order_item.shipping_address.city.Trim().ToUpper(),
                                                                    Ulke = "TÜRKİYE",
                                                                    Telefon = order_item.shipping_address.telephone.Replace("+9", "").Replace(" ", "").Replace("(", "").Replace(")", ""),
                                                                    GsmNo = order_item.shipping_address.telephone.Replace("+9", "").Replace(" ", "").Replace("(", "").Replace(")", ""),
                                                                    FaksNo = string.Empty,
                                                                    EPosta = order_item.email,
                                                                    WebSiteURL = string.Empty,
                                                                    VergiNo = order_item.billing_address.is_corporate ? order_item.billing_address.firma_vergino : order_item.billing_address.tc_no,
                                                                    VergiDaireAdi = order_item.billing_address.is_corporate ? order_item.billing_address.firma_vergidairesi.Trim().ToUpper() : "YOK",
                                                                    OzelKod = string.Empty, //assign order_shipping_barcode maybe
                                                                    KargoKodu = order_item.shipment_method == Constants.ARAS ? "ARAS" : order_item.shipment_method,
                                                                    PostaKodu = string.Empty, Notlar = string.Empty
                                                                },
                                                                Irsaliye = string.Empty,
                                                                Siparis = string.Empty,
                                                                IsEmri = string.Empty,
                                                                BELTUR = "SIPARIS",
                                                                MS = "M"
                                                            }
                                                        }
                                                    }
                                                }
                                            };
                                            #endregion

                                            #region ORDER ITEMS
                                            foreach (var item in order_item.order_items) {
                                                var sold_product = db_helper.GetProductBySku(customer.customer_id, item.sku);
                                                if (sold_product != null) {
                                                    dokuman.DokumanPaket.Eleman.ElemanListe.BelgeSicil.SatirDetay.Add(new SatirDetaySip() {
                                                        Items = new ItemsSip() {
                                                            BarkodKodu = sold_product.barcode,
                                                            UrunGrubu = sold_product.extension.brand.brand_name.Trim().ToUpper(),
                                                            UrunKodu = item.sku,
                                                            UrunTanim = sold_product.name?.Trim().ToUpper() ?? string.Empty,
                                                            Miktar = (byte)item.qty_ordered,
                                                            OlcuBirim = "ADET",
                                                            BirimFiyat = Math.Round((decimal)(item.price + item.tax_amount), 2, MidpointRounding.AwayFromZero),
                                                            Tutar = Math.Round((decimal)((item.price + item.tax_amount) * item.qty_ordered), 2, MidpointRounding.AwayFromZero),
                                                            IndOran = 0,
                                                            KdvOran = (byte)item.tax,
                                                            Notlar = string.Empty
                                                        },
                                                        StokSicil = new StokSicilSip() {
                                                            UrunGrubu = sold_product.extension.brand.brand_name.Trim().ToUpper(),
                                                            UrunKodu = item.sku,
                                                            UrunTanim = sold_product.name?.Trim().ToUpper() ?? string.Empty,
                                                            OlcuBirim = "ADET",
                                                            KdvOran = (byte)item.tax,
                                                        },
                                                        HizmasSicil = new HizmasSicilSip() {
                                                            UrunGrubu = sold_product.extension.brand.brand_name.Trim().ToUpper(),
                                                            UrunKodu = item.sku,
                                                            UrunTanim = sold_product.name?.Trim().ToUpper() ?? string.Empty,
                                                            OlcuBirim = "ADET",
                                                            KdvOran = (byte)item.tax,
                                                        }
                                                    });
                                                }
                                                else { health = false; }
                                            }
                                            #endregion

                                            #region ORDER SHIPMENT
                                            if (order_item.shipment_amount > 0) {
                                                dokuman.DokumanPaket.Eleman.ElemanListe.BelgeSicil.SatirDetay.Add(new SatirDetaySip() {
                                                    Items = new ItemsSip() {
                                                        BarkodKodu = string.Empty,
                                                        UrunGrubu = "GELİR",
                                                        UrunKodu = Helper.global.order.siparis_kargo_sku,
                                                        UrunTanim = "ARAS KARGO KARGO ÜCRETİ",
                                                        Miktar = 1,
                                                        OlcuBirim = "ADET",
                                                        BirimFiyat = Math.Round((decimal)order_item.shipment_amount, 2, MidpointRounding.AwayFromZero),
                                                        Tutar = Math.Round((decimal)order_item.shipment_amount, 2, MidpointRounding.AwayFromZero),
                                                        IndOran = 0,
                                                        KdvOran = 20,
                                                        Notlar = string.Empty
                                                    },
                                                    StokSicil = new StokSicilSip() {
                                                        UrunGrubu = "GELİR",
                                                        UrunKodu = Helper.global.order.siparis_kargo_sku,
                                                        UrunTanim = "ARAS KARGO",
                                                        OlcuBirim = "ADET",
                                                        KdvOran = 20,
                                                    },
                                                    HizmasSicil = new HizmasSicilSip() {
                                                        UrunGrubu = "GELİR",
                                                        UrunKodu = Helper.global.order.siparis_kargo_sku,
                                                        UrunTanim = "ARAS KARGO",
                                                        OlcuBirim = "ADET",
                                                        KdvOran = 20,
                                                    }
                                                });
                                            }
                                            #endregion
                                            #endregion

                                            if (!health) continue;

                                            var serializer = new XmlSerializer(dokuman.GetType());
                                            using var stringwriter = new Utf8StringWriter();
                                            var ns = new XmlSerializerNamespaces();
                                            ns.Add("td", "http://www.ankarayazilim.com/TicariDokumanZarfi");
                                            serializer.Serialize(stringwriter, dokuman, ns);
                                            var order_xml = stringwriter.ToString();
                                            if (!string.IsNullOrWhiteSpace(order_xml)) {
                                                inserted_musteri_siparis_no = ank_erp.SendOrder(guid.ToString(), order_xml).Result;
                                                if (!string.IsNullOrWhiteSpace(inserted_musteri_siparis_no)) {
                                                    db_helper.LogToServer(thread_id, "new_order", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + order_item.grand_total.ToString() + order_item.currency, customer.customer_id, "order");
                                                    #region Notify Order - NEW_ORDER
                                                    notifications.Add(new Notification() {
                                                        customer_id = customer.customer_id,
                                                        type = Notification.NotificationTypes.NEW_ORDER,
                                                        order_label = order_item.order_label,
                                                        notification_content = Constants.ANK_ERP,
                                                        is_notification_sent = true
                                                    });
                                                    #endregion
                                                }
                                                else {
                                                    db_helper.LogToServer(thread_id, "new_order_error", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + order_item.grand_total.ToString() + order_item.currency, customer.customer_id, "order");
                                                    #region Notify Order - NEW_ORDER_ERROR
                                                    notifications.Add(new Notification() {
                                                        customer_id = customer.customer_id,
                                                        type = Notification.NotificationTypes.NEW_ORDER_ERROR,
                                                        order_label = order_item.order_label,
                                                        notification_content = Constants.ANK_ERP,
                                                        is_notification_sent = true
                                                    });
                                                    #endregion
                                                }
                                            }
                                            else {
                                                db_helper.LogToServer(thread_id, "order_process_error", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + "XML Serialization Error " + Constants.ANK_ERP, customer.customer_id, "order");
                                            }
                                        }
                                    }
                                }

                                if (inserted_musteri_siparis_no != null) {
                                    if (order_sources.Contains(Constants.MAGENTO2) && order_item.order_status != "HAZIRLANIYOR") { //processed | do order status change => processing
                                        if (!string.IsNullOrWhiteSpace(Helper.CreateOrderInvoice(order_item))) {
                                            Helper.ChangeOrderStatus(order_item, Helper.global.order_statuses.Where(x => x.status_code == "HAZIRLANIYOR" && x.platform == Constants.MAGENTO2).FirstOrDefault()?.platform_status_code);
                                            db_helper.LogToServer(thread_id, "order_process", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + Helper.global.order_statuses.Where(x => x.status_code == "HAZIRLANIYOR" && x.platform == Constants.MAGENTO2).FirstOrDefault()?.platform_status_code, customer.customer_id, "order");
                                            #region Notify Order - ORDER_PROCESS
                                            notifications.Add(new Notification() {
                                                customer_id = customer.customer_id,
                                                type = Notification.NotificationTypes.ORDER_PROCESS,
                                                order_label = order_item.order_label,
                                                notification_content = inserted_musteri_siparis_no,
                                                is_notification_sent = true
                                            });
                                            #endregion
                                        }
                                    }

                                    if (order_sources.Contains(Constants.IDEASOFT) && order_item.order_status == "YENİ_SİPARİŞ") {

                                    }

                                    if (db_helper.SetOrderProcess(customer.customer_id, order_item.order_id, inserted_musteri_siparis_no)) {
                                        db_helper.LogToServer(thread_id, "order_processed", "Order:" + order_item.order_source + ":" + order_item.order_label + ":" + inserted_musteri_siparis_no + " => " + order_item.grand_total.ToString() + order_item.currency, customer.customer_id, "order");
                                        #region Notify Order - NEW_ORDER, OUT_OF_STOCK_PRODUCT_SOLD
                                        foreach (var sold_item in order_item.order_items) {
                                            if (live_products.Count > 0) {
                                                var sold_product = live_products.Where(x => x.sku == sold_item.sku).FirstOrDefault();
                                                if (sold_product != null) {
                                                    var sold_product_sources = db_helper.GetProductSources(customer.customer_id, sold_product.sku);
                                                    if (sold_product_sources != null) {
                                                        var sold_product_main_source = sold_product_sources.Where(x => x.name == product_main_source).FirstOrDefault();
                                                        if (sold_product_main_source != null) {
                                                            if (sold_product_main_source.qty <= 0) {
                                                                db_helper.LogToServer(thread_id, "out_of_stock_product_sold", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + sold_product.sku, customer.customer_id, "product");
                                                                notifications.Add(new Notification() { customer_id = customer.customer_id, type = Notification.NotificationTypes.OUT_OF_STOCK_PRODUCT_SOLD, order_label = order_item.order_label, product_sku = sold_product.sku, xproduct_barcode = sold_product.barcode });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else {  //query ship
                                if (order_item.order_shipping_barcode != not_available) {
                                    List<string> tracking_codes = [];
                                    if (Helper.global.shipment.yurtici_kargo && available_shipments.Contains(Constants.YURTICI)) {
                                        if (Helper.global.shipment.yurtici_kargo_user_name != null && Helper.global.shipment.yurtici_kargo_password != null) {
                                            YK yk = new YK(Helper.global.shipment.yurtici_kargo_user_name, Helper.global.shipment.yurtici_kargo_password);
                                            List<string>? tk = yk.GetShipment(selected_order.order_shipping_barcode);
                                            if (tk != null) {
                                                tracking_codes.AddRange(tk);
                                                //Debug.WriteLine( order_item.order_shipping_barcode + ":TK:" + string.Join( "|", tk ) );
                                            }
                                        }
                                    }
                                    if (Helper.global.shipment.mng_kargo && available_shipments.Contains(Constants.MNG)) { }
                                    if (Helper.global.shipment.aras_kargo && available_shipments.Contains(Constants.ARAS)) { }

                                    if (tracking_codes.Count > 0) { //insert tracking code | order status change => complete
                                        Shipment? shipment = db_helper.GetShipment(customer.customer_id, order_item.order_id);
                                        if (shipment != null) {
                                            if (!shipment.is_shipped) {
                                                if (order_sources.Contains(Constants.MAGENTO2) && order_item.order_status != "TAMAMLANDI") {
                                                    Helper.CreateOrderShipment(order_item.order_id, order_item.order_label, string.Join(",", tracking_codes),
                                                        "Siparişiniz " + ShipmentMethod.GetShipmentName(shipment.shipment_method) + " ile kargolanmıştır. " +
                                                        "Kargo Takip Numaranız: " + string.Join(",", tracking_codes) + "." +
                                                        " Detaylı takip için: https://www.yurticikargo.com/tr/online-servisler/gonderi-sorgula?code=" +
                                                        string.Join(",", tracking_codes),
                                                        order_item.shipment_method, ShipmentMethod.GetShipmentName(order_item.shipment_method));
                                                    db_helper.LogToServer(thread_id, "order_process", "Order:" + order_item.order_source + ":" + order_item.order_label + " => " + Helper.global.order_statuses.Where(x => x.status_code == "TAMAMLANDI" && x.platform == Constants.MAGENTO2).FirstOrDefault()?.platform_status_code, customer.customer_id, "order");
                                                    #region Notify Order - ORDER_COMPLETE
                                                    notifications.Add(new Notification() {
                                                        customer_id = customer.customer_id,
                                                        type = Notification.NotificationTypes.ORDER_COMPLETE,
                                                        order_label = order_item.order_label,
                                                        notification_content = order_item.order_label + " => " + string.Join(",", tracking_codes),
                                                        is_notification_sent = true
                                                    });
                                                    #endregion
                                                }

                                                if (db_helper.SetShipped(customer.customer_id, order_item.order_label, string.Join(",", tracking_codes))) {
                                                    db_helper.LogToServer(thread_id, "order_shipped", order_item.order_source + ":" + order_item.order_label + ":" + shipment.barcode + " => " + string.Join(",", tracking_codes), customer.customer_id, "order");
                                                    #region Notify Order - ORDER_SHIPPED
                                                    notifications.Add(new Notification() {
                                                        customer_id = customer.customer_id,
                                                        type = Notification.NotificationTypes.ORDER_SHIPPED,
                                                        order_label = order_item.order_label,
                                                        notification_content = shipment.barcode + " => " + string.Join(",", tracking_codes),
                                                        is_notification_sent = true
                                                    });
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (selected_order.order_status != order_item.order_status) {
                                db_helper.UpdateOrders(customer.customer_id, [order_item]);
                            }
                        }
                        else {  //insert shipment barcodes
                            if (Helper.global.shipment.yurtici_kargo && available_shipments.Contains(Constants.YURTICI)) {
                                if (Helper.global.shipment.yurtici_kargo_user_name != null && Helper.global.shipment.yurtici_kargo_password != null) {
                                    YK yk = new YK(Helper.global.shipment.yurtici_kargo_user_name, Helper.global.shipment.yurtici_kargo_password);
                                    order_item.order_shipping_barcode ??= yk.InsertShipment(order_item.order_source, order_item.order_label, order_item.shipping_address.firstname + " " + order_item.shipping_address.lastname, order_item.shipping_address.street, order_item.shipping_address.telephone, order_item.shipping_address.city, order_item.shipping_address.region);
                                }
                                if (order_item.order_shipping_barcode != null) {
                                    Shipment shipment = new() {
                                        order_id = order_item.order_id,
                                        order_label = order_item.order_label,
                                        order_source = order_item.order_source,
                                        shipment_method = order_item.shipment_method,
                                        tracking_number = null,
                                        shipment_date = null,
                                        order_date = order_item.order_date,
                                        barcode = order_item.order_shipping_barcode,
                                        is_shipped = false
                                    };
                                    if (db_helper.InsertShipments(customer.customer_id, [shipment])) {
                                        if (db_helper.SetOrderShipmentBarcode(customer.customer_id, order_item.order_id, shipment.barcode)) {
                                            PrintConsole(Constants.YURTICI + " ShipmentInserted:" + shipment.order_id + ":" + shipment.order_label + ":" + shipment.order_source + " => " + shipment.barcode);
                                            db_helper.LogToServer(thread_id, "shipment_inserted", _message: string.Format("{1}-shipment_inserted {0}", shipment.order_id + ":" + shipment.order_label + ":" + shipment.order_source + " => " + shipment.barcode, customer.customer_id), customer.customer_id, "shipment");
                                        }
                                    }
                                }
                                else {
                                    PrintConsole(Constants.YURTICI + " ShipmentError:" + order_item.order_id + ":" + order_item.order_label + ":" + order_item.order_source + " => " + order_item.shipment_method);
                                    db_helper.LogToServer(thread_id, "shipment_error", string.Format("{1}-shipment_error {0}", order_item.order_id + ":" + order_item.order_label + ":" + order_item.order_source + " => " + order_item.shipment_method, customer.customer_id), customer.customer_id, "shipment");
                                }
                            }
                            else if (Helper.global.shipment.mng_kargo && available_shipments.Contains(Constants.MNG)) { }
                            else if (Helper.global.shipment.aras_kargo && available_shipments.Contains(Constants.ARAS)) { }
                        }
                    }
                    else {  //update
                        selected_order = db_helper.GetOrder(customer.customer_id, order_item.order_id);
                        if (selected_order != null) {
                            if (selected_order.order_status != order_item.order_status) {
                                db_helper.UpdateOrders(customer.customer_id, [order_item]);
                                //TODO: Can check need to delete processed order ?
                            }
                        }
                        else {
                            db_helper.InsertOrders(customer.customer_id, [order_item]);
                        }
                    }
                }

                if (notifications.Count > 0) {
                    db_helper.InsertNotifications(customer.customer_id, notifications);
                }
            }
        }
        catch (Exception _ex) {
            db_helper.LogToServer(thread_id, "order_update_error", _ex.Message + newline + _ex.ToString(), customer.customer_id, "order");
            _health = false;
        }
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Writes console and debug messages.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="is_console">Is Console ?</param>
    /// <param name="is_debug">Is Debug ?</param>
    private static void PrintConsole(string message, bool is_console = true, bool is_debug = true) {
        if (is_console)
            Console.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + Helper.global.settings.company_name + ", " + message);
        if (is_debug)
            Debug.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + Helper.global.settings.company_name + ", " + message);
    }

    /// <summary>
    /// Writes colorful console messages.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="color">Color</param>
    private static void PrintConsole(string message, ConsoleColor color) {
        Console.BackgroundColor = color;
        Console.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + Helper.global.settings.company_name + ", " + message);
        Console.BackgroundColor = ConsoleColor.White;
    }
    #endregion
}