namespace Merchanter.Classes {
    public class InvoiceItem {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string invoice_no { get; set; }
        public string erp_no { get; set; }
        public string sku { get; set; }
        public int qty { get; set; }
        public string[] serials { get; set; }
        public DateTime? update_date { get; set; }
        public DateTime? create_date { get; set; }
    }
}
