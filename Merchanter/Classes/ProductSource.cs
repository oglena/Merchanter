namespace Merchanter.Classes {
    public class ProductSource {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; } 
        public string barcode { get; set; }
        public int qty { get; set; }
        public bool is_active { get; set; } = false;

        public ProductSource( int _customer_id, int _id,  string _name, string _sku, string _barcode, int _qty, bool _is_active ) {
            customer_id = _customer_id;
            id = _id;
            name = _name;
            sku = _sku;
            barcode = _barcode;
            qty = _qty;
            is_active = _is_active;
        }
    }
}
