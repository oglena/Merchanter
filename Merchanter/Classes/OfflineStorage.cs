namespace Merchanter.Classes {
    public class OfflineStorage {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public string sku { get; set; }
        public bool is_active { get; set; }
        public DateTime? update_date { get; set; }
    }
}
