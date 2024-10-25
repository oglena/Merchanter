namespace Merchanter.Classes {
    public class Brand {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string brand_name { get; set; } = string.Empty;
        public bool status { get; set; }
    }
}
