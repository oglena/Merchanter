namespace Merchanter.Classes {
    public class Product {
        public int id { get; set; }
        public int source_product_id { get; set; }
        public int customer_id { get; set; }
        public DateTime update_date { get; set; }
        public string sku { get; set; }
        public ProductTypes type { get; set; } = ProductTypes.SIMPLE;
        public string barcode { get; set; }
        public string? name { get; set; } = string.Empty;
        public int total_qty { get; set; } = 0;
        public decimal price { get; set; }
        public decimal special_price { get; set; }
        public decimal custom_price { get; set; }
        public string currency { get; set; }
        public int tax { get; set; } = 20;
        public bool tax_included { get; set; } = false;
        public List<ProductSource> sources { get; set; }
        public ProductExtension extension { get; set; }

        public enum ProductTypes {
            SIMPLE = 0,
            CONFIGURABLE = 1,
            GROUPED = 2,
            BUNDLE = 3
        }
    }
}
