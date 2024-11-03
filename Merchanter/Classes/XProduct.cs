namespace Merchanter.Classes {
    public class XProduct {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string barcode { get; set; }
        public string source_sku { get; set; }
        public string source_brand { get; set; }
        public string? source_product_group { get; set; } = null;
        public string xml_source { get; set; }
        public bool is_infosent { get; set; } = false;
        public bool is_active { get; set; } = true;
        public int qty { get; set; }
        public decimal price1 { get; set; }
        public decimal price2 { get; set; }
        public string currency { get; set; }
        public DateTime update_date { get; set; }
    }
}
