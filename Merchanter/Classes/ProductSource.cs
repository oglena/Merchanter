using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public record class ProductSource {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; } 
        public string barcode { get; set; }
        public int qty { get; set; }
        public bool is_active { get; set; } = false;
        public DateTime update_date { get; set; } = DateTime.Now;

        [JsonConstructor]
        public ProductSource() {
            
        }

        /// <summary>
        /// ProductSource constructor
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_id">Product Source ID</param>
        /// <param name="_name">Product Name</param>
        /// <param name="_sku">Product SKU</param>
        /// <param name="_barcode">Product Barcode</param>
        /// <param name="_qty">Product Quantity</param>
        /// <param name="_is_active">Product Source Active Status</param>
        public ProductSource( int _customer_id, int _id,  string _name, string _sku, string _barcode, int _qty, bool _is_active, DateTime _update_date ) {
            customer_id = _customer_id;
            id = _id;
            name = _name;
            sku = _sku;
            barcode = _barcode;
            qty = _qty;
            is_active = _is_active;
            update_date = _update_date;
        }
    }
}
