namespace Merchanter.Classes {
    public class Shipment {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int order_id { get; set; }
        public string order_label { get; set; }
        public string order_source { get; set; }
        public string shipment_method { get; set; }
        public string barcode { get; set; }
        public bool is_shipped { get; set; } = false;
        public string? tracking_number { get; set; } = null;
        public DateTime order_date { get; set; }
        public DateTime update_date { get; set; }
        public DateTime? shipment_date { get; set; } = null;
    }
}
