namespace Merchanter.Classes {
    public class ProductExtension {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string sku { get; set; }
        public string barcode { get; set; }
        public int brand_id { get; set; }
        public string category_ids { get; set; }
        public bool is_xml_enabled { get; set; } = false;
        public string[]? xml_sources { get; set; } = null;
        public DateTime update_date { get; set; }


        public List<Category> categories { get; set; }
        public Brand brand { get; set; }
    }
}
