namespace Merchanter.Classes {
    public record class OfflineStorage {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public string sku { get; set; }
        public bool is_active { get; set; } = false;
        public DateTime update_date { get; set; }
    }
}
