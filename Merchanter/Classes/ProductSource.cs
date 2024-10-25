namespace Merchanter.Classes {
    public class ProductSource {
        public ProductSource() {

        }

        public ProductSource( int _id, int _customer_id, string _name, string _sku, string _barcode, int _qty ) {
            id = _id;
            customer_id = _customer_id;
            name = _name;
            sku = _sku;
            barcode = _barcode;
            qty = _qty;
        }

        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string sku { get; set; } = string.Empty;
        public string barcode { get; set; } = string.Empty;
        public int qty { get; set; } = 0;
        public bool is_active { get; set; }
    }
}
