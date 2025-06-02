using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public record class ProductExtension {
        /// <summary>
        /// Tracks the source of each property value for this product instance.
        /// </summary>
        [JsonIgnore]
        public List<PropertyMapping> property_mappings { get; set; } = [];
        public int id { get; set; }
        public int customer_id { get; set; }
        public bool is_enabled { get; set; } = true;
        public string sku { get; set; }
        public string barcode { get; set; }
        public int brand_id { get; set; }
        public string category_ids { get; set; }
        public bool is_xml_enabled { get; set; } = false;
        public string[]? xml_sources { get; set; } = [];
        public decimal weight { get; set; } = 0;
        public decimal volume { get; set; } = 0;
        public string? description { get; set; }
        public DateTime update_date { get; set; } = DateTime.Now;
        public List<Category> categories { get; set; }
        public Brand brand { get; set; }

        public static List<PropertyMapping> SetDefaultPropertyMappings_forQP() {
            return
            [
                new() { id = 0, property = nameof(sku), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.SKU },
                new() { id = 1, property = nameof(is_enabled), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 2, property = nameof(brand_id), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 3, property = nameof(category_ids), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 4, property = nameof(is_xml_enabled), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 5, property = nameof(xml_sources), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 6, property = nameof(weight), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 7, property = nameof(volume), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 8, property = nameof(description), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 9, property = nameof(barcode), source = Constants.NETSIS, sync_type = PropertyMapping.SyncType.OnlyInsert },
            ];
        }

        public static List<PropertyMapping> SetDefaultPropertyMappings_forANK_ERP() {
            return
            [
                new() { id = 0, property = nameof(sku), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.SKU },
                new() { id = 1, property = nameof(is_enabled), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 2, property = nameof(brand_id), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.Brand },
                new() { id = 3, property = nameof(category_ids), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.Category },
                new() { id = 4, property = nameof(is_xml_enabled), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 5, property = nameof(xml_sources), source = Constants.MERCHANTER, sync_type = PropertyMapping.SyncType.None },
                new() { id = 6, property = nameof(weight), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() { id = 7, property = nameof(volume), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() { id = 8, property = nameof(description), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
                new() { id = 9, property = nameof(barcode), source = Constants.ANK_ERP, sync_type = PropertyMapping.SyncType.FullSync },
            ];
        }
    }
}
