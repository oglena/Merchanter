namespace Merchanter.Classes {
    public class Invoice {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? order_id { get; set; }
        public string? order_label { get; set; }
        public string erp_customer_code { get; set; }
        public string? erp_customer_group { get; set; }
        public string erp_no { get; set; }
        public string? invoice_no { get; set; }
        public string? gib_fatura_no { get; set; }
        public bool is_belge_created { get; set; } = false;
        public string? belge_url { get; set; }
        public DateTime order_date { get; set; }
        public DateTime update_date { get; set; }
        public List<InvoiceItem> items { get; set; }
    }
}
