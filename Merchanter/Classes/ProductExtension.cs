namespace Merchanter.Classes {
    public class ProductExtension {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string sku { get; set; }
        public int brand_id { get; set; }
        public string category_ids { get; set; }
        public string barcode { get; set; }
        public bool is_xml_enabled { get; set; }
        public string[] xml_sources { get; set; }
        public DateTime? update_date { get; set; }
    }
}
