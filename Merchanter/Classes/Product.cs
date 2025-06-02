using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public record class Product {
        [JsonIgnore]
        public List<PropertyMapping> property_mappings { get; set; } = [];
        public int id { get; set; }
        public int source_product_id { get; set; }
        public int customer_id { get; set; }
        public DateTime update_date { get; set; } = DateTime.Now;
        public string sku { get; set; }
        public ProductTypes type { get; set; } = ProductTypes.SIMPLE;
        public string barcode { get; set; }
        public string? name { get; set; } = string.Empty;
        public int total_qty { get; set; } = 0;
        public decimal price { get; set; }
        public decimal special_price { get; set; }
        public decimal custom_price { get; set; }
        public Currency currency { get; set; } = Currency.GetCurrency("TL");
        public int tax { get; set; } = 20;
        public bool tax_included { get; set; } = false;
        public List<ProductSource> sources { get; set; }
        public ProductExtension extension { get; set; }
        public List<ProductPrice> target_prices { get; set; } = [];
        public List<ProductImage> images { get; set; } = [];
        public List<ProductAttribute> attributes { get; set; } = [];

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ProductTypes {
            SIMPLE = 0,
            CONFIGURABLE = 1,
            GROUPED = 2,
            BUNDLE = 3
        }

        public static List<PropertyMapping> SetDefaultPropertyMappings_forQP() {
            return
            [
                new() {id = 0, property = nameof(source_product_id), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 1, property = nameof(sku), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.SKU },
                new() {id = 2, property = nameof(type), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 3, property = nameof(barcode), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.OnlyInsert },
                new() {id = 4, property = nameof(name), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.OnlyInsert },
                new() {id = 5, property = nameof(total_qty), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 6, property = nameof(price), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 7, property = nameof(special_price), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 8, property = nameof(custom_price), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 9, property = nameof(currency), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.Currency },
                new() {id = 10, property = nameof(tax), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.OnlyInsert },
                new() {id = 11, property = nameof(tax_included), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 12, property = nameof(images), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 13, property = nameof(attributes), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 14, property = nameof(target_prices), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None }
            ];
        }

        public static List<PropertyMapping> SetDefaultPropertyMappings_forANK_ERP() {
            return
            [
                new() {id = 0, property = nameof(source_product_id), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 1, property = nameof(sku), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.SKU },
                new() {id = 2, property = nameof(type), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 3, property = nameof(barcode), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 4, property = nameof(name), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 5, property = nameof(total_qty), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 6, property = nameof(price), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 7, property = nameof(special_price), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 8, property = nameof(custom_price), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 9, property = nameof(currency), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.Currency },
                new() {id = 10, property = nameof(tax), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 11, property = nameof(tax_included), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() {id = 12, property = nameof(images), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.Image },
                new() {id = 13, property = nameof(attributes), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() {id = 14, property = nameof(target_prices), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None }
            ];
        }
    }
}
